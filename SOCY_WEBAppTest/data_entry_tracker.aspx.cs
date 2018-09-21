using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOCY_WEBAppTest.AppCode;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace SOCY_WEBAppTest
{
    public partial class data_entry_tracker : System.Web.UI.Page
    {
        #region DBConnection
        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
        #endregion DBConnection
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session[utilSOCYWeb.cSVUserID] == null)
                {
                    Response.Redirect("LogIn.aspx");
                }
                else
                {
                    #region Set Report Type
                    SystemConstants.SelectedReport = Request.QueryString["reportid"] != null ? Request.QueryString["reportid"].ToString() : string.Empty;
                    #endregion Set Report Type

                    returnLookupsPrimary("return_regions", cboRegion, "rgn_id", "rgn_name");
                    returnLookupsPrimary("return_partner_list", cboPartner, "prt_id", "prt_name");

                    returnLookupsSeconday("return_district_list", cboRegion.SelectedValue.ToString() != string.Empty ? cboRegion.SelectedValue.ToString() : string.Empty, cboDistrict, "dst_id", "dst_name");
                    // Set_user_region_district();
                    GetDataEntryTrackerList();
                }

            }
        }

        protected void returnLookupsPrimary(string query, DropDownList cbo, string dtValue, string dtText)
        {
            DataTable dt = Lookups.ReturnLookupsPrimary(query);
            if (dt.Rows.Count > 0)
            {
                DataRow dtEmptyRow = dt.NewRow();
                dtEmptyRow[dtValue] = string.Empty;
                dtEmptyRow[dtText] = string.Empty;
                dt.Rows.InsertAt(dtEmptyRow, 0);

                cbo.DataSource = dt;
                cbo.DataValueField = dtValue;
                cbo.DataTextField = dtText;
                cbo.DataBind();
            }
        }

        protected void returnLookupsSeconday(string query, string id, DropDownList cbo, string dtValue, string dtText)
        {
            DataTable dt = Lookups.ReturnLookupsSecondary(query, id);
            if (dt.Rows.Count > 0)
            {
                DataRow dtEmptyRow = dt.NewRow();
                dtEmptyRow[dtValue] = string.Empty;
                dtEmptyRow[dtText] = string.Empty;
                dt.Rows.InsertAt(dtEmptyRow, 0);

                cbo.DataSource = dt;
                cbo.DataValueField = dtValue;
                cbo.DataTextField = dtText;
                cbo.DataBind();
            }
        }

        protected void Set_user_region_district()
        {
            string rgn_id = string.Empty;
            string dst_id = string.Empty;

            DataTable dt = LogInDB.Set_user_region_district(Session[utilSOCYWeb.cSVUserID].ToString());
            if (dt.Rows.Count > 0)
            {
                DataRow dtRow = dt.Rows[0];
                rgn_id = dtRow["rgn_id"].ToString();
                dst_id = dtRow["dst_id"].ToString();

                #region Set region
                if (rgn_id != "-1")
                {
                    cboRegion.SelectedValue = rgn_id;
                    cboRegion.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    cboRegion.SelectedValue = rgn_id;
                    cboRegion.Enabled = true;
                }
                #endregion Set region

                #region Set district
                if (dst_id != "-1")
                {
                    cboDistrict.SelectedValue = dst_id;
                    cboDistrict.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    cboDistrict.SelectedValue = rgn_id;
                    cboDistrict.Enabled = true;
                }
                #endregion Set district

            }
        }

        protected void cboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnLookupsSeconday("return_district_list", cboRegion.SelectedValue.ToString() != string.Empty ? cboRegion.SelectedValue.ToString() : string.Empty, cboDistrict, "dst_id", "dst_name");
        }

        public void StoreExcelFileToDatabase(string excelFileName)
        {
            string insertStmt = string.Empty;
            // if file doesn't exist --> terminate (you might want to show a message box or something)  
            if (!File.Exists(excelFileName))
            {
                string script = "alert(\"No excel file selected!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }

            // get all the bytes of the file into memory  
            byte[] excelContents = File.ReadAllBytes(excelFileName);

            // define SQL statement to use  
            if (DataEntryTracker.tracker_id != string.Empty)
            {
                insertStmt = @"UPDATE [dbo].[um_weekly_data_entry_tracker] SET prt_id = @prt_id,dst_id = @dst_id,file_name = @file_name,file_content = @file_content,
                                tracker_date_from = @tracker_date_from,tracker_date_to = @tracker_date_to,date_uploaded = @date_uploaded
                                WHERE tracker_id = @tracker_id";
            }
            else
            {
                insertStmt = @"INSERT INTO [dbo].[um_weekly_data_entry_tracker]
                       ([prt_id],[dst_id],[usr_id],[file_name],[file_content],[tracker_date_from],[tracker_date_to],date_uploaded) VALUES(@prt_id,@dst_id,@usr_id,@file_name,@file_content,@tracker_date_from,@tracker_date_to,@date_uploaded)";
            }


            // set up connection and command to do INSERT  
            using (conn)
            using (SqlCommand cmdInsert = new SqlCommand(insertStmt, conn))
            {
                cmdInsert.Parameters.Add("@prt_id", SqlDbType.VarChar, 50).Value = cboPartner.SelectedValue.ToString(); ;
                cmdInsert.Parameters.Add("@dst_id", SqlDbType.VarChar, 50).Value = cboDistrict.SelectedValue.ToString();
                cmdInsert.Parameters.Add("@usr_id", SqlDbType.VarChar, 50).Value = Session[utilSOCYWeb.cSVUserID].ToString();
                cmdInsert.Parameters.Add("@file_name", SqlDbType.NVarChar, 1024).Value = excelFileName.Substring(excelFileName.IndexOf('_') + 1);
                cmdInsert.Parameters.Add("@file_content", SqlDbType.VarBinary, int.MaxValue).Value = excelContents;


                #region Dates
                cmdInsert.Parameters.Add("@tracker_date_from", SqlDbType.VarChar).Value = Convert.ToDateTime(txtCreateDateFrom.Text).ToString("yyyy-MM-dd");
                cmdInsert.Parameters.Add("@tracker_date_to", SqlDbType.VarChar).Value = Convert.ToDateTime(txtCreateDateTo.Text).ToString("yyyy-MM-dd");
                cmdInsert.Parameters.Add("@date_uploaded", SqlDbType.VarChar).Value = DateTime.Today.ToString("yyyy-MM-dd");
                #endregion Dates

                cmdInsert.Parameters.Add("@tracker_id", SqlDbType.VarChar, 50).Value = DataEntryTracker.tracker_id;
                // open connection, execute SQL statement, close connection again  
                conn.Open();
                cmdInsert.ExecuteNonQuery();
                conn.Close();

                DataEntryTracker.tracker_id = string.Empty;
            }
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                if (ValidateDuplicate())
                {
                    if (ValidateWeekDay())
                    {
                        if (FileUploadControl.HasFile)
                        {
                            string path = Server.MapPath("/DataEntryTracker");
                            string file_name = FileUploadControl.PostedFile.FileName;
                            FileUploadControl.SaveAs(Server.MapPath("~/DataEntryTracker/") + (Session[utilSOCYWeb.cSVUserID].ToString() + "_" + file_name));
                            string serverPath = Server.MapPath("~/DataEntryTracker/") + ((Session[utilSOCYWeb.cSVUserID].ToString() + "_" + file_name));

                            #region Save
                            StoreExcelFileToDatabase(serverPath);
                            GetDataEntryTrackerList();
                            #endregion Save

                            #region Clear Folder
                            if (serverPath != null || serverPath != string.Empty)
                            {
                                if ((System.IO.File.Exists(serverPath)))
                                {
                                    System.IO.File.Delete(serverPath);
                                }

                            }
                            #endregion Clear Folder
                        }
                        else
                        {
                            string script = "alert(\"No excel file selected!\");";
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                                  "ServerControlScript", script, true);
                        }
                    }
                    else
                    {
                        string script = "alert(\"Start or End Date cannot fall on weekend!\");";
                        ScriptManager.RegisterStartupScript(this, GetType(),
                                              "ServerControlScript", script, true);
                    }
                  
                }
                else
                {
                    string script = "alert(\"Data Entry Tracker already uploaded for the selected district & period,consider editing!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                }

            }
            else
            {
                string script = "alert(\"All values are required!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
            }

        }

        protected void GetDataEntryTrackerList()
        {
            DataTable dt = DataEntryTracker.ReturnDataEntryTrackerList(Session[utilSOCYWeb.cSVUserID].ToString());
            if (dt.Rows.Count > 0)
            {
                gdvDataEntry.DataSource = dt;
                gdvDataEntry.DataBind();
            }
        }

        public void RetrieveExcelFileFromDatabase(string tracker_id, string excelFileName)
        {
            byte[] excelContents;
            string SQL = "SELECT file_content FROM um_weekly_data_entry_tracker WHERE tracker_id = @tracker_id";


            using (conn)
            using (SqlCommand cmdSelect = new SqlCommand(SQL, conn))
            {
                cmdSelect.Parameters.Add("@tracker_id", SqlDbType.VarChar).Value = tracker_id;

                conn.Open();
                excelContents = (byte[])cmdSelect.ExecuteScalar();
                conn.Close();
            }

            File.WriteAllBytes(excelFileName, excelContents);
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            string tracker_id = (sender as LinkButton).CommandArgument;
            byte[] bytes;
            string fileName, dateUploaded;
            using (conn)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT file_content,date_uploaded FROM um_weekly_data_entry_tracker WHERE tracker_id = @tracker_id";
                    cmd.Parameters.AddWithValue("@tracker_id", tracker_id);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["file_content"];
                        dateUploaded = sdr["date_uploaded"].ToString();
                        fileName = "DataEntryTracker_" + dateUploaded + ".xlsx";
                    }
                    conn.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        protected void GetFileDetails(object sender, EventArgs e)
        {
            string tracker_id = (sender as LinkButton).CommandArgument;
            string usr_id = string.Empty;

            DataTable dt = DataEntryTracker.ReturnFileDetails(tracker_id);
            if (dt.Rows.Count > 0)
            {
                DataRow dtrow = dt.Rows[0];

                cboPartner.SelectedValue = dtrow["prt_id"].ToString();
                cboDistrict.SelectedValue = dtrow["dst_id"].ToString();
                cboRegion.SelectedValue = dtrow["rgn_id"].ToString();
                txtCreateDateFrom.Text = dtrow["tracker_date_from"].ToString();
                txtCreateDateTo.Text = dtrow["tracker_date_to"].ToString();
                DataEntryTracker.tracker_id = dtrow["tracker_id"].ToString();
                usr_id = dtrow["usr_id"].ToString();

                if (Session[utilSOCYWeb.cSVUserID].ToString().Equals(usr_id))
                {
                    btnupload.Enabled = true;

                }
                else
                {
                    btnupload.Enabled = false;
                }
            }

        }

        protected void gdvDataEntry_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void Clear()
        {
            cboPartner.SelectedValue = string.Empty;
            cboDistrict.SelectedValue = string.Empty;
            cboRegion.SelectedValue = string.Empty;
            txtCreateDateFrom.Text = string.Empty;
            txtCreateDateTo.Text = string.Empty;
            DataEntryTracker.tracker_id = string.Empty;
            FileUploadControl.PostedFile.InputStream.Dispose();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected bool ValidateInput()
        {
            bool isValid = false;

            if (cboDistrict.SelectedValue.ToString() == string.Empty || cboPartner.SelectedValue.ToString() == string.Empty || cboRegion.SelectedValue.ToString() == string.Empty ||
                txtCreateDateFrom.Text == string.Empty || txtCreateDateTo.Text == string.Empty)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        protected bool ValidateDuplicate()
        {
            int count = 0;
            bool isValid = false;

            DataEntryTracker.tracker_date_from = Convert.ToDateTime(txtCreateDateFrom.Text).ToString("yyyy-MM-dd");
            DataEntryTracker.tracker_date_to = Convert.ToDateTime(txtCreateDateTo.Text).ToString("yyyy-MM-dd");
            DataEntryTracker.prt_id = cboPartner.SelectedValue.ToString();
            DataEntryTracker.dst_id = cboDistrict.SelectedValue.ToString();

            count = DataEntryTracker.ValidateDuplicate();
            if (count > 0)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        protected bool ValidateWeekDay()
        {
            bool isValid = false;
            if (Convert.ToDateTime(txtCreateDateFrom.Text).DayOfWeek != DayOfWeek.Saturday && Convert.ToDateTime(txtCreateDateFrom.Text).DayOfWeek != DayOfWeek.Sunday && Convert.ToDateTime(txtCreateDateTo.Text).DayOfWeek != DayOfWeek.Saturday
                && Convert.ToDateTime(txtCreateDateTo.Text).DayOfWeek != DayOfWeek.Sunday)
            {
                isValid = true;
            }
            else if(Convert.ToDateTime(txtCreateDateFrom.Text).DayOfWeek == DayOfWeek.Saturday || Convert.ToDateTime(txtCreateDateFrom.Text).DayOfWeek == DayOfWeek.Sunday || Convert.ToDateTime(txtCreateDateTo.Text).DayOfWeek == DayOfWeek.Saturday
                || Convert.ToDateTime(txtCreateDateTo.Text).DayOfWeek == DayOfWeek.Sunday || Convert.ToDateTime(txtCreateDateFrom.Text).DayOfWeek != DayOfWeek.Monday || Convert.ToDateTime(txtCreateDateTo.Text).DayOfWeek != DayOfWeek.Friday)
            {
                isValid = false;
            }

            return isValid;
        }

        protected bool ValidateTotalWeekDays()
        {
            bool isvalid = false;
            TimeSpan count = Convert.ToDateTime(txtCreateDateFrom.Text) - Convert.ToDateTime(txtCreateDateTo.Text);
            int x = (int)count.TotalDays;
            if ((x + 1) < 5 )
            {
                isvalid = false;
            }
            else
            {
                isvalid = true;
            }

            return isvalid;
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }

        protected void gdvDataEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GetDataEntryTrackerList();
            gdvDataEntry.PageIndex = e.NewPageIndex;
            gdvDataEntry.DataBind();
        }
    }
}
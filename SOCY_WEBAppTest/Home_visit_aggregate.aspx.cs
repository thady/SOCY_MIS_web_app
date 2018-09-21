using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using SOCY_WEBAppTest.AppCode;
using System.Configuration;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SOCY_WEBAppTest
{
    public partial class Home_visit_aggregate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["reportid"] == null)
                {
                    Response.Redirect("default.aspx");
                }
                else
                {
                    #region Set Report Type
                    Session["reportid"] = Request.QueryString["reportid"] != null ? Request.QueryString["reportid"].ToString() : string.Empty;
                    SystemConstants.SelectedReport = Request.QueryString["reportid"] != null ? Request.QueryString["reportid"].ToString() : string.Empty;
                    #endregion Set Report Type

                    returnLookupsPrimary("return_regions", cboRegion, "rgn_id", "rgn_name");
                    returnLookupsPrimary("return_partner_list", cboPartner, "prt_id", "prt_name");

                    returnLookupsSeconday("return_CSO_list", cboPartner.SelectedValue.ToString() != string.Empty ? cboPartner.SelectedValue.ToString() : string.Empty, cboCSO, "cso_id", "cso_name");
                    returnLookupsSeconday("return_district_list", cboRegion.SelectedValue.ToString() != string.Empty ? cboRegion.SelectedValue.ToString() : string.Empty, cboDistrict, "dst_id", "dst_name");
                    returnLookupsSeconday("return_subcounty_list", cboDistrict.SelectedValue.ToString() != string.Empty ? cboDistrict.SelectedValue.ToString() : string.Empty, cboSubCounty, "sct_id", "sct_name");
                    returnLookupsSeconday("return_ward_list", cboSubCounty.SelectedValue.ToString() != string.Empty ? cboSubCounty.SelectedValue.ToString() : string.Empty, cboParish, "wrd_id", "wrd_name");

                    Set_user_region_district();

                    GetQuarters();
                }

            }
        }

        protected void cboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnLookupsSeconday("return_district_list", cboRegion.SelectedValue.ToString() != string.Empty ? cboRegion.SelectedValue.ToString() : string.Empty, cboDistrict, "dst_id", "dst_name");
            cboDistrict_SelectedIndexChanged(cboDistrict, null);
            cboSubCounty_SelectedIndexChanged(cboSubCounty, null);
        }

        protected void cboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnLookupsSeconday("return_subcounty_list", cboDistrict.SelectedValue.ToString() != string.Empty ? cboDistrict.SelectedValue.ToString() : string.Empty, cboSubCounty, "sct_id", "sct_name");
        }

        protected void cboSubCounty_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnLookupsSeconday("return_ward_list", cboSubCounty.SelectedValue.ToString() != string.Empty ? cboSubCounty.SelectedValue.ToString() : string.Empty, cboParish, "wrd_id", "wrd_name");
        }

        protected void cboPartner_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnLookupsSeconday("return_CSO_list", cboPartner.SelectedValue.ToString() != string.Empty ? cboPartner.SelectedValue.ToString() : string.Empty, cboCSO, "cso_id", "cso_name");
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
                    cboRegion_SelectedIndexChanged(cboRegion, null);
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
                    cboDistrict_SelectedIndexChanged(cboDistrict, null);
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

        protected void GetQuarters()
        {
            DataTable dt = Lookups.ReturnQuarters();
            if (dt.Rows.Count > 0)
            {
                DataRow dtEmptyRow = dt.NewRow();
                dtEmptyRow["qm_id"] = string.Empty;
                dtEmptyRow["qm_name"] = string.Empty;
                dt.Rows.InsertAt(dtEmptyRow, 0);

                cboQuarter.DataSource = dt;
                cboQuarter.DataTextField = "qm_name";
                cboQuarter.DataValueField = "qm_id";
                cboQuarter.DataBind();
            }
        }

        protected void cboQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboQuarter.SelectedValue.ToString() != string.Empty)
            {
                DataTable dt = Lookups.ReturnQuarterDates(cboQuarter.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    DataRow dtRow = dt.Rows[0];
                    txtCreateDateFrom.Text = Convert.ToDateTime(dtRow["qm_date_begin"]).ToShortDateString();
                    txtCreateDateTo.Text = Convert.ToDateTime(dtRow["qm_date_end"]).ToShortDateString();
                }
                else
                {
                    txtCreateDateFrom.Text = string.Empty;
                    txtCreateDateTo.Text = string.Empty;
                }
            }
            else
            {
                txtCreateDateFrom.Text = string.Empty;
                txtCreateDateTo.Text = string.Empty;
            }
        }

        private void ExportToExcel()
        {
            #region Variables
            DBConnection dbCon = null;
            SLDocument slDoc = new SLDocument();
            SLStyle slStyle = null;
            DataSet ds = null;
            DataTable dt = null;
            int intCriteria = 0;
            int intLineStart = 3;
            int intLine = intLineStart;
            string[,] arrCriteria = new string[16, 2];
            string strAppPath = Server.MapPath("~");
            string strFileName = string.Empty;
            string strFolder = ConfigurationManager.AppSettings[utilSOCYWeb.cWCKReportFolder].ToString();
            #endregion Variables

            //lblError.Text = "";

            #region Export to Excel
            slDoc.SetCellValue("A1", lblReportTitle.Text);
            slStyle = slDoc.CreateStyle();
            slStyle.SetFontBold(true);
            slStyle.Font.FontSize = 14;
            slDoc.SetCellStyle("A1", slStyle);
            slDoc.SetCellValue("F1", "Report Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"));

            #region Criteria
            slDoc.SetCellValue("A2", "Report Criteria"); //set report heading
            slStyle = slDoc.CreateStyle();
            slStyle.SetFontBold(true);
            slDoc.SetCellStyle("A2", slStyle);

            arrCriteria[intCriteria, 0] = "prt_id";
            if (cboPartner.Enabled && cboPartner.SelectedIndex != 0)
            {
                slDoc.SetCellValue("A" + intLine, lblPartner.Text);
                slDoc.SetCellValue("B" + intLine, cboPartner.SelectedItem.Text);
                intLine++;

            }
            else
                arrCriteria[intCriteria, 1] = "";
            intCriteria++;

            arrCriteria[intCriteria, 0] = "cso_id";
            if (cboCSO.Enabled && cboCSO.SelectedIndex != 0)
            {
                slDoc.SetCellValue("A" + intLine, lblCSO.Text);
                slDoc.SetCellValue("B" + intLine, cboCSO.SelectedItem.Text);
                intLine++;

            }
            else
                arrCriteria[intCriteria, 1] = "";
            intCriteria++;

            arrCriteria[intCriteria, 0] = "rgn_id";
            if (cboRegion.Enabled && cboRegion.SelectedIndex != 0)
            {
                slDoc.SetCellValue("A" + intLine, lblRegion.Text);
                slDoc.SetCellValue("B" + intLine, cboRegion.SelectedItem.Text);
                intLine++;

            }
            else
                arrCriteria[intCriteria, 1] = "";
            intCriteria++;

            arrCriteria[intCriteria, 0] = "dst_id";
            if (cboDistrict.Enabled && cboDistrict.SelectedIndex != 0)
            {
                slDoc.SetCellValue("A" + intLine, lblDistrict.Text);
                slDoc.SetCellValue("B" + intLine, cboDistrict.SelectedItem.Text);
                intLine++;

            }
            else
                arrCriteria[intCriteria, 1] = "";
            intCriteria++;

            arrCriteria[intCriteria, 0] = "sct_id";
            if (cboSubCounty.Enabled && cboSubCounty.SelectedIndex != 0)
            {
                slDoc.SetCellValue("A" + intLine, lblSubcounty.Text);
                slDoc.SetCellValue("B" + intLine, cboSubCounty.SelectedItem.Text);
                intLine++;
            }
            else
                arrCriteria[intCriteria, 1] = "";
            intCriteria++;

            arrCriteria[intCriteria, 0] = "wrd_id";
            if (cboParish.Enabled && cboParish.SelectedIndex != 0)
            {
                slDoc.SetCellValue("A" + intLine, lblParish.Text);
                slDoc.SetCellValue("B" + intLine, cboParish.SelectedItem.Text);
                intLine++;
            }
            else
                arrCriteria[intCriteria, 1] = "";
            intCriteria++;

            #endregion Criteria

            #region Report
            dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);
            try
            {
                switch (Session["reportid"].ToString())
                {
                    case utilSOCYWeb.CRHomeVisitAggregate:
                        ReportsCapturedDataDB.reportType = utilSOCYWeb.CRHomeVisitAggregate;
                        ReportsCapturedDataDB.prt_id = cboPartner.SelectedValue.ToString() != string.Empty ? cboPartner.SelectedValue.ToString() : null;
                        ReportsCapturedDataDB.cso = cboCSO.SelectedValue != string.Empty ? cboCSO.SelectedValue.ToString() : null;
                        ReportsCapturedDataDB.region = cboRegion.SelectedValue != string.Empty ? cboRegion.SelectedValue.ToString() : null;
                        ReportsCapturedDataDB.district = cboDistrict.SelectedValue != string.Empty ? cboDistrict.SelectedValue.ToString() : null;
                        ReportsCapturedDataDB.subcounty = cboSubCounty.SelectedValue != string.Empty ? cboSubCounty.SelectedValue.ToString() : null;
                        ReportsCapturedDataDB.parish = cboParish.SelectedValue != string.Empty ? cboParish.SelectedValue.ToString() : null;

                        #region Dates
                        if (txtCreateDateFrom.Text != string.Empty)
                        {
                            ReportsCapturedDataDB.datecreateFrom = Convert.ToDateTime(txtCreateDateFrom.Text);
                        }
                        else
                            ReportsCapturedDataDB.datecreateFrom = null;


                        if (txtCreateDateTo.Text != string.Empty) { ReportsCapturedDataDB.datecreateTo = Convert.ToDateTime(txtCreateDateTo.Text); }
                        else
                            ReportsCapturedDataDB.datecreateTo = null;
                        #endregion Dates


                        ds = ReportsCapturedDataDB.GetReportData();
                        break;
                }

                if (ds != null)
                {
                    for (int intCount = 0; intCount < ds.Tables.Count; intCount++)
                    {
                        #region Sheet Setup
                        dt = ds.Tables[intCount];

                        if (intCount == 0)
                        {
                            slDoc.RenameWorksheet(SLDocument.DefaultFirstSheetName, dt.TableName);
                        }
                        else
                        {
                            slDoc.AddWorksheet(dt.TableName);
                            intLine = 1;
                        }

                        #endregion Sheet Setup

                        #region Headers
                        for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                        {
                            slDoc.SetCellValue(Number2ExcelColumn(intCol) + intLine, dt.Columns[intCol].ColumnName);
                            slStyle = slDoc.CreateStyle();
                            slStyle.SetFontBold(true);
                            slDoc.SetCellStyle(Number2ExcelColumn(intCol) + intLine, slStyle);
                        }
                        intLine++;
                        #endregion Headers

                        #region Data
                        for (int intRow = 0; intRow < dt.Rows.Count; intRow++)
                        {
                            for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                            {
                                slDoc.SetCellValue(Number2ExcelColumn(intCol) + (intLine + intRow), dt.Rows[intRow][intCol].ToString());
                            }
                        }
                        intLine = intLine + dt.Rows.Count;
                        #endregion Data
                    }
                }
            }
            finally
            {
                dbCon.Dispose();
            }
            #endregion Report

            #region Save File
            #region File Name
            strFileName = "Captured_Data_" + Session["reportid"].ToString() + "_" + DateTime.Now.ToString("yyyyMMMddHHmmss");
            #endregion File Name

            try
            {
                slDoc.SaveAs(strAppPath + "\\" + strFolder + "\\" + strFileName + ".xlsx");
                Response.Redirect(strFolder + "\\" + strFileName + ".xlsx");

                //reset the report type
                Session["reportid"] = null;
            }
            catch (OutOfMemoryException exc)
            {
                string script = "alert(\"Not enough Server Memory to process request. Please add additional Report Criteria to reduce the number of records returned\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
            }
            slDoc.Dispose();
            #endregion Save File
            #endregion Export to Excel
        }

        private string Number2ExcelColumn(int intNum)
        {
            #region Variables
            int int01 = 0;
            int int02 = 0;
            string strColumn = string.Empty;
            #endregion Variables

            #region Get Column
            if (intNum > 25)
            {
                int01 = (intNum) / 26;
                strColumn = Number2String(int01 - 1, true);
            }

            int02 = intNum - (int01 * 26);
            strColumn = strColumn + Number2String(int02, true);
            #endregion Get Column

            return strColumn;
        }

        private string Number2String(int intNum, bool blnCaps)
        {
            #region Variables
            Char chrAlpha;
            #endregion Variables

            #region Get Alphabet Character
            chrAlpha = (Char)((blnCaps ? 65 : 97) + intNum);
            #endregion Get Alphabet Character

            return chrAlpha.ToString();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}
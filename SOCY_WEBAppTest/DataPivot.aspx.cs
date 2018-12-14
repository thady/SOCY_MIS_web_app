using System;
using System.Data;
using NReco.PivotData;
using System.Text;

using System.Data.SqlClient;
using NReco.PivotData.Output;
using CsvHelper;

using System.IO;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using SOCY_WEBAppTest.AppCode;

namespace SOCY_WEBAppTest
{
    public partial class DataPivot : System.Web.UI.Page
    {
        #region DBConnection
        static SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
        #endregion DBConnection
        public static DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { LoadDistricts(); LoadQuarters(); }
        }

       
        public void PivotTable_OVCSERV()
        {

            var pvtData = new PivotData(new[] { "District", "SubCounty", "wrd_name", "Sex", "Age", "Quarter" }, new CountAggregatorFactory());
            pvtData.ProcessData(new DataTableReader(ReturnMembersServed()));
            var pvtTbl = new PivotTable(
                new[] { "Quarter", "District", "SubCounty"}, //rows
                new[] { "Age", "Sex"}, //columns
                pvtData);

            var outputWr = new StringWriter();
            var pvtHtmlWr = new PivotTableHtmlWriter(outputWr);
            pvtHtmlWr.SubtotalRows = true;
            pvtHtmlWr.AllowHtml = true;
            pvtHtmlWr.TotalsRowHeaderText = "Grand Total";
            pvtHtmlWr.TableClass = "table border = '1' table - bordered table-hover";

            pvtHtmlWr.Write(pvtTbl);

            var pvtTblHtml = outputWr.ToString();
            LitPivot.Text = pvtTblHtml;
        }

        public void Export_OVCSERV()
        {

            var pvtData = new PivotData(new[] { "District", "SubCounty", "wrd_name", "Sex", "Age", "Quarter" }, new CountAggregatorFactory());
            pvtData.ProcessData(new DataTableReader(ReturnMembersServed()));
            var pvtTbl = new PivotTable(
                new[] { "Quarter", "District", "SubCounty" }, //rows
                new[] { "Age", "Sex" }, //columns
                pvtData);

            var outputWr = new StringWriter();
            var pvtHtmlWr = new PivotTableHtmlWriter(outputWr);
            pvtHtmlWr.SubtotalRows = true;
            pvtHtmlWr.AllowHtml = true;
            pvtHtmlWr.TotalsRowHeaderText = "Grand Total";
            pvtHtmlWr.TableClass = "table border = '1' table - bordered table-hover";

            var pvtCsvWr = new PivotTableCsvWriter(outputWr);
            pvtCsvWr.Write(pvtTbl);
          


        }
        public void PivotTable_ES()
        {

            var pvtData = new PivotData(new[] { "District", "SubCounty", "wrd_name", "Sex","Quarter", "HES" }, new CountAggregatorFactory());
            pvtData.ProcessData(new DataTableReader(ReturnEconomic_strengthening()));
            var pvtTbl = new PivotTable(
                new[] { "Quarter", "District", "SubCounty" }, //rows
                new[] { "HES"}, //columns
                pvtData);

            var outputWr = new StringWriter();
            var pvtHtmlWr = new PivotTableHtmlWriter(outputWr);
            pvtHtmlWr.SubtotalRows = true;
            pvtHtmlWr.AllowHtml = true;
            pvtHtmlWr.TotalsRowHeaderText = "Grand Total";
            pvtHtmlWr.TotalsColumnHeaderText = "Sub County Totals";
            pvtHtmlWr.TableClass = "table border = '1' table - bordered table-hover";

            pvtHtmlWr.Write(pvtTbl);

            var pvtTblHtml = outputWr.ToString();
            LitPivot.Text = pvtTblHtml;
        }



        public  DataTable ReturnMembersServed()
        {
            dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
                RadioButton radioBtn = this.Controls.OfType<RadioButton>()
                                       .Where(x => x.Checked).FirstOrDefault();

                if (radovcActive.Checked == true)
                {
                    SQL = @"SELECT* FROM hh_household_member_ovc_pivot
                            WHERE dst_id IN('{0}') AND sct_id IN('{1}') AND qm_id IN('{2}') AND hhs_name = 'Active'";
                    SQL = string.Format(SQL, JoinDistrictSelectString(), JoinSubCountySelectString(), JoinQuarterSelectString());
                }
                else if (radovcGraduated.Checked == true)
                {
                    SQL = @"SELECT* FROM hh_household_member_ovc_pivot
                            WHERE dst_id IN('{0}') AND sct_id IN('{1}') AND qm_id IN('{2}') AND hhs_name = 'Graduated'";
                    SQL = string.Format(SQL, JoinDistrictSelectString(), JoinSubCountySelectString(), JoinQuarterSelectString());
                }
                else
                {
                    SQL = @"SELECT* FROM hh_household_member_ovc_pivot
                            WHERE dst_id IN('{0}') AND sct_id IN('{1}') AND qm_id IN('{2}')";
                    SQL = string.Format(SQL, JoinDistrictSelectString(), JoinSubCountySelectString(), JoinQuarterSelectString());
                }

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        
                    }
                    Adapt = new SqlDataAdapter(cmd);
                    Adapt.Fill(dt);

                    cmd.Parameters.Clear();

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }

            return dt;
        }

        public DataTable ReturnEconomic_strengthening()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @";With Cte_ES AS(
                        SELECT dst.dst_name AS District,sct.sct_name AS SubCounty,W.wrd_name,gnd.gnd_name AS Sex,hv.hhv_date,hvm.hhm_id,(YEAR(GETDATE()) -  YEAR(hhm.hhm_year_of_birth)) AS Age,Q.qm_name AS Quarter,CASE
                                        WHEN hvm.yn_id_es_aflateen = '1' THEN 'Aflateen'
				                        WHEN hvm.yn_id_es_agro = '1' THEN 'Agro Enterprise'
				                        WHEN hvm.yn_id_es_silc = '1' THEN 'SILC'
                                        WHEN hvm.yn_id_es_apprenticeship = '1' THEN 'Apprenticeship'
                                    END AS HES FROM hh_household_home_visit_member hvm
                        INNER JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                        INNER JOIN hh_household_home_visit hv ON hvm.hhv_id = hv.hhv_id
                        INNER JOIN hh_household hh ON hv.hh_id = hh.hh_id
                        INNER JOIN lst_ward W ON hh.wrd_id = W.wrd_id
                        INNER JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
                        INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id
                        INNER JOIN lst_gender gnd ON hhm.gnd_id = gnd.gnd_id
                        INNER JOIN lst_quarter_range Q ON (hv.hhv_date between Q.qm_date_begin AND Q.qm_date_end)
                        WHERE dst.dst_id IN('{0}') AND sct.sct_id IN('{1}') AND Q.qm_id IN('{2}')
                        ),
                        Cte_Remove_duplicates AS (
                        SELECT Cte_ES.*,ROW_NUMBER() OVER (PARTITION BY Cte_ES.Quarter,  Cte_ES.hhm_id, Cte_ES.HES ORDER BY Cte_ES.hhm_id) AS rn FROM Cte_ES WHERE Cte_ES.HES IS NOT NULL
                        )
                        SELECT* FROM Cte_Remove_duplicates WHERE Cte_Remove_duplicates.rn = 1";
                SQL = string.Format(SQL, JoinDistrictSelectString(), JoinSubCountySelectString(), JoinQuarterSelectString());

                //SQL = string.Format(SQL, tracker_id);

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Adapt = new SqlDataAdapter(cmd);
                    Adapt.Fill(dt);

                    cmd.Parameters.Clear();

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
            }

            return dt;
        }

        public string JoinDistrictSelectString()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("dst_name", typeof(string));
            string district_where_string = String.Empty;

            foreach (ListItem item in lstDistricts.Items)
            {
                if (item.Selected)
                {
                    dt.Rows.Add(item.Value);
                    
                }
            }
            List<string> districtlist = dt.AsEnumerable()
                           .Select(r => r.Field<string>("dst_name"))
                           .ToList();

            district_where_string = string.Format("{0}", string.Join("','", districtlist));
            return district_where_string;
        }

        public string JoinSubCountySelectString()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sct_name", typeof(string));
            string subcounty_where_string = String.Empty;

            foreach (ListItem item in lstSubcounty.Items)
            {
                if (item.Selected)
                {
                    dt.Rows.Add(item.Value);

                }
            }
            List<string> SubCountylist = dt.AsEnumerable()
                           .Select(r => r.Field<string>("sct_name"))
                           .ToList();

            subcounty_where_string = string.Format("{0}", string.Join("','", SubCountylist));
            return subcounty_where_string;
        }

        public string JoinQuarterSelectString()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("qm_name", typeof(string));
            string Quarter_where_string = String.Empty;

            foreach (ListItem item in lst_quarter.Items)
            {
                if (item.Selected)
                {
                    dt.Rows.Add(item.Value);

                }
            }
            List<string> SubCountylist = dt.AsEnumerable()
                           .Select(r => r.Field<string>("qm_name"))
                           .ToList();

            Quarter_where_string = string.Format("{0}", string.Join("','", SubCountylist));
            return Quarter_where_string;
        }



        public static DataTable GetData(string SQL)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            try
            {

                //SQL = string.Format(SQL, tracker_id);

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Adapt = new SqlDataAdapter(cmd);
                    Adapt.Fill(dt);

                    cmd.Parameters.Clear();

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
            }

            return dt;
        }

        private void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode)
        {
            foreach (DataRow row in dtParent.Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = row["Name"].ToString(),
                    Value = row["Id"].ToString()
                };
                if (parentId == 0)
                {
                    //TreeView1.Nodes.Add(child);
                    DataTable dtChild = GetData("SELECT sct_id AS Id,sct_name AS Name FROM lst_sub_county WHERE dst_id = " + child.Value);
                    PopulateTreeView(dtChild, int.Parse(child.Value), child);
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                }
            }
        }

        protected void LoadTreeview()
        {
            DataTable dt = GetData("SELECT dst_id,dst_name FROM lst_district ");
            this.PopulateTreeView(dt, 0, null);
            //TreeView1.CollapseAll();

        }

        protected void LoadDistricts()
        {
            DataTable dt = GetData("SELECT dst_id,dst_name FROM lst_district ");
            lstDistricts.DataSource = dt;
            lstDistricts.DataValueField = "dst_id";
            lstDistricts.DataTextField = "dst_name";
            lstDistricts.DataBind();
            
        }

        protected void LoadSubCounties()
        {
            DataTable dt = GetData("SELECT sct_id,sct_name FROM lst_sub_county WHERE dst_id IN('"+ JoinDistrictSelectString() +"')");
            lstSubcounty.DataSource = dt;
            lstSubcounty.DataValueField = "sct_id";
            lstSubcounty.DataTextField = "sct_name";
            lstSubcounty.DataBind();

        }

        protected void LoadQuarters()
        {
            DataTable dt = GetData("SELECT qm_id,qm_name FROM lst_quarter_range WHERE qm_active = 1");
            lst_quarter.DataSource = dt;
            lst_quarter.DataValueField = "qm_id";
            lst_quarter.DataTextField = "qm_name";
            lst_quarter.DataBind();

        }


        protected void lstDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubCounties();
        }

        protected void lstSubcounty_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (radOvcserv.Checked == true)
            {
                System.Threading.Thread.Sleep(5000);
                PivotTable_OVCSERV();
            }
            else if (radES.Checked == true)
            {
                System.Threading.Thread.Sleep(5000);
                PivotTable_ES();
            }
           
        }

        protected void radOvcserv_CheckedChanged(object sender, EventArgs e)
        {
            if (radOvcserv.Checked == true)
            {
                radES.Checked = false;
                radFS.Checked = false;
                radHIV.Checked = false;
                radovcGraduated.Checked = false;
                radovcActive.Checked = false;
            }
        }

        protected void radES_CheckedChanged(object sender, EventArgs e)
        {
            if (radES.Checked == true)
            {
                radOvcserv.Checked = false;
                radFS.Checked = false;
                radHIV.Checked = false;
            }
           
        }

        protected void radFS_CheckedChanged(object sender, EventArgs e)
        {
            if (radFS.Checked == true)
            {
                radOvcserv.Checked = false;
                radES.Checked = false;
                radHIV.Checked = false;
            }
           
        }

        protected void radHIV_CheckedChanged(object sender, EventArgs e)
        {
            if (radHIV.Checked == true)
            {
                radOvcserv.Checked = false;
                radES.Checked = false;
                radFS.Checked = false;
            }
           
        }

        protected void btndownload_Click(object sender, EventArgs e)
        {
            Export_OVCSERV();
        }

        protected void radovcActive_CheckedChanged(object sender, EventArgs e)
        {
            if (radovcActive.Checked == true) { radovcGraduated.Checked = false; }
        }

        protected void radovcGraduated_CheckedChanged(object sender, EventArgs e)
        {
            if (radovcGraduated.Checked == true) { radovcActive.Checked = false; }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
    }
}
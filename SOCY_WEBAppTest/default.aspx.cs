using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOCY_WEBAppTest.AppCode;
using System.Data;
//using System.Web.UI.DataVisualization.Charting;
using System.Web.Caching;
using InfoSoftGlobal;
using FusionCharts.Charts;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Configuration;

namespace SOCY_WEBAppTest
{
    public partial class _default : System.Web.UI.Page
    {
        DataTable dt = null;
        DateTime dtFrom, dtTo;
        string dst_id = string.Empty;
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
                   
                    LoadChart();
                    LoadChart_beneficiaries();
                    //LoadChart_beneficiaries_served_in_quarter();
                    LoadChart_households_served_in_quarter();

                    _LoadChart_beneficiaries_served_in_quarter();

                  //LoadDistrict();
                  //cboDistrict_SelectedIndexChanged(cboDistrict, null);
                    Lookups.ReturnCurrentQuarterDates();
                    SetQuarterName();
                    //return_Stats();
                    //returnDataEntryReports();
                    LoadOVC_stat_Data();
                    //ReturnOvcServer_Stats("Main");
                }
            }
            
        }

        #region Charts
        protected void LoadChart()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Number of active households' xAxisName='District' yAxisName='Households' showValues='0' formatNumberScale='0' showBorder='1' theme='fusion'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name ,hhs_staus AS hh_status FROM dashboard_active_households";

                SqlCommand query = new SqlCommand(strsQL, conn);

                // Begin iterating through the result set
                SqlDataReader rst = query.ExecuteReader();


                while (rst.Read())
                {
                    xmlStr.AppendFormat("<set label='{0}' value = '{1}'/>", rst["dst_name"].ToString(), rst["hh_status"].ToString());
                }

                // End the XML string
                xmlStr.Append("</chart>");


                // Close the result set Reader object and the Connection object
                rst.Close();
                conn.Close();
                Chart hh_chart = new Chart("column2d", "hh_chart", "100%", "350", "xml", xmlStr.ToString());
                //Chart MyFirstChart = new Chart("column2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                //Chart MyFirstChart3 = new Chart("pie2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                lit_active_households.Text = hh_chart.Render();
            }
        }

        protected void LoadChart_beneficiaries()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Number of active OVCs' xAxisName='District' yAxisName='Total OVC' showValues='0' formatNumberScale='0' showBorder='1' theme='zune'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name, hh_status FROM dashboard_active_household_member";

                SqlCommand query = new SqlCommand(strsQL, conn);

                // Begin iterating through the result set
                SqlDataReader rst = query.ExecuteReader();


                while (rst.Read())
                {
                    xmlStr.AppendFormat("<set label='{0}' value = '{1}'/>", rst["dst_name"].ToString(), rst["hh_status"].ToString());
                }

                // End the XML string
                xmlStr.Append("</chart>");


                // Close the result set Reader object and the Connection object
                rst.Close();
                conn.Close();
                Chart hhm_chart = new Chart("column2d", "hhm_chart", "100%", "350", "xml", xmlStr.ToString());
                //Chart MyFirstChart = new Chart("column2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                //Chart MyFirstChart3 = new Chart("pie2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                lit_active_household_members.Text = hhm_chart.Render();
            }
        }

        protected void LoadChart_beneficiaries_served_in_quarter()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='OVCs served in current quarter' xAxisName='District' yAxisName='Total OVCs served' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            string tmp = null;
            tmp = @"<categories font='Arial' fontSize='11' fontColor='000000'>";

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name,total_hhm FROM dashboard_current_quarter_hhm_served";

                SqlCommand query = new SqlCommand(strsQL, conn);

                // Begin iterating through the result set
                SqlDataReader rst = query.ExecuteReader();

                while (rst.Read())
                {
                    xmlStr.AppendFormat("<set label='{0}' value = '{1}' link='j-getNode_position-{0}'/>", rst["dst_name"].ToString(), rst["total_hhm"].ToString());
                }

                // End the XML string
                xmlStr.Append("</chart>");

                // Close the result set Reader object and the Connection object
                rst.Close();
                conn.Close();
                Chart hhm_served_chart = new Chart("line", "hhm_served_chart", "100%", "350", "xml", xmlStr.ToString());
                //Chart MyFirstChart = new Chart("column2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                //Chart MyFirstChart3 = new Chart("pie2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                lit_ben_served.Text = hhm_served_chart.Render();
            }
        }

        protected void LoadChart_households_served_in_quarter()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;
            DataRow dtRow = null;

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Households served in current quarter' xAxisName='District' yAxisName='Households served' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            xmlStr.Append("<categories font='Arial' fontSize='11' fontColor='000000'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT S.dst_name,S.total_hh,hh.hhs_staus AS total_hh_target FROM dashboard_current_quarter_hh_served S
                                  INNER JOIN dashboard_active_households hh ON S.dst_name = hh.dst_name";

                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);

                #region Chart categories
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<category label ='" + dtRow["dst_name"].ToString() + "' />");
                }
                xmlStr.Append("</categories>");

                #endregion Chart categories

                #region Chart Datasets
                xmlStr.Append("<dataset seriesName='Households Served' color='1D8BD1' anchorBorderColor='1D8BD1' anchorBgColor='1D8BD1'>");

                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_hh"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='Households Target' color='008E8E' anchorBorderColor='008E8E' anchorBgColor='008E8E'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_hh_target"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");
                #endregion Chart Datasets

                // End the XML string
                xmlStr.Append("</chart>");

                conn.Close();
                Chart hhm_served_chart = new Chart("msline", "hh_served_chart", "100%", "350", "xml", xmlStr.ToString());
              
                lit_households_served.Text = hhm_served_chart.Render();
            }
        }


        protected void _LoadChart_beneficiaries_served_in_quarter()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
         
            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;
            DataRow dtRow = null;

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='OVCs served in current quarter' xAxisName='District' yAxisName='Total OVCs served' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            xmlStr.Append("<categories font='Arial' fontSize='11' fontColor='000000'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT S.dst_name,S.total_hhm,hhm.hh_status AS total_hhm_target,T.ovc_target  FROM dashboard_current_quarter_hhm_served S
                                INNER JOIN dashboard_active_household_member hhm ON S.dst_name = hhm.dst_name
                                LEFT JOIN dashboard_ovc_target T ON S.dst_name = T.dst_name";

                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);


                #region Chart categories
                for (int x = 0;x<_dt.Rows.Count;x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<category label ='" + dtRow["dst_name"].ToString() + "' />");
                }
                xmlStr.Append("</categories>");

                #endregion Chart categories

                #region Chart Datasets
                xmlStr.Append("<dataset seriesName='OVC Served' color='0963F8' anchorBorderColor='0963F8' anchorBgColor='0963F8'>");

                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    string err = "Kyoteraa";
                    xmlStr.Append("<set value='" + dtRow["total_hhm"].ToString() + "'/>");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='Active Beneficiaries' color='09F846' anchorBorderColor='09F846' anchorBgColor='09F846'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_hhm_target"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='COP18 Target' color='F83309' anchorBorderColor='F83309' anchorBgColor='F83309' dashed = '1'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["ovc_target"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");
                #endregion Chart Datasets

                // End the XML string
                xmlStr.Append("</chart>");

                Chart hhm_served_chart = new Chart("msline", "hhm_served_chart", "100%", "350", "xml", xmlStr.ToString());
                lit_ben_served.Text = hhm_served_chart.Render();
            }
        }



        public   void CodeBehind()
        {
           // hidden.Value = "Bae";
            //Response. Redirect("http://www.google.com");
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + hidden.Value.ToString() + "');", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('"+ hidden.Value +"');", true);
            //return "this is a string from C# code behind " + DateTime.Now.ToString();

        }

        #endregion Charts

        protected void SetQuarterName()
        {
            //lblQuarter.Text = "Quarter " + Lookups.qm_date_begin.ToShortDateString() + " to " + Lookups.qm_date_end.ToShortDateString();
            //lblovchiv_stat.Text = "(" +  Lookups.qm_date_begin.ToShortDateString() + " to " + Lookups.qm_date_end.ToShortDateString() + ")";
            //lblCpaQuarter.Text = "(" + Lookups.qm_date_begin.ToShortDateString() + " to " + Lookups.qm_date_end.ToShortDateString() + ")";
        }

        protected void linkbtnOVC_Click(object sender, EventArgs e)
        {
            //Response.Redirect("CapturedDataReports.aspx");
        }

        //protected void return_Stats()
        //{
        //    lblTotalHouseholds.Text = Lookups.ReturnLookupsStats("return_total_hh").ToString();
        //    lblTotalBen.Text = Lookups.ReturnLookupsStats("return_total_ben").ToString();
        //    lblTotalSILCGroups.Text = Lookups.ReturnLookupsStats("return_total_silk_grps").ToString();
        //    lblSilcMembers.Text = Lookups.ReturnLookupsStats("return_total_silk_grps_members").ToString();
        //}

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }

        //protected void returnDataEntryReports()
        //{
        //    if (HttpContext.Current.Cache["DataEntryReports"] != null)
        //    {
        //        DataEntryChart.DataSource = HttpContext.Current.Cache["DataEntryReports"];
        //        DataEntryChart.DataBind();
        //        //DataEntryChart.Series["Series1"].SetCustomProperty("PixelPointWidth", "80");
        //        //DataEntryChart.Series["Series2"].SetCustomProperty("PixelPointWidth", "80");
        //        //DataEntryChart.Series["Series3"].SetCustomProperty("PixelPointWidth", "80");
        //        //DataEntryChart.Series["Series4"].SetCustomProperty("PixelPointWidth", "80");
        //        //DataEntryChart.Series["Series5"].SetCustomProperty("PixelPointWidth", "80");
        //        //DataEntryChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
        //        //gdvDataEntry.DataSource = HttpContext.Current.Cache["DataEntryReports"];
        //        //gdvDataEntry.DataBind();
        //    }
        //    else
        //    {
        //        CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(DataEntryReports_CacheItemRemovedCallback);
        //        SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
        //        dt = Lookups.ReturnDataEntryStats();
        //        Cache.Insert("DataEntryReports", dt, sqlDependency,DateTime.Now.AddHours(24),Cache.NoSlidingExpiration,CacheItemPriority.Default, onCacheITemRemoved);
        //        if (dt.Rows.Count > 0)
        //        {
        //            DataEntryChart.DataSource = dt;
        //            DataEntryChart.DataBind();

        //            //DataEntryChart.Series["Series1"].SetCustomProperty("PixelPointWidth", "80");
        //            //DataEntryChart.Series["Series2"].SetCustomProperty("PixelPointWidth", "80");
        //            //DataEntryChart.Series["Series3"].SetCustomProperty("PixelPointWidth", "80");
        //            //DataEntryChart.Series["Series4"].SetCustomProperty("PixelPointWidth", "80");
        //            //DataEntryChart.Series["Series5"].SetCustomProperty("PixelPointWidth", "80");
        //            //DataEntryChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
        //            //gdvDataEntry.DataSource = dt;
        //            //gdvDataEntry.DataBind();
        //        }
        //    }
        //}

        public void DataEntryReports_CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(DataEntryReports_CacheItemRemovedCallback);
            SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
            dt = Lookups.ReturnDataEntryStats();
            Cache.Insert("DataEntryReports", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        }

        protected void LoadOVC_stat_Data()
        {

            #region OVC_stat_Data
            if (HttpContext.Current.Cache["OVC_stat_Data"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Cache["OVC_stat_Data"];

                if (dt.Rows.Count > 0)
                {
                    DataRow dtRow = dt.Rows[0];

                    Lookups.total_ovc = Convert.ToInt32(dtRow["total_ovc"].ToString()).ToString();
                    Lookups.ovc_report_hiv_status = Convert.ToInt32(dtRow["ovc_report_hiv_status"].ToString()).ToString();
                    Lookups.ovc_report_hiv_status_percent = Convert.ToInt32(dtRow["ovc_report_hiv_status_percent"].ToString()).ToString();

                    Lookups.ovc_on_art = Convert.ToInt32(dtRow["ovc_on_art"].ToString()).ToString();
                    Lookups.ovc_on_art_adhering = Convert.ToInt32(dtRow["ovc_on_art_adhering"].ToString()).ToString();
                    Lookups.ovc_not_on_art = Convert.ToInt32(dtRow["ovc_not_on_art"].ToString()).ToString();
                    Lookups.ovc_reported_hiv_neg_to_partner = Convert.ToInt32(dtRow["ovc_reported_hiv_neg_to_partner"].ToString()).ToString();
                    Lookups.ovc_reported_hiv_pos_to_partner = Convert.ToInt32(dtRow["ovc_reported_hiv_pos_to_partner"].ToString()).ToString();
                    Lookups.ovc_no_reported_hiv_status_to_partner = Convert.ToInt32(dtRow["ovc_no_reported_hiv_status_to_partner"].ToString()).ToString();

                    //lblTotalovc.Text = Lookups.total_ovc;
                    //lblTotalovcReportedHIVstatus.Text = Lookups.ovc_report_hiv_status;
                    //lblTotalOVCPositive.Text = Lookups.ovc_reported_hiv_pos_to_partner;
                    //lblTotalOVCNegative.Text = Lookups.ovc_reported_hiv_neg_to_partner;
                    //lblReportedstatuspercentage.Text = Lookups.ovc_report_hiv_status_percent;
                    //lblARTYes.Text = Lookups.ovc_on_art;
                    //lblARTNo.Text = Lookups.ovc_not_on_art;
                    //lblNostatus.Text = Lookups.ovc_no_reported_hiv_status_to_partner;
                    //lblAdhering.Text = Lookups.ovc_on_art_adhering;

                    //string script = "alert(\"From Cache!\");";
                    //ScriptManager.RegisterStartupScript(this, GetType(),
                    //                      "ServerControlScript", script, true);


                }
            }
            else
            {
                CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVC_stat_Data_CacheItemRemovedCallback);
                SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
                DataTable dt = Lookups.ReturnOVCHIV_STAT();
                Cache.Insert("OVC_stat_Data", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
                if (dt.Rows.Count > 0)
                {
                    DataRow dtRow = dt.Rows[0];

                    Lookups.total_ovc = Convert.ToInt32(dtRow["total_ovc"].ToString()).ToString();
                    Lookups.ovc_report_hiv_status = Convert.ToInt32(dtRow["ovc_report_hiv_status"].ToString()).ToString();
                    Lookups.ovc_report_hiv_status_percent = Convert.ToInt32(dtRow["ovc_report_hiv_status_percent"].ToString()).ToString();

                    Lookups.ovc_on_art = Convert.ToInt32(dtRow["ovc_on_art"].ToString()).ToString();
                    Lookups.ovc_on_art_adhering = Convert.ToInt32(dtRow["ovc_on_art_adhering"].ToString()).ToString();
                    Lookups.ovc_not_on_art = Convert.ToInt32(dtRow["ovc_not_on_art"].ToString()).ToString();
                    Lookups.ovc_reported_hiv_neg_to_partner = Convert.ToInt32(dtRow["ovc_reported_hiv_neg_to_partner"].ToString()).ToString();
                    Lookups.ovc_reported_hiv_pos_to_partner = Convert.ToInt32(dtRow["ovc_reported_hiv_pos_to_partner"].ToString()).ToString();
                    Lookups.ovc_no_reported_hiv_status_to_partner = Convert.ToInt32(dtRow["ovc_no_reported_hiv_status_to_partner"].ToString()).ToString();

                    //lblTotalovc.Text = Lookups.total_ovc;
                    //lblTotalovcReportedHIVstatus.Text = Lookups.ovc_report_hiv_status;
                    //lblTotalOVCPositive.Text = Lookups.ovc_reported_hiv_pos_to_partner;
                    //lblTotalOVCNegative.Text = Lookups.ovc_reported_hiv_neg_to_partner;
                    //lblReportedstatuspercentage.Text = Lookups.ovc_report_hiv_status_percent;
                    //lblARTYes.Text = Lookups.ovc_on_art;
                    //lblARTNo.Text = Lookups.ovc_not_on_art;
                    //lblNostatus.Text = Lookups.ovc_no_reported_hiv_status_to_partner;
                    //lblAdhering.Text = Lookups.ovc_on_art_adhering;
                }

                //string script = "alert(\"From Database!\");";
                //ScriptManager.RegisterStartupScript(this, GetType(),
                //                      "ServerControlScript", script, true);
            }

            #endregion OVC_stat_Data


            #region CPA_stats
            if (HttpContext.Current.Cache["CPA_Stats"] != null)
            {
                dt = (DataTable)HttpContext.Current.Cache["CPA_Stats"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dtRow = dt.Rows[0];
                    Lookups.total_eco_strengthening = Convert.ToInt32(dtRow["total_eco_strengthening"].ToString()).ToString();
                    Lookups.total_education_support = Convert.ToInt32(dtRow["total_education_support"].ToString()).ToString();
                    Lookups.total_food_security = Convert.ToInt32(dtRow["total_food_security"].ToString()).ToString();
                    Lookups.total_health_hiv_prevention = Convert.ToInt32(dtRow["total_health_hiv_prevention"].ToString()).ToString();
                    Lookups.total_protection = Convert.ToInt32(dtRow["total_protection"].ToString()).ToString();
                    Lookups.total_pyschosocial_support = Convert.ToInt32(dtRow["total_pyschosocial_support"].ToString()).ToString();

                    //lbles.Text = Lookups.total_eco_strengthening;
                    //lblfsn.Text = Lookups.total_food_security;
                    //lblhiv.Text = Lookups.total_health_hiv_prevention;
                    //lblprotection.Text = Lookups.total_protection;
                    //lblEducation.Text = Lookups.total_education_support;
                    //lblps.Text = Lookups.total_pyschosocial_support;
                }
            }
            else
            {
                CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(CPA_stats_Data_CacheItemRemovedCallback);
                SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
                dt = Lookups.ReturnCPA_Stats();
                HttpContext.Current.Cache.Insert("CPA_Stats", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);

                if (dt.Rows.Count > 0)
                {
                    DataRow dtRow = dt.Rows[0];
                    Lookups.total_eco_strengthening = Convert.ToInt32(dtRow["total_eco_strengthening"].ToString()).ToString();
                    Lookups.total_education_support = Convert.ToInt32(dtRow["total_education_support"].ToString()).ToString();
                    Lookups.total_food_security = Convert.ToInt32(dtRow["total_food_security"].ToString()).ToString();
                    Lookups.total_health_hiv_prevention = Convert.ToInt32(dtRow["total_health_hiv_prevention"].ToString()).ToString();
                    Lookups.total_protection = Convert.ToInt32(dtRow["total_protection"].ToString()).ToString();
                    Lookups.total_pyschosocial_support = Convert.ToInt32(dtRow["total_pyschosocial_support"].ToString()).ToString();

                    //lbles.Text = Lookups.total_eco_strengthening;
                    //lblfsn.Text = Lookups.total_food_security;
                    //lblhiv.Text = Lookups.total_health_hiv_prevention;
                    //lblprotection.Text = Lookups.total_protection;
                    //lblEducation.Text = Lookups.total_education_support;
                    //lblps.Text = Lookups.total_pyschosocial_support;
                }
            }

            #endregion CPA_stats

        }

        public void OVC_stat_Data_CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVC_stat_Data_CacheItemRemovedCallback);
            SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
            DataTable dt = Lookups.ReturnOVCHIV_STAT();
            Cache.Insert("OVC_stat_Data", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        }

        public void CPA_stats_Data_CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(CPA_stats_Data_CacheItemRemovedCallback);
            SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
            dt = Lookups.ReturnCPA_Stats();
            Cache.Insert("CPA_Stats", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        }

        protected void gdvDataEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gdvDataEntry.PageIndex = e.NewPageIndex;
            //returnDataEntryReports();
        }

        protected void Load_Chart(DataTable dt)
        {
            string[] x = new string[dt.Rows.Count];
            int[] y = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][1]);
            }
            //char_Dataupload.Series[0].Points.DataBindXY(x, y);
            //char_Dataupload.Series[0].ChartType = SeriesChartType.Bar;

            //char_Dataupload.Legends[0].Enabled = true;
            //char_Dataupload.Visible = true;
        }

        //protected void ReturnOvcServer_Stats(string category)
        //{
        //    if (category == "Main")
        //    {
        //        if (HttpContext.Current.Cache["OVCServeCache"] != null)
        //        {
        //            //gdvOVCServe.DataSource = HttpContext.Current.Cache["OVCServeCache"];
        //            //gdvOVCServe.DataBind();
        //        }
        //        else
        //        {
        //            CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_stats_Data_CacheItemRemovedCallback);
        //            SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
        //            DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), Lookups.qm_date_begin, Lookups.qm_date_end);
        //            HttpContext.Current.Cache.Insert("OVCServeCache", dt, sqlDependency,DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        //            if (dt.Rows.Count > 0)
        //            {
        //                //gdvOVCServe.DataSource = dt;
        //                //gdvOVCServe.DataBind();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (HttpContext.Current.Cache["OVCServe_Search_Cache"] != null)
        //        {
        //            gdvOVCServe.DataSource = HttpContext.Current.Cache["OVCServe_Search_Cache"];
        //            gdvOVCServe.DataBind();
        //            //string script = "alert(\"From Cache!\");";
        //            //ScriptManager.RegisterStartupScript(this, GetType(),
        //            //                      "ServerControlScript", script, true);
        //        }
        //        else
        //        {
        //            CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_search_stats_Data_CacheItemRemovedCallback);
        //            SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
        //            DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), dtFrom, dtTo);
        //            HttpContext.Current.Cache.Insert("OVCServe_Search_Cache", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        //            if (dt.Rows.Count > 0)
        //            {
        //                gdvOVCServe.DataSource = dt;
        //                gdvOVCServe.DataBind();
        //            }
        //        }
        //    }
           
        //}

        //public void OVCSERVE_stats_Data_CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        //{
        //    CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_stats_Data_CacheItemRemovedCallback);
        //    SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
        //    DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), Convert.ToDateTime("2018-04-01"), Convert.ToDateTime("2018-06-30"));
        //    Cache.Insert("OVCServeCache", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        //}

        //public void OVCSERVE_search_stats_Data_CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        //{
        //    CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_search_stats_Data_CacheItemRemovedCallback);
        //    SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
        //    DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), dtFrom, dtTo);
        //    Cache.Insert("OVCServe_Search_Cache", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        //}


        protected void returnLookupsSeconday(string query, string id, DropDownList cbo, string dtValue, string dtText)
        {
           
        }
       
    }
}
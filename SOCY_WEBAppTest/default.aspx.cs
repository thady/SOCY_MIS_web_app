using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOCY_WEBAppTest.AppCode;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Web.Caching;

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
                    LoadDistrict();
                    cboDistrict_SelectedIndexChanged(cboDistrict, null);
                    Lookups.ReturnCurrentQuarterDates();
                    SetQuarterName();
                    return_Stats();
                    returnDataEntryReports();
                    LoadOVC_stat_Data();
                    ReturnOvcServer_Stats("Main");
                }
            }
            
        }

        protected void SetQuarterName()
        {
            //lblQuarter.Text = "Quarter " + Lookups.qm_date_begin.ToShortDateString() + " to " + Lookups.qm_date_end.ToShortDateString();
            lblovchiv_stat.Text = "(" +  Lookups.qm_date_begin.ToShortDateString() + " to " + Lookups.qm_date_end.ToShortDateString() + ")";
            lblCpaQuarter.Text = "(" + Lookups.qm_date_begin.ToShortDateString() + " to " + Lookups.qm_date_end.ToShortDateString() + ")";
        }

        protected void linkbtnOVC_Click(object sender, EventArgs e)
        {
            //Response.Redirect("CapturedDataReports.aspx");
        }

        protected void return_Stats()
        {
            lblTotalHouseholds.Text = Lookups.ReturnLookupsStats("return_total_hh").ToString();
            lblTotalBen.Text = Lookups.ReturnLookupsStats("return_total_ben").ToString();
            lblTotalSILCGroups.Text = Lookups.ReturnLookupsStats("return_total_silk_grps").ToString();
            lblSilcMembers.Text = Lookups.ReturnLookupsStats("return_total_silk_grps_members").ToString();
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }

        protected void returnDataEntryReports()
        {
            if (HttpContext.Current.Cache["DataEntryReports"] != null)
            {
                DataEntryChart.DataSource = HttpContext.Current.Cache["DataEntryReports"];
                DataEntryChart.DataBind();
                //DataEntryChart.Series["Series1"].SetCustomProperty("PixelPointWidth", "80");
                //DataEntryChart.Series["Series2"].SetCustomProperty("PixelPointWidth", "80");
                //DataEntryChart.Series["Series3"].SetCustomProperty("PixelPointWidth", "80");
                //DataEntryChart.Series["Series4"].SetCustomProperty("PixelPointWidth", "80");
                //DataEntryChart.Series["Series5"].SetCustomProperty("PixelPointWidth", "80");
                //DataEntryChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                //gdvDataEntry.DataSource = HttpContext.Current.Cache["DataEntryReports"];
                //gdvDataEntry.DataBind();
            }
            else
            {
                CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(DataEntryReports_CacheItemRemovedCallback);
                SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
                dt = Lookups.ReturnDataEntryStats();
                Cache.Insert("DataEntryReports", dt, sqlDependency,DateTime.Now.AddHours(24),Cache.NoSlidingExpiration,CacheItemPriority.Default, onCacheITemRemoved);
                if (dt.Rows.Count > 0)
                {
                    DataEntryChart.DataSource = dt;
                    DataEntryChart.DataBind();

                    //DataEntryChart.Series["Series1"].SetCustomProperty("PixelPointWidth", "80");
                    //DataEntryChart.Series["Series2"].SetCustomProperty("PixelPointWidth", "80");
                    //DataEntryChart.Series["Series3"].SetCustomProperty("PixelPointWidth", "80");
                    //DataEntryChart.Series["Series4"].SetCustomProperty("PixelPointWidth", "80");
                    //DataEntryChart.Series["Series5"].SetCustomProperty("PixelPointWidth", "80");
                    //DataEntryChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                    //gdvDataEntry.DataSource = dt;
                    //gdvDataEntry.DataBind();
                }
            }
        }

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

                    lblTotalovc.Text = Lookups.total_ovc;
                    lblTotalovcReportedHIVstatus.Text = Lookups.ovc_report_hiv_status;
                    lblTotalOVCPositive.Text = Lookups.ovc_reported_hiv_pos_to_partner;
                    lblTotalOVCNegative.Text = Lookups.ovc_reported_hiv_neg_to_partner;
                    lblReportedstatuspercentage.Text = Lookups.ovc_report_hiv_status_percent;
                    lblARTYes.Text = Lookups.ovc_on_art;
                    lblARTNo.Text = Lookups.ovc_not_on_art;
                    lblNostatus.Text = Lookups.ovc_no_reported_hiv_status_to_partner;
                    lblAdhering.Text = Lookups.ovc_on_art_adhering;

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

                    lblTotalovc.Text = Lookups.total_ovc;
                    lblTotalovcReportedHIVstatus.Text = Lookups.ovc_report_hiv_status;
                    lblTotalOVCPositive.Text = Lookups.ovc_reported_hiv_pos_to_partner;
                    lblTotalOVCNegative.Text = Lookups.ovc_reported_hiv_neg_to_partner;
                    lblReportedstatuspercentage.Text = Lookups.ovc_report_hiv_status_percent;
                    lblARTYes.Text = Lookups.ovc_on_art;
                    lblARTNo.Text = Lookups.ovc_not_on_art;
                    lblNostatus.Text = Lookups.ovc_no_reported_hiv_status_to_partner;
                    lblAdhering.Text = Lookups.ovc_on_art_adhering;
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

                    lbles.Text = Lookups.total_eco_strengthening;
                    lblfsn.Text = Lookups.total_food_security;
                    lblhiv.Text = Lookups.total_health_hiv_prevention;
                    lblprotection.Text = Lookups.total_protection;
                    lblEducation.Text = Lookups.total_education_support;
                    lblps.Text = Lookups.total_pyschosocial_support;
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

                    lbles.Text = Lookups.total_eco_strengthening;
                    lblfsn.Text = Lookups.total_food_security;
                    lblhiv.Text = Lookups.total_health_hiv_prevention;
                    lblprotection.Text = Lookups.total_protection;
                    lblEducation.Text = Lookups.total_education_support;
                    lblps.Text = Lookups.total_pyschosocial_support;
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
            gdvDataEntry.PageIndex = e.NewPageIndex;
            returnDataEntryReports();
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

        protected void ReturnOvcServer_Stats(string category)
        {
            if (category == "Main")
            {
                if (HttpContext.Current.Cache["OVCServeCache"] != null)
                {
                    gdvOVCServe.DataSource = HttpContext.Current.Cache["OVCServeCache"];
                    gdvOVCServe.DataBind();
                }
                else
                {
                    CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_stats_Data_CacheItemRemovedCallback);
                    SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
                    DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), Lookups.qm_date_begin, Lookups.qm_date_end);
                    HttpContext.Current.Cache.Insert("OVCServeCache", dt, sqlDependency,DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
                    if (dt.Rows.Count > 0)
                    {
                        gdvOVCServe.DataSource = dt;
                        gdvOVCServe.DataBind();
                    }
                }
            }
            else
            {
                if (HttpContext.Current.Cache["OVCServe_Search_Cache"] != null)
                {
                    gdvOVCServe.DataSource = HttpContext.Current.Cache["OVCServe_Search_Cache"];
                    gdvOVCServe.DataBind();
                    //string script = "alert(\"From Cache!\");";
                    //ScriptManager.RegisterStartupScript(this, GetType(),
                    //                      "ServerControlScript", script, true);
                }
                else
                {
                    CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_search_stats_Data_CacheItemRemovedCallback);
                    SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
                    DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), dtFrom, dtTo);
                    HttpContext.Current.Cache.Insert("OVCServe_Search_Cache", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
                    if (dt.Rows.Count > 0)
                    {
                        gdvOVCServe.DataSource = dt;
                        gdvOVCServe.DataBind();
                    }
                }
            }
           
        }

        public void OVCSERVE_stats_Data_CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_stats_Data_CacheItemRemovedCallback);
            SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
            DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), Convert.ToDateTime("2018-04-01"), Convert.ToDateTime("2018-06-30"));
            Cache.Insert("OVCServeCache", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        }

        public void OVCSERVE_search_stats_Data_CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            CacheItemRemovedCallback onCacheITemRemoved = new CacheItemRemovedCallback(OVCSERVE_search_stats_Data_CacheItemRemovedCallback);
            SqlCacheDependency sqlDependency = new SqlCacheDependency("SOCY_LIVE", "hh_household_home_visit_member");
            DataTable dt = Indicators.GetOvcSERVEData(cboDistrict.SelectedValue.ToString(), dtFrom, dtTo);
            Cache.Insert("OVCServe_Search_Cache", dt, sqlDependency, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheITemRemoved);
        }


        protected void returnLookupsSeconday(string query, string id, DropDownList cbo, string dtValue, string dtText)
        {
           
        }

        protected void LoadDistrict()
        {
            if (HttpContext.Current.Cache["lstdistrict"] != null)
            {
                cboDistrict.DataSource = HttpContext.Current.Cache["lstdistrict"];
                cboDistrict.DataValueField = "dst_id";
                cboDistrict.DataTextField = "dst_name";
                cboDistrict.DataBind();
            }
            else
            {
                System.Web.Caching.SqlCacheDependency sqlDependency = new System.Web.Caching.SqlCacheDependency("SOCY_LIVE", "lst_district");
                dt = Indicators.LoadDistricts();
                HttpContext.Current.Cache.Insert("lstdistrict", dt, sqlDependency);
                if (dt.Rows.Count > 0)
                {
                    cboDistrict.DataSource = dt;
                    cboDistrict.DataValueField = "dst_id";
                    cboDistrict.DataTextField = "dst_name";
                    cboDistrict.DataBind();
                }
            }
        }

        protected void cboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblheaderOVCServe.Text = "(" + cboDistrict.SelectedItem.ToString() + " District" + ")";
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
           
            if (cboDistrict.SelectedItem.ToString() == string.Empty || txtCreateDateFrom.Text == string.Empty || txtCreateDateTo.Text == string.Empty)
            {
                Response.Write("<script>alert('All filter values are required');</script>");
            }
            else
            {
                #region Dates
                if (txtCreateDateFrom.Text != string.Empty)
                {
                    dtFrom = Convert.ToDateTime(txtCreateDateFrom.Text);
                }
                else
                    dtFrom = Convert.ToDateTime("2018-04-01");


                if (txtCreateDateTo.Text != string.Empty) { dtTo = Convert.ToDateTime(txtCreateDateTo.Text); }
                else
                    dtTo = Convert.ToDateTime("2018-06-30");

                #endregion Dates
            }

            ReturnOvcServer_Stats("Filter");
        }
    }
}
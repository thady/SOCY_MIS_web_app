using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using SOCY_WEBAppTest.AppCode;

namespace SOCY_WEBAppTest
{
    public partial class Result_area_three_dashboard_reports : System.Web.UI.Page
    {
        #region Variables
        DataTable dt = null;
        DateTime? datefrom = null;
        DateTime? dateTo = null;
        #endregion Variables
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
                    #region Lookups
                    returnLookupsSeconday("return_district_list", string.Empty, cboDistrict, "dst_id", "dst_name");

                    returnLookupsSeconday("return_CSO_list", string.Empty, cboCSO, "cso_id", "cso_name");
                    Set_cso();
                    #endregion Lookups
                }
                
            }
            Set_cso();
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

        protected void Set_cso()
        {

            cboCSO.SelectedValue = "CSO001";
            cboCSO.Attributes.Add("disabled", "disabled");
        }

        protected void LoadResultArea01Reports(string dst_id, string cso_id, string report_type)
        {
            #region Dates
            if (txtCreateDateFrom.Text != string.Empty)
            {
                string short_date = Convert.ToDateTime(txtCreateDateFrom.Text).ToShortDateString();
                datefrom = Convert.ToDateTime(short_date);
            }
            else
                datefrom = null;


            if (txtCreateDateTo.Text != string.Empty) { dateTo = Convert.ToDateTime(txtCreateDateTo.Text); }
            else
                dateTo = null;
            #endregion Dates

            switch(report_type) {
                case "ovc_served":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_ovc_served.DataSource = dt;
                        gdv_ovc_served.DataBind();
                       
                    }
                    else
                    {
                        dt = null;
                        gdv_ovc_served.DataSource = dt;
                        gdv_ovc_served.DataBind();
                    }
                    break;
                case "ovc_more_than_one_service_received":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_more_than_one_service.DataSource = dt;
                        gdv_more_than_one_service.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_more_than_one_service.DataSource = dt;
                        gdv_more_than_one_service.DataBind();
                    }
                    break;
                case "number_of_home_visits_conducted":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_total_home_visits.DataSource = dt;
                        gdv_total_home_visits.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_total_home_visits.DataSource = dt;
                        gdv_total_home_visits.DataBind();
                    }
                    break;
                case "ovc_known_hiv_status":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_ovc_known_hiv_status.DataSource = dt;
                        gdv_ovc_known_hiv_status.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_ovc_known_hiv_status.DataSource = dt;
                        gdv_ovc_known_hiv_status.DataBind();
                    }
                    break;
                case "caregiver_known_hiv_status":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_caregiver_known_hiv_status.DataSource = dt;
                        gdv_caregiver_known_hiv_status.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_caregiver_known_hiv_status.DataSource = dt;
                        gdv_caregiver_known_hiv_status.DataBind();
                    }
                    break;
                case "ovc_hiv_positive":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_ovc_hiv_positive.DataSource = dt;
                        gdv_ovc_hiv_positive.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_ovc_hiv_positive.DataSource = dt;
                        gdv_ovc_hiv_positive.DataBind();
                    }
                    break;
                case "caregiver_hiv_positive":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_caregiver_hiv_positive.DataSource = dt;
                        gdv_caregiver_hiv_positive.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_caregiver_hiv_positive.DataSource = dt;
                        gdv_caregiver_hiv_positive.DataBind();
                    }
                    break;
                case "ovc_linked_to_care":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_ovc_linked_to_care.DataSource = dt;
                        gdv_ovc_linked_to_care.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_ovc_linked_to_care.DataSource = dt;
                        gdv_ovc_linked_to_care.DataBind();
                    }
                    break;
                case "caregiver_linked_to_care":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_caregiver_linked_to_care.DataSource = dt;
                        gdv_caregiver_linked_to_care.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_caregiver_linked_to_care.DataSource = dt;
                        gdv_caregiver_linked_to_care.DataBind();
                    }
                    break;
            }

            
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_served");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "number_of_home_visits_conducted");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_more_than_one_service_received");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_known_hiv_status");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "caregiver_known_hiv_status");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_hiv_positive");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "caregiver_hiv_positive");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_linked_to_care");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "caregiver_linked_to_care");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
    }
}
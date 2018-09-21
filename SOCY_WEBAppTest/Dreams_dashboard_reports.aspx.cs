using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOCY_WEBAppTest.AppCode;
using System.Data;

namespace SOCY_WEBAppTest
{
    public partial class Dreams_dashboard_reports : System.Web.UI.Page
    {
        #region Variables
        DataTable dt = null;
        DateTime? datefrom = null;
        DateTime? dateTo = null;
        DataSet ds = null;
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
                    returnLookupsSeconday("return_dreams_districts", string.Empty, cboDistrict, "dst_id", "dst_name");

                    returnLookupsSeconday("return_CSO_list", string.Empty, cboCSO, "cso_id", "cso_name");
                    Set_cso();
                    #endregion Lookups
                }

            }
            Set_cso();
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
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

            switch (report_type)
            {
                case "total_girls_enrolled_in_dreams":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_drm_girls.DataSource = dt;
                        gdv_drm_girls.DataBind();

                    }
                    else
                    {
                        dt = null;
                        gdv_drm_girls.DataSource = dt;
                        gdv_drm_girls.DataBind();
                    }
                    break;
                case "girls_completing_14_sessions":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_drm_all_sessions.DataSource = dt;
                        gdv_drm_all_sessions.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_drm_all_sessions.DataSource = dt;
                        gdv_drm_all_sessions.DataBind();
                    }
                    break;
                case "total_groups_receiving_sinovuyo":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_drm_groups.DataSource = dt;
                        gdv_drm_groups.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_drm_groups.DataSource = dt;
                        gdv_drm_groups.DataBind();
                    }
                    break;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "total_girls_enrolled_in_dreams");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "girls_completing_14_sessions");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "total_groups_receiving_sinovuyo");
        }
    }
}
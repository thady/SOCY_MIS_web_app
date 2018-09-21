using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

using System.Data;
using SOCY_WEBAppTest.AppCode;

namespace SOCY_WEBAppTest
{
    public partial class Result_area_three_reports_refferals : System.Web.UI.Page
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

            switch (report_type)
            {
                case "ovc_refferals_made":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_ovc_refferals_made.DataSource = dt;
                        gdv_ovc_refferals_made.DataBind();

                    }
                    else
                    {
                        dt = null;
                        gdv_ovc_refferals_made.DataSource = dt;
                        gdv_ovc_refferals_made.DataBind();
                    }
                    break;
                case "ov_refferals_completed":
                    dt = Result_Area_three_reports.GetRsesultArea01Report(dst_id, cso_id, report_type, datefrom, dateTo);
                    if (dt.Rows.Count > 0)
                    {
                        gdv_ovc_refferals_completed.DataSource = dt;
                        gdv_ovc_refferals_completed.DataBind();
                    }
                    else
                    {
                        dt = null;
                        gdv_ovc_refferals_completed.DataSource = dt;
                        gdv_ovc_refferals_completed.DataBind();
                    }
                    break;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_refferals_made");
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ov_refferals_completed");
            LoadChartData(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_referral_made_vs_completed");
        }

        protected void gdv_ovc_refferals_made_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdv_ovc_refferals_made.PageIndex = e.NewPageIndex;
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ovc_refferals_made");
        }

        protected void gdv_ovc_refferals_completed_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdv_ovc_refferals_completed.PageIndex = e.NewPageIndex;
            LoadResultArea01Reports(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), "ov_refferals_completed");
        }

        protected void LoadChartData(string dst_id, string cso_id, string report_type)
        {
            #region Clear Chart
            foreach (var series in OVCReferralChart.Series)
            {
                series.Points.Clear();
            }
            #endregion 

            dt = Result_Area_three_reports.GetOVCReferral_graph_data(dst_id, cso_id, report_type, datefrom, dateTo);
            if (dt.Rows.Count > 0)
            {
                string[] x = new string[dt.Rows.Count];
                int[] y = new int[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x[i] = dt.Rows[i][0].ToString();
                    y[i] = Convert.ToInt32(dt.Rows[i][1]);
                }
                OVCReferralChart.Series[0].Points.DataBindXY(x, y);

                OVCReferralChart.Series[0].ChartType = SeriesChartType.StackedColumn;

                OVCReferralChart.Series[0].LegendText = "Referral Completion Rate:";

                OVCReferralChart.Legends[0].Enabled = true;
                OVCReferralChart.Visible = true;
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
    }
}
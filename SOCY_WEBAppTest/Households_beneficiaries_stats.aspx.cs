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
    public partial class Households_beneficiaries_stats : System.Web.UI.Page
    {
        DataTable dt = null;
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
                    return_Stats();
                    returnLookupsSeconday("return_district_list", string.Empty, cboDistrict, "dst_id", "dst_name");
                    returnLookupsSeconday("return_subcounty_list", string.Empty, cboSubcounty, "sct_id", "sct_name");

                    cboDistrict_SelectedIndexChanged(cboDistrict, null);
                    GetHHStats();
                }
            }
        }

        protected void return_Stats()
        {
            lblTotalHouseholds.Text = Lookups.ReturnLookupsStats("return_total_hh").ToString();
            lblTotalBen.Text = Lookups.ReturnLookupsStats("return_total_ben").ToString();
            lblTotalSILCGroups.Text = Lookups.ReturnLookupsStats("return_total_silk_grps").ToString();
            lblSilcMembers.Text = Lookups.ReturnLookupsStats("return_total_silk_grps_members").ToString();
        }

        protected void GetHHStats()
        {
            dt = Indicators.ReturnHHBenStats("TotalHH_Ben_combined", cboDistrict.SelectedValue.ToString(),cboSubcounty.SelectedValue.ToString());
            if (dt.Rows.Count > 0)
            {
                gdvHH.DataSource = dt;
                gdvHH.DataBind();
            }
            else
            {
                dt = null;
                gdvHH.DataSource = dt;
                gdvHH.DataBind();
            }

          
        }


        protected void returnLookupsSeconday(string query, string id, DropDownList cbo, string dtValue, string dtText)
        {
            DataTable dt = Lookups.ReturnLookupsSecondary(query, id);
            if (dt.Rows.Count > 0)
            {
                cbo.DataSource = dt;
                cbo.DataValueField = dtValue;
                cbo.DataTextField = dtText;
                cbo.DataBind();
            }
        }

        protected void gdvHH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvHH.PageIndex = e.NewPageIndex;
            GetHHStats();
        }

        protected void cboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnLookupsSeconday("return_subcounty_list", cboDistrict.SelectedValue.ToString(), cboSubcounty, "sct_id", "sct_name");

            GetHHStats();
        }


        protected void cboSubcounty_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetHHStats();
        }

        protected void gdvHH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // grab the Label Control.
                Label lblTotalHH = e.Row.FindControl("lblTotalHH") as Label;
                Label lblTotalBen = e.Row.FindControl("lblTotalBen") as Label;
                lblTotalHH.ForeColor = System.Drawing.Color.Red;
                lblTotalBen.ForeColor = System.Drawing.Color.Blue;
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
    }
}
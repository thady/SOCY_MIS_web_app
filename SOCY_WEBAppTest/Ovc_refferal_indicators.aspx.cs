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
    public partial class Ovc_refferal_indicators : System.Web.UI.Page
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
                    ReturnOvcServer_Stats();
                }
            }
        }

        protected void ReturnOvcServer_Stats()
        {
            dt = Indicators.GetOvcServerData("Refferal_indicators");
            if (dt.Rows.Count > 0)
            {
                gdv_refferal.DataSource = dt;
                gdv_refferal.DataBind();
            }  
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
    }
}
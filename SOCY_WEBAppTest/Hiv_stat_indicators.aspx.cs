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
    public partial class Hiv_stat_indicators : System.Web.UI.Page
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
            #region Below 18 Years
             dt = Indicators.GetOvcServerData("HIV_Stat");
            if (dt.Rows.Count > 0)
            {
                gdvHivStat.DataSource = dt;
                gdvHivStat.DataBind();
            }
            #endregion Below 18 Years

            #region 18 and above
            dt = Indicators.GetOvcServerData("HIV_StatAdult");
            if (dt.Rows.Count > 0)
            {
                gdvHivStat_adult.DataSource = dt;
                gdvHivStat_adult.DataBind();
            }
            #endregion 18 and above

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
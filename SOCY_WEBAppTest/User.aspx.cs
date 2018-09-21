using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using SOCY_WEBAppTest.AppCode;

namespace SOCY_WEBAppTest
{
    public partial class User : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                returnLookupsPrimary("return_regions", cboRegion, "rgn_id", "rgn_name");
                returnLookupsPrimary("honorofic", cboTitle, "hnr_id", "hnr_name");
                returnLookupsSeconday("return_district_list", cboRegion.SelectedValue.ToString() != string.Empty ? cboRegion.SelectedValue.ToString() : string.Empty, cboDistrict, "dst_id", "dst_name");
            }
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

        protected void cboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnLookupsSeconday("return_district_list", cboRegion.SelectedValue.ToString() != string.Empty ? cboRegion.SelectedValue.ToString() : string.Empty, cboDistrict, "dst_id", "dst_name");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
    }
}
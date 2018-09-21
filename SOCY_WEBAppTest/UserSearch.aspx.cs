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
    public partial class UserSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                returnLookupsPrimary("um_role", cbofilter, "rl_id", "rl_name");
                SearchUsers("return_user_list");
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

        protected void SearchUsers(string query)
        {
            DataTable dtUsers = Lookups.ReturnLookupsPrimary(query);
            if (dtUsers.Rows.Count > 0)
            {
                gdv_users.DataSource = dtUsers;
                gdv_users.DataBind();
            }
        }

        protected void gdv_users_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                //Determine the RowIndex of the Row whose LinkButton was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = gdv_users.Rows[rowIndex];

                //Fetch value of Name.
                string usr_id = (row.FindControl("txtusr_id") as TextBox).Text;

                if (usr_id != string.Empty)
                {
                    Response.Redirect("User.aspx?usr_id=" + usr_id);
                }

                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name: " + name + "\\nCountry: " + country + "');", true);
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
    }
}
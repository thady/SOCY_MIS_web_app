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
    public partial class Cluster_data_capture_offices : System.Web.UI.Page
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
                    get_district_list();
                    return_office_groups();
                }    
            }
        }

        protected void CboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CboDistrict.SelectedValue.ToString() != "-1")
            {
                get_office_list(CboDistrict.SelectedValue.ToString());
            }
        }

        protected void get_district_list()
        {
            dt = Cluster_data_capture_officesDB.Return_list_of_districts();
            if (dt.Rows.Count != 0)
            {
                DataRow dtRow = dt.NewRow();
                dtRow["dst_id"] = -1;
                dtRow["dst_name"] = "select district";


                dt.Rows.InsertAt(dtRow, 0);

                CboDistrict.DataValueField = "dst_id";
                CboDistrict.DataTextField = "dst_name";

                CboDistrict.DataSource = dt;
                CboDistrict.DataBind();
            }
        }

        //get office list
        protected void get_office_list(string district_id)
        {
            dt = Cluster_data_capture_officesDB.Return_list_of_district_offices(district_id);
            if (dt.Rows.Count > 0)
            {
                chkListOffices.DataTextField = "ofc_name";
                chkListOffices.DataValueField = "ofc_id";
                chkListOffices.DataSource = dt;
                chkListOffices.DataBind();
            }
            else
            {
                chkListOffices.DataTextField = "ofc_name";
                chkListOffices.DataValueField = "ofc_id";
                chkListOffices.DataSource = dt;
                chkListOffices.DataBind();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

            if (CboDistrict.SelectedValue.ToString() != "-1" && txt_groupname.Text != String.Empty)
            {
                Cluster_data_capture_officesDB.Create_Office_group(chkListOffices, CboDistrict.SelectedValue.ToString(), txt_groupname.Text);
            }
        }

        protected void return_office_groups()
        {
            DataTable dt = Cluster_data_capture_officesDB.Return_list_of_office_groups();
            if (dt != null)
            {
                gdv_office_group.DataSource = dt;
                gdv_office_group.DataBind();
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
    }
}
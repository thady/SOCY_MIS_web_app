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
    public partial class _data_entry_tracker_v2_view : System.Web.UI.Page
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
                    LoadDistricts();
                    LoadTrackerList();
                }
                
            }
        }

        protected void LoadTrackerList()
        {
            dt = DataEntryTracker.LoadTrackerLists();
            if (dt.Rows.Count != 0)
            {
                gdvDataEntry.DataSource = dt;
                gdvDataEntry.DataBind();
            }

        }

        protected void LoadDistricts()
        {
            DataTable dt = DataEntryTracker.ReturnDistrictList();
            if (dt.Rows.Count > 0)
            {
                DataRow dtEmptyRow = dt.NewRow();
                dtEmptyRow["dst_id"] = "-1";
                dtEmptyRow["dst_name"] = "Select District";
                dt.Rows.InsertAt(dtEmptyRow, 0);

                cbo_district.DataSource = dt;
                cbo_district.DataValueField = "dst_id";
                cbo_district.DataTextField = "dst_name";
                cbo_district.DataBind();
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            string tracker_id = (sender as LinkButton).CommandArgument;
            Response.Redirect("cr_DataEntryTrackerReport.aspx?Token_id=" + tracker_id);
            
        }

        protected void GetTrackerDetails(object sender, EventArgs e)
        {
            string tracker_id = (sender as LinkButton).CommandArgument;
            Response.Redirect("_data_entry_tracker_v2.aspx?Token_id=" + tracker_id);

        }

        protected void gdvDataEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadTrackerList();
            gdvDataEntry.PageIndex = e.NewPageIndex;
            gdvDataEntry.DataBind();
        }
    }
}
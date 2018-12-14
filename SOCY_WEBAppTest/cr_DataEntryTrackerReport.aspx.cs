using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;


using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SOCY_WEBAppTest.AppCode;
using CrystalDecisions.Web;

namespace SOCY_WEBAppTest
{
    public partial class cr_DataEntryTrackerReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Token_id"].ToString() != string.Empty)
                {
                    LoadBushenyiTrackerReport(Request.QueryString["Token_id"].ToString());
                }
                else
                {
                    Response.Redirect("_data_entry_tracker_v2_view.aspx");
                }
                
            }
        }

        protected void LoadReport(string tracker_id)
        {
            ReportDocument crystalReport = new ReportDocument();
            crystalReport.Load(Server.MapPath("~/cr_DataEntryTracker.rpt"));

            foreach (CrystalDecisions.CrystalReports.Engine.Table tbCurrent in crystalReport.Database.Tables)
            {
                Set_Report_logons.SetTableLogin(tbCurrent);
            }

            DataSet ds = DataEntryTracker._ReturnDataEntryReport("weekly_tracker_report",tracker_id);
           
            crystalReport.SetDataSource(ds);
            crystalReport.SetParameterValue("@QueryType", "weekly_tracker_report");
            CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            CrystalReportViewer1.ReportSource = crystalReport;
           
        }

        protected void LoadBushenyiTrackerReport(string tracker_id)
        {
            
            cr_DataEntryTracker objRptMain = new cr_DataEntryTracker();
            ReportDocument objSubRpt = new ReportDocument();

            objSubRpt.Load(Server.MapPath("~/Bushenyi_sub_report.rpt"));

            //foreach (CrystalDecisions.CrystalReports.Engine.Table tbCurrent in objSubRpt.Database.Tables)
            //{
            //    Set_Report_logons.SetTableLogin(tbCurrent);
            //}

            foreach (CrystalDecisions.CrystalReports.Engine.Table tbCurrent in objRptMain.Database.Tables)
            {
                Set_Report_logons.SetTableLogin(tbCurrent);
            }

            foreach (ReportDocument subreport in objRptMain.Subreports)
            {
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in subreport.Database.Tables)
                {
                    Set_Report_logons.SetTableLogin(CrTable);
                }
            }


            DataSet ds = DataEntryTracker._ReturnDataEntryReport("Data_entry_weekly_tracker_report",tracker_id);

            objSubRpt = objRptMain.OpenSubreport("Bushenyi_sub_report.rpt");
            objSubRpt.SetDataSource(ds);
            //objRptMain.SetParameterValue("@QueryType", "weekly_tracker_report");
            objRptMain.SetParameterValue("@QueryType", "Data_entry_weekly_tracker_report", objRptMain.Subreports[0].Name.ToString());
            objRptMain.SetParameterValue("@Tracker_id", tracker_id, objRptMain.Subreports[0].Name.ToString());
            CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            objRptMain.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Tracker");
            CrystalReportViewer1.ReportSource = objRptMain;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using SOCY_WEBAppTest.AppCode;
using System.Web.UI.DataVisualization.Charting;

namespace SOCY_WEBAppTest
{
    public partial class Household_economic_strengthening_reports : System.Web.UI.Page
    {
        DataTable dt = null;
        DateTime? datefrom = null;
        DateTime? dateTo = null;
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
                    
                    returnLookupsSeconday("return_CSO_list",string.Empty, cboCSO, "cso_id", "cso_name");
                    #endregion Lookups
 
                    CreateHES_Categories();

                    GetHES_report(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text);
                    Load_Chart(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text, "Bar");
                } 
            }
        }

        protected void GetHES_report(string dst_id,string cso_id,string reportType)
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

            #region Load Report
            switch (reportType)
            {
                case "Agronomy Training":
                    dt = HES_reports.GetHES_report(dst_id,cso_id,"Agronomy",datefrom,dateTo);
                    break;
                case "Apprenticeship":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "Apprenticeship", datefrom, dateTo);
                    break;
                case "AFLateen":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "AFLateen", datefrom, dateTo);
                    break;
                case "Youth in savings groups":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "YouthGroup", datefrom, dateTo);
                    break;
                case "Care giver group":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "CaregiverGroup", datefrom, dateTo);
                    break;
                case "Belong to SILC Group":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "SILC", datefrom, dateTo);
                    break;
                case "Better Parenting":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "better_parenting", datefrom, dateTo);
                    break;
                case "Financial Literacy":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "better_parenting", datefrom, dateTo);
                    break;
                case "Faithful house":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "faithful_house", datefrom, dateTo);
                    break;
                case "Youth entrepreneurship skills":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "entrepreneurship_skills", datefrom, dateTo);
                    break;
            }
           
            if (dt.Rows.Count > 0)
            {
                gdvHES.DataSource = dt;
                gdvHES.DataBind();

                #region Set Header
                lblGridHeader.Text = "Number of Youth Reached for " + cboReportname.Text ;
                if (reportType == "Better Parenting")
                {
                    lblGridHeader.Text = "Households reached with:" + reportType;
                }
                else if (reportType == "Financial Literacy")
                {
                    lblGridHeader.Text = "Households reached with:" + reportType;
                }
                else if (reportType == "Faithful house")
                {
                    lblGridHeader.Text = "Households reached with:" + reportType;
                }
                else if (reportType == "Youth entrepreneurship skills")
                {
                    lblGridHeader.Text = "Households reached with:" + reportType;
                }
                lblgraphHeader.Text = "Visualized Dataset: " + cboReportname.Text;
                #endregion Set Header
            }
            else
            {
                dt = null;
                gdvHES.DataSource = dt;
                gdvHES.DataBind();
            }
            #endregion Load Report

        }

        protected void Load_Chart(string dst_id,string cso_id, string reportType, string chartType)
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

            switch (reportType)
            {
                case "Agronomy Training":
                    dt = HES_reports.GetHES_report(dst_id, cso_id, "Agronomy", datefrom, dateTo);
                    break;
                case "Apprenticeship":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "Apprenticeship", datefrom, dateTo);
                    break;
                case "AFLateen":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "AFLateen", datefrom, dateTo);
                    break;
                case "Youth in savings groups":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "YouthGroup", datefrom, dateTo);
                    break;
                case "Care giver group":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "CaregiverGroup", datefrom, dateTo);
                    break;
                case "Belong to SILC Group":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "SILC", datefrom, dateTo);
                    break;
                case "Better Parenting":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "better_parenting", datefrom, dateTo);
                    break;
                case "Financial Literacy":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "better_parenting", datefrom, dateTo);
                    break;
                case "Faithful house":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "faithful_house", datefrom, dateTo);
                    break;
                case "Youth entrepreneurship skills":
                    dt = HES_reports.GetHES_report(dst_id, cboCSO.SelectedValue.ToString(), "entrepreneurship_skills", datefrom, dateTo);
                    break;
            }
            string[] x = new string[dt.Rows.Count];
            int[] y = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][1]);
            }
            char_HES.Series[0].Points.DataBindXY(x, y);
            if (chartType == "Bar")
            {
                char_HES.Series[0].ChartType = SeriesChartType.Bar;
            }
            else if (chartType == "Pie")
            {
                char_HES.Series[0].ChartType = SeriesChartType.Pie;
            }
            else if (chartType == "Line")
            {
                char_HES.Series[0].ChartType = SeriesChartType.Line;
            }
           // char_HES.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            char_HES.Legends[0].Enabled = true;
            char_HES.Visible = true;
        }

        protected void rdnPie_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnPie.Checked == true)
            {
                rdnBar.Checked = false;
                rdnLine.Checked = false;
                Load_Chart(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(),cboReportname.Text,"Pie");
            }
        }

        protected void rdnBar_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnBar.Checked == true)
            {
                rdnLine.Checked = false;
                rdnPie.Checked = false;
                Load_Chart(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text, "Bar");
            }
           
        }

        protected void rdnLine_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnLine.Checked == true)
            {
                rdnPie.Checked = false;
                rdnBar.Checked = false;
                Load_Chart(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text, "Line");
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

        protected  void CreateHES_Categories()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Category", typeof(string));

            table.Rows.Add("Agronomy Training");
            table.Rows.Add("Apprenticeship");
            table.Rows.Add("AFLateen");
            table.Rows.Add("Youth in savings groups");
            table.Rows.Add("Care giver group");
            table.Rows.Add("Belong to SILC Group");
            table.Rows.Add("Better Parenting");
            table.Rows.Add("Financial Literacy");
            table.Rows.Add("Faithful house");
            table.Rows.Add("Youth entrepreneurship skills");
           
            cboReportname.DataTextField = "Category";
            cboReportname.DataValueField = "Category";
            cboReportname.DataSource = table;
            cboReportname.DataBind();
        }

        protected void cboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetHES_report(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text);
            //Load_Chart(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text, "Bar");
        }

        protected void cboReportname_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetHES_report(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text);
            //Load_Chart(cboDistrict.SelectedValue.ToString(),cboCSO.SelectedValue.ToString(), cboReportname.Text, "Bar");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            GetHES_report(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), cboReportname.Text);
            Load_Chart(cboDistrict.SelectedValue.ToString(), cboCSO.SelectedValue.ToString(), cboReportname.Text, "Bar");
        }
    }
}
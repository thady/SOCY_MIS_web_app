using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOCY_WEBAppTest.AppCode;
using System.Data;
//using System.Web.UI.DataVisualization.Charting;
using System.Web.Caching;
using InfoSoftGlobal;
using FusionCharts.Charts;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Configuration;

namespace SOCY_WEBAppTest
{
    public partial class _default : System.Web.UI.Page
    {
        DataTable dt = null;
        string dst_id = string.Empty;
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
                    _LoadChart_Hiv_positive_Art_status();
                    LoadChart_households_served_in_quarter();

                    _LoadChart_Hiv_stat();
                    _LoadChart_beneficiaries_served_in_quarter();
                }
            }
            
        }

        #region Charts
        protected void LoadChart()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Number of active households' xAxisName='District' yAxisName='Households' showValues='0' formatNumberScale='0' showBorder='1' theme='fusion'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name ,hhs_staus AS hh_status FROM dashboard_active_households";

                SqlCommand query = new SqlCommand(strsQL, conn);

                // Begin iterating through the result set
                SqlDataReader rst = query.ExecuteReader();


                while (rst.Read())
                {
                    xmlStr.AppendFormat("<set label='{0}' value = '{1}'/>", rst["dst_name"].ToString(), rst["hh_status"].ToString());
                }

                // End the XML string
                xmlStr.Append("</chart>");


                // Close the result set Reader object and the Connection object
                rst.Close();
                conn.Close();
                conn.Dispose();
                Chart hh_chart = new Chart("column2d", "hh_chart", "100%", "350", "xml", xmlStr.ToString());
                lit_active_households.Text = hh_chart.Render();
            }
        }

        protected void LoadChart_beneficiaries()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Number of active OVCs' xAxisName='District' yAxisName='Total OVC' showValues='0' formatNumberScale='0' showBorder='1' theme='zune'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name, hh_status FROM dashboard_active_household_member";

                SqlCommand query = new SqlCommand(strsQL, conn);

                // Begin iterating through the result set
                SqlDataReader rst = query.ExecuteReader();


                while (rst.Read())
                {
                    xmlStr.AppendFormat("<set label='{0}' value = '{1}'/>", rst["dst_name"].ToString(), rst["hh_status"].ToString());
                }

                // End the XML string
                xmlStr.Append("</chart>");


                // Close the result set Reader object and the Connection object
                rst.Close();
                conn.Close();
                conn.Dispose();
                Chart hhm_chart = new Chart("column2d", "hhm_chart", "100%", "350", "xml", xmlStr.ToString());
                lit_active_household_members.Text = hhm_chart.Render();
            }
        }

        protected void LoadChart_beneficiaries_served_in_quarter()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='OVCs served in current quarter' xAxisName='District' yAxisName='Total OVCs served' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            string tmp = null;
            tmp = @"<categories font='Arial' fontSize='11' fontColor='000000'>";

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name,total_hhm FROM dashboard_current_quarter_hhm_served";

                SqlCommand query = new SqlCommand(strsQL, conn);

                // Begin iterating through the result set
                SqlDataReader rst = query.ExecuteReader();

                while (rst.Read())
                {
                    xmlStr.AppendFormat("<set label='{0}' value = '{1}' link='j-getNode_position-{0}'/>", rst["dst_name"].ToString(), rst["total_hhm"].ToString());
                }

                // End the XML string
                xmlStr.Append("</chart>");

                // Close the result set Reader object and the Connection object
                rst.Close();
                conn.Close();
                conn.Dispose();
                Chart hhm_served_chart = new Chart("line", "hhm_served_chart", "100%", "350", "xml", xmlStr.ToString());
                lit_ben_served.Text = hhm_served_chart.Render();
            }
        }

        protected void LoadChart_households_served_in_quarter()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;
            DataRow dtRow = null;

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Households served in current quarter' xAxisName='District' yAxisName='Households served' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            xmlStr.Append("<categories font='Arial' fontSize='11' fontColor='000000'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT S.dst_name,S.total_hh,hh.hhs_staus AS total_hh_target FROM dashboard_current_quarter_hh_served S
                                  INNER JOIN dashboard_active_households hh ON S.dst_name = hh.dst_name";

                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);

                #region Chart categories
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<category label ='" + dtRow["dst_name"].ToString() + "' />");
                }
                xmlStr.Append("</categories>");

                #endregion Chart categories

                #region Chart Datasets
                xmlStr.Append("<dataset seriesName='Households Served' color='1D8BD1' anchorBorderColor='1D8BD1' anchorBgColor='1D8BD1'>");

                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_hh"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='Active Households' color='008E8E' anchorBorderColor='008E8E' anchorBgColor='008E8E'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_hh_target"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");
                #endregion Chart Datasets

                // End the XML string
                xmlStr.Append("</chart>");

                conn.Close();
                conn.Dispose();
                Chart hhm_served_chart = new Chart("msline", "hh_served_chart", "100%", "350", "xml", xmlStr.ToString());
              
                lit_households_served.Text = hhm_served_chart.Render();
            }
        }


        protected void _LoadChart_beneficiaries_served_in_quarter()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
         
            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;
            DataRow dtRow = null;

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='OVCs served in current quarter' xAxisName='District' yAxisName='Total OVCs served' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            xmlStr.Append("<categories font='Arial' fontSize='11' fontColor='000000'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT S.dst_name,S.total_hhm,hhm.hh_status AS total_hhm_target,T.ovc_target  FROM dashboard_current_quarter_hhm_served S
                                INNER JOIN dashboard_active_household_member hhm ON S.dst_name = hhm.dst_name
                                LEFT JOIN dashboard_ovc_target T ON S.dst_name = T.dst_name";

                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);


                #region Chart categories
                for (int x = 0;x<_dt.Rows.Count;x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<category label ='" + dtRow["dst_name"].ToString() + "' />");
                }
                xmlStr.Append("</categories>");

                #endregion Chart categories

                #region Chart Datasets
                xmlStr.Append("<dataset seriesName='OVC Served' color='0963F8' anchorBorderColor='0963F8' anchorBgColor='0963F8'>");

                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    string err = "Kyoteraa";
                   // xmlStr.Append("<set value='" + dtRow["total_hhm"].ToString() + "'/>");
                    xmlStr.AppendFormat("<set value = '{0}' link='{1}'/>", dtRow["total_hhm"].ToString(), Server.UrlEncode("P-detailsWin,width=800,height=400,toolbar=no,scrollbars=no, resizable=0-detailed_modal.aspx?token_id=" + dtRow["dst_name"].ToString() + "&modalType=ovc_serve"));                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='Active Beneficiaries' color='09F846' anchorBorderColor='09F846' anchorBgColor='09F846'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_hhm_target"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='COP18 Target' color='F83309' anchorBorderColor='F83309' anchorBgColor='F83309' dashed = '1'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["ovc_target"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");
                #endregion Chart Datasets

                // End the XML string
                xmlStr.Append("</chart>");
                conn.Close();
                conn.Dispose();
                Chart hhm_served_chart = new Chart("msline", "hhm_served_chart", "100%", "350", "xml", xmlStr.ToString());
                lit_ben_served.Text = hhm_served_chart.Render();
            }
        }

        protected void _LoadChart_Hiv_stat()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;
            DataRow dtRow = null;

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='OVC HIV STAT' xAxisName='District' yAxisName='HIV Status' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            xmlStr.Append("<categories font='Arial' fontSize='11' fontColor='000000'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name, total_negative,total_positive,total_unknown FROM dashboard_hiv_stat
                                  WHERE dst_name <> 'KASESE' AND dst_name <> 'KAMWENGE'";

                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);


                #region Chart categories
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<category label ='" + dtRow["dst_name"].ToString() + "' />");
                }
                xmlStr.Append("</categories>");

                #endregion Chart categories

                #region Chart Datasets
                xmlStr.Append("<dataset seriesName='OVC Reported Negative' color='0963F8' anchorBorderColor='0963F8' anchorBgColor='0963F8'>");

                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_negative"].ToString() + "'/>");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='OVC Reported Positive' color='09F846' anchorBorderColor='09F846' anchorBgColor='09F846'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_positive"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='Uknown Status' color='F83309' anchorBorderColor='F83309' anchorBgColor='F83309' dashed = '1'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_unknown"].ToString() + "' />");
                }

                xmlStr.Append("</dataset>");
                #endregion Chart Datasets

                // End the XML string
                xmlStr.Append("</chart>");

                conn.Close();
                conn.Dispose();
                Chart hiv_stat = new Chart("stackedcolumn2d", "hiv_stat", "100%", "350", "xml", xmlStr.ToString());
                lit_active_household_members.Text = hiv_stat.Render();
            }
        }

        protected void _LoadChart_Hiv_positive_Art_status()
        {
            // Construct the connection string to interface with the SQL Server Database
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;
            DataRow dtRow = null;

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='HIV positive on ART Vs not on ART' xAxisName='District' yAxisName='HIV ART Status' showValues='0' formatNumberScale='0' showBorder='1' theme='zune' labelDisplay='Auto' useEllipsesWhenOverflow = '0' numdivlines='3' numVdivlines='0' rotateNames='1'>");
            xmlStr.Append("<categories font='Arial' fontSize='11' fontColor='000000'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst_name,total_on_art,total_not_on_art FROM dashboard_positive_art
                                  WHERE dst_name <> 'KASESE' AND dst_name <> 'KAMWENGE'";

                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);


                #region Chart categories
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<category label ='" + dtRow["dst_name"].ToString() + "' />");
                }
                xmlStr.Append("</categories>");

                #endregion Chart categories

                #region Chart Datasets
                xmlStr.Append("<dataset seriesName='On ART' color='0963F8' anchorBorderColor='0963F8' anchorBgColor='0963F8'>");

                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    xmlStr.Append("<set value='" + dtRow["total_on_art"].ToString() + "'/>");
                }

                xmlStr.Append("</dataset>");

                xmlStr.Append("<dataset seriesName='Not on ART' color='09F846' anchorBorderColor='09F846' anchorBgColor='09F846'>");
                for (int x = 0; x < _dt.Rows.Count; x++)
                {
                    dtRow = _dt.Rows[x];
                    string district_name = dtRow["dst_name"].ToString();

                    xmlStr.AppendFormat("<set value = '{0}' link='{1}'/>", dtRow["total_not_on_art"].ToString(), Server.UrlEncode("P-detailsWin,width=800,height=400,toolbar=no,scrollbars=no, resizable=0-detailed_modal.aspx?token_id=" + dtRow["dst_name"].ToString() + "&modalType=hiv_art"));
                }

                xmlStr.Append("</dataset>");

                #endregion Chart Datasets

                // End the XML string
                xmlStr.Append("</chart>");

                conn.Close();
                conn.Dispose();
                Chart hiv_art = new Chart("msline", "hiv_art", "100%", "350", "xml", xmlStr.ToString());
                lit_active_households.Text = hiv_art.Render();
            }
        }
        #endregion Charts

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            SystemConstants.LogOut("LogIn");
        }
       
    }
}
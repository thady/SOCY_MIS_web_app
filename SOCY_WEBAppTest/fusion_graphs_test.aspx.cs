using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using InfoSoftGlobal;
using FusionCharts.Charts;

namespace SOCY_WEBAppTest
{
    public partial class fusion_graphs_test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { LoadChart(); }

        }

        protected void LoadChart()
        {
            // Construct the connection string to interface with the SQL Server Database
             SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_MIS"].ToString());

            // Initialize the string which would contain the chart data in XML format
            StringBuilder xmlStr = new StringBuilder();

            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Number of Active households by district' xAxisName='District' yAxisName='Sub County' showValues='0' formatNumberScale='0' showBorder='1' theme='fusion'>");

            // Create a SQLConnection object 
            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                // Construct and execute SQL query which would return the total amount of sales for each year
                string strsQL = @"SELECT dst.dst_name,hs.hhs_name,COUNT(hs.hhs_name) AS hh_status FROM hh_household hh
                                INNER JOIN lst_ward W ON hh.wrd_id = W.wrd_id
                                INNER JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
                                INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id
                                INNER JOIN lst_household_status hs ON hh.hhs_id = hs.hhs_id
                                WHERE hs.hhs_id = '1'
                                GROUP BY dst.dst_name,hs.hhs_name";

                SqlCommand query = new SqlCommand(strsQL, conn);

                // Begin iterating through the result set
                SqlDataReader rst = query.ExecuteReader();
                

                while (rst.Read())
                {
                    xmlStr.AppendFormat("<set label='{0}' value = '{1}'/>", rst["dst_name"].ToString(), rst["hh_status"].ToString());
                }

                // End the XML string
                xmlStr.Append("</chart>");

                StringBuilder xmlData = new StringBuilder();

                xmlData.Append("<chart caption='Status Of Period' numberSuffix='%'>");
                xmlData.AppendFormat("<set label='Period Elapsed' value='4'/>");//temp1.ToString());
                xmlData.AppendFormat("<set label='Total Expensed' value='5'/>");//temp2.ToString());
                xmlData.AppendFormat("<set label='Hours Delivered' value='6'/>");//temp3.ToString());
                xmlData.Append("</chart>");

                // Close the result set Reader object and the Connection object
                rst.Close();
                conn.Close();
                Chart MyFirstChart = new Chart("column2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                //Chart MyFirstChart = new Chart("column2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                //Chart MyFirstChart3 = new Chart("pie2d", "MyFirstChart", "700", "400", "xml", xmlStr.ToString());
                Literal1.Text = MyFirstChart.Render();
               
                // Call the RenderChart method, pass the correct parameters, and write the return value to the Literal tag
                //chart_from_db.Text = MyFirstChart.Render(
                //    "../FusionChartsXT/FCF_MSColumn3D.swf", // Path to chart's SWF
                //    "",         // Leave blank when using Data String method
                //    xmlStr.ToString(),  // xmlStr contains the chart data
                //    "annual_revenue",   // Unique chart ID
                //    "640", "340",       // Width & Height of chart
                //    false,              // Disable Debug Mode
                //    true);              // Register with JavaScript object
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;

namespace SOCY_WEBAppTest
{
    public partial class ChartSample : System.Web.UI.Page
    {
        static string SQLConnection = System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ConnectionString;
        SqlConnection conn = new SqlConnection(SQLConnection);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public List<WebService1> getTrafficSourceData(List<string> gData)
        {
            List<WebService1> t = new List<WebService1>();
            string[] arrColor = new string[] { "#231F20", "#FFC200", "#F44937", "#16F27E", "#FC9775", "#5A69A6" };

            using (conn)
            {
                string myQuery = @";with cte_HH_Last_Home_visit
                                    AS
                                    (SELECT V.hh_id,hs.hhs_name, V.hhv_date, RANK() OVER (PARTITION BY V.hh_id ORDER BY V.hhv_date) AS rnk
                                    FROM hh_household_home_visit V
                                    INNER JOIN lst_household_status hs ON V.hvhs_id = hs.hhs_id
                                    WHERE YEAR(hhv_date) >= @year1 AND YEAR(hhv_date) <= @year2
                                    )
                                    ,
                                    Cte_select_last_hh_visit AS(
                                    SELECT hh_id,hhs_name, hhv_date FROM cte_HH_Last_Home_visit
                                    WHERE rnk = 1
                                    ),
                                    CteQ1 AS (
                                    SELECT YEAR(hhv_date) AS YEAR,COUNT(hhs_name) AS Q1Total FROM Cte_select_last_hh_visit
                                    WHERE hhs_name = 'Active' AND MONTH(hhv_date) >= 10 AND MONTH(hhv_date) <= 12
                                    GROUP BY ALL YEAR(hhv_date)
                                    ),
                                    CteQ2 AS (
                                    SELECT YEAR(hhv_date) AS YEAR,COUNT(hhs_name) AS Q2Total FROM Cte_select_last_hh_visit
                                    WHERE hhs_name = 'Active' AND MONTH(hhv_date) >= 1 AND MONTH(hhv_date) <= 3
                                    GROUP BY ALL YEAR(hhv_date)
                                    ),
                                    CteQ3 AS (
                                    SELECT YEAR(hhv_date) AS YEAR,COUNT(hhs_name) AS Q3Total FROM Cte_select_last_hh_visit
                                    WHERE hhs_name = 'Active' AND MONTH(hhv_date) >= 4 AND MONTH(hhv_date) <= 6
                                    GROUP BY ALL YEAR(hhv_date)
                                    ),
                                    CteQ4 AS (
                                    SELECT YEAR(hhv_date) AS YEAR,COUNT(hhs_name) AS Q4Total FROM Cte_select_last_hh_visit
                                    WHERE hhs_name = 'Active' AND MONTH(hhv_date) >= 7 AND MONTH(hhv_date) <= 9
                                    GROUP BY ALL YEAR(hhv_date)
                                    )
                                    SELECT CteQ1.YEAR,CteQ1.Q1Total,CteQ2.Q2Total,CteQ3.Q3Total,CteQ4.Q4Total FROM CteQ1
                                    INNER JOIN CteQ2 ON CteQ1.YEAR = CteQ2.YEAR
                                    INNER JOIN CteQ3 ON CteQ2.YEAR = CteQ3.YEAR
                                    INNER JOIN CteQ4 ON CteQ3.YEAR = CteQ4.YEAR";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = myQuery;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@year1", gData[0]);
                cmd.Parameters.AddWithValue("@year2", gData[1]);
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    int counter = 0;
                    while (dr.Read())
                    {
                        WebService1 tsData = new WebService1();
                        tsData.value = dr["Q1Total"].ToString();
                        tsData.value = dr["Q2Total"].ToString();
                        tsData.value = dr["Q3Total"].ToString();
                        tsData.value = dr["Q4Total"].ToString();
                        tsData.label = dr["YEAR"].ToString();
                        tsData.color = arrColor[counter];
                        t.Add(tsData);
                        counter++;
                    }
                }
            }
            return t;
        }

        [WebMethod]
        public static string GetChart(string status)
        {
            string constr = ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string strSQL = @"SELECT dst.dst_name,COUNT(sct.sct_id) FROM lst_sub_county sct
                                INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id
                                WHERE dst.dst_active = '{0}'
                                GROUP BY dst.dst_name";
                string query = string.Format(strSQL, status);
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                        while (sdr.Read())
                        {
                            sb.Append("{");
                            System.Threading.Thread.Sleep(50);
                            string color = String.Format("#{0:X6}", new Random().Next(0x1000000));
                            sb.Append(string.Format("text :'{0}', value:{1}, color: '{2}'", sdr[0], sdr[1], color));
                            sb.Append("},");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");
                        con.Close();
                        return sb.ToString();
                    }
                }
            }
        }
    }
}
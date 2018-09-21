using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace SOCY_WEBAppTest.AppCode
{
    public class HES_reports
    {

        #region DBConnection
        static SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
        #endregion DBConnection

        public static DataTable GetHES_report(string dst_id,string cso_id,string report_type,DateTime? datefrom,DateTime? dateTo)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlDataAdapter Adapt;
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand("sp_SOCY_hh_Household_economic_strengthening_reports", conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@district", SqlDbType.VarChar, 50);
                    cmd.Parameters["@district"].Value = dst_id;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@cso", SqlDbType.VarChar, 50);
                    cmd.Parameters["@cso"].Value = cso_id;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@reportType", SqlDbType.VarChar, 150);
                    cmd.Parameters["@reportType"].Value = report_type;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@datecreateFrom", SqlDbType.Date);
                    cmd.Parameters["@datecreateFrom"].Value = datefrom;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@datecreateTo", SqlDbType.Date);
                    cmd.Parameters["@datecreateTo"].Value = dateTo;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Adapt = new SqlDataAdapter(cmd);
                    Adapt.Fill(dt);

                    int x = dt.Rows.Count;
                    cmd.Parameters.Clear();

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return dt;
        }
    }
}
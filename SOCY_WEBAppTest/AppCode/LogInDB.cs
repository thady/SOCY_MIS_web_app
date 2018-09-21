using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace SOCY_WEBAppTest.AppCode
{
    public class LogInDB
    {
        static SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

        /// <summary>
        /// Validates specified email and password against the values in the database by using the utilUserAccess class
        /// </summary>
        /// <param name="strUserEmail">Email of User to be logged in</param>
        /// <param name="strPassword">Password of User to be logged in</param>
        /// <param name="strLngID">Language Message must be returned in</param>
        /// <returns>Returns string array, value key pair, based on log in result</returns>
        public string[] LogIn(string strUserEmail, string strPassword)
        {
            #region Variables
            utilUserAccess utilAU = new utilUserAccess();
            DBConnection dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);

            string[] arrResult;
            #endregion Variables

            try
            {
                arrResult = utilAU.LogIn(strUserEmail, strPassword, dbCon);
            }
            finally
            {
                dbCon.Dispose();
            }

            return arrResult;
        }

        public static DataTable Set_user_region_district(string usr_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string strQuery = "  SELECT rgn_id,dst_id FROM um_user WHERE usr_id = '"+ usr_id +"'";
            try
            {

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;
                   
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Adapt = new SqlDataAdapter(cmd);
                    Adapt.Fill(dt);

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
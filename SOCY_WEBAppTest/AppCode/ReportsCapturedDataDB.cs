using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace SOCY_WEBAppTest.AppCode
{
    public static class ReportsCapturedDataDB
    {
        #region DBConnection
        static SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
        #endregion DBConnection

        #region Variables
        public static string reportType = string.Empty;
        public static string prt_id = null;
        public static string cso = null;
        public static string region = null;
        public static string district = null;
        public static string subcounty = null;
        public static string parish = null;
        public static DateTime? datecreateFrom = null;
        public static DateTime? datecreateTo = null;
        public static string lastuploadOffice = null;
        public static DateTime? uploaddateFrom = null;
        public static DateTime? uploaddateTo = null;
        public static DateTime? processdateFrom = null;
        public static DateTime? processdateTo = null;
        public static string ReportTitle = string.Empty;
        #endregion Variables


        public static DataSet GetReportData()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlDataAdapter Adapt;
            try
            {

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand("sp_SOCYReportSQueries", conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@reportType", SqlDbType.VarChar, 150);
                    cmd.Parameters["@reportType"].Value = reportType;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@prt_id", SqlDbType.VarChar, 50);
                    cmd.Parameters["@prt_id"].Value = prt_id;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@cso", SqlDbType.VarChar, 50);
                    cmd.Parameters["@cso"].Value = cso;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@region", SqlDbType.VarChar, 50);
                    cmd.Parameters["@region"].Value = region;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@district", SqlDbType.VarChar, 100);
                    cmd.Parameters["@district"].Value = district;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@subcounty", SqlDbType.VarChar, 200);
                    cmd.Parameters["@subcounty"].Value = subcounty;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@parish", SqlDbType.VarChar, 400);
                    cmd.Parameters["@parish"].Value = parish;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@datecreateFrom", SqlDbType.DateTime);
                    cmd.Parameters["@datecreateFrom"].Value = datecreateFrom;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@datecreateTo", SqlDbType.Date);
                    cmd.Parameters["@datecreateTo"].Value = datecreateTo;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Adapt = new SqlDataAdapter(cmd);
                    Adapt.Fill(ds);
                    if (reportType == "HouseholdReferral")
                    {
                        ds.Tables[0].TableName = "Referral";
                        ds.Tables[1].TableName = "ServicesToChild";
                        ds.Tables[2].TableName = "FeedbackServices";
                    }
                    else if (reportType == "SocialWorker")
                    {
                        ds.Tables[0].TableName = "Social Worker";
                        ds.Tables[1].TableName = "Para Social Worker";
                    }
                    else if (reportType == "Linkages")
                    {
                        ds.Tables[0].TableName = "Linkages for ES";
                        ds.Tables[1].TableName = "Linkages Services Provided";
                        ds.Tables[2].TableName = "Linkages Services Required";
                    }
                    else
                    {
                        ds.Tables[0].TableName = "Exported"; //set a static variable for this
                    }
                    
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

            return ds;
        }
    }
}
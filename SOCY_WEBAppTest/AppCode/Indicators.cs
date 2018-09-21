using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace SOCY_WEBAppTest.AppCode
{
    public class Indicators
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

        public static DataTable GetOvcServerData(string indicator_name)
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
                    cmd.Parameters["@reportType"].Value = indicator_name;

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

        public static DataTable GetOvcSERVEData(string dst_id,DateTime dtFrom,DateTime dtTo)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
           
                SQL = @";With Cte_DSD_OVC_Serve
				AS(
					SELECT hh_household_home_visit_member.hhm_id FROM hh_household_home_visit_member
					LEFT JOIN hh_household_home_visit V ON hh_household_home_visit_member.hhv_id = V.hhv_id
					LEFT JOIN hh_household H ON V.hh_id = H.hh_id
					LEFT JOIN lst_ward W ON H.wrd_id = W.wrd_id
					LEFT JOIN lst_sub_county sct ON sct.sct_id = W.sct_id
					LEFT JOIN lst_district dst ON dst.dst_id = sct.dst_id
				    WHERE yn_id_hhm_active = 1
				    AND (
				    yn_id_edu_sensitised = 1
				    OR yn_id_es_aflateen = 1
				    OR yn_id_es_agro = 1
				    OR yn_id_es_apprenticeship = 1
				    OR yn_id_es_caregiver_group = 1
				    OR yn_id_es_silc = 1
				    OR yn_id_fsn_nutrition = 1
				    OR yn_id_fsn_referred = 1
				    OR yn_id_fsn_wash = 1
				    OR ynna_id_edu_attend_school_regularly = 1
				    OR ynna_id_edu_enrolled = 1
				    OR ynna_id_edu_support = 1
				    OR ynna_id_fsn_education = 1
				    OR ynna_id_fsn_support = 1
				    OR ynna_id_hhp_adhering = 1
				    OR ynna_id_hhp_art =1
				    OR ynna_id_hhp_referred = 1
				    OR ynna_id_pro_birth_certificate =1
				    OR ynna_id_pro_birth_registration =1
				    OR ynna_id_pro_child_abuse = 1
				    OR ynna_id_pro_child_labour = 1
				    OR ynna_id_pro_reintegrated = 1
				    OR ynna_id_ps_parenting = 1
				    OR ynna_id_ps_support = 1
				    OR ynna_id_ps_violence = 1
				    OR yn_id_es_caregiver_group = 1
				    OR ynna_id_es_other_lending_group = 1
				    OR ynna_id_edu_attend_school_regularly = 1
				    )
				    AND V.hhv_date BETWEEN '{1}' AND '{2}'
				    AND dst.dst_id = '{0}'
				    ),Cte_Stats AS(
				    SELECT(SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve) AS [Total],
				    (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) < 1 AND HHM.hhm_year_of_birth <> '*')) AS [Age<1],(SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 1 AND YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) <= 9 AND HHM.hhm_year_of_birth <> '*')) AS [Age 1 to 9],
				     (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 10 AND YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) <= 14 AND HHM.gnd_id = 'm26e435b-1478-4978-aad5-58c3677a1f70' AND HHM.hhm_year_of_birth <> '*')) AS [Age 10-14 Male],
				      (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 10 AND YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) <= 14 AND HHM.gnd_id = 'f05d3f3c-9aac-4f12-b0cd-1c4ae9294da3' AND HHM.hhm_year_of_birth <> '*')) AS [Age 10-14 Female],
				     (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 15 AND YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) <= 17 AND HHM.gnd_id = 'm26e435b-1478-4978-aad5-58c3677a1f70' AND HHM.hhm_year_of_birth <> '*')) AS [Age 15-17 Male],
				     (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 15 AND YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) <= 17 AND HHM.gnd_id = 'f05d3f3c-9aac-4f12-b0cd-1c4ae9294da3' AND HHM.hhm_year_of_birth <> '*')) AS [Age 15-17 Female],
				     (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 18 AND YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) <= 24 AND HHM.gnd_id = 'm26e435b-1478-4978-aad5-58c3677a1f70' AND HHM.hhm_year_of_birth <> '*')) AS [Age 18-24 Male],
				      (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 18 AND YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) <= 24 AND HHM.gnd_id = 'f05d3f3c-9aac-4f12-b0cd-1c4ae9294da3' AND HHM.hhm_year_of_birth <> '*')) AS [Age 18-24 Female],
				      (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 25 AND HHM.gnd_id = 'm26e435b-1478-4978-aad5-58c3677a1f70' AND HHM.hhm_year_of_birth <> '*')) AS [Age 25+ Male],
				     (SELECT COUNT(Cte_DSD_OVC_Serve.hhm_id) FROM Cte_DSD_OVC_Serve LEFT JOIN hh_household_member HHM ON Cte_DSD_OVC_Serve.hhm_id = HHM.hhm_id WHERE
				     (YEAR(GETDATE()) - CONVERT(INT,  HHM.hhm_year_of_birth) >= 25 AND HHM.gnd_id = 'f05d3f3c-9aac-4f12-b0cd-1c4ae9294da3' AND HHM.hhm_year_of_birth <> '*')) AS [Age 25+ Female]
				     ),cte_Pivoted AS
				     (
				     SELECT 'Total' AS OVC_SERVE_INDICATOR,[Total] FROM Cte_Stats
				     UNION ALL
				     SELECT 'Age<1' AS [statNAME],[Age<1] FROM Cte_Stats
				     UNION ALL
				     SELECT '1--9' AS statName,[Age 1 to 9] FROM Cte_Stats
				     UNION ALL
				     SELECT '10-14 Male' AS statName, [Age 10-14 Male] FROM Cte_Stats
				     UNION ALL
				     SELECT '10-14 Female' AS statName, [Age 10-14 Female] FROM Cte_Stats
				     UNION ALL
				     SELECT '15-17 Male' AS statName, [Age 15-17 Male] FROM Cte_Stats
				     UNION ALL
				     SELECT '15-17 Female' AS statName, [Age 15-17 Female] FROM Cte_Stats
				     UNION ALL
				     SELECT '18-24 Male' AS statName, [Age 18-24 Male] FROM Cte_Stats
				     UNION ALL
				     SELECT '18-24 Female' AS statName, [Age 18-24 Female] FROM Cte_Stats
				     UNION ALL
				     SELECT '25+ Male' AS statName, [Age 25+ Male] FROM Cte_Stats
				     UNION ALL
				     SELECT '25+ Female' AS statName, [Age 25+ Female] FROM Cte_Stats
				     )
				     SELECT P.OVC_SERVE_INDICATOR,I.Target,P.Total AS [Value],CAST(ROUND(100.0 * P.Total / I.Target, 1) AS DECIMAL(10, 2))  AS percentage_Archived FROM cte_Pivoted P 
				     LEFT JOIN TblOVCServeIndicators I ON P.OVC_SERVE_INDICATOR = I.OVC_SERVE_INDICATOR 
				     WHERE I.dst_id = '{0}'";

                SQL = string.Format(SQL, dst_id, dtFrom, dtTo);

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
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
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }

            return dt;
        }

        public static DataTable LoadDistricts()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT* FROM lst_district WHERE dst_id <> 8 AND dst_id <> 6";

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
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
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }

            return dt;
        }


        public static DataTable ReturnHHBenStats(string QueryName, string disdtrict_id,string sct_id)
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
                    cmd.Parameters["@reportType"].Value = QueryName;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@district", SqlDbType.VarChar, 150);
                    cmd.Parameters["@district"].Value = disdtrict_id;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@subcounty", SqlDbType.VarChar, 150);
                    cmd.Parameters["@subcounty"].Value = sct_id;


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
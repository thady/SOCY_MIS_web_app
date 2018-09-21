using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data.SqlClient;
using System.Data;

namespace SOCY_WEBAppTest.AppCode
{
    public static class Lookups
    {
        #region Variables
        public static string total_ovc = string.Empty;
        public static string ovc_report_hiv_status = string.Empty;
        public static string ovc_report_hiv_status_percent = string.Empty;
        public static string ovc_on_art = string.Empty;
        public static string ovc_on_art_adhering = string.Empty;
        public static string ovc_not_on_art = string.Empty;
        public static string ovc_reported_hiv_neg_to_partner = string.Empty;
        public static string ovc_reported_hiv_pos_to_partner = string.Empty;
        public static string ovc_no_reported_hiv_status_to_partner = string.Empty;

        public static string total_eco_strengthening = string.Empty;
        public static string total_education_support = string.Empty;
        public static string total_food_security = string.Empty;
        public static string total_health_hiv_prevention = string.Empty;
        public static string total_protection = string.Empty;
        public static string total_pyschosocial_support = string.Empty;

        public static DateTime qm_date_begin = DateTime.Now;
        public static DateTime qm_date_end = DateTime.Now;

        #endregion Variables
        static SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

        public static DataTable ReturnLookupsPrimary(string QueryName)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            try
            {

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand("sp_SOCYReportSearchQueries", conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@queryName", SqlDbType.VarChar, 150);
                    cmd.Parameters["@queryName"].Value = QueryName;


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
        public static DataTable ReturnLookupsSecondary(string QueryName, string id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            try
            {
               
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand("sp_SOCYReportSearchQueries", conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@queryName", SqlDbType.VarChar, 150);
                    cmd.Parameters["@queryName"].Value = QueryName;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.VarChar, 50);
                    cmd.Parameters["@id"].Value = id;

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

        public static int ReturnLookupsStats(string QueryName)
        {

            int intCount = 0;
            try
            {

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand("sp_SOCYReportSearchQueries", conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@queryName", SqlDbType.VarChar, 150);
                    cmd.Parameters["@queryName"].Value = QueryName;


                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    intCount = (int)cmd.ExecuteScalar();

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

            return intCount;
        }

        public static DataTable ReturnDataEntryStatus(string QueryName)
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

        public static DataTable ReturnDataEntryStats()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
   //             SQL = @"SELECT dst.dst_name,COUNT(V.hh_id) AS hv FROM hh_household_home_visit V
			//RIGHT JOIN hh_household H ON V.hh_id = H.hh_id
			//RIGHT JOIN lst_ward W ON H.wrd_id = W.wrd_id
			//RIGHT JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
			//RIGHT JOIN lst_district dst ON sct.dst_id = dst.dst_id
			//WHERE V.hhv_date BETWEEN '{0}' AND '{1}'
   //         AND dst.dst_id <> '6' AND dst.dst_id <> '8'
			//GROUP BY ALL dst.dst_name";

                SQL = @"With CteHomeVisits AS
			(
			SELECT dst.dst_name,COUNT(V.hh_id) AS [Home Visits] FROM hh_household_home_visit V
			RIGHT JOIN hh_household H ON V.hh_id = H.hh_id
			RIGHT JOIN lst_ward W ON H.wrd_id = W.wrd_id
			RIGHT JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
			RIGHT JOIN lst_district dst ON sct.dst_id = dst.dst_id
			WHERE V.hhv_date BETWEEN '{0}' AND '{1}'
            AND dst.dst_id <> '6' AND dst.dst_id <> '8'
			GROUP BY ALL dst.dst_name
			),
			CteRefferals AS
			(
			SELECT dst.dst_name,COUNT(R.rfr_id) AS [Referrals] FROM hh_referral R
			RIGHT JOIN hh_household_member hhm ON R.hhm_id = hhm.hhm_id
			RIGHT JOIN hh_household H ON hhm.hh_id = H.hh_id
			RIGHT JOIN lst_ward W ON H.wrd_id = W.wrd_id
			RIGHT JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
			RIGHT JOIN lst_district dst ON sct.dst_id = dst.dst_id
			WHERE R.rfr_ra_date BETWEEN '{0}' AND '{1}'
            AND dst.dst_id <> '6' AND dst.dst_id <> '8'
			GROUP BY ALL dst.dst_name
			),
			CteRiskAssessment AS
			(
			SELECT dst.dst_name,COUNT(RA.ras_id) AS [Risk Assessment] FROM hh_household_risk_assessment_header RA
			RIGHT JOIN hh_household H ON RA.hh_id = H.hh_id
			RIGHT JOIN lst_ward W ON H.wrd_id = W.wrd_id
			RIGHT JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
			RIGHT JOIN lst_district dst ON sct.dst_id = dst.dst_id
			WHERE RA.date_of_visit BETWEEN '{0}' AND '{1}'
            AND dst.dst_id <> '6' AND dst.dst_id <> '8'
			GROUP BY ALL dst.dst_name
			),
			CteImprovementPlan AS
			(
			SELECT dst.dst_name,COUNT(P.hip_id) AS [Improvement Plan] FROM hh_household_improvement_plan P
			RIGHT JOIN hh_household H ON P.hh_id = H.hh_id
			RIGHT JOIN lst_ward W ON H.wrd_id = W.wrd_id
			RIGHT JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
			RIGHT JOIN lst_district dst ON sct.dst_id = dst.dst_id
			WHERE P.visit_date BETWEEN '{0}' AND '{1}'
            AND dst.dst_id <> '6' AND dst.dst_id <> '8'
			GROUP BY ALL dst.dst_name
			),
			CteCommunityTrainingRegister AS
			(
			SELECT dst.dst_name,COUNT(TR.ctr_id) AS [Community Training Register] FROM silc_community_training_register TR
			RIGHT JOIN lst_district dst ON TR.dst_id = dst.dst_id
			WHERE TR.tr_date_from BETWEEN '{0}' AND '{1}'
            AND dst.dst_id <> '6' AND dst.dst_id <> '8'
			GROUP BY ALL dst.dst_name
			)
			SELECT CteHomeVisits.dst_name,CteHomeVisits.[Home Visits] AS hv,CteRefferals.Referrals,CteRiskAssessment.[Risk Assessment] AS RAS,CteImprovementPlan.[Improvement Plan] AS HIP,
			CteCommunityTrainingRegister.[Community Training Register] AS coomunity_tr
			FROM CteHomeVisits 
			LEFT JOIN CteRefferals on CteHomeVisits.dst_name = CteRefferals.dst_name
			LEFT JOIN CteRiskAssessment ON CteRiskAssessment.dst_name = CteHomeVisits.dst_name
			LEFT JOIN CteImprovementPlan ON CteHomeVisits.dst_name = CteImprovementPlan.dst_name
			LEFT JOIN CteCommunityTrainingRegister ON CteHomeVisits.dst_name = CteCommunityTrainingRegister.dst_name";

                SQL = string.Format(SQL, qm_date_begin, qm_date_end);

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

        public static DataTable ReturnCurrentQuarterDates()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT qm_id,qm_date_begin,qm_date_end
                        FROM lst_quarter_range WHERE qm_active = 1"; 

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

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dtRow = dt.Rows[0];
                        qm_date_begin = Convert.ToDateTime(dtRow["qm_date_begin"]);
                        qm_date_end = Convert.ToDateTime(dtRow["qm_date_end"]);
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
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }

            return dt;
        }

        public static DataTable ReturnDataEntryStatsGraph()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT D.dst_name,COUNT(H.hh_id) total_hh FROM hh_household H
                        LEFT JOIN lst_ward W ON W.wrd_id = H.wrd_id
                        LEFT JOIN lst_sub_county sct on w.sct_id = sct.sct_id
                        LEFT JOIN lst_district D ON sct.dst_id = D.dst_id
                        GROUP BY D.dst_name";

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


        public static DataTable ReturnOVCHIV_STAT()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
                #region Query
                SQL = @"
                    DECLARE @total_ovc int
                    DECLARE @ovc_report_hiv_status int
                    DECLARE @ovc_report_hiv_status_percent int
                    DECLARE @ovc_on_art int
                    DECLARE @ovc_on_art_adhering int
                    DECLARE @ovc_not_on_art int
                    DECLARE @ovc_reported_hiv_neg_to_partner int
                    DECLARE @ovc_reported_hiv_pos_to_partner int
                    DECLARE @ovc_no_reported_hiv_status_to_partner int
                    DECLARE @datefrom date = '2018-04-01'
                    DECLARE @dateTo date = '2018-06-30'

                   DECLARE @tempTable TABLE (total_ovc int,ovc_report_hiv_status int,ovc_report_hiv_status_percent int,ovc_on_art int,ovc_on_art_adhering int,ovc_not_on_art int
                    ,ovc_reported_hiv_neg_to_partner int,ovc_reported_hiv_pos_to_partner int,ovc_no_reported_hiv_status_to_partner int )

                    BEGIN

                    SET @ovc_on_art = (SELECT COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE YEAR(GETDATE()) - CONVERT(INT,  hhm.hhm_year_of_birth) < 18
                    AND hhm.hhm_year_of_birth <> '-1' AND hhm.hhm_year_of_birth <> '*'
                    AND hvm.hst_id = '1' AND hvm.ynna_id_hhp_art = '1' AND hhv.hhv_date BETWEEN '{0}' AND '{1}')

                    SET @ovc_on_art_adhering = (SELECT COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE YEAR(GETDATE()) - CONVERT(INT,  hhm.hhm_year_of_birth) < 18
                    AND hhm.hhm_year_of_birth <> '-1' AND hhm.hhm_year_of_birth <> '*'
                    AND hvm.hst_id = '1' AND hvm.ynna_id_hhp_art = '1' AND ynna_id_hhp_adhering = '1' AND hhv.hhv_date BETWEEN '{0}' AND '{1}')

                    SET @ovc_not_on_art = (SELECT COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE YEAR(GETDATE()) - CONVERT(INT,  hhm.hhm_year_of_birth) < 18
                    AND hhm.hhm_year_of_birth <> '-1' AND hhm.hhm_year_of_birth <> '*'
                    AND hvm.hst_id = '1' AND hvm.ynna_id_hhp_art = '0' AND hvm.ynna_id_hhp_art <> '-1' AND hvm.ynna_id_hhp_art <> '2'  AND hhv.hhv_date BETWEEN '{0}' AND '{1}')

                    SET @ovc_reported_hiv_neg_to_partner = (SELECT COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE YEAR(GETDATE()) - CONVERT(INT,  hhm.hhm_year_of_birth) < 18
                    AND hhm.hhm_year_of_birth <> '-1' AND hhm.hhm_year_of_birth <> '*'
                    AND hvm.hst_id = '2' AND hhv.hhv_date BETWEEN '{0}' AND '{1}')

                    SET @ovc_reported_hiv_pos_to_partner = (SELECT COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE YEAR(GETDATE()) - CONVERT(INT,  hhm.hhm_year_of_birth) < 18
                    AND hhm.hhm_year_of_birth <> '-1' AND hhm.hhm_year_of_birth <> '*'
                    AND hvm.hst_id = '1' AND(hvm.ynna_id_hhp_art = '0' OR hvm.ynna_id_hhp_art = '1') AND hhv.hhv_date BETWEEN '{0}' AND '{1}')

                    SET @ovc_no_reported_hiv_status_to_partner = (SELECT COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE YEAR(GETDATE()) - CONVERT(INT,  hhm.hhm_year_of_birth) < 18
                    AND hhm.hhm_year_of_birth <> '-1' AND hhm.hhm_year_of_birth <> '*'
                    AND (hvm.hst_id = '3' OR hvm.hst_id = '4' OR hvm.hst_id = '-1') AND hhv.hhv_date BETWEEN '{0}' AND '{1}')

                    SET @ovc_report_hiv_status = (@ovc_reported_hiv_neg_to_partner + @ovc_reported_hiv_pos_to_partner)

                    SET @total_ovc = (@ovc_report_hiv_status + @ovc_no_reported_hiv_status_to_partner)

                    IF @total_ovc <> 0
                    BEGIN
                    SET @ovc_report_hiv_status_percent = (@ovc_report_hiv_status * 100) / @total_ovc
                    END


                     INSERT INTO @tempTable(total_ovc,ovc_report_hiv_status,ovc_report_hiv_status_percent,ovc_on_art,ovc_on_art_adhering,ovc_not_on_art,ovc_reported_hiv_neg_to_partner,ovc_no_reported_hiv_status_to_partner,ovc_reported_hiv_pos_to_partner)
                     VALUES(@total_ovc,@ovc_report_hiv_status,@ovc_report_hiv_status_percent,@ovc_on_art,@ovc_on_art_adhering,@ovc_not_on_art,@ovc_reported_hiv_neg_to_partner,@ovc_no_reported_hiv_status_to_partner,@ovc_reported_hiv_pos_to_partner)

                     SELECT total_ovc,ovc_report_hiv_status, ISNULL(ovc_report_hiv_status_percent, 0) AS ovc_report_hiv_status_percent,ovc_on_art,ovc_on_art_adhering,ovc_not_on_art,ovc_reported_hiv_neg_to_partner,ovc_no_reported_hiv_status_to_partner,ovc_reported_hiv_pos_to_partner
                      FROM @tempTable
                    END";

                SQL = string.Format(SQL, qm_date_begin, qm_date_end);
          #endregion Query

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

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dtRow = dt.Rows[0];

                        //total_ovc = Convert.ToInt32(dtRow["total_ovc"].ToString()).ToString();
                        //ovc_report_hiv_status = Convert.ToInt32(dtRow["ovc_report_hiv_status"].ToString()).ToString();
                        //ovc_report_hiv_status_percent = Convert.ToInt32(dtRow["ovc_report_hiv_status_percent"].ToString()).ToString();

                        //ovc_on_art = Convert.ToInt32(dtRow["ovc_on_art"].ToString()).ToString();
                        //ovc_on_art_adhering = Convert.ToInt32(dtRow["ovc_on_art_adhering"].ToString()).ToString();
                        //ovc_not_on_art = Convert.ToInt32(dtRow["ovc_not_on_art"].ToString()).ToString();
                        //ovc_reported_hiv_neg_to_partner = Convert.ToInt32(dtRow["ovc_reported_hiv_neg_to_partner"].ToString()).ToString();
                        //ovc_reported_hiv_pos_to_partner = Convert.ToInt32(dtRow["ovc_reported_hiv_pos_to_partner"].ToString()).ToString();
                        //ovc_no_reported_hiv_status_to_partner = Convert.ToInt32(dtRow["ovc_no_reported_hiv_status_to_partner"].ToString()).ToString();
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
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }

            return dt;
        }

        public static DataTable ReturnCPA_Stats()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
                #region Query
                SQL = @"DECLARE @total_eco_strengthening int
                    DECLARE @total_education_support int
                    DECLARE @total_food_security int
                    DECLARE @total_health_hiv_prevention int
                    DECLARE @total_protection int
                    DECLARE @total_pyschosocial_support int

                    DECLARE @total_other_services int
                    DECLARE @datefrom date = '2018-04-01'
                    DECLARE @dateTo date = '2018-06-30'

                    DECLARE @tempTable TABLE (total_eco_strengthening int,total_education_support int,total_food_security int,total_health_hiv_prevention int,total_protection int,total_pyschosocial_support int)

                    BEGIN
                    SET @total_eco_strengthening = (
                    SELECT  COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE (yn_id_es_silc = 1 OR yn_id_es_apprenticeship = 1 OR yn_id_es_agro = 1 OR yn_id_es_aflateen = 1 OR yn_id_es_caregiver_group = 1)
                    AND hhv.hhv_date BETWEEN '{0}' AND '{1}'
                    )

                    SET @total_food_security = (
                    SELECT  COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE (yn_id_fsn_nutrition = 1 OR ynna_id_fsn_support = 1 OR yn_id_fsn_referred = 1 OR yn_id_fsn_wash = 1 )
                    AND hhv.hhv_date BETWEEN '{0}' AND '{1}'
                    )

                    SET @total_health_hiv_prevention = (
                    SELECT  COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE (hvm.hst_id = 1 OR hvm.hst_id = 2 OR ynna_id_hhp_art = 1 OR ynna_id_hhp_adhering = 1 OR ynna_id_hhp_referred = 1)
                    AND hhv.hhv_date BETWEEN '{0}' AND '{1}'
                    )

                    SET @total_protection = (
                    SELECT  COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE (ynna_id_pro_birth_certificate = 1 OR ynna_id_pro_birth_registration = 1 OR ynna_id_pro_child_abuse = 1 OR ynna_id_pro_child_labour = 1 OR ynna_id_pro_reintegrated = 1)
                    AND hhv.hhv_date BETWEEN '{0}' AND '{1}'
                    )

                    SET @total_education_support = (
                    SELECT  COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE ynna_id_edu_support = 1 
                    AND hhv.hhv_date BETWEEN '{0}' AND '{1}')


                    SET @total_pyschosocial_support = (
                    SELECT  COUNT(DISTINCT hvm.hhm_id) FROM hh_household_home_visit_member hvm
                    LEFT JOIN hh_household_member hhm ON hvm.hhm_id = hhm.hhm_id
                    LEFT JOIN hh_household_home_visit hhv ON hvm.hhv_id = hhv.hhv_id
                    WHERE (ynna_id_ps_parenting = 1 OR ynna_id_ps_support = 1 OR ynna_id_ps_violence = 1) AND hhv.hhv_date BETWEEN '{0}' AND '{1}'
                    
                    )
                     INSERT INTO @tempTable(total_eco_strengthening,total_education_support,total_food_security,total_health_hiv_prevention,total_protection,total_pyschosocial_support)
                     VALUES(@total_eco_strengthening,@total_education_support,@total_food_security,@total_health_hiv_prevention,@total_protection,@total_pyschosocial_support)

                     SELECT* FROM @tempTable
                    END";

                SQL = string.Format(SQL, qm_date_begin, qm_date_end);
                #endregion Query

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

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dtRow = dt.Rows[0];

                        total_eco_strengthening = Convert.ToInt32(dtRow["total_eco_strengthening"].ToString()).ToString();
                        total_education_support = Convert.ToInt32(dtRow["total_education_support"].ToString()).ToString();
                        total_food_security = Convert.ToInt32(dtRow["total_food_security"].ToString()).ToString();
                        total_health_hiv_prevention = Convert.ToInt32(dtRow["total_health_hiv_prevention"].ToString()).ToString();
                        total_protection = Convert.ToInt32(dtRow["total_protection"].ToString()).ToString();
                        total_pyschosocial_support = Convert.ToInt32(dtRow["total_pyschosocial_support"].ToString()).ToString();
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
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }

            return dt;
        }

        public static DataTable ReturnQuarters()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
                #region Query
                SQL = @"SELECT R.qm_name,R.qm_id FROM lst_quarter_range R";

                #endregion Query

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

        public static DataTable ReturnQuarterDates(string qm_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
                #region Query
                SQL = @"SELECT R.qm_date_begin,R.qm_date_end FROM lst_quarter_range R
                        WHERE R.qm_id = '{0}'";

                SQL = string.Format(SQL, qm_id);
                #endregion Query

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
    }
}
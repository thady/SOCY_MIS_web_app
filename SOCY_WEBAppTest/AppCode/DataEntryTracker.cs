using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;

namespace SOCY_WEBAppTest.AppCode
{
    public static class DataEntryTracker
    {
        #region DBConnection
        static SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());
        #endregion DBConnection

        #region Variables
        public static string prt_id = string.Empty;
        public static string dst_id = string.Empty;
        public static string usr_id = string.Empty;
        public static string file_name = string.Empty;
        public static byte[] file_content = new byte[0];
        public static string tracker_date_from = string.Empty;
        public static string tracker_date_to = string.Empty;
        public static string tracker_id = string.Empty;

        public static string usr_id_create = string.Empty;
        public static DateTime tracker_date = DateTime.Now;
        public static int t_ipt_total_received = 0;
        public static int t_ipt_total_entered = 0;
        public static int t_hat_total_received = 0;
        public static int t_hat_total_entered = 0;
        public static int t_hip_total_received = 0;
        public static int t_hip_total_entered = 0;
        public static int t_hv_total_received = 0;
        public static int t_hv_total_entered = 0;
        public static int t_rat_total_received = 0;
        public static int t_rat_total_entered = 0;
        public static int t_linkage_total_received = 0;
        public static int t_linkage_total_entered = 0;
        public static int t_refferal_total_received = 0;
        public static int t_refferal_total_entered = 0;
        public static int t_edu_subsidy_total_received = 0;
        public static int t_edu_subsidy_total_entered = 0;
        public static int t_cTraining_SILC_total_received = 0;
        public static int t_cTraining_SILC_total_entered = 0;
        public static int t_cTraining_youth_total_received = 0;
        public static int t_cTraining_youth_total_entered = 0;
        public static int t_youth_savings_total_received = 0;
        public static int t_youth_savings_total_entered = 0;
        public static int t_appre_progress_total_received = 0;
        public static int t_appre_progress_total_entered = 0;
        public static int t_training_inventory_total_received = 0;
        public static int t_training_inventory_total_entered = 0;
        public static int t_appre_skill_aqui_total_received = 0;
        public static int t_appre_skill_aqui_total_entered = 0;
        public static int t_training_comple_total_received = 0;
        public static int t_training_comple_total_entered = 0;
        public static int t_youth_ass_toolkit_total_received = 0;
        public static int t_youth_ass_toolkit_total_entered = 0;
        public static int t_youth_tracer_total_received = 0;
        public static int t_youth_tracer_total_entered = 0;
        public static int t_dovcc_total_received = 0;
        public static int t_dovcc_total_entered = 0;
        public static int t_sovcc_total_received = 0;
        public static int t_sovcc_total_entered = 0;
        public static int t_cbsd_resource_alloc_total_received = 0;
        public static int t_cbsd_resource_alloc_total_entered = 0;
        public static int t_cbsd_staff_appra_total_received = 0;
        public static int t_cbsd_staff_appra_total_entered = 0;
        public static int t_reintergration_total_received = 0;
        public static int t_reintergration_total_entered = 0;
        public static int t_dreams_registration_total_received = 0;
        public static int t_dreams_registration_total_entered = 0;
        public static int t_hct_total_received = 0;
        public static int t_hct_total_entered = 0;
        public static int t_dreams_partner_total_received = 0;
        public static int t_dreams_partner_total_entered = 0;
        public static int t_sunovuyo_total_received = 0;
        public static int t_sunovuyo_total_entered = 0;
        public static int t_dreams_stepping_stones_total_received = 0;
        public static int t_dreams_stepping_stones_total_entered = 0;

        public static int t_dreams_screening_total_received = 0;
        public static int t_dreams_screening_total_entered = 0;
        public static int t_dreams_sasa_total_received = 0;
        public static int t_dreams_sasa_total_entered = 0;
        public static int t_viral_load_total_received = 0;
        public static int t_viral_load_total_entered = 0;

        public static string t_comments = string.Empty;

        public static string t_ipt_percent_entered = string.Empty;
        public static string t_hat_percent_entered = string.Empty;
        public static string t_hip_percent_entered = string.Empty;
        public static string t_hv_percent_entered = string.Empty;
        public static string t_rat_percent_entered = string.Empty;
        public static string t_linkage_percent_entered = string.Empty;
        public static string t_refferal_percent_entered = string.Empty;
        public static string t_edu_subsidy_percent_entered = string.Empty;
        public static string t_cTraining_SILC_percent_entered = string.Empty;
        public static string t_cTraining_youth_percent_entered = string.Empty;
        public static string t_youth_savings_percent_entered = string.Empty;
        public static string t_appre_progress_percent_entered = string.Empty;
        public static string t_training_inventory_percent_entered = string.Empty;
        public static string t_appre_skill_aqui_percent_entered = string.Empty;
        public static string t_training_comple_percent_entered = string.Empty;
        public static string t_youth_ass_toolkit_percent_entered = string.Empty;
        public static string t_youth_tracer_percent_entered = string.Empty;
        public static string t_dovcc_percent_entered = string.Empty;
        public static string t_sovcc_percent_entered = string.Empty;
        public static string t_cbsd_resource_alloc_percent_entered = string.Empty;
        public static string t_cbsd_staff_appra_percent_entered = string.Empty;
        public static string t_reintergration_percent_entered = string.Empty;
        public static string t_dreams_registration_percent_entered = string.Empty;
        public static string t_hct_percent_entered = string.Empty;
        public static string t_dreams_partner_percent_entered = string.Empty;
        public static string t_sunovuyo_percent_entered = string.Empty;
        public static string t_dreams_stepping_stones_percent_entered = string.Empty;

        public static string t_dreams_screening_percent_entered = string.Empty;
        public static string t_dreams_sasa_percent_entered = string.Empty;
        public static string t_viral_load_percent_entered = string.Empty;

        #region Received_previous
        public static int _t_ipt_total_received = 0;
        public static int _t_hat_total_received = 0;
        public static int _t_hip_total_received = 0;
        public static int _t_hv_total_received = 0;
        public static int _t_rat_total_received = 0;
        public static int _t_linkage_total_received = 0;
        public static int _t_refferal_total_received = 0;
        public static int _t_edu_subsidy_total_received = 0;
        public static int _t_cTraining_SILC_total_received = 0;
        public static int _t_cTraining_youth_total_received = 0;
        public static int _t_youth_savings_total_received = 0;
        public static int _t_appre_progress_total_received = 0;
        public static int _t_training_inventory_total_received = 0;
        public static int _t_appre_skill_aqui_total_received = 0;
        public static int _t_training_comple_total_received = 0;
        public static int _t_youth_ass_toolkit_total_received = 0;
        public static int _t_youth_tracer_total_received = 0;
        public static int _t_dovcc_total_received = 0;
        public static int _t_sovcc_total_received = 0;
        public static int _t_cbsd_resource_alloc_total_received = 0;
        public static int _t_cbsd_staff_appra_total_received = 0;
        public static int _t_reintergration_total_received = 0;
        public static int _t_dreams_registration_total_received = 0;
        public static int _t_hct_total_received = 0;
        public static int _t_dreams_partner_total_received = 0;
        public static int _t_sunovuyo_total_received = 0;
        public static int _t_dreams_stepping_stones_total_received = 0;

        #endregion


        #region tools_entered_in_socymis
        public static int _t_ipt_total_entered = 0;
        public static int _t_hat_total_entered = 0;
        public static int _t_hip_total_entered = 0;
        public static int _t_hv_total_entered = 0;
        public static int _t_rat_total_entered = 0;
        public static int _t_linkage_total_entered = 0;
        public static int _t_refferal_total_entered = 0;
        public static int _t_edu_subsidy_total_entered = 0;
        public static int _t_cTraining_SILC_total_entered = 0;
        public static int _t_cTraining_youth_total_entered = 0;
        public static int _t_youth_savings_total_entered = 0;
        public static int _t_appre_progress_total_entered = 0;
        public static int _t_training_inventory_total_entered = 0;
        public static int _t_appre_skill_aqui_total_entered = 0;
        public static int _t_training_comple_total_entered = 0;
        public static int _t_youth_ass_toolkit_total_entered = 0;
        public static int _t_youth_tracer_total_entered = 0;
        public static int _t_dovcc_total_entered = 0;
        public static int _t_sovcc_total_entered = 0;
        public static int _t_cbsd_resource_alloc_total_entered = 0;
        public static int _t_cbsd_staff_appra_total_entered = 0;
        public static int _t_reintergration_total_entered = 0;
        public static int _t_dreams_registration_total_entered = 0;
        public static int _t_hct_total_entered = 0;
        public static int _t_dreams_partner_total_entered = 0;
        public static int _t_sunovuyo_total_entered = 0;
        public static int _t_dreams_stepping_stones_total_entered = 0;

        public static int _t_daily_ipt_total_received = 0;
        public static int _t_daily_hat_total_receivedd = 0;
        public static int _t_daily_hip_total_received = 0;
        public static int _t_daily_hv_total_receivedd = 0;
        public static int _t_daily_rat_total_received = 0;
        public static int _t_daily_linkage_total_received = 0;
        public static int _t_daily_refferal_total_received = 0;
        public static int _t_daily_edu_subsidy_total_received = 0;
        public static int _t_daily_cTraining_SILC_total_received = 0;
        public static int _t_daily_cTraining_youth_total_received = 0;
        public static int _t_daily_youth_savings_total_received = 0;
        public static int _t_daily_appre_progress_total_received = 0;
        public static int _t_daily_training_inventory_total_received = 0;
        public static int _t_daily_appre_skill_aqui_total_received = 0;
        public static int _t_daily_training_comple_total_received = 0;
        public static int _t_daily_youth_ass_toolkit_total_received = 0;
        public static int _t_daily_youth_tracer_total_received = 0;
        public static int _t_daily_dovcc_total_received = 0;
        public static int _t_daily_sovcc_total_received = 0;
        public static int _t_daily_cbsd_resource_alloc_total_received = 0;
        public static int _t_daily_cbsd_staff_appra_total_received = 0;
        public static int _t_daily_reintergration_total_received = 0;
        public static int _t_daily_dreams_registration_total_received = 0;
        public static int _t_daily_hct_total_received = 0;
        public static int _t_daily_dreams_partner_total_received = 0;
        public static int _t_daily_sunovuyo_total_received = 0;
        public static int _t_daily_dreams_stepping_stones_total_received = 0;
        #endregion

        #endregion Variables


        public static DataTable ReturnDataEntryTrackerList(string usr_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
                if (usr_id.Equals("1022b6b0-a0f9-463d-bf10-cf451ac2d119") || usr_id.Equals("379a52fd-9e69-4bb5-b533-899c2e327690") || usr_id.Equals("7479ea58-a2d1-4d06-986f-4a25f26d5979")
                    || usr_id.Equals("a73f025d-2232-45d3-a48a-6787920fe2db") || usr_id.Equals("aec74cc2-96f2-4281-b815-2361feda9cbb") || usr_id.Equals("e6ce68fa-b02b-48a1-b841-ef4f7c7862a4") || usr_id.Equals("ef4e471a-a058-478d-86e1-302ba34ac856")
                    || usr_id.Equals("1") || usr_id.Equals("7a6ceaa5-b4fb-4d05-b885-d79c2e4c674e"))
                {
                    SQL = @"SELECT prt.prt_name,dst.dst_name,'Data Entry Tracker' AS dte_tracker,um.usr_first_name + ' ' + usr_last_name AS username,
                        T.tracker_date_from AS tracker_date_from,T.tracker_date_to AS tracker_date_to,T.date_uploaded AS date_uploaded,T.tracker_id,T.usr_id
                        FROM um_weekly_data_entry_tracker T
                        LEFT JOIN lst_partner prt ON prt.prt_id = T.prt_id
                        LEFT JOIN lst_district dst ON dst.dst_id = T.dst_id
                        LEFT JOIN um_user um ON um.usr_id = T.usr_id
                        ORDER BY CAST(date_uploaded AS DATETIME) DESC";
                }
                else
                {
                    SQL = @"SELECT prt.prt_name,dst.dst_name,'Data Entry Tracker' AS dte_tracker,um.usr_first_name + ' ' + usr_last_name AS username,
                        T.tracker_date_from AS tracker_date_from,T.tracker_date_to AS tracker_date_to,T.date_uploaded AS date_uploaded,T.tracker_id,T.usr_id
                        FROM um_weekly_data_entry_tracker T
                        LEFT JOIN lst_partner prt ON prt.prt_id = T.prt_id
                        LEFT JOIN lst_district dst ON dst.dst_id = T.dst_id
                        LEFT JOIN um_user um ON um.usr_id = T.usr_id
                        WHERE T.usr_id = '{0}'
                        ORDER BY CAST(date_uploaded AS DATETIME) DESC";
                    SQL = string.Format(SQL, usr_id);
                }

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

        public static DataTable ReturnFileDetails(string tracker_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT T.*,R.rgn_id FROM um_weekly_data_entry_tracker T
                    LEFT JOIN lst_district dst ON T.dst_id = dst.dst_id
                    LEFT JOIN lst_region R ON dst.rgn_id = R.rgn_id
                    WHERE tracker_id = '{0}'";

                SQL = string.Format(SQL, tracker_id);

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

        public static DataTable ReturnDistrictList()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT dst_id,dst_name FROM lst_district WHERE dst_id <> '6' AND dst_id <> '8' ";

                SQL = string.Format(SQL, tracker_id);

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

        public static int ValidateDuplicate()
        {
            int count = 0;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT COUNT(tracker_id) FROM um_weekly_data_entry_tracker
                        WHERE tracker_date_from = '{0}'
                        AND tracker_date_to = '{1}'
                        AND prt_id = '{2}'
                        AND dst_id = '{3}'";

                SQL = string.Format(SQL, tracker_date_from, tracker_date_to, prt_id, dst_id);

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    count = (int)cmd.ExecuteScalar();

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

            return count;
        }

        public static int Check_if_tracker_exists(string dst_id,DateTime tracker_date)
        {
            string SQL = string.Empty;
            int x = 0;
            try
            {

                SQL = @"SELECT COUNT(*) FROM um_weekly_dst_data_entry_tracker WHERE dst_id = '{0}' AND tracker_date = '{1}'";

                SQL = string.Format(SQL, dst_id,tracker_date);

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    x = Convert.ToInt32(cmd.ExecuteScalar());

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

            return x;
        }
        public static string save_data_entry_tracker(string tracker_id)
        {

            string strSQL = string.Empty;
            string InsertResult = string.Empty;
            try
            {
                if (tracker_id == string.Empty)
                {
                    #region Save
                    strSQL = @"INSERT INTO [dbo].[um_weekly_dst_data_entry_tracker]
                       ([dst_id] ,[usr_id_create],[tracker_date],[t_ipt_total_received],[t_ipt_total_entered],[t_ipt_percent_entered],[t_hat_total_received]
                       ,[t_hat_total_entered],[t_hat_percent_entered],[t_hip_total_received],[t_hip_total_entered],[t_hip_percent_entered],[t_hv_total_received],[t_hv_total_entered],[t_hv_percent_entered]
                       ,[t_rat_total_received] ,[t_rat_total_entered],[t_rat_percent_entered],[t_linkage_total_received],[t_linkage_total_entered],[t_linkage_percent_entered],[t_refferal_total_received]
                       ,[t_refferal_total_entered],[t_refferal_percent_entered] ,[t_edu_subsidy_total_received],[t_edu_subsidy_total_entered],[t_edu_subsidy_percent_entered],[t_cTraining_SILC_total_received]
                       ,[t_cTraining_SILC_total_entered],[t_cTraining_SILC_percent_entered],[t_cTraining_youth_total_received],[t_cTraining_youth_total_entered],[t_cTraining_youth_percent_entered] ,[t_youth_savings_total_received]
                       ,[t_youth_savings_total_entered],[t_youth_savings_percent_entered],[t_appre_progress_total_received],[t_appre_progress_total_entered],[t_appre_progress_percent_entered] ,[t_training_inventory_total_received]
                       ,[t_training_inventory_total_entered],[t_training_inventory_percent_entered],[t_appre_skill_aqui_total_received],[t_appre_skill_aqui_total_entered],[t_appre_skill_aqui_percent_entered] ,[t_training_comple_total_received]
                       ,[t_training_comple_total_entered] ,[t_training_comple_percent_entered],[t_youth_ass_toolkit_total_received] ,[t_youth_ass_toolkit_total_entered] ,[t_youth_ass_toolkit_percent_entered],[t_youth_tracer_total_received]
                       ,[t_youth_tracer_total_entered],[t_youth_tracer_percent_entered] ,[t_dovcc_total_received],[t_dovcc_total_entered],[t_dovcc_percent_entered],[t_sovcc_total_received],[t_sovcc_total_entered],[t_sovcc_percent_entered]
                       ,[t_cbsd_resource_alloc_total_received],[t_cbsd_resource_alloc_total_entered],[t_cbsd_resource_alloc_percent_entered],[t_cbsd_staff_appra_total_received] ,[t_cbsd_staff_appra_total_entered] ,[t_cbsd_staff_appra_percent_entered]
                       ,[t_reintergration_total_received] ,[t_reintergration_total_entered],[t_reintergration_percent_entered] ,[t_dreams_registration_total_received] ,[t_dreams_registration_total_entered],[t_dreams_registration_percent_entered]
                       ,[t_hct_total_received],[t_hct_total_entered] ,[t_hct_percent_entered] ,[t_dreams_partner_total_received] ,[t_dreams_partner_total_entered],[t_dreams_partner_percent_entered] ,[t_sunovuyo_total_received] ,[t_sunovuyo_total_entered]
                       ,[t_sunovuyo_percent_entered],[t_dreams_stepping_stones_total_received],[t_dreams_stepping_stones_total_entered],[t_dreams_stepping_stones_percent_entered] ,[t_dreams_screening_total_received],[t_dreams_screening_total_entered],
                        [t_dreams_screening_percent_entered] ,[t_dreams_sasa_total_received],[t_dreams_sasa_total_entered],[t_dreams_sasa_percent_entered],[t_viral_load_total_received],[t_viral_load_total_entered],[t_viral_load_percent_entered]
                        ,[t_comments]) OUTPUT INSERTED.tracker_id
                 VALUES
                       ('{0}' ,'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}' ,'{11}','{12}','{13}' ,'{14}','{15}','{16}','{17}' ,'{18}','{19}','{20}','{21}','{22}' ,'{23}' ,'{24}','{25}','{26}' ,'{27}','{28}','{29}' ,'{30}','{31}' ,'{32}'
                       ,'{33}' ,'{34}' ,'{35}' ,'{36}','{37}','{38}' ,'{39}','{40}','{41}','{42}','{43}','{44}' ,'{45}' ,'{46}','{47}','{48}' ,'{49}','{50}' ,'{51}','{52}','{53}' ,'{54}' ,'{55}','{56}' ,'{57}' ,'{58}','{59}' ,'{60}' ,'{61}' ,'{62}' ,'{63}' ,'{64}'
                       ,'{65}','{66}' ,'{67}' ,'{68}' ,'{69}' ,'{70}','{71}','{72}' ,'{73}','{74}','{75}','{76}','{77}' ,'{78}' ,'{79}' ,'{80}','{81}','{82}' ,'{83}','{84}','{85}','{86}','{87}','{88}','{89}','{90}','{91}','{92}','{93}')";

                    strSQL = string.Format(strSQL, dst_id, usr_id_create, tracker_date, t_ipt_total_received, t_ipt_total_entered, t_ipt_percent_entered, t_hat_total_received
                           , t_hat_total_entered, t_hat_percent_entered, t_hip_total_received, t_hip_total_entered, t_hip_percent_entered, t_hv_total_received, t_hv_total_entered, t_hv_percent_entered
                           , t_rat_total_received, t_rat_total_entered, t_rat_percent_entered, t_linkage_total_received, t_linkage_total_entered, t_linkage_percent_entered, t_refferal_total_received
                           , t_refferal_total_entered, t_refferal_percent_entered, t_edu_subsidy_total_received, t_edu_subsidy_total_entered, t_edu_subsidy_percent_entered, t_cTraining_SILC_total_received
                           , t_cTraining_SILC_total_entered, t_cTraining_SILC_percent_entered, t_cTraining_youth_total_received, t_cTraining_youth_total_entered, t_cTraining_youth_percent_entered, t_youth_savings_total_received
                           , t_youth_savings_total_entered, t_youth_savings_percent_entered, t_appre_progress_total_received, t_appre_progress_total_entered, t_appre_progress_percent_entered, t_training_inventory_total_received
                           , t_training_inventory_total_entered, t_training_inventory_percent_entered, t_appre_skill_aqui_total_received, t_appre_skill_aqui_total_entered, t_appre_skill_aqui_percent_entered, t_training_comple_total_received
                           , t_training_comple_total_entered, t_training_comple_percent_entered, t_youth_ass_toolkit_total_received, t_youth_ass_toolkit_total_entered, t_youth_ass_toolkit_percent_entered, t_youth_tracer_total_received
                           , t_youth_tracer_total_entered, t_youth_tracer_percent_entered, t_dovcc_total_received, t_dovcc_total_entered, t_dovcc_percent_entered, t_sovcc_total_received, t_sovcc_total_entered, t_sovcc_percent_entered
                           , t_cbsd_resource_alloc_total_received, t_cbsd_resource_alloc_total_entered, t_cbsd_resource_alloc_percent_entered, t_cbsd_staff_appra_total_received, t_cbsd_staff_appra_total_entered, t_cbsd_staff_appra_percent_entered
                           , t_reintergration_total_received, t_reintergration_total_entered, t_reintergration_percent_entered, t_dreams_registration_total_received, t_dreams_registration_total_entered, t_dreams_registration_percent_entered
                           , t_hct_total_received, t_hct_total_entered, t_hct_percent_entered, t_dreams_partner_total_received, t_dreams_partner_total_entered, t_dreams_partner_percent_entered, t_sunovuyo_total_received, t_sunovuyo_total_entered
                           , t_sunovuyo_percent_entered, t_dreams_stepping_stones_total_received, t_dreams_stepping_stones_total_entered, t_dreams_stepping_stones_percent_entered, t_dreams_screening_total_received, t_dreams_screening_total_entered,
                           t_dreams_screening_percent_entered, t_dreams_sasa_total_received, t_dreams_sasa_total_entered, t_dreams_sasa_percent_entered, t_viral_load_total_received,t_viral_load_total_entered, t_viral_load_percent_entered, t_comments);
                    #endregion Save

                }
                else
                {
                    #region update
                    strSQL = @"UPDATE [dbo].[um_weekly_dst_data_entry_tracker]
                           SET [dst_id] = '{1}'
                              ,[usr_id_create] = '{2}'
                              ,[tracker_date] = '{3}'
                              ,[t_ipt_total_received] = '{4}'
                              ,[t_ipt_total_entered] = '{5}'
                              ,[t_ipt_percent_entered] = '{6}'
                              ,[t_hat_total_received] = '{7}'
                              ,[t_hat_total_entered] = '{8}'
                              ,[t_hat_percent_entered] = '{9}'
                              ,[t_hip_total_received] = '{10}'
                              ,[t_hip_total_entered] = '{11}'
                              ,[t_hip_percent_entered] = '{12}'
                              ,[t_hv_total_received] = '{13}'
                              ,[t_hv_total_entered] = '{14}'
                               ,[t_hv_percent_entered] = '{15}'
                              ,[t_rat_total_received] = '{16}'
                              ,[t_rat_total_entered] = '{17}'
                              ,[t_rat_percent_entered] = '{18}'
                              ,[t_linkage_total_received] = '{19}'
                              ,[t_linkage_total_entered] = '{20}'
                              ,[t_linkage_percent_entered] = '{21}'
                              ,[t_refferal_total_received] = '{22}'
                              ,[t_refferal_total_entered] = '{23}'
                              ,[t_refferal_percent_entered] = '{24}'
                              ,[t_edu_subsidy_total_received] = '{25}'
                              ,[t_edu_subsidy_total_entered] = '{26}'
                              ,[t_edu_subsidy_percent_entered] = '{27}'
                              ,[t_cTraining_SILC_total_received] = '{28}'
                              ,[t_cTraining_SILC_total_entered] = '{29}'
                              ,[t_cTraining_SILC_percent_entered] = '{30}'
                              ,[t_cTraining_youth_total_received] = '{31}'
                              ,[t_cTraining_youth_total_entered] = '{32}'
                              ,[t_cTraining_youth_percent_entered] = '{33}'
                              ,[t_youth_savings_total_received] = '{34}'
                              ,[t_youth_savings_total_entered] = '{35}'
                              ,[t_youth_savings_percent_entered] = '{36}'
                              ,[t_appre_progress_total_received] = '{37}'
                              ,[t_appre_progress_total_entered] = '{38}'
                              ,[t_appre_progress_percent_entered] = '{39}'
                              ,[t_training_inventory_total_received] = '{40}'
                              ,[t_training_inventory_total_entered] = '{41}'
                              ,[t_training_inventory_percent_entered] = '{42}'
                              ,[t_appre_skill_aqui_total_received] = '{43}'
                              ,[t_appre_skill_aqui_total_entered] = '{44}'
                              ,[t_appre_skill_aqui_percent_entered] = '{45}'
                              ,[t_training_comple_total_received] = '{46}'
                              ,[t_training_comple_total_entered] = '{47}'
                              ,[t_training_comple_percent_entered] = '{48}'
                              ,[t_youth_ass_toolkit_total_received] = '{49}'
                              ,[t_youth_ass_toolkit_total_entered] = '{50}'
                              ,[t_youth_ass_toolkit_percent_entered] = '{51}'
                              ,[t_youth_tracer_total_received] = '{52}'
                              ,[t_youth_tracer_total_entered] = '{53}'
                              ,[t_youth_tracer_percent_entered] = '{54}'
                              ,[t_dovcc_total_received] = '{55}'
                              ,[t_dovcc_total_entered] = '{56}'
                              ,[t_dovcc_percent_entered] = '{57}'
                              ,[t_sovcc_total_received] = '{58}'
                              ,[t_sovcc_total_entered] = '{59}'
                              ,[t_sovcc_percent_entered] = '{60}'
                              ,[t_cbsd_resource_alloc_total_received] = '{61}'
                              ,[t_cbsd_resource_alloc_total_entered] = '{62}'
                              ,[t_cbsd_resource_alloc_percent_entered] = '{63}'
                              ,[t_cbsd_staff_appra_total_received] = '{64}'
                              ,[t_cbsd_staff_appra_total_entered] = '{65}'
                              ,[t_cbsd_staff_appra_percent_entered] = '{66}'
                              ,[t_reintergration_total_received] = '{67}'
                              ,[t_reintergration_total_entered] = '{68}'
                              ,[t_reintergration_percent_entered] = '{69}'
                              ,[t_dreams_registration_total_received] = '{70}'
                              ,[t_dreams_registration_total_entered] = '{71}'
                              ,[t_dreams_registration_percent_entered] = '{72}'
                              ,[t_hct_total_received] = '{73}'
                              ,[t_hct_total_entered] = '{74}'
                              ,[t_hct_percent_entered] = '{75}'
                              ,[t_dreams_partner_total_received] = '{76}'
                              ,[t_dreams_partner_total_entered] = '{77}'
                              ,[t_dreams_partner_percent_entered] = '{78}'
                              ,[t_sunovuyo_total_received] = '{79}'
                              ,[t_sunovuyo_total_entered] = '{80}'
                              ,[t_sunovuyo_percent_entered] = '{81}'
                              ,[t_dreams_stepping_stones_total_received] = '{82}'
                              ,[t_dreams_stepping_stones_total_entered] = '{83}'
                              ,[t_dreams_stepping_stones_percent_entered] = '{84}'
                              ,[t_dreams_screening_total_received] = '{85}'
                              ,[t_dreams_screening_total_entered] = '{86}'
                              ,[t_dreams_screening_percent_entered] = '{87}'
                              ,[t_dreams_sasa_total_received] = '{88}'
                              ,[t_dreams_sasa_total_entered] = '{89}'
                              ,[t_dreams_sasa_percent_entered] = '{90}'
                              ,[t_viral_load_total_received] = '{91}'
                              ,[t_viral_load_total_entered] = '{92}'
                              ,[t_viral_load_percent_entered] = '{93}'
                              ,[t_comments] = '{94}'
                         WHERE  [tracker_id] = '{0}'";

                    strSQL = string.Format(strSQL, tracker_id, dst_id, usr_id_create, tracker_date, t_ipt_total_received, t_ipt_total_entered, t_ipt_percent_entered, t_hat_total_received
                           , t_hat_total_entered, t_hat_percent_entered, t_hip_total_received, t_hip_total_entered, t_hip_percent_entered, t_hv_total_received, t_hv_total_entered, t_hv_percent_entered
                           , t_rat_total_received, t_rat_total_entered, t_rat_percent_entered, t_linkage_total_received, t_linkage_total_entered, t_linkage_percent_entered, t_refferal_total_received
                           , t_refferal_total_entered, t_refferal_percent_entered, t_edu_subsidy_total_received, t_edu_subsidy_total_entered, t_edu_subsidy_percent_entered, t_cTraining_SILC_total_received
                           , t_cTraining_SILC_total_entered, t_cTraining_SILC_percent_entered, t_cTraining_youth_total_received, t_cTraining_youth_total_entered, t_cTraining_youth_percent_entered, t_youth_savings_total_received
                           , t_youth_savings_total_entered, t_youth_savings_percent_entered, t_appre_progress_total_received, t_appre_progress_total_entered, t_appre_progress_percent_entered, t_training_inventory_total_received
                           , t_training_inventory_total_entered, t_training_inventory_percent_entered, t_appre_skill_aqui_total_received, t_appre_skill_aqui_total_entered, t_appre_skill_aqui_percent_entered, t_training_comple_total_received
                           , t_training_comple_total_entered, t_training_comple_percent_entered, t_youth_ass_toolkit_total_received, t_youth_ass_toolkit_total_entered, t_youth_ass_toolkit_percent_entered, t_youth_tracer_total_received
                           , t_youth_tracer_total_entered, t_youth_tracer_percent_entered, t_dovcc_total_received, t_dovcc_total_entered, t_dovcc_percent_entered, t_sovcc_total_received, t_sovcc_total_entered, t_sovcc_percent_entered
                           , t_cbsd_resource_alloc_total_received, t_cbsd_resource_alloc_total_entered, t_cbsd_resource_alloc_percent_entered, t_cbsd_staff_appra_total_received, t_cbsd_staff_appra_total_entered, t_cbsd_staff_appra_percent_entered
                           , t_reintergration_total_received, t_reintergration_total_entered, t_reintergration_percent_entered, t_dreams_registration_total_received, t_dreams_registration_total_entered, t_dreams_registration_percent_entered
                           , t_hct_total_received, t_hct_total_entered, t_hct_percent_entered, t_dreams_partner_total_received, t_dreams_partner_total_entered, t_dreams_partner_percent_entered, t_sunovuyo_total_received, t_sunovuyo_total_entered
                           , t_sunovuyo_percent_entered, t_dreams_stepping_stones_total_received, t_dreams_stepping_stones_total_entered, t_dreams_stepping_stones_percent_entered, t_dreams_screening_total_received, t_dreams_screening_total_entered,
                           t_dreams_screening_percent_entered, t_dreams_sasa_total_received, t_dreams_sasa_total_entered, t_dreams_sasa_percent_entered, t_viral_load_total_received, t_viral_load_total_entered, t_viral_load_percent_entered, t_comments);
                    #endregion update

                }
                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    InsertResult = (String)cmd.ExecuteScalar();
                    //InsertResult = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd.Parameters.Clear();

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
            return InsertResult;
        }

        public static void Return_previous_tracker_total_tools_received(string dst_id,string tracker_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {
                if (tracker_id == string.Empty)
                {
                    SQL = @"SELECT [t_ipt_total_received],[t_hat_total_received],[t_hip_total_received],[t_hv_total_received],[t_rat_total_received],
                        [t_linkage_total_received],[t_refferal_total_received],[t_edu_subsidy_total_received],[t_cTraining_SILC_total_received],
                        [t_cTraining_youth_total_received],[t_youth_savings_total_received],[t_appre_progress_total_received],[t_training_inventory_total_received],
                        [t_appre_skill_aqui_total_received],[t_training_comple_total_received],[t_youth_ass_toolkit_total_received] ,[t_youth_tracer_total_received],
                        [t_dovcc_total_received],[t_sovcc_total_received],[t_cbsd_resource_alloc_total_received],[t_cbsd_staff_appra_total_received]
                        ,[t_reintergration_total_received],[t_dreams_registration_total_received] ,[t_hct_total_received],[t_dreams_partner_total_received] 
                        ,[t_sunovuyo_total_received] ,[t_dreams_stepping_stones_total_received]  FROM um_weekly_dst_data_entry_tracker WHERE dst_id = '{0}' AND tracker_sid = (SELECT MAX(tracker_sid) FROM um_weekly_dst_data_entry_tracker)";
                }
                else
                {
                    SQL = @"SELECT [t_ipt_total_received],[t_hat_total_received],[t_hip_total_received],[t_hv_total_received],[t_rat_total_received],
                        [t_linkage_total_received],[t_refferal_total_received],[t_edu_subsidy_total_received],[t_cTraining_SILC_total_received],
                        [t_cTraining_youth_total_received],[t_youth_savings_total_received],[t_appre_progress_total_received],[t_training_inventory_total_received],
                        [t_appre_skill_aqui_total_received],[t_training_comple_total_received],[t_youth_ass_toolkit_total_received] ,[t_youth_tracer_total_received],
                        [t_dovcc_total_received],[t_sovcc_total_received],[t_cbsd_resource_alloc_total_received],[t_cbsd_staff_appra_total_received]
                        ,[t_reintergration_total_received],[t_dreams_registration_total_received] ,[t_hct_total_received],[t_dreams_partner_total_received] 
                        ,[t_sunovuyo_total_received] ,[t_dreams_stepping_stones_total_received]  FROM um_weekly_dst_data_entry_tracker WHERE dst_id = '{0}' AND tracker_sid = (SELECT MAX(tracker_sid) FROM um_weekly_dst_data_entry_tracker WHERE tracker_sid < (SELECT MAX(tracker_sid) FROM um_weekly_dst_data_entry_tracker))";
                }
                

                SQL = string.Format(SQL, dst_id);

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

                        _t_ipt_total_received = Convert.ToInt32(dtRow["t_ipt_total_received"].ToString());
                        _t_hat_total_received = Convert.ToInt32(dtRow["t_hat_total_received"].ToString());
                        _t_hip_total_received = Convert.ToInt32(dtRow["t_hip_total_received"].ToString());
                        _t_hv_total_received = Convert.ToInt32(dtRow["t_hv_total_received"].ToString());
                        _t_rat_total_received = Convert.ToInt32(dtRow["t_rat_total_received"].ToString());
                        _t_linkage_total_received = Convert.ToInt32(dtRow["t_linkage_total_received"].ToString());
                        _t_refferal_total_received = Convert.ToInt32(dtRow["t_refferal_total_received"].ToString());
                        _t_edu_subsidy_total_received = Convert.ToInt32(dtRow["t_edu_subsidy_total_received"].ToString());
                        _t_cTraining_SILC_total_received = Convert.ToInt32(dtRow["t_cTraining_SILC_total_received"].ToString());
                        _t_cTraining_youth_total_received = Convert.ToInt32(dtRow["t_cTraining_youth_total_received"].ToString());
                        _t_youth_savings_total_received = Convert.ToInt32(dtRow["t_youth_savings_total_received"].ToString());
                        _t_appre_progress_total_received = Convert.ToInt32(dtRow["t_appre_progress_total_received"].ToString());
                        _t_training_inventory_total_received = Convert.ToInt32(dtRow["t_training_inventory_total_received"].ToString());
                        _t_appre_skill_aqui_total_received = Convert.ToInt32(dtRow["t_appre_skill_aqui_total_received"].ToString());
                        _t_training_comple_total_received = Convert.ToInt32(dtRow["t_training_comple_total_received"].ToString());
                        _t_youth_ass_toolkit_total_received = Convert.ToInt32(dtRow["t_youth_ass_toolkit_total_received"].ToString());
                        _t_youth_tracer_total_received = Convert.ToInt32(dtRow["t_youth_tracer_total_received"].ToString());
                        _t_dovcc_total_received = Convert.ToInt32(dtRow["t_dovcc_total_received"].ToString());
                        _t_sovcc_total_received = Convert.ToInt32(dtRow["t_sovcc_total_received"].ToString());
                        _t_cbsd_resource_alloc_total_received = Convert.ToInt32(dtRow["t_cbsd_resource_alloc_total_received"].ToString());
                        _t_cbsd_staff_appra_total_received = Convert.ToInt32(dtRow["t_cbsd_staff_appra_total_received"].ToString());
                        _t_reintergration_total_received = Convert.ToInt32(dtRow["t_reintergration_total_received"].ToString());
                        _t_dreams_registration_total_received = Convert.ToInt32(dtRow["t_dreams_registration_total_received"].ToString());
                        _t_hct_total_received = Convert.ToInt32(dtRow["t_hct_total_received"].ToString());
                        _t_dreams_partner_total_received = Convert.ToInt32(dtRow["t_dreams_partner_total_received"].ToString());
                        _t_sunovuyo_total_received = Convert.ToInt32(dtRow["t_sunovuyo_total_received"].ToString());
                        _t_dreams_stepping_stones_total_received = Convert.ToInt32(dtRow["t_dreams_stepping_stones_total_received"].ToString());

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

        }


        public static void Return_Tools_entered(string dst_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT*  FROM um_weekly_data_entracker_tools_entered WHERE dst_id = '{0}'";

                SQL = string.Format(SQL, dst_id);

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

                        _t_ipt_total_entered = Convert.ToInt32(dtRow["t_ipt_total_entered"].ToString());
                        _t_hat_total_entered = Convert.ToInt32(dtRow["t_hat_total_entered"].ToString());
                        _t_hip_total_entered = Convert.ToInt32(dtRow["t_hip_total_entered"].ToString());
                        _t_hv_total_entered = Convert.ToInt32(dtRow["t_hv_total_entered"].ToString());
                        _t_rat_total_entered = Convert.ToInt32(dtRow["t_rat_total_entered"].ToString());
                        _t_linkage_total_entered = Convert.ToInt32(dtRow["t_linkage_total_entered"].ToString());
                        _t_refferal_total_entered = Convert.ToInt32(dtRow["t_refferal_total_entered"].ToString());
                        _t_edu_subsidy_total_entered = Convert.ToInt32(dtRow["t_edu_subsidy_total_entered"].ToString());
                        _t_cTraining_SILC_total_entered = Convert.ToInt32(dtRow["t_cTraining_SILC_total_entered"].ToString());
                        _t_cTraining_youth_total_entered = Convert.ToInt32(dtRow["t_cTraining_youth_total_entered"].ToString());
                        _t_youth_savings_total_entered = Convert.ToInt32(dtRow["t_youth_savings_total_entered"].ToString());
                        _t_appre_progress_total_entered = Convert.ToInt32(dtRow["t_appre_progress_total_entered"].ToString());
                        _t_training_inventory_total_entered = Convert.ToInt32(dtRow["t_training_inventory_total_entered"].ToString());
                        _t_appre_skill_aqui_total_entered = Convert.ToInt32(dtRow["t_appre_skill_aqui_total_entered"].ToString());
                        _t_training_comple_total_entered = Convert.ToInt32(dtRow["t_training_comple_total_entered"].ToString());
                        _t_youth_ass_toolkit_total_entered = Convert.ToInt32(dtRow["t_youth_ass_toolkit_total_entered"].ToString());
                        _t_youth_tracer_total_entered = Convert.ToInt32(dtRow["t_youth_tracer_total_entered"].ToString());
                        _t_dovcc_total_entered = Convert.ToInt32(dtRow["t_dovcc_total_entered"].ToString());
                        _t_sovcc_total_entered = Convert.ToInt32(dtRow["t_sovcc_total_entered"].ToString());
                        _t_cbsd_resource_alloc_total_entered = Convert.ToInt32(dtRow["t_cbsd_resource_alloc_total_entered"].ToString());
                        _t_cbsd_staff_appra_total_entered = Convert.ToInt32(dtRow["t_cbsd_staff_appra_total_entered"].ToString());
                        _t_reintergration_total_entered = Convert.ToInt32(dtRow["t_reintergration_total_entered"].ToString());
                        _t_dreams_registration_total_entered = Convert.ToInt32(dtRow["t_dreams_registration_total_entered"].ToString());
                        _t_hct_total_entered = Convert.ToInt32(dtRow["t_hct_total_entered"].ToString());
                        _t_dreams_partner_total_entered = Convert.ToInt32(dtRow["t_dreams_partner_total_entered"].ToString());
                        _t_sunovuyo_total_entered = Convert.ToInt32(dtRow["t_sunovuyo_total_entered"].ToString());
                        _t_dreams_stepping_stones_total_entered = Convert.ToInt32(dtRow["t_dreams_stepping_stones_total_entered"].ToString());

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

        }

        public static void Update_manual_tools(string dst_id,string tool_type,int total_entered)
        {
            string SQL = string.Empty;
            try
            {
                switch (tool_type) {
                    case "t_linkage_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_linkage_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_youth_savings_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_youth_savings_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_appre_progress_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_appre_progress_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_training_inventory_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_training_inventory_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_youth_ass_toolkit_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_youth_ass_toolkit_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_reintergration_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_reintergration_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_dreams_registration_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_dreams_registration_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_hct_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_hct_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_dreams_partner_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_dreams_partner_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_sunovuyo_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_sunovuyo_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                    case "t_dreams_stepping_stones_total_entered":
                        SQL = @"UPDATE um_weekly_data_entracker_tools_entered SET t_dreams_stepping_stones_total_entered = '{0}' WHERE dst_id = '{1}'";
                        break;
                }

                SQL = string.Format(SQL,total_entered,dst_id);

                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.ExecuteNonQuery();

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

        }


        public static int save_update_daily_tools_entered(string tracker_id)
        {

            string strSQL = string.Empty;
            int InsertResult = 0;
            try
            {

                #region save
                strSQL = @"IF NOT EXISTS(SELECT tracker_id FROM [um_weekly_data_tracker_daily_tools_entered] WHERE tracker_id = '{0}')
	                        BEGIN
	                        INSERT INTO [dbo].[um_weekly_data_tracker_daily_tools_entered]
                            ([tracker_id] ,[t_ipt_total_entered],[t_hat_total_entered] ,[t_hip_total_entered],[t_hv_total_entered],[t_rat_total_entered]
                            ,[t_linkage_total_entered] ,[t_refferal_total_entered] ,[t_edu_subsidy_total_entered],[t_cTraining_SILC_total_entered] ,[t_cTraining_youth_total_entered]
                            ,[t_youth_savings_total_entered] ,[t_appre_progress_total_entered],[t_training_inventory_total_entered],[t_appre_skill_aqui_total_entered],[t_training_comple_total_entered]
                            ,[t_youth_ass_toolkit_total_entered],[t_youth_tracer_total_entered] ,[t_dovcc_total_entered] ,[t_sovcc_total_entered],[t_cbsd_resource_alloc_total_entered],[t_cbsd_staff_appra_total_entered]
                            ,[t_reintergration_total_entered],[t_dreams_registration_total_entered],[t_hct_total_entered] ,[t_dreams_partner_total_entered],[t_sunovuyo_total_entered],[t_dreams_stepping_stones_total_entered])
                        VALUES
                            ('{0}' ,'{1}','{2}' ,'{3}','{4}' ,'{5}','{6}','{7}','{8}','{9}','{10}' ,'{11}','{12}','{13}','{14}'
                            ,'{15}','{16}','{17}','{18}' ,'{19}' ,'{20}' ,'{21}' ,'{22}' ,'{23}' ,'{24}','{25}' ,'{26}','{27}')
                        END ELSE BEGIN
			                        UPDATE [dbo].[um_weekly_data_tracker_daily_tools_entered]
                                    SET [t_ipt_total_entered] = '{1}'
                                        ,[t_hat_total_entered] = '{2}'
                                        ,[t_hip_total_entered] = '{3}'
                                        ,[t_hv_total_entered] = '{4}'
                                        ,[t_rat_total_entered] = '{5}'
                                        ,[t_linkage_total_entered] = '{6}'
                                        ,[t_refferal_total_entered] = '{7}'
                                        ,[t_edu_subsidy_total_entered] = '{8}'
                                        ,[t_cTraining_SILC_total_entered] = '{9}'
                                        ,[t_cTraining_youth_total_entered] = '{10}'
                                        ,[t_youth_savings_total_entered] = '{11}'
                                        ,[t_appre_progress_total_entered] = '{12}'
                                        ,[t_training_inventory_total_entered] = '{13}'
                                        ,[t_appre_skill_aqui_total_entered] = '{14}'
                                        ,[t_training_comple_total_entered] = '{15}'
                                        ,[t_youth_ass_toolkit_total_entered] = '{16}'
                                        ,[t_youth_tracer_total_entered] = '{17}'
                                        ,[t_dovcc_total_entered] = '{18}'
                                        ,[t_sovcc_total_entered] = '{19}'
                                        ,[t_cbsd_resource_alloc_total_entered] = '{20}'
                                        ,[t_cbsd_staff_appra_total_entered] = '{21}'
                                        ,[t_reintergration_total_entered] = '{22}'
                                        ,[t_dreams_registration_total_entered] = '{23}'
                                        ,[t_hct_total_entered] = '{24}'
                                        ,[t_dreams_partner_total_entered] = '{25}'
                                        ,[t_sunovuyo_total_entered] = '{26}'
                                        ,[t_dreams_stepping_stones_total_entered] = '{27}'
                                    WHERE [tracker_id] = '{0}'
                        END";
                strSQL = string.Format(strSQL, tracker_id, _t_daily_ipt_total_received, _t_daily_hat_total_receivedd, _t_daily_hip_total_received, _t_daily_hv_total_receivedd, _t_daily_rat_total_received,
                               _t_daily_linkage_total_received, _t_daily_refferal_total_received, _t_daily_edu_subsidy_total_received, _t_daily_cTraining_SILC_total_received,
                               _t_daily_cTraining_youth_total_received, _t_daily_youth_savings_total_received, _t_daily_appre_progress_total_received, _t_daily_training_inventory_total_received,
                               _t_daily_appre_skill_aqui_total_received, _t_daily_training_comple_total_received, _t_daily_youth_ass_toolkit_total_received, _t_daily_youth_tracer_total_received,
                               _t_daily_dovcc_total_received, _t_daily_sovcc_total_received, _t_daily_cbsd_resource_alloc_total_received, _t_daily_cbsd_staff_appra_total_received, _t_daily_reintergration_total_received,
                               _t_daily_dreams_registration_total_received, _t_daily_hct_total_received, _t_daily_dreams_partner_total_received, _t_daily_sunovuyo_total_received, _t_daily_dreams_stepping_stones_total_received);
                #endregion save

                //if (tracker_id == string.Empty)
                //{
                //    #region Save
                //    strSQL = @"INSERT INTO [dbo].[um_weekly_data_tracker_daily_tools_entered]
                //               ([tracker_id] ,[t_ipt_total_entered],[t_hat_total_entered] ,[t_hip_total_entered],[t_hv_total_entered],[t_rat_total_entered]
                //               ,[t_linkage_total_entered] ,[t_refferal_total_entered] ,[t_edu_subsidy_total_entered],[t_cTraining_SILC_total_entered] ,[t_cTraining_youth_total_entered]
                //               ,[t_youth_savings_total_entered] ,[t_appre_progress_total_entered],[t_training_inventory_total_entered],[t_appre_skill_aqui_total_entered],[t_training_comple_total_entered]
                //               ,[t_youth_ass_toolkit_total_entered],[t_youth_tracer_total_entered] ,[t_dovcc_total_entered] ,[t_sovcc_total_entered],[t_cbsd_resource_alloc_total_entered],[t_cbsd_staff_appra_total_entered]
                //               ,[t_reintergration_total_entered],[t_dreams_registration_total_entered],[t_hct_total_entered] ,[t_dreams_partner_total_entered],[t_sunovuyo_total_entered],[t_dreams_stepping_stones_total_entered])
                //         VALUES
                //               ('{0}' ,'{1}','{2}' ,'{3}','{4}' ,'{5}','{6}','{7}','{8}','{9}','{10}' ,'{11}','{12}','{13}','{14}'
                //               ,'{15}','{16}','{17}','{18}' ,'{19}' ,'{20}' ,'{21}' ,'{22}' ,'{23}' ,'{24}','{25}' ,'{26}','{27}')";

                //    strSQL = string.Format(strSQL, tracker_id, _t_daily_ipt_total_received, _t_daily_hat_total_receivedd, _t_daily_hip_total_received, _t_daily_hv_total_receivedd, _t_daily_rat_total_received,
                //                _t_daily_linkage_total_received, _t_daily_refferal_total_received, _t_daily_edu_subsidy_total_received, _t_daily_cTraining_SILC_total_received,
                //                _t_daily_cTraining_youth_total_received, _t_daily_youth_savings_total_received, _t_daily_appre_progress_total_received, _t_daily_training_inventory_total_received,
                //                _t_daily_appre_skill_aqui_total_received, _t_daily_training_comple_total_received, _t_daily_youth_ass_toolkit_total_received, _t_daily_youth_tracer_total_received,
                //                _t_daily_dovcc_total_received, _t_daily_sovcc_total_received, _t_daily_cbsd_resource_alloc_total_received, _t_daily_cbsd_staff_appra_total_received, _t_daily_reintergration_total_received,
                //                _t_daily_dreams_registration_total_received, _t_daily_hct_total_received, _t_daily_dreams_partner_total_received, _t_daily_sunovuyo_total_received, _t_daily_dreams_stepping_stones_total_received);

                //    #endregion Save

                //}
                //else
                //{
                //    #region update
                //    strSQL = @"UPDATE [dbo].[um_weekly_data_tracker_daily_tools_entered]
                //               SET [t_ipt_total_entered] = '{1}'
                //                  ,[t_hat_total_entered] = '{2}'
                //                  ,[t_hip_total_entered] = '{3}'
                //                  ,[t_hv_total_entered] = '{4}'
                //                  ,[t_rat_total_entered] = '{5}'
                //                  ,[t_linkage_total_entered] = '{6}'
                //                  ,[t_refferal_total_entered] = '{7}'
                //                  ,[t_edu_subsidy_total_entered] = '{8}'
                //                  ,[t_cTraining_SILC_total_entered] = '{9}'
                //                  ,[t_cTraining_youth_total_entered] = '{10}'
                //                  ,[t_youth_savings_total_entered] = '{11}'
                //                  ,[t_appre_progress_total_entered] = '{12}'
                //                  ,[t_training_inventory_total_entered] = '{13}'
                //                  ,[t_appre_skill_aqui_total_entered] = '{14}'
                //                  ,[t_training_comple_total_entered] = '{15}'
                //                  ,[t_youth_ass_toolkit_total_entered] = '{16}'
                //                  ,[t_youth_tracer_total_entered] = '{17}'
                //                  ,[t_dovcc_total_entered] = '{18}'
                //                  ,[t_sovcc_total_entered] = '{19}'
                //                  ,[t_cbsd_resource_alloc_total_entered] = '{20}'
                //                  ,[t_cbsd_staff_appra_total_entered] = '{21}'
                //                  ,[t_reintergration_total_entered] = '{22}'
                //                  ,[t_dreams_registration_total_entered] = '{23}'
                //                  ,[t_hct_total_entered] = '{24}'
                //                  ,[t_dreams_partner_total_entered] = '{25}'
                //                  ,[t_sunovuyo_total_entered] = '{26}'
                //                  ,[t_dreams_stepping_stones_total_entered] = '{27}'
                //             WHERE [tracker_id] = '{0}'";

                //    strSQL = string.Format(strSQL, tracker_id, _t_daily_ipt_total_received, _t_daily_hat_total_receivedd, _t_daily_hip_total_received, _t_daily_hv_total_receivedd, _t_daily_rat_total_received,
                //                _t_daily_linkage_total_received, _t_daily_refferal_total_received, _t_daily_edu_subsidy_total_received, _t_daily_cTraining_SILC_total_received,
                //                _t_daily_cTraining_youth_total_received, _t_daily_youth_savings_total_received, _t_daily_appre_progress_total_received, _t_daily_training_inventory_total_received,
                //                _t_daily_appre_skill_aqui_total_received, _t_daily_training_comple_total_received, _t_daily_youth_ass_toolkit_total_received, _t_daily_youth_tracer_total_received,
                //                _t_daily_dovcc_total_received, _t_daily_sovcc_total_received, _t_daily_cbsd_resource_alloc_total_received, _t_daily_cbsd_staff_appra_total_received, _t_daily_reintergration_total_received,
                //                _t_daily_dreams_registration_total_received, _t_daily_hct_total_received, _t_daily_dreams_partner_total_received, _t_daily_sunovuyo_total_received, _t_daily_dreams_stepping_stones_total_received);
                //    #endregion update

                //}
                using (conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    cmd.CommandTimeout = 3600;

                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    cmd.ExecuteNonQuery();
                    //InsertResult = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd.Parameters.Clear();

                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
            return InsertResult;
        }

        #region Reports
        public static DataSet _ReturnDataEntryReport(string myQuery,string tracker_id)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlDataAdapter adapt;
            try
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_DataEntryTrackerReports", conn))
                        {
                            cmd.CommandTimeout = 3600;

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 50);
                            cmd.Parameters["@QueryType"].Value = myQuery;

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Tracker_id", SqlDbType.NVarChar, 100);
                            cmd.Parameters["@Tracker_id"].Value = tracker_id;

                            if (conn.State == ConnectionState.Closed)
                            {
                                conn.Open();
                            }
                            cmd.Connection = conn;
                            adapt = new SqlDataAdapter(cmd);
                            adapt.Fill(ds,"report");
                            


                            cmd.Parameters.Clear();
                            if (conn.State != ConnectionState.Closed)
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (SqlException sqlException)
                {
                    throw new Exception(sqlException.ToString());
                }
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
        #endregion

        #region LoadTrackerLists
        public static DataTable LoadTrackerLists()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT tracker_id, dst.dst_name,CAST(T.tracker_date AS DATE) AS tracker_date,um.usr_first_name + ' ' + um.usr_last_name AS um_name,um.usr_email  FROM um_weekly_dst_data_entry_tracker T
                        INNER JOIN lst_district dst ON T.dst_id = dst.dst_id
                        INNER JOIN um_user um ON T.usr_id_create = um.usr_id
                        ORDER BY T.tracker_date DESC";

                SQL = string.Format(SQL, tracker_id);

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

        public static DataTable LoadTrackerDetails(string tracker_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = string.Empty;
            try
            {

                SQL = @"SELECT T.* FROM um_weekly_dst_data_entry_tracker T WHERE T.tracker_id = '{0}'";

                SQL = string.Format(SQL, tracker_id);

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
        #endregion
    }
}
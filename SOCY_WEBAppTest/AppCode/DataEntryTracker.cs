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
        public static DateTime tracker_begin_date = DateTime.Now;
        public static DateTime tracker_end_date = DateTime.Now;
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
        public static string t_comments = string.Empty;

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

        public static int save_data_entry_tracker()
        {

            string strSQL = string.Empty;
            int InsertResult = 0;
            try
            {
                strSQL = @"INSERT INTO [dbo].[um_weekly_dst_data_entry_tracker]
           ([dst_id] ,[usr_id_create] ,[tracker_begin_date],[tracker_end_date] ,[t_ipt_total_received] ,[t_ipt_total_entered],[t_hat_total_received],[t_hat_total_entered]
           ,[t_hip_total_received],[t_hip_total_entered],[t_hv_total_received] ,[t_hv_total_entered],[t_rat_total_received],[t_rat_total_entered] ,[t_linkage_total_received]
           ,[t_linkage_total_entered] ,[t_refferal_total_received],[t_refferal_total_entered],[t_edu_subsidy_total_received],[t_edu_subsidy_total_entered],[t_cTraining_SILC_total_received]
           ,[t_cTraining_SILC_total_entered] ,[t_cTraining_youth_total_received] ,[t_cTraining_youth_total_entered],[t_youth_savings_total_received],[t_youth_savings_total_entered],[t_appre_progress_total_received]
           ,[t_appre_progress_total_entered] ,[t_training_inventory_total_received] ,[t_training_inventory_total_entered],[t_appre_skill_aqui_total_received],[t_appre_skill_aqui_total_entered] ,[t_training_comple_total_received]
           ,[t_training_comple_total_entered] ,[t_youth_ass_toolkit_total_received],[t_youth_ass_toolkit_total_entered],[t_youth_tracer_total_received],[t_youth_tracer_total_entered] ,[t_dovcc_total_received],[t_dovcc_total_entered]
           ,[t_sovcc_total_received],[t_sovcc_total_entered] ,[t_cbsd_resource_alloc_total_received] ,[t_cbsd_resource_alloc_total_entered],[t_cbsd_staff_appra_total_received],[t_cbsd_staff_appra_total_entered] ,[t_reintergration_total_received]
           ,[t_reintergration_total_entered],[t_dreams_registration_total_received],[t_dreams_registration_total_entered],[t_hct_total_received],[t_hct_total_entered],[t_dreams_partner_total_received],[t_dreams_partner_total_entered]
           ,[t_sunovuyo_total_received],[t_sunovuyo_total_entered],[t_dreams_stepping_stones_total_received],[t_dreams_stepping_stones_total_entered],[t_comments])
     VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}' ,'{14}' ,'{15}','{16}' ,'{17}','{18}','{19}','{20}'
           ,'{21}' ,'{22}','{23}','{24}','{25}','{26}','{27}' ,'{28}','{29}' ,'{30}','{31}','{32}','{33}','{34}' ,'{35}','{36}' ,'{37}' ,'{38}','{39}','{40}'
           ,'{41}','{42}' ,'{43}','{44}','{45}' ,'{46}','{47}' ,'{48}' ,'{49}','{50}','{51}' ,'{52}','{53}' ,'{54}','{55}' ,'{56}','{57}' ,'{58}')";

                strSQL = string.Format(strSQL, dst_id, usr_id_create, tracker_begin_date, tracker_end_date, t_ipt_total_received, t_ipt_total_entered, t_hat_total_received, t_hat_total_entered
                        , t_hip_total_received, t_hip_total_entered, t_hv_total_received, t_hv_total_entered, t_rat_total_received, t_rat_total_entered, t_linkage_total_received
                        , t_linkage_total_entered, t_refferal_total_received, t_refferal_total_entered, t_edu_subsidy_total_received, t_edu_subsidy_total_entered, t_cTraining_SILC_total_received
                        , t_cTraining_SILC_total_entered, t_cTraining_youth_total_received, t_cTraining_youth_total_entered, t_youth_savings_total_received, t_youth_savings_total_entered, t_appre_progress_total_received
                        , t_appre_progress_total_entered, t_training_inventory_total_received, t_training_inventory_total_entered, t_appre_skill_aqui_total_received, t_appre_skill_aqui_total_entered, t_training_comple_total_received
                        , t_training_comple_total_entered, t_youth_ass_toolkit_total_received, t_youth_ass_toolkit_total_entered, t_youth_tracer_total_received, t_youth_tracer_total_entered, t_dovcc_total_received, t_dovcc_total_entered
                        , t_sovcc_total_received, t_sovcc_total_entered, t_cbsd_resource_alloc_total_received, t_cbsd_resource_alloc_total_entered, t_cbsd_staff_appra_total_received, t_cbsd_staff_appra_total_entered, t_reintergration_total_received
                        , t_reintergration_total_entered, t_dreams_registration_total_received, t_dreams_registration_total_entered, t_hct_total_received, t_hct_total_entered, t_dreams_partner_total_received, t_dreams_partner_total_entered
                        , t_sunovuyo_total_received, t_sunovuyo_total_entered, t_dreams_stepping_stones_total_received, t_dreams_stepping_stones_total_entered, t_comments
                        );

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
    }
}
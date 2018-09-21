using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SOCY_WEBAppTest.AppCode
{
    public static class Cluster_data_capture_officesDB
    {
        static DBConnection dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);
        static string SQLConnection = System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ConnectionString;
        static SqlConnection conn;

        static string district_id = string.Empty;
        static string office_group_id = string.Empty;

        public static DataTable Return_list_of_districts()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = "SELECT dst_id,dst_name FROM lst_district";
            try
            {

                using (conn = new SqlConnection(SQLConnection))
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

        //return list of offices by district
        public static DataTable Return_list_of_district_offices(string district_id)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = "SELECT ofc_id,ofc_name FROM um_office WHERE district_id = '" + district_id + "'";
            try
            {

                using (conn = new SqlConnection(SQLConnection))
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

        #region create office groups
        public static void Create_Office_group(CheckBoxList chklist, string district_id, string group_name)
        {

            if (Cluster_data_capture_officesDB.district_id != string.Empty)
            {
                //fire updates
            }
            else
            {
                string Query = "INSERT INTO um_office_groups_main(district_id,group_name) OUTPUT INSERTED.group_record_guid VALUES('{0}','{1}')";

                string SQL = string.Empty;
                try
                {
                    SQL = string.Format(Query, district_id, group_name);

                    using (conn = new SqlConnection(SQLConnection))
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        cmd.CommandTimeout = 3600;

                        cmd.CommandType = CommandType.Text;

                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        office_group_id = (string)cmd.ExecuteScalar(); //save office grp

                        save_offices_mapped_to_office_grp(chklist, office_group_id); //save office-office grp mapping

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
        }

        //save offices mapped to offcice grp
        public static void save_offices_mapped_to_office_grp(CheckBoxList chklist, string office_grp_record_guid)
        {
            string ofc_id = string.Empty;
            bool active = true;

            //loop through the list to get selected items
            foreach (ListItem li in chklist.Items)
            {
                if (li.Selected)
                {
                    ofc_id = li.Value;

                    string Query = "INSERT INTO um_office_group_details(office_grp_record_guid,ofc_id,active) VALUES('{0}','{1}','{2}')";

                    string SQL = string.Empty;
                    try
                    {
                        SQL = string.Format(Query, office_grp_record_guid, ofc_id, active);

                        using (conn = new SqlConnection(SQLConnection))
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
            }
        }

        //return office grps
        public static DataTable Return_list_of_office_groups()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter Adapt;
            string SQL = "SELECT group_record_guid,group_name,D.dst_name FROM um_office_groups_main M " +
                "LEFT JOIN lst_district D ON M.district_id = D.dst_id " +
                "ORDER BY D.dst_name";
            try
            {

                using (conn = new SqlConnection(SQLConnection))
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
        #endregion  create office groups
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using PSAUtilsWeb;

namespace SOCY_WEBAppTest
{
    public class OfficeDB
    {
        #region Variables
        #region Public
        public string ofc_id = string.Empty;
        public string ofc_name = string.Empty;
        public string ofc_server_id = string.Empty;
        public string ofc_email = string.Empty;
        public string ofc_phone = string.Empty;
        public string ofc_address = string.Empty;
        public string ofc_app_version = string.Empty;
        public string ost_id = utilSOCYWeb.cEmptyListValue;
        public string otp_id = utilSOCYWeb.cEmptyListValue;
        public string ousr_id_contact = string.Empty;
        public string usr_id_update = string.Empty;
        public string ousr_name = string.Empty;
        public string imp_date = string.Empty;
        #endregion Public
        #endregion Variables

        #region Function Methods

        /// <summary>
        /// Defaults all the variables in the class
        /// </summary>
        public void Default()
        {
            #region Default
            ofc_id = string.Empty;
            ofc_name = string.Empty;
            ofc_server_id = string.Empty;
            ofc_email = string.Empty;
            ofc_phone = string.Empty;
            ofc_address = string.Empty;
            ofc_app_version = string.Empty;
            ost_id = utilSOCYWeb.cEmptyListValue;
            otp_id = utilSOCYWeb.cEmptyListValue;
            ousr_id_contact = string.Empty;
            usr_id_update = string.Empty;
            ousr_name = string.Empty;
            imp_date = string.Empty;
            #endregion Default
        }

        /// <summary>
        /// Loads the class variables with the specified Office's information
        /// </summary>
        /// <param name="strID">Office to be loaded</param>
        public void Load(string strID)
        {
            #region Variables
            DataTable dt = GetObject(strID);
            #endregion Variables

            #region Load Role
            if (utilCollections.HasRows(dt))
            {
                Load(dt.Rows[0]);
            }
            else
            {
                Default();
            }
            #endregion Load Role
        }

        /// <summary>
        /// Loads the class variables with the specified Office's information
        /// </summary>
        /// <param name="dr">DataRow containing the data</param>
        public void Load(DataRow dr)
        {
            #region Load Variables
            ofc_id = dr["ofc_id"].ToString();
            ofc_name = dr["ofc_name"].ToString();
            ofc_server_id = dr["ofc_server_id"].ToString();
            ofc_email = dr["ofc_email"].ToString();
            ofc_phone = dr["ofc_phone"].ToString();
            ofc_address = dr["ofc_address"].ToString();
            ofc_app_version = dr["ofc_app_version"].ToString();
            ost_id = dr["ost_id"].ToString();
            otp_id = dr["otp_id"].ToString();
            ousr_id_contact = dr["ousr_id_contact"].ToString();
            usr_id_update = dr["usr_id_update"].ToString();
            ousr_name = dr["ousr_name"].ToString();
            imp_date = dr["imp_date"].ToString();
            #endregion Load Variables
        }

        /// <summary>
        /// Updates the Office into the database
        /// </summary>
        public void Update()
        {
            #region Variables
            DBConnection dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);
            string strSQL = string.Empty;
            #endregion Variables

            try
            {
                #region SQL
                strSQL = string.Format("UPDATE um_office " +
                    "SET ofc_name = '{1}', ofc_server_id = '{2}', " +
                    "ofc_email = '{3}', ofc_phone = '{4}', ofc_address = '{5}', " +
                    "ofc_app_version = '{6}', " +
                    "ost_id = '{7}', otp_id = '{8}', ousr_id_contact = '{9}', " +
                    "usr_id_update = '{10}', usr_date_update = GETDATE() " +
                    "WHERE ofc_id = '{0}' ",
                    ofc_id, utilFormatting.StringForSQL(ofc_name), ofc_server_id,
                    utilFormatting.StringForSQL(ofc_email), utilFormatting.StringForSQL(ofc_phone), utilFormatting.StringForSQL(ofc_address),
                    ofc_app_version,
                    ost_id, otp_id, ousr_id_contact,
                    usr_id_update);
                dbCon.ExecuteNonQuery(strSQL);
                #endregion SQL
            }
            finally
            {
                dbCon.Dispose();
            }
        }
        #endregion Function Methods

        #region Get Methods
        /// <summary>
        /// Gets Offices based of filter criteria
        /// </summary>
        /// <param name="arrFilter">Array, value key pair, containing the filter criteria to apply</param>
        /// <param name="intArrayLength">Number of filter criteria to apply</param>
        /// <returns>DataSet contaning DataTables with Offices and support data</returns>  
        public DataTable GetByCriteria(string[,] arrFilter, int intArrayLength, string strLngId)
        {
            #region Variables
            DBConnection dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);

            DataTable dt = null;
            string strSQL = string.Empty;
            string strWHERE = "WHERE 1=1 ";
            #endregion Variables

            try
            {
                #region SQL
                #region WHERE
                for (int intCount = 0; intCount < intArrayLength; intCount++)
                {
                    switch (arrFilter[intCount, 0])
                    {
                        case "ofc_name":
                            strWHERE = strWHERE + string.Format("AND LOWER(RTRIM(LTRIM(ofc.ofc_name))) LIKE '%{0}%' ",
                                arrFilter[intCount, 1].ToLower());
                            break;
                        case "ost_id":
                            strWHERE = strWHERE + string.Format("AND ofc.ost_id = '{0}' ",
                                arrFilter[intCount, 1].ToLower());
                            break;
                    }
                }
                #endregion WHERE

                strSQL = "SELECT ofc.ofc_id, ofc.ofc_name, " +
                    "ofc.ofc_email, ofc.ofc_phone, ofc.ofc_app_version, " +
                    "RTRIM(LTRIM(ousr.ousr_first_name + ' ' + ousr.ousr_last_name)) AS ousr_name, ost.ost_name, " +
                    "ISNULL(imp.imp_date, '') AS imp_date " +
                    "FROM um_office ofc " +
                    "INNER JOIN um_office_user ousr ON ofc.ofc_id = ousr.ofc_id " +
                    "INNER JOIN (SELECT ost_id, ost_name FROM lst_office_status WHERE lng_id = '{0}') ost ON ofc.ost_id = ost.ost_id " +
                    "LEFT JOIN (" +
                    "SELECT CONVERT(CHAR(20), MAX(imp_date), 113) AS imp_date, ofc_id " +
                    "FROM um_import_history GROUP BY ofc_id) imp ON ofc.ofc_id = imp.ofc_id " +
                    strWHERE +
                    "ORDER BY ofc_name ";
                strSQL = string.Format(strSQL, strLngId);
                dt = dbCon.ExecuteQueryDataTable(strSQL);
                #endregion SQL
            }
            finally
            {
                dbCon.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// Gets specified Office
        /// </summary>
        /// <param name="strID">Office to be retrieved</param>
        /// <returns>DataTable containing the Office</returns>
        public DataTable GetObject(string strID)
        {
            #region Variables
            DBConnection dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);

            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            try
            {
                strSQL = string.Format("SELECT ofc.ofc_id, ofc.ofc_name, ofc.ofc_server_id, " +
                    "ofc.ofc_email, ofc.ofc_phone, ofc.ofc_address, ofc.ofc_app_version, " +
                    "ofc.ost_id, ofc.otp_id, ofc.ousr_id_contact, ofc.usr_id_update, " +
                    "RTRIM(LTRIM(ousr.ousr_first_name + ' ' + ousr.ousr_last_name)) AS ousr_name, " +
                    "ISNULL(imp.imp_date, '') AS imp_date " +
                    "FROM um_office ofc " +
                    "INNER JOIN um_office_user ousr ON ofc.ofc_id = ousr.ofc_id " +
                    "LEFT JOIN (" +
                    "SELECT CONVERT(CHAR(20), MAX(imp_date), 113) AS imp_date, ofc_id " +
                    "FROM um_import_history WHERE ofc_id = '{0}' GROUP BY ofc_id) imp ON ofc.ofc_id = imp.ofc_id " +
                    "WHERE ofc.ofc_id = '{0}' ", strID);
                dt = dbCon.ExecuteQueryDataTable(strSQL);
            }
            finally
            {
                dbCon.Dispose();
            }
            #endregion SQL

            return dt;
        }
        /// <summary>
        /// Gets Offices that have uploaded data
        /// </summary>
        /// <param name="dbCon">Database connection to be used</param>
        /// <returns>DataSet contaning Offices that have uploaded data</returns>  
        public DataTable GetUploadOffices(DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT ofc.ofc_id, ofc.ofc_name " +
                "FROM um_office ofc " +
                "INNER JOIN um_import_history imp ON ofc.ofc_id = imp.ofc_id  " +
                "ORDER BY ofc_name ";
            strSQL = string.Format(strSQL);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }
        #endregion Get Methods
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Data;
using System.Data.SqlClient;
using PSAUtilsWeb;
using System.Configuration;
using System.IO;
using SOCY_WEBAppTest.AppCode;

namespace SOCY_WEBAppTest
{
    //// <summary>
    /// Summary description for SOCY_WS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class SOCY_WS : System.Web.Services.WebService
    {
        #region Valiables
        private int pintSessionDateLength = 8;
        private int pintSessionKeyLength = 8;
        static string SQLConnection = System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ConnectionString;
        #endregion Variables

        #region Public
        #region Check Methods
        /// <summary>
        /// Checks response that webservice responds
        /// </summary>
        /// <returns>true</returns>
        [WebMethod]
        public bool OnlineCheck()
        {
            return true;
        }

        /// <summary>
        /// Checks response that webservice responds
        /// </summary>
        /// <param name="strOfcId">Calling Office Id</param>
        /// <param name="strVersion">Version of Capture App</param>
        /// <returns>Blank string</returns>
        [WebMethod]
        public string OnlineMessageCheck(string strOfcId, string strVersion)
        {
            return "";
        }
        #endregion Check Methods

        #region Function Methods
        [WebMethod]
        public  string DownloadData(string strSsnId, string strLDImpSid, string strLDTable, string strLDObjId, string district_list)
        {
            #region Variables
            DBConnection dbCon = null;

            DataTable dt = null;
            DateTime dtmSession = GetSessionDate(strSsnId);
            int intTop = 5;
            string strImpSid = GetImportId(strSsnId);
            string strKey = strSsnId.Substring(0, pintSessionKeyLength);
            string strOfcId = string.Empty;
            string strOstId = string.Empty;
            string strSQL = string.Empty;
            string strXML = string.Empty;
            #endregion Varaibles

            if (dtmSession > DateTime.Now.AddDays(-1))
            {
                dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);
                dbCon.TransactionBegin();
                try
                {
                    #region Get the Office ID
                    dt = GetImportHistory(strImpSid, dbCon);
                    #endregion Get the Office ID

                    if (utilCollections.HasRows(dt))
                    {
                        strOfcId = dt.Rows[0]["ofc_id"].ToString();
                        dt = GetData("um_office", "ofc_id", strOfcId, dbCon); //get the office details
                        if (utilCollections.HasRows(dt))
                            strOstId = dt.Rows[0]["ost_id"].ToString(); //extract the office status id
                        if (strOstId.Equals(utilSOCYWeb.cOSTValidated)) //if office is validated then continue with download
                        {
                            #region Manage last Downloaded
                            if (strLDImpSid.Length != 0)  //last download import sid
                            {
                                SaveOfficeDownload(strOfcId, strLDImpSid, strLDTable, strLDObjId, dbCon);//strLDImpSid(last download import sid),strLDTable(last download Table),strLDObjId(last downlaod object id)
                            }
                            else
                            {
                                dt = GetData("um_office_download", "ofc_id", strOfcId, dbCon);
                                if (utilCollections.HasRows(dt))
                                {
                                    strLDImpSid = dt.Rows[0]["imp_sid"].ToString();
                                    strLDTable = dt.Rows[0]["odl_table"].ToString();
                                    strLDObjId = dt.Rows[0]["odl_obj_id"].ToString();
                                }
                                else
                                {
                                    strLDImpSid = GetFirstImportHistoryId(strOfcId, dbCon);
                                    dt = GetFirstDownloadTable(dbCon);
                                    strLDTable = dt.Rows[0]["sdl_name"].ToString();
                                    strLDObjId = "";
                                }
                            }
                            #endregion  Manage last Downloaded

                            if (!strLDImpSid.Equals("0"))
                            {
                                #region Get Next Set of Data
                                intTop = Convert.ToInt32(ConfigurationManager.AppSettings[utilSOCYWeb.cWCKDownloadRecords].ToString()); //number of records to dowload per select
                                strXML = GetDownloadData(strOfcId, strLDImpSid, strLDTable, strLDObjId, intTop, strKey, district_list, dbCon);
                                #endregion Get Next Set of Data
                            }
                        }
                    }

                    dbCon.TransactionCommit();
                }
                catch (Exception exc)
                {
                    dbCon.TransactionRollback();
                    throw exc;
                }
                finally
                {
                    dbCon.Dispose();
                }
            }

            return strXML;
        }

        /// <summary>
        /// Checks status of calling office
        /// </summary>
        /// <param name="strOfcId">Calling Office Id</param>
        /// <returns>Office Status ID</returns>
        [WebMethod]
        public string OfficeStatus(string strOfcId)
        {
            #region Variables
            OfficeDB dbOfc = new OfficeDB();
            #endregion Variables

            #region Get Data
            dbOfc.Load(strOfcId);
            #endregion Get Data

            return dbOfc.ost_id;
        }

        /// <summary>
        /// Validates and Updates the calling Office's data
        /// </summary>
        /// <param name="strOfcId">Calling Office Id</param>
        /// <param name="strOfficeDataXML">Calling Office data in XML</param>
        /// <param name="strContactDataXML">Calling Office Contact data in XML</param>
        /// <returns>Session ID, blank if rejected</returns>
        [WebMethod]
        public string OfficeValidation(string strOfcId, string strOfficeDataXML, string strContactDataXML)
        {
            #region Variables
            DBConnection dbCon = null;

            DataTable dt = null;
            DataTable dtContact = null;
            DataTable dtOffice = null;
            string strImpId = string.Empty;
            string strOstId = string.Empty;
            string strSsnId = string.Empty;
            #endregion Variables

            dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);
            dbCon.TransactionBegin();
            try
            {
                #region Office Data
                dt = GetData("um_office", "ofc_id", strOfcId, dbCon);
                dtOffice = ConvertXMLToDataTable(strOfficeDataXML, strOfcId.Substring(0, pintSessionKeyLength));
                if (utilCollections.HasRows(dt))
                {
                    strOstId = dt.Rows[0]["ost_id"].ToString(); //office status
                    UpdateOffice(dtOffice.Rows[0], dbCon);
                }
                else
                    InsertOffice(dtOffice.Rows[0], dbCon);
                #endregion Office Data

                if (!strOstId.Equals(utilSOCYWeb.cOSTRejected))
                {
                    #region Contact Data
                    dt = GetData("um_office_user", "ousr_id", dtOffice.Rows[0]["usr_id_contact"].ToString(), dbCon);
                    dtContact = ConvertXMLToDataTable(strContactDataXML, strOfcId.Substring(0, pintSessionKeyLength));
                    if (utilCollections.HasRows(dt))
                        UpdateContact(dtContact.Rows[0], dbCon);
                    else
                        InsertContact(dtContact.Rows[0], dbCon);
                    #endregion Contact Data

                    #region Insert Import Record
                    strImpId = InsertImportHistory(strOfcId, dbCon);
                    #endregion Insert Import Record
                }
                dbCon.TransactionCommit();
                strSsnId = GetSessionId(strImpId);
            }
            catch (Exception exc)
            {
                dbCon.TransactionRollback();
                throw exc;
            }
            finally
            {
                dbCon.Dispose();
            }

            return strSsnId;
        }

        [WebMethod]
        public DataTable Download_Office_group(string ofc_id)
        {
            #region Variables

            SqlDataAdapter Adapt = new SqlDataAdapter();
            DataRow dtRow;

            string ofc_grp_id = string.Empty;
            string office_id = string.Empty;
            bool active = false;

            DataTable dt = null;
            DataTable dt_ofc_grp_districts = null;
            DataTable returnDt = new DataTable { TableName = "ofcgrpTable" };

            string strOfcId = string.Empty;
            string strOstId = string.Empty;
            string strSQL = string.Empty;
            string strXML = string.Empty;
            #endregion Varaibles

            SqlConnection conn = new SqlConnection(SQLConnection);
            SqlCommand cmd, cmdGrps;

            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string SQL = "SELECT office_grp_record_guid FROM um_office_group_details WHERE ofc_id = '" + ofc_id + "' ";
                string SQL_ofc = "SELECT office_grp_record_guid, ofc_id,active FROM um_office_group_details WHERE office_grp_record_guid = '{0}'";
                string Query;

                //instantiate officegroup datatable
                returnDt.Columns.Add("office_grp_record_guid", typeof(string));
                returnDt.Columns.Add("ofc_id", typeof(string));
                returnDt.Columns.Add("active", typeof(bool));

                cmd = new SqlCommand(SQL, conn);
                Adapt = new SqlDataAdapter(cmd);
                dt = new DataTable();
                Adapt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                    {
                        dtRow = dt.Rows[intCount];
                        ofc_grp_id = dtRow["office_grp_record_guid"].ToString();

                        if (conn.State == ConnectionState.Open) { conn.Close(); }
                        if (conn.State == ConnectionState.Closed) { conn.Open(); }

                        //get list of offices
                        Query = string.Empty;
                        Query = string.Format(SQL_ofc, ofc_grp_id);
                        cmdGrps = new SqlCommand(Query, conn);
                        Adapt = new SqlDataAdapter(cmdGrps);
                        dt_ofc_grp_districts = new DataTable();
                        Adapt.Fill(dt_ofc_grp_districts);
                        if (dt_ofc_grp_districts.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt_ofc_grp_districts.Rows.Count; i++)
                            {

                                DataRow dtRowOfc = dt_ofc_grp_districts.Rows[i];

                                ofc_grp_id = dtRowOfc["office_grp_record_guid"].ToString();
                                office_id = dtRowOfc["ofc_id"].ToString();
                                active = Convert.ToBoolean(dtRowOfc["active"]);

                                returnDt.Rows.Add(ofc_grp_id, office_id, active);

                                int count = returnDt.Rows.Count;
                            }
                        }
                    }

                }

                strXML = ConvertDataTableToXML(dt_ofc_grp_districts, "um_office_group_details", "");
            }
            catch (Exception ex) { throw ex; }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close();conn.Dispose(); }
            }

            return returnDt;
            
        }

        [WebMethod]
        public DataTable Download_Data(string dst_id,DateTime startDate,DateTime endDate,string DownLoadTable)
        {
            #region Variables

            SqlDataAdapter Adapt = new SqlDataAdapter();

            DataTable dt = new DataTable { TableName = DownLoadTable };
            string strSQL = string.Empty;
            #endregion Varaibles

            SqlConnection conn = new SqlConnection(SQLConnection);
            SqlCommand cmd;

            try
            {
                using (SqlConnection SQLConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmCommand = new SqlCommand("sp_SOCY_MIS_DATA_DOWNLOAD", SQLConn))
                {
                    cmCommand.CommandTimeout = 3600;

                    cmCommand.CommandType = CommandType.StoredProcedure;
                    cmCommand.Parameters.Add("@DownLoadTable", SqlDbType.VarChar, 150);
                    cmCommand.Parameters["@DownLoadTable"].Value = DownLoadTable;

                    cmCommand.CommandType = CommandType.StoredProcedure;
                    cmCommand.Parameters.Add("@dst_id", SqlDbType.VarChar, 150);
                    cmCommand.Parameters["@dst_id"].Value = dst_id;

                    cmCommand.CommandType = CommandType.StoredProcedure;
                    cmCommand.Parameters.Add("@startDate", SqlDbType.Date);
                    cmCommand.Parameters["@startDate"].Value = startDate;

                    cmCommand.CommandType = CommandType.StoredProcedure;
                    cmCommand.Parameters.Add("@endDate", SqlDbType.Date);
                    cmCommand.Parameters["@endDate"].Value = @endDate;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Adapt = new SqlDataAdapter(cmCommand);
                    Adapt.Fill(dt);
                }

            }
            catch (Exception ex) { throw ex; }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
            }

            return dt;

        }

        [WebMethod]
        public bool CheckOfficeValidation(string strOfcID)
        {
            #region Variables

            SqlDataAdapter Adapt = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string strSQL = string.Empty;
            DataRow dtRow;
            string ost_id = string.Empty;
            bool isValidated = false;
            #endregion Varaibles

            SqlConnection conn = new SqlConnection(SQLConnection);
            strSQL = "SELECT ost_id FROM um_office WHERE ofc_id = '{0}'";
            strSQL = string.Format(strSQL, strOfcID);
            try
            {
                using (SqlConnection SQLConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString()))
                using (SqlCommand cmCommand = new SqlCommand(strSQL, SQLConn))
                {
                    cmCommand.CommandTimeout = 3600;

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Adapt = new SqlDataAdapter(cmCommand);
                    Adapt.Fill(dt);
                    dtRow = dt.Rows[0];
                    ost_id = dtRow["ost_id"].ToString();

                    if (ost_id  == "1")
                    {
                        isValidated = true;
                    }
                    else
                    {
                        isValidated = false;
                    }
                }
                

            }
            catch (Exception ex) { throw ex; }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
            }

            return isValidated;

        }



        #region TestDataDownload
        [WebMethod]
        public DataTable DownLoadHomeVisit(string name)
        {
            #region Variables

            SqlDataAdapter Adapt = new SqlDataAdapter();

            DataTable dt = new DataTable { TableName = "at_training" };

            #endregion Varaibles

            SqlConnection conn = new SqlConnection(SQLConnection);
            SqlCommand cmd;
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string SQL = "SELECT TOP 1000 * FROM [SOCY_LIVE].[dbo].[hh_household]";


               
                cmd = new SqlCommand(SQL, conn);
                Adapt = new SqlDataAdapter(cmd);
                Adapt.Fill(dt);
            }
            catch (Exception ex) { throw ex; }

            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
            }

            return dt;

        }
        #endregion TestData Download

        [WebMethod]
        public bool ProcessRecord(string strSsnId, string strXML)
        {
            #region Variables
            DBConnection dbCon = null;

            bool blnResult = false;
            DataRow dr = null;
            DataTable dt = null;
            DateTime dtmSession = GetSessionDate(strSsnId);
            string strImpSid = GetImportId(strSsnId);
            string strKey = strSsnId.Substring(0, pintSessionKeyLength);
            string strSQL = string.Empty;
            #endregion Varaibles

            if (dtmSession > DateTime.Now.AddDays(-1))
            {
                #region Process Record
                dt = ConvertXMLToDataTable(strXML, strKey);
                dr = dt.Rows[0];
                switch (dt.TableName)
                {
                    #region ben_activity_training_upload
                    case "ben_activity_training_upload":
                        strSQL = "INSERT INTO ben_activity_training " +
                            "(at_id, " +
                            "at_activity, at_training_for, at_training_point, " +
                            "at_date_begin, at_date_end, " +
                            "at_days, at_session, " +
                            "at_coordinator, at_coordinator_tel, " +
                            "cso_id, ttp_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', '{3}', " +
                            "'{4}', '{5}', {6}, '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', {17}, {18},'{19}') ";
                        DataTable dtr = dt;
                        strSQL = string.Format(strSQL, dr["at_id"].ToString(),
                            utilFormatting.StringForSQL(dr["at_activity"].ToString()), utilFormatting.StringForSQL(dr["at_training_for"].ToString()), utilFormatting.StringForSQL(dr["at_training_point"].ToString()),
                            Convert.ToDateTime(dr["at_date_begin"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["at_date_end"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["at_days"].ToString(), dr["at_session"].ToString(),
                            utilFormatting.StringForSQL(dr["at_coordinator"].ToString()), utilFormatting.StringForSQL(dr["at_coordinator_tel"].ToString()),
                            dr["cso_id"].ToString(), dr["ttp_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_activity_training_upload

                    #region ben_activity_training_participant_upload
                    case "ben_activity_training_participant_upload":
                        strSQL = "INSERT INTO ben_activity_training_participant " +
                            "(atp_id, atp_name, atp_year_of_birth, atp_days, " +
                            "at_id, gnd_id, hhm_id, mtp_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', {3}, " +
                            "'{4}', '{5}', '{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', {13}, {14},'{15}') ";
                        strSQL = string.Format(strSQL, dr["atp_id"].ToString(), utilFormatting.StringForSQL(dr["atp_name"].ToString()), dr["atp_year_of_birth"].ToString(), dr["atp_days"].ToString(),
                            dr["at_id"].ToString(), dr["gnd_id"].ToString(), dr["hhm_id"].ToString(), dr["mtp_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_activity_training_participant_upload

                    #region ben_apprenticeship_register_line_upload
                    case "ben_apprenticeship_register_line_upload":
                        strSQL = "INSERT INTO ben_apprenticeship_register_line " +
                            "(aprl_id, aprl_enterprise, " +
                            "aprl_date_begin, aprl_date_complete, " +
                            "apr_id, apt_id, cso_id, hhm_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', " +
                            "'{4}', '{5}', '{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', {13}, {14},'{15}') ";
                        strSQL = string.Format(strSQL, dr["aprl_id"].ToString(), utilFormatting.StringForSQL(dr["aprl_enterprise"].ToString()),
                            Convert.ToDateTime(dr["aprl_date_begin"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["aprl_date_complete"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["apr_id"].ToString(), dr["apt_id"].ToString(), dr["cso_id"].ToString(), dr["hhm_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_apprenticeship_register_line_upload

                    #region ben_girl_education_register_upload
                    case "ben_girl_education_register_upload":
                        strSQL = "INSERT INTO ben_girl_education_register " +
                            "(ger_id, ger_contact_details, " +
                            "cso_id, fy_id, qy_id, sct_id, swk_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', '{10}', " +
                            "'{11}', {12}, {13},'{14}') ";
                        strSQL = string.Format(strSQL, dr["ger_id"].ToString(), utilFormatting.StringForSQL(dr["ger_contact_details"].ToString()),
                            dr["cso_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(), dr["sct_id"].ToString(), dr["swk_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_girl_education_register_upload

                    #region ben_girl_education_register_child_upload
                    case "ben_girl_education_register_child_upload":
                        strSQL = "INSERT INTO ben_girl_education_register_child " +
                            "(gerc_id, gerc_support_institution, " +
                            "edu_id, fst_id, ger_id, hhm_id, hhm_id_caregiver, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id ) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', '{10}', " +
                            "'{11}', {12}, {13},'{14}') ";
                        strSQL = string.Format(strSQL, dr["gerc_id"].ToString(), utilFormatting.StringForSQL(dr["gerc_support_institution"].ToString()),
                            dr["edu_id"].ToString(), dr["fst_id"].ToString(), dr["ger_id"].ToString(), dr["hhm_id"].ToString(), dr["hhm_id_caregiver"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_girl_education_register_child_upload

                    #region ben_service_register_upload
                    case "ben_service_register_upload":
                        strSQL = "INSERT INTO ben_service_register " +
                            "(svr_id, svr_contact_details, " +
                            "cso_id, dst_id, fy_id, qy_id, swk_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id ) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', '{10}', " +
                            "'{11}', {12}, {13},'{14}') ";
                        strSQL = string.Format(strSQL, dr["svr_id"].ToString(), utilFormatting.StringForSQL(dr["svr_contact_details"].ToString()),
                            dr["cso_id"].ToString(), dr["dst_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(), dr["swk_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_service_register_upload

                    #region ben_service_register_line_upload
                    case "ben_service_register_line_upload":
                        strSQL = "INSERT INTO ben_service_register_line " +
                            "(svrl_id, svrl_eco_strength_other, " +
                            "hhm_id, " +
                            "yn_id_agricalture_advisory, yn_id_apprentice_skills, yn_id_basic_care, yn_id_birth_registration, " +
                            "yn_id_case_handled, yn_id_eco_strength_other, yn_id_newly_enrolled, yn_id_parenting, " +
                            "yn_id_psych_support, yn_id_reintegrated, yn_id_silc_intervention, " +
                            "svr_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id ) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', " +
                            "'{3}', '{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', '{10}', " +
                            "'{11}', '{12}', '{13}', " +
                            "'{14}', " +
                            "'{15}', '{16}', " +
                            "'{17}', '{18}', " +
                            "'{19}', {20}, {21},'{22}') ";
                        strSQL = string.Format(strSQL, dr["svrl_id"].ToString(), utilFormatting.StringForSQL(dr["svrl_eco_strength_other"].ToString()),
                            dr["hhm_id"].ToString(),
                            dr["yn_id_agricalture_advisory"].ToString(), dr["yn_id_apprentice_skills"].ToString(), dr["yn_id_basic_care"].ToString(), dr["yn_id_birth_registration"].ToString(),
                            dr["yn_id_case_handled"].ToString(), dr["yn_id_eco_strength_other"].ToString(), dr["yn_id_newly_enrolled"].ToString(), dr["yn_id_parenting"].ToString(),
                            dr["yn_id_psych_support"].ToString(), dr["yn_id_reintegrated"].ToString(), dr["yn_id_silc_intervention"].ToString(),
                            dr["svr_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_service_register_line_upload

                    #region ben_service_register_line_social_economic_upload
                    case "ben_service_register_line_social_economic_upload":
                        strSQL = "INSERT INTO ben_service_register_line_social_economic " +
                            "(svrlse_id, svrl_id, sec_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id ) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["svrlse_id"].ToString(), dr["svrl_id"].ToString(), dr["sec_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_service_register_line_social_economic_upload

                    #region ben_value_chain_register_upload
                    case "ben_value_chain_register_upload":
                        strSQL = "INSERT INTO ben_value_chain_register " +
                            "(vcr_id, " +
                            "cso_id, fy_id, qy_id, sct_id, swk_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id ) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', '{3}', '{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["vcr_id"].ToString(),
                            dr["cso_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(), dr["sct_id"].ToString(), dr["swk_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_value_chain_register_upload

                    #region ben_value_chain_register_actor_upload
                    case "ben_value_chain_register_actor_upload":
                        strSQL = "INSERT INTO ben_value_chain_register_actor " +
                            "(vcra_id, vcra_commodity, vcra_bds_service, " +
                            "vcra_id_price, vcra_id_qty, vcra_id_revenue, " +
                            "vcra_pb_price, vcra_pb_qty, vcra_pb_revenue, " +
                            "hhm_id, vcr_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "{3}, {4}, {5}, " +
                            "{6}, {7}, {8}, " +
                            "'{9}', '{10}', " +
                            "'{11}', '{12}', " +
                            "'{13}', '{14}', " +
                            "'{15}', {16}, {17},'{18}') ";
                        strSQL = string.Format(strSQL, dr["vcra_id"].ToString(), utilFormatting.StringForSQL(dr["vcra_commodity"].ToString()), utilFormatting.StringForSQL(dr["vcra_bds_service"].ToString()),
                            dr["vcra_id_price"].ToString(), dr["vcra_id_qty"].ToString(), dr["vcra_id_revenue"].ToString(),
                            dr["vcra_pb_price"].ToString(), dr["vcra_pb_qty"].ToString(), dr["vcra_pb_revenue"].ToString(),
                            dr["hhm_id"].ToString(), dr["vcr_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion ben_value_chain_register_actor_upload

                    #region drm_enrollment_upload
                    case "drm_enrollment_upload":
                        strSQL = "INSERT INTO drm_enrollment " +
                            "(de_id, " +
                            "dst_id, flt_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', " +
                            "'{3}', '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', {8}, {9},'{10}') ";
                        strSQL = string.Format(strSQL, dr["de_id"].ToString(),
                            dr["dst_id"].ToString(), dr["flt_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_enrollment_upload

                    #region drm_enrollment_member_upload
                    case "drm_enrollment_member_upload":
                        strSQL = "INSERT INTO drm_enrollment_member " +
                            "(dem_id, dem_sn, " +
                            "de_id, dm_id, est_id, pst_id, sst_id, " +
                            "yn_id_disability, yn_id_given_birth, yn_id_married, " +
                            "yn_id_partner, yn_id_pregnant, yn_id_ts, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', " +
                            "'{10}', '{11}', '{12}', " +
                            "'{13}', '{14}', " +
                            "'{15}', '{16}', " +
                            "'{17}', {18}, {19},'{10}') ";
                        strSQL = string.Format(strSQL, dr["dem_id"].ToString(), utilFormatting.StringForSQL(dr["dem_sn"].ToString()),
                            dr["de_id"].ToString(), dr["dm_id"].ToString(), dr["est_id"].ToString(), dr["pst_id"].ToString(), dr["sst_id"].ToString(),
                            dr["yn_id_disability"].ToString(), dr["yn_id_given_birth"].ToString(), dr["yn_id_married"].ToString(),
                            dr["yn_id_partner"].ToString(), dr["yn_id_pregnant"].ToString(), dr["yn_id_ts"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_enrollment_member_upload

                    #region drm_enrollment_member_segment_upload
                    case "drm_enrollment_member_segment_upload":
                        strSQL = "INSERT INTO drm_enrollment_member_segment " +
                            "(dems_id, dem_id, sgm_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id ) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["dems_id"].ToString(), dr["dem_id"].ToString(), dr["sgm_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_enrollment_member_segment_upload

                    #region drm_enrollment_member_visit_upload
                    case "drm_enrollment_member_visit_upload":
                        strSQL = "INSERT INTO drm_enrollment_member_visit " +
                            "(demv_id, demv_comment, demv_referral, " +
                            "demv_date, " +
                            "dem_id, vst_id, " +
                            "yn_id_anc, yn_id_art, yn_id_cmnc, " +
                            "yn_id_condom_promotion, yn_id_contraceptive_mix, yn_id_hiv_testing, " +
                            "yn_id_parenting_program, yn_id_post_violence_care, yn_id_school_based_prevention, " +
                            "yn_id_social_economic, yn_id_vmmc, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', '{5}', " +
                            "'{6}', '{7}', '{8}', " +
                            "'{9}', '{10}', '{11}', " +
                            "'{12}', '{13}', '{14}', " +
                            "'{15}', '{16}', " +
                            "'{17}', '{18}', " +
                            "'{19}', '{20}', " +
                            "'{21}', {22}, {23},'{24}') ";
                        strSQL = string.Format(strSQL, dr["demv_id"].ToString(), utilFormatting.StringForSQL(dr["demv_comment"].ToString()), utilFormatting.StringForSQL(dr["demv_referral"].ToString()),
                            Convert.ToDateTime(dr["demv_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["dem_id"].ToString(), dr["vst_id"].ToString(),
                            dr["yn_id_anc"].ToString(), dr["yn_id_art"].ToString(), dr["yn_id_cmnc"].ToString(),
                            dr["yn_id_condom_promotion"].ToString(), dr["yn_id_contraceptive_mix"].ToString(), dr["yn_id_hiv_testing"].ToString(),
                            dr["yn_id_parenting_program"].ToString(), dr["yn_id_post_violence_care"].ToString(), dr["yn_id_school_based_prevention"].ToString(),
                            dr["yn_id_social_economic"].ToString(), dr["yn_id_vmmc"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_enrollment_member_visit_upload

                    #region drm_htc_register_upload
                    case "drm_htc_register_upload":
                        strSQL = "INSERT INTO drm_htc_register " +
                            "(dhr_id, " +
                            "dhr_result_01, dhr_result_01_date, " +
                            "dhr_result_02, dhr_result_02_date, " +
                            "dhr_result_03, dhr_result_03_date, " +
                            "dhr_result_04, dhr_result_04_date, " +
                            "dhr_result_05, dhr_result_05_date, " +
                            "dm_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', " +
                            "'{3}', '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', '{10}', " +
                            "'{11}', " +
                            "'{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', {17}, {18},'{19}') ";
                        strSQL = string.Format(strSQL, dr["dhr_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dhr_result_01"].ToString()), Convert.ToDateTime(dr["dhr_result_01_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["dhr_result_02"].ToString()), Convert.ToDateTime(dr["dhr_result_02_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["dhr_result_03"].ToString()), Convert.ToDateTime(dr["dhr_result_03_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["dhr_result_04"].ToString()), Convert.ToDateTime(dr["dhr_result_04_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["dhr_result_05"].ToString()), Convert.ToDateTime(dr["dhr_result_05_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["dm_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_htc_register_upload

                    #region drm_member_upload
                    case "drm_member_upload":
                        strSQL = "INSERT INTO drm_member " +
                            "(dm_id, " +
                            "dm_first_name, dm_last_name, dm_id_no, " +
                            "dm_village, dm_phone, dm_status_reason, " +
                            "dm_active, " +
                            "dm_dob, dm_registration,  " +
                            "etp_id, hhm_id, mtp_id, wrd_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', '{3}', " +
                            "'{4}', '{5}', '{6}', " +
                            "{7}, " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', '{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', '{17}', " +
                            "'{18}', {19}, {20},'{21}') ";
                        strSQL = string.Format(strSQL, dr["dm_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dm_first_name"].ToString()), utilFormatting.StringForSQL(dr["dm_last_name"].ToString()), utilFormatting.StringForSQL(dr["dm_id_no"].ToString()),
                            utilFormatting.StringForSQL(dr["dm_village"].ToString()), utilFormatting.StringForSQL(dr["dm_phone"].ToString()), utilFormatting.StringForSQL(dr["dm_status_reason"].ToString()),
                            Convert.ToInt32(Convert.ToBoolean(dr["dm_active"])),
                            Convert.ToDateTime(dr["dm_dob"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["dm_registration"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["etp_id"].ToString(), dr["hhm_id"].ToString(), dr["mtp_id"].ToString(), dr["wrd_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_member_upload

                    #region drm_partner_upload
                    case "drm_partner_upload":
                        strSQL = "INSERT INTO drm_partner " +
                            "(dp_id, dp_first_name, dp_last_name, dm_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', '{3}', " +
                            "'{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', {9}, {10},'{11}') ";
                        strSQL = string.Format(strSQL, dr["dp_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dp_first_name"].ToString()), utilFormatting.StringForSQL(dr["dp_last_name"].ToString()), dr["dm_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_partner_upload

                    #region drm_partner_tracking_upload
                    case "drm_partner_tracking_upload":
                        strSQL = "INSERT INTO drm_partner_tracking " +
                            "(dpt_id, dpt_date, " +
                            "dpt_dptp_other, dpt_phone, " +
                            "dpt_address, dpt_service, " +
                            "dp_id, dptp_id, hst_id, " +
                            "yn_id_traced, ynd_id_vmmc, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', " +
                            "'{2}', '{3}', " +
                            "'{4}', '{5}', " +
                            "'{6}', '{7}', '{8}', " +
                            "'{9}', '{10}', " +
                            "'{11}', '{12}', " +
                            "'{13}', '{14}', " +
                            "'{15}', {16}, {17},'{18}') ";
                        strSQL = string.Format(strSQL, dr["dpt_id"].ToString(),
                            Convert.ToDateTime(dr["dpt_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["dpt_dptp_other"].ToString()), utilFormatting.StringForSQL(dr["dpt_phone"].ToString()),
                            utilFormatting.StringForSQL(dr["dpt_address"].ToString()), utilFormatting.StringForSQL(dr["dpt_service"].ToString()),
                            dr["dp_id"].ToString(), dr["dptp_id"].ToString(), dr["hst_id"].ToString(),
                            dr["yn_id_traced"].ToString(), dr["ynd_id_vmmc"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_partner_tracking_upload

                    #region drm_partner_tracking_service_upload
                    case "drm_partner_tracking_service_upload":
                        strSQL = "INSERT INTO drm_partner_tracking_service " +
                            "(dpts_id, dpt_id, dsrv_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id ) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["dpts_id"].ToString(), dr["dpt_id"].ToString(), dr["dsrv_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_partner_tracking_service

                    #region drm_post_violence_care_upload
                    case "drm_post_violence_care_upload":
                        strSQL = "INSERT INTO drm_post_violence_care " +
                            "(dpvc_id, " +
                            "flt_id, sct_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', " +
                            "'{3}', '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', {8}, {9},'{10}') ";
                        strSQL = string.Format(strSQL, dr["dpvc_id"].ToString(),
                            dr["flt_id"].ToString(), dr["sct_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_post_violence_care_upload

                    #region drm_post_violence_care_line_upload
                    case "drm_post_violence_care_line_upload":
                        strSQL = "INSERT INTO drm_post_violence_care_line " +
                            "(dpvcl_id, " +
                            "dpvcl_referred_from, dpvcl_date, " +
                            "dm_id, dpvc_id, gbv_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', " +
                            "'{3}', '{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["dpvcl_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dpvcl_referred_from"].ToString()), Convert.ToDateTime(dr["dpvcl_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["dm_id"].ToString(), dr["dpvc_id"].ToString(), dr["gbv_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_post_violence_care_line_upload

                    #region drm_post_violence_care_line_dreams_service_upload
                    case "drm_post_violence_care_line_dreams_service_upload":
                        strSQL = "INSERT INTO drm_post_violence_care_line_dreams_service " +
                            "(dpvclds_id, dpvcl_id, dso_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["dpvclds_id"].ToString(), dr["dpvcl_id"].ToString(), dr["dso_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_post_violence_care_line_dreams_service_upload

                    #region drm_post_violence_care_line_service_upload
                    case "drm_post_violence_care_line_service_upload":
                        strSQL = "INSERT INTO drm_post_violence_care_line_service " +
                            "(dpvcls_id, dpvcl_id, pvcs_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["dpvcls_id"].ToString(), dr["dpvcl_id"].ToString(), dr["pvcs_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_post_violence_care_line_service_upload

                    #region drm_sinovuyo_missed_session_upload
                    case "drm_sinovuyo_missed_session_upload":
                        strSQL = "INSERT INTO drm_sinovuyo_missed_session " +
                            "(dsms_id, dsms_contact, " +
                            "dsms_date, dsms_date_followup, " +
                            "dsms_action_other, dsms_followup_other, dsms_followup_method_other, " +
                            "dm_id, dsa_id, dsf_id, dsfm_id, yn_id_followup, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', " +
                            "'{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', '{10}', '{11}', " +
                            "'{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', {17}, {18},'{19}') ";
                        strSQL = string.Format(strSQL, dr["dsms_id"].ToString(), utilFormatting.StringForSQL(dr["dsms_contact"].ToString()),
                            Convert.ToDateTime(dr["dsms_date"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["dsms_date_followup"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["dsms_action_other"].ToString()), utilFormatting.StringForSQL(dr["dsms_followup_other"].ToString()), utilFormatting.StringForSQL(dr["dsms_followup_method_other"].ToString()),
                            dr["dm_id"].ToString(), dr["dsa_id"].ToString(), dr["dsf_id"].ToString(), dr["dsfm_id"].ToString(), dr["yn_id_followup"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_sinovuyo_missed_session_upload

                    #region drm_sinovuyo_register_upload
                    case "drm_sinovuyo_register_upload":
                        strSQL = "INSERT INTO drm_sinovuyo_register " +
                            "(dsr_id, " +
                            "dsr_facilitator, dsr_group, dsr_village, " +
                            "dsr_date, " +
                            "wrd_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', '{3}', " +
                            "'{4}', " +
                            "'{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["dsr_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dsr_facilitator"].ToString()), utilFormatting.StringForSQL(dr["dsr_group"].ToString()), utilFormatting.StringForSQL(dr["dsr_village"].ToString()),
                            Convert.ToDateTime(dr["dsr_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["wrd_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_sinovuyo_register_upload

                    #region drm_sinovuyo_register_line_upload
                    case "drm_sinovuyo_register_line_upload":
                        strSQL = "INSERT INTO drm_sinovuyo_register_line " +
                            "(dsrl_id, " +
                            "dsrl_contact, dm_id, dsr_id, " +
                            "pca_id_01, pca_id_02, pca_id_03, " +
                            "pca_id_04, pca_id_05, pca_id_06, " +
                            "pca_id_07, pca_id_08, pca_id_09, " +
                            "pca_id_10, pca_id_11, pca_id_12, " +
                            "pca_id_13, pca_id_14, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', '{3}', " +
                            "'{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', " +
                            "'{10}', '{11}', '{12}', " +
                            "'{13}', '{14}', '{15}', " +
                            "'{16}', '{17}', " +
                            "'{18}', '{19}', " +
                            "'{20}', '{21}', " +
                            "'{22}', {23}, {24},'{25}') ";
                        strSQL = string.Format(strSQL, dr["dsrl_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dsrl_contact"].ToString()), dr["dm_id"].ToString(), dr["dsr_id"].ToString(),
                            dr["pca_id_01"].ToString(), dr["pca_id_02"].ToString(), dr["pca_id_03"].ToString(),
                            dr["pca_id_04"].ToString(), dr["pca_id_05"].ToString(), dr["pca_id_06"].ToString(),
                            dr["pca_id_07"].ToString(), dr["pca_id_08"].ToString(), dr["pca_id_09"].ToString(),
                            dr["pca_id_10"].ToString(), dr["pca_id_11"].ToString(), dr["pca_id_12"].ToString(),
                            dr["pca_id_13"].ToString(), dr["pca_id_14"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_sinovuyo_register_line_upload

                    #region drm_stepping_stones_missed_session_upload
                    case "drm_stepping_stones_missed_session_upload":
                        strSQL = "INSERT INTO drm_stepping_stones_missed_session " +
                            "(dssms_id, dssms_contact, " +
                            "dssms_date, dssms_date_followup, " +
                            "dssms_action_other, dssms_followup_other, dssms_followup_method_other, " +
                            "dm_id, dsa_id, dsf_id, dsfm_id, yn_id_followup, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', " +
                            "'{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', '{10}', '{11}', " +
                            "'{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', {17}, {18},'{19}') ";
                        strSQL = string.Format(strSQL, dr["dssms_id"].ToString(), utilFormatting.StringForSQL(dr["dssms_contact"].ToString()),
                            Convert.ToDateTime(dr["dssms_date"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["dssms_date_followup"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["dssms_action_other"].ToString()), utilFormatting.StringForSQL(dr["dssms_followup_other"].ToString()), utilFormatting.StringForSQL(dr["dssms_followup_method_other"].ToString()),
                            dr["dm_id"].ToString(), dr["dsa_id"].ToString(), dr["dsf_id"].ToString(), dr["dsfm_id"].ToString(), dr["yn_id_followup"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_stepping_stones_missed_session_upload

                    #region drm_stepping_stones_register_upload
                    case "drm_stepping_stones_register_upload":
                        strSQL = "INSERT INTO drm_stepping_stones_register " +
                            "(dssr_id, " +
                            "dssr_facilitator, dssr_group, dssr_village, " +
                            "dssr_date, " +
                            "wrd_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', '{3}', " +
                            "'{4}', " +
                            "'{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["dssr_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dssr_facilitator"].ToString()), utilFormatting.StringForSQL(dr["dssr_group"].ToString()), utilFormatting.StringForSQL(dr["dssr_village"].ToString()),
                            Convert.ToDateTime(dr["dssr_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["wrd_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_stepping_stones_register_upload

                    #region drm_stepping_stones_register_line_upload
                    case "drm_stepping_stones_register_line_upload":
                        strSQL = "INSERT INTO drm_stepping_stones_register_line " +
                            "(dssrl_id, " +
                            "dssrl_contact, dm_id, dssr_id, " +
                            "yn_id_m1, yn_id_m2, yn_id_m3, " +
                            "yn_id_sa, yn_id_sb, yn_id_sc, " +
                            "yn_id_sd, yn_id_se, yn_id_sf, " +
                            "yn_id_sg, yn_id_sh, yn_id_si, " +
                            "yn_id_sj, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', '{3}', " +
                            "'{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', " +
                            "'{10}', '{11}', '{12}', " +
                            "'{13}', '{14}', '{15}', " +
                            "'{16}', " +
                            "'{17}', '{18}', " +
                            "'{19}', '{20}', " +
                            "'{21}', {22}, {23},'{24}') ";
                        strSQL = string.Format(strSQL, dr["dssrl_id"].ToString(),
                            utilFormatting.StringForSQL(dr["dssrl_contact"].ToString()), dr["dm_id"].ToString(), dr["dssr_id"].ToString(),
                            dr["yn_id_m1"].ToString(), dr["yn_id_m2"].ToString(), dr["yn_id_m3"].ToString(),
                            dr["yn_id_sa"].ToString(), dr["yn_id_sb"].ToString(), dr["yn_id_sc"].ToString(),
                            dr["yn_id_sd"].ToString(), dr["yn_id_se"].ToString(), dr["yn_id_sf"].ToString(),
                            dr["yn_id_sg"].ToString(), dr["yn_id_sh"].ToString(), dr["yn_id_si"].ToString(),
                            dr["yn_id_sj"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion drm_stepping_stones_register_line_upload

                    #region hh_home_visit_upload
                    case "hh_home_visit_upload":
                        strSQL = "INSERT INTO hh_home_visit " +
                            "(hv_id, hv_number_of_children, " +
                            "hv_date, hv_previous_visit_date, " +
                            "hv_previous_visit_purpose, hv_previous_visit_service, " +
                            "hv_actions, hv_findings, hv_next_steps, " +
                            "hv_girl_name, hv_girl_age, " +
                            "hv_girl_education_type, " +
                            "edu_id, hh_id, hhm_id, swk_id, " +
                            "yn_id_consumption_program, yn_id_girl_education_support, yn_id_improvement_plan, " +
                            "yn_id_previous_visit, yn_id_referral, yn_id_referral_completed, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', {1}, " +
                            "'{2}', '{3}', " +
                            "'{4}', '{5}', " +
                            "'{6}', '{7}', '{8}', " +
                            "'{9}', {10}, " +
                            "'{11}', " +
                            "'{12}', '{13}', '{14}', '{15}', " +
                            "'{16}', '{17}', '{18}', " +
                            "'{19}', '{20}', '{21}', " +
                            "'{22}', '{23}', " +
                            "'{24}', '{25}', " +
                            "'{26}', {27}, {28},'{29}') ";
                        strSQL = string.Format(strSQL, dr["hv_id"].ToString(), dr["hv_number_of_children"].ToString(),
                            Convert.ToDateTime(dr["hv_date"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["hv_previous_visit_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["hv_previous_visit_purpose"].ToString()), utilFormatting.StringForSQL(dr["hv_previous_visit_service"].ToString()),
                            utilFormatting.StringForSQL(dr["hv_actions"].ToString()), utilFormatting.StringForSQL(dr["hv_findings"].ToString()), utilFormatting.StringForSQL(dr["hv_next_steps"].ToString()),
                            utilFormatting.StringForSQL(dr["hv_girl_name"].ToString()), dr["hv_girl_age"].ToString(),
                            utilFormatting.StringForSQL(dr["hv_girl_education_type"].ToString()),
                            dr["edu_id"].ToString(), dr["hh_id"].ToString(), dr["hhm_id"].ToString(), dr["swk_id"].ToString(),
                            dr["yn_id_consumption_program"].ToString(), dr["yn_id_girl_education_support"].ToString(), dr["yn_id_improvement_plan"].ToString(),
                            dr["yn_id_previous_visit"].ToString(), dr["yn_id_referral"].ToString(), dr["yn_id_referral_completed"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_home_visit_upload

                    #region hh_home_visit_service_upload
                    case "hh_home_visit_service_upload":
                        strSQL = "INSERT INTO hh_home_visit_service " +
                            "(hvs_id, hv_id, shv_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["hvs_id"].ToString(), dr["hv_id"].ToString(), dr["shv_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_home_visit_service_upload

                    #region hh_home_visit_service_previous_upload
                    case "hh_home_visit_service_previous_upload":
                        strSQL = "INSERT INTO hh_home_visit_service_previous " +
                            "(hvsp_id, hv_id, shvp_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["hvsp_id"].ToString(), dr["hv_id"].ToString(), dr["shvp_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_home_visit_service_previous_upload

                    #region hh_household_upload
                    case "hh_household_upload":
                        strSQL = "INSERT INTO hh_household " +
                            "(hh_id, hh_code, " +
                            "hh_status_reason, hh_tel, hh_village, " +
                            "hhs_id, hhsr_id, swk_id, wrd_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', " +
                            "'{5}', '{6}', '{7}', '{8}', " +
                            "'{9}', '{10}', " +
                            "'{11}', '{12}', " +
                            "'{13}', {14}, {15},'{16}') ";
                        //DataTable dttest = dt;
                        strSQL = string.Format(strSQL, dr["hh_id"].ToString(), utilFormatting.StringForSQL(dr["hh_code"].ToString()),
                            utilFormatting.StringForSQL(dr["hh_status_reason"].ToString()), utilFormatting.StringForSQL(dr["hh_tel"].ToString()), utilFormatting.StringForSQL(dr["hh_village"].ToString()),
                            dr["hhs_id"].ToString(), dr["hhsr_id"].ToString(), dr["swk_id"].ToString(), dr["wrd_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_household_upload

                    #region hh_household_assessment_upload
                    case "hh_household_assessment_upload":
                        strSQL = "INSERT INTO hh_household_assessment " +
                            "(hha_id, hha_comments, " +
                            "hha_date, " +
                            "hha_num_of_meals, " +
                            "hh_id, hhm_id, icc_id, " +
                            "ics_id, osn_id_disagreement, swk_id, " +
                            "yn_id_child_separation, yn_id_financial_savings, " +
                            "yn_id_food_body_building, yn_id_food_energy, yn_id_food_protective, yn_id_water, " +
                            "ynna_id_expenses_food, ynna_id_expenses_health, ynna_id_expenses_school, " +
                            "ynns_id_assets, yns_id_latrine, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id,[swk_phone],[caregiver_phone],[_18_years_male],[_18_years_female],[below_18_male],[below_18_female],[total_hhm_male],[total_hhm_female],[yn_child_headed]" +
                           ",[yn_hh_disabled],[yn_hhm_sick],[yn_hhm_employed],[yn_pay_unexpected_expense],[yn_function_tp_means],[yn_hhm_vocational_skills],[yn_domestic_animals],[yn_hh_access_to_land]" + 
                           ",[hh_food_source],[hhm_go_hungry_past_month],[yn_caregiver_knows_hiv_status],[yn_children_tested],[yn_eligible_child_on_treatment],[yn_hh_access_water]" + 
                           ",[yn_hhm_mosquito_net] ,[yn_hh_access_public_health_facility],[yn_ob_clean_compound],[yn_ob_drying_rack],[yn_ob_garbage_pit],[yn_ob_animal_house],[yn_ob_washing_facility]" + 
                           ",[hh_stable_shelter],[ynna_children_go_to_school],[total_hh_children_not_go_to_school],[yn_children_sad_unhappy],[yn_cp_repeated_abuse],[yn_cp_child_labour],[yn_cp_sexually_abused]" + 
                           ",[yn_cp_stigmatised],[hhs_id_visit_from_volunteer],[hhs_id_financial_support],[hhs_id_parenting_counsiling],[hhs_id_early_child_dev],[hhs_id_health_hygiene],[hhs_id_hiv_gbv_prevention]" + 
                           ",[hhs_id_nutrition_counsiling],[hhs_id_pre_post_partum],[hhs_id_hiv_testing],[hhs_id_couples_counsiling],[hhs_id_birth_certificate],[hhs_id_child_protection],[hhs_id_psychosocial]" + 
                           ",[hhs_id_food_security],[hhs_id_other],[hhs_id_none],[hhcs_id_savings_groups],[hhcs_id_parenting_program],[hhcs_id_govt_sage_program],[hhcs_id_other_cash_transfer],[hhcs_id_voluntary_hiv_testing]" +
                           ",[hhcs_id_food_security_nutrition],[hhcs_id_skills_employ_training],[hhcs_id_entrepreneurship_training],[hhcs_id_other] ,[hhcs_id_none],[hh_child_abuse_action],[yn_cp_conflict_with_law],[yn_cp_withheld_meal],[yn_cp_abusive_language]) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', '{13}', '{14}', '{15}', " +
                            "'{16}', '{17}', '{18}', " +
                            "'{19}', '{20}', " +
                            "'{21}', '{22}', " +
                            "'{23}', '{24}', " +
                            "'{25}', {26}, {27},'{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}'," +
                            "'{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}'," +
                        "'{57}','{58}','{59}','{60}','{61}','{62}','{63}','{64}','{65}','{66}','{67}','{68}','{69}','{70}','{71}','{72}','{73}','{74}','{75}','{76}','{77}','{78}','{79}','{80}'," +
                        "'{81}','{82}','{83}','{84}','{85}','{86}','{87}','{88}','{89}','{90}','{91}','{92}','{93}','{94}','{95}','{96}') ";
                        strSQL = string.Format(strSQL, dr["hha_id"].ToString(), utilFormatting.StringForSQL(dr["hha_comments"].ToString()),
                            Convert.ToDateTime(dr["hha_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["hha_num_of_meals"].ToString(),
                            dr["hh_id"].ToString(), dr["hhm_id"].ToString(), dr["icc_id"].ToString(),
                            dr["ics_id"].ToString(), dr["osn_id_disagreement"].ToString(), dr["swk_id"].ToString(),
                            dr["yn_id_child_separation"].ToString(), dr["yn_id_financial_savings"].ToString(),
                            dr["yn_id_food_body_building"].ToString(), dr["yn_id_food_energy"].ToString(), dr["yn_id_food_protective"].ToString(), dr["yn_id_water"].ToString(),
                            dr["ynna_id_expenses_food"].ToString(), dr["ynna_id_expenses_health"].ToString(), dr["ynna_id_expenses_school"].ToString(),
                            dr["ynns_id_assets"].ToString(), dr["yns_id_latrine"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString(),

                            dr["swk_phone"].ToString(), dr["caregiver_phone"].ToString(), dr["_18_years_male"].ToString(), dr["_18_years_female"].ToString(), dr["below_18_male"].ToString(), dr["below_18_female"].ToString(), dr["total_hhm_male"].ToString(),
                            dr["total_hhm_female"].ToString(), dr["yn_child_headed"].ToString(), dr["yn_hh_disabled"].ToString(), dr["yn_hhm_sick"].ToString(), dr["yn_hhm_employed"].ToString(), dr["yn_pay_unexpected_expense"].ToString(), dr["yn_function_tp_means"].ToString(),
                            dr["yn_hhm_vocational_skills"].ToString(), dr["yn_domestic_animals"].ToString(), dr["yn_hh_access_to_land"].ToString(), dr["hh_food_source"].ToString(), dr["hhm_go_hungry_past_month"].ToString(), dr["yn_caregiver_knows_hiv_status"].ToString(), dr["yn_children_tested"].ToString(),
                            dr["yn_eligible_child_on_treatment"].ToString(), dr["yn_hh_access_water"].ToString(), dr["yn_hhm_mosquito_net"].ToString(), dr["yn_hh_access_public_health_facility"].ToString(), dr["yn_ob_clean_compound"].ToString(), dr["yn_ob_drying_rack"].ToString(), dr["yn_ob_garbage_pit"].ToString(),
                            dr["yn_ob_animal_house"].ToString(), dr["yn_ob_washing_facility"].ToString(), dr["hh_stable_shelter"].ToString(), dr["ynna_children_go_to_school"].ToString(), dr["total_hh_children_not_go_to_school"].ToString(), dr["yn_children_sad_unhappy"].ToString(), dr["yn_cp_repeated_abuse"].ToString(), dr["yn_cp_child_labour"].ToString(),
                            dr["yn_cp_sexually_abused"].ToString(), dr["yn_cp_stigmatised"].ToString(), dr["hhs_id_visit_from_volunteer"].ToString(), dr["hhs_id_financial_support"].ToString(), dr["hhs_id_parenting_counsiling"].ToString(), dr["hhs_id_early_child_dev"].ToString(), dr["hhs_id_health_hygiene"].ToString(),
                            dr["hhs_id_hiv_gbv_prevention"].ToString(), dr["hhs_id_nutrition_counsiling"].ToString(), dr["hhs_id_pre_post_partum"].ToString(), dr["hhs_id_hiv_testing"].ToString(), dr["hhs_id_couples_counsiling"].ToString(), dr["hhs_id_birth_certificate"].ToString(), dr["hhs_id_child_protection"].ToString(),
                            dr["hhs_id_psychosocial"].ToString(), dr["hhs_id_food_security"].ToString(), dr["hhs_id_other"].ToString(), dr["hhs_id_none"].ToString(), dr["hhcs_id_savings_groups"].ToString(), dr["hhcs_id_parenting_program"].ToString(), dr["hhcs_id_govt_sage_program"].ToString(),
                            dr["hhcs_id_other_cash_transfer"].ToString(), dr["hhcs_id_voluntary_hiv_testing"].ToString(), dr["hhcs_id_food_security_nutrition"].ToString(), dr["hhcs_id_skills_employ_training"].ToString(), dr["hhcs_id_entrepreneurship_training"].ToString(), dr["hhcs_id_other"].ToString(), dr["hhcs_id_none"].ToString(),
                            dr["hh_child_abuse_action"].ToString(),dr["yn_cp_conflict_with_law"].ToString(), dr["yn_cp_withheld_meal"].ToString(), dr["yn_cp_abusive_language"].ToString());
                        break;
                    #endregion hh_household_assessment_upload

                    #region hh_household_assessment_member_upload
                    case "hh_household_assessment_member_upload":
                        strSQL = "INSERT INTO hh_household_assessment_member " +
                            "(ham_id, " +
                            "ham_first_name, ham_last_name, " +
                            "ham_year_of_birth, " +
                            "dtp_id, edu_id, gnd_id, " +
                            "hha_id, hhm_id, hst_id, mst_id, " +
                            "prf_id, prt_id, " +
                            "yn_id_art, yn_id_birth_registration, yn_id_caregiver, " +
                            "yn_id_disability, yn_id_given_birth, yn_id_hoh, " +
                            "yn_id_immun, yn_id_pregnant, yn_id_school, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id,yn_attained_vocational_skill) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', '{10}', " +
                            "'{11}', '{12}', " +
                            "'{13}', '{14}', '{15}', " +
                            "'{16}', '{17}', '{18}', " +
                            "'{19}', '{20}', '{21}', " +
                            "'{22}', '{23}', " +
                            "'{24}', '{25}', " +
                            "'{26}', {27}, {28},'{29}','{30}') ";
                        strSQL = string.Format(strSQL, dr["ham_id"].ToString(),
                            utilFormatting.StringForSQL(dr["ham_first_name"].ToString()), utilFormatting.StringForSQL(dr["ham_last_name"].ToString()),
                            dr["ham_year_of_birth"].ToString(),
                            dr["dtp_id"].ToString(), dr["edu_id"].ToString(), dr["gnd_id"].ToString(),
                            dr["hha_id"].ToString(), dr["hhm_id"].ToString(), dr["hst_id"].ToString(), dr["mst_id"].ToString(),
                            dr["prf_id"].ToString(), dr["prt_id"].ToString(),
                            dr["yn_id_art"].ToString(), dr["yn_id_birth_registration"].ToString(), dr["yn_id_caregiver"].ToString(),
                            dr["yn_id_disability"].ToString(), dr["yn_id_given_birth"].ToString(), dr["yn_id_hoh"].ToString(),
                            dr["yn_id_immun"].ToString(), dr["yn_id_pregnant"].ToString(), dr["yn_id_school"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString(), dr["yn_attained_vocational_skill"].ToString());
                        break;
                    #endregion hh_household_assessment_member_upload

                    #region hh_household_member_upload
                    case "hh_household_member_upload":
                        strSQL = "INSERT INTO hh_household_member " +
                            "(hhm_id, " +
                            "hhm_first_name, hhm_last_name, " +
                            "hhm_number, hhm_year_of_birth, " +
                            "dtp_id, edu_id, gnd_id, " +
                            "hh_id, hst_id, mst_id, " +
                            "prf_id, prt_id, " +
                            "yn_id_art, yn_id_birth_registration, yn_id_caregiver, " +
                            "yn_id_disability, yn_id_given_birth, yn_id_hoh, " +
                            "yn_id_immun, yn_id_pregnant, yn_id_school, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', " +
                            "'{3}', '{4}', " +
                            "'{5}', '{6}', '{7}', " +
                            "'{8}', '{9}', '{10}', " +
                            "'{11}', '{12}', " +
                            "'{13}', '{14}', '{15}', " +
                            "'{16}', '{17}', '{18}', " +
                            "'{19}', '{20}', '{21}', " +
                            "'{22}', '{23}', " +
                            "'{24}', '{25}', " +
                            "'{26}', {27}, {28},'{29}') ";

                        strSQL = string.Format(strSQL, dr["hhm_id"].ToString(),
                            utilFormatting.StringForSQL(dr["hhm_first_name"].ToString()), utilFormatting.StringForSQL(dr["hhm_last_name"].ToString()),
                            dr["hhm_number"].ToString(), dr["hhm_year_of_birth"].ToString(),
                            dr["dtp_id"].ToString(), dr["edu_id"].ToString(), dr["gnd_id"].ToString(),
                            dr["hh_id"].ToString(), dr["hst_id"].ToString(), dr["mst_id"].ToString(),
                            dr["prf_id"].ToString(), dr["prt_id"].ToString(),
                            dr["yn_id_art"].ToString(), dr["yn_id_birth_registration"].ToString(), dr["yn_id_caregiver"].ToString(),
                            dr["yn_id_disability"].ToString(), dr["yn_id_given_birth"].ToString(), dr["yn_id_hoh"].ToString(),
                            dr["yn_id_immun"].ToString(), dr["yn_id_pregnant"].ToString(), dr["yn_id_school"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_household_member_upload 

                    #region hh_household_home_visit_upload
                    case "hh_household_home_visit_upload":
                        strSQL = "INSERT INTO hh_household_home_visit " +
                            "(hhv_id, " +
                            "hhv_date, hhv_date_next_visit, " +
                            "hhv_household_income, " +
                            "hhv_comments, hhv_next_steps, " +
                            "hhv_swk_code, hhv_visitor_tel, " +
                            "am_id, hvhs_id, hvr_id, hh_id, hhm_id, hnr_id_visitor, swk_id, swk_id_visitor, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "'{1}', '{2}', " +
                            "{3}, " +
                            "'{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', " +
                            "'{16}', '{17}', " +
                            "'{18}', '{19}', " +
                            "'{20}', {21}, {22},'{23}') ";
                        strSQL = string.Format(strSQL, dr["hhv_id"].ToString(),
                            Convert.ToDateTime(dr["hhv_date"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["hhv_date_next_visit"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["hhv_household_income"].ToString(),
                            utilFormatting.StringForSQL(dr["hhv_comments"].ToString()), utilFormatting.StringForSQL(dr["hhv_next_steps"].ToString()),
                            utilFormatting.StringForSQL(dr["hhv_swk_code"].ToString()), utilFormatting.StringForSQL(dr["hhv_visitor_tel"].ToString()),
                            dr["am_id"].ToString(), dr["hvhs_id"].ToString(), dr["hvr_id"].ToString(), dr["hh_id"].ToString(),
                            dr["hhm_id"].ToString(), dr["hnr_id_visitor"].ToString(), dr["swk_id"].ToString(), dr["swk_id_visitor"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_household_home_visit

                    #region hh_household_home_visit_member_upload
                    case "hh_household_home_visit_member_upload":
                        strSQL = "INSERT INTO hh_household_home_visit_member " +
                            "(hhvm_id, hhm_id, hhv_id, hst_id, " +
                            "yn_id_hhm_active, " +
                            "yn_id_edu_sensitised, yn_id_es_aflateen, yn_id_es_agro, yn_id_es_apprenticeship, yn_id_es_silc, " +
                            "yn_id_fsn_nutrition, yn_id_fsn_referred, yn_id_fsn_wash, " +
                            "ynna_id_edu_enrolled, ynna_id_edu_support, ynna_id_fsn_education, ynna_id_fsn_support, " +
                            "ynna_id_hhp_adhering, ynna_id_hhp_art, ynna_id_hhp_referred, " +
                            "ynna_id_pro_birth_certificate, ynna_id_pro_birth_registration, ynna_id_pro_child_abuse, " +
                            "ynna_id_pro_child_labour, ynna_id_pro_reintegrated, " +
                            "ynna_id_ps_parenting, ynna_id_ps_support, ynna_id_ps_violence, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id,yn_id_es_caregiver_group,ynna_id_edu_attend_school_regularly,ynna_id_es_other_lending_group) " +
                            "VALUES ('{0}', '{1}', '{2}', '{3}', " +
                            "'{4}', " +
                            "'{5}', '{6}', '{7}', '{8}', '{9}', " +
                            "'{10}', '{11}', '{12}', " +
                            "'{13}', '{14}', '{15}', '{16}', " +
                            "'{17}', '{18}', '{19}', " +
                            "'{20}', '{21}', '{22}', " +
                            "'{23}', '{24}', " +
                            "'{25}', '{26}', '{27}', " +
                            "'{28}', '{29}', " +
                            "'{30}', '{31}', " +
                            "'{32}', {33}, {34},'{35}','{36}','{37}','{38}') ";
                        strSQL = string.Format(strSQL, dr["hhvm_id"].ToString(), dr["hhm_id"].ToString(), dr["hhv_id"].ToString(), dr["hst_id"].ToString(),
                            dr["yn_id_hhm_active"].ToString(),
                            dr["yn_id_edu_sensitised"].ToString(), dr["yn_id_es_aflateen"].ToString(), dr["yn_id_es_agro"].ToString(), dr["yn_id_es_apprenticeship"].ToString(), dr["yn_id_es_silc"].ToString(),
                            dr["yn_id_fsn_nutrition"].ToString(), dr["yn_id_fsn_referred"].ToString(), dr["yn_id_fsn_wash"].ToString(),
                            dr["ynna_id_edu_enrolled"].ToString(), dr["ynna_id_edu_support"].ToString(), dr["ynna_id_fsn_education"].ToString(), dr["ynna_id_fsn_support"].ToString(),
                            dr["ynna_id_hhp_adhering"].ToString(), dr["ynna_id_hhp_art"].ToString(), dr["ynna_id_hhp_referred"].ToString(),
                            dr["ynna_id_pro_birth_certificate"].ToString(), dr["ynna_id_pro_birth_registration"].ToString(), dr["ynna_id_pro_child_abuse"].ToString(),
                            dr["ynna_id_pro_child_labour"].ToString(), dr["ynna_id_pro_reintegrated"].ToString(),
                            dr["ynna_id_ps_parenting"].ToString(), dr["ynna_id_ps_support"].ToString(), dr["ynna_id_ps_violence"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString(), dr["yn_id_es_caregiver_group"].ToString(), dr["ynna_id_edu_attend_school_regularly"].ToString(),
                            dr["ynna_id_es_other_lending_group"].ToString());
                        break;
                    #endregion hh_household_home_visit_member

                    #region hh_ovc_identification_prioritization_upload
                    case "hh_ovc_identification_prioritization_upload":
                        strSQL = "INSERT INTO hh_ovc_identification_prioritization " +
                            "(oip_id, oip_comments, " +
                            "oip_date, " +
                            "oip_18_above_female, oip_18_above_male, oip_18_below_female, " +
                            "oip_18_below_male, oip_hiv_adult, oip_hiv_children, " +
                            "oip_cp_month, oip_interviewer_tel, " +
                            "cso_id, hh_id, hhm_id, swk_id, " +
                            "yn_id_children, " +
                            "yn_id_cp_abuse, yn_id_cp_abuse_physical, yn_id_cp_abuse_sexual, " +
                            "yn_id_cp_marriage_teen_parent, yn_id_cp_neglected, yn_id_cp_no_birth_register, " +
                            "yn_id_cp_orphan, yn_id_cp_pregnancy, yn_id_cp_referred, " +
                            "yn_id_edu_referred, yn_id_es_child_headed, yn_id_es_disability, " +
                            "yn_id_es_employment, yn_id_es_expense, yn_id_es_referred, " +
                            "yn_id_fsn_meals, yn_id_fsn_malnourished, yn_id_fsn_referred, " +
                            "yn_id_hwss_hiv_positive, yn_id_hwss_hiv_status, yn_id_hwss_referred, " +
                            "yn_id_hwss_shelter, yn_id_hwss_water, " +
                            "yn_id_psbc_referred, yn_id_psbc_stigmatized, " +
                            "ynna_id_edu_missed_school, ynna_id_edu_not_enrolled, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id,ids_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', " +
                            "{3}, {4}, {5}, " +
                            "{6}, {7}, {8}, " +
                            "'{9}', '{10}', " +
                            "'{11}', '{12}', '{13}', '{14}', " +
                            "'{15}', " +
                            "'{16}', '{17}', '{18}', " +
                            "'{19}', '{20}', '{21}', " +
                            "'{22}', '{23}', '{24}', " +
                            "'{25}', '{26}', '{27}', " +
                            "'{28}', '{29}', '{30}', " +
                            "'{31}', '{32}', '{33}', " +
                            "'{34}', '{35}', '{36}', " +
                            "'{37}', '{38}', " +
                            "'{39}', '{40}', " +
                            "'{41}', '{42}', " +
                            "'{43}', '{44}', " +
                            "'{45}', '{46}', " +
                            "'{47}', {48}, {49},'{50}','{51}') ";
                        strSQL = string.Format(strSQL, dr["oip_id"].ToString(), utilFormatting.StringForSQL(dr["oip_comments"].ToString()),
                            Convert.ToDateTime(dr["oip_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["oip_18_above_female"].ToString(), dr["oip_18_above_male"].ToString(), dr["oip_18_below_female"].ToString(),
                            dr["oip_18_below_male"].ToString(), dr["oip_hiv_adult"].ToString(), dr["oip_hiv_children"].ToString(),
                            dr["oip_cp_month"].ToString(), utilFormatting.StringForSQL(dr["oip_interviewer_tel"].ToString()),
                            dr["cso_id"].ToString(), dr["hh_id"].ToString(), dr["hhm_id"].ToString(), dr["swk_id"].ToString(),
                            dr["yn_id_children"].ToString(),
                            dr["yn_id_cp_abuse"].ToString(), dr["yn_id_cp_abuse_physical"].ToString(), dr["yn_id_cp_abuse_sexual"].ToString(),
                            dr["yn_id_cp_marriage_teen_parent"].ToString(), dr["yn_id_cp_neglected"].ToString(), dr["yn_id_cp_no_birth_register"].ToString(),
                            dr["yn_id_cp_orphan"].ToString(), dr["yn_id_cp_pregnancy"].ToString(), dr["yn_id_cp_referred"].ToString(),
                            dr["yn_id_edu_referred"].ToString(), dr["yn_id_es_child_headed"].ToString(), dr["yn_id_es_disability"].ToString(),
                            dr["yn_id_es_employment"].ToString(), dr["yn_id_es_expense"].ToString(), dr["yn_id_es_referred"].ToString(),
                            dr["yn_id_fsn_meals"].ToString(), dr["yn_id_fsn_malnourished"].ToString(), dr["yn_id_fsn_referred"].ToString(),
                            dr["yn_id_hwss_hiv_positive"].ToString(), dr["yn_id_hwss_hiv_status"].ToString(), dr["yn_id_hwss_referred"].ToString(),
                            dr["yn_id_hwss_shelter"].ToString(), dr["yn_id_hwss_water"].ToString(),
                            dr["yn_id_psbc_referred"].ToString(), dr["yn_id_psbc_stigmatized"].ToString(),
                            dr["ynna_id_edu_missed_school"].ToString(), dr["ynna_id_edu_not_enrolled"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString(), dr["ids_id"].ToString());
                        break;
                    #endregion hh_ovc_identification_prioritization_upload

                    #region hh_referral_upload
                    case "hh_referral_upload":
                        strSQL = "INSERT INTO hh_referral " +
                            "(rfr_id, rfr_serial_no, rfr_ra_location, " +
                            "rfr_ra_tel, rfr_ra_email, " +
                            "rfr_ra_person_name, rfr_ra_person_title, " +
                            "rfr_ra_person_tel, rfr_ra_person_email, " +
                            "rfr_ra_date, " +
                            "rfr_cd_case_no, " +
                            "rfr_cd_nature, rfr_cd_perpetrator, rfr_cd_perpetrator_relationship, " +
                            "rfr_cd_date_occured, rfr_cd_other, " +
                            "rfr_cd_accompany_name, rfr_cd_accompany_tel, " +
                            "rfr_cd_accompany_email, rfr_cd_accompany_relationship, " +
                            "rfr_cd_guardian_name, rfr_cd_guardian_tel, rfr_cd_guardian_village, " +
                            "rfr_service_before, rfr_service_referral, rfr_service_discussion, " +
                            "rfr_ar_name, rfr_ar_location, rfr_ar_contact_name, " +
                            "rfr_ar_contact_tel, rfr_ar_contact_email, " +
                            "rfr_fb_agency_name, rfr_fb_person_name, rfr_fb_person_title, " +
                            "rfr_fb_location, rfr_fb_tel, rfr_fb_email, " +
                            "rfr_fb_date, " +
                            "rfr_fb_id_no, rfr_fb_case_no, " +
                            "rfr_fb_service, rfr_fb_other, " +
                            "hhm_id, prt_cso_id_ra, wrd_id_guardian, " +
                            "yn_id_discussion, yn_id_helpline, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', " +
                            "'{10}', " +
                            "'{11}', '{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', '{17}', " +
                            "'{18}', '{19}', " +
                            "'{20}', '{21}', '{22}', " +
                            "'{23}', '{24}', '{25}', " +
                            "'{26}', '{27}', '{28}', " +
                            "'{29}', '{30}', " +
                            "'{31}', '{32}', '{33}', " +
                            "'{34}', '{35}', '{36}', " +
                            "'{37}', " +
                            "'{38}', '{39}', " +
                            "'{40}', '{41}', " +
                            "'{42}', '{43}', '{44}', " +
                            "'{45}', '{46}', " +
                            "'{47}', '{48}', " +
                            "'{49}', '{50}', " +
                            "'{51}', {52}, {53},'{54}') ";
                        strSQL = string.Format(strSQL, dr["rfr_id"].ToString(), utilFormatting.StringForSQL(dr["rfr_serial_no"].ToString()), utilFormatting.StringForSQL(dr["rfr_ra_location"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_ra_tel"].ToString()), utilFormatting.StringForSQL(dr["rfr_ra_email"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_ra_person_name"].ToString()), utilFormatting.StringForSQL(dr["rfr_ra_person_title"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_ra_person_tel"].ToString()), utilFormatting.StringForSQL(dr["rfr_ra_person_email"].ToString()),
                            Convert.ToDateTime(dr["rfr_ra_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["rfr_cd_case_no"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_cd_nature"].ToString()), utilFormatting.StringForSQL(dr["rfr_cd_perpetrator"].ToString()), utilFormatting.StringForSQL(dr["rfr_cd_perpetrator_relationship"].ToString()),
                            Convert.ToDateTime(dr["rfr_cd_date_occured"]).ToString("dd MMM yyyy HH:mm:ss"), utilFormatting.StringForSQL(dr["rfr_cd_other"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_cd_accompany_name"].ToString()), utilFormatting.StringForSQL(dr["rfr_cd_accompany_tel"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_cd_accompany_email"].ToString()), utilFormatting.StringForSQL(dr["rfr_cd_accompany_relationship"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_cd_guardian_name"].ToString()), utilFormatting.StringForSQL(dr["rfr_cd_guardian_tel"].ToString()), utilFormatting.StringForSQL(dr["rfr_cd_guardian_village"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_service_before"].ToString()), utilFormatting.StringForSQL(dr["rfr_service_referral"].ToString()), utilFormatting.StringForSQL(dr["rfr_service_discussion"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_ar_name"].ToString()), utilFormatting.StringForSQL(dr["rfr_ar_location"].ToString()), utilFormatting.StringForSQL(dr["rfr_ar_contact_name"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_ar_contact_tel"].ToString()), utilFormatting.StringForSQL(dr["rfr_ar_contact_email"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_fb_agency_name"].ToString()), utilFormatting.StringForSQL(dr["rfr_fb_person_name"].ToString()), utilFormatting.StringForSQL(dr["rfr_fb_person_title"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_fb_location"].ToString()), utilFormatting.StringForSQL(dr["rfr_fb_tel"].ToString()), utilFormatting.StringForSQL(dr["rfr_fb_email"].ToString()),
                            Convert.ToDateTime(dr["rfr_fb_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            utilFormatting.StringForSQL(dr["rfr_fb_id_no"].ToString()), utilFormatting.StringForSQL(dr["rfr_fb_case_no"].ToString()),
                            utilFormatting.StringForSQL(dr["rfr_fb_service"].ToString()), utilFormatting.StringForSQL(dr["rfr_fb_other"].ToString()),
                            dr["hhm_id"].ToString(), dr["prt_cso_id_ra"].ToString(), dr["wrd_id_guardian"].ToString(),
                            dr["yn_id_discussion"].ToString(), dr["yn_id_helpline"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_referral_upload

                    #region hh_referral_service_provided_upload
                    case "hh_referral_service_provided_upload":
                        strSQL = "INSERT INTO hh_referral_service_provided " +
                            "(rsp_id, rfr_id, svp_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["rsp_id"].ToString(), dr["rfr_id"].ToString(), dr["svp_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_referral_service_provided_upload

                    #region hh_referral_service_referred_upload
                    case "hh_referral_service_referred_upload":
                        strSQL = "INSERT INTO hh_referral_service_referred " +
                            "(rsr_id, rfr_id, svr_id, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', " +
                            "'{4}', " +
                            "'{5}', {6}, {7},'{8}') ";
                        strSQL = string.Format(strSQL, dr["rsr_id"].ToString(), dr["rfr_id"].ToString(), dr["svr_id"].ToString(),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_referral_service_referred_upload

                    #region prt_alternative_care_panel_upload
                    case "prt_alternative_care_panel_upload":
                        strSQL = "INSERT INTO prt_alternative_care_panel " +
                            "(acp_id, acp_date, " +
                            "fy_id, prt_id, rgn_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', {10}, {11},'{12}') ";
                        strSQL = string.Format(strSQL, dr["acp_id"].ToString(), Convert.ToDateTime(dr["acp_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["fy_id"].ToString(), dr["prt_id"].ToString(), dr["rgn_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion prt_alternative_care_panel_upload

                    #region prt_alternative_care_panel_district_upload
                    case "prt_alternative_care_panel_district_upload":
                        strSQL = "INSERT INTO prt_alternative_care_panel_district " +
                            "(acpd_id, acpd_support_extended, " +
                            "acp_id, dst_id, yn_id_established, yn_id_functional, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["acpd_id"].ToString(), utilFormatting.StringForSQL(dr["acpd_support_extended"].ToString()),
                            dr["acp_id"].ToString(), dr["dst_id"].ToString(), dr["yn_id_established"].ToString(), dr["yn_id_functional"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion prt_alternative_care_panel_district_upload

                    #region prt_cbsd_resource_allocation_upload
                    case "prt_cbsd_resource_allocation_upload":
                        strSQL = "INSERT INTO prt_cbsd_resource_allocation " +
                            "(cra_id, cra_date, " +
                            "fy_id, prt_id, rgn_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', {10}, {11},'{12}') ";
                        strSQL = string.Format(strSQL, dr["cra_id"].ToString(), Convert.ToDateTime(dr["cra_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["fy_id"].ToString(), dr["prt_id"].ToString(), dr["rgn_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion prt_cbsd_resource_allocation_upload

                    #region prt_cbsd_resource_allocation_district_upload
                    case "prt_cbsd_resource_allocation_district_upload":
                        strSQL = "INSERT INTO prt_cbsd_resource_allocation_district " +
                            "(crad_id, " +
                            "crad_cbsd_budget, crad_cbsd_realization, " +
                            "crad_district_grant_budget, crad_probation_realization, crad_probation_share, " +
                            "cra_id, dst_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id,crad_partner_funding) " +
                            "VALUES ('{0}', " +
                            "{1}, {2}, " +
                            "{3}, {4}, {5}, " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', {13}, {14},'{15}',{16}) ";
                        strSQL = string.Format(strSQL, dr["crad_id"].ToString(),
                            dr["crad_cbsd_budget"].ToString(), dr["crad_cbsd_realization"].ToString(),
                            dr["crad_district_grant_budget"].ToString(), dr["crad_probation_realization"].ToString(), dr["crad_probation_share"].ToString(),
                            dr["cra_id"].ToString(), dr["dst_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString(), Convert.ToDecimal(dr["crad_partner_funding"].ToString()));
                        break;
                    #endregion prt_cbsd_resource_allocation_district_upload

                    #region prt_cbsd_staff_appraisal_tracking_upload
                    case "prt_cbsd_staff_appraisal_tracking_upload":
                        strSQL = "INSERT INTO prt_cbsd_staff_appraisal_tracking " +
                            "(csat_id, " +
                            "csat_date, csat_comment, " +
                            "dst_id, fy_id, prt_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', '{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["csat_id"].ToString(),
                            Convert.ToDateTime(dr["csat_date"]).ToString("dd MMM yyyy HH:mm:ss"), utilFormatting.StringForSQL(dr["csat_comment"].ToString()),
                            dr["dst_id"].ToString(), dr["fy_id"].ToString(), dr["prt_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion prt_cbsd_staff_appraisal_tracking_upload

                    #region prt_cbsd_staff_appraisal_tracking_line_upload
                    case "prt_cbsd_staff_appraisal_tracking_line_upload":
                        strSQL = "INSERT INTO prt_cbsd_staff_appraisal_tracking_line " +
                            "(csatl_id, " +
                            "csatl_posts_approved, csatl_posts_filled, " +
                            "csat_id, ss_id, yn_id_conducted, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "{1}, {2}, " +
                            "'{3}', '{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["csatl_id"].ToString(),
                            dr["csatl_posts_approved"].ToString(), dr["csatl_posts_filled"].ToString(),
                            dr["csat_id"].ToString(), dr["ss_id"].ToString(), dr["yn_id_conducted"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion prt_cbsd_staff_appraisal_tracking_line_upload

                    #region prt_district_ovc_checklist_upload
                    case "prt_district_ovc_checklist_upload":
                        strSQL = "INSERT INTO prt_district_ovc_checklist " +
                            "(doc_id, doc_date, " +
                            "doc_cso_report, doc_cso_total, doc_sub_county_reviewed, doc_sub_county_total, " +
                            "dst_id, fy_id, qy_id, " +
                            "yn_id_dovcc_actions_taken, yn_id_dovcc_minutes, yn_id_dovcc_minutes_available, " +
                            "yn_id_meetings_held, yn_id_membership_constituted, yn_id_ovcmis_district, " +
                            "yn_id_supervision_reports, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "{2}, {3}, {4}, {5}, " +
                            "'{6}', '{7}', '{8}', " +
                            "'{9}', '{10}', '{11}', " +
                            "'{12}', '{13}', '{14}', " +
                            "'{15}', " +
                            "'{16}', '{17}', " +
                            "'{18}', '{19}', " +
                            "'{20}', {21}, {22},'{23}') ";
                        strSQL = string.Format(strSQL, dr["doc_id"].ToString(), Convert.ToDateTime(dr["doc_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["doc_cso_report"].ToString(), dr["doc_cso_total"].ToString(), dr["doc_sub_county_reviewed"].ToString(), dr["doc_sub_county_total"].ToString(),
                            dr["dst_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(),
                            dr["yn_id_dovcc_actions_taken"].ToString(), dr["yn_id_dovcc_minutes"].ToString(), dr["yn_id_dovcc_minutes_available"].ToString(),
                            dr["yn_id_meetings_held"].ToString(), dr["yn_id_membership_constituted"].ToString(), dr["yn_id_ovcmis_district"].ToString(),
                            dr["yn_id_supervision_reports"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion prt_district_ovc_checklist_upload

                    #region prt_institutional_care_summary_upload
                    case "prt_institutional_care_summary_upload":
                        strSQL = "INSERT INTO prt_institutional_care_summary " +
                            "(ics_id, ics_date, " +
                            "dst_id, fy_id, qy_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id,prt_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', {10}, {11},'{12}','{13}') ";
                        strSQL = string.Format(strSQL, dr["ics_id"].ToString(), Convert.ToDateTime(dr["ics_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["dst_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString(), dr["prt_id"].ToString());
                        break;
                    #endregion prt_institutional_care_summary_upload

                    #region prt_institutional_care_summary_line_upload
                    case "prt_institutional_care_summary_line_upload":
                        strSQL = "INSERT INTO prt_institutional_care_summary_line " +
                            "(icsl_id, " +
                            "icsl_caregiver_age, icsl_caregiver_name, " +
                            "icsl_child_age, icsl_child_name, " +
                            "icsl_contact_tel, icsl_contact_village, " +
                            "gnd_id_caregiver, gnd_id_child, ics_id, ins_id, wrd_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id,idst_other,isct_other,iwrd_other) " +
                            "VALUES ('{0}', " +
                            "{1}, '{2}', " +
                            "{3}, '{4}', " +
                            "'{5}', '{6}', " +
                            "'{7}', '{8}', '{9}', '{10}', '{11}', " +
                            "'{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', {17}, {18},'{19}','{20}','{21}','{22}') ";
                        strSQL = string.Format(strSQL, dr["icsl_id"].ToString(),
                            dr["icsl_caregiver_age"].ToString(), utilFormatting.StringForSQL(dr["icsl_caregiver_name"].ToString()),
                            dr["icsl_child_age"].ToString(), utilFormatting.StringForSQL(dr["icsl_child_name"].ToString()),
                            utilFormatting.StringForSQL(dr["icsl_contact_tel"].ToString()), utilFormatting.StringForSQL(dr["icsl_contact_village"].ToString()),
                            dr["gnd_id_caregiver"].ToString(), dr["gnd_id_child"].ToString(), dr["ics_id"].ToString(), dr["ins_id"].ToString(), dr["wrd_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString(), dr["idst_other"].ToString(),dr["isct_other"].ToString(),dr["iwrd_other"].ToString());
                        break;
                    #endregion prt_institutional_care_summary_line_upload

                    #region silc_financial_register_upload
                    case "silc_financial_register_upload":
                        strSQL = "INSERT INTO silc_financial_register " +
                            "(sfr_id, sfr_contact_details, " +
                            "cso_id, fy_id, qy_id, swk_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', '{4}', '{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', {11}, {12},'{13}') ";
                        strSQL = string.Format(strSQL, dr["sfr_id"].ToString(), utilFormatting.StringForSQL(dr["sfr_contact_details"].ToString()),
                            dr["cso_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(), dr["swk_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion silc_financial_register_upload

                    #region silc_financial_register_group_upload
                    case "silc_financial_register_group_upload":
                        strSQL = "INSERT INTO silc_financial_register_group " +
                            "(sfrg_id, " +
                            "sfrg_members_female, sfrg_members_male, sfrg_amount_borrowed, " +
                            "sfr_id, sg_id, yn_id_borrowed, yn_id_linked, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "{1}, {2}, {3}, " +
                            "'{4}', '{5}', '{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', {13}, {14},'{15}') ";
                        strSQL = string.Format(strSQL, dr["sfrg_id"].ToString(),
                            dr["sfrg_members_female"].ToString(), dr["sfrg_members_male"].ToString(), dr["sfrg_amount_borrowed"].ToString(),
                            dr["sfr_id"].ToString(), dr["sg_id"].ToString(), dr["yn_id_borrowed"].ToString(), dr["yn_id_linked"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion silc_financial_register_group_upload

                    #region silc_group_upload
                    case "silc_group_upload":
                        strSQL = "INSERT INTO silc_group " +
                            "(sg_id, sg_name, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}', " +
                            "'{4}', '{5}', " +
                            "'{6}', {7}, {8},'{9}') ";
                        strSQL = string.Format(strSQL, dr["sg_id"].ToString(), utilFormatting.StringForSQL(dr["sg_name"].ToString()),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion silc_group_upload

                    #region silc_group_member_upload
                    case "silc_group_member_upload":
                        strSQL = "INSERT INTO silc_group_member " +
                            "(sgm_id, sgm_name, " +
                            "sgm_status_reason, sgm_active, " +
                            "hhm_id, mtp_id, sg_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', " +
                            "'{2}', {3}, " +
                            "'{4}', '{5}', '{6}', " +
                            "'{7}', '{8}', " +
                            "'{9}', '{10}', " +
                            "'{11}', {12}, {13},'{14}') ";
                        strSQL = string.Format(strSQL, dr["sgm_id"].ToString(), utilFormatting.StringForSQL(dr["sgm_name"].ToString()),
                            utilFormatting.StringForSQL(dr["sgm_status_reason"].ToString()), Convert.ToInt32(Convert.ToBoolean(dr["sgm_active"])),
                            dr["hhm_id"].ToString(), dr["mtp_id"].ToString(), dr["sg_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion silc_group_member_upload

                    #region silc_savings_register_upload
                    case "silc_savings_register_upload":
                        strSQL = "INSERT INTO silc_savings_register " +
                            "(ssr_id, ssr_cycle_number, ssr_share_value, " +
                            "cso_id, fy_id, qy_id, wrd_id, sg_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', {2},  " +
                            "'{3}', '{4}', '{5}', '{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', {13}, {14},'{15}') ";
                        strSQL = string.Format(strSQL, dr["ssr_id"].ToString(), utilFormatting.StringForSQL(dr["ssr_cycle_number"].ToString()), dr["ssr_share_value"].ToString(),
                            dr["cso_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(), dr["wrd_id"].ToString(), dr["sg_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion silc_savings_register_upload

                    #region silc_savings_register_member_upload
                    case "silc_savings_register_member_upload":
                        strSQL = "INSERT INTO silc_savings_register_member " +
                            "(ssrm_id, " +
                            "ssrm_shares_bought_today, ssrm_shares_brought_forward, " +
                            "ssrm_shares_redeemed, ssrm_shares_total, " +
                            "ssrm_welfare_fund, " +
                            "sgm_id, ssr_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', " +
                            "{1}, {2}, " +
                            "{3}, {4}, " +
                            "'{5}', " +
                            "'{6}', '{7}', " +
                            "'{8}', '{9}', " +
                            "'{10}', '{11}', " +
                            "'{12}', {13}, {14},'{15}') ";
                        strSQL = string.Format(strSQL, dr["ssrm_id"].ToString(),
                            dr["ssrm_shares_bought_today"].ToString(), dr["ssrm_shares_brought_forward"].ToString(),
                            dr["ssrm_shares_redeemed"].ToString(), dr["ssrm_shares_total"].ToString(),
                            utilFormatting.StringForSQL(dr["ssrm_welfare_fund"].ToString()),
                            dr["sgm_id"].ToString(), dr["ssr_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion silc_savings_register_member_upload

                    #region ss_error_upload
                    case "ss_error_upload":
                        strSQL = "INSERT INTO ss_error " +
                            "(err_sid, err_form, err_method, " +
                            "err_message, err_stack, " +
                            "usr_id_create, " +
                            "usr_date_create, " +
                            "ofc_id, trg_action, imp_sid) " +
                            "VALUES ({0}, '{1}', '{2}', " +
                            "'{3}', '{4}', " +
                            "'{5}', " +
                            "'{6}', " +
                            "'{7}', {8}, {9}) ";
                        strSQL = string.Format(strSQL, dr["err_sid"].ToString(), utilFormatting.StringForSQL(dr["err_form"].ToString()), utilFormatting.StringForSQL(dr["err_method"].ToString()),
                            utilFormatting.StringForSQL(dr["err_message"].ToString()), utilFormatting.StringForSQL(dr["err_stack"].ToString()),
                            dr["usr_id_create"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid);
                        break;
                    #endregion ss_error_upload

                    #region swm_social_worker_upload
                    case "swm_social_worker_upload":
                        strSQL = "INSERT INTO swm_social_worker " +
                            "(swk_id, swk_first_name, swk_last_name, " +
                            "swk_email, swk_phone, swk_phone_other, " +
                            "swk_status_reason, swk_village, " +
                            "cso_id, hnr_id, swk_id_manager, " +
                            "sws_id, swt_id, wrd_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid,district_id) " +
                            "VALUES ('{0}', '{1}', '{2}', " +
                            "'{3}', '{4}', " +
                            "'{5}', '{6}', '{7}', " +
                            "'{8}', '{9}', '{10}', " +
                            "'{11}', '{12}', '{13}', " +
                            "'{14}', '{15}', " +
                            "'{16}', '{17}', " +
                            "'{18}', {19}, {20},'{21}') ";
                        strSQL = string.Format(strSQL, dr["swk_id"].ToString(), utilFormatting.StringForSQL(dr["swk_first_name"].ToString()), utilFormatting.StringForSQL(dr["swk_last_name"].ToString()),
                            utilFormatting.StringForSQL(dr["swk_email"].ToString()), utilFormatting.StringForSQL(dr["swk_phone"].ToString()), utilFormatting.StringForSQL(dr["swk_phone_other"].ToString()),
                            utilFormatting.StringForSQL(dr["swk_status_reason"].ToString()), utilFormatting.StringForSQL(dr["swk_village"].ToString()),
                            dr["cso_id"].ToString(), dr["hnr_id"].ToString(), dr["swk_id_manager"].ToString(),
                            dr["sws_id"].ToString(), dr["swt_id"].ToString(), dr["wrd_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion swm_social_worker_upload

                    #region um_user_upload
                    case "um_user_upload":
                        strSQL = "INSERT INTO um_office_user " +
                            "(ousr_id, ousr_password, ousr_password_format, ousr_password_salt, " +
                            "ousr_first_name, ousr_last_name, " +
                            "ousr_email, ousr_phone, ousr_skype, ousr_position, " +
                            "ousr_active, hnr_id, lng_id, " +
                            "usr_id_create, usr_id_update, " +
                            "usr_date_create, usr_date_update, " +
                            "ofc_id, trg_action, imp_sid) " +
                            "VALUES ('{0}', '{1}', {2}, '{3}', " +
                            "'{4}', '{5}', " +
                            "'{6}', '{7}', '{8}', '{9}', " +
                            "{10}, '{11}', '{12}', " +
                            "'{13}', '{14}', " +
                            "'{15}', '{16}', " +
                            "'{17}', {18}, {19}) ";
                        strSQL = string.Format(strSQL, dr["usr_id"].ToString(), utilFormatting.StringForSQL(dr["usr_password"].ToString()), dr["usr_password_format"].ToString(), dr["usr_password_salt"].ToString(),
                            utilFormatting.StringForSQL(dr["usr_first_name"].ToString()), utilFormatting.StringForSQL(dr["usr_last_name"].ToString()),
                            utilFormatting.StringForSQL(dr["usr_email"].ToString()), utilFormatting.StringForSQL(dr["usr_phone"].ToString()), utilFormatting.StringForSQL(dr["usr_skype"].ToString()), utilFormatting.StringForSQL(dr["usr_position"].ToString()),
                            Convert.ToInt32(Convert.ToBoolean(dr["usr_active"])), dr["hnr_id"].ToString(), dr["lng_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid);
                        break;
                    #endregion um_user_upload

                    #region hh_household_linkages_tracking_upload
                    case "hh_household_linkages_tracking_upload":
                        strSQL = "INSERT INTO [dbo].[hh_household_linkages_tracking]" +
                               "([hhm_linkages_record_guid] ,[partner_id] ,[hhm_district_id] ,[subcounty_id]" +
                               ",[parish_id],[village],[hhm_id] ,[service_provider_id] ,[usr_id_create],[usr_id_update]" +
                               ",[usr_date_create],[usr_date_update],[ofc_id],[trg_action],imp_sid ,[district_id]) " +
                                "VALUES ('{0}', '{1}', {2}, '{3}', " +
                                "'{4}', '{5}', " +
                                "'{6}', '{7}', '{8}', '{9}', " +
                                "'{10}', '{11}', '{12}', " +
                                "'{13}','{14}','{15}')";
                        strSQL = string.Format(strSQL, dr["hhm_linkages_record_guid"].ToString(), utilFormatting.StringForSQL(dr["partner_id"].ToString()), dr["hhm_district_id"].ToString(), dr["subcounty_id"].ToString(),
                            utilFormatting.StringForSQL(dr["parish_id"].ToString()), utilFormatting.StringForSQL(dr["village"].ToString()),
                            utilFormatting.StringForSQL(dr["hhm_id"].ToString()), utilFormatting.StringForSQL(dr["service_provider_id"].ToString()), utilFormatting.StringForSQL(dr["usr_id_create"].ToString()), utilFormatting.StringForSQL(dr["usr_id_update"].ToString()),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, utilFormatting.StringForSQL(dr["district_id"].ToString()));
                        break;
                    #endregion hh_household_linkages_tracking_upload

                    #region hh_household_linkages_services_required_upload
                    case "hh_household_linkages_services_required_upload":
                        strSQL = "INSERT INTO [dbo].[hh_household_linkages_services_required] " +
                               "([record_guid],[hhm_linkages_record_guid] ,[lsr_id],[usr_id_create] ,[usr_id_update],[usr_date_create] " +
                               ",[usr_date_update],[ofc_id],trg_action,imp_sid,[district_id]) " +
                                "VALUES ('{0}', '{1}', '{2}', '{3}', " +
                                "'{4}', '{5}', " +
                                "'{6}', '{7}','{8}','{9}','{10}')";
                        strSQL = string.Format(strSQL, dr["record_guid"].ToString(), dr["hhm_linkages_record_guid"].ToString(), utilFormatting.StringForSQL(dr["lsr_id"].ToString()),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                            Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_household_linkages_services_required_upload

                    #region hh_household_linkages_services_provided_upload
                    case "hh_household_linkages_services_provided_upload":
                        strSQL = "INSERT INTO [dbo].[hh_household_linkages_services_provided]" +
                               "([record_guid],[lsp_id],[hhm_linkages_record_guid] ,[usr_id_create],[usr_date_create] ,[ofc_id],[trg_action],imp_sid " +
                                ",[district_id]) " +
                                "VALUES ('{0}', '{1}', '{2}', '{3}', " +
                                "'{4}', '{5}', " +
                                "'{6}', '{7}','{8}')";
                        strSQL = string.Format(strSQL, dr["record_guid"].ToString(), dr["lsp_id"].ToString(), utilFormatting.StringForSQL(dr["hhm_linkages_record_guid"].ToString()),
                            dr["usr_id_create"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                    #endregion hh_household_linkages_services_provided_upload

                    #region hh_household_risk_assessment_header_upload
                    case "hh_household_risk_assessment_header_upload":
                        strSQL = @"INSERT INTO [dbo].[hh_household_risk_assessment_header]([ras_id],[hh_code],[hh_id],[interviewed_member_id],[date_of_visit],[usr_id_create]
                                   ,[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
                                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')";

                        strSQL = string.Format(strSQL, dr["ras_id"].ToString(), dr["hh_code"].ToString(), utilFormatting.StringForSQL(dr["hh_id"].ToString()),
                            dr["interviewed_member_id"].ToString(), Convert.ToDateTime(dr["date_of_visit"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                             Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;

                    #endregion hh_household_risk_assessment_header_upload

                    #region hh_household_risk_assessment_beneficiaries_upload
                    case "hh_household_risk_assessment_beneficiaries_upload":
                        strSQL = @"INSERT INTO [dbo].[hh_household_risk_assessment_beneficiaries]([ras_id],[rasm_id],[hh_member_id],[hh_member_code],[current_hiv_status_id]
                                   ,[is_on_art],[screen_hospital_last_six_months],[screen_either_parents_deceased],[screen_either_siblings_deceased] ,[screen_poor_health_last_three_months]
                                   ,[screen_adult_child_with_hiv_or_tb],[screen_below_relative_grade],[child_eligible_for_test_refferal],[care_giver_accepted_to_test_child]
                                   ,[test_result],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id],[yn_accidental_exposure],[yn_drug_abuse])
                                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}',
                                    '{19}','{20}','{21}','{22}','{23}','{24}')";

                        strSQL = string.Format(strSQL, dr["ras_id"].ToString(), utilFormatting.StringForSQL(dr["rasm_id"].ToString()),
                            dr["hh_member_id"].ToString(), dr["hh_member_code"].ToString(), dr["current_hiv_status_id"].ToString(), dr["is_on_art"].ToString(),
                             dr["screen_hospital_last_six_months"].ToString(), dr["screen_either_parents_deceased"].ToString(), dr["screen_either_siblings_deceased"].ToString(),
                             dr["screen_poor_health_last_three_months"].ToString(), dr["screen_adult_child_with_hiv_or_tb"].ToString(), dr["screen_below_relative_grade"].ToString(),
                             dr["child_eligible_for_test_refferal"].ToString(), dr["care_giver_accepted_to_test_child"].ToString(), dr["test_result"].ToString(),
                             dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                             Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString(), dr["yn_accidental_exposure"].ToString(), dr["yn_drug_abuse"].ToString());
                        break;

                    #endregion hh_household_risk_assessment_beneficiaries_upload

                    #region hh_household_improvement_plan_upload
                    case "hh_household_improvement_plan_upload":
                        strSQL = @"INSERT INTO [dbo].[hh_household_improvement_plan]
                               ([hip_id],[hh_code],[hh_id],[visit_date],[ov_below_seventeen_yrs_male],[ov_below_seventeen_yrs_female],[ov_above_eighteen_yrs_male]
                               ,[ov_above_eighteen_yrs_female],[health_knows_status_of_children],[health_enrolled_on_art] ,[health_action_plan],[health_follow_up_date]
                               ,[household_is_healthy],[safe_has_birth_certificates],[safe_no_child_abuse],[safe_action_plan],[safe_follow_up_date],[household_is_safe]
                               ,[stable_source_of_income],[stable_financial_services],[stable_two_or_more_meals],[stable_action_plan],[stable_follow_up_date],[household_is_stable]
		                        ,[schooled_all_attending_school],[schooled_attained_techinical_skill],[schooled_others] ,[schooled_action_plan],[schooled_follow_up_date]
			                    ,[household_is_schooled],[sw_id],[sw_comment],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action]
			                    ,[imp_sid],[district_id])
                               VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}',
                               '{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}')";

                        strSQL = string.Format(strSQL, dr["hip_id"].ToString(), utilFormatting.StringForSQL(dr["hh_code"].ToString()),
                            dr["hh_id"].ToString(), Convert.ToDateTime(dr["visit_date"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToInt32(dr["ov_below_seventeen_yrs_male"].ToString()), Convert.ToInt32(dr["ov_below_seventeen_yrs_female"].ToString()),
                             Convert.ToInt32(dr["ov_above_eighteen_yrs_male"].ToString()), Convert.ToInt32(dr["ov_above_eighteen_yrs_female"].ToString()), dr["health_knows_status_of_children"].ToString(),
                             dr["health_enrolled_on_art"].ToString(), dr["health_action_plan"].ToString(), dr["health_follow_up_date"].ToString(),
                             dr["household_is_healthy"].ToString(), dr["safe_has_birth_certificates"].ToString(), dr["safe_no_child_abuse"].ToString(),
                             dr["safe_action_plan"].ToString(), dr["safe_follow_up_date"].ToString(), dr["household_is_safe"].ToString(), dr["stable_source_of_income"].ToString(), dr["stable_financial_services"].ToString(),
                              dr["stable_two_or_more_meals"].ToString(), dr["stable_action_plan"].ToString(), dr["stable_follow_up_date"].ToString(), dr["household_is_stable"].ToString(), dr["schooled_all_attending_school"].ToString(),
                              dr["schooled_attained_techinical_skill"].ToString(), dr["schooled_others"].ToString(), dr["schooled_action_plan"].ToString(), dr["schooled_follow_up_date"].ToString(),
                              dr["household_is_schooled"].ToString(), dr["sw_id"].ToString(), dr["sw_comment"].ToString(), dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                              Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                              dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion hh_household_improvement_plan_upload

                    #region silc_community_training_register_upload

                    case "silc_community_training_register_upload":
                        strSQL = @"INSERT INTO [dbo].[silc_community_training_register]
                                       ([ctr_id],[prt_id],[cso_id],[dst_id],[sct_id],[tr_name],[module_name],[tr_total_days],[tr_date_from]
		                               ,[tr_date_to],[module_desc],[tr_venue],[trainer_type] ,[artisan_name],[facilitator_trainer_name],[usr_id_create],[usr_id_update]
                                       ,[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
                                 VALUES
                                       ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'
                                       ,'{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')";

                        strSQL = string.Format(strSQL, dr["ctr_id"].ToString(), utilFormatting.StringForSQL(dr["prt_id"].ToString()),
                            dr["cso_id"].ToString(),  dr["dst_id"].ToString(),
                             dr["sct_id"].ToString(), dr["tr_name"].ToString(), dr["module_name"].ToString(),
                             dr["tr_total_days"].ToString(), Convert.ToDateTime(dr["tr_date_from"]).ToString("dd MMM yyyy HH:mm:ss"),
                             Convert.ToDateTime(dr["tr_date_to"]).ToString("dd MMM yyyy HH:mm:ss"), dr["module_desc"].ToString(),
                             dr["tr_venue"].ToString(), dr["trainer_type"].ToString(), dr["artisan_name"].ToString(), dr["facilitator_trainer_name"].ToString(), dr["usr_id_create"].ToString(),
                              dr["usr_id_update"].ToString(),
                              Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                              dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;

                    #endregion silc_community_training_register_upload

                    #region silc_community_training_register_member_upload
                    case "silc_community_training_register_member_upload":
                        strSQL = @"INSERT INTO [dbo].[silc_community_training_register_member]
                                       ([ctrm_id],[ctr_id],[ben_type],[hhm_code],[parcipant_name],[gnd_id],[age]
                                       ,[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id]
                                       ,[trg_action],[imp_sid] ,[district_id])
                                VALUES
                                        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')";

                        strSQL = string.Format(strSQL, dr["ctrm_id"].ToString(), utilFormatting.StringForSQL(dr["ctr_id"].ToString()),
                            dr["ben_type"].ToString(), dr["hhm_code"].ToString(),
                             dr["parcipant_name"].ToString(), dr["gnd_id"].ToString(), dr["age"].ToString(), dr["usr_id_create"].ToString(),dr["usr_id_update"].ToString(),
                              Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                              dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;

                    #endregion silc_community_training_register_member_upload

                    #region silc_community_training_register_member_attendance_dates_upload
                    case "silc_community_training_register_member_attendance_dates_upload":
                        strSQL = @"INSERT INTO [dbo].[silc_community_training_register_member_attendance_dates]
                                           ([ctrmD_id],[ctrm_id],[date],[ofc_id],[trg_action],[imp_sid] ,[district_id])
                                     VALUES
                                           ('{0}','{1}','{2}' ,'{3}','{4}','{5}','{6}')";

                        strSQL = string.Format(strSQL, dr["ctrmD_id"].ToString(), utilFormatting.StringForSQL(dr["ctrm_id"].ToString()),
                            Convert.ToDateTime(dr["date"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                            dr["district_id"].ToString());
                        break;
                    #endregion silc_community_training_register_member_attendance_dates_upload

                    #region ben_youth_training_inventory_upload
                    case "ben_youth_training_inventory_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youth_training_inventory]
                                   ([yti_id],[prt_id],[cso_id],[dst_id],[sct_id],[wrd_id],[begin_date],[hhm_code],[hhm_name]
                                   ,[grp_name],[age],[gnd_id],[training_type],[trainer_name],[exp_date_completion],[actual_date_completion]
                                   ,[usr_id_create] ,[usr_id_update] ,[usr_date_create] ,[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
                                    VALUES('{0}','{1}','{2}' ,'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',
                                    '{18}','{19}','{20}','{21}','{22}','{23}')";

                        strSQL = string.Format(strSQL, dr["yti_id"].ToString(), utilFormatting.StringForSQL(dr["prt_id"].ToString()),
                            dr["cso_id"].ToString(), dr["dst_id"].ToString(),
                             dr["sct_id"].ToString(), dr["wrd_id"].ToString(), Convert.ToDateTime(dr["begin_date"]).ToString("dd MMM yyyy HH:mm:ss"),

                             dr["hhm_code"].ToString(), dr["hhm_name"].ToString(), dr["grp_name"].ToString(), dr["age"].ToString(), dr["gnd_id"].ToString(),

                             dr["training_type"].ToString(),dr["trainer_name"].ToString(), Convert.ToDateTime(dr["exp_date_completion"]).ToString("dd MMM yyyy HH:mm:ss"),
                             Convert.ToDateTime(dr["actual_date_completion"]).ToString("dd MMM yyyy HH:mm:ss"), dr["usr_id_create"].ToString(),
                             dr["usr_id_update"].ToString(),
                             Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_youth_training_inventory_upload

                    #region ben_youthgroup_savings_register_upload
                    case "ben_youthgroup_savings_register_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youthgroup_savings_register]
                                   ([ysr_id],[prt_id],[cso_id],[dst_id],[sct_id],[wrd_id],[village],[month],[year]
                                   ,[ygrp_name],[ygrp_chairperson_name],[ygrp_chairperson_name_phone],[youth_field_assisstant_name],[usr_id_create],[usr_id_update],[usr_date_create]
		                           ,[usr_date_update],[ofc_id],[trg_action] ,[imp_sid],[district_id])
                             VALUES
                                   ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}' ,'{8}','{9}','{10}' ,'{11}','{12}' ,'{13}','{14}'
                                   ,'{15}' ,'{16}','{17}' ,'{18}','{19}','{20}')";

                        strSQL = string.Format(strSQL, dr["ysr_id"].ToString(), utilFormatting.StringForSQL(dr["prt_id"].ToString()),
                            dr["cso_id"].ToString(), dr["dst_id"].ToString(),
                             dr["sct_id"].ToString(), dr["wrd_id"].ToString(), dr["village"].ToString(), dr["month"].ToString(), dr["year"].ToString(),
                             dr["ygrp_name"].ToString(), dr["ygrp_chairperson_name"].ToString(), dr["ygrp_chairperson_name_phone"].ToString(),
                             dr["youth_field_assisstant_name"].ToString(),dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                             Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_youthgroup_savings_register_upload

                    #region ben_youthgroup_savings_register_member_upload
                    case "ben_youthgroup_savings_register_member_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youthgroup_savings_register_member]
                                       ([ysrm_id],[ysr_id],[hhm_code],[hhm_name],[usr_id_create],[usr_id_update],[usr_date_create]
                                       ,[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id],[hhm_id],[yn_direct_beneficiary])
                                 VALUES
                                       ('{0}','{1}' ,'{2}' ,'{3}','{4}' ,'{5}' ,'{6}','{7}' ,'{8}','{9}' ,'{10}' ,'{11}','{12}','{13}')";

                        strSQL = string.Format(strSQL, dr["ysrm_id"].ToString(), utilFormatting.StringForSQL(dr["ysr_id"].ToString()),
                            dr["hhm_code"].ToString(), dr["hhm_name"].ToString(), dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                             Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString(), dr["hhm_id"].ToString(), dr["yn_direct_beneficiary"].ToString());
                        break;
                    #endregion ben_youthgroup_savings_register_member_upload

                    #region ben_youthgroup_savings_register_member_amount_upload
                    case "ben_youthgroup_savings_register_member_amount_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youthgroup_savings_register_member_amount]
                                       ([ysrm_id] ,[ysrms_id],[total_savings] ,[amout_borrowed] ,[loan_purpose],[loan_purpose_other] ,[trg_action] ,[imp_sid],[district_id])
                                 VALUES
                                       ('{0}','{1}' ,'{2}','{3}','{4}' ,'{5}','{6}','{7}','{8}')";

                        strSQL = string.Format(strSQL, dr["ysrm_id"].ToString(), utilFormatting.StringForSQL(dr["ysrms_id"].ToString()),
                            dr["total_savings"].ToString(), dr["amout_borrowed"].ToString(), dr["loan_purpose"].ToString(), dr["loan_purpose_other"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_youthgroup_savings_register_member

                    #region ben_education_subsidy_assessment_upload
                    case "ben_education_subsidy_assessment_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_education_subsidy_assessment]
                   ([ed_sub_id],[prt_id],[cso_id],[dst_id],[sct_id],[wrd_id],[village],[assessment_date],[hhm_id_caregiver],[caregiver_phone],[hh_id],[usr_id_create]
                   ,[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action] ,[imp_sid],[district_id])
                                 VALUES
                                       ('{0}','{1}' ,'{2}','{3}','{4}' ,'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}')";

                        strSQL = string.Format(strSQL, dr["ed_sub_id"].ToString(), utilFormatting.StringForSQL(dr["prt_id"].ToString()),
                            dr["cso_id"].ToString(), dr["dst_id"].ToString(), dr["sct_id"].ToString(), dr["wrd_id"].ToString(), dr["village"].ToString(), Convert.ToDateTime(dr["assessment_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                           dr["hhm_id_caregiver"].ToString(), dr["caregiver_phone"].ToString(), dr["hh_id"].ToString(), dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                           Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_education_subsidy_assessment_upload

                    #region ben_education_subsidy_assessment_member_upload
                    case "ben_education_subsidy_assessment_member_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_education_subsidy_assessment_member]
                       ([ed_subm_id],[ed_sub_id],[hhm_id],[last_class_completed],[prev_school],[drop_out_year],[dropout_reason],[yn_id_willing_to_study]
                       ,[enrollment_class],[ttps_id],[preffered_school],[caregiver_contribution],[yn_id_hh_head_in_silc_grp]
                       ,[yn_id_caregiver_commit_sch_attendance],[yn_id_caregiver_commit_pta_meeting],[yn_id_caregiver_commit_acad_performance]
                       ,[yn_id_caregiver_commit_project_interventions],[yn_id_caregiver_commit_contribute_fee],[yn_id_caregiver_commit_keep_child_in_sch]
                       ,[swk_id],[swk_phone],[psw_id],[psw_phone],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action] ,[imp_sid],[district_id])
                        VALUES
                    ('{0}','{1}' ,'{2}','{3}','{4}' ,'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}',
                    '{25}','{26}','{27}','{28}','{29}','{30}')";

                        strSQL = string.Format(strSQL, dr["ed_subm_id"].ToString(), dr["ed_sub_id"].ToString(), utilFormatting.StringForSQL(dr["hhm_id"].ToString()),
                            dr["last_class_completed"].ToString(), dr["prev_school"].ToString(), dr["drop_out_year"].ToString(), dr["dropout_reason"].ToString(), dr["yn_id_willing_to_study"].ToString(), dr["enrollment_class"].ToString(), dr["ttps_id"].ToString(),
                           dr["preffered_school"].ToString(), dr["caregiver_contribution"].ToString(), dr["yn_id_hh_head_in_silc_grp"].ToString(), dr["yn_id_caregiver_commit_sch_attendance"].ToString(),
                           dr["yn_id_caregiver_commit_pta_meeting"].ToString(), dr["yn_id_caregiver_commit_acad_performance"].ToString(), dr["yn_id_caregiver_commit_project_interventions"].ToString(),
                           dr["yn_id_caregiver_commit_contribute_fee"].ToString(), dr["yn_id_caregiver_commit_keep_child_in_sch"].ToString(), dr["swk_id"].ToString(), dr["swk_phone"].ToString(),
                           dr["psw_id"].ToString(), dr["psw_phone"].ToString(), dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                           Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_education_subsidy_assessment_member_upload 

                    #region prt_subcounty_ovc_checklist
                    case "prt_subcounty_ovc_checklist_upload":
                        strSQL = @"INSERT INTO [dbo].[prt_subcounty_ovc_checklist]
				       ([soc_id],[soc_date],[soc_cso_report],[soc_cso_total],[soc_action_points_implemented],[soc_action_points_total_identified],[dst_id]
				       ,[fy_id],[qy_id],[yn_id_meetings_held],[yn_id_membership_constituted],[yn_id_cdo_supervision],[yn_signed_minutes_available]
				       ,[yn_id_sovcc_discussed_minutes_available],[yn_id_ovcmis_district],[usr_id_create] ,[usr_id_update],[usr_date_create],[usr_date_update]
				       ,[ofc_id],[trg_action],[imp_sid],[district_id],[sct_id])
                        VALUES
                    ('{0}','{1}' ,'{2}','{3}','{4}' ,'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')";

                        strSQL = string.Format(strSQL, dr["soc_id"].ToString(), Convert.ToDateTime(dr["soc_date"]).ToString("dd MMM yyyy HH:mm:ss"),Convert.ToInt32(dr["soc_cso_report"].ToString()),
                            Convert.ToInt32(dr["soc_cso_total"].ToString()), Convert.ToInt32(dr["soc_action_points_implemented"].ToString()), Convert.ToInt32(dr["soc_action_points_total_identified"].ToString()),
                            dr["dst_id"].ToString(), dr["fy_id"].ToString(), dr["qy_id"].ToString(), dr["yn_id_meetings_held"].ToString(), dr["yn_id_membership_constituted"].ToString(), dr["yn_id_cdo_supervision"].ToString(),
                            dr["yn_signed_minutes_available"].ToString(), dr["yn_id_sovcc_discussed_minutes_available"].ToString(), dr["yn_id_ovcmis_district"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString(), dr["sct_id"].ToString());
                        break;
                    #endregion prt_subcounty_ovc_checklist

                    #region ben_agro_enterprise_ranking_matrix_upload
                    case "ben_agro_enterprise_ranking_matrix_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_agro_enterprise_ranking_matrix]
                               ([agro_ent_id] ,[prt_id],[cso_id],[wrd_id],[date],[hhm_id]
                               ,[fa_name],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
                               VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')";

                        strSQL = string.Format(strSQL, dr["agro_ent_id"].ToString(), dr["prt_id"].ToString(), dr["cso_id"].ToString(), dr["wrd_id"].ToString(), Convert.ToDateTime(dr["date"]).ToString("dd MMM yyyy HH:mm:ss"), dr["hhm_id"].ToString(),
                            dr["fa_name"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_agro_enterprise_ranking_matrix_upload

                    #region ben_agro_enterprise_ranking_matrix_crop_ranking_upload
                    case "ben_agro_enterprise_ranking_matrix_crop_ranking_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_agro_enterprise_ranking_matrix_crop_ranking]
                       ([agro_entm_id],[agro_ent_id],[crop_1_id],[crop_1_param1_score],[crop_1_param2_score],[crop_1_param3_score],[crop_1_param4_score],[crop_1_param5_score]
                       ,[crop_1_param6_score],[crop_1_param7_score],[crop_1_param8_score],[crop_1_total_score],[crop_1_rank]
                       ,[crop_2_id],[crop_2_param1_score],[crop_2_param2_score],[crop_2_param3_score],[crop_2_param4_score],[crop_2_param5_score],[crop_2_param6_score]
                       ,[crop_2_param7_score],[crop_2_param8_score],[crop_2_total_score],[crop_2_rank]
                       ,[crop3_id],[crop_3_param1_score],[crop_3_param2_score],[crop_3_param3_score],[crop_3_param4_score],[crop_3_param5_score],[crop_3_param6_score],[crop_3_param7_score]
                       ,[crop_3_param8_score],[crop_3_total_score],[crop_3_rank]
                       ,[crop_4_id],[crop_4_param1_score],[crop_4_param2_score],[crop_4_param3_score],[crop_4_param4_score],[crop_4_param5_score],[crop_4_param6_score],[crop_4_param7_score]
                       ,[crop_4_param8_score],[crop_4_total_score],[crop_4_rank]
		               ,[crop_5_id],[crop_5_param1_score],[crop_5_param2_score],[crop_5_param3_score],[crop_5_param4_score],[crop_5_param5_score],[crop_5_param6_score],[crop_5_param7_score]
                       ,[crop_5_param8_score],[crop_5_total_score],[crop_5_rank],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
                       VALUES('{0}','{1}','{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},'{13}',{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},'{24}',{25},{26},{27},{28},{29},{30},{31},
                    {32},{33},{34},'{35}',{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},'{46}',{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},'{57}','{58}','{59}','{60}','{61}','{62}','{63}','{64}')";
            
                        strSQL = string.Format(strSQL, dr["agro_entm_id"].ToString(), dr["agro_ent_id"].ToString(), dr["crop_1_id"].ToString(), Convert.ToInt32( dr["crop_1_param1_score"].ToString()), Convert.ToInt32(dr["crop_1_param2_score"].ToString()),
                            Convert.ToInt32(dr["crop_1_param3_score"].ToString()), Convert.ToInt32(dr["crop_1_param4_score"].ToString()), Convert.ToInt32(dr["crop_1_param5_score"].ToString()), Convert.ToInt32(dr["crop_1_param6_score"].ToString()), Convert.ToInt32(dr["crop_1_param7_score"].ToString()),
                            Convert.ToInt32(dr["crop_1_param8_score"].ToString()), Convert.ToInt32(dr["crop_1_total_score"].ToString()), Convert.ToInt32(dr["crop_1_rank"].ToString()), dr["crop_2_id"].ToString(), Convert.ToInt32(dr["crop_2_param1_score"].ToString()), Convert.ToInt32(dr["crop_2_param2_score"].ToString()),
                            Convert.ToInt32(dr["crop_2_param3_score"].ToString()), Convert.ToInt32(dr["crop_2_param4_score"].ToString()), Convert.ToInt32(dr["crop_2_param5_score"].ToString()), Convert.ToInt32(dr["crop_2_param6_score"].ToString()), Convert.ToInt32(dr["crop_2_param7_score"].ToString()),
                            Convert.ToInt32(dr["crop_2_param8_score"].ToString()), Convert.ToInt32(dr["crop_2_total_score"].ToString()), Convert.ToInt32(dr["crop_2_rank"].ToString()), dr["crop3_id"].ToString(), Convert.ToInt32(dr["crop_3_param1_score"].ToString()), Convert.ToInt32(dr["crop_3_param2_score"].ToString()),
                            Convert.ToInt32(dr["crop_3_param3_score"].ToString()), Convert.ToInt32(dr["crop_3_param4_score"].ToString()), Convert.ToInt32(dr["crop_3_param5_score"].ToString()), Convert.ToInt32(dr["crop_3_param6_score"].ToString()), Convert.ToInt32(dr["crop_3_param7_score"].ToString()),
                            Convert.ToInt32(dr["crop_3_param8_score"].ToString()), Convert.ToInt32(dr["crop_3_total_score"].ToString()), Convert.ToInt32(dr["crop_3_rank"].ToString()), dr["crop_4_id"].ToString(), Convert.ToInt32(dr["crop_4_param1_score"].ToString()), Convert.ToInt32(dr["crop_4_param2_score"].ToString()),
                            Convert.ToInt32(dr["crop_4_param3_score"].ToString()), Convert.ToInt32(dr["crop_4_param4_score"].ToString()), Convert.ToInt32(dr["crop_4_param5_score"].ToString()), Convert.ToInt32(dr["crop_4_param6_score"].ToString()), Convert.ToInt32(dr["crop_4_param7_score"].ToString()),
                            Convert.ToInt32(dr["crop_4_param8_score"].ToString()), Convert.ToInt32(dr["crop_4_total_score"].ToString()), Convert.ToInt32(dr["crop_4_rank"].ToString()), dr["crop_5_id"].ToString(), Convert.ToInt32(dr["crop_5_param1_score"].ToString()), Convert.ToInt32(dr["crop_5_param2_score"].ToString()),
                            Convert.ToInt32(dr["crop_5_param3_score"].ToString()), Convert.ToInt32(dr["crop_5_param4_score"].ToString()), Convert.ToInt32(dr["crop_5_param5_score"].ToString()), Convert.ToInt32(dr["crop_5_param6_score"].ToString()), Convert.ToInt32(dr["crop_5_param7_score"].ToString()),
                            Convert.ToInt32(dr["crop_5_param8_score"].ToString()), Convert.ToInt32(dr["crop_5_total_score"].ToString()), Convert.ToInt32(dr["crop_5_rank"].ToString()),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_agro_enterprise_ranking_matrix_crop_ranking_upload

                    #region ben_apprenticeship_skill_acquisition_tracking_upload
                    case "ben_apprenticeship_skill_acquisition_tracking_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_apprenticeship_skill_acquisition_tracking]
                                   ([asat_id],[review_date_from],[review_date_to],[wrd_id],[hhm_id],[trade_id],[module_id],[youth_acquire_not_acquire_skill_reason]
                                   ,[recommended_steps],[artisan_name],[artisan_report_date],[youth_skills_acquired],[yn_skill_not_acquired_well],[skill_not_acquired_well],[skill_not_acquired_well_reason],
                                   [youth_report_date],[dyo_name],[dyo_review_date],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
                             VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'
                                   ,'{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}')";

                        strSQL = string.Format(strSQL, dr["asat_id"].ToString(), Convert.ToDateTime(dr["review_date_from"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["review_date_to"]).ToString("dd MMM yyyy HH:mm:ss"), dr["wrd_id"].ToString(), dr["hhm_id"].ToString(), dr["trade_id"].ToString(),
                            dr["module_id"].ToString(), dr["youth_acquire_not_acquire_skill_reason"].ToString(), dr["recommended_steps"].ToString(), dr["artisan_name"].ToString(),
                            Convert.ToDateTime(dr["artisan_report_date"]).ToString("dd MMM yyyy HH:mm:ss"), dr["youth_skills_acquired"].ToString(), dr["yn_skill_not_acquired_well"].ToString(),
                            dr["skill_not_acquired_well"].ToString(), dr["skill_not_acquired_well_reason"].ToString(), Convert.ToDateTime(dr["youth_report_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["dyo_name"].ToString(), Convert.ToDateTime(dr["dyo_review_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_apprenticeship_skill_acquisition_tracking_upload

                    #region ben_apprenticeship_skill_acquisition_tracking_skill_upload
                    case "ben_apprenticeship_skill_acquisition_tracking_skill_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_apprenticeship_skill_acquisition_tracking_skill]
                               ([asatskill_id],[asat_id],[module_id],[skill_id],[excellent_acquired_skr_id],[average_acquired_skr_id],[not_acquired_skr_id],[usr_id_create],[usr_id_update]
                               ,[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
		                       VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')";

                        strSQL = string.Format(strSQL, dr["asatskill_id"].ToString(), dr["asat_id"].ToString(), dr["module_id"].ToString(), dr["skill_id"].ToString(),
                            dr["excellent_acquired_skr_id"].ToString(), dr["average_acquired_skr_id"].ToString(), dr["not_acquired_skr_id"].ToString(),
                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_apprenticeship_skill_acquisition_tracking_skill_upload

                    #region ben_education_subsidy_school_readiness
                    case "ben_education_subsidy_school_readiness_upload":
                            strSQL = @"INSERT INTO [dbo].[ben_education_subsidy_school_readiness]
                           ([edsr_id],[ip_id],[cso_id],[wrd_id],[edsr_ass_date],[hh_id],[hhm_id_caregiver],[hhm_caregiver_phone],[yn_hh_silc]
                           ,[yn_child_in_school],[hhm_id],[last_class_completed],[prev_school_name],[drop_out_yr],[child_next_steps],[current_class]
                           ,[child_future_plans],[current_school_name],[sw_id],[psw_id],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update]
                           ,[ofc_id],[trg_action],[imp_sid],[district_id])
		                       VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}')";

                        strSQL = string.Format(strSQL, dr["edsr_id"].ToString(), dr["ip_id"].ToString(), dr["cso_id"].ToString(), dr["wrd_id"].ToString(),
                            Convert.ToDateTime(dr["edsr_ass_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["hh_id"].ToString(), dr["hhm_id_caregiver"].ToString(), dr["hhm_caregiver_phone"].ToString(), dr["yn_hh_silc"].ToString(),
                            dr["yn_child_in_school"].ToString(), dr["hhm_id"].ToString(), dr["last_class_completed"].ToString(), dr["prev_school_name"].ToString(),
                            dr["drop_out_yr"].ToString(), dr["child_next_steps"].ToString(), dr["current_class"].ToString(), dr["child_future_plans"].ToString(),
                            dr["current_school_name"].ToString(), dr["sw_id"].ToString(), dr["psw_id"].ToString(),



                            dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_education_subsidy_school_readiness

                    #region ben_ovc_viral_load
                    case "ben_ovc_viral_load_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_ovc_viral_load]
                                   ([vl_id],[prt_id],[cso_id],[wrd_id],[hh_id],[qrt_start_date],[qrt_end_date],[linc_id],[sw_id]
                                   ,[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
		                       VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')";

                        strSQL = string.Format(strSQL, dr["vl_id"].ToString(), dr["prt_id"].ToString(), dr["cso_id"].ToString(), dr["wrd_id"].ToString(),
                            dr["hh_id"].ToString(), Convert.ToDateTime(dr["qrt_start_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["qrt_end_date"]).ToString("dd MMM yyyy HH:mm:ss"), dr["linc_id"].ToString(),
                            dr["sw_id"].ToString(), dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_ovc_viral_load

                    #region ben_ovc_viral_load_member
                    case "ben_ovc_viral_load_member_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_ovc_viral_load_member]
                                   ([vlm_id],[vl_id],[hhm_id],[hef_name],[art_number],[vl_eligible],[vl_done],[vl_date]
                                   ,[vl_nextdate],[suppressed],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update]
                                   ,[ofc_id],[trg_action],[imp_sid],[district_id])
		                       VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')";
                        strSQL = string.Format(strSQL, dr["vlm_id"].ToString(), dr["vl_id"].ToString(), dr["hhm_id"].ToString(), dr["hef_name"].ToString(), dr["art_number"].ToString(),
                            dr["vl_eligible"].ToString(), dr["vl_done"].ToString(), dr["vl_date"].ToString(), Convert.ToDateTime(dr["vl_nextdate"]).ToString("dd MMM yyyy HH:mm:ss"),
                             dr["suppressed"].ToString(), dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                             dr["district_id"].ToString());
                        break;
                    #endregion ben_ovc_viral_load_member

                    #region ben_youth_training_completion_upload
                    case "ben_youth_training_completion_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youth_training_completion] 
                           ([ytc_id],[prt_id],[cso_id],[wrd_id],[date],[hh_id],[hhm_id],[hhm_tel],[yfo_name],[y_adress],[skills_learnt],[skills_more_training]
                           ,[yn_id_graduate],[yn_id_graduate_no_reason],[artisan_rating],[yn_id_fam_support],[yn_id_fam_support_yes_how],[yn_id_fam_support_no_reason],[yn_id_training_challenges]
                           ,[yn_id_training_challenges_yes_list],[yn_id_earn_money],[yn_id_earn_money_yes_weekly_amt],[plan_after_training],[youth_rate_attendance]
                           ,[youth_rate_commitment],[youth_rate_participation],[youth_rate_comprehension],[module_id],[yn_id_retain_youth],[yn_id_retain_youth_no_recommend]
                           ,[yn_id_open_own_biz],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
		                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'
                           ,'{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}'
                           ,'{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}')";

                        strSQL = string.Format(strSQL, dr["ytc_id"].ToString(), dr["prt_id"].ToString(), dr["cso_id"].ToString(), dr["wrd_id"].ToString(), Convert.ToDateTime(dr["date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["hh_id"].ToString(), dr["hhm_id"].ToString(), dr["hhm_tel"].ToString(), dr["yfo_name"].ToString(), dr["y_adress"].ToString(), dr["skills_learnt"].ToString(), dr["skills_more_training"].ToString(),
                            dr["yn_id_graduate"].ToString(), dr["yn_id_graduate_no_reason"].ToString(), dr["artisan_rating"].ToString(), dr["yn_id_fam_support"].ToString(), dr["yn_id_fam_support_yes_how"].ToString(), dr["yn_id_fam_support_no_reason"].ToString(),
                            dr["yn_id_training_challenges"].ToString(), dr["yn_id_training_challenges_yes_list"].ToString(), dr["yn_id_earn_money"].ToString(), dr["yn_id_earn_money_yes_weekly_amt"].ToString(),
                            dr["plan_after_training"].ToString(), dr["youth_rate_attendance"].ToString(), dr["youth_rate_commitment"].ToString(), dr["youth_rate_participation"].ToString(), dr["youth_rate_comprehension"].ToString(),
                            dr["module_id"].ToString(), dr["yn_id_retain_youth"].ToString(), dr["yn_id_retain_youth_no_recommend"].ToString(), dr["yn_id_open_own_biz"].ToString(), dr["usr_id_create"].ToString(),
                            dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                            dr["district_id"].ToString());
                        break;
                    #endregion ben_youth_training_completion_upload

                    #region ben_youth_training_completion_skill_acquisition_tracking_upload
                    case "ben_youth_training_completion_skill_acquisition_tracking_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youth_training_completion_skill_acquisition_tracking] 
                           ([ytc_skill_id],[ytc_id],[module_id],[skill_id],[excellent_acquired_skr_id],[average_acquired_skr_id],[not_acquired_skr_id],[usr_id_create],[usr_id_update]
                            ,[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
		                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')";
                        strSQL = string.Format(strSQL, dr["ytc_skill_id"].ToString(), dr["ytc_id"].ToString(), dr["module_id"].ToString(), dr["skill_id"].ToString(), dr["excellent_acquired_skr_id"].ToString(),
                            dr["average_acquired_skr_id"].ToString(), dr["not_acquired_skr_id"].ToString(),dr["usr_id_create"].ToString(),
                            dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,
                            dr["district_id"].ToString());
                        break;
                    #endregion ben_youth_training_completion_skill_acquisition_tracking_upload

                    #region ben_youth_assessment_scoring_upload
                    case "ben_youth_assessment_scoring_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youth_assessment_scoring]
                            ([yas_id],[prt_id],[cso_id],[wrd_id],[dt_ass_date],[hh_id],[hhm_id],[ttp_id],[ttps_id],[ys1_id]
                            ,[ys2_id],[ys3_id],[ys4_id],[ys5_id],[ys6_id],[total_score],[youth_notes],[assessor_name],[date_assessor_sign]
                            ,[approver_name],[date_approver_sign],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],[trg_action],[imp_sid],[district_id])
		                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'
                           ,'{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}')";

                        strSQL = string.Format(strSQL, dr["yas_id"].ToString(), dr["prt_id"].ToString(), dr["cso_id"].ToString(), dr["wrd_id"].ToString(), Convert.ToDateTime(dr["dt_ass_date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["hh_id"].ToString(), dr["hhm_id"].ToString(), dr["ttp_id"].ToString(), dr["ttps_id"].ToString(), dr["ys1_id"].ToString(), dr["ys2_id"].ToString(), dr["ys3_id"].ToString(),
                            dr["ys4_id"].ToString(), dr["ys5_id"].ToString(), dr["ys6_id"].ToString(), dr["total_score"].ToString(), dr["youth_notes"].ToString(), dr["assessor_name"].ToString(),
                            Convert.ToDateTime(dr["date_assessor_sign"]).ToString("dd MMM yyyy HH:mm:ss"), dr["approver_name"].ToString(), Convert.ToDateTime(dr["date_approver_sign"]).ToString("dd MMM yyyy HH:mm:ss")
                            , dr["usr_id_create"].ToString(),dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid,dr["district_id"].ToString());
                        break;
                    #endregion ben_youth_training_completion_upload

                    #region ben_youth_tracer_upload
                    case "ben_youth_tracer_upload":
                        strSQL = @"INSERT INTO [dbo].[ben_youth_tracer]
                               ([ytr_id],[prt_id],[cso_id],[wrd_id],[date],[hhm_id],[fo_name],[ttp_received],[ttp_other],[employment_status],[yn_using_acquired_skills]
                               ,[yn_using_acquired_skills_no_reason],[yn_market_available],[average_income],[formal_bussiness_sector],[formal_employment_search_period],[formal_current_job_challenges]
                               ,[self_bussiness_sector],[self_source_of_startup_capital],[sponsor_name],[startup_amt],[bussiness_setup_help_source],[bussiness_startup_duration],[occupation_before_business_startup]
                               ,[bussiness_problems_faced],[unemployed_reason],[unemployed_reason_other],[unemployment_action],[yn_recommend_programme] ,[hhm_comments],[usr_id_create],[usr_id_update],[usr_date_create],[usr_date_update],[ofc_id],
                                [trg_action],[imp_sid],[district_id])
                               
                         VALUES
                               ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'
                               ,'{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}'
                               ,'{30}','{31}' ,'{32}','{33}','{34}','{35}','{36}','{37}')";

                        strSQL = string.Format(strSQL, dr["ytr_id"].ToString(), dr["prt_id"].ToString(), dr["cso_id"].ToString(), dr["wrd_id"].ToString(), Convert.ToDateTime(dr["date"]).ToString("dd MMM yyyy HH:mm:ss"),
                            dr["hhm_id"].ToString(), dr["fo_name"].ToString(), dr["ttp_received"].ToString(), dr["ttp_other"].ToString(), dr["employment_status"].ToString(), dr["yn_using_acquired_skills"].ToString(),
                            dr["yn_using_acquired_skills_no_reason"].ToString(),dr["yn_market_available"].ToString(), dr["average_income"].ToString(), dr["formal_bussiness_sector"].ToString(), dr["formal_employment_search_period"].ToString(), dr["formal_current_job_challenges"].ToString(),
                            dr["self_bussiness_sector"].ToString(), dr["self_source_of_startup_capital"].ToString(), dr["sponsor_name"].ToString(), dr["startup_amt"].ToString(), dr["bussiness_setup_help_source"].ToString(),
                            dr["bussiness_startup_duration"].ToString(), dr["occupation_before_business_startup"].ToString(), dr["bussiness_problems_faced"].ToString(), dr["unemployed_reason"].ToString(), dr["unemployed_reason_other"].ToString(),
                            dr["unemployment_action"].ToString(), dr["yn_recommend_programme"].ToString(), dr["hhm_comments"].ToString(), dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(), Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"),
                            Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["ofc_id"].ToString(), dr["trg_action"].ToString(), strImpSid, dr["district_id"].ToString());
                        break;
                        #endregion ben_youth_training_completion_upload
                }

                if (strSQL.Length != 0)
                {
                    dbCon = new DBConnection(utilSOCYWeb.cWCKConnectionImport);
                    try
                    {
                        dbCon.ExecuteNonQuery(strSQL);
                    }
                    finally
                    {
                        dbCon.Dispose();
                    }
                    blnResult = true;
                }
                #endregion Process Record
            }

            return blnResult;
        }
        #endregion Function methods

        #region Get Methods
        [WebMethod]
        public int DownloadTotal(string strOfcId, string district_list)
        {
            #region Variables
            DBConnection dbCon = null;
            int intTotal = 0;
            #endregion Varaibles

            dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);

            try
            {
                intTotal = GetDownloadTotal(strOfcId, ConfigurationManager.AppSettings[utilSOCYWeb.cWCKDBArchive].ToString(), dbCon, district_list);
            }
            finally
            {
                dbCon.Dispose();
            }

            return intTotal;
        }
        #endregion Get Methods
        #endregion Public

        #region Private
        #region Download Data
        private string GetDownloadData(string strOfcId, string strLDImpSid, string strLDTable, string strLDObjId, int intTop, string strKey, string district_list, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            DataTable dtData = null;

            bool blnComplete = false;
            string strPrimaryKey = string.Empty;
            string strTrgAction = utilSOCYWeb.cTAInsert;
            string strXML = string.Empty;
            #endregion Variables

            #region Get Download Data
            #region Primary Key
            dt = GetData("lst_sync_download", "sdl_name", strLDTable, dbCon); //list of tables from which to download data
            strPrimaryKey = dt.Rows[0]["sdl_key"].ToString(); //primary key for each table
            #endregion Primary Key

            //The original MaxGuid is smaller that the Import Details ID
            if (strLDObjId.Equals(utilSOCYWeb.cMaxGUID))
                strLDObjId = utilSOCYWeb.cMaxGUIDX;
            do
            {
                #region Get Data
                dtData = GetNextData(strLDImpSid, strLDTable, strPrimaryKey, strLDObjId, intTop, district_list, dbCon);
                #endregion Get Data

                if (!utilCollections.HasRows(dtData))
                {
                    #region Get Delete Data
                    //If the Max GUID was returned it means the delete data for a table was just processed. Skip it for this table.
                    if (strLDObjId.Equals(utilSOCYWeb.cMaxGUIDX))
                        strLDObjId = "";
                    else
                        dtData = GetNextDeleteData(strLDImpSid, strLDTable, strPrimaryKey, ConfigurationManager.AppSettings[utilSOCYWeb.cWCKDBArchive].ToString(), dbCon);
                    #endregion Get Delete Data

                    if (!utilCollections.HasRows(dtData))
                    {
                        #region Next Set
                        strLDObjId = "";
                        dt = GetNextDownloadTable(strLDTable, dbCon);
                        if (utilCollections.HasRows(dt))
                        {
                            strLDTable = dt.Rows[0]["sdl_name"].ToString();
                            strPrimaryKey = dt.Rows[0]["sdl_key"].ToString();
                        }
                        else
                        {
                            dt = GetFirstDownloadTable(dbCon);
                            strLDTable = dt.Rows[0]["sdl_name"].ToString();
                            strPrimaryKey = dt.Rows[0]["sdl_key"].ToString();

                            strLDImpSid = GetNextImportHistoryId(strOfcId, strLDImpSid, dbCon);
                            if (strLDImpSid.Equals("0"))
                                blnComplete = true;
                        }
                        #endregion Next Set
                    }
                    else
                        strTrgAction = utilSOCYWeb.cTADelete;
                }

            } while (!utilCollections.HasRows(dtData) && !blnComplete);
            #endregion Get Download Data

            #region Convert DataTable to XML
            if (utilCollections.HasRows(dtData))
                strXML = ConvertDataTableToXML(dtData, strLDImpSid + utilSOCYWeb.cDownloadDelimiter + strLDTable + utilSOCYWeb.cDownloadDelimiter + strTrgAction, strKey);
            #endregion Convert DataTable to XML

            return strXML;
        }

        private int GetDownloadTotal(string strOfcId, string strDBArchive, DBConnection dbCon, string district_list)
        {
            #region Variables
            DataTable dt = null;
            int intTotal = 0;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT sdl_name, sdl_key FROM lst_sync_download ";
            dt = dbCon.ExecuteQueryDataTable(strSQL);

            if (utilCollections.HasRows(dt))
            {
                strSQL = string.Format("DECLARE @Import TABLE (imp_sid int) " +
                    "INSERT INTO @Import (imp_sid) " +
                    "SELECT imp_sid FROM um_import_history WHERE imp_processed = 1 AND NOT ofc_id = '{0}' " +
                    "AND imp_process_date > ( " +
                    "SELECT ISNULL(imp.imp_process_date, '1900/01/01') " +
                    "FROM um_office ofc " +
                    "LEFT JOIN um_office_download od ON ofc.ofc_id = od.ofc_id " +
                    "LEFT JOIN um_import_history imp ON od.imp_sid = imp.imp_sid " +
                    "WHERE ofc.ofc_id = '{0}') " +
                    "AND NOT ofc_id IN ('a76380b1-d4ac-4621-bfd8-beec9422188e', '15122685-a68b-4e7a-9d66-d94251e02965') " +
                    "SELECT SUM(the_count) AS the_count FROM (", strOfcId);

                strSQL = strSQL + string.Format("SELECT COUNT({1}) AS the_count FROM {0} itmp INNER JOIN @Import imp ON itmp.imp_sid = imp.imp_sid WHERE itmp.district_id IN({4}) " +
                    "UNION ALL SELECT COUNT({1}) AS the_count FROM [{2}].[dbo].[{0}] itmp INNER JOIN @Import imp ON itmp.imp_sid = imp.imp_sid WHERE trg_action = '{3}' AND itmp.district_id IN({4}) ",
                    dt.Rows[0]["sdl_name"].ToString(), dt.Rows[0]["sdl_key"].ToString(), strDBArchive, utilSOCYWeb.cTADelete,district_list);
                for (int intCount = 1; intCount < dt.Rows.Count; intCount++)
                {
                    strSQL = strSQL + string.Format("UNION ALL SELECT COUNT({1}) AS the_count FROM {0} itmp INNER JOIN @Import imp ON itmp.imp_sid = imp.imp_sid WHERE itmp.district_id IN({4})  " +
                        "UNION ALL SELECT COUNT({1}) AS the_count FROM [{2}].[dbo].[{0}] itmp INNER JOIN @Import imp ON itmp.imp_sid = imp.imp_sid WHERE trg_action = '{3}' AND itmp.district_id IN({4}) ",
                        dt.Rows[intCount]["sdl_name"].ToString(), dt.Rows[intCount]["sdl_key"].ToString(), strDBArchive, utilSOCYWeb.cTADelete, district_list);
                }
                strSQL = strSQL + ") temp ";
                intTotal = Convert.ToInt32(dbCon.ExecuteScalar(strSQL));
            }
            #endregion SQL

            return intTotal;
        }

        private DataTable GetFirstDownloadTable(DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT TOP 1 * FROM lst_sync_download ORDER BY sdl_name ";
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        private string GetFirstImportHistoryId(string strOfcId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strResult = "0";
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("SELECT TOP 1 imp_sid FROM um_import_history WHERE imp_processed = 1 AND NOT ofc_id = '{0}' " +
                "AND NOT ofc_id IN ('a76380b1-d4ac-4621-bfd8-beec9422188e', '15122685-a68b-4e7a-9d66-d94251e02965') " +
                "ORDER BY imp_process_date ", strOfcId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);

            if (utilCollections.HasRows(dt))
                strResult = dt.Rows[0]["imp_sid"].ToString();
            #endregion SQL

            return strResult;
        }

        private DataTable GetNextData(string strImpSid, string strTable, string strPrimaryKey, string strObjId, int intTop, string district_list, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("DECLARE @topCount INT;SET @topCount = '"+ intTop +"'; SELECT TOP (@topCount) *, '1' AS trg_action FROM {0} WHERE imp_sid = {3} AND {1} > '{4}' AND district_id IN({5}) ORDER BY {1} ", strTable, strPrimaryKey, intTop, strImpSid, strObjId, district_list);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        private DataTable GetNextDeleteData(string strImpSid, string strTable, string strPrimaryKey, string strDBArchive, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("SELECT {1} FROM [{2}].[dbo].[{0}] WHERE imp_sid = {3} AND trg_action = '{4}' ", strTable, strPrimaryKey, strDBArchive, strImpSid, utilSOCYWeb.cTADelete);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        private DataTable GetNextDownloadTable(string strTable, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("SELECT TOP 1 * FROM lst_sync_download WHERE sdl_name > '{0}' ORDER BY sdl_name ", strTable);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        private string GetNextImportHistoryId(string strOfcId, string strImpSid, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strResult = "0";
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("SELECT TOP 1 imp_sid FROM um_import_history WHERE imp_processed = 1 AND NOT ofc_id = '{0}' " +
                "AND imp_process_date > (SELECT imp_process_date FROM um_import_history WHERE imp_sid = {1}) " +
                "AND NOT ofc_id IN ('a76380b1-d4ac-4621-bfd8-beec9422188e', '15122685-a68b-4e7a-9d66-d94251e02965') " +
                "ORDER BY imp_process_date ", strOfcId, strImpSid);
            dt = dbCon.ExecuteQueryDataTable(strSQL);

            if (utilCollections.HasRows(dt))
                strResult = dt.Rows[0]["imp_sid"].ToString();
            #endregion SQL

            return strResult;
        }
        #endregion Download Data

        #region Function Methods
        private string ConvertDataTableToXML(DataTable dt, string strTable, string strKey)
        {
            #region Variables
            StringWriter sw = new StringWriter();
            string strXML;
            #endregion Variables

            #region Convert to XML
            dt.TableName = strTable;
            dt.WriteXml(sw);
            strXML = sw.ToString();
            strXML = utilEncryption.StringEncryption(strXML, strKey);
            #endregion Convert to XML

            return strXML;
        }

        private DataTable ConvertXMLToDataTable(string strXML, string strKey)
        {
            #region Variables
            DataSet ds = new DataSet();
            StringReader sr = null;
            #endregion Variables

            #region Convert to DataTable
            if (strKey.Length != 0)
                strXML = utilEncryption.StringDecryption(strXML, strKey);
            sr = new StringReader(strXML);
            ds.ReadXml(sr);
            #endregion Convert to DataTable

            return ds.Tables[0];
        }

        private void InsertContact(DataRow dr, DBConnection dbCon)
        {
            #region Variables
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "INSERT INTO um_office_user " +
                "(ousr_id, ousr_password, ousr_password_format, ousr_password_salt, " +
                "ousr_first_name, ousr_last_name, " +
                "ousr_email, ousr_phone, ousr_skype, ousr_position, " +
                "ousr_active, hnr_id, lng_id, " +
                "usr_id_create, usr_id_update, " +
                "usr_date_create, usr_date_update, " +
                "ofc_id) " +
                "VALUES ('{0}', '{1}', {2}, '{3}', " +
                "'{4}', '{5}', " +
                "'{6}', '{7}', '{8}', '{9}', " +
                "{10}, '{11}', '{12}', " +
                "'{13}', '{14}', " +
                "'{15}', '{16}', " +
                "'{17}') ";
            strSQL = string.Format(strSQL, dr["usr_id"].ToString(), utilFormatting.StringForSQL(dr["usr_password"].ToString()), dr["usr_password_format"].ToString(), dr["usr_password_salt"].ToString(),
                utilFormatting.StringForSQL(dr["usr_first_name"].ToString()), utilFormatting.StringForSQL(dr["usr_last_name"].ToString()),
                utilFormatting.StringForSQL(dr["usr_email"].ToString()), utilFormatting.StringForSQL(dr["usr_phone"].ToString()), utilFormatting.StringForSQL(dr["usr_skype"].ToString()), utilFormatting.StringForSQL(dr["usr_position"].ToString()),
                Convert.ToInt32(Convert.ToBoolean(dr["usr_active"])), dr["hnr_id"].ToString(), dr["lng_id"].ToString(),
                dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"),
                dr["ofc_id"].ToString());
            dbCon.ExecuteNonQuery(strSQL);
            #endregion SQL
        }

        private void InsertOffice(DataRow dr, DBConnection dbCon)
        {
            #region Variables
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "INSERT INTO  um_office " +
                "(ofc_id, ofc_name, ofc_server_id, " +
                "ofc_email, ofc_phone, " +
                "ofc_address, ofc_app_version, " +
                "ost_id, otp_id, ousr_id_contact, " +
                "usr_id_create, usr_id_update, usr_date_create, usr_date_update,district_id) " +
                "VALUES ('{0}', '{1}', '{2}', " +
                "'{3}', '{4}', '{5}', '{6}', " +
                "'{7}', '{8}', '{9}', " +
                "'{10}', '{11}', " +
                "'{12}', '{13}','{14}') ";
            strSQL = string.Format(strSQL, dr["ofc_id"].ToString(), utilFormatting.StringForSQL(dr["ofc_name"].ToString()), utilFormatting.StringForSQL(dr["ofc_server_id"].ToString()),
                utilFormatting.StringForSQL(dr["ofc_email"].ToString()), utilFormatting.StringForSQL(dr["ofc_phone"].ToString()),
                utilFormatting.StringForSQL(dr["ofc_address"].ToString()), utilFormatting.StringForSQL(dr["ofc_app_version"].ToString()),
                utilSOCYWeb.cOSTWaitingValidation, dr["otp_id"].ToString(), dr["usr_id_contact"].ToString(),
                dr["usr_id_create"].ToString(), dr["usr_id_update"].ToString(),
                Convert.ToDateTime(dr["usr_date_create"]).ToString("dd MMM yyyy HH:mm:ss"), Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["district_id"].ToString());
            dbCon.ExecuteNonQuery(strSQL);
            #endregion SQL
        }

        private string InsertImportHistory(string strOfcId, DBConnection dbCon)
        {
            #region Variables
            string strResult = string.Empty;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "INSERT INTO um_import_history (imp_date, ofc_id, imp_processed, imp_process_date) VALUES (GETDATE(), '{0}', 0, GETDATE()) " +
                "SELECT MAX(imp_sid) FROM um_import_history WHERE ofc_id = '{0}' ";
            strSQL = string.Format(strSQL, strOfcId);
            strResult = dbCon.ExecuteScalar(strSQL);
            #endregion SQL

            return strResult;
        }

        private void InsertOfficeDownload(string strOfcId, string strImpSid, string strTable, string strObjId, DBConnection dbCon)
        {
            #region Variables
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "INSERT INTO um_office_download (odl_id, odl_table, odl_obj_id, imp_sid, ofc_id, usr_date_update) " +
                "VALUES (LOWER(NEWID()), '{0}', '{1}', {2}, '{3}', GETDATE()) ";
            strSQL = string.Format(strSQL, strTable, strObjId, strImpSid, strOfcId);
            dbCon.ExecuteNonQuery(strSQL);
            #endregion SQL
        }

        private void SaveOfficeDownload(string strOfcId, string strImpSid, string strTable, string strObjId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strOdlId = string.Empty;
            #endregion Variables

            #region Save
            dt = GetData("um_office_download", "ofc_id", strOfcId, dbCon);
            if (utilCollections.HasRows(dt))
            {
                strOdlId = dt.Rows[0]["odl_id"].ToString();
                UpdateOfficeDownload(strOdlId, strImpSid, strTable, strObjId, dbCon);
            }
            else
            {
                InsertOfficeDownload(strOfcId, strImpSid, strTable, strObjId, dbCon);
            }
            #endregion Save
        }

        private void UpdateContact(DataRow dr, DBConnection dbCon)
        {
            #region Variables
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "UPDATE um_office_user " +
                "SET ousr_password = '{1}', ousr_password_format = {2}, ousr_password_salt = '{3}', " +
                "ousr_first_name = '{4}', ousr_last_name = '{5}', " +
                "ousr_email = '{6}', ousr_phone = '{7}', ousr_skype = '{8}', ousr_position = '{9}', " +
                "ousr_active = {10}, hnr_id = '{11}', lng_id = '{12}', " +
                "usr_id_update = '{13}', " +
                "usr_date_update = '{14}' " +
                "WHERE ousr_id = '{0}' ";
            strSQL = string.Format(strSQL, dr["usr_id"].ToString(), utilFormatting.StringForSQL(dr["usr_password"].ToString()), dr["usr_password_format"].ToString(), dr["usr_password_salt"].ToString(),
                utilFormatting.StringForSQL(dr["usr_first_name"].ToString()), utilFormatting.StringForSQL(dr["usr_last_name"].ToString()),
                utilFormatting.StringForSQL(dr["usr_email"].ToString()), utilFormatting.StringForSQL(dr["usr_phone"].ToString()), utilFormatting.StringForSQL(dr["usr_skype"].ToString()), utilFormatting.StringForSQL(dr["usr_position"].ToString()),
                Convert.ToInt32(Convert.ToBoolean(dr["usr_active"])), dr["hnr_id"].ToString(), dr["lng_id"].ToString(),
                dr["usr_id_update"].ToString(),
                Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"));
            dbCon.ExecuteNonQuery(strSQL);
            #endregion SQL
        }

        private void UpdateOffice(DataRow dr, DBConnection dbCon)
        {
            #region Variables
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "UPDATE um_office " +
                    "SET ofc_name = '{1}', ofc_server_id = '{2}', " +
                    "ofc_email = '{3}', ofc_phone = '{4}', ofc_address = '{5}', " +
                    "ofc_app_version = '{6}', " +
                    " otp_id = '{7}', ousr_id_contact = '{8}', " +
                    "usr_id_update = '{9}', usr_date_update = '{10}',district_id = '{11}' " +
                    "WHERE ofc_id = '{0}' ";
            strSQL = string.Format(strSQL, dr["ofc_id"].ToString(), utilFormatting.StringForSQL(dr["ofc_name"].ToString()), utilFormatting.StringForSQL(dr["ofc_server_id"].ToString()),
                utilFormatting.StringForSQL(dr["ofc_email"].ToString()), utilFormatting.StringForSQL(dr["ofc_phone"].ToString()), utilFormatting.StringForSQL(dr["ofc_address"].ToString()),
                utilFormatting.StringForSQL(dr["ofc_app_version"].ToString()),
                dr["otp_id"].ToString(), dr["usr_id_contact"].ToString(),
                dr["usr_id_update"].ToString(),
                Convert.ToDateTime(dr["usr_date_update"]).ToString("dd MMM yyyy HH:mm:ss"), dr["district_id"].ToString());
            dbCon.ExecuteNonQuery(strSQL);
            #endregion SQL
        }

        private void UpdateOfficeDownload(string strOdlId, string strImpSid, string strTable, string strObjId, DBConnection dbCon)
        {
            #region Variables
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "UPDATE um_office_download " +
                "SET odl_table = '{1}', odl_obj_id ='{2}', imp_sid = {3}, usr_date_update = GETDATE() " +
                "WHERE odl_id = '{0}' ";
            strSQL = string.Format(strSQL, strOdlId, strTable, strObjId, strImpSid);
            dbCon.ExecuteNonQuery(strSQL);
            #endregion SQL
        }
        #endregion Function Methods

        #region Get Methods
        public DataTable GetData(string strTable, string strPrimaryKey, string strID, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("SELECT * FROM {0} WHERE {1} = '{2}' ", strTable, strPrimaryKey, strID);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetImportHistory(string strID, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("SELECT * FROM um_import_history WHERE imp_sid = {0} ", strID);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        private string GetImportId(string strSsnId)
        {
            return strSsnId.Substring(0, strSsnId.Length - pintSessionDateLength);
        }

        private DateTime GetSessionDate(string strSsnId)
        {
            #region Variables
            string strDate = strSsnId.Substring(strSsnId.Length - pintSessionDateLength, pintSessionDateLength);
            #endregion Variables

            #region Get Date
            strDate = DateTime.Now.Year.ToString() + "-" + strDate.Substring(0, 2) + "-" + strDate.Substring(2, 2) + " " + strDate.Substring(4, 2) + ":" + strDate.Substring(6, 2) + ":00";
            #endregion Get Date

            return Convert.ToDateTime(strDate);
        }

        private string GetSessionId(string strImpId)
        {
            return strImpId + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
        }
        #endregion Get Methods
        #endregion Private
    }
}

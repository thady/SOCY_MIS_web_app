using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using PSAUtilsWeb;
using System.Security.Cryptography;

namespace SOCY_WEBAppTest
{
    public class utilUserAccess
    {
        #region Variables
        #region Applications
        public const string cAppBantuPortal = "1";
        #endregion Applications

        #region Permissions
        #region User Management
        public const string cUMSearchUser = "100";
        public const string cUMCreateUser = "101";
        public const string cUMViewUser = "102";
        public const string cUMEditUser = "103";

        public const string cUMResetUserPassword = "104";
        public const string cUMDeactivateUser = "105";
        public const string cUMDeleteUser = "106";

        public const string cUMAssignRolesToUser = "107";
        public const string cUMAssignPermissionsToUser = "108";
        public const string cUMAssignUMAdminRole = "109";

        public const string cUMSearchRole = "110";
        public const string cUMCreateRole = "111";
        public const string cUMViewRole = "112";
        public const string cUMEditRole = "113";
        public const string cUMDeleteNonAdminRole = "114";

        public const string cUMSearchOffice = "115";
        public const string cUMViewOffice = "116";
        public const string cUMEditOffice = "117";
        #endregion User Management

        #region Report Management
        public const string cRMViewResultArea01Indicators = "200";
        public const string cRMViewResultArea02Indicators = "201";
        public const string cRMViewDREAMSIndicators = "202";
        public const string cRMViewSILCIndicators = "203";
        public const string cRMViewServiceIndicators = "204";
        public const string cRMViewOtherIndicators = "205";
        public const string cRMViewCapturedDataReports = "206";
        #endregion Report Management

        #region Capture Reports
        public const string cCROVCIdentificationAndPrioritization = "300";
        public const string cCRHouseholdData = "301";
        public const string cCRHouseholdMemberData = "302";
        public const string cCRHouseholdAssessmentData = "303";
        public const string cCRHouseholdAssessmentMemberData = "304";
        public const string cCRHomeVisitData = "305";
        public const string cCRHouseholdReferral = "306";
        public const string cCRActivityTraining = "307";
        public const string cCRApprenticeshipRegister = "308";
        public const string cCRServiceRegister = "309";
        public const string cCRAlternativeCarePanel = "310";
        public const string cCRCBSDResourceAllocation = "311";
        public const string cCRCBSDStaffAppraisalTracking = "312";
        public const string cCRDistrictOVCCheckList = "313";
        public const string cCRInstitutionalCareSummary = "314";
        public const string cCRDREAMSEnrolment = "315";
        public const string cCRSILCGroups = "316";
        public const string cCRSILCGroupMembers = "317";
        public const string cCRSILCFinancialRegister = "318";
        public const string cCRSILCSavingsRegister = "319";
        public const string cCRSocialWorker = "320";
        #endregion Capture Reports
        #endregion Permissions

        #region Role Types
        public const string cRTUMAdmin = "1";
        #endregion Role Types

        #region User Types
        public const string cUTPSuperUser = "1";
        #endregion User Types
        #endregion Variables

        /// <summary>
        /// Generates a Salt value
        /// </summary>
        /// <returns>The Salt value</returns>
        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        #region Get Methods
        /// <summary>
        /// Gets the name of the specified User
        /// </summary>
        /// <param name="strID">User for who's name must be returned</param>
        /// <param name="dbCon">Database connection that must be used</param>
        /// <returns>The User's name</returns>
        public string GetUserName(string strID, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strResult = string.Empty;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT usr_first_name + ' ' + usr_last_name AS usr_name FROM um_user " +
                     "WHERE usr_id = '{0}' ";
            strSQL = string.Format(strSQL, strID);
            dt = dbCon.ExecuteQueryDataTable(strSQL);

            if (utilCollections.HasRows(dt))
                strResult = dt.Rows[0]["usr_name"].ToString();
            #endregion SQL

            return strResult;
        }


        #region Get Users With Permission
        public DataTable GetUsersWithPermission(string strPrmID, DBConnection dbCon)
        {
            return GetUsersWithPermissionPrivate(strPrmID, "", -1, dbCon);
        }

        public DataTable GetUsersWithPermission(string strPrmID, bool blnActiveStatus, DBConnection dbCon)
        {
            return GetUsersWithPermissionPrivate(strPrmID, "", Convert.ToInt32(blnActiveStatus), dbCon);
        }

        public DataTable GetUsersWithPermission(string strPrmID, string strIncludeUserID, DBConnection dbCon)
        {
            return GetUsersWithPermissionPrivate(strPrmID, strIncludeUserID, -1, dbCon);
        }

        public DataTable GetUsersWithPermission(string strPrmID, string strIncludeUserID, bool blnActiveStatus, DBConnection dbCon)
        {
            return GetUsersWithPermissionPrivate(strPrmID, strIncludeUserID, Convert.ToInt32(blnActiveStatus), dbCon);
        }

        /// <summary>
        /// Gets Users with specified Permission
        /// </summary>
        /// <param name="strPrmID">Permission Users must have</param>
        /// <param name="strIncludeUserID">User to be included regardless of Permission and Status, blank if must be excluded</param>
        /// <param name="intActiveStatus">Status of the Users to be returned, -1 if must be excluded</param>
        /// <param name="dbCon">Database connection that must be used</param>
        /// <returns>DataTable containing the Users</returns>
        private DataTable GetUsersWithPermissionPrivate(string strPrmID, string strIncludeUserID, int intActiveStatus, DBConnection dbCon)
        {
            #region Variables
            utilUserAccess uAccess = new utilUserAccess();
            DataTable dt = null;
            string strSQL = string.Empty;
            string strWHERE = string.Empty;
            #endregion Variables

            #region SQL
            #region User Status - Check if Users must be Active, InActive or Exclude Check
            if (intActiveStatus == -1)
                strWHERE = "";
            else
                strWHERE = string.Format("WHERE usr.usr_active = {0} ", intActiveStatus);
            #endregion User Status - Check if Users must be Active, InActive or Exclude Check

            strSQL = "CREATE TABLE #temp (usr_id VARCHAR(50)) " +
                     "INSERT INTO #temp " +
                     "SELECT upr.usr_id " +
                     "FROM um_user_permission upr " +
                     "INNER JOIN um_permission prm ON upr.prm_id = prm.prm_id " +
                     "WHERE upr.prm_id = '{0}' AND upr.upr_active = 1 " +
                     "UNION " +
                     "SELECT url.usr_id " +
                     "FROM um_user_role url " +
                     "INNER JOIN um_role_permission rlpr ON url.rl_id = rlpr.rl_id " +
                     "INNER JOIN um_role rl ON rlpr.rl_id = rl.rl_id " +
                     "WHERE rlpr.prm_id = '{0}' AND rl.rl_active = 1 AND url.url_active = 1 " +
                     "AND NOT url.usr_id IN (SELECT usr_id FROM um_user_permission WHERE prm_id = '{0}' AND upr_active = 0) " +
                     "SELECT usr.usr_id, usr.usr_first_name + ' ' + usr.usr_last_name AS usr_name " +
                     "FROM um_user usr INNER JOIN #temp tmp ON usr.usr_id = tmp.usr_id " +
                     "{1} ";

            #region Include User Regardless of Permission
            if (strIncludeUserID.Length != 0)
            {
                if (strIncludeUserID.IndexOf("'") == -1)
                    strIncludeUserID = "'" + strIncludeUserID + "'";
                strSQL = strSQL + string.Format("UNION " +
                                                "SELECT usr.usr_id, usr.usr_first_name + ' ' + usr.usr_last_name AS usr_name FROM um_user usr " +
                                                "WHERE usr.usr_id IN ({0}) ", strIncludeUserID);
            }
            #endregion Include User Regardless of Permission

            strSQL = strSQL + "ORDER BY usr_name " +
                     "DROP TABLE #temp ";
            strSQL = string.Format(strSQL, strPrmID, strWHERE);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }
        #endregion Get Users With Permission
        #endregion Get Methods

        #region LogIn
        /// <summary>
        /// Validates specified email and password against the values in the database
        /// </summary>
        /// <param name="strUserEmail">Email of User to be logged in</param>
        /// <param name="strPassword">Password of User to be logged in</param>
        /// <param name="strLngID">Language Message must be returned in</param>
        /// <param name="dbCon">Database connection that must be used</param>
        /// <returns>Returns string array, value key pair, based on log in result</returns>
        public string[] LogIn(string strUserEmail, string strPassword, DBConnection dbCon)
        {
            #region Variables
            utilLanguageTranslation utilLT = new utilLanguageTranslation();
            DataTable dt = null;
            int intPasswordFormat = -1;
            string[] arrResult = new string[2];
            string strPasswordCheck = string.Empty;
            string strSalt = string.Empty;
            string strSQL = string.Empty;
            string strUserID = string.Empty;
            #endregion Variables

            #region LogIn

            strSQL = "SELECT usr_id, usr_password, usr_password_format, usr_password_salt, usr_active FROM um_user WHERE LOWER(usr_email) = '{0}' ";
            strSQL = string.Format(strSQL, strUserEmail.ToLower());
            dt = dbCon.ExecuteQueryDataTable(strSQL);

            if (utilCollections.HasRows(dt))
            {
                if (Convert.ToBoolean(dt.Rows[0]["usr_active"]))
                {
                    strUserID = dt.Rows[0]["usr_id"].ToString();
                    if (utilFormatting.IsInt(dt.Rows[0]["usr_password_format"].ToString()))
                    {
                        intPasswordFormat = Convert.ToInt32(dt.Rows[0]["usr_password_format"]);

                        switch (intPasswordFormat)
                        {
                            case 0:
                                strPasswordCheck = dt.Rows[0]["usr_password"].ToString();
                                break;

                            case 1:
                                strPasswordCheck = dt.Rows[0]["usr_password"].ToString();
                                strSalt = dt.Rows[0]["usr_password_salt"].ToString();
                                strPassword = utilEncryption.HashText(strPassword, strSalt);
                                break;

                            default:
                                arrResult[0] = "2";
                                arrResult[1] = utilLT.GetMessageTranslation(utilSOCYWeb.cMIDPasswordFormatInvalid, dbCon.dbCon);
                                break;
                        }

                        if (strPasswordCheck != string.Empty)
                        {
                            if (strPassword.Equals(strPasswordCheck))
                            {
                                arrResult[0] = "0";
                                arrResult[1] = strUserID;
                            }
                            else
                            {
                                arrResult[0] = "3";
                                arrResult[1] = utilLT.GetMessageTranslation(utilSOCYWeb.cMIDPasswordIncorrect, dbCon.dbCon);
                            }
                        }
                    }
                    else
                    {
                        arrResult[0] = "2";
                        arrResult[1] = utilLT.GetMessageTranslation(utilSOCYWeb.cMIDPasswordFormatInvalid, dbCon.dbCon);
                    }
                }
                else
                {
                    arrResult[0] = "4";
                    arrResult[1] = utilLT.GetMessageTranslation(utilSOCYWeb.cMIDAccountInactive, dbCon.dbCon);
                }
            }
            else
            {
                arrResult[0] = "1";
                arrResult[1] = utilLT.GetMessageTranslation(utilSOCYWeb.cMIDEmailAddressInvalid, dbCon.dbCon);
            }
            #endregion LogIn

            return arrResult;
        }
        #endregion LogIn

        #region Permissions
        /// <summary>
        /// Checks if the specified User has the specified Permission
        /// </summary>
        /// <param name="strUserID">User to be checked</param>
        /// <param name="strPermissionID">Permission to be checked</param>
        /// <param name="dbCon">Database connection that must be used</param>
        /// <returns>Boolean based on the result</returns>
        public Boolean HasPermission(string strUserID, string strPermissionID, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            bool blnResult = false;
            string strSQL = string.Empty;
            #endregion Variables

            #region Super User Check
            blnResult = HasUserType(strUserID, cUTPSuperUser, dbCon);
            #endregion Super User Check

            #region SQL

            if (!blnResult)
            {
                strSQL = "SELECT upr.prm_id " +
                         "FROM um_user_permission upr " +
                         "INNER JOIN um_permission prm ON upr.prm_id = prm.prm_id " +
                         "WHERE upr.usr_id = '{0}' AND upr.prm_id = '{1}' " +
                         "AND prm.prm_active = 1 AND upr.upr_active = 1 " +
                         "UNION " +
                         "SELECT rlpr.prm_id " +
                         "FROM um_user_role url " +
                         "INNER JOIN um_role_permission rlpr ON url.rl_id = rlpr.rl_id " +
                         "INNER JOIN um_permission prm ON rlpr.prm_id = prm.prm_id " +
                         "INNER JOIN um_role rl ON rlpr.rl_id = rl.rl_id " +
                         "WHERE url.usr_id = '{0}' AND rlpr.prm_id = '{1}' " +
                         "AND prm.prm_active = 1 AND rl.rl_active = 1 AND url.url_active = 1 " +
                         "AND NOT rlpr.prm_id IN ( " +
                         "SELECT prm_id " +
                         "FROM um_user_permission " +
                         "WHERE usr_id = '{0}' AND upr_active = 0) ";
                strSQL = string.Format(strSQL, strUserID, strPermissionID);

                dt = dbCon.ExecuteQueryDataTable(strSQL);
                blnResult = utilCollections.HasRows(dt);
            }

            #endregion SQL

            return blnResult;
        }
        #endregion Permissions

        #region User Type
        /// <summary>
        /// Checks if the specified User has the specified User Type
        /// </summary>
        /// <param name="strUserID">User to be checked</param>
        /// <param name="strUserTypeID">User Type to be checked</param>
        /// <param name="dbCon">Database connection that must be used</param>
        /// <returns>Boolean based on the result</returns>
        public Boolean HasUserType(string strUserID, string strUserTypeID, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            bool blnResult = false;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = string.Format("SELECT usr_id FROM um_user_type WHERE usr_id = '{0}' AND utp_id = '{1}' ", strUserID, strUserTypeID);

            dt = dbCon.ExecuteQueryDataTable(strSQL);
            blnResult = utilCollections.HasRows(dt);
            #endregion SQL

            return blnResult;
        }
        #endregion User Type
    }
}
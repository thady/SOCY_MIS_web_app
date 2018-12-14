using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PSAUtilsWeb;
using Telerik.Web.UI;
using System.Data;
using System.Data.SqlClient;

namespace SOCY_WEBAppTest
{
    public class utilSOCYWeb
    {
        #region Constants
        #region Defaults
        public const string cArrOtherID = "Other";
        public const string cDownloadDelimiter = "~~88~~";
        public const string cEmptyDateValue = "1900/01/01";
        public const string cEmptyListValue = "-1";
        public const string cLanguageDefault = "EN";
        public const string cMaxGUID = "ffffffff-ffff-ffff-ffff-ffffffffffff";
        public const string cMaxGUIDX = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
        public const string cReplaceValue = "REPLACE_VALUE";
        #endregion Defaults

        #region Gender
        public const string cGNDFemale = "f05d3f3c-9aac-4f12-b0cd-1c4ae9294da3";
        public const string cGNDMale = "m26e435b-1478-4978-aad5-58c3677a1f70";
        #endregion Gender

        #region Icons
        public const string cIconInfo = "Images/info.png";
        public const string cIconSuccess = "Images/smiley.png";
        public const string cIconWarning = "Images/warning.png";
        #endregion Icons

        #region Member Type
        public const string cMTExternal = "2";
        public const string cMTHousehold = "1";
        #endregion Member Type

        #region Message IDs
        public const string cMIDAccountInactive = "20";
        public const string cMIDEmailAddressInvalid = "21";
        public const string cMIDEmailAddressInUse = "13";
        public const string cMIDEmailFormatInvalid = "1";
        public const string cMIDEmptyListMultiSelect = "9";
        public const string cMIDEmptyListSingleSelect = "10";
        public const string cMIDUserDeleteValidation = "17";
        public const string cMIDUserNotFoundByEmail = "18";
        public const string cMIDListRoleText = "2";
        public const string cMIDPasswordConfirmMatch = "11";
        public const string cMIDPasswordFormat = "12";
        public const string cMIDPasswordFormatInvalid = "22";
        public const string cMIDPasswordIncorrect = "23";
        public const string cMIDRequiredFields = "5";
        public const string cMIDRoleCannotBeDeleted = "37";
        public const string cMIDRoleDeleted = "36";
        public const string cMIDRoleMinimumPermission = "6";
        public const string cMIDRoleNameExists = "7";
        public const string cMIDSaved = "8";
        public const string cMIDTextLength8000 = "40";
        public const string cMIDUserCannotBeDeleted = "25";
        public const string cMIDUserDeleteConformation = "15";
        public const string cMIDUserDeleted = "24";
        #endregion Messgae IDs

        #region Language IDs
        public const string cLIDEnglish = "EN";
        #endregion Language IDs

        #region Office Status
        public const string cOSTRejected = "0";
        public const string cOSTValidated = "1";
        public const string cOSTWaitingValidation = "2";
        #endregion Office Status

        #region Session Variables
        public const string cSVLanguage = "SOCYLanguage";
        public const string cSVSearchKeep = "SOCYSearchKeep";
        public const string cSVSearchOffice = "SOCYSearchRole";
        public const string cSVSearchRole = "SOCYSearchRole";
        public const string cSVSearchUser = "SOCYSearchUser";
        public const string cSVUserID = "UMUserID";
        #endregion Session Variables

        #region Social Worker Type
        public const string cSWTParaSocialWorker = "2";
        public const string cSWTSocialWorker = "1";
        #endregion Social Worker Type

        #region Reports Capture
        public const string cRCOVCIdentification = "OVCIdentification";
        public const string cRCHousehold = "Household";
        public const string cRCHouseholdMember = "HouseholdMember";
        public const string cRCHouseholdAssessment = "HouseholdAssessment";
        public const string HouseholdAssessment_New = "HouseholdAssessment_New";
        public const string cRCHouseholdAssessmentMember = "HouseholdAssessmentMember";
        public const string cRCHomeVisit = "HomeVisit";
        public const string cRCHomeVisitArchive = "HomeVisitArcive";
        public const string cRCHomeVisitMember = "HomeVisitMember";
        public const string cRCHouseholdReferral = "HouseholdReferral";
        public const string cRCSocialWorker = "SocialWorker";
        public const string cRCActivityTraining = "ActivityTraining";
        public const string cRCApprenticeshipRegister = "ApprenticeshipRegister";
        public const string cRCServiceRegister = "ServiceRegister";
        public const string cRCAlternativeCarePanel = "AlternativeCarePanel";
        public const string cRCCBSDResourceAllocation = "CBSDResourceAllocation";
        public const string cRCCBSDStaffAppraisalTracking = "CBSDStaffAppraisalTracking";
        public const string cRCDistrictOVCCheckList = "DistrictOVCCheckList";
        public const string cRCInstitutionalCareSummary = "InstitutionalCareSummary";
        public const string cRCDREAMSEnrolment = "DREAMSEnrolment";
        public const string cRCSILCGroups = "SILCGroups";
        public const string cRCSILCGroupMembers = "SILCGroupMembers";
        public const string cRCSILCFinancialRegister = "SILCFinancialRegister";
        public const string cRCSILCSavingsRegister = "SILCSavingsRegister";
        public const string cRCHIP = "HIP";
        public const string cRCRAS = "RASM";
        public const string cRCRASNEW = "RASMNEW";
        public const string cRCLinkages = "Linkages";
        public const string CRCommunityTrainingRegister = "CommunityTrainingRegister";
        public const string CRHomeVisitAggregate = "hv_aggregate";
        public const string CRHouseholdsNotVisited = "HouseholdsNotVisited"; 

        public const string CRagroEnterpriseRanking = "agroEnterpriseRanking";
        public const string CRcottageEnterpriseRanking = "cottageEnterpriseRanking";
        public const string CRapprenticeshipSkillAquisitionTracking = "apprenticeshipSkillAquisitionTracking";
        public const string CRYouthTrainingCompletiion = "YouthTrainingCompletiion";
        public const string CRYouthAssessmentScoring = "YouthAssessmentScoring";
        public const string CRovcViralLoadMonitoring = "ovcViralLoadMonitoring";
        public const string CRbeneficiarySchoolReadiness = "beneficiarySchoolReadiness";
        public const string CRbenYouthTrainingInventory = "benYouthTrainingInventory";
        public const string CRYouthSavingsRegister = "YouthSavingsRegister";
        #endregion Reports Capture

        #region Trigger Action
        public const string cTADelete = "3";
        public const string cTAInsert = "1";
        #endregion Trigger Action

        #region Web Config Keys
        public const string cWCKConnectionImport = "SOCY_IMPORT";
        public const string cWCKConnection = "SOCY_LIVE";
        public const string cWCKDBArchive = "SOCY_ARCHIVE_DB";
        public const string cWCKDownloadRecords = "DOWNLOAD_RECORDS";
        public const string cWCKReportFolder = "REPORT_FOLDER";
        #endregion Web Config Keys
        #endregion Constants

        #region Get Methods
        /// <summary>
        /// Gets a list of all the Applications
        /// </summary>
        /// <returns>DataTable of all the Applications</returns>
        public DataTable GetApplications()
        {
            #region Variables
            DBConnection dbCon = new DBConnection(cWCKConnection);

            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            try
            {
                #region SQL
                strSQL = "SELECT * FROM um_application ORDER BY app_order ";
                strSQL = string.Format(strSQL);
                dt = dbCon.ExecuteQueryDataTable(strSQL);
                #endregion SQL
            }
            finally
            {
                dbCon.Dispose();
            }

            return dt;
        }
        #endregion Get Methods

        #region District, Sub County and Ward
        #region Public Methods
        public DataTable GetDistrictParents(string strId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT dst.rgn_id " +
                "FROM lst_district dst " +
                "WHERE dst.dst_id = '{0}' ";
            strSQL = string.Format(strSQL, strId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetDistrictByParents(string strRgnId, string strLngId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT dst.dst_id AS lt_id, dst.dst_name AS lt_name " +
                "FROM lst_district dst " +
                "WHERE dst.lng_id = '{0}' ";
            if (strRgnId.Length != 0)
                strSQL = strSQL + "AND dst.rgn_id = '" + strRgnId + "' ";
            strSQL = strSQL + "ORDER BY lt_name ";
            strSQL = string.Format(strSQL, strLngId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetSubCountyParents(string strId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT sct.dst_id, dst.rgn_id " +
                "FROM lst_sub_county sct " +
                "INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id " +
                "WHERE sct.sct_id = '{0}' ";
            strSQL = string.Format(strSQL, strId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetSubCountyParentsWithNames(string strId, string strLngId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT dst.dst_id, dst.dst_name, rgn.rgn_id, rgn.rgn_name " +
                "FROM lst_sub_county sct " +
                "INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id " +
                "INNER JOIN lst_region rgn ON dst.rgn_id = rgn.rgn_id " +
                "WHERE sct.sct_id = '{0}' " +
                "AND sct.lng_id = '{1}' AND dst.lng_id = '{1}' AND rgn.lng_id = '{1}' ";
            strSQL = string.Format(strSQL, strId, strLngId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetSubCountyByParents(string strRgnId, string strDstId, string strLngId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT sct.sct_id AS lt_id, sct.sct_name AS lt_name " +
                "FROM lst_sub_county sct " +
                "INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id " +
                "WHERE sct.lng_id = '{0}' ";
            if (strDstId.Length != 0)
                strSQL = strSQL + "AND sct.dst_id = '" + strDstId + "' ";
            if (strRgnId.Length != 0)
                strSQL = strSQL + "AND dst.rgn_id = '" + strRgnId + "' ";
            strSQL = strSQL + "ORDER BY lt_name ";
            strSQL = string.Format(strSQL, strLngId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetWardParents(string strId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT sct.sct_id, sct.dst_id, dst.rgn_id " +
                "FROM lst_ward wrd " +
                "INNER JOIN lst_sub_county sct ON wrd.sct_id = sct.sct_id " +
                "INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id " +
                "WHERE wrd.wrd_id = '{0}' ";
            strSQL = string.Format(strSQL, strId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetWardParentsWithNames(string strId, string strLngId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT sct.sct_id, sct.sct_name, dst.dst_id, dst.dst_name, rgn.rgn_id, rgn.rgn_name " +
                "FROM lst_ward wrd " +
                "INNER JOIN lst_sub_county sct ON wrd.sct_id = sct.sct_id " +
                "INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id " +
                "INNER JOIN lst_region rgn ON dst.rgn_id = rgn.rgn_id " +
                "WHERE wrd.wrd_id = '{0}' " +
                "AND wrd.lng_id = '{1}' AND sct.lng_id = '{1}' AND dst.lng_id = '{1}' AND rgn.lng_id = '{1}' ";
            strSQL = string.Format(strSQL, strId, strLngId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public DataTable GetWardByParents(string strRgnId, string strDstId, string strSctId, string strLngId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT wrd.wrd_id AS lt_id, wrd.wrd_name AS lt_name " +
                "FROM lst_ward wrd " +
                "INNER JOIN lst_sub_county sct ON wrd.sct_id = sct.sct_id " +
                "INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id " +
                "WHERE wrd.lng_id = '{0}' ";
            if (strDstId.Length != 0)
                strSQL = strSQL + "AND sct.dst_id = '" + strDstId + "' ";
            if (strSctId.Length != 0)
                strSQL = strSQL + "AND sct.sct_id = '" + strSctId + "' ";
            if (strRgnId.Length != 0)
                strSQL = strSQL + "AND dst.rgn_id = '" + strRgnId + "' ";
            strSQL = strSQL + "ORDER BY lt_name ";
            strSQL = string.Format(strSQL, strLngId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public void LoadListsArea(string strDstId, string strSctId,
            RadComboBox cbDistrict, RadComboBox cbSubCounty,
            string strLngId, DBConnection dbCon)
        {
            LoadListsArea("", strDstId, strSctId, "",
                null, cbDistrict, cbSubCounty, null,
                strLngId, dbCon);
        }

        public void LoadListsArea(string strDstId, string strSctId, string strWrdId,
            RadComboBox cbDistrict, RadComboBox cbSubCounty, RadComboBox cbWard,
            string strLngId, DBConnection dbCon)
        {
            LoadListsArea("", strDstId, strSctId, strWrdId,
                null, cbDistrict, cbSubCounty, cbWard,
                strLngId, dbCon);
        }

        public void LoadListsArea(string strRgnId, string strDstId, string strSctId, string strWrdId,
            RadComboBox cbRegion, RadComboBox cbDistrict, RadComboBox cbSubCounty, RadComboBox cbWard,
            string strLngId, DBConnection dbCon)
        {
            #region Variables
            utilLanguageTranslation utilLT = null;
            utilListTable uLT = null;

            DataTable dt = null;
            string strEmptySingleSelect = string.Empty;
            #endregion Variables

            utilLT = new utilLanguageTranslation();
            utilLT.Language = strLngId;
            strEmptySingleSelect = utilLT.GetMessageTranslation(cMIDEmptyListSingleSelect, dbCon.dbCon);

            #region Get Parent Values
            if (strWrdId.Length != 0 && !strWrdId.Equals(cEmptyListValue))
            {
                dt = GetWardParents(strWrdId, dbCon);
                if (utilCollections.HasRows(dt))
                {
                    strSctId = dt.Rows[0]["sct_id"].ToString();
                    strDstId = dt.Rows[0]["dst_id"].ToString();
                    if (cbRegion != null)
                        strRgnId = dt.Rows[0]["rgn_id"].ToString();
                }
                else
                {
                    strWrdId = string.Empty;
                    strSctId = string.Empty;
                    strDstId = string.Empty;
                    strRgnId = string.Empty;
                }
            }
            else if (strSctId.Length != 0 && !strSctId.Equals(cEmptyListValue))
            {
                dt = GetSubCountyParents(strSctId, dbCon);
                if (utilCollections.HasRows(dt))
                {
                    strDstId = dt.Rows[0]["dst_id"].ToString();
                    if (cbRegion != null)
                        strRgnId = dt.Rows[0]["rgn_id"].ToString();
                }
                else
                {
                    strSctId = string.Empty;
                    strDstId = string.Empty;
                    strRgnId = string.Empty;
                }
                strWrdId = string.Empty;
            }
            else if (strDstId.Length != 0 && !strDstId.Equals(cEmptyListValue))
            {
                dt = GetDistrictParents(strDstId, dbCon);
                if (utilCollections.HasRows(dt))
                {
                    if (cbRegion != null)
                        strRgnId = dt.Rows[0]["rgn_id"].ToString();
                }
                else
                {
                    strDstId = string.Empty;
                    strRgnId = string.Empty;
                }
                strWrdId = string.Empty;
                strSctId = string.Empty;
            }
            else if (strRgnId.Length != 0 && !strRgnId.Equals(cEmptyListValue))
            {
                strWrdId = string.Empty;
                strSctId = string.Empty;
                strDstId = string.Empty;
            }
            else
            {
                strWrdId = string.Empty;
                strSctId = string.Empty;
                strDstId = string.Empty;
                strRgnId = string.Empty;
            }
            #endregion Get Parent Values

            #region Load Lists
            uLT = new utilListTable();

            if (cbRegion != null)
            {
                dt = uLT.GetData("lst_region", true, strRgnId, false, strLngId, dbCon.dbCon);
                dt = utilCollections.AddEmptyItemFront(dt, "lt_id", "lt_name", cEmptyListValue, strEmptySingleSelect);
                utilControls.RadComboBoxFill(cbRegion, dt, "lt_id", "lt_name");

                if (strRgnId.Length != 0)
                    cbRegion.SelectedValue = strRgnId;
                else
                    cbRegion.SelectedIndex = 0;
            }

            if (cbDistrict != null)
            {
                dt = GetDistrictByParents(strRgnId, strLngId, dbCon);
                dt = utilCollections.AddEmptyItemFront(dt, "lt_id", "lt_name", cEmptyListValue, strEmptySingleSelect);
                utilControls.RadComboBoxFill(cbDistrict, dt, "lt_id", "lt_name");

                if (strDstId.Length != 0)
                    cbDistrict.SelectedValue = strDstId;
                else
                    cbDistrict.SelectedIndex = 0;
            }

            if (cbSubCounty != null)
            {
                dt = GetSubCountyByParents(strRgnId, strDstId, strLngId, dbCon);
                dt = utilCollections.AddEmptyItemFront(dt, "lt_id", "lt_name", cEmptyListValue, strEmptySingleSelect);
                utilControls.RadComboBoxFill(cbSubCounty, dt, "lt_id", "lt_name");

                if (strSctId.Length != 0)
                    cbSubCounty.SelectedValue = strSctId;
                else
                    cbSubCounty.SelectedIndex = 0;
            }

            if (cbWard != null)
            {
                dt = GetWardByParents(strRgnId, strDstId, strSctId, strLngId, dbCon);
                dt = utilCollections.AddEmptyItemFront(dt, "lt_id", "lt_name", cEmptyListValue, strEmptySingleSelect);
                utilControls.RadComboBoxFill(cbWard, dt, "lt_id", "lt_name");

                if (strWrdId.Length != 0)
                    cbWard.SelectedValue = strWrdId;
                else
                    cbWard.SelectedIndex = 0;
            }
            #endregion Load Lists
        }
        #endregion Public Methods
        #endregion District, Sub County and Ward

        #region CSO and Partner/Region
        #region Private Methods
        private DataTable GetCSOByParents(string strPrtId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT cso.cso_id AS lt_id, cso.cso_name AS lt_name " +
                "FROM lst_cso cso ";
            if (strPrtId.Length != 0)
                strSQL = strSQL + "WHERE cso.prt_id = '" + strPrtId + "' ";
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        private DataTable GetCSOParent(string strId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT DISTINCT cso.prt_id " +
                "FROM lst_cso cso " +
                "WHERE cso.cso_id = '{0}' ";
            strSQL = string.Format(strSQL, strId);
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }
        #endregion Private Methods

        #region Public Methods
        public DataTable GetListParentCSO(bool blnActiveOnly, string strId, DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = strSQL + "SELECT prt_id AS lt_id, prt_name AS lt_name, '1' AS lt_order FROM lst_partner ";
            if (blnActiveOnly)
                strSQL = strSQL + "WHERE prt_active = 1 ";
            strSQL = strSQL + "UNION SELECT cso_id AS lt_id, cso_name AS lt_name, '2' AS lt_order FROM lst_cso ";
            if (blnActiveOnly)
                strSQL = strSQL + "WHERE cso_active = 1 ";
            if (strId.Length != 0)
            {
                strSQL = strSQL + string.Format("UNION SELECT prt_id AS lt_id, prt_name AS lt_name, '1' AS lt_order FROM lst_partner WHERE prt_id = '{0}' " +
                    "UNION SELECT cso_id AS lt_id, cso_name AS lt_name, '2' AS lt_order FROM lst_cso WHERE cso_id = '{0}' ", strId);
            }
            strSQL = strSQL + "ORDER BY lt_order, lt_name ";
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }

        public void LoadListsOrganization(string strPrtId, string strCsoId,
            RadComboBox cbPartner, RadComboBox cbCSO,
            string strLngId, DBConnection dbCon)
        {
            #region Variables
            utilLanguageTranslation utilLT = null;
            utilListTable uLT = null;

            DataTable dt = null;
            string strEmptySingleSelect = string.Empty;
            #endregion Variables

            utilLT = new utilLanguageTranslation();
            utilLT.Language = strLngId;
            strEmptySingleSelect = utilLT.GetMessageTranslation(cMIDEmptyListSingleSelect, dbCon.dbCon);

            #region Get Parent Values
            if (strCsoId.Length != 0 && !strCsoId.Equals(cEmptyListValue))
            {
                dt = GetCSOParent(strCsoId, dbCon);
                if (utilCollections.HasRows(dt))
                {
                    strPrtId = dt.Rows[0]["prt_id"].ToString();
                }
                else
                {
                    strCsoId = string.Empty;
                    strPrtId = string.Empty;
                }
            }
            else if (strPrtId.Length != 0 && !strPrtId.Equals(cEmptyListValue))
            {
                strCsoId = string.Empty;
            }
            #endregion Get Parent Values

            #region Load Lists
            uLT = new utilListTable();

            dt = uLT.GetData("lst_partner", true, strPrtId, dbCon.dbCon);
            dt = utilCollections.AddEmptyItemFront(dt, "lt_id", "lt_name", cEmptyListValue, strEmptySingleSelect);
            utilControls.RadComboBoxFill(cbPartner, dt, "lt_id", "lt_name");

            dt = GetCSOByParents(strPrtId, dbCon);
            dt = utilCollections.AddEmptyItemFront(dt, "lt_id", "lt_name", cEmptyListValue, strEmptySingleSelect);
            utilControls.RadComboBoxFill(cbCSO, dt, "lt_id", "lt_name");
            #endregion Load Lists

            #region Set List Selection
            if (strPrtId.Length != 0)
                cbPartner.SelectedValue = strPrtId;
            else
                cbPartner.SelectedIndex = 0;
            if (strCsoId.Length != 0)
                cbCSO.SelectedValue = strCsoId;
            else
                cbCSO.SelectedIndex = 0;
            #endregion Set List Selection
        }
        #endregion Public Methods
        #endregion CSO and Partner/Region

        #region Quarter Year
        public DateTime[] GetQuarterDates(string strQyId, string strFyId, DBConnection dbCon)
        {
            #region Variables
            DateTime[] dtmDate = new DateTime[2];

            DataTable dtQuarter = null;
            DataTable dtYear = null;

            string strSQL = string.Empty;
            string strYears = string.Empty;
            #endregion Variables

            #region Get Dates
            strSQL = string.Format("SELECT * FROM lst_quarter_year WHERE qy_id = '{0}' ", strQyId);
            dtQuarter = dbCon.ExecuteQueryDataTable(strSQL);

            strSQL = string.Format("SELECT * FROM lst_financial_year WHERE fy_id = '{0}' ", strFyId);
            dtYear = dbCon.ExecuteQueryDataTable(strSQL);
            strYears = dtYear.Rows[0]["fy_name"].ToString();

            if (dtQuarter.Rows[0]["qy_order"].ToString().Equals("1"))
            {
                dtmDate[0] = Convert.ToDateTime(strYears.Substring(0, 4) + "/" + dtQuarter.Rows[0]["qy_begin"].ToString() + "/01");
                dtmDate[1] = Convert.ToDateTime(strYears.Substring(0, 4) + "/" + dtQuarter.Rows[0]["qy_end"].ToString() + "/01").AddMonths(1).AddDays(-1);
            }
            else
            {
                dtmDate[0] = Convert.ToDateTime(strYears.Substring(strYears.Length - 4, 4) + "/" + dtQuarter.Rows[0]["qy_begin"].ToString() + "/01");
                dtmDate[1] = Convert.ToDateTime(strYears.Substring(strYears.Length - 4, 4) + "/" + dtQuarter.Rows[0]["qy_end"].ToString() + "/01").AddMonths(1).AddDays(-1);
            }
            #endregion Get Dates

            return dtmDate;
        }
        #endregion Quarter Year

        #region SILC Groups
        public DataTable GetSILCGroups(DBConnection dbCon)
        {
            #region Variables
            DataTable dt = null;
            string strSQL = string.Empty;
            string strSQLSelect = string.Empty;
            #endregion Variables

            #region SQL
            strSQL = "SELECT sg.sg_id, sg.sg_name FROM silc_group sg " +
                "ORDER BY sg_name ";
            dt = dbCon.ExecuteQueryDataTable(strSQL);
            #endregion SQL

            return dt;
        }
        #endregion SILC Groups
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PSAUtilsWeb;
using System.Configuration;
using SOCY_WEBAppTest.AppCode;

namespace SOCY_WEBAppTest
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Clear Session Variables
                Session[utilSOCYWeb.cSVUserID] = null;
                #endregion Clear Session Variables

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            #region Variables
            LogInDB dbLogIn = new LogInDB();
            string[] arrResult = null;
            string strMessage = ValidatePage();
            string strUserID = string.Empty;
            #endregion Variables

            #region LogIn
            if (strMessage.Length == 0)
            {
                arrResult = dbLogIn.LogIn(txtEmail.Text, txtpassword.Text);

                switch (arrResult[0])
                {
                    case "0":
                        strUserID = arrResult[1];
                        Session[utilSOCYWeb.cSVUserID] = strUserID;
                        RedirectToPage("default");
                        break;
                    default:
                        //lblLoginError.Text = arrResult[1]; throw error
                        break;
                }
            }
            else { }
                //lblLoginError.Text = strMessage;
            #endregion LogIn
        }

        #region Page Methods

        #endregion Page Methods
        #region Private Methods
        /// <summary>
        /// Redirect to specified page
        /// </summary>
        /// <param name="strPage">Page to redirect to</param>
        private void RedirectToPage(string strPage)
        {
            Response.Redirect(strPage + ".aspx");
        }

        /// <summary>
        /// Runs validation again user input
        /// </summary>
        /// <returns>Returns a blank string if the user input validated, else returns the validation error messages</returns>
        private string ValidatePage()
        {
            #region Variables
            DBConnection dbCon = null;
            utilLanguageTranslation utilLT = null;

            bool blnValid = true;
            string[] arrMessage = null;
            string strEmail = txtEmail.Text.Trim();
            string strMessage = string.Empty;
            #endregion Variables

            #region Required Fields
            if (strEmail.Length == 0)
                blnValid = false;
            else if (txtpassword.Text.Trim().Length == 0)
                blnValid = false;

            if (!blnValid)
            {
                if (strMessage.Trim().Length == 0)
                    strMessage = utilSOCYWeb.cMIDRequiredFields;
                else
                    strMessage = strMessage + "," + utilSOCYWeb.cMIDRequiredFields;
            }
            #endregion Required Fields

            #region Get Messages
            if (strMessage.Length != 0)
            {
                dbCon = new DBConnection(utilSOCYWeb.cWCKConnection);

                try
                {
                    utilLT = new utilLanguageTranslation();
                    utilLT.Language = Session[utilSOCYWeb.cSVLanguage].ToString();
                    arrMessage = utilLT.GetMessagesTranslation(strMessage.Split(','), dbCon.dbCon);
                    if (arrMessage.Length != 0)
                    {
                        strMessage = arrMessage[0];
                        for (int intCount = 1; intCount < arrMessage.Length; intCount++)
                            strMessage = strMessage + "<br/>" + arrMessage[intCount];
                    }
                }
                finally
                {
                    dbCon.Dispose();
                }
            }
            #endregion Get Messages

            return strMessage;
        }
        #endregion Private Methods
    }
}
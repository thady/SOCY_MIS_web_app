using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Telerik.Web.UI;
using System.Data;

namespace SOCY_WEBAppTest
{
    public class utilControls
    {
        #region RadComboBox
        public static void RadComboBoxFill(RadComboBox rcbBox, DataTable dt, string strValue, string strDisplay)
        {
            RadComboBoxFillPrivate(rcbBox, dt, strValue, strDisplay);
        }

        /// <summary>
        /// Fills indicated DropDownList with supplied data
        /// </summary>
        /// <param name="rcbBox">RadComboBox to be filled</param>
        /// <param name="dt">Data the DropDownList must be filled with</param>
        /// <param name="strValue">Field in data that represents the DropDownList value</param>
        /// <param name="strDisplay">Field in data that represents the DropDownList display text</param>
        private static void RadComboBoxFillPrivate(RadComboBox rcbBox, DataTable dt, string strValue, string strDisplay)
        {
            #region Bind List
            rcbBox.DataSource = dt;
            rcbBox.DataTextField = strDisplay;
            rcbBox.DataValueField = strValue;
            rcbBox.DataBind();
            #endregion Bind List
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SOCY_WEBAppTest.AppCode
{
    public static class SystemConstants
    {
        public static string SelectedReport = string.Empty;
       
        public static void LogOut(string webpage)
        {

            if (System.Web.HttpContext.Current.Session[utilSOCYWeb.cSVUserID].ToString() != "")
            {
                System.Web.HttpContext.Current.Session.Remove(utilSOCYWeb.cSVUserID);
            }

            HttpContext.Current.Response.Redirect(webpage + ".aspx", true);
        }
    }

}
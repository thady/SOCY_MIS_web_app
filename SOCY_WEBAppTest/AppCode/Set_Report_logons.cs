using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace SOCY_WEBAppTest.AppCode
{
    public static class Set_Report_logons
    {
        private static string SQLConnection;

        private static SqlConnectionStringBuilder builder;

        static Set_Report_logons()
        {
            Set_Report_logons.SQLConnection = ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString();
            Set_Report_logons.builder = new SqlConnectionStringBuilder(Set_Report_logons.SQLConnection);
        }

        public static void SetTableLogin(CrystalDecisions.CrystalReports.Engine.Table table)
        {
            CrystalDecisions.Shared.TableLogOnInfo tliCurrent = table.LogOnInfo;

            tliCurrent.ConnectionInfo.UserID = builder.UserID;
            tliCurrent.ConnectionInfo.Password = builder.Password;
            if (builder.InitialCatalog != null)
                tliCurrent.ConnectionInfo.DatabaseName = builder.InitialCatalog;
            if (builder.DataSource != null)
                tliCurrent.ConnectionInfo.ServerName = builder.DataSource;
            table.ApplyLogOnInfo(tliCurrent);
        }
    }
}
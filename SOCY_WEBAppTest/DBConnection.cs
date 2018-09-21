using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SOCY_WEBAppTest
{
    public class DBConnection
    {
        #region Class Variables
        public PSAUtilsWeb.DBConnection dbCon = null;
        #endregion Class Variables

        #region Class Methods
        public DBConnection(string strDatabase)
        {
            #region Variables

            string strConnection = "";

            #endregion Variables

            #region Create Connection

            strConnection = ConfigurationManager.AppSettings[strDatabase].ToString();
            dbCon = new PSAUtilsWeb.DBConnection(strConnection, strDatabase);

            #endregion Create Connection
        }

        public void Dispose()
        {
            dbCon.Dispose();
        }

        public void TransactionBegin()
        {
            dbCon.TransactionBegin();
        }

        public void TransactionCommit()
        {
            dbCon.TransactionCommit();
        }

        public void TransactionRollback()
        {
            dbCon.TransactionRollback();
        }

        #endregion Class Methods

        #region Database Functions
        public void DatabaseBackup(string strPath, string strFileName)
        {
            dbCon.DatabaseBackup(strPath, strFileName);
        }
        #endregion Database Functions

        #region SQL Method
        public int ExecuteNonQuery(string strSQL)
        {
            return dbCon.ExecuteNonQuery(strSQL);
        }

        public DataTable ExecuteQueryDataTable(string strSQL)
        {
            return dbCon.ExecuteQueryDataTable(strSQL);
        }

        public DataSet ExecuteQueryDataSet(string strSQL)
        {
            return dbCon.ExecuteQueryDataSet(strSQL);
        }

        public string ExecuteScalar(string strSQL)
        {
            return dbCon.ExecuteScalar(strSQL);
        }
        #endregion SQL Method
    }
}
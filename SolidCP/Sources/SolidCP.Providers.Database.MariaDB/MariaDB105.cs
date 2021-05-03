// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.IO;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using SolidCP.Providers;
using System.Reflection;
using System.Data.Common;

using SolidCP.Providers.Database;

namespace SolidCP.Providers.Database
{
    public class MariaDB105 : MariaDB101
    {

        public MariaDB105()
        {

        }

        public override bool IsInstalled()
        {
            return true;
        }

        public override long CalculateDatabaseSize(string database)
        {
            DataTable dt = ExecuteQuery(string.Format("SELECT SUM(data_length + index_length) / 1024 AS 'Size' FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{0}'", database));
            List<string> dbsize = new List<string>();
            foreach (DataRow dr in dt.Rows)
                dbsize.Add(dr["Size"].ToString());
            return Convert.ToInt64(dbsize.ToString());
        }

        #region private helper methods

        private int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, ConnectionString);
        }

        private int ExecuteNonQuery(string commandText, string connectionString)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(commandText, conn);
            conn.Open();
            int ret = cmd.ExecuteNonQuery();
            conn.Close();
            return ret;
        }

        private DataTable ExecuteQuery(string commandText, string connectionString)
        {
            return ExecuteQueryDataSet(commandText, connectionString).Tables[0];
        }

        private DataTable ExecuteQuery(string commandText)
        {
            return ExecuteQueryDataSet(commandText).Tables[0];
        }

        private DataSet ExecuteQueryDataSet(string commandText)
        {
            return ExecuteQueryDataSet(commandText, ConnectionString);
        }

        private DataSet ExecuteQueryDataSet(string commandText, string connectionString)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter(commandText, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        private string[] GetDatabaseUsers(string databaseName)
        {
            DataTable dtResult = ExecuteQuery(String.Format("SELECT User FROM db WHERE Db='{0}' AND Host='%' AND " +
                "Select_priv = 'Y' AND " +
                "Insert_priv = 'Y' AND " +
                "Update_priv = 'Y' AND  " +
                "Delete_priv = 'Y' AND  " +
                "Index_priv = 'Y' AND  " +
                "Alter_priv = 'Y' AND  " +
                "Create_priv = 'Y' AND  " +
                "Drop_priv = 'Y' AND  " +
                "Create_tmp_table_priv = 'Y' AND  " +
                "Lock_tables_priv = 'Y'", databaseName.ToLower()));
            //
            List<string> users = new List<string>();
            //
            if (dtResult != null)
            {
                if (dtResult.DefaultView != null)
                {
                    DataView dvUsers = dtResult.DefaultView;
                    //
                    foreach (DataRowView drUser in dvUsers)
                    {
                        if (!Convert.IsDBNull(drUser["user"]))
                        {
                            users.Add(Convert.ToString(drUser["user"]));
                        }
                    }
                }
            }
            //
            return users.ToArray();
        }

        private string[] GetUserDatabases(string username)
        {
            DataTable dtResult = ExecuteQuery(String.Format("SELECT Db FROM db WHERE LOWER(User)='{0}' AND Host='%' AND " +
                "Select_priv = 'Y' AND " +
                "Insert_priv = 'Y' AND " +
                "Update_priv = 'Y' AND  " +
                "Delete_priv = 'Y' AND  " +
                "Index_priv = 'Y' AND  " +
                "Alter_priv = 'Y' AND  " +
                "Create_priv = 'Y' AND  " +
                "Drop_priv = 'Y' AND  " +
                "Create_tmp_table_priv = 'Y' AND  " +
                "Lock_tables_priv = 'Y'", username.ToLower()));
            //
            List<string> databases = new List<string>();
            //
            //
            if (dtResult != null)
            {
                if (dtResult.DefaultView != null)
                {
                    DataView dvDatabases = dtResult.DefaultView;
                    //
                    foreach (DataRowView drDatabase in dvDatabases)
                    {
                        if (!Convert.IsDBNull(drDatabase["db"]))
                        {
                            databases.Add(Convert.ToString(drDatabase["db"]));
                        }
                    }
                }
            }
            //
            return databases.ToArray();
        }

        private void AddUserToDatabase(string databaseName, string user)
        {
            // grant database access
            ExecuteNonQuery(String.Format("GRANT ALL PRIVILEGES ON `{0}`.* TO '{1}'@'%'",
                    databaseName, user));
        }

        private void RemoveUserFromDatabase(string databaseName, string user)
        {
            // revoke db access
            ExecuteNonQuery(String.Format("REVOKE ALL PRIVILEGES ON `{0}`.* FROM '{1}'@'%'",
                    databaseName, user));
        }

        private void CloseDatabaseConnections(string database)
        {
            DataTable dtProcesses = ExecuteQuery("SHOW PROCESSLIST");
            //
            string filter = String.Format("db = '{0}'", database);
            //
            if (dtProcesses.Columns["db"].DataType == typeof(System.Byte[]))
                filter = String.Format("Convert(db, 'System.String') = '{0}'", database);

            DataView dvProcesses = new DataView(dtProcesses);
            foreach (DataRowView rowSid in dvProcesses)
            {
                string cmdText = String.Format("KILL {0}", rowSid["Id"]);
                try
                {
                    ExecuteNonQuery(cmdText);
                }
                catch (Exception ex)
                {
                    Log.WriteError("Cannot drop MariaDB connection: " + cmdText, ex);
                }
            }
        }

         #endregion


    }
}
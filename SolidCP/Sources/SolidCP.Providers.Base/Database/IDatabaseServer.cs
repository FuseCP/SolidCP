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
using System.Data;

namespace SolidCP.Providers.Database
{
	/// <summary>
	/// Summary description for IMsSqlProvider.
	/// </summary>
	public interface IDatabaseServer
	{
		// databases
		bool CheckConnectivity(string databaseName, string username, string password);
        DataSet ExecuteSqlQuery(string databaseName, string commandText);
		void ExecuteSqlNonQuery(string databaseName, string commandText);
        DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText);
        void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText);
		bool DatabaseExists(string databaseName);
        string[] GetDatabases();
		SqlDatabase GetDatabase(string databaseName);
        void CreateDatabase(SqlDatabase database);
		void UpdateDatabase(SqlDatabase database);
		void DeleteDatabase(string databaseName);
		long CalculateDatabaseSize(string database);

		// database maintenaince
		void TruncateDatabase(string databaseName);
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        string BackupDatabase(string databaseName, string backupFileName, bool zipBackupFile);
		void RestoreDatabase(string databaseName, string[] fileNames);

		// users
		bool UserExists(string userName);
        string[] GetUsers();
		SqlUser GetUser(string username, string[] databases);
		void CreateUser(SqlUser user, string password);
		void UpdateUser(SqlUser user, string[] databases);
        void DeleteUser(string username, string[] databases);
		void ChangeUserPassword(string username, string password);
    }
}

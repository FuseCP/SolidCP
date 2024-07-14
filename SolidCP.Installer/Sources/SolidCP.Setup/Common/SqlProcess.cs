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
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SolidCP.Setup.Actions;
using SolidCP.UniversalInstaller.Core;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.Setup
{
	/// <summary>
	/// Shows sql script process.
	/// </summary>
	public sealed class SqlProcess
	{
		private string scriptFile; 
		private string connectionString;
		private string database;

		public event EventHandler<ActionProgressEventArgs<int>> ProgressChange;
		
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="file">Sql script file</param>
		/// <param name="connection">Sql connection string</param>
		/// <param name="db">Sql server database name</param>
		public SqlProcess(string file, string connection, string db)
		{
			this.scriptFile = file;
			this.connectionString = connection;
			this.database = db;
		}

		private void OnProgressChange(float ratio)
		{
			if (ProgressChange == null)
				return;
			//
			ProgressChange(this, new ActionProgressEventArgs<int>
			{
				EventData = (int)(ratio * 100 + 0.5)
			});
		}


		internal void RunSqlServer(int commandCount)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			string sql;
			int i = 0;
			float n = commandCount;

			try
			{
				// iterate through delimited command text
				using (StreamReader reader = new StreamReader(scriptFile))
				{
					SqlCommand command = new SqlCommand();
					connection.Open();
					command.Connection = connection;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandTimeout = 600;

					while (null != (sql = ReadNextStatementFromStream(reader)))
					{
						sql = ProcessInstallVariables(sql);
						command.CommandText = sql;
						try
						{
							command.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							throw new Exception("Error executing SQL command: " + sql, ex);
						}

						i++;
						if (commandCount != 0)
						{
							OnProgressChange((float)i / n);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Can't run SQL script " + scriptFile, ex);
			}
			finally
			{
				connection.Close();
			}
		}

		void SetCommandCount(int n) => Log.WriteInfo($"Executing {n} database commands");

		/// <summary>
		/// Executes sql script file.
		/// </summary>
		internal void Run()
		{
			//
			OnProgressChange(0);
			//
			using (var stream = new FileStream(scriptFile, FileMode.Open, FileAccess.Read))
			{
				DatabaseUtils.RunSqlScript(connectionString, stream, OnProgressChange, SetCommandCount, ProcessInstallVariables, scriptFile, database);
			}
		}

		public bool IsDelimiter(string cmd) => Regex.IsMatch(cmd, "^(GO|DELIMITER)(?=$|[^a-zA-Z])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private string ReadNextStatementFromStream(StreamReader reader)
		{
			StringBuilder sb = new StringBuilder();
			string lineOfText;
	
			while(true) 
			{
				lineOfText = reader.ReadLine();
				if( lineOfText == null ) 
				{
					if( sb.Length > 0 ) 
					{
						return sb.ToString();
					}
					else 
					{
						return null;
					}
				}

				if(IsDelimiter(lineOfText)) 
				{
					break;
				}
				
				sb.Append(lineOfText + Environment.NewLine);
			}

			return sb.ToString();
		}

		private string ProcessInstallVariables(string input)
		{
			//replace install variables
			string output = input;
			output = output.Replace("${install.database}", database);
			return output;
		}
	}
}

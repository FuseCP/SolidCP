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
using System.Configuration;
using System.IO;
using System.Web;

namespace SolidCP.AWStats.Viewer
{
	/// <summary>
	/// Summary description for ConfigFileAuthenticationProvider.
	/// </summary>
	public class ConfigFileAuthenticationProvider : AuthenticationProvider
	{
		public override AuthenticationResult AuthenticateUser(string domain, string username, string password)
		{
            string dataFolder = ConfigurationManager.AppSettings["AWStats.ConfigFileAuthenticationProvider.DataFolder"];
			if(dataFolder.StartsWith("~"))
				dataFolder =  HttpContext.Current.Server.MapPath(dataFolder);

            string awStatsScript = ConfigurationManager.AppSettings["AWStats.URL"];
			int idx = awStatsScript.LastIndexOf("/");
			awStatsScript = (idx == -1) ? awStatsScript : awStatsScript.Substring(idx + 1);

			string prefix = awStatsScript;
			int dotIdx = prefix.LastIndexOf(".");
			if(dotIdx != -1)
				prefix = prefix.Substring(0, dotIdx);

			string dataFile = Path.Combine(dataFolder, prefix + "." + domain + ".conf");
			if(!File.Exists(dataFile))
				return AuthenticationResult.DomainNotFound;

			string[] pairs = new string[0];

			// read file contents
			StreamReader reader = null;
			try
			{
				reader = new StreamReader(dataFile);
				string line;
				while((line = reader.ReadLine()) != null)
				{
					idx = line.IndexOf("=");
					if(idx == -1)
						continue;

					string key = line.Substring(0, idx).Trim();
					if(key.ToLower() == "siteusers")
					{
						pairs = line.Substring(idx + 1).Trim().Split(';');
						foreach(string pair in pairs)
						{
							string[] credentials = pair.Split('=');
							if(String.Compare(credentials[0], username, true) == 0)
							{
								// check password
								return (password == credentials[1]) ? AuthenticationResult.OK : AuthenticationResult.WrongPassword;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				HttpContext.Current.Response.Write(ex.ToString());
			}
			finally
			{
				if(reader != null)
					reader.Close();
			}
			return AuthenticationResult.WrongUsername;
		}
	}
}

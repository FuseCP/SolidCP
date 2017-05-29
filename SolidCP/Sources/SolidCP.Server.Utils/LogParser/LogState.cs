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
using System.IO;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.Utils.LogParser
{
	/// <summary>
	/// Summary description for WebSiteState.
	/// </summary>
	public class LogState
	{
		private long lastAccessed = 0;
		private long line = 0;
		private string siteFileName = null;

		public LogState(string logName)
		{
			// make file name
			siteFileName = logName;

			// open and parse site state file
			if(!File.Exists(siteFileName))
				return;

		    StreamReader reader = null;
			try
			{
				reader = new StreamReader(siteFileName);
				string s = null;

				// last accesses time
				if((s = reader.ReadLine()) != null)
					lastAccessed = Int64.Parse(s.Trim());

				// line
				if((s = reader.ReadLine()) != null)
					line = Int64.Parse(s.Trim());

				reader.Close();
                
			}
			catch(Exception ex)
			{
                Log.WriteError(ex);

			}
            finally
			{
			    if (reader != null)
			    {
			        reader.Dispose();
			    }
			}
		}

		public void Save()
		{
			// create directory if required
			string dir = Path.GetDirectoryName(siteFileName);
			if(!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			StreamWriter writer = new StreamWriter(siteFileName);
			// last accesses time
			writer.WriteLine(lastAccessed.ToString());

			// line
			writer.WriteLine(line);

			// close writer
			writer.Close();
		}

		public long LastAccessed
		{
			get { return lastAccessed; }
			set { lastAccessed = value; }
		}

		public long Line
		{
			get { return line; }
			set { line = value; }
		}
	}
}

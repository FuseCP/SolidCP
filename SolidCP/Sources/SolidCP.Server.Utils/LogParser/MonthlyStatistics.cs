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
using System.Collections;

namespace SolidCP.Providers.Utils.LogParser
{
	/// <summary>
	/// Summary description for WebSiteStatistics.
	/// </summary>
	public class MonthlyStatistics
	{
		private string statsFile = null;
		private Hashtable days = new Hashtable();


		public MonthlyStatistics(string statsFile)
		{
			// save file name
			this.statsFile = statsFile;
		}

		public MonthlyStatistics(string statsFile, bool load)
		{
			// save file name
			this.statsFile = statsFile;
			//
			if (load)
			{
			    if (File.Exists(statsFile))
			    {
			        Load();
			    }
				//else
				//{
				//    throw new ArgumentException(String.Format("File with specified name doesn't exist: {0}", statsFile), "statsFile");
				//}
			}
		}

		public StatsLine this[int day]
		{
			get { return (StatsLine)days[day]; }
			set { days[day] = value; }
		}

		public Hashtable Days
		{
			get { return days; }
		}

		private void Load()
		{
			//
			StreamReader reader = new StreamReader(statsFile);
			string line = null;
			while ((line = reader.ReadLine()) != null)
			{
				// parse line
				string[] columns = line.Split(new char[] { ' ' });
				int day = Int32.Parse(columns[0]);

				// add new stats line to the hash
				StatsLine statsLine = new StatsLine();
				statsLine.BytesSent = Int64.Parse(columns[1]);
				statsLine.BytesReceived = Int64.Parse(columns[2]);

				days.Add(day, statsLine);
			}
			reader.Close();
		}

		public void Save()
		{
			Save(Path.GetDirectoryName(statsFile));
		}

		public void Save(string dir)
		{
			// create directory if required
			//string dir = Path.GetDirectoryName(statsFile);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			StreamWriter writer = null;

			try
			{
				writer = new StreamWriter(Path.Combine(dir, statsFile));

				foreach (int day in days.Keys)
				{
					StatsLine statsLine = (StatsLine)days[day];

					// write line
					writer.WriteLine("{0} {1} {2}", day, statsLine.BytesSent, statsLine.BytesReceived);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("Can't open '{0}' log file", statsFile), ex);
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}
	}
}

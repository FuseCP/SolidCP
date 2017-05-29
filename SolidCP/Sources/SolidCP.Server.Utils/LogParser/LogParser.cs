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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Web;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.Utils.LogParser
{
	public delegate void ProcessKeyFieldsEventHandler (string[] key_fields, string[] key_values, string[] log_fields, string[] log_values);
	public delegate void CalculateStatsLineEventHandler(StatsLine line, string[] fields, string[] values);
	/// <summary>
	/// Summary description for LogParser.
	/// </summary>
	public class LogParser
	{
		string serviceName;
		string logName;
		string logsPath;
		string[] keyFields;
		int keyFieldsLength;
		Regex re_clean = new Regex("\\W+", RegexOptions.Compiled);
        string[] _fields;

		public event ProcessKeyFieldsEventHandler ProcessKeyFields;
		public event CalculateStatsLineEventHandler CalculateStatisticsLine;

		public LogParser(string serviceName, string logName, string logsPath, params string[] keyFields)
		{
			this.serviceName = serviceName;
			this.logName = logName;
			this.logsPath = logsPath;
			this.keyFields = keyFields;
			this.keyFieldsLength = keyFields.Length;
            //
            List<string> fShadow = new List<string>(keyFields);
            fShadow.Add("sc-bytes");
            fShadow.Add("cs-bytes");
            fShadow.Add("date");
            //
            this._fields = fShadow.ToArray();
		}

		public void ParseLogs()
		{
			ParseLogs<LogReader>();
		}

		public void ParseLogs<T>() where T : LogReader
		{
			// ensure calculation logic has been initialized
			// with the default calculating routine
			if (CalculateStatisticsLine == null)
				CalculateStatisticsLine += new CalculateStatsLineEventHandler(Default_CalculateStatisticLine);
			//
			string statsDir = GetStatsFilePath();
		    string statsName = null;
			// get site state
			LogState logState = new LogState(statsDir + logName + ".state");

			LogReader reader = (LogReader)Activator.CreateInstance(typeof(T));
			reader.Open(logsPath, logState.LastAccessed, logState.Line);

			Hashtable monthlyLogs = new Hashtable();

			while (reader.Read())
			{
				try
				{
					// skip error and system lines
					if (reader.ErrorLine || reader.SystemLine)
						continue;
                    // skip block with log data if fields aren't available
                    if (!reader.CheckFieldsAvailable(_fields))
                        continue;
                    //
					string[] dateParts = reader["date"].Split('-'); // yyyy-mm-dd
					int day = Int32.Parse(dateParts[2]);

					string[] keyValues = new string[keyFieldsLength];
					//
					for (int i = 0; i < keyFieldsLength; i++)
						keyValues[i] = reader[keyFields[i]];
					//
					if (ProcessKeyFields != null)
						ProcessKeyFields(keyFields, keyValues, reader.LineFields, reader.LineValues);
					// build stats file name
					statsName = GetMothlyStatsFileName(dateParts[0], dateParts[1], keyValues);
					//
					MonthlyStatistics monthlyStats = (MonthlyStatistics)monthlyLogs[statsName];
					if (monthlyStats == null)
					{
						// add new statistics
						try
						{
							monthlyStats = new MonthlyStatistics(Path.Combine(statsDir, statsName), true);
							monthlyLogs[statsName] = monthlyStats;
						}
						catch (Exception ex)
						{
							// Handle an exception
							Log.WriteError(String.Format("LogParser: Failed to instantiate monthly stats file '{0}' at '{0}' path", statsName, statsDir), ex);
							// SKIP OVER THE NEXT ITERATION
							continue;
						}
					}

					// get current day from statistic
					StatsLine dayStats = monthlyStats[day];
					if (dayStats == null)
					{
						dayStats = new StatsLine();
						monthlyStats[day] = dayStats;
					}
					// perform statistics line calculation
					// this workaround has been added due to avoid 
					// IIS 6 vs. IIS 7 log files calculation logic discrepancies
					CalculateStatisticsLine(dayStats, reader.LineFields, reader.LineValues);
				}
				catch(Exception ex)
				{
                    Log.WriteError(String.Format("Failed to process line {0}, statistics directory path {1}, statistics file name {2}", reader.LogLine, statsDir, reader.LogName), ex);
				}
			}

			// save all accumulated statistics
			foreach (MonthlyStatistics monthlyStats in monthlyLogs.Values)
				monthlyStats.Save(statsDir);

			// save site state
			logState.LastAccessed = reader.LogDate;
			logState.Line = reader.LogLine;
			logState.Save();
		}

		public MonthlyStatistics GetMonthlyStatistics(int year, int month, params string[] keyValues)
		{
			string statsName = GetMothlyStatsFileName(year.ToString(), month.ToString(), keyValues);
			return new MonthlyStatistics(Path.Combine(this.GetStatsFilePath(), statsName), true);
		}

		public DailyStatistics[] GetDailyStatistics(DateTime since, params string[] keyValues)
		{
			ArrayList days = new ArrayList();

			// read statistics
			DateTime now = DateTime.Now;
			DateTime date = since;

			if (date == DateTime.MinValue)
				date = GetLogsBeginDate();

			// iterate from since to now
			while (date < now)
			{
				// get monthly statistics
				MonthlyStatistics stats = GetMonthlyStatistics(date.Year, date.Month, keyValues);
				foreach (int day in stats.Days.Keys)
				{
					StatsLine line = stats[day];
					DailyStatistics dailyStats = new DailyStatistics();
					dailyStats.Year = date.Year;
					dailyStats.Month = date.Month;
					dailyStats.Day = day;
					dailyStats.BytesSent = line.BytesSent;
					dailyStats.BytesReceived = line.BytesReceived;

					days.Add(dailyStats);
				}

				// advance month
				date = date.AddMonths(1);
			}

			return (DailyStatistics[])days.ToArray(typeof(DailyStatistics));
		}

		/// <summary>
		/// Perform default statistics line calculation.
		///  Suits for the following providers: IIS 6.0;
		/// </summary>
		/// <param name="line">Statistic line is being calculated</param>
		/// <param name="fields">Log file available fields</param>
		/// <param name="values">Log file available values for the line is being read</param>
		private void Default_CalculateStatisticLine(StatsLine line, string[] fields, string[] values)
		{
			int cs_bytes = Array.IndexOf(fields, "cs-bytes");
			int sc_bytes = Array.IndexOf(fields, "sc-bytes");
			// run default calculation logic
			if (cs_bytes > -1)
				line.BytesReceived += Int64.Parse(values[cs_bytes]);
			//
			if (sc_bytes > -1)
				line.BytesSent += Int64.Parse(values[sc_bytes]);
		}

		private DateTime GetLogsBeginDate()
		{
			DirectoryInfo dir = new DirectoryInfo(logsPath);
			FileInfo[] files = dir.GetFiles();
			DateTime minDate = DateTime.Now;
			foreach (FileInfo file in files)
			{
				if (file.CreationTime < minDate)
					minDate = file.CreationTime;
			}
			return minDate;
		}

		private string GetMothlyStatsFileName(string year, string month, string[] keyValues)
		{
            // build statistics name
            StringBuilder sb = new StringBuilder();
            int i = 0;
            try
            {
                // loop for key values
                for (i = 0; i < keyFieldsLength; i++)
                {
                    if (keyValues[i] != null && keyValues[i] != "")
                        sb.Append(re_clean.Replace(keyValues[i], "_")).Append("_");
                }
                // normalize year
                if (year.Length == 2)
                    sb.Append("20");
                sb.Append(year);
                // normalize month
                if (month.Length == 1)
                    sb.Append("0");
                sb.Append(month);
                // append log file extension
                sb.Append(".log");
                //
            }
            catch(Exception ex)
            {
                Log.WriteError(String.Format("Monthly Statistics FileName: year - {0}, month - {1}, keyValue - {2}", year, month, keyValues[i]), ex);
            }
		    return sb.ToString();
		}

		private string GetStatsFilePath()
		{
			string homePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogParser");
			homePath = Path.Combine(homePath, serviceName);
			//
			return homePath.EndsWith(@"\") ? homePath : homePath + @"\";
		}
	}

}

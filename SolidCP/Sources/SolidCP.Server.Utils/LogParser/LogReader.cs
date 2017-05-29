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
using System.Collections.Specialized;
using System.Diagnostics;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.Utils.LogParser
{

	/// <summary>
	/// Summary description for LogReader.
	/// </summary>
	public class LogReader
	{
		public static long TotalBytesProcessed = 0;

		public class FilesByCreationDateComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return DateTime.Compare(((FileInfo)x).CreationTime, ((FileInfo)y).CreationTime);
			}
		}

		private const string FIELDS_LINE = "#Fields: ";
		private const int FIELDS_LINE_LENGTH = 9;
		protected const char FIELDS_SEPARATOR = ' ';
		protected static readonly char[] FIELDS_SPLITTER = new char[] { FIELDS_SEPARATOR };

		protected int fieldsLength;

		// initial log position
		private int fileIndex = -1;
		private long lineIndex = 0;
		private long lastLineIndex = 0;
		protected string line = null; // current log line
		protected bool errorLine = false;
        protected bool systemLine = false;
		private string logName = null;
		private long logDate = 0;
		private StreamReader reader;

		// log files in the log directory
		protected ArrayList logFiles = new ArrayList();

		// fields available in the current log file
		private string[] fields = new string[0];
		protected string[] fieldsValues = null;

		// field values of the current log line
		protected Hashtable record = new Hashtable();

		public LogReader()
		{
			// nothing to do
		}

        public void Open(string logsPath)
		{
			// use this
			this.Open(logsPath, 0, 0);
		}

		public void Open(string logsPath, long lastAccessed, long line)
		{
			// save specified line
			lineIndex = line;

			// get logs directory
			DirectoryInfo logsDir = new DirectoryInfo(FileUtils.EvaluateSystemVariables(logsPath));

			// get the list of files
			if (logsDir.Exists)
				GetDirectoryFiles(logsDir, logFiles);

			// sort files by date
			logFiles.Sort(new FilesByCreationDateComparer());

			// try to find specified file
			if (lastAccessed != 0)
			{
				for (int i = 0; i < logFiles.Count; i++)
				{
					if (((FileInfo)logFiles[i]).CreationTime.Ticks == lastAccessed)
					{
						fileIndex = i;
					}
				}
			}

			// check whether specified file was found
			if (fileIndex == -1)
			{
				// no
				fileIndex = 0;
				lineIndex = 0;
			}
		}

        /// <summary>
        /// Checks whether requested fields are available against currently reading block with log data
        /// </summary>
        /// <param name="keyFields">Fields names to be accessed</param>
        /// <returns>Returns true if all of requested fields are available. Otherwise returns false.</returns>
        public bool CheckFieldsAvailable(string[] keyFields)
        {
            try
            {
                if (fields == null || fields.Length == 0)
                    return false;
                //
                foreach (string keyField in keyFields)
                {
                    //
                    if (Array.IndexOf(fields, keyField) == -1)
                        return false;
                }
                //
            }
            catch(Exception ex)
            {
                Log.WriteError(ex);
            }
            return true;
        }

		public bool Read()
		{
			// first of all try to open the "next" log file
			while (reader == null && fileIndex < logFiles.Count)
			{
				// open reader
				FileInfo file = (FileInfo)logFiles[fileIndex];
				logName = file.FullName;
				logDate = file.CreationTime.Ticks;

				try
				{
					Stream logStream = new FileStream(logName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					reader = new StreamReader(logStream);

					// update statistics counter
					TotalBytesProcessed += file.Length;

					// position file to the specified line
					if (lineIndex > 0)
					{
						// skip lines up to required
						int seekLine = 0;
						while (((line = reader.ReadLine()) != null) && ++seekLine < lineIndex)
						{
							// try to get field names
							ParseFieldNames();
						}

						lastLineIndex = lineIndex;
					}
				}
				catch(Exception ex)
				{
					// can't open log file
					// skip it
					Log.WriteError(String.Format("An error occured while reading log file: {0}", logName), ex);
				}

				fileIndex++; // increment files pointer
			}

			// return if it was the last log file in the list
			if (reader == null)
				return false;

			// assumed that log file is already opened
			// read next line
			line = reader.ReadLine();
			if (line == null)
			{
				// the most propbably current log file is finished
				// reset current reader
				reader.Close();
				reader = null;
				lineIndex = 0;

				// try to open next reader and read the first line
				return Read();
			}

			// increment line index
			lineIndex++;
			lastLineIndex = lineIndex;

			// parse line
			if (!String.IsNullOrEmpty(line))
			{
				if (line[0] == '#')
				{
				    ParseFieldNames();
				}
				else
				{
				    ParseFieldValues();
				}
			}
			else
			{
				errorLine = true;
			}
			return true;
		}


		private void ParseFieldNames()
		{
			systemLine = true;

			if (!line.StartsWith(FIELDS_LINE))
				return;

			fields = line.Substring(FIELDS_LINE_LENGTH).Trim().Split(FIELDS_SPLITTER);
			fieldsLength = fields.Length;
		}

		protected virtual void ParseFieldValues()
		{
			// clear state
			errorLine = systemLine = false;

			// clear values hash
			// record.Clear();

			/*if (line[0] == '#')
			{
				// it is a system line
				// skip it and go ahead
				systemLine = true;
				return;
			}*/
			//
			fieldsValues = line.Split(FIELDS_SPLITTER, StringSplitOptions.None);
			// error line
			if (fieldsValues.Length != fieldsLength)
				errorLine = true;
		}

		private void GetDirectoryFiles(DirectoryInfo root, ArrayList files)
		{
			// get the files in the current directory
			files.AddRange(root.GetFiles());

			// scan subdirectories
			DirectoryInfo[] dirs = root.GetDirectories();
			foreach (DirectoryInfo dir in dirs)
				GetDirectoryFiles(dir, files);
		}

		// provide read-only access to the current log line fields
		public string this[string field]
		{
			get
			{
                if (!errorLine && !systemLine)
                {
                    // get field index in fields array
                    int indexOf = Array.IndexOf(fields, field);
                    // ensure field exists
                    if (indexOf > -1)
                        return fieldsValues[indexOf]; ;
                }
                // field not found or line is either system or error
				return null;
			}
		}

		public bool ErrorLine
		{
			get
			{
				return errorLine;
			}
            set
            {
                errorLine = value;
            }
		}

		public bool SystemLine
		{
			get
			{
				return systemLine;
			}
            set
            {
                systemLine = value;
            }
		}

		public long LogDate
		{
			get
			{
				return logDate;
			}
		}

		public string LogName
		{
			get
			{
				return logName;
			}
		}

		public string[] LineFields
		{
			get { return fields; }
		}

		public string[] LineValues
		{
			get { return fieldsValues; }
		}

		public string CurrentLine
		{
			get { return line; }
		}

		public long LogLine
		{
			get
			{
				return lastLineIndex;
			}
		}

	}
}

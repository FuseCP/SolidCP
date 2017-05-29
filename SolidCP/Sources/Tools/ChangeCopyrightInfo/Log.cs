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
using System.Diagnostics;
using System.IO;

namespace ChangeCopyrightInfo
{
	/// <summary>
	/// Simple log
	/// </summary>
	public sealed class Log
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private Log()
		{
		}

		private static string logFile = "SolidCP.Import.CsvBulk.log";

		/// <summary>
		/// Initializes trace listeners.
		/// </summary>
		public static void Initialize(string fileName)
		{
			logFile = fileName;
			FileStream fileLog = new FileStream(logFile, FileMode.Append);
			TextWriterTraceListener fileListener = new TextWriterTraceListener(fileLog);
			fileListener.TraceOutputOptions = TraceOptions.DateTime;
			Trace.UseGlobalLock = true;
			Trace.Listeners.Clear();
			Trace.Listeners.Add(fileListener);
			TextWriterTraceListener consoleListener = new TextWriterTraceListener(System.Console.Out);
			Trace.Listeners.Add(consoleListener);
			Trace.AutoFlush = true;
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="ex">Exception.</param>
		internal static void WriteError(string message, Exception ex)
		{
			try
			{
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				Trace.WriteLine(ex);
			}
			catch { }
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		internal static void WriteError(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write to log
		/// </summary>
		/// <param name="message"></param>
		internal static void Write(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.Write(line);
			}
			catch { }
		}


		/// <summary>
		/// Write line to log
		/// </summary>
		/// <param name="message"></param>
		internal static void WriteLine(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write info message to log
		/// </summary>
		/// <param name="message"></param>
		internal static void WriteInfo(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] INFO: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write start message to log
		/// </summary>
		/// <param name="message"></param>
		internal static void WriteStart(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] START: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write end message to log
		/// </summary>
		/// <param name="message"></param>
		internal static void WriteEnd(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] END: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		internal static void WriteApplicationStart()
		{
			try
			{
				string name = typeof(Log).Assembly.GetName().Name;
				string version = typeof(Log).Assembly.GetName().Version.ToString(3);
				string line = string.Format("[{0:G}] ***** {1} {2} Started *****", DateTime.Now, name, version);
				Trace.WriteLine(line);
			}
			catch { }
		}

		internal static void WriteApplicationEnd()
		{
			try
			{
				string name = typeof(Log).Assembly.GetName().Name;
				string line = string.Format("[{0:G}] ***** {1} Ended *****", DateTime.Now, name);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Opens notepad to view log file.
		/// </summary>
		public static void ShowLogFile()
		{
			try
			{
				string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFile);
				Process.Start("notepad.exe", path);
			}
			catch { }
		}
	}
}

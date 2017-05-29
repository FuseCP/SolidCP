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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SolidCP.VmConfig
{
	/// <summary>
	/// Log.
	/// </summary>
	internal sealed class ServiceLog
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private ServiceLog()
		{
		}

		/// <summary>
		/// Initializes trace listeners.
		/// </summary>
		static ServiceLog()
		{
			string fileName = LogFile;
			FileStream fileLog = new FileStream(fileName, FileMode.Append);
			TextWriterTraceListener fileListener = new TextWriterTraceListener(fileLog);
			fileListener.TraceOutputOptions = TraceOptions.DateTime;
			Trace.UseGlobalLock = true;
			Trace.Listeners.Clear();
			Trace.Listeners.Add(fileListener);
			TextWriterTraceListener consoleListener = new TextWriterTraceListener(Console.Out);
			Trace.Listeners.Add(consoleListener);
			Trace.AutoFlush = true;
		}

		private static string LogFile
		{
			get
			{
				Assembly assembly = typeof(ServiceLog).Assembly;
				return Path.Combine(Path.GetDirectoryName(assembly.Location), assembly.GetName().Name + ".log");
			}
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
				string name = typeof(ServiceLog).Assembly.GetName().Name;
				string version = typeof(ServiceLog).Assembly.GetName().Version.ToString();
				string line = string.Format("[{0:G}] APP: {1} {2} started successfully", DateTime.Now, name, version);
				Trace.WriteLine(line);
			}
			catch { }
		}

		internal static void WriteApplicationStop()
		{
			try
			{
				string name = typeof(ServiceLog).Assembly.GetName().Name;
				string version = typeof(ServiceLog).Assembly.GetName().Version.ToString();
				string line = string.Format("[{0:G}] APP: {1} {2} stopped successfully", DateTime.Now, name, version);
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
				string path = LogFile;
				path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
				Process.Start("notepad.exe", path);
			}
			catch { }
		}
	}
}

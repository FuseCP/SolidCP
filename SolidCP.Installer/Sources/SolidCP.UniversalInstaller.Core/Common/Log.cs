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

using System.Security.Principal;
using System.Reflection;
using Renci.SshNet.Messages;

namespace SolidCP.UniversalInstaller
{
	/// <summary>
	/// Installer Log.
	/// </summary>
	public class LogWriter
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public LogWriter()
		{
			Installer.Current.Shell.Log += WriteLine;
		}

		/// <summary>
		/// Initializes trace listeners.
		/// </summary>
		static LogWriter()
		{
			Initialize();
		}

		static void Initialize()
		{
			string fileName = DefaultFile;
			//
			Trace.Listeners.Clear();
			//
			//FileStream fileLog = new FileStream(fileName, FileMode.Append);
			//
			TextWriterTraceListener fileListener = new TextWriterTraceListener(fileName);
			fileListener.TraceOutputOptions = TraceOptions.DateTime;
			Trace.Listeners.Add(fileListener);
			//
			Trace.AutoFlush = true;
		}

		public static string DefaultFile
		{
			get
			{
				string fileName = "SolidCP.Installer.log";
				//
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = "Installer.log";
				}
				// Ensure the path is correct
				if (!Path.IsPathRooted(fileName))
				{
					fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
				}
				//
				return fileName;
			}
		}

		string logFile = null;
		public virtual string File { get => logFile ??= DefaultFile; set => logFile = value; }

		public Action OnWrite { get; set; }

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="ex">Exception.</param>
		public virtual void WriteError(string message, Exception ex)
		{
			try
			{
				OnWrite?.Invoke();
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
		public virtual void WriteError(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void Write(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.Write(line);
			}
			catch { }
		}

 
		/// <summary>
		/// Write line to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteLine(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Writes formatted informational message into the log
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public virtual void WriteInfo(string format, params object[] args)
		{
			WriteInfo(String.Format(format, args));
		}

		/// <summary>
		/// Write info message to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteInfo(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] INFO: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write start message to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteStart(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] START: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}
		
		/// <summary>
		/// Write end message to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteEnd(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] END: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		public virtual void WriteApplicationStart()
		{
			try
			{
				OnWrite?.Invoke();
				string name = Installer.Current.GetEntryAssembly().GetName().Name;
				string version = Installer.Current.GetEntryAssembly().GetName().Version.ToString();
				string identity = WindowsIdentity.GetCurrent().Name;
				string line = string.Format("[{0:G}] {1} {2} Started by {3}", DateTime.Now, name, version, identity);
				Trace.WriteLine(line);
			}
			catch { }
		}

		public virtual void WriteApplicationEnd()
		{
			try
			{
				OnWrite?.Invoke();
				string name = Installer.Current.GetEntryAssembly().GetName().Name;
				string line = string.Format("[{0:G}] {1} Ended", DateTime.Now, name);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Opens notepad to view log file.
		/// </summary>
		public virtual void ShowLogFile() => Installer.Current.ShowLogFile();
		public void ProgressOne()
		{
			OnWrite?.Invoke();
			Trace.Write(".");
		}
	}

	public static class Log
	{
		public static string File => Installer.Current.Log.File;
		public static void WriteError(string message, Exception ex) => Installer.Current.Log.WriteError(message, ex);
		public static void WriteError(string message) => Installer.Current.Log.WriteError(message);
		public static void Write(string message) => Installer.Current.Log.Write(message);
		public static void WriteLine(string message) => Installer.Current.Log.WriteLine(message);
		public static void WriteInfo(string format, params object[] args) => Installer.Current.Log.WriteInfo(format, args);
		public static void WriteInfo(string message) => Installer.Current.Log.WriteInfo(message);
		public static void WriteStart(string message) => Installer.Current.Log.WriteStart(message);
		public static void WriteEnd(string message) => Installer.Current.Log.WriteEnd(message);
		public static void WriteApplicationStart() => Installer.Current.Log.WriteApplicationStart();
		public static void WriteApplicationEnd() => Installer.Current.Log.WriteApplicationEnd();
		public static void ShowLogFile() => Installer.Current.Log.ShowLogFile();
		public static void ProgressOne() => Installer.Current.Log.ProgressOne();
	}
}

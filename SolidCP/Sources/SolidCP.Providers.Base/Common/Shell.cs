using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace SolidCP.Providers
{

	public class LogReader : StreamReader
	{
		public LogReader(Shell shell, StreamReader r, Action<string> log) : base(r.BaseStream)
		{
			Shell = shell;
			Log = log;
			Reader = r;
		}


		public Shell Shell { get; set; }
		public StreamReader Reader { get; set; }

		public Action<string> Log { get; set; }
		public async Task Run()
		{
			string text;
			while (!Reader.EndOfStream)
			{
				text = await Reader.ReadLineAsync();
				Log(text);
			}
		}
	}


	public abstract class Shell : INotifyCompletion
	{

		public Shell() : base()
		{
			output = new StringBuilder();
			error = new StringBuilder();
			outputAndError = new StringBuilder();
			ReceiveLog(this);
		}

		//methods to support await on Shell type
		public Shell GetAwaiter() => this;
		public void OnCompleted(Action continuation) => continuation();
		public bool IsCompleted => Process == null || Process.HasExited;
		public Shell GetResult() => this;

		public virtual char PathSeparator => Path.PathSeparator;
		public abstract string ShellExe { get; }

		Process process;
		LogReader LogOutputReader, LogErrorReader;

		public virtual Process Process
		{
			get { return process; }
			protected set
			{
				if (process != value)
				{
					process = value;
					if (process != null)
					{
						LogOutputReader = new LogReader(this, process.StandardOutput, text =>
						{
							Log(text);
							LogOutput(text);
						});
						LogErrorReader = new LogReader(this, process.StandardError, text =>
						{
							Log(text);
							LogError(text);
						});
						Task.Run(LogOutputReader.Run);
						Task.Run(LogErrorReader.Run);
					}
				}
			}
		}
		public virtual string Find(string cmd) =>
			Environment.GetEnvironmentVariable("PATH")
				.Split(new char[] { PathSeparator })
				.SelectMany(p =>
				{
					var p1 = Path.Combine(p, cmd);
					return new string[] { p1, Path.ChangeExtension(p1, "exe") };
				})
				.FirstOrDefault(p => File.Exists(p));

		protected virtual string ToTempFile(string script)
		{
			var file = Path.GetTempFileName();
			File.WriteAllText(file, script);
			return file;
		}

		public virtual Shell ExecAsync(string cmd)
		{
			var pos = cmd.IndexOf(' ');
			var arguments = cmd.Substring(pos, cmd.Length - pos);
			cmd = cmd.Substring(0, pos);
			cmd = Find(cmd);
			var child = Clone;
			child.Process = Process.Start(cmd, arguments);
			return child;
		}

		protected virtual Shell Clone
		{
			get
			{
				Shell clone = Activator.CreateInstance(GetType()) as Shell;
				ReceiveLog(clone);
				return clone;
			}
		}

		protected virtual void ReceiveLog(Shell clone)
		{
			clone.Log += OnLog;
			clone.LogOutput += OnLogOutput;
			clone.LogError += OnLogError;
		}
		public virtual Shell Run(string script)
		{
			var file = ToTempFile(script);
			return ExecAsync($"{ShellExe} {file}");
		}

		public virtual Shell Wait(int milliseconds = 0)
		{
			if (milliseconds == 0) Process.WaitForExit();
			else Process.WaitForExit(milliseconds);
			return this;
		}

		public Action<string> Log { get; set; }
		public Action<string> LogOutput { get; set; }
		public Action<string> LogError { get; set; }

		StringBuilder output, error, outputAndError;
		public async Task<string> Output()
		{
			await this;
			lock (output) return output.ToString();
		}
		public async Task<string> Error()
		{
			await this;
			lock (error) return error.ToString();
		}
		public async Task<string> OutputAndError()
		{
			await this;
			lock (outputAndError) return outputAndError.ToString();
		}

		public string LogFile { get; set; } = null;
		protected virtual void OnLog(string text)
		{
			lock (outputAndError)
			{
				outputAndError.Append(text);
				if (LogFile != null) File.AppendAllText(LogFile, text);
			}
		}

		protected virtual void OnLogOutput(string text)
		{
			lock (output) output.Append(text);
			OnLog(text);
		}
		protected virtual void OnLogError(string text)
		{
			lock (error) error.Append(text);
			OnLog(text);
		}

	}
}
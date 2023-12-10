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
using System.ComponentModel.Design;

namespace SolidCP.Providers.OS
{

	public abstract class Shell : INotifyCompletion
	{
		const bool DoNotWaitForProcessExit = false;

		static int N = 0;

		public int ShellId = N++;
		public Shell() : base()
		{
			output = new StringBuilder();
			error = new StringBuilder();
			outputAndError = new StringBuilder();
			ReceiveLog(this);
		}

		//methods to support await on Shell type
		public Shell GetAwaiter() => this;

		Action Continuation = null;
		public void OnCompleted(Action continuation)
		{
			lock (this) Continuation += continuation;
			CheckCompleted();
		}

		bool errorEOF = true, outputEOF = true, hasProcessExited = true;
		int exitCode = 0;
		public bool IsCompleted => Process == null || ((DoNotWaitForProcessExit || hasProcessExited || Process.HasExited) && errorEOF && outputEOF);
		public Shell GetResult() => this;
		public virtual char PathSeparator => Path.PathSeparator;
		public abstract string ShellExe { get; }

		Process process;
		public virtual Process Process
		{
			get { return process; }
			protected set
			{
				if (process != value)
				{
					lock (this)
					{
						hasProcessExited = outputEOF = errorEOF = value == null;
						process = value;
					}
				}
			}
		}

		public bool NotFound { get; set; }
		public virtual string Find(string cmd)
		{
			string file = null;
			if (cmd.IndexOf(Path.DirectorySeparatorChar) >= 0)
			{
				if (File.Exists(cmd)) file = cmd;
			}
			else
			{
				file = Environment.GetEnvironmentVariable("PATH")
					  .Split(new char[] { PathSeparator })
					  .SelectMany(p =>
					  {
						  var p1 = Path.Combine(p, cmd);
						  return new string[] { p1, Path.ChangeExtension(p1, "exe") };
					  })
					  .FirstOrDefault(p => File.Exists(p));
			}
			NotFound = file == null;
			return file;
		}

		protected virtual string ToTempFile(string script)
		{
			var file = Path.GetTempFileName();
			File.WriteAllText(file, script);
			return file;
		}

		void CheckCompleted()
		{
			Action cnt = null;
			lock (this)
			{
				if (IsCompleted && Continuation != null)
				{
					cnt = Continuation;
					Continuation = null;
				}
			}
			cnt?.Invoke();
		}

		public virtual Shell ExecAsync(string cmd)
		{
			Log($"{cmd}{Environment.NewLine}");
			// separate command from arguments
			string arguments;
			if (cmd.Length > 0 && cmd[0] == '"') // command is a " delimited string
			{
				var pos = cmd.IndexOf('"', 1);
				if (pos >= 1)
				{
					if (pos < cmd.Length - 1)
					{
						arguments = cmd.Substring(pos + 1).Trim();
						cmd = cmd.Substring(1, pos - 1);
					}
					else
					{
						cmd = cmd.Substring(1, pos - 1);
						arguments = "";
					}
				}
				else
				{
					cmd = cmd.Substring(1);
					arguments = "";
				}
			}
			else // command is the first token of space separated tokens
			{
				var pos = cmd.IndexOf(' ');
				if (pos >= 0 && pos < cmd.Length - 1)
				{
					arguments = cmd.Substring(pos + 1);
					cmd = cmd.Substring(0, pos);
				}
				else arguments = "";
			}

			var cmdWithPath = Find(cmd);
			if (cmdWithPath != null)
			{
				var child = Clone;
				var process = new Process();
				Process = child.Process = process;
				process.StartInfo.FileName = cmdWithPath;
				process.StartInfo.Arguments = arguments;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.Exited += (obj, args) =>
				{
					exitCode = Process.ExitCode;
					lock (this) hasProcessExited = true;
					lock (child) child.hasProcessExited = true;
					child.CheckCompleted();
					CheckCompleted();
				};
				process.EnableRaisingEvents = true;
				process.ErrorDataReceived += (p, data) =>
				{
					if (data.Data == null)
					{
						lock (this) errorEOF = true;
						lock (child) child.errorEOF = true;
						child.CheckCompleted();
						CheckCompleted();
					}
					else if (data.Data != string.Empty)
					{
						child.Log(data.Data);
						child.LogError(data.Data);
					}
				};
				process.OutputDataReceived += (p, data) =>
				{
					if (data.Data == null)
					{
						lock (this) outputEOF = true;
						lock (child) child.outputEOF = true;
						child.CheckCompleted();
						CheckCompleted();
					}
					else if (data.Data != string.Empty)
					{
						child.Log(data.Data);
						child.LogOutput(data.Data);
					}
				};
				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				return child;
			}
			else
			{
				LogError($"Error {cmd} not found.{Environment.NewLine}");
				var child = Clone;
				child.Process = null;
				child.NotFound = true;
				return child;
			}
		}
		public virtual Shell Exec(string command) => ExecAsync(command).Task().Result;
		public virtual Shell Clone
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
		public virtual Shell ExecScriptAsync(string script)
		{
			var file = ToTempFile(script.Trim());
			var shell = ExecAsync($"{ShellExe} {file}");
			if (shell.Process != null)
			{
				shell.Process.Exited += (sender, args) =>
				{
					File.Delete(file);
				};
			}
			return shell;
		}
		public virtual Shell ExecScript(string script) => ExecScriptAsync(script).Task().Result;


		/* public virtual async Task<Shell> Wait(int milliseconds = Timeout.Infinite)
		{
			if (milliseconds == Timeout.Infinite) Process.WaitForExit();
			else Process.WaitForExit(milliseconds);
			return await this;
		} */

		public Action<string> Log { get; set; }
		public Action<string> LogOutput { get; set; }
		public Action<string> LogError { get; set; }

		StringBuilder output, error, outputAndError;

		public async Task<Shell> Task()
		{
			return await this;
		}

		public async Task<string> Output()
		{
			if (Process == null && NotFound) return null;
			await this;
			lock (output) return output.ToString();
		}

		public async Task<string> Error()
		{
			if (Process == null && NotFound) return null;
			await this;
			lock (error) return error.ToString();
		}
		public async Task<string> OutputAndError()
		{
			if (Process == null && NotFound) return null;
			await this;
			lock (outputAndError) return outputAndError.ToString();
		}

		public async Task<int> ExitCode()
		{
			if (Process == null && NotFound) return -500;
			await this;
			return exitCode;
		}
		public string LogFile { get; set; } = null;
		protected virtual void OnLog(string text)
		{
			lock (outputAndError)
			{
				outputAndError.AppendLine(text);
				if (LogFile != null) File.AppendAllText(LogFile, $"{text}{Environment.NewLine}");
			}
		}

		protected virtual void OnLogOutput(string text)
		{
			var id = ShellId;
			lock (output) output.AppendLine(text);
			OnLog(text);
		}
		protected virtual void OnLogError(string text)
		{
			lock (error) error.AppendLine(text);
			OnLog(text);
		}

		public static Shell Default => OSInfo.Current.DefaultShell;
	}
}
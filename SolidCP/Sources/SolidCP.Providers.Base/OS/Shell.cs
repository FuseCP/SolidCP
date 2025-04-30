using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

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
			Log += OnLog;
			LogCommand += OnLogCommand;
			LogCommandEnd += OnLogCommandEnd;
			LogError += OnLogError;
			LogOutput += OnLogOutput;
		}

		SemaphoreSlim Lock = new SemaphoreSlim(1, 1), OutputLock = new SemaphoreSlim(1, 1),
			ErrorLock = new SemaphoreSlim(1, 1), OutputAndErrorLock = new SemaphoreSlim(1, 1);

		//methods to support await on Shell type
		public Shell GetAwaiter() => this;

		Action Continuation = null;
		public void OnCompleted(Action continuation)
		{
			Lock.Wait();
			Continuation += continuation;
			Lock.Release();

			CheckCompleted();
		}

		bool errorEOF = true, outputEOF = true, hasProcessExited = true;
		int exitCode = 0;
		bool checkHasExited = true;

		// Process.HasExited can cause deadlock because it raises Exit event, therefore disable it in CheckCompleted by checkHasExited = false
		public bool IsCompleted => Process == null ||
			((DoNotWaitForProcessExit || hasProcessExited || (checkHasExited && Process.HasExited)) && errorEOF && outputEOF);
		public Shell GetResult() => this;
		public Shell Parent { get; set; } = null;
		public virtual char PathSeparator => Path.PathSeparator;
		public bool CreateNoWindow = true;
		public virtual string WorkingDirectory { get; set; } = null;
		public Encoding Encoding = null;
		public Dictionary<string, string> Environment = new Dictionary<string, string>();

		public ProcessWindowStyle WindowStyle = ProcessWindowStyle.Minimized;
		public abstract string ShellExe { get; }

		Process process;
		public virtual Process Process
		{
			get { return process; }
			protected set
			{
				if (process != value)
				{
					Lock.Wait();
					hasProcessExited = outputEOF = errorEOF = value == null;
					process = value;
					Lock.Release();
				}
			}
		}

		public bool NotFound { get; set; }
		public static IEnumerable<string> Paths
		{
			get
			{
				string proc, machine = "", user = "";
				string[] sources;
				proc = System.Environment.GetEnvironmentVariable("PATH");
				if (IsWindows)
				{
					machine = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
					user = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
					sources = new string[] {
						System.Environment.GetFolderPath(System.Environment.SpecialFolder.System),
						System.Environment.GetFolderPath(System.Environment.SpecialFolder.SystemX86),
						proc, machine, user };
				}
				else sources = new string[] { proc };

				return sources
					.SelectMany(paths => paths.Split(new char[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries))
					.Select(path => path.Trim())
					.Distinct();
			}
		}

		public virtual string Find(string cmd)
		{
			string file = null;
			cmd = cmd.Trim('"');
			if (cmd.IndexOf(Path.DirectorySeparatorChar) >= 0)
			{
				if (File.Exists(cmd)) file = cmd;
			}
			else
			{
				file = Paths
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
			var exited = Process.HasExited;
			Lock.Wait();
			try
			{
				hasProcessExited = hasProcessExited || exited;
				checkHasExited = false;
				if (IsCompleted && Continuation != null)
				{
					cnt = Continuation;
					Continuation = null;
				}
				checkHasExited = true;
			}
			finally
			{
				Lock.Release();
			}
			cnt?.Invoke();
		}

		public virtual StreamWriter StandardInput => process.StandardInput;
		public virtual Shell ExecAsync(string cmd, Encoding encoding = null, Dictionary<string, string> environment = null)
		{
			LogCommand?.Invoke(cmd);

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
				child.Process = process;
				process.StartInfo.FileName = cmdWithPath;
				process.StartInfo.Arguments = arguments;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = CreateNoWindow;
				process.StartInfo.WindowStyle = WindowStyle;
				process.StartInfo.WorkingDirectory = WorkingDirectory ??
					process.StartInfo.WorkingDirectory;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.RedirectStandardInput = true;
				process.StartInfo.StandardOutputEncoding = encoding ?? Encoding ?? Encoding.Default;
				process.StartInfo.StandardErrorEncoding = encoding ?? Encoding ?? Encoding.Default;
				var env = environment ?? Environment;
				if (env != null)
				{
					foreach (var variable in env)
					{
						if (!process.StartInfo.EnvironmentVariables.ContainsKey(variable.Key))
							process.StartInfo.EnvironmentVariables.Add(variable.Key, variable.Value);
						else
							process.StartInfo.EnvironmentVariables[variable.Key] = variable.Value;
					}
				}
				process.Exited += (obj, args) =>
				{
					child.exitCode = child.Process.ExitCode;
					child.Lock.Wait();
					child.hasProcessExited = true;
					child.Lock.Release();

					child.CheckCompleted();
				};
				process.EnableRaisingEvents = true;
				process.ErrorDataReceived += (p, data) =>
				{
					if (data.Data == null)
					{
						child.Lock.Wait();
						child.errorEOF = true;
						child.Lock.Release();

						child.CheckCompleted();
					}
					else
					{
						var line = $"{data.Data}{System.Environment.NewLine}";
						var shell = child;
						while (shell != null)
						{
							shell.Log?.Invoke(line);
							shell.LogError?.Invoke(line);
							shell = shell.Parent;
						}
					}
				};
				process.OutputDataReceived += (p, data) =>
				{
					if (data.Data == null)
					{
						child.Lock.Wait();
						child.outputEOF = true;
						child.Lock.Release();

						child.CheckCompleted();
						LogCommandEnd?.Invoke();
					}
					else
					{
						var line = $"{data.Data}{System.Environment.NewLine}";
						var shell = child;
						while (shell != null)
						{
							shell.Log?.Invoke(line);
							shell.LogOutput?.Invoke(line);
							shell = shell.Parent;
						}
					}
				};
				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				return child;
			}
			else
			{
				LogError?.Invoke($"Error {cmd} not found.{System.Environment.NewLine}");
				var child = Clone;
				child.Process = null;
				child.NotFound = true;
				return child;
			}
		}
		public virtual Shell Exec(string command, Encoding encoding = null, Dictionary<string, string> environment = null) => ExecAsync(command, encoding, environment).Task().Result;
		public virtual Shell Clone
		{
			get
			{
				Shell clone = Activator.CreateInstance(GetType()) as Shell;
				clone.Parent = this;
				clone.CreateNoWindow = this.CreateNoWindow;
				clone.WindowStyle = this.WindowStyle;
				clone.WorkingDirectory = this.WorkingDirectory;
				clone.Encoding = this.Encoding;
				clone.Environment = new Dictionary<string, string>();
				foreach (var item in this.Environment) clone.Environment.Add(item.Key, item.Value);

				return clone;
			}
		}

		public virtual Shell SilentClone
		{
			get
			{
				var clone = Clone;
				clone.Log = clone.LogCommand = clone.LogOutput = clone.LogError = null;
				clone.Parent = null;
				return clone;
			}
		}

		public virtual Shell ExecScriptAsync(string script, string args = null, Encoding encoding = null, Dictionary<string, string> environment = null)
		{
			script = script.Trim();
			// adjust new lines to OS type
			script = Regex.Replace(script, @"\r?\n", System.Environment.NewLine);
			var file = ToTempFile(script.Trim());
			var cmd = new StringBuilder();
			cmd.Append(ShellExe);
			cmd.Append(" \"");
			cmd.Append(file);
			cmd.Append("\"");
			if (args != null)
			{
				cmd.Append(" ");
				cmd.Append(args);
			}
			var shell = ExecAsync(cmd.ToString(), encoding, environment);
			if (shell.Process != null)
			{
				shell.Process.Exited += (sender, args) =>
				{
					File.Delete(file);
				};
			}
			return shell;
		}

		public virtual Shell ExecScript(string script, string args = null, Encoding encoding = null, Dictionary<string, string> environment = null)
			=> ExecScriptAsync(script, args, encoding, environment).Task().Result;


		/* public virtual async Task<Shell> Wait(int milliseconds = Timeout.Infinite)
		{
			if (milliseconds == Timeout.Infinite) Process.WaitForExit();
			else Process.WaitForExit(milliseconds);
			return await this;
		} */

		public Action<string> Log { get; set; }
		public Action<string> LogCommand { get; set; }
		public Action LogCommandEnd { get; set; }
		public Action<string> LogOutput { get; set; }
		public Action<string> LogError { get; set; }

		StringBuilder output, error, outputAndError;

		public async Task<Shell> Task()
		{
			return await this;
		}

		public void Wait() => Task().Wait();

		public async Task<string> Output()
		{
			if (Process == null && NotFound) return null;
			await this;
			await OutputLock.WaitAsync();
			try
			{
				return output.ToString();
			}
			finally
			{
				OutputLock.Release();
			}
		}

		public async Task<string> Error()
		{
			if (Process == null && NotFound) return null;
			await this;
			await ErrorLock.WaitAsync();
			try
			{
				return error.ToString();
			}
			finally
			{
				ErrorLock.Release();
			}
		}
		public async Task<string> OutputAndError()
		{
			if (Process == null && NotFound) return null;
			await this;
			await OutputAndErrorLock.WaitAsync();
			try
			{
				return outputAndError.ToString();
			}
			finally
			{
				OutputAndErrorLock.Release();
			}
		}

		public async Task<int> ExitCode()
		{
			if (Process == null && NotFound) return -500;
			await this;
			return exitCode;
		}
		public bool Redirect = false;
		public string LogFile = null;
		public void AppendAllText(string filename, string text)
		{
			try
			{
				using (var file = new FileStream(filename, FileMode.Append, FileAccess.Write))
				using (var writer = new StreamWriter(file, Encoding.UTF8))
				{
					writer.Write(text);
				}
			} catch { }
		}
		protected virtual void OnLog(string text)
		{
			OutputAndErrorLock.Wait();
			try
			{
				outputAndError.Append(text);
				if (LogFile != null) AppendAllText(LogFile, text);
			}
			finally
			{
				OutputAndErrorLock.Release();
			}
		}

		protected virtual void OnLogCommand(string text)
		{
			OutputAndErrorLock.Wait();
			try
			{
				text = $"> {text}";
				if (Redirect) Console.WriteLine(text);
				if (LogFile != null) AppendAllText(LogFile, text);
			}
			finally
			{
				OutputAndErrorLock.Release();
			}
		}
		protected virtual void OnLogCommandEnd()
		{
			OutputAndErrorLock.Wait();
			try
			{
				if (Redirect) Console.WriteLine();
				if (LogFile != null) AppendAllText(LogFile, System.Environment.NewLine);
			} finally
			{
				OutputAndErrorLock.Release();
			}
		}
		protected virtual void OnLogOutput(string text)
		{
			OutputLock.Wait();
			try
			{
				output.Append(text);
				if (Redirect) Console.Write(text);
			}
			finally
			{
				OutputLock.Release();
			}
		}
		protected virtual void OnLogError(string text)
		{
			ErrorLock.Wait();
			try
			{
				error.Append(text);
				if (Redirect) Console.Error.Write(text);
			}
			finally
			{
				ErrorLock.Release();
			}
		}
		public StreamWriter Input => Process?.StandardInput;

		static Shell standard = null;
		public static Shell Standard => standard ??= new StandardShell();

#if wpkg
		public readonly static Shell Default = new StandardShell(); // OSInfo.Current.DefaultShell;
		public static bool IsWindows => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
#else
		public static Shell Default => OSInfo.Current.DefaultShell;
		public static bool IsWindows => OSInfo.IsWindows;
#endif
	}

	public class StandardShell : Shell
	{
		public override string ShellExe => Shell.IsWindows ? "cmd" : "sh";
	}
}
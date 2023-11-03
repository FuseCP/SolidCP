using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace SolidCP.Server.Utils {

    public interface IShell {

        char PathSeparator { get; }
        string Find(string cmd);
        Process Start(string cmd);
        Shell Run(string script);
        Process Current { get; }
        Action<string> Log { get; set;}
        Action<string> LogOutput { get; set; }
        Action<string> LogError { get; set; }
        string Output { get; }
        string Error { get; }
        string OutputAndError { get; }
    }

    public abstract class Shell: IShell {
        public virtual char PathSeparator => Path.PathSeparator;
        public abstract string ShellExe { get; }
        public abstract Process Current { get; protected set; }
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
        public virtual Shell Start(string cmd)
        {
            var pos = cmd.IndexOf(' ');
            var arguments = cmd.Substring(pos, cmd.Length - pos);
            cmd = cmd.Substring(0, pos);
            cmd = Find(cmd);
            var child = Clone;
            child.Current = Process.Start(cmd, arguments);
            return child;
        }

        public abstract Shell Clone { get; }
        public virtual Shell Run(string script)
        {
            var shell = Clone;
            var file = ToTempFile(script);
            return Start($"{ShellExe} {file}");
        }

        public virtual void WaitForExit(int milliseconds = 0)
        {
            if (milliseconds == 0) Current.WaitForExit();
            else Current.WaitForExit(milliseconds);
        }


}
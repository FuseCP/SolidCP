using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.Providers.OS
{
    public class Yum : Installer
    {
        public override bool IsInstallerInstalled => Shell.Find("yum") != null;

        public override Shell AddSourcesAsync(string sources)
        {
			throw new NotSupportedException("AddSources not supported for yum.");
		}

		public override Shell InstallAsync(string apps)
        {
			return Shell.ExecAsync($"yum -y install {string.Join(" ", apps.Split(' ', ',', ';'))}");
        }

		public override bool IsInstalled(string apps)
		{
			var output = Shell.ExecAsync($"yum info {string.Join(" ", apps.Split(' ', ',', ';'))}").Output().Result;
			var installed = Regex.Matches(output, @"(?<!^Available Packages$[\s\r\n]*?)^Name\s*:\s*(?<name>.*?)$", RegexOptions.Multiline)
				.OfType<Match>()
				.Select(m => m.Groups["name"].Value.Trim())
				.ToArray();
			var applist = apps.Split(' ', ',', ';')
				.Select(app => app.Trim())
				.Except(installed);
			return !applist.Any();
		}

		public override Shell RemoveAsync(string apps)
		{
			return Shell.ExecAsync($"yum -y remove {string.Join(" ", apps.Split(' ', ',', ';'))}");
		}
		public override Shell UpdateAsync() => Shell;
	}
}

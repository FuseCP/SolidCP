using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SolidCP.Providers.OS
{
    public class Dnf : Installer
    {
		const string RepoFile = "/etc/yum.repos.d/solidcp.repo";
        public override bool IsInstallerInstalled => Shell.Find("dnf") != null;

        public override Shell AddSourcesAsync(string sources)
        {
			throw new NotImplementedException("AddSources not supported for dnf.");
		}

		public override Shell InstallAsync(string apps)
        {
			return Shell.ExecAsync($"dnf -y install {string.Join(" ", apps.Split(' ', ',', ';'))}");
        }

		public override bool IsInstalled(string apps)
		{
			var output = Shell.ExecAsync($"dnf info {string.Join(" ", apps.Split(' ', ',', ';'))}").Output().Result;
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
			return Shell.ExecAsync($"dnf -y remove {string.Join(" ", apps.Split(' ', ',', ';'))}");
		}
		public override Shell UpdateAsync() => Shell;
	}
}

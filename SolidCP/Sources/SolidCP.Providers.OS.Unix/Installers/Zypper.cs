using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.Providers.OS
{
    public class Zypper : Installer
    {
        public override bool IsInstallerInstalled => Shell.Find("zypper") != null;

        public override Shell AddSourcesAsync(string sources)
        {
            throw new NotImplementedException("AddSources not supported for Zypper.");
        }

        public override Shell InstallAsync(string apps)
        {
			return Shell.ExecAsync($"zypper -n install {string.Join(" ", apps.Split(' ', ',', ';'))}");
        }

		public override bool IsInstalled(string apps)
		{
			var applist = apps.Split(' ', ',', ';')
				.Select(app => app.Trim())
				.ToArray();
			var output = Shell.Exec($"zypper -n info {string.Join(" ", applist)}").Output().Result;
			var installed = Regex.Matches(output, @"^Name\s*:\s*(?<name>.*?)$(?=\s*(?:^[^:]+:.+$\s*)+^Installed\s*:\s*Yes)", RegexOptions.Multiline)
					.OfType<Match>()
					.Select(m => m.Groups["name"].Value.Trim());
			return applist.Except(installed)
				.Any();
		}

		public override Shell RemoveAsync(string apps)
		{
			return Shell.ExecAsync($"zypper -n remove {string.Join(" ", apps.Split(' ', ',', ';'))}");
		}
		public override Shell UpdateAsync() => Shell;
	}
}

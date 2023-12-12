using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.Providers.OS
{
    public class Pacman: Installer
    {
        public override bool IsInstallerInstalled => Shell.Find("pacman") != null;

        public override Shell AddSourcesAsync(string sources)
        {
			throw new NotSupportedException("AddSources is not supported for pacman installer");
        }

        public override Shell InstallAsync(string apps)
        {
			return Shell.Exec($"pacman -S {string.Join(" ", apps.Split(' ', ',', ';'))}");
        }

		public override bool IsInstalled(string apps)
		{
			var applist = apps.Split(' ', ',', ';')
				.Select(app => app.Trim());
			foreach (var app in applist)
			{
				var output = Shell.Exec($"pacman -Qs {$"^{app}$"}").Output().Result ?? "";
				var installed = Regex.IsMatch(output, $"^local/{app}", RegexOptions.Multiline);
				if (!installed) return false;
			}
			return true;
		}

		public override Shell RemoveAsync(string apps)
		{
			return Shell.Exec($"pacman -R {string.Join(" ", apps.Split(' ', ',', ';'))}");
		}
		public override Shell UpdateAsync() => Shell;
	}
}

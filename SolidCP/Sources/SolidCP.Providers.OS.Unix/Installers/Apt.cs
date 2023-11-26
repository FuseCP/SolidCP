using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;


namespace SolidCP.Providers.OS
{
    public class Apt: Installer
	{

		public override bool IsInstallerInstalled => Shell.Find("apt-get") != null;
		public override Shell InstallAsync(string apps)
		{
			return Shell.ExecAsync($"apt-get install -y {apps.Replace(',', ' ').Replace(';', ' ')}");
		}

		public override Shell AddSourcesAsync(string sources)
		{
			string list;
			IEnumerable<string> store;

			if (Directory.Exists("/etc/apt/sources.list.d"))
			{
				list = "/etc/apt/source.list.d/solidcp.list";
				store = Directory.EnumerateFiles("/etc/apt/source.list.d/*.list")
					.SelectMany(file => File.ReadAllLines(file))
					.Select(line => line.Trim());
			}
			else
			{
				list = "/etc/apt/source.list";
				store = (File.Exists(list) ? File.ReadAllLines(list) : new string[0])
					.Select(line => line.Trim());
			}
			var lines = sources.Split(';')
				.Select(line => line.Trim())
				.Except(store);

			File.AppendAllLines(list, lines);

			return UpdateAsync();
		}
		public override bool IsInstalled(string apps)
		{
			return !Shell.Exec($"dpkg -l {apps.Replace(';', ' ').Replace(';', ' ')}").Output().Result?.Contains("no packages found") ?? false;
		}

		public override Shell RemoveAsync(string apps)
		{
			return Shell.ExecAsync($"apt-get remove {apps.Replace(';', ' ').Replace(',', ' ')}");
		}

		public override Shell UpdateAsync()
		{
			return Shell.ExecAsync("apt-get update");
		}
	}
}

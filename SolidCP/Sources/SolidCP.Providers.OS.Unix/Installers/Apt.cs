using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace SolidCP.Providers.OS
{
    public class Apt: Installer
	{
		public override bool IsInstalled => Shell.Find("apt-get") != null;
		public override Shell InstallAsync(string apps)
		{
			return Shell.ExecAsync($"apt-get install -y {apps}");
		}

		public override void AddSources(string sources)
		{
			string list;
			IEnumerable<string> store;

			if (Directory.Exists("/etc/apt/sources.list.d"))
			{
				list = "/etc/apt/source.list.d/solidcp.list";
				store = Directory.EnumerateFiles("/etc/apt/source.list.d/*.list")
					.SelectMany(file => File.ReadAllLines(file));
			}
			else
			{
				list = "/etc/apt/source.list";
				store = File.Exists(list) ? File.ReadAllLines(list) : new string[0];
			}
			var lines = sources.Split(';')
				.Except(store);

			File.AppendAllLines(list, lines);
		}
	}
}

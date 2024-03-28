using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using SolidCP.Providers.OS;

namespace SolidCP.Providers.Virtualization
{
	public class ProxmoxvpsLocal : Proxmoxvps
	{
		protected override string ProxmoxClusterServerHost => "localhost";
		public string GetInstalledVersion()
		{
			try
			{
				var output = Shell.Default.Exec($"pveversion -v").Output().Result;
				var match = Regex.Match(output, @"(?<=^proxmox-ve:\s*)(?<version>[0-9][0-9.]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				if (match.Success)
				{
					return match.Groups["version"].Value;
				}
			}
			catch { }

			return "";
		}
		public override bool IsInstalled() => !string.IsNullOrEmpty(GetInstalledVersion());
	}
}
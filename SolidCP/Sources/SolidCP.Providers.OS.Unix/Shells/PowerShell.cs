﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
    public class UnixPowerShell: Shell
	{
		public override string ShellExe => "powershell";
	}
}

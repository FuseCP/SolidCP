using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SolidCP.Core
{
	public static class Net
	{
		public static bool IsMono => Type.GetType("Mono.Runtime") != null;
		public static bool IsCore => !IsMono && (RuntimeInformation.FrameworkDescription == ".NET" || RuntimeInformation.FrameworkDescription == ".NET Core");
		public static bool IsFramework => !IsMono && RuntimeInformation.FrameworkDescription == ".NET Framework";
		public static bool IsNet7 => !IsMono && RuntimeInformation.FrameworkDescription == ".NET" && Environment.Version.Major >= 7;
		public static bool IsNet6 => !IsMono && RuntimeInformation.FrameworkDescription == ".NET" && Environment.Version.Major >= 6;
		public static bool IsNet5 => !IsMono && RuntimeInformation.FrameworkDescription == ".NET" && Environment.Version.Major >= 5;
		public static bool IsNet4 => IsFramework && Environment.Version.Major == 4;
		public static bool IsNet35 => IsFramework && Environment.Version.Major == 3;
		public static bool IsNet2 => IsFramework && Environment.Version.Major == 2;

	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
	public class UnixFileOwner
	{
		public string Owner { get; set; }
		public string Group { get; set; }
	}

	public class UnixFilePermissions : UnixFileOwner
	{
		public UnixFileMode Permissions { get; set; }
	}

}

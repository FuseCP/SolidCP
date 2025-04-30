using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public interface ISetupInstaller
	{
		public bool Install(string args);
		public bool Setup(string args);
		public bool Update(string args);
		public bool Uninstall(string args);
	}
}

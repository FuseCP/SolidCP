using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public interface ILoadContext
	{
		public object Execute(string fileName, string typeName, string methodName, object[] parameters);
		public string GetShellVersion();
	}
}

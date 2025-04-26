using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.Web.Clients;

namespace SolidCP.Server.Tests
{
	public class Server
	{
		public const string AssemblyUrl = "assembly://SolidCP.Server";
		public static void Init()
		{
			AssemblyLoader.Init(@"..\SolidCP.Server\bin;..\SolidCP.Server\bin\Lazy;..\SolidCP.Server\bin\netstandard", "none", false);
		}
	}
}

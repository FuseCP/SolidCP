using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Tests
{
	[TestClass]
	public class Initialization
	{

		[AssemblyInitialize]
		public static void Init(TestContext context)
		{
			Certificate.TrustAll();
			Server.Init();			
		}

		[AssemblyCleanup]
		public static void Cleanup()
		{
			// remove the certificate
			//Certificate.Remove();

			// stop the servers
			Servers.StopAll();
		}
	}
}

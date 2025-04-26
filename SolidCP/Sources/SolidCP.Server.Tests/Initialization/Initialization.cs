using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Server.Tests
{
	[TestClass]
	public class Initialization
	{

		[AssemblyInitialize]
		public static void Init(TestContext context)
		{
			Server.Init();
			
			// install the certificate
			Certificate.Install();
			Certificate.TrustAll();

			// start the servers
			IISExpress.Start();
			Kestrel.Start();
		}

		[AssemblyCleanup]
		public static void Cleanup()
		{
			// remove the certificate
			Certificate.Remove();
			// stop the servers
			IISExpress.Stop();
			Kestrel.Stop();
		}
	}
}

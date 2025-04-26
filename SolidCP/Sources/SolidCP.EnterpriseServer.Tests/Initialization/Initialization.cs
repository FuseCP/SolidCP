using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.Tests
{
	[TestClass]
	public class Initialization
	{

		[AssemblyInitialize]
		public static void Init(TestContext context)
		{
			// Init the assembly loader
			EnterpriseServer.InitAssemblyLoader();

			// Init all db flavors DbConfiguration
			DbConfiguration.InitAllDatabaseProviders();

			// install the certificate
			Certificate.Install();
			Certificate.TrustAll();
			
			// Setup EnterpriseServer website & database
			EnterpriseServer.Clone();
			var connStr = EnterpriseServer.SetupLocalDb();
			EnterpriseServer.ConfigureDatabase(connStr);

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
			EnterpriseServer.DeleteDatabases();
			EnterpriseServer.Delete();
		}
	}
}

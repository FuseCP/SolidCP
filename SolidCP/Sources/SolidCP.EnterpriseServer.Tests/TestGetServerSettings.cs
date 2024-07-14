using System.Runtime.Serialization;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Client;

namespace SolidCP.Tests
{
	[TestClass]
	public class TestServerSettings
	{
		static readonly object Lock = new object();
		public TestContext? TestContext { get; set; }

		[ClassInitialize]
		public static void InitTest(TestContext context)
		{
			Kestrel.Start();
			IISExpress.Start();
		}

		[TestMethod]
		public async Task Test()
		{
			var testClient = new esTest();
			testClient.Url = "https://localhost:44332";
			testClient.Protocol = Web.Clients.Protocols.BasicHttps;
			Assert.AreEqual("Hello", testClient.Echo("Hello"));
			Assert.AreEqual("Hello", await testClient.EchoAsync("Hello"));
			//var settings = testClient.GetSystemSettings(SystemSettings.ACCESS_IP_SETTINGS);
			var esClient = new esSystem();
			esClient.Url = "https://localhost:44332";
			esClient.Protocol = Web.Clients.Protocols.BasicHttps;
			esClient.Credentials.UserName = "serveradmin";
			esClient.Credentials.Password = "";
			var settings = esClient.GetSystemSettings(SystemSettings.ACCESS_IP_SETTINGS);
			Assert.IsTrue(true);
		}

	}
}
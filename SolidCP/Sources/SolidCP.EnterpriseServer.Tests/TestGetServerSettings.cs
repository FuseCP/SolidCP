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
			TestWebSite.Clone();
			TestWebSite.SetupDatabase();

			Kestrel.Start();
			IISExpress.Start();
		}

		[TestMethod]
		public async Task TestESAccess()
		{
			var testClient = new esTest();
			testClient.Url = IISExpress.HttpsUrl;
			testClient.Protocol = Web.Clients.Protocols.BasicHttps;
			Assert.AreEqual("Hello", testClient.Echo("Hello"));
			Assert.AreEqual("Hello", await testClient.EchoAsync("Hello"));
			//var settings = testClient.GetSystemSettings(SystemSettings.ACCESS_IP_SETTINGS);
			var esClient = new esSystem();
			esClient.Url = IISExpress.HttpsUrl;
			esClient.Protocol = Web.Clients.Protocols.BasicHttps;
			esClient.Credentials.UserName = "serveradmin";
			esClient.Credentials.Password = CryptoUtils.SHA256("123456");
			var settings = esClient.GetSystemSettings(SystemSettings.ACCESS_IP_SETTINGS);
			Assert.IsTrue(true);
		}

	}
}
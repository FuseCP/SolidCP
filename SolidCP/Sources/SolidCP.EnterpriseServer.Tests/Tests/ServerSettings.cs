using System.Runtime.Serialization;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Client;

namespace SolidCP.Tests
{
	[TestClass]
	public class TestServerSettings
	{
		static readonly object Lock = new object();
		public TestContext TestContext { get; set; }

		[TestMethod]
		public async Task TestESAccess()
		{
			var testClient = new esTest();
			testClient.Url = IISExpress.HttpsUrl;
			testClient.Protocol = Web.Clients.Protocols.BasicHttps;
			Assert.AreEqual("Hello", testClient.Echo("Hello"));
			Assert.AreEqual("Hello", await testClient.EchoAsync("Hello"));

			var esClient = new esSystem();
			esClient.Url = IISExpress.HttpsUrl;
			esClient.Protocol = Web.Clients.Protocols.BasicHttps;
			esClient.Credentials.UserName = "serveradmin";
			esClient.Credentials.Password = CryptoUtils.SHA256("123456");
			var settings = esClient.GetSystemSettings(SystemSettings.DEBUG_SETTINGS);
		}

		[TestMethod]
		public async Task TestESAssemblyAccess()
		{
			var testClient = new esTest();
			testClient.Url = EnterpriseServer.AssemblyUrl;
			Assert.AreEqual("Hello", testClient.Echo("Hello"));
			Assert.AreEqual("Hello", await testClient.EchoAsync("Hello"));

			var esClient = new esSystem();
			esClient.Url = EnterpriseServer.AssemblyUrl;
			esClient.Credentials.UserName = "serveradmin";
			esClient.Credentials.Password = CryptoUtils.SHA256("123456");
			var settings = esClient.GetSystemSettings(SystemSettings.DEBUG_SETTINGS);
		}
	}
}
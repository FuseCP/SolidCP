namespace SolidCP.Server.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolidCP.Providers;
    using SolidCP.Server.Client;
    using SolidCP.Web.Clients;
    using System.ServiceModel;

    [TestClass]
    public class CoreWCF
    {
        public const string DevServerPassword = "cRDtpNCeBiql5KOQsKVyrA0sAiA=";
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataRow(Protocols.BasicHttps)]
        [DataRow(Protocols.WSHttps)]
        [DataRow(Protocols.NetHttps)]
        public void TestAnonymousNet8(Protocols protocol)
        {
            using (var client = new AutoDiscovery() { Url = Kestrel.HttpsUrl })
            {
                try
                {
                    client.Protocol = protocol;
                    var path = client.GetServerFilePath();
                }
                catch (FaultException fex)
                {
                    TestContext.WriteLine($"Fault: {fex};{fex.InnerException}");
                }
                catch (Exception ex)
                {
                    throw;
                    Assert.Fail("Exception", ex);
                }
            }
        }

        [TestMethod]
        public async Task TestAnonymousNet8Async()
        {
            using (var client = new AutoDiscovery() { Url = Kestrel.HttpsUrl })
            {
                try
                {
                    var path = await client.GetServerFilePathAsync();
                }
                catch (Exception ex)
                {
                    throw;
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        [DataRow(Protocols.BasicHttps)]
        [DataRow(Protocols.WSHttps)]
        [DataRow(Protocols.NetHttps)]
        public async Task TestPasswordNet8(Protocols protocol)
        {
            using (var client = new Client.OperatingSystem() { Url = Kestrel.HttpsUrl })
            {
                try
                {
                    client.SoapHeader = new ServiceProviderSettingsSoapHeader()
                    {
                        Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
                    };
                    client.Credentials.Password = DevServerPassword;
                    client.Protocol = protocol;
                    var res = client.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
                    Assert.Fail();
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
        [TestMethod]
        [DataRow(Protocols.BasicHttps)]
        [DataRow(Protocols.WSHttps)]
        [DataRow(Protocols.NetHttps)]
        public void TestAnonymousNet48(Protocols protocol)
        {
            using (var client = new AutoDiscovery() { Url = IISExpress.HttpsUrl })
            {
                try
                {
                    client.Protocol = protocol;
                    var path = client.GetServerFilePath();
                }
                catch (FaultException fex)
                {
                    TestContext.WriteLine($"Fault: {fex};{fex.InnerException}");
                }
                catch (Exception ex)
                {
                    throw;
                    Assert.Fail("Exception", ex);
                }
            }
        }

        [TestMethod]
        public async Task TestAnonymousNet48Async()
        {
            using (var client = new AutoDiscovery() { Url = IISExpress.HttpsUrl })
            {
                try
                {
                    var path = await client.GetServerFilePathAsync();
                }
                catch (Exception ex)
                {
                    throw;
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        [DataRow(Protocols.BasicHttps)]
        [DataRow(Protocols.WSHttps)]
        [DataRow(Protocols.NetHttps)]
        public async Task TestPasswordNet48(Protocols protocol)
        {
            using (var client = new Client.OperatingSystem() { Url = IISExpress.HttpsUrl })
            {
                try
                {
                    client.SoapHeader = new ServiceProviderSettingsSoapHeader()
                    {
                        Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
                    };
                    client.Credentials.Password = DevServerPassword;
                    client.Protocol = protocol;
                    var res = client.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}
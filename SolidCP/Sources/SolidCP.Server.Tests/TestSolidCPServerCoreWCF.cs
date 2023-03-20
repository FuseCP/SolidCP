namespace SolidCP.Server.Tests
{
    using SolidCP.Server.Client;

    [TestClass]
    public class TestSolidCPServerCoreWCF
    {

        Kestrel Server;

        //[ClassInitialize]
        public void InitTest(TestContext context)
        {
            Server = new Kestrel();
        }

        //[ClassCleanup]
        public void Dispose()
        {
            Server.Dispose();
        }



        [TestMethod]
        public void TestAutoDiscovery()
        {
            using (var client = new AutoDiscovery() { Url = "https://localhost:9007" })
            {
                try
                {
                    var path = client.GetServerFilePath();
                }
                catch
                {
                    Assert.Fail();
                }
            }
        }
    }
}
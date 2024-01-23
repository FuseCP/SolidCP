using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Tests
{
	[TestClass]
	public class CryptoTests
	{

		[TestMethod]
		public void TestCryptoSshUrl()
		{
			var url = "ssh://test:testpassword@testhost/9015";
			var encurl = CryptoUtils.EncryptServerUrl(url);
			Assert.AreNotEqual<string>(url, encurl);
			Assert.IsTrue(encurl.StartsWith("sshencrypted://"));
			var decurl = CryptoUtils.DecryptServerUrl(encurl);
			Assert.AreEqual<string>(url, decurl);

			encurl = "sshencrypted://SkIOeHzRREU5W7V+XIaAbFf+DctFmdUoW5di3nypxI9Ka1UAFCnJRJKF4PYJ9dvUF68Xn62ck5075fF1K/tVH";

		}
	}
}

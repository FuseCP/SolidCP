using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer;

namespace SolidCP.Tests
{
	[TestClass]
	public class Crypto
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
		}

		[TestMethod]
		public void TestEncryptDecrypt()
		{
			var txt = "Hello World!";
			var entxt = CryptoUtils.Encrypt(txt);
			var detxt = CryptoUtils.Decrypt(entxt);
			Assert.AreEqual<string>(txt, detxt);
		}
	}
}

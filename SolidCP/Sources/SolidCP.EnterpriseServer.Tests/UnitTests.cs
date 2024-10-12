using System.Reflection;
using System.Runtime.Serialization;
using SolidCP.EnterpriseServer;

namespace SolidCP.Tests
{
	[TestClass]
	public class UnitTests
	{

		public TestContext TestContext { get; set; }

		public void TestSerializeType<T>(T obj)
		{
			DataContractSerializer s = new DataContractSerializer(typeof(T));
			using (MemoryStream fs = new MemoryStream())
			{
				TestContext.WriteLine("Testing for type: {0}", typeof(T));
				s.WriteObject(fs, obj);
				fs.Seek(0, SeekOrigin.Begin);
				object s2 = s.ReadObject(fs);
				if (s2 == null)
				{
					TestContext.WriteLine("  Deserialized object is null");
					Assert.IsNotNull(s2);
				}
				else
				{
					TestContext.WriteLine("  Deserialized type: {0}", s2.GetType());
					Assert.AreEqual<Type>(obj.GetType(), s2.GetType());
				}
				fs.Seek(0, SeekOrigin.Begin);
				var reader = new StreamReader(fs);
				var text = reader.ReadToEnd();
				TestContext.WriteLine(text);
				Assert.IsFalse(text.Contains("settingsHash"));
			}
		}

		[TestMethod]
		public void SerializeServerSettings()
		{
			var setting = new SystemSettings();
			setting.SettingsArray = new string[][] { new string[] { "test", "a" }, new string[] { "test2", "m"} };
			TestSerializeType<SystemSettings>(new SystemSettings());
			Assert.IsTrue(true);
		}

	}
}
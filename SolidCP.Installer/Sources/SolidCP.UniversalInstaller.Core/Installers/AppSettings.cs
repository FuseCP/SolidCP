using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.UniversalInstaller
{
	public class AppSettings
	{
		public class ServerSetting
		{
			public string Password { get; set; }
		}
		public class EnterpriseServerSetting
		{
			public string WebApplicationPath { get; set; }
			public int? ServerRequestTimeout { get; set; }
			public string ConnectionString { get; set; }
			public string AltConnectionString { get; set; }
			public string CryptoKey { get; set; }
			public string AltCryptoKey { get; set; }
			public bool? EncryptionEnabled { get; set; }
		}

		public class CertificateSetting
		{
			public StoreLocation StoreLocation;
			public StoreName StoreName;
			public X509FindType FindType;
			public string FindValue { get; set; }
			public string File { get; set; }
			public string Password { get; set; }
		}

		public class LettuceEncryptSetting
		{
			public bool AcceptTermOfService { get; set; }
			public string[] DomainNames { get; set; }
			public string EmailAddress { get; set; }
		}
		public class IgnoreAllowedHostsResolver : DefaultContractResolver
		{
			protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
			{
				JsonProperty property = base.CreateProperty(member, memberSerialization);
				if (property.PropertyName == "AllowedHosts" || property.PropertyName == "Certificate" ||
					property.PropertyName == "Server" || property.PropertyName == "LettuceEncrypt" ||
					property.PropertyName == "EnterpriseServer" || property.PropertyName == "ServerRequestTimeout" ||
					property.PropertyName == "EncryptionEnabled")
				{
					property.NullValueHandling = NullValueHandling.Ignore;
				}
				return property;
			}
		}

		public string applicationUrls { get; set; } = null;
		public string probingPaths { get; set; } = null;
		public string AllowedHosts { get; set; } = null;
		public ServerSetting Server { get; set; }
		public EnterpriseServerSetting EnterpriseServer { get; set; }
		public CertificateSetting Certificate { get; set; }
		public LettuceEncryptSetting LettuceEncrypt { get; set; }
		public TraceLevel TraceLevel { get; set; }
	}
}

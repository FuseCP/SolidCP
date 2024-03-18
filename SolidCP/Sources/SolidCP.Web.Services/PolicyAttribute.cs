using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
	public enum Policies { Encrypted, ServerAuthenticated, EnterpriseServerAuthenticated }
	public class PolicyAttribute : Attribute
	{
		public const string Encrypted = "CommonPolicy";
		public const string ServerAuthenticated = "ServerPolicy";
		public const string EnterpriseServerAuthenticated = "EnterpriseServerPolicy";

		public const bool AllowInsecureHttp = true;
		public const bool UseMessageSecurityOverHttp = true;
		public string Policy { get; set; }
		public PolicyAttribute(string policy) { Policy = policy; }
		public PolicyAttribute(Policies policy)
		{
			switch (policy)
			{
				case Policies.Encrypted: Policy = Encrypted; break;
				case Policies.ServerAuthenticated: Policy = ServerAuthenticated; break;
				case Policies.EnterpriseServerAuthenticated: Policy = EnterpriseServerAuthenticated; break;
			}
		}
	}

}

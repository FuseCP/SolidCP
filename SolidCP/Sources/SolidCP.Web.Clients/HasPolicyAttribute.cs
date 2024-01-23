using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Clients
{
	public enum Policies { Encrypted, ServerAuthenticated, EnterpriseServerAuthenticated }
	public class HasPolicyAttribute : Attribute
	{
		public const string Encrypted = "CommonPolicy";
		public const string ServerAuthenticated = "ServerPolicy";
		public const string EnterpriseServerAuthenticated = "EnterpriseServerPolicy";

		public string Policy { get; set; }
		public HasPolicyAttribute(string policy) { Policy = policy; }
		public HasPolicyAttribute(Policies policy)
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

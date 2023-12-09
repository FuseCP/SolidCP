using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
	public class PolicyAttribute : Attribute
	{

		public const bool AllowInsecureHttp = true;
		public const bool UseMessageSecurityOverHttp = true;
		public string Policy { get; set; }
		public PolicyAttribute(string policy) { Policy = policy; }
	}

}

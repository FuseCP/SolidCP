using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Client
{
	public class HasPolicyAttribute : Attribute
	{

		public string Policy { get; set; }
		public HasPolicyAttribute(string policy) { Policy = policy; }
	}

}

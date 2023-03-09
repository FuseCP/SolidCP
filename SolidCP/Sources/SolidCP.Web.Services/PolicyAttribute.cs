using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Web.Services3
{
	public class PolicyAttribute : Attribute
	{

		public string Policy { get; set; }
		public PolicyAttribute(string policy) { Policy = policy; }
	}

}

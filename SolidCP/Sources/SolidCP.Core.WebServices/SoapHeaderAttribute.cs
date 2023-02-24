using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Services.Protocols
{
	public class SoapHeaderAttribute : Attribute
	{
		public string Field { get; set; }

		public SoapHeaderAttribute() { }
		public SoapHeaderAttribute(string field)
		{
			Field = field;
		}
	}
	public class SoapHeader { }
}
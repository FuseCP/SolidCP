using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class WCFServiceTypesAttribute: Attribute 
	{
		public Type[] Types { get; set; }
		public WCFServiceTypesAttribute(Type[] types)
		{
			Types = types;
		}
	}

    [AttributeUsage(AttributeTargets.Assembly)]
    public class HttpHandlerTypesAttribute : Attribute
    {
        public Type[] Types { get; set; }
        public HttpHandlerTypesAttribute(Type[] types)
        {
            Types = types;
        }
    }
}

using System;

namespace System.Web.Services
{
	public enum WsiProfiles { BasicProfile1_1 }

	public class WebService { }
	public class WebServiceAttribute : Attribute
	{
		public WebServiceAttribute() { }

		public string Namespace { get; set; }
	}

	public class WebServiceBindingAttribute : Attribute
	{
		public WebServiceBindingAttribute() { }

		public WsiProfiles ConformsTo { get; set; }
	}
	public class WebMethodAttribute : Attribute
	{
		public WebMethodAttribute() { }
	}

}
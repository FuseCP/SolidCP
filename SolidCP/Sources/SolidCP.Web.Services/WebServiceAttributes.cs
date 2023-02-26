using System;

namespace System.Web.Services
{
#if NET48
	public enum WsiProfiles { BasicProfile1_1 }
#endif  
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
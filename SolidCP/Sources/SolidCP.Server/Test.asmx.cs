using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers;
using System.Linq;

namespace SolidCP.Server
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]

    public class Test : HostingServiceProviderWebService
    {

        // a simple method echoing the message
        [WebMethod]
        public string Echo(string message)
        {
            return message;
        }

        // a method that receives a soap header. The soap header is written to the field settings of the base type HostingServiceProviderWebService.
        // In the client proxy, the soap header can be set by assign the header to  the SoapHeader property. The soap header is set in the client
        // in SoapHeaderMessageInspector in SolidCP.Web.Clients, and is read in the server by SoapHeaderMessageInspector in SolidCP.Web.Services

        [WebMethod, SoapHeader("settings")]
        public string EchoSettings()
        {
            return settings.Settings.FirstOrDefault() ?? string.Empty;
        }

        [WebMethod]
        public void Touch() { }
    }
}

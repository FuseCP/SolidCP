using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers;
using System.Linq;

namespace SolidCP.Server
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    // setting the policy causes the UserNamePasswordValidator in SolidCP.Web.Services to validate the password against the 
    // server password specified in web.config.
    [Policy("ServerPolicy")]

    [ToolboxItem(false)]

    public class TestWithAuthentication : HostingServiceProviderWebService
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
    }
}

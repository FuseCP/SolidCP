#if !Client
using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers;
using System.Linq;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // setting the policy causes the UserNamePasswordValidator in SolidCP.Web.Services to validate the password against the 
    // server password specified in web.config.
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface ITestWithAuthentication
    {
        [WebMethod]
        [OperationContract]
        string Echo(string message);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string EchoSettings();
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class TestWithAuthentication : SolidCP.Server.TestWithAuthentication, ITestWithAuthentication
    {
    }
}
#endif
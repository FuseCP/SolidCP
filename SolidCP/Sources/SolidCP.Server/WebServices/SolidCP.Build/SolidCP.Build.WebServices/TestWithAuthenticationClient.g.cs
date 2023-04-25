#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ITestWithAuthentication", Namespace = "http://tempuri.org/")]
    public interface ITestWithAuthentication
    {
        [OperationContract(Action = "http://tempuri.org/ITestWithAuthentication/Echo", ReplyAction = "http://tempuri.org/ITestWithAuthentication/EchoResponse")]
        string Echo(string message);
        [OperationContract(Action = "http://tempuri.org/ITestWithAuthentication/Echo", ReplyAction = "http://tempuri.org/ITestWithAuthentication/EchoResponse")]
        System.Threading.Tasks.Task<string> EchoAsync(string message);
        [OperationContract(Action = "http://tempuri.org/ITestWithAuthentication/EchoSettings", ReplyAction = "http://tempuri.org/ITestWithAuthentication/EchoSettingsResponse")]
        string EchoSettings();
        [OperationContract(Action = "http://tempuri.org/ITestWithAuthentication/EchoSettings", ReplyAction = "http://tempuri.org/ITestWithAuthentication/EchoSettingsResponse")]
        System.Threading.Tasks.Task<string> EchoSettingsAsync();
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class TestWithAuthenticationAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ITestWithAuthentication
    {
        public string Echo(string message)
        {
            return Invoke<string>("SolidCP.Server.TestWithAuthentication", "Echo", message);
        }

        public async System.Threading.Tasks.Task<string> EchoAsync(string message)
        {
            return await InvokeAsync<string>("SolidCP.Server.TestWithAuthentication", "Echo", message);
        }

        public string EchoSettings()
        {
            return Invoke<string>("SolidCP.Server.TestWithAuthentication", "EchoSettings");
        }

        public async System.Threading.Tasks.Task<string> EchoSettingsAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.TestWithAuthentication", "EchoSettings");
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class TestWithAuthentication : SolidCP.Web.Client.ClientBase<ITestWithAuthentication, TestWithAuthenticationAssemblyClient>, ITestWithAuthentication
    {
        public string Echo(string message)
        {
            return base.Client.Echo(message);
        }

        public async System.Threading.Tasks.Task<string> EchoAsync(string message)
        {
            return await base.Client.EchoAsync(message);
        }

        public string EchoSettings()
        {
            return base.Client.EchoSettings();
        }

        public async System.Threading.Tasks.Task<string> EchoSettingsAsync()
        {
            return await base.Client.EchoSettingsAsync();
        }
    }
}
#endif
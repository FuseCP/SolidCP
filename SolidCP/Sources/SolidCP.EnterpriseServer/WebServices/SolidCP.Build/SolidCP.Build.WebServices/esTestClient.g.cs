#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesTest", Namespace = "http://tempuri.org/")]
    public interface IesTest
    {
        [OperationContract(Action = "http://tempuri.org/IesTest/Echo", ReplyAction = "http://tempuri.org/IesTest/EchoResponse")]
        string Echo(string msg);
        [OperationContract(Action = "http://tempuri.org/IesTest/Echo", ReplyAction = "http://tempuri.org/IesTest/EchoResponse")]
        System.Threading.Tasks.Task<string> EchoAsync(string msg);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esTestAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesTest
    {
        public string Echo(string msg)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esTest", "Echo", msg);
        }

        public async System.Threading.Tasks.Task<string> EchoAsync(string msg)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esTest", "Echo", msg);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esTest : SolidCP.Web.Client.ClientBase<IesTest, esTestAssemblyClient>, IesTest
    {
        public string Echo(string msg)
        {
            return base.Client.Echo(msg);
        }

        public async System.Threading.Tasks.Task<string> EchoAsync(string msg)
        {
            return await base.Client.EchoAsync(msg);
        }
    }
}
#endif
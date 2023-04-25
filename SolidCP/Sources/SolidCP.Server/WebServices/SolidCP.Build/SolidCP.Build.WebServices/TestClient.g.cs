#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ITest", Namespace = "http://tempuri.org/")]
    public interface ITest
    {
        [OperationContract(Action = "http://tempuri.org/ITest/Echo", ReplyAction = "http://tempuri.org/ITest/EchoResponse")]
        string Echo(string message);
        [OperationContract(Action = "http://tempuri.org/ITest/Echo", ReplyAction = "http://tempuri.org/ITest/EchoResponse")]
        System.Threading.Tasks.Task<string> EchoAsync(string message);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class TestAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ITest
    {
        public string Echo(string message)
        {
            return Invoke<string>("SolidCP.Server.Test", "Echo", message);
        }

        public async System.Threading.Tasks.Task<string> EchoAsync(string message)
        {
            return await InvokeAsync<string>("SolidCP.Server.Test", "Echo", message);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class Test : SolidCP.Web.Client.ClientBase<ITest, TestAssemblyClient>, ITest
    {
        public string Echo(string message)
        {
            return base.Client.Echo(message);
        }

        public async System.Threading.Tasks.Task<string> EchoAsync(string message)
        {
            return await base.Client.EchoAsync(message);
        }
    }
}
#endif
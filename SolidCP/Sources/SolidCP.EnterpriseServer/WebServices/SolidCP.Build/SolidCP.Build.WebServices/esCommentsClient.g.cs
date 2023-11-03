#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesComments", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesComments
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesComments/GetComments", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesComments/GetCommentsResponse")]
        System.Data.DataSet GetComments(int userId, string itemTypeId, int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesComments/GetComments", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesComments/GetCommentsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetCommentsAsync(int userId, string itemTypeId, int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesComments/AddComment", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesComments/AddCommentResponse")]
        int AddComment(string itemTypeId, int itemId, string commentText, int severityId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesComments/AddComment", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesComments/AddCommentResponse")]
        System.Threading.Tasks.Task<int> AddCommentAsync(string itemTypeId, int itemId, string commentText, int severityId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesComments/DeleteComment", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesComments/DeleteCommentResponse")]
        int DeleteComment(int commentId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesComments/DeleteComment", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesComments/DeleteCommentResponse")]
        System.Threading.Tasks.Task<int> DeleteCommentAsync(int commentId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esCommentsAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesComments
    {
        public System.Data.DataSet GetComments(int userId, string itemTypeId, int itemId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esComments", "GetComments", userId, itemTypeId, itemId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetCommentsAsync(int userId, string itemTypeId, int itemId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esComments", "GetComments", userId, itemTypeId, itemId);
        }

        public int AddComment(string itemTypeId, int itemId, string commentText, int severityId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esComments", "AddComment", itemTypeId, itemId, commentText, severityId);
        }

        public async System.Threading.Tasks.Task<int> AddCommentAsync(string itemTypeId, int itemId, string commentText, int severityId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esComments", "AddComment", itemTypeId, itemId, commentText, severityId);
        }

        public int DeleteComment(int commentId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esComments", "DeleteComment", commentId);
        }

        public async System.Threading.Tasks.Task<int> DeleteCommentAsync(int commentId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esComments", "DeleteComment", commentId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esComments : SolidCP.Web.Client.ClientBase<IesComments, esCommentsAssemblyClient>, IesComments
    {
        public System.Data.DataSet GetComments(int userId, string itemTypeId, int itemId)
        {
            return base.Client.GetComments(userId, itemTypeId, itemId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetCommentsAsync(int userId, string itemTypeId, int itemId)
        {
            return await base.Client.GetCommentsAsync(userId, itemTypeId, itemId);
        }

        public int AddComment(string itemTypeId, int itemId, string commentText, int severityId)
        {
            return base.Client.AddComment(itemTypeId, itemId, commentText, severityId);
        }

        public async System.Threading.Tasks.Task<int> AddCommentAsync(string itemTypeId, int itemId, string commentText, int severityId)
        {
            return await base.Client.AddCommentAsync(itemTypeId, itemId, commentText, severityId);
        }

        public int DeleteComment(int commentId)
        {
            return base.Client.DeleteComment(commentId);
        }

        public async System.Threading.Tasks.Task<int> DeleteCommentAsync(int commentId)
        {
            return await base.Client.DeleteCommentAsync(commentId);
        }
    }
}
#endif
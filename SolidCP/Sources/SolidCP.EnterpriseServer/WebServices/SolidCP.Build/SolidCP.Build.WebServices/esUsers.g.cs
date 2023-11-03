#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesUsers
    {
        [WebMethod(Description = "Checks if the account with the specified username exists.")]
        [OperationContract]
        bool UserExists(string username);
        [WebMethod]
        [OperationContract]
        UserInfo GetUserById(int userId);
        [WebMethod]
        [OperationContract]
        UserInfo GetUserByUsername(string username);
        [WebMethod]
        [OperationContract]
        List<UserInfo> GetUsers(int ownerId, bool recursive);
        [WebMethod]
        [OperationContract]
        void AddUserVLan(int userId, UserVlan vLan);
        [WebMethod]
        [OperationContract]
        void DeleteUserVLan(int userId, ushort vLanId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawUsers(int ownerId, bool recursive);
        [WebMethod]
        [OperationContract]
        DataSet GetUsersPaged(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetUsersPagedRecursive(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetUsersSummary(int userId);
        [WebMethod]
        [OperationContract]
        DataSet GetUserDomainsPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetRawUserPeers(int userId);
        [WebMethod]
        [OperationContract]
        List<UserInfo> GetUserPeers(int userId);
        [WebMethod]
        [OperationContract]
        List<UserInfo> GetUserParents(int userId);
        [WebMethod]
        [OperationContract]
        int AddUser(UserInfo user, bool sendLetter, string password, string[] notes);
        [WebMethod]
        [OperationContract]
        int AddUserLiteral(int ownerId, int roleId, int statusId, bool isPeer, bool isDemo, string username, string password, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled, bool sendLetter);
        [WebMethod]
        [OperationContract]
        int UpdateUserTask(string taskId, UserInfo user);
        [WebMethod]
        [OperationContract]
        int UpdateUserTaskAsynchronously(string taskId, UserInfo user);
        [WebMethod]
        [OperationContract]
        int UpdateUser(UserInfo user);
        [WebMethod]
        [OperationContract]
        int UpdateUserLiteral(int userId, int roleId, int statusId, bool isPeer, bool isDemo, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled);
        [WebMethod]
        [OperationContract]
        int DeleteUser(int userId);
        [WebMethod]
        [OperationContract]
        int ChangeUserPassword(int userId, string password);
        [WebMethod]
        [OperationContract]
        bool UpdateUserMfa(string username, bool activate);
        [WebMethod]
        [OperationContract]
        string[] GetUserMfaQrCodeData(string username);
        [WebMethod]
        [OperationContract]
        bool ActivateUserMfaQrCode(string username, string pin);
        [WebMethod]
        [OperationContract]
        int ChangeUserStatus(int userId, UserStatus status);
        [WebMethod]
        [OperationContract]
        UserSettings GetUserSettings(int userId, string settingsName);
        [WebMethod]
        [OperationContract]
        int UpdateUserSettings(UserSettings settings);
        [WebMethod]
        [OperationContract]
        DataSet GetUserThemeSettings(int userId);
        [WebMethod]
        [OperationContract]
        void UpdateUserThemeSetting(int userId, string PropertyName, string PropertyValue);
        [WebMethod]
        [OperationContract]
        void DeleteUserThemeSetting(int userId, string PropertyName);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esUsers : SolidCP.EnterpriseServer.esUsers, IesUsers
    {
    }
}
#endif
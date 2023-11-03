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
    [Policy("CommonPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesAuthentication
    {
        [WebMethod]
        [OperationContract]
        int AuthenticateUser(string username, string password, string ip);
        [WebMethod]
        [OperationContract]
        UserInfo GetUserByUsernamePassword(string username, string password, string ip);
        [WebMethod]
        [OperationContract]
        int ChangeUserPasswordByUsername(string username, string oldPassword, string newPassword, string ip);
        [WebMethod]
        [OperationContract]
        int SendPasswordReminder(string username, string ip);
        [WebMethod]
        [OperationContract]
        bool GetSystemSetupMode();
        [WebMethod]
        [OperationContract]
        int SetupControlPanelAccounts(string passwordA, string passwordB, string ip);
        [WebMethod]
        [OperationContract]
        DataSet GetLoginThemes();
        [WebMethod]
        [OperationContract]
        bool ValidatePin(string username, string pin);
        [WebMethod]
        [OperationContract]
        int SendPin(string username);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esAuthentication : SolidCP.EnterpriseServer.esAuthentication, IesAuthentication
    {
    }
}
#endif
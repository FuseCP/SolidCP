#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
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
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IesSystem
    {
        [WebMethod]
        [OperationContract]
        SystemSettings GetSystemSettings(string settingsName);
        [WebMethod]
        [OperationContract]
        SystemSettings GetSystemSettingsActive(string settingsName, bool decrypt);
        [WebMethod]
        [OperationContract]
        bool CheckIsTwilioEnabled();
        [WebMethod]
        [OperationContract]
        int SetSystemSettings(string settingsName, SystemSettings settings);
        [WebMethod]
        [OperationContract]
        DataSet GetThemes();
        [WebMethod]
        [OperationContract]
        DataSet GetThemeSettings(int ThemeID);
        [WebMethod]
        [OperationContract]
        DataSet GetThemeSetting(int ThemeID, string SettingsName);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esSystem : SolidCP.EnterpriseServer.esSystem, IesSystem
    {
    }
}
#endif
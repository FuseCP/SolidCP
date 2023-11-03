#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesSystem", Namespace = "http://tempuri.org/")]
    public interface IesSystem
    {
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetSystemSettings", ReplyAction = "http://tempuri.org/IesSystem/GetSystemSettingsResponse")]
        SolidCP.EnterpriseServer.SystemSettings GetSystemSettings(string settingsName);
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetSystemSettings", ReplyAction = "http://tempuri.org/IesSystem/GetSystemSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetSystemSettingsAsync(string settingsName);
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetSystemSettingsActive", ReplyAction = "http://tempuri.org/IesSystem/GetSystemSettingsActiveResponse")]
        SolidCP.EnterpriseServer.SystemSettings GetSystemSettingsActive(string settingsName, bool decrypt);
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetSystemSettingsActive", ReplyAction = "http://tempuri.org/IesSystem/GetSystemSettingsActiveResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetSystemSettingsActiveAsync(string settingsName, bool decrypt);
        [OperationContract(Action = "http://tempuri.org/IesSystem/CheckIsTwilioEnabled", ReplyAction = "http://tempuri.org/IesSystem/CheckIsTwilioEnabledResponse")]
        bool CheckIsTwilioEnabled();
        [OperationContract(Action = "http://tempuri.org/IesSystem/CheckIsTwilioEnabled", ReplyAction = "http://tempuri.org/IesSystem/CheckIsTwilioEnabledResponse")]
        System.Threading.Tasks.Task<bool> CheckIsTwilioEnabledAsync();
        [OperationContract(Action = "http://tempuri.org/IesSystem/SetSystemSettings", ReplyAction = "http://tempuri.org/IesSystem/SetSystemSettingsResponse")]
        int SetSystemSettings(string settingsName, SolidCP.EnterpriseServer.SystemSettings settings);
        [OperationContract(Action = "http://tempuri.org/IesSystem/SetSystemSettings", ReplyAction = "http://tempuri.org/IesSystem/SetSystemSettingsResponse")]
        System.Threading.Tasks.Task<int> SetSystemSettingsAsync(string settingsName, SolidCP.EnterpriseServer.SystemSettings settings);
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetThemes", ReplyAction = "http://tempuri.org/IesSystem/GetThemesResponse")]
        System.Data.DataSet GetThemes();
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetThemes", ReplyAction = "http://tempuri.org/IesSystem/GetThemesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetThemesAsync();
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetThemeSettings", ReplyAction = "http://tempuri.org/IesSystem/GetThemeSettingsResponse")]
        System.Data.DataSet GetThemeSettings(int ThemeID);
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetThemeSettings", ReplyAction = "http://tempuri.org/IesSystem/GetThemeSettingsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetThemeSettingsAsync(int ThemeID);
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetThemeSetting", ReplyAction = "http://tempuri.org/IesSystem/GetThemeSettingResponse")]
        System.Data.DataSet GetThemeSetting(int ThemeID, string SettingsName);
        [OperationContract(Action = "http://tempuri.org/IesSystem/GetThemeSetting", ReplyAction = "http://tempuri.org/IesSystem/GetThemeSettingResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetThemeSettingAsync(int ThemeID, string SettingsName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSystemAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesSystem
    {
        public SolidCP.EnterpriseServer.SystemSettings GetSystemSettings(string settingsName)
        {
            return Invoke<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esSystem", "GetSystemSettings", settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetSystemSettingsAsync(string settingsName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esSystem", "GetSystemSettings", settingsName);
        }

        public SolidCP.EnterpriseServer.SystemSettings GetSystemSettingsActive(string settingsName, bool decrypt)
        {
            return Invoke<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esSystem", "GetSystemSettingsActive", settingsName, decrypt);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetSystemSettingsActiveAsync(string settingsName, bool decrypt)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esSystem", "GetSystemSettingsActive", settingsName, decrypt);
        }

        public bool CheckIsTwilioEnabled()
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esSystem", "CheckIsTwilioEnabled");
        }

        public async System.Threading.Tasks.Task<bool> CheckIsTwilioEnabledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esSystem", "CheckIsTwilioEnabled");
        }

        public int SetSystemSettings(string settingsName, SolidCP.EnterpriseServer.SystemSettings settings)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSystem", "SetSystemSettings", settingsName, settings);
        }

        public async System.Threading.Tasks.Task<int> SetSystemSettingsAsync(string settingsName, SolidCP.EnterpriseServer.SystemSettings settings)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSystem", "SetSystemSettings", settingsName, settings);
        }

        public System.Data.DataSet GetThemes()
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esSystem", "GetThemes");
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetThemesAsync()
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esSystem", "GetThemes");
        }

        public System.Data.DataSet GetThemeSettings(int ThemeID)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esSystem", "GetThemeSettings", ThemeID);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetThemeSettingsAsync(int ThemeID)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esSystem", "GetThemeSettings", ThemeID);
        }

        public System.Data.DataSet GetThemeSetting(int ThemeID, string SettingsName)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esSystem", "GetThemeSetting", ThemeID, SettingsName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetThemeSettingAsync(int ThemeID, string SettingsName)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esSystem", "GetThemeSetting", ThemeID, SettingsName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSystem : SolidCP.Web.Client.ClientBase<IesSystem, esSystemAssemblyClient>, IesSystem
    {
        public SolidCP.EnterpriseServer.SystemSettings GetSystemSettings(string settingsName)
        {
            return base.Client.GetSystemSettings(settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetSystemSettingsAsync(string settingsName)
        {
            return await base.Client.GetSystemSettingsAsync(settingsName);
        }

        public SolidCP.EnterpriseServer.SystemSettings GetSystemSettingsActive(string settingsName, bool decrypt)
        {
            return base.Client.GetSystemSettingsActive(settingsName, decrypt);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetSystemSettingsActiveAsync(string settingsName, bool decrypt)
        {
            return await base.Client.GetSystemSettingsActiveAsync(settingsName, decrypt);
        }

        public bool CheckIsTwilioEnabled()
        {
            return base.Client.CheckIsTwilioEnabled();
        }

        public async System.Threading.Tasks.Task<bool> CheckIsTwilioEnabledAsync()
        {
            return await base.Client.CheckIsTwilioEnabledAsync();
        }

        public int SetSystemSettings(string settingsName, SolidCP.EnterpriseServer.SystemSettings settings)
        {
            return base.Client.SetSystemSettings(settingsName, settings);
        }

        public async System.Threading.Tasks.Task<int> SetSystemSettingsAsync(string settingsName, SolidCP.EnterpriseServer.SystemSettings settings)
        {
            return await base.Client.SetSystemSettingsAsync(settingsName, settings);
        }

        public System.Data.DataSet GetThemes()
        {
            return base.Client.GetThemes();
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetThemesAsync()
        {
            return await base.Client.GetThemesAsync();
        }

        public System.Data.DataSet GetThemeSettings(int ThemeID)
        {
            return base.Client.GetThemeSettings(ThemeID);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetThemeSettingsAsync(int ThemeID)
        {
            return await base.Client.GetThemeSettingsAsync(ThemeID);
        }

        public System.Data.DataSet GetThemeSetting(int ThemeID, string SettingsName)
        {
            return base.Client.GetThemeSetting(ThemeID, SettingsName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetThemeSettingAsync(int ThemeID, string SettingsName)
        {
            return await base.Client.GetThemeSettingAsync(ThemeID, SettingsName);
        }
    }
}
#endif
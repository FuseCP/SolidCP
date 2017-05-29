using System.Configuration;
using SolidCP.WebDav.Core.Config.Entities;
using SolidCP.WebDavPortal.WebConfigSections;

namespace SolidCP.WebDav.Core.Config
{
    public class WebDavAppConfigManager : IWebDavAppConfig
    {
        private static WebDavAppConfigManager _instance;
        private readonly WebDavExplorerConfigurationSettingsSection _configSection;

        private WebDavAppConfigManager()
        {
            _configSection = ((WebDavExplorerConfigurationSettingsSection) ConfigurationManager.GetSection(WebDavExplorerConfigurationSettingsSection.SectionName));
            SolidCPConstantUserParameters = new SolidCPConstantUserParameters();
            ElementsRendering = new ElementsRendering();
            SessionKeys = new SessionKeysCollection();
            FileIcons = new FileIconsDictionary();
            HttpErrors = new HttpErrorsCollection();
            OfficeOnline = new OfficeOnlineCollection();
            OwaSupportedBrowsers = new OwaSupportedBrowsersCollection();
            FilesToIgnore = new FilesToIgnoreCollection();
            FileOpener = new OpenerCollection();
            TwilioParameters = new TwilioParameters();
        }

        public static WebDavAppConfigManager Instance
        {
            get { return _instance ?? (_instance = new WebDavAppConfigManager()); }
        }

        public string UserDomain
        {
            get { return _configSection.UserDomain.Value; }
        }

        public string WebdavRoot
        {
            get { return _configSection.WebdavRoot.Value; }
        }

        public string ApplicationName
        {
            get { return _configSection.ApplicationName.Value; }
        }

        public string AuthTimeoutCookieName
        {
            get { return _configSection.AuthTimeoutCookieName.Value; }
        }

        public string EnterpriseServerUrl
        {
            get { return _configSection.EnterpriseServerUrl.Value; }
        }

        public ElementsRendering ElementsRendering { get; private set; }
        public SolidCPConstantUserParameters SolidCPConstantUserParameters { get; private set; }
        public TwilioParameters TwilioParameters { get; private set; }
        public SessionKeysCollection SessionKeys { get; private set; }
        public FileIconsDictionary FileIcons { get; private set; }
        public HttpErrorsCollection HttpErrors { get; private set; }
        public OfficeOnlineCollection OfficeOnline { get; private set; }
        public OwaSupportedBrowsersCollection OwaSupportedBrowsers { get; private set; }
        public FilesToIgnoreCollection FilesToIgnore { get; private set; }
        public OpenerCollection FileOpener { get; private set; }
    }
}
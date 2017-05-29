using System.Configuration;
using SolidCP.WebDav.Core.Config.WebConfigSections;

namespace SolidCP.WebDavPortal.WebConfigSections
{
    public class WebDavExplorerConfigurationSettingsSection : ConfigurationSection
    {
        private const string UserDomainKey = "userDomain";
        private const string WebdavRootKey = "webdavRoot";
        private const string AuthTimeoutCookieNameKey = "authTimeoutCookieName";
        private const string AppName = "applicationName";
        private const string EnterpriseServerUrlNameKey = "enterpriseServer";
        private const string SolidCPConstantUserKey = "SolidCPConstantUser";
        private const string ElementsRenderingKey = "elementsRendering";
        private const string Rfc2898CryptographyKey = "rfc2898Cryptography";
        private const string ConnectionStringsKey = "appConnectionStrings";
        private const string SessionKeysKey = "sessionKeys";
        private const string FileIconsKey = "fileIcons";
        private const string OwaSupportedBrowsersKey = "owaSupportedBrowsers";
        private const string OfficeOnlineKey = "officeOnline";
        private const string FilesToIgnoreKey = "filesToIgnore";
        private const string TypeOpenerKey = "typeOpener";
        private const string TwilioKey = "twilio";

        public const string SectionName = "webDavExplorerConfigurationSettings";

        [ConfigurationProperty(AuthTimeoutCookieNameKey, IsRequired = true)]
        public AuthTimeoutCookieNameElement AuthTimeoutCookieName
        {
            get { return (AuthTimeoutCookieNameElement)this[AuthTimeoutCookieNameKey]; }
            set { this[AuthTimeoutCookieNameKey] = value; }
        }

        [ConfigurationProperty(EnterpriseServerUrlNameKey, IsRequired = true)]
        public EnterpriseServerElement EnterpriseServerUrl
        {
            get { return (EnterpriseServerElement)this[EnterpriseServerUrlNameKey]; }
            set { this[EnterpriseServerUrlNameKey] = value; }
        }

        [ConfigurationProperty(WebdavRootKey, IsRequired = true)]
        public WebdavRootElement WebdavRoot
        {
            get { return (WebdavRootElement)this[WebdavRootKey]; }
            set { this[WebdavRootKey] = value; }
        }

        [ConfigurationProperty(UserDomainKey, IsRequired = true)]
        public UserDomainElement UserDomain
        {
            get { return (UserDomainElement) this[UserDomainKey]; }
            set { this[UserDomainKey] = value; }
        }

        [ConfigurationProperty(AppName, IsRequired = true)]
        public ApplicationNameElement ApplicationName
        {
            get { return (ApplicationNameElement)this[AppName]; }
            set { this[AppName] = value; }
        }

        [ConfigurationProperty(SolidCPConstantUserKey, IsRequired = true)]
        public SolidCPConstantUserElement SolidCPConstantUser
        {
            get { return (SolidCPConstantUserElement)this[SolidCPConstantUserKey]; }
            set { this[SolidCPConstantUserKey] = value; }
        }

        [ConfigurationProperty(TwilioKey, IsRequired = true)]
        public TwilioElement Twilio
        {
            get { return (TwilioElement)this[TwilioKey]; }
            set { this[TwilioKey] = value; }
        }

        [ConfigurationProperty(ElementsRenderingKey, IsRequired = true)]
        public ElementsRenderingElement ElementsRendering
        {
            get { return (ElementsRenderingElement)this[ElementsRenderingKey]; }
            set { this[ElementsRenderingKey] = value; }
        }

        [ConfigurationProperty(SessionKeysKey, IsDefaultCollection = false)]
        public SessionKeysElementCollection SessionKeys
        {
            get { return (SessionKeysElementCollection) this[SessionKeysKey]; }
            set { this[SessionKeysKey] = value; }
        }

        [ConfigurationProperty(FileIconsKey, IsDefaultCollection = false)]
        public FileIconsElementCollection FileIcons
        {
            get { return (FileIconsElementCollection) this[FileIconsKey]; }
            set { this[FileIconsKey] = value; }
        }

        [ConfigurationProperty(OwaSupportedBrowsersKey, IsDefaultCollection = false)]
        public OwaSupportedBrowsersElementCollection OwaSupportedBrowsers
        {
            get { return (OwaSupportedBrowsersElementCollection)this[OwaSupportedBrowsersKey]; }
            set { this[OwaSupportedBrowsersKey] = value; }
        }

        [ConfigurationProperty(OfficeOnlineKey, IsDefaultCollection = false)]
        public OfficeOnlineElementCollection OfficeOnline
        {
            get { return (OfficeOnlineElementCollection)this[OfficeOnlineKey]; }
            set { this[OfficeOnlineKey] = value; }
        }

        [ConfigurationProperty(TypeOpenerKey, IsDefaultCollection = false)]
        public OpenerElementCollection TypeOpener
        {
            get { return (OpenerElementCollection)this[TypeOpenerKey]; }
            set { this[TypeOpenerKey] = value; }
        }

        [ConfigurationProperty(FilesToIgnoreKey, IsDefaultCollection = false)]
        public FilesToIgnoreElementCollection FilesToIgnore
        {
            get { return (FilesToIgnoreElementCollection)this[FilesToIgnoreKey]; }
            set { this[FilesToIgnoreKey] = value; }
        }
    }
}
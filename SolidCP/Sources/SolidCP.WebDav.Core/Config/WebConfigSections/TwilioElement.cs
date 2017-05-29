using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class TwilioElement : ConfigurationElement
    {
        private const string AccountSidPropName = "accountSid";
        private const string AuthorizationTokenPropName = "authorizationToken";
        private const string PhoneFromPropName = "phoneFrom";

        [ConfigurationProperty(AccountSidPropName, IsKey = true, IsRequired = true)]
        public string AccountSid
        {
            get { return this[AccountSidPropName].ToString(); }
            set { this[AccountSidPropName] = value; }
        }

        [ConfigurationProperty(AuthorizationTokenPropName, IsKey = true, IsRequired = true)]
        public string AuthorizationToken
        {
            get { return this[AuthorizationTokenPropName].ToString(); }
            set { this[AuthorizationTokenPropName] = value; }
        }

        [ConfigurationProperty(PhoneFromPropName, IsKey = true, IsRequired = true)]
        public string PhoneFrom
        {
            get { return this[PhoneFromPropName].ToString(); }
            set { this[PhoneFromPropName] = value; }
        }
    }
}
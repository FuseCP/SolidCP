using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class SolidCPConstantUserElement : ConfigurationElement
    {
        private const string LoginKey = "login";
        private const string PasswordKey = "password";

        [ConfigurationProperty(LoginKey, IsKey = true, IsRequired = true)]
        public string Login
        {
            get { return this[LoginKey].ToString(); }
            set { this[LoginKey] = value; }
        }

        [ConfigurationProperty(PasswordKey, IsKey = true, IsRequired = true)]
        public string Password
        {
            get { return this[PasswordKey].ToString(); }
            set { this[PasswordKey] = value; }
        }
    }
}
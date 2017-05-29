using System.Security.Principal;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml.Serialization;

namespace SolidCP.WebDav.Core.Security.Authentication.Principals
{
    public class ScpPrincipal : IPrincipal
    {
        public int AccountId { get; set; }
        public string OrganizationId { get; set; }
        public int ItemId { get; set; }

        public string Login { get; set; }

        public string DisplayName { get; set; }
        public string AccountName { get; set; }

        public string UserName
        {
            get
            {
                return !string.IsNullOrEmpty(Login) ? Login.Split('@')[0] : string.Empty;
            }
        }

        [XmlIgnore, ScriptIgnore]
        public IIdentity Identity { get; private set; }

        public string EncryptedPassword { get; set; }

        public ScpPrincipal(string username)
        {
            Identity = new GenericIdentity(username);//new WindowsIdentity(username, "WindowsAuthentication");
            Login = username;
	    }

        public ScpPrincipal()
        {
        }

        public bool IsInRole(string role)
        {
            return Identity.IsAuthenticated 
                && !string.IsNullOrWhiteSpace(role) 
                && Roles.IsUserInRole(Identity.Name, role);
        }
    }
}

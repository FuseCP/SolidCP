using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.WebDav.Core.Security.Authentication.Principals;

namespace SolidCP.WebDav.Core.Interfaces.Security
{
    public interface IAuthenticationService
    {
        ScpPrincipal LogIn(string login, string password);
        void CreateAuthenticationTicket(ScpPrincipal principal);
        void LogOut();
        bool ValidateAuthenticationData(string login, string password);
    }
}

// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

namespace SolidCP.Providers.Web.Iis.Authentication
{
    using System;

    internal static class AuthenticationGlobals
    {
        public const string ActiveDirectoryAuthenticationModuleName = "ActiveDirectoryAuthentication";
        public const string ActiveDirectoryAuthenticationSectionName = "system.webServer/security/authentication/clientCertificateMappingAuthentication";
        
        public const string AnonymousAuthenticationModuleName = "AnonymousAuthentication";
        public const string AnonymousAuthenticationSectionName = "system.webServer/security/authentication/anonymousAuthentication";
        
        public const string AuthenticationModuleName = "Authentication";
        public const string AuthenticationSectionName = "system.web/authentication";
        
        public const string BasicAuthenticationModuleName = "BasicAuthentication";
        public const string BasicAuthenticationSectionName = "system.webServer/security/authentication/basicAuthentication";
        public const string DigestAuthenticationModuleName = "DigestAuthentication";
        public const string DigestAuthenticationSectionName = "system.webServer/security/authentication/digestAuthentication";

        public const string PasswordProperty = "password";

        public const string WindowsAuthenticationModuleName = "WindowsAuthentication";
        public const string WindowsAuthenticationSectionName = "system.webServer/security/authentication/windowsAuthentication";


        public const int AnonymousAuthenticationUserName = 0;
        public const int AnonymousAuthenticationPassword = 1;

        public const int BasicAuthenticationDefaultLogonDomain = 2;
        public const int BasicAuthenticationRealm = 3;

        public const int DigestAuthenticationRealm = 4;
        public const int DigestAuthenticationHasDomain = 5;

        public const int FormsAuthenticationLoginPageUrl = 6;
        public const int FormsAuthenticationExpiration = 7;
        public const int FormsAuthenticationCookieMode = 8;
        public const int FormsAuthenticationCookieName = 9;
        public const int FormsAuthenticationProtectionMode = 10;
        public const int FormsAuthenticationRequireSsl = 11;
        public const int FormsAuthenticationSlidingExpiration = 12;

        public const int ActiveDirectoryAuthenticationHasSslRequirements = 13;

        public const int ModuleName = 14;

        public const int Enabled = 15;

        public const int IsLocked = 16;
    }
}


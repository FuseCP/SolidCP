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

namespace SolidCP.Providers.HostedSolution
{
    public class ADAttributes
    {
        public const string Initials = "initials";
        public const string JobTitle = "title";
        public const string Company = "company";
        public const string Department = "department";
        public const string Office = "physicalDeliveryOfficeName";
        public const string BusinessPhone = "telephoneNumber";
        public const string Fax = "facsimileTelephoneNumber";
        public const string HomePhone = "homePhone";
        public const string MobilePhone = "mobile";
        public const string Pager = "pager";
        public const string WebPage = "wWWHomePage";
        public const string Address = "streetAddress";
        public const string City = "l";
        public const string State = "st";
        public const string Zip = "postalCode";
        public const string Country = "c";
        public const string Notes = "info";
        public const string FirstName = "givenName";
        public const string LastName = "sn";
        public const string DisplayName = "displayName";
        public const string AccountDisabled = "AccountDisabled";
        public const string AccountLocked = "IsAccountLocked";
        public const string Manager = "manager";
        public const string SetPassword = "SetPassword";
        public const string SAMAccountName = "sAMAccountName";
        public const string UserPrincipalName = "UserPrincipalName";
        public const string GroupType = "GroupType";
        public const string Name = "Name";
        public const string ExternalEmail = "mail";
        public const string CustomAttribute2 = "extensionAttribute2";
        public const string DistinguishedName = "distinguishedName";
        public const string SID = "objectSid";
        public const string PwdLastSet = "pwdLastSet";
        public const string PasswordExpirationDateTime = "msDS-UserPasswordExpiryTimeComputed";
        public const string PasswordNeverExpires = "PasswordNeverExpires";
        public const string UserAccountControl = "UserAccountControl";
        public const string Description = "description";
        public const string dSHeuristics = "dSHeuristics";

        // ACL Attributes
        public const string ReadCn = @"bf96793f-0de6-11d0-a285-00aa003049e2";
        public const string ReadCanonicalName = @"9a7ad945-ca53-11d1-bbd0-0080c76670c0";
        public const string ReadDistinguishedName = @"bf9679e4-0de6-11d0-a285-00aa003049e2";
        public const string ReadGpLink = @"f30e3bbe-9ff0-11d1-b603-0000f80367c1";
        public const string ReadGpOption = @"f30e3bbf-9ff0-11d1-b603-0000f80367c1";
    }
}

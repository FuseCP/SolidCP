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

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Globalization;
using System.Text;
using System.Linq;

namespace SolidCP.Providers.HostedSolution
{
    public class ActiveDirectoryUtils
    {
        public static DirectoryEntry GetADObject(string path)
        {
            DirectoryEntry de = new DirectoryEntry(path);
            de.RefreshCache();
            return de;
        }

        public static string[] GetGroupObjects(string group, string objectType)
        {
            return GetGroupObjects(group, objectType, null);
        }

        public static string[] GetGroupObjects(string group, string objectType, DirectoryEntry entry)
        {
            List<string> rets = new List<string>();  

            DirectorySearcher deSearch = new DirectorySearcher
            {
                Filter =
                    "(&(objectClass=" + objectType + "))"
            };

            if (entry != null)
            {
                deSearch.SearchRoot = entry;
            }

            SearchResultCollection srcObjects = deSearch.FindAll();

            foreach (SearchResult srcObject in srcObjects)
            {
                DirectoryEntry de = srcObject.GetDirectoryEntry();
                PropertyValueCollection props = de.Properties["memberOf"];

                foreach (string str in props)
                {
                    string[] parts = str.Split(',');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i].StartsWith("CN="))
                        {
                            if (parts[i].Substring(3) == group)
                            {
                                rets.Add(de.Path);
                            }
                            break;
                        }
                    }
                }
            }

            return rets.ToArray();
        }

        public static DirectoryEntry GetGroupPolicyContainer(string displayName)
        {
            DirectorySearcher deSearch = new DirectorySearcher
            {
                Filter =
                    ("(&(objectClass=groupPolicyContainer)(displayName=" + displayName + "))")
            };

            SearchResult results = deSearch.FindOne();
            DirectoryEntry de = results.GetDirectoryEntry();

            return de;
        }

        public static bool IsUserInGroup(string samAccountName, string group)
        {
            bool res = false;
            DirectorySearcher deSearch = new DirectorySearcher
            {
                Filter =
                    ("(&(objectClass=user)(samaccountname=" + samAccountName + "))")
            };

            //get the group result
            SearchResult results = deSearch.FindOne();
            DirectoryEntry de = results.GetDirectoryEntry();
            PropertyValueCollection props = de.Properties["memberOf"];

            foreach (string str in props)
            {
                if (str.IndexOf(group) != -1)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public static bool IsComputerInGroup(string samAccountName, string group)
        {
            bool res = false;
            DirectorySearcher deSearch = new DirectorySearcher
            {
                Filter =
                    ("(&(objectClass=computer)(samaccountname=" + samAccountName + "))")
            };

            //get the group result
            SearchResult results = deSearch.FindOne();
            DirectoryEntry de = results.GetDirectoryEntry();
            PropertyValueCollection props = de.Properties["memberOf"];

            foreach (string str in props)
            {
                if (str.IndexOf(group) != -1)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public static string CreateOrganizationalUnit(string name, string parentPath)
        {
            string ret;
            DirectoryEntry ou = null;
            DirectoryEntry parent = null;

            try
            {
                parent = GetADObject(parentPath);

                ou = parent.Children.Add(
                     string.Format("OU={0}", name),
                     parent.SchemaClassName);

                ret = ou.Path;
                ou.CommitChanges();

            }
            finally
            {
                if (ou != null)
                    ou.Close();
                if (parent != null)
                    parent.Close();
            }

            return ret;
        }

        public static void DeleteADObject(string path, bool removeChild)
        {
            DirectoryEntry entry = GetADObject(path);

            if (removeChild && entry.Children != null)
            {
                foreach (DirectoryEntry child in entry.Children)
                {
                    //entry.Children.Remove(child);
                    child.DeleteTree();
                }
                entry.CommitChanges();
            }

            DirectoryEntry parent = entry.Parent;
            if (parent != null)
            {
                //parent.Children.Remove(entry);
                entry.DeleteTree();
                parent.CommitChanges();
            }
        }


        public static void DeleteADObject(string path)
        {
            DirectoryEntry entry = GetADObject(path);
            DirectoryEntry parent = entry.Parent;
            if (parent != null)
            {
                parent.Children.Remove(entry);
                parent.CommitChanges();
            }
        }

        public static void SetADObjectProperty(DirectoryEntry oDE, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (oDE.Properties.Contains(name))
                {
                    oDE.Properties[name][0] = value;
                }
                else
                {
                    oDE.Properties[name].Add(value);
                }
            }
            else
            {
                if (oDE.Properties.Contains(name))
                {
                    oDE.Properties[name].Remove(oDE.Properties[name][0]);
                }

            }
        }

        public static void SetADObjectPropertyValue(DirectoryEntry oDE, string name, string value)
        {
            PropertyValueCollection collection = oDE.Properties[name];
            collection.Value = value;
        }

        public static void SetADObjectPropertyValue(DirectoryEntry oDE, string name, string[] values)
        {
            PropertyValueCollection collection = oDE.Properties[name];
            collection.Value = values;
        }



        public static void SetADObjectPropertyValue(DirectoryEntry oDE, string name, Guid value)
        {
            PropertyValueCollection collection = oDE.Properties[name];
            collection.Value = value.ToByteArray();
        }

        public static void ClearADObjectPropertyValue(DirectoryEntry oDE, string name)
        {
            PropertyValueCollection collection = oDE.Properties[name];
            collection.Clear();
        }


        public static object GetADObjectProperty(DirectoryEntry entry, string name)
        {
            return entry.Properties.Contains(name) ? entry.Properties[name][0] : null;
        }

        public static string[] GetADObjectPropertyMultiValue(DirectoryEntry entry, string name)
        {
            if (!entry.Properties.Contains(name))
                return null;

            List<string> returnList = new List<string>();
            for (int i = 0; i < entry.Properties[name].Count; i++)
                returnList.Add(entry.Properties[name][i].ToString());

            return returnList.ToArray();
        }


        public static string GetADObjectStringProperty(DirectoryEntry entry, string name)
        {
            object ret = GetADObjectProperty(entry, name);
            return ret != null ? ret.ToString() : string.Empty;
        }

        public static string GetCNFromADPath(string path)
        {
            string[] parts = path.Substring(path.ToUpper().IndexOf("CN=")).Split(',');

            if (parts.Length > 0)
            {
                return parts[0].Substring(3);
            }

            return null;
        }

        public static string ConvertADPathToCanonicalName(string name)
        {

            if (string.IsNullOrEmpty(name))
                return null;

            StringBuilder ret = new StringBuilder();
            List<string> cn = new List<string>();
            List<string> dc = new List<string>();

            name = RemoveADPrefix(name);

            string[] parts = name.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].StartsWith("DC="))
                {
                    dc.Add(parts[i].Substring(3));
                }
                else if (parts[i].StartsWith("OU=") || parts[i].StartsWith("CN="))
                {
                    cn.Add(parts[i].Substring(3));
                }
            }

            for (int i = 0; i < dc.Count; i++)
            {
                ret.Append(dc[i]);
                if (i < dc.Count - 1)
                    ret.Append(".");
            }
            for (int i = cn.Count - 1; i != -1; i--)
            {
                ret.Append("/");
                ret.Append(cn[i]);
            }
            return ret.ToString();
        }

        public static string ConvertDomainName(string name)
        {

            if (string.IsNullOrEmpty(name))
                return null;

            StringBuilder ret = new StringBuilder("LDAP://");

            string[] parts = name.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                ret.Append("DC=");
                ret.Append(parts[i]);
                if (i < parts.Length - 1)
                    ret.Append(",");
            }
            return ret.ToString();
        }

        public static string GetNETBIOSDomainName(string rootDomain)
        {
            string ret = string.Empty;

            string path = string.Format("LDAP://{0}/RootDSE", rootDomain);
            DirectoryEntry rootDSE = GetADObject(path);
            string contextPath = GetADObjectProperty(rootDSE, "ConfigurationNamingContext").ToString();
            string defaultContext = GetADObjectProperty(rootDSE, "defaultNamingContext").ToString();
            DirectoryEntry partitions = GetADObject("LDAP://cn=Partitions," + contextPath);

            DirectorySearcher searcher = new DirectorySearcher();
            searcher.SearchRoot = partitions;
            searcher.Filter = string.Format("(&(objectCategory=crossRef)(nCName={0}))", defaultContext);
            searcher.SearchScope = SearchScope.OneLevel;

            //find the first instance
            SearchResult result = searcher.FindOne();
            if (result != null)
            {
                DirectoryEntry partition = GetADObject(result.Path);
                ret = GetADObjectProperty(partition, "nETBIOSName").ToString();
                partition.Close();
            }
            partitions.Close();
            rootDSE.Close();

            return ret;
        }


        public static string CreateUser(string path, string user, string displayName, string password, bool enabled)
        {
            return CreateUser(path, "", user, displayName, password, enabled);
        }

        public static string CreateUser(string path, string upn, string user, string displayName, string password, bool enabled)
        {
            DirectoryEntry currentADObject = new DirectoryEntry(path);

            string cn = string.Empty;
            if (string.IsNullOrEmpty(upn)) cn = user; else cn = upn;

            DirectoryEntry newUserObject = currentADObject.Children.Add("CN=" + cn, "User");

            newUserObject.Properties[ADAttributes.SAMAccountName].Add(user);
            SetADObjectProperty(newUserObject, ADAttributes.DisplayName, displayName);
            newUserObject.CommitChanges();
            newUserObject.Invoke(ADAttributes.SetPassword, password);
            newUserObject.InvokeSet(ADAttributes.AccountDisabled, !enabled);

            newUserObject.CommitChanges();

            return newUserObject.Path;
        }

        public static void CreateGroup(string path, string group)
        {
            DirectoryEntry currentADObject = new DirectoryEntry(path);

            DirectoryEntry newGroupObject = currentADObject.Children.Add("CN=" + group, "Group");

            newGroupObject.Properties[ADAttributes.SAMAccountName].Add(group);

            newGroupObject.Properties[ADAttributes.GroupType].Add(-2147483640);

            newGroupObject.CommitChanges();
        }

        public static void AddObjectToGroup(string objectPath, string groupPath)
        {
            DirectoryEntry obj = new DirectoryEntry(objectPath);
            DirectoryEntry group = new DirectoryEntry(groupPath);

            group.Invoke("Add", obj.Path);

            group.CommitChanges();
        }

        public static void RemoveObjectFromGroup(string obejctPath, string groupPath)
        {
            DirectoryEntry obj = new DirectoryEntry(obejctPath);
            DirectoryEntry group = new DirectoryEntry(groupPath);

            group.Invoke("Remove", obj.Path);

            group.CommitChanges();
        }

        public static bool AdObjectExists(string path)
        {
            try
            {
                return DirectoryEntry.Exists(path);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unknown error (0x80005000)")
                {
                    throw new Exception("Probably an authentication issue. Did you set the Active Directory settings of the involved SCP Server correctly (NONE/yourADdomain.com/blank/blank)?", ex);
                }
                throw;
            }
        }

        public static string RemoveADPrefix(string path)
        {
            string dn = path;
            if (dn.ToUpper().StartsWith("LDAP://"))
            {
                dn = dn.Substring(7);
            }
            int index = dn.IndexOf("/");

            if (index != -1)
            {
                dn = dn.Substring(index + 1);
            }
            return dn;
        }

        public static string AddADPrefix(string path, string primaryDomainController)
        {
            string dn = path;
            if (!dn.ToUpper().StartsWith("LDAP://"))
            {
                if (string.IsNullOrEmpty(primaryDomainController))
                {
                    dn = string.Format("LDAP://{0}", dn);

                }
                else
                    dn = string.Format("LDAP://{0}/{1}", primaryDomainController, dn);
            }
            return dn;
        }

        public static string AddADPrefix(string path)
        {
            return AddADPrefix(path, null);
        }

        private static void AddADObjectProperty(DirectoryEntry oDE, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                oDE.Properties[name].Add(value);

            }
        }

        public static void AddUPNSuffix(string ouPath, string suffix)
        {
            //Add UPN Suffix to the OU
            DirectoryEntry ou = GetADObject(ouPath);
            AddADObjectProperty(ou, "uPNSuffixes", suffix);
            ou.CommitChanges();
            ou.Close();
        }

        public static void RemoveUPNSuffix(string ouPath, string suffix)
        {
            if (DirectoryEntry.Exists(ouPath))
            {
                DirectoryEntry ou = GetADObject(ouPath);
                PropertyValueCollection prop = null;

                prop = ou.Properties["uPNSuffixes"];

                if (prop != null)
                {
                    if (ou.Properties["uPNSuffixes"].Contains(suffix))
                    {
                        ou.Properties["uPNSuffixes"].Remove(suffix);
                        ou.CommitChanges();
                    }
                    ou.Close();
                }
            }
        }


        public static string GetDomainName(string userName)
        {
            string str4;
            string filter = string.Format(CultureInfo.InvariantCulture, "(&(objectClass=user)(Name={0}))", new object[] { userName });
            using (DirectoryEntry entry = Domain.GetComputerDomain().GetDirectoryEntry())
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry, filter))
                {
                    searcher.PropertiesToLoad.Add("sAMAccountName");
                    string str2 = searcher.FindOne().Properties["sAMAccountName"][0] as string;
                    str4 = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", new object[] { entry.Properties["Name"].Value as string, str2 });
                }
            }
            return str4;
        }

        public static void AddOUSecurityfromUser(string ouPath, string domain, string Username, ActiveDirectoryRights rights, AccessControlType type, ActiveDirectorySecurityInheritance inheritance)
        {
                DirectoryEntry ou = GetADObject(ouPath);
                

                NTAccount ADUser = new NTAccount(domain, Username);

                ActiveDirectoryAccessRule ruleRead = new ActiveDirectoryAccessRule(
                                                                ADUser,
                                                                rights,
                                                                type,
                                                                inheritance
                                                                );

                ou.ObjectSecurity.AddAccessRule(ruleRead);
                ou.CommitChanges();
                ou.Close();
            
        }

        public static void RemoveOUSecurityfromUser(string ouPath, string domain, string Username, ActiveDirectoryRights rights, AccessControlType type, ActiveDirectorySecurityInheritance inheritance)
        {
                DirectoryEntry ou = GetADObject(ouPath);
                NTAccount ADUser = new NTAccount(domain, Username);
                ActiveDirectoryAccessRule ruleRead = new ActiveDirectoryAccessRule(
                                                                ADUser,
                                                                rights,
                                                                type,
                                                                inheritance
                                                                );

                ou.ObjectSecurity.RemoveAccessRule(ruleRead);
                ou.CommitChanges();
                ou.Close();
        }

        public static void RemoveOUSecurityfromSid(string ouPath, WellKnownSidType UserSid, ActiveDirectoryRights rights, AccessControlType type, ActiveDirectorySecurityInheritance inheritance)
        {
            DirectoryEntry ou = GetADObject(ouPath);
            

            SecurityIdentifier identity = null;
            identity = new SecurityIdentifier(UserSid, null);

            ActiveDirectoryAccessRule ruleRead = new ActiveDirectoryAccessRule(
                                                            identity,
                                                            rights,
                                                            type,
                                                            inheritance
                                                            );

            

            ou.ObjectSecurity.RemoveAccessRule(ruleRead);
            ou.CommitChanges();
            ou.Close();
        }

        #region ACL

        public static bool IsInheritanceEnabled(string objectPath)
        {
            return !new DirectoryEntry(objectPath).ObjectSecurity.AreAccessRulesProtected;
        }

        public static void DisableInheritance(string objectPath)
        {
            var obj = new DirectoryEntry(objectPath);
            obj.ObjectSecurity.SetAccessRuleProtection(true, true);
            obj.CommitChanges();
            CanonicalizeDacl(objectPath);
        }

        private static void CanonicalizeDacl(string objectPath)
        {
            var obj = new DirectoryEntry(objectPath);
            ActiveDirectorySecurity objectSecurity = obj.ObjectSecurity;
            if (objectSecurity == null) { throw new ArgumentNullException("objectSecurity"); }
            if (objectSecurity.AreAccessRulesCanonical) { return; }

            // A canonical ACL must have ACES sorted according to the following order:
            //   1. Access-denied on the object
            //   2. Access-denied on a child or property
            //   3. Access-allowed on the object
            //   4. Access-allowed on a child or property
            //   5. All inherited ACEs 
            RawSecurityDescriptor descriptor = new RawSecurityDescriptor(objectSecurity.GetSecurityDescriptorSddlForm(AccessControlSections.Access));

            List<QualifiedAce> implicitDenyDacl = new List<QualifiedAce>();
            List<QualifiedAce> implicitDenyObjectDacl = new List<QualifiedAce>();
            List<QualifiedAce> inheritedDacl = new List<QualifiedAce>();
            List<QualifiedAce> implicitAllowDacl = new List<QualifiedAce>();
            List<QualifiedAce> implicitAllowObjectDacl = new List<QualifiedAce>();

            foreach (QualifiedAce ace in descriptor.DiscretionaryAcl)
            {
                if ((ace.AceFlags & AceFlags.Inherited) == AceFlags.Inherited) { inheritedDacl.Add(ace); }
                else
                {
                    switch (ace.AceType)
                    {
                        case AceType.AccessAllowed:
                            implicitAllowDacl.Add(ace);
                            break;

                        case AceType.AccessDenied:
                            implicitDenyDacl.Add(ace);
                            break;

                        case AceType.AccessAllowedObject:
                            implicitAllowObjectDacl.Add(ace);
                            break;

                        case AceType.AccessDeniedObject:
                            implicitDenyObjectDacl.Add(ace);
                            break;
                    }
                }
            }

            Int32 aceIndex = 0;
            RawAcl newDacl = new RawAcl(descriptor.DiscretionaryAcl.Revision, descriptor.DiscretionaryAcl.Count);
            implicitDenyDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            implicitDenyObjectDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            implicitAllowDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            implicitAllowObjectDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            inheritedDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));

            if (aceIndex != descriptor.DiscretionaryAcl.Count)
            {
                throw new ArgumentException("The DACL cannot be canonicalized since it would potentially result in a loss of information");
            }

            descriptor.DiscretionaryAcl = newDacl;
            objectSecurity.SetSecurityDescriptorSddlForm(descriptor.GetSddlForm(AccessControlSections.Access), AccessControlSections.Access);
            obj.CommitChanges();
        }

        public static bool IsIdentityAllowed(string objectPath, IdentityReference identity)
        {
            var obj = new DirectoryEntry(objectPath);
            return GetRules(obj.ObjectSecurity, identity).Any();
        }

        public static int GetIdentityAllowedCount(string objectPath, IdentityReference identity)
        {
            var obj = new DirectoryEntry(objectPath);
            return GetRules(obj.ObjectSecurity, identity).Count();
        }

        public static bool IsIdentityExistsNotInherited(string objectPath, IdentityReference identity)
        {
            var obj = new DirectoryEntry(objectPath);
            return GetRules(obj.ObjectSecurity, identity).Any(r => !r.IsInherited);
        }

        public static void RemoveIdentityAllows(string objectPath, IdentityReference identity)
        {
            var obj = new DirectoryEntry(objectPath);
            //obj.ObjectSecurity.PurgeAccessRules(identity);
            var rulesForDelete = GetRules(obj.ObjectSecurity, identity).ToList();
            rulesForDelete.ForEach(r => obj.ObjectSecurity.RemoveAccessRule(r));
            obj.CommitChanges();
        }

        public static void RemoveIdentityNotInheritedRules(string objectPath, IdentityReference identity)
        {
            var obj = new DirectoryEntry(objectPath);
            var rulesForDelete = GetRules(obj.ObjectSecurity, identity).Where(r => !r.IsInherited).ToList();
            rulesForDelete.ForEach(r => obj.ObjectSecurity.RemoveAccessRule(r));
            obj.CommitChanges();
        }

        public static bool HasPermission(string objectPath, IdentityReference identity, ActiveDirectoryRights permission)
        {
            var obj = new DirectoryEntry(objectPath);
            return GetRules(obj.ObjectSecurity, identity).Any(r => r.ActiveDirectoryRights == permission);
        }

        public static void AddPermission(string objectPath, IdentityReference identity, ActiveDirectoryRights permission, ActiveDirectorySecurityInheritance inheritance = ActiveDirectorySecurityInheritance.All)
        {
            var obj = new DirectoryEntry(objectPath);
            var rule = new ActiveDirectoryAccessRule(identity, permission, AccessControlType.Allow, inheritance);
            obj.ObjectSecurity.AddAccessRule(rule);
            obj.CommitChanges();
        }

        public static bool HasPropertyAccess(string objectPath, IdentityReference identity, string propertyGuid)
        {
            var obj = new DirectoryEntry(objectPath);
            return GetRules(obj.ObjectSecurity, identity).Any(r => r.ActiveDirectoryRights == ActiveDirectoryRights.ReadProperty && r.ObjectType == new Guid(propertyGuid));
        }

        public static void AddPropertyAccess(string objectPath, IdentityReference identity, string propertyGuid)
        {
            var obj = new DirectoryEntry(objectPath);
            var rule = new ActiveDirectoryAccessRule(identity, ActiveDirectoryRights.ReadProperty, AccessControlType.Allow, new Guid(propertyGuid), ActiveDirectorySecurityInheritance.All);
            obj.ObjectSecurity.AddAccessRule(rule);
            obj.CommitChanges();
        }

        private static IEnumerable<ActiveDirectoryAccessRule> GetRules(ActiveDirectorySecurity security, IdentityReference identity)
        {
            return security
                .GetAccessRules(true, true, typeof(SecurityIdentifier))
                .Cast<ActiveDirectoryAccessRule>()
                .Where(r => r.IdentityReference.Value == GetIdentitySid(identity))
                .Where(r => r.AccessControlType == AccessControlType.Allow);
        }

        public static void AddOrgPermisionsToIdentity(string orgPath, IdentityReference identity, bool isOu = true)
        {
            RemoveIdentityAllows(orgPath, identity);

            AddPermission(orgPath, identity, ActiveDirectoryRights.ListObject);

            if (isOu)
            {
                AddPropertyAccess(orgPath, identity, ADAttributes.ReadCn);
                AddPropertyAccess(orgPath, identity, ADAttributes.ReadGpLink);
                AddPropertyAccess(orgPath, identity, ADAttributes.ReadGpOption);
            }
            else
            {
                AddPropertyAccess(orgPath, identity, ADAttributes.ReadCanonicalName);
            }

            AddPropertyAccess(orgPath, identity, ADAttributes.ReadDistinguishedName);
        }


        public static string GetObjectTargetAccountName(string accountName, string domain)
        {
            return $"{domain}\\{accountName}";
        }

        private static string GetIdentitySid(IdentityReference identity)
        {
            return identity.Translate(typeof(SecurityIdentifier)).Value;
        }

        public static bool AccountExists(string name)
        {
            bool bRet = false;

            try
            {
                NTAccount acct = new NTAccount(name);
                SecurityIdentifier id = (SecurityIdentifier)acct.Translate(typeof(SecurityIdentifier));

                bRet = id.IsAccountSid();
            }
            catch (IdentityNotMappedException)
            {
                /* Invalid account */
            }

            return bRet;
        }

        #endregion

    }
}

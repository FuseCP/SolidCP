using System.Collections.Generic;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Entities.Account;
using SolidCP.WebDav.Core.Security.Authorization.Enums;
using SolidCP.WebDavPortal.Models.Common;

namespace SolidCP.WebDavPortal.Models
{
    public class ModelForWebDav 
    {
        public IEnumerable<IHierarchyItem> Items { get; set; }
        public string UrlSuffix { get; set; }
        public string Error { get; set; }
        public string SearchValue { get; set; }
        public WebDavPermissions Permissions { get; set; }
        public UserPortalSettings UserSettings { get; set; }
    }
}
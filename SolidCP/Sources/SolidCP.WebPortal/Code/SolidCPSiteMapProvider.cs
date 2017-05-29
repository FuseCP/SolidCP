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
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SolidCP.WebPortal
{
    public class SolidCPSiteMapProvider : SiteMapProvider
    {
        private const string DEFAULT_PAGE_URL = "~/Default.aspx?pid=";
        private const string ROOT_NODE_KEY = "scp_root";
        private const string PAGE_ID_PARAM = "pid";

        private SiteMapProvider parentSiteMapProvider = null;

        public SolidCPSiteMapProvider()
        {
        }

        // Implement the CurrentNode property.
        public override SiteMapNode CurrentNode
        {
            get
            {
                // page id
                string pid = GetCurrentPageID();

                // find page by id
                if (PortalConfiguration.Site.Pages.ContainsKey(pid))
                {
                    return CreateNodeFromPage(PortalConfiguration.Site.Pages[pid]);
                }
                return null;
            }
        }

        // Implement the RootNode property.
        public override SiteMapNode RootNode
        {
            get { return new SiteMapNode(this, ROOT_NODE_KEY, "", ""); }
        }

        // Implement the ParentProvider property.
        public override SiteMapProvider ParentProvider
        {
            get { return parentSiteMapProvider; }
            set { parentSiteMapProvider = value; }
        }

        // Implement the RootProvider property.
        public override SiteMapProvider RootProvider
        {
            get
            {
                // If the current instance belongs to a provider hierarchy, it
                // cannot be the RootProvider. Rely on the ParentProvider.
                if (this.ParentProvider != null)
                {
                    return ParentProvider.RootProvider;
                }
                // If the current instance does not have a ParentProvider, it is
                // not a child in a hierarchy, and can be the RootProvider.
                else
                {
                    return this;
                }
            }
        }

        // Implement the FindSiteMapNode method.
        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            int idx = rawUrl.IndexOf("?pid=");
            if (idx == -1)
                return null;

            // page id
            string pid = null;
            int ampIdx = rawUrl.IndexOf("&", idx);
            pid = (ampIdx == -1) ? rawUrl.Substring(idx + 5) : rawUrl.Substring(idx + 5, ampIdx - idx - 5);

            // find page by id
            if (PortalConfiguration.Site.Pages.ContainsKey(pid))
            {
                return CreateNodeFromPage(PortalConfiguration.Site.Pages[pid]);
            }
            return null;
        }

        // Implement the GetChildNodes method.
        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            // pid
            string pid = node.Key;

            SiteMapNodeCollection children = new SiteMapNodeCollection();

            if (PortalConfiguration.Site.Pages.ContainsKey(pid))
            {
                // fill collection
                foreach (PortalPage page in PortalConfiguration.Site.Pages[pid].Pages)
                {
                    if (page.Hidden)
                        continue;

                    SiteMapNode childNode = CreateNodeFromPage(page);
                    if (childNode != null)
                        children.Add(childNode);
                }
            }
            else
            {
                // check if this is a root node
                if (node.Key == ROOT_NODE_KEY)
                {
                    foreach (PortalPage page in PortalConfiguration.Site.Pages.Values)
                    {
                        if (page.ParentPage == null && !page.Hidden)
                        {
                            SiteMapNode childNode = CreateNodeFromPage(page);
                            if (childNode != null)
                                children.Add(childNode);
                        }
                    }
                }
            }
            return children;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }

        // Implement the GetParentNode method.
        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            string pid = node.Key;
            if (pid == ROOT_NODE_KEY)
                return null;

            // find page
            if (PortalConfiguration.Site.Pages.ContainsKey(pid))
            {
                PortalPage page = PortalConfiguration.Site.Pages[pid];
                if (page.ParentPage != null)
                    return CreateNodeFromPage(page.ParentPage);
            }
            return null;
        }

        // Implement the ProviderBase.Initialize property.
        // Initialize is used to initialize the state that the Provider holds, but
        // not actually build the site map.
        public override void Initialize(string name, NameValueCollection attributes)
        {
            lock (this)
            {
                base.Initialize(name, attributes);
            }
        }

        private SiteMapNode CreateNodeFromPage(PortalPage page)
        {
			string url = String.IsNullOrEmpty(page.Url) ? DEFAULT_PAGE_URL + page.Name : page.Url;

            string localizedName = DefaultPage.GetLocalizedPageName(page.Name);

            NameValueCollection attrs = new NameValueCollection();
            attrs["target"] = page.Target;
            attrs["align"] = page.Align;

            SiteMapNode node = new SiteMapNode(this, page.Name,
                url,
                localizedName,
                localizedName, page.Roles, attrs, null, null);

            if (!page.Enabled)
                node.Url = "";

            if (IsNodeAccessibleToUser(HttpContext.Current, node))
            {
                return node;
            }
            return null;
        }

        private bool IsNodeAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (!this.SecurityTrimmingEnabled)
            {
                return true;
            }
            if (node.Roles != null)
            {
                return DefaultPage.IsAccessibleToUser(HttpContext.Current, node.Roles);
            }
            return false;
        }

        // Private helper methods
        // Get the URL of the currently displayed page.
        private string GetCurrentPageID()
        {
            try
            {
                // The current HttpContext.
                HttpContext currentContext = HttpContext.Current;
                if (currentContext != null)
                {
                    return (currentContext.Request[PAGE_ID_PARAM] != null) ? currentContext.Request[PAGE_ID_PARAM] : "";
                }
                else
                {
                    throw new Exception("HttpContext.Current is Invalid");
                }
            }
            catch (Exception e)
            {
                throw new NotSupportedException("This provider requires a valid context.", e);
            }
        }
    }
}

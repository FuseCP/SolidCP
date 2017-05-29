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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.WebPortal;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.SkinControls
{
    public partial class TopMenu : System.Web.UI.UserControl
    {
        public string Align
        {
            get
            {
                if (ViewState["Align"] == null)
                {
                    return "top"; 
                }
                return ViewState["Align"].ToString(); 
            }
            set { ViewState["Align"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void topMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            var node = ((SiteMapNode)e.Item.DataItem);
            e.Item.Value = node.Key; 
            if (node["align"] == Align)
            {
                topMenu.Items.Remove(e.Item);
                return;
            }
            
            //if (Align.Equals("left") && node.Title.ToLower().Equals("account home"))
            //{
            //    e.Item.Text = string.Empty;

            //    string imagePath = String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images", "/");

            //    e.Item.ImageUrl = imagePath + "home_24.png"; 
            //}

            string target = node["target"];

            if(!String.IsNullOrEmpty(target))
                e.Item.Target = target;

            //for Selected == added kuldeep 
            if (Request.QueryString.Get("pid") != null)
            {
                string pid = Request.QueryString.Get("pid").ToString();
                if(e.Item.DataPath == pid)
                {
                    e.Item.Selected = true;
                }
            }
        
        }
    }
}

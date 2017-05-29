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

namespace SolidCP.Portal
{
    public partial class CollapsiblePanel : SolidCPControlBase
    {
        public const string DEFAULT_EXPAND_IMAGE = "shevron_expand.gif";
        public const string DEFAULT_COLLAPSE_IMAGE = "shevron_collapse.gif";

        public string CssClass
        {
            get { return HeaderPanel.CssClass; }
            set { HeaderPanel.CssClass = value; }
        }

        private string _resourceKey;
        public string ResourceKey
        {
            get { return _resourceKey; }
            set { _resourceKey = value; }
        }

        bool _isCollapsed = false;
        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set { _isCollapsed = value; }
        }

        string _expandImage;
        public string ExpandImage
        {
            get { return _expandImage; }
            set { _expandImage = value; }
        }

        string _collapseImage;
        public string CollapseImage
        {
            get { return _collapseImage; }
            set { _collapseImage = value; }
        }

        public string TargetControlID
        {
            get { return cpe.TargetControlID; }
            set { cpe.TargetControlID = value; }
        }

        public string Text
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        protected void cpe_ResolveControlID(object sender, AjaxControlToolkit.ResolveControlEventArgs e)
        {
            e.Control = this.Parent.FindControl(e.ControlID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_collapseImage))
                _collapseImage = DEFAULT_COLLAPSE_IMAGE;

            if (String.IsNullOrEmpty(_expandImage))
                _expandImage = DEFAULT_EXPAND_IMAGE;

            // Initialize the ContentPanel to be either expanded or collapsed depending on the flag
			// Due to the fact that this control can loaded dynamically we need to setup images every time.
            cpe.Collapsed = _isCollapsed;
            cpe.CollapsedImage = GetThemedImage(_expandImage);
            cpe.ExpandedImage = GetThemedImage(_collapseImage);

            // get localized title
            if (!String.IsNullOrEmpty(ResourceKey))
            {
                SolidCPControlBase parentControl = this.Parent as SolidCPControlBase;
                if(parentControl != null)
                    lblTitle.Text = parentControl.GetLocalizedString(ResourceKey + ".Text");
            }

            ToggleImage.ImageUrl = GetThemedImage(_isCollapsed ? _expandImage : _collapseImage);
        }
    }
}

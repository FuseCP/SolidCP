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
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace SolidCP.Portal.UserControls
{
    public partial class DomainControl : SolidCPControlBase
    {
        public class DomainNameEventArgs : EventArgs
        {
            public string DomainName { get; set; }
        }

        public event EventHandler<DomainNameEventArgs> TextChanged;

        public virtual void OnTextChanged()
        {
            var handler = TextChanged;
            if (handler != null) handler(this, new DomainNameEventArgs {DomainName = Text});
        }

        public object DataSource
        {
            set { DomainsList.DataSource = value; }
        }

        public bool AutoPostBack
        {
            get { return txtDomainName.AutoPostBack; }
            set { txtDomainName.AutoPostBack = value; }
        }

        public Unit Width
        {
            get { return txtDomainName.Width; }
            set { txtDomainName.Width = value; }
        }

        public bool RequiredEnabled
        {
            get { return DomainRequiredValidator.Enabled; }
            set { DomainRequiredValidator.Enabled = value; }
        }

        public string Text
        {
            get
            {
                var domainName = txtDomainName.Text.Trim();
                if (!IsSubDomain) return domainName;

                if (string.IsNullOrEmpty(domainName))
                {
                    // Only return selected domain from DomainsList when no subdomain is entered yet
                    return DomainsList.SelectedValue;
                }

                return domainName + "." + DomainsList.SelectedValue;
            }
            set { txtDomainName.Text = value; }
        }

        public string ValidationGroup
        {
            get { return DomainRequiredValidator.ValidationGroup; }
            set { DomainRequiredValidator.ValidationGroup = value; DomainFormatValidator.ValidationGroup = value; }
        }

        public bool IsSubDomain {
            get { return SubDomainSeparator.Visible; }
            set
            {
                SubDomainSeparator.Visible = value;
                DomainsList.Visible = value;
                DomainRequiredValidator.Enabled = !value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected new void DataBind()
        {
            DomainsList.DataBind();
        }

        protected void DomainFormatValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var idn = new IdnMapping();
            try
            {
                var ascii = idn.GetAscii(Text);
                var regex = new Regex(@"^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,15}$");
                args.IsValid = regex.IsMatch(ascii);
            }
            catch (Exception)
            {
                args.IsValid = false;
            }
        }

        protected void txtDomainName_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged();
        }
    }
}

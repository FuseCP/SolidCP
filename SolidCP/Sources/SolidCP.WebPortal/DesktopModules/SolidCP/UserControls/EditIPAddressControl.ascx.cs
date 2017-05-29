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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolidCP.Portal.UserControls
{
	[Flags]
	public enum IPValidationMode { V4 = 1, V6 = 2, V4AndV6 = 3 };

    public partial class EditIPAddressControl : SolidCPControlBase
    {

		public IPValidationMode Validation { get; set; }

		public EditIPAddressControl() { Validation = IPValidationMode.V4AndV6; AllowSubnet = false; }

        public bool Required
        {
            get { return requireAddressValidator.Enabled; }
            set { requireAddressValidator.Enabled = value; }
        }

        public string ValidationGroup
        {
            get { return requireAddressValidator.ValidationGroup; }
            set
            {
                requireAddressValidator.ValidationGroup = value;
                addressValidator.ValidationGroup = value;
            }
        }

        public string CssClass
        {
            get { return txtAddress.CssClass; }
            set { txtAddress.CssClass = value; }
        }

        public Unit Width
        {
            get { return txtAddress.Width; }
            set { txtAddress.Width = value; }
        }

        public string Text
        {
            get { return txtAddress.Text.Trim(); }
            set { txtAddress.Text = value; }
        }

        public string RequiredErrorMessage
        {
            get { return requireAddressValidator.ErrorMessage; }
            set { requireAddressValidator.ErrorMessage = value; }
        }

        public string FormatErrorMessage
        {
            get { return addressValidator.ErrorMessage; }
            set { addressValidator.ErrorMessage = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

		public bool AllowSubnet { get; set; }
		public bool IsV6 { get; private set; }
		public bool IsMask { get; private set; }

		public void Validate(object source, ServerValidateEventArgs args) {
			IsMask = IsV6 = false;
			var ip = args.Value;
			int net = 0;
			if (ip.Contains("/")) {
				args.IsValid = AllowSubnet;
				var tokens = ip.Split('/');
				ip = tokens[0];
				args.IsValid &= int.TryParse(tokens[1], out net) && net <= 128;
				if (string.IsNullOrEmpty(ip)) {
					IsMask = true;
					return;
				}
			}
			System.Net.IPAddress ipaddr;
			args.IsValid &= System.Net.IPAddress.TryParse(ip, out ipaddr) && (ip.Contains(":") || ip.Contains(".")) &&
                (((Validation & IPValidationMode.V6) != 0 && (IsV6 = ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)) ||
				((Validation & IPValidationMode.V4) != 0 && ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
			args.IsValid &= ipaddr.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork || net < 32;
		}
    }
}

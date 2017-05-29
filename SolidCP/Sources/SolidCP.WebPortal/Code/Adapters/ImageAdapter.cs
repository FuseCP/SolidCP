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
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
	public class ImageAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
	{
		protected override void Render(HtmlTextWriter writer)
		{
			Image img = Control as Image;
			//
			if (img != null && Page != null)
			{
				//
				HttpBrowserCapabilities browser = Page.Request.Browser;
				//
				if (browser.Browser == "IE" &&
					(browser.Version == "5.0" || browser.Version == "5.5" || browser.Version == "6.0")
					&& !String.IsNullOrEmpty(img.ImageUrl) && img.ImageUrl.ToLower().EndsWith(".png"))
				{
					// add other attributes
					string imageUrl = img.ImageUrl; // save original URL

					// change original URL to empty
					img.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "SolidCP.WebPortal.Code.Adapters.empty.gif");

					imageUrl = img.ResolveClientUrl(imageUrl);
					img.Attributes["style"] =
						String.Format("filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}', sizingMethod='scale');",
						imageUrl);
				}
			}
			//
			base.Render(writer);
		}
	}
}

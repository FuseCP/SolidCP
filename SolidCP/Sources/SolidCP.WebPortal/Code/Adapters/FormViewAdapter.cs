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

// Material sourced from the bluePortal project (http://blueportal.codeplex.com).
// Licensed under the Microsoft Public License (available at http://www.opensource.org/licenses/ms-pl.html).

using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
    public class FormViewAdapter : CompositeDataBoundControlAdapter
    {
        protected override string HeaderText { get { return ControlAsFormView.HeaderText; } }
        protected override string FooterText { get { return ControlAsFormView.FooterText; } }
        protected override ITemplate HeaderTemplate { get { return ControlAsFormView.HeaderTemplate; } }
        protected override ITemplate FooterTemplate { get { return ControlAsFormView.FooterTemplate; } }
        protected override TableRow HeaderRow { get { return ControlAsFormView.HeaderRow; } }
        protected override TableRow FooterRow { get { return ControlAsFormView.FooterRow; } }
        protected override bool AllowPaging { get { return ControlAsFormView.AllowPaging; } }
        protected override int DataItemCount { get { return ControlAsFormView.DataItemCount; } }
        protected override int DataItemIndex { get { return ControlAsFormView.DataItemIndex; } }
        protected override PagerSettings PagerSettings { get { return ControlAsFormView.PagerSettings; } }

        public FormViewAdapter()
        {
            _classMain = "AspNet-FormView";
            _classHeader = "AspNet-FormView-Header";
            _classData = "AspNet-FormView-Data";
            _classFooter = "AspNet-FormView-Footer";
            _classPagination = "AspNet-FormView-Pagination";
            _classOtherPage = "AspNet-FormView-OtherPage";
            _classActivePage = "AspNet-FormView-ActivePage";
        }

        protected override void BuildItem(HtmlTextWriter writer)
        {
            if ((ControlAsFormView.Row != null) &&
                (ControlAsFormView.Row.Cells.Count > 0) &&
                (ControlAsFormView.Row.Cells[0].Controls.Count > 0))
            {
                writer.WriteLine();
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", _classData);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                writer.WriteLine();

                foreach (Control itemCtrl in ControlAsFormView.Row.Cells[0].Controls)
                {
                    itemCtrl.RenderControl(writer);
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("div");
            }
        }
    }
}

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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
    public class DetailsViewAdapter : CompositeDataBoundControlAdapter
    {
        protected override string HeaderText { get { return ControlAsDetailsView.HeaderText; } }
        protected override string FooterText { get { return ControlAsDetailsView.FooterText; } }
        protected override ITemplate HeaderTemplate { get { return ControlAsDetailsView.HeaderTemplate; } }
        protected override ITemplate FooterTemplate { get { return ControlAsDetailsView.FooterTemplate; } }
        protected override TableRow HeaderRow { get { return ControlAsDetailsView.HeaderRow; } }
        protected override TableRow FooterRow { get { return ControlAsDetailsView.FooterRow; } }
        protected override bool AllowPaging { get { return ControlAsDetailsView.AllowPaging; } }
        protected override int DataItemCount { get { return ControlAsDetailsView.DataItemCount; } }
        protected override int DataItemIndex { get { return ControlAsDetailsView.DataItemIndex; } }
        protected override PagerSettings PagerSettings { get { return ControlAsDetailsView.PagerSettings; } }

        public DetailsViewAdapter()
        {
            _classMain = "AspNet-DetailsView";
            _classHeader = "AspNet-DetailsView-Header";
            _classData = "AspNet-DetailsView-Data";
            _classFooter = "AspNet-DetailsView-Footer";
            _classPagination = "AspNet-DetailsView-Pagination";
            _classOtherPage = "AspNet-DetailsView-OtherPage";
            _classActivePage = "AspNet-DetailsView-ActivePage";
        }

        protected override void BuildItem(HtmlTextWriter writer)
        {
            if (IsDetailsView && (ControlAsDetailsView.Rows.Count > 0))
            {
                writer.WriteLine();
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", _classData);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                writer.WriteLine();
                writer.WriteBeginTag("ul");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                int countRenderedRows = 0;
                for (int iRow = 0; iRow < ControlAsDetailsView.Rows.Count; iRow++)
                {
                    if (ControlAsDetailsView.Fields[iRow].Visible)
                    {
                        DetailsViewRow row = ControlAsDetailsView.Rows[iRow];
                        if ((!ControlAsDetailsView.AutoGenerateRows) &&
                            ((row.RowState & DataControlRowState.Insert) == DataControlRowState.Insert) &&
                            (!ControlAsDetailsView.Fields[row.RowIndex].InsertVisible))
                        {
                            continue;
                        }

                        writer.WriteLine();
                        writer.WriteBeginTag("li");
                        string theClass = ((countRenderedRows % 2) == 1) ? "AspNet-DetailsView-Alternate" : "";
                        if ((ControlAsDetailsView.Fields[iRow].ItemStyle != null) && (!String.IsNullOrEmpty(ControlAsDetailsView.Fields[iRow].ItemStyle.CssClass)))
                        {
                            if (!String.IsNullOrEmpty(theClass))
                            {
                                theClass += " ";
                            }
                            theClass += ControlAsDetailsView.Fields[iRow].ItemStyle.CssClass;
                        }
                        if (!String.IsNullOrEmpty(theClass))
                        {
                            writer.WriteAttribute("class", theClass);
                        }
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();

                        for (int iCell = 0; iCell < row.Cells.Count; iCell++)
                        {
                            TableCell cell = row.Cells[iCell];
                            writer.WriteBeginTag("span");
                            if (iCell == 0)
                            {
                                writer.WriteAttribute("class", "AspNet-DetailsView-Name");
                            }
                            else if (iCell == 1)
                            {
                                writer.WriteAttribute("class", "AspNet-DetailsView-Value");
                            }
                            else
                            {
                                writer.WriteAttribute("class", "AspNet-DetailsView-Misc");
                            }
                            writer.Write(HtmlTextWriter.TagRightChar);
                            if (!String.IsNullOrEmpty(cell.Text))
                            {
                                writer.Write(cell.Text);
                            }
                            foreach (Control cellChildControl in cell.Controls)
                            {
                                cellChildControl.RenderControl(writer);
                            }
                            writer.WriteEndTag("span");
                        }

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("li");
                        countRenderedRows++;
                    }
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("ul");

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("div");
            }
        }
    }
}

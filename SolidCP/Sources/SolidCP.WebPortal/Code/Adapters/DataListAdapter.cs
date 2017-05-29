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
    public class DataListAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                    ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        private int RepeatColumns
        {
            get
            {
                DataList dataList = Control as DataList;
                int nRet = 1;
                if (dataList != null)
                {
                    if (dataList.RepeatColumns == 0)
                    {
                        if (dataList.RepeatDirection == RepeatDirection.Horizontal)
                        {
                            nRet = dataList.Items.Count;
                        }
                    }
                    else
                    {
                        nRet = dataList.RepeatColumns;
                    }
                }
                return nRet;
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PROTECTED        
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Extender.AdapterEnabled)
            {
                RegisterScripts();
            }
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "AspNet-DataList");
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderEndTag(writer);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                DataList dataList = Control as DataList;
                if (dataList != null)
                {
                    writer.Indent++;
                    writer.WriteLine();
                    writer.WriteBeginTag("table");
                    writer.WriteAttribute("cellpadding", "0");
                    writer.WriteAttribute("cellspacing", "0");
                    writer.WriteAttribute("summary", Control.ToolTip);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    if (dataList.HeaderTemplate != null)
                    {
                        PlaceHolder container = new PlaceHolder();
                        dataList.HeaderTemplate.InstantiateIn(container);
                        container.DataBind();

                        if ((container.Controls.Count == 1) && typeof(LiteralControl).IsInstanceOfType(container.Controls[0]))
                        {
                            writer.WriteLine();
                            writer.WriteBeginTag("caption");
                            writer.Write(HtmlTextWriter.TagRightChar);

                            LiteralControl literalControl = container.Controls[0] as LiteralControl;
                            writer.Write(literalControl.Text.Trim());

                            writer.WriteEndTag("caption");
                        }
                        else
                        {
                            writer.WriteLine();
                            writer.WriteBeginTag("thead");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            writer.WriteLine();
                            writer.WriteBeginTag("tr");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            writer.WriteLine();
                            writer.WriteBeginTag("th");
                            writer.WriteAttribute("colspan", RepeatColumns.ToString());
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            writer.WriteLine();
                            container.RenderControl(writer);

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("th");

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("tr");

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("thead");
                        }
                    }

                    if (dataList.FooterTemplate != null)
                    {
                        writer.WriteLine();
                        writer.WriteBeginTag("tfoot");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        writer.WriteLine();
                        writer.WriteBeginTag("tr");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        writer.WriteLine();
                        writer.WriteBeginTag("td");
                        writer.WriteAttribute("colspan", RepeatColumns.ToString());
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        PlaceHolder container = new PlaceHolder();
                        dataList.FooterTemplate.InstantiateIn(container);
                        container.DataBind();
                        container.RenderControl(writer);

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("td");

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("tr");

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("tfoot");
                    }

                    if (dataList.ItemTemplate != null)
                    {
                        writer.WriteLine();
                        writer.WriteBeginTag("tbody");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        int nItemsInColumn = (int)Math.Ceiling(((Double)dataList.Items.Count) / ((Double)RepeatColumns));
                        for (int iItem = 0; iItem < dataList.Items.Count; iItem++)
                        {
                            int nRow = iItem / RepeatColumns;
                            int nCol = iItem % RepeatColumns;
                            int nDesiredIndex = iItem;
                            if (dataList.RepeatDirection == RepeatDirection.Vertical)
                            {
                                nDesiredIndex = (nCol * nItemsInColumn) + nRow;
                            }

                            if ((iItem % RepeatColumns) == 0)
                            {
                                writer.WriteLine();
                                writer.WriteBeginTag("tr");
                                writer.Write(HtmlTextWriter.TagRightChar);
                                writer.Indent++;
                            }

                            writer.WriteLine();
                            writer.WriteBeginTag("td");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            foreach (Control itemCtrl in dataList.Items[iItem].Controls)
                            {
                                itemCtrl.RenderControl(writer);
                            }

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("td");

                            if (((iItem + 1) % RepeatColumns) == 0)
                            {
                                writer.Indent--;
                                writer.WriteLine();
                                writer.WriteEndTag("tr");
                            }
                        }

                        if ((dataList.Items.Count % RepeatColumns) != 0)
                        {
                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("tr");
                        }

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("tbody");
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("table");

                    writer.Indent--;
                    writer.WriteLine();
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PRIVATE        

        private void RegisterScripts()
        {
        }
    }
}

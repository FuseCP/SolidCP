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
    public class GridViewAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
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
                Extender.RenderBeginTag(writer, "AspNet-GridView");
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
                GridView gridView = Control as GridView;
                if (gridView != null)
                {
                    writer.Indent++;
                    WritePagerSection(writer, PagerPosition.Top);

                    writer.WriteLine();
                    writer.WriteBeginTag("table");
                    writer.WriteAttribute("id", gridView.ClientID);
                    writer.WriteAttribute("cellpadding", "0");
                    writer.WriteAttribute("cellspacing", "0");
                    writer.WriteAttribute("summary", Control.ToolTip);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    ArrayList rows = new ArrayList();
                    GridViewRowCollection gvrc = null;

                    ///////////////////// HEAD /////////////////////////////

                    rows.Clear();
                    if (gridView.ShowHeader && (gridView.HeaderRow != null))
                    {
                        rows.Add(gridView.HeaderRow);
                    }
                    gvrc = new GridViewRowCollection(rows);
                    WriteRows(writer, gridView, gvrc, "thead");

                    ///////////////////// FOOT /////////////////////////////

                    rows.Clear();
                    if (gridView.ShowFooter && (gridView.FooterRow != null))
                    {
                        rows.Add(gridView.FooterRow);
                    }
                    gvrc = new GridViewRowCollection(rows);
                    WriteRows(writer, gridView, gvrc, "tfoot");

                    ///////////////////// BODY /////////////////////////////

                    WriteRows(writer, gridView, gridView.Rows, "tbody");

                    ////////////////////////////////////////////////////////

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("table");

					WriteEmptyTextSection(writer);
                    WritePagerSection(writer, PagerPosition.Bottom);

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

        private void WriteRows(HtmlTextWriter writer, GridView gridView, GridViewRowCollection rows, string tableSection)
        {
            if (rows.Count > 0)
            {
                writer.WriteLine();
                writer.WriteBeginTag(tableSection);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                foreach (GridViewRow row in rows)
                {
                    writer.WriteLine();
                    writer.WriteBeginTag("tr");

                    string className = GetRowClass(gridView, row);
                    if (!String.IsNullOrEmpty(className))
                    {
                        writer.WriteAttribute("class", className);
                    }
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    int i = 0;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (!gridView.Columns[i++].Visible)
                            continue;

                        DataControlFieldCell fieldCell = cell as DataControlFieldCell;

                        if(tableSection == "tbody" && fieldCell != null
                            && (fieldCell.Text.Trim() == "") && fieldCell.Controls.Count == 0)
                            cell.Controls.Add(new LiteralControl("<div style=\"width:1px;height:1px;\">&nbsp;</div>"));
                        else if (tableSection == "thead" && fieldCell != null
                            && !String.IsNullOrEmpty(gridView.SortExpression)
                            && fieldCell.ContainingField.SortExpression == gridView.SortExpression)
                        {
                            cell.Attributes["class"] = "AspNet-GridView-Sort";
                        }

                        if ((fieldCell != null) && (fieldCell.ContainingField != null))
                        {
                            DataControlField field = fieldCell.ContainingField;
                            if (!field.Visible)
                            {
                                cell.Visible = false;
                            }

							if (field.ItemStyle.Width != Unit.Empty)
								cell.Style["width"] = field.ItemStyle.Width.ToString();

							if (!field.ItemStyle.Wrap)
								cell.Style["white-space"] = "nowrap";

                            if ((field.ItemStyle != null) && (!String.IsNullOrEmpty(field.ItemStyle.CssClass)))
                            {
                                if (!String.IsNullOrEmpty(cell.CssClass))
                                {
                                    cell.CssClass += " ";
                                }
                                cell.CssClass += field.ItemStyle.CssClass;
                            }
                        }
                        
                        writer.WriteLine();
                        cell.RenderControl(writer);
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("tr");
                }
                
                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag(tableSection);
            }
        }

        private string GetRowClass(GridView gridView, GridViewRow row)
        {
            string className = "";

            if ((row.RowState & DataControlRowState.Alternate) == DataControlRowState.Alternate)
            {
                className += " AspNet-GridView-Alternate ";
                if (gridView.AlternatingRowStyle != null)
                {
                    className += gridView.AlternatingRowStyle.CssClass;
                }
            }

            if ((row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                className += " AspNet-GridView-Edit ";
                if (gridView.EditRowStyle != null)
                {
                    className += gridView.EditRowStyle.CssClass;
                }
            }

            if ((row.RowState & DataControlRowState.Insert) == DataControlRowState.Insert)
            {
                className += " AspNet-GridView-Insert ";
            }

            if ((row.RowState & DataControlRowState.Selected) == DataControlRowState.Selected)
            {
                className += " AspNet-GridView-Selected ";
                if (gridView.SelectedRowStyle != null)
                {
                    className += gridView.SelectedRowStyle.CssClass;
                }
            }

            return className.Trim();
        }

		private void WriteEmptyTextSection(HtmlTextWriter writer)
		{
            GridView gridView = Control as GridView;
			if (gridView != null && gridView.Rows.Count == 0)
			{
				string className = "AspNet-GridView-Empty";

				writer.WriteLine();
				writer.WriteBeginTag("div");
				writer.WriteAttribute("class", className);
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Indent++;

				writer.Write(gridView.EmptyDataText);

				writer.Indent--;
				writer.WriteLine();
				writer.WriteEndTag("div");
			}
		}

        private void WritePagerSection(HtmlTextWriter writer, PagerPosition pos)
        {
            GridView gridView = Control as GridView;
            if ((gridView != null) &&
                gridView.AllowPaging &&
                ((gridView.PagerSettings.Position == pos) || (gridView.PagerSettings.Position == PagerPosition.TopAndBottom)))
            {
                Table innerTable = null;
                if ((pos == PagerPosition.Top) &&
                    (gridView.TopPagerRow != null) &&
                    (gridView.TopPagerRow.Cells.Count == 1) &&
                    (gridView.TopPagerRow.Cells[0].Controls.Count == 1) &&
                    typeof(Table).IsAssignableFrom(gridView.TopPagerRow.Cells[0].Controls[0].GetType()))
                {
                    innerTable = gridView.TopPagerRow.Cells[0].Controls[0] as Table;
                }
                else if ((pos == PagerPosition.Bottom) &&
                    (gridView.BottomPagerRow != null) &&
                    (gridView.BottomPagerRow.Cells.Count == 1) &&
                    (gridView.BottomPagerRow.Cells[0].Controls.Count == 1) &&
                    typeof(Table).IsAssignableFrom(gridView.BottomPagerRow.Cells[0].Controls[0].GetType()))
                {
                    innerTable = gridView.BottomPagerRow.Cells[0].Controls[0] as Table;
                }

                if ((innerTable != null) && (innerTable.Rows.Count == 1))
                {
                    string className = "AspNet-GridView-Pagination AspNet-GridView-";
                    className += (pos == PagerPosition.Top) ? "Top " : "Bottom ";
                    if (gridView.PagerStyle != null)
                    {
                        className += gridView.PagerStyle.CssClass;
                    }
                    className = className.Trim();

                    writer.WriteLine();
                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", className);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    TableRow row = innerTable.Rows[0];
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            writer.WriteLine();
                            ctrl.RenderControl(writer);
                        }
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("div");
                }
            }
        }
    }
}

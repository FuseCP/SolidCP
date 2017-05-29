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

using SolidCP.Portal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SolidCP.Portal.SkinControls
{
    [ParseChildren(true, "Items")]
    [PersistChildren(false)]
    [ValidationProperty("SelectedValue")]
    public partial class BootstrapDropDownList : UserControl
    {
        private ListItemCollection items = new ListItemCollection();

        public object DataSource;

        private bool initialized;

        protected HtmlGenericControl ddl;

        protected HtmlInputHidden hdSelectedIndex;

        protected HtmlButton btn;

        protected HtmlGenericControl lit;

        protected HtmlInputHidden hdSelectedValue;

        public bool AutoPostBack
        {
            get;
            set;
        }

        public string ControlID
        {
            get
            {
                return this.hdSelectedValue.ClientID;
            }
        }

        public string CssClass { get; set; }

        public string DataTextField
        {
            get;
            set;
        }

        public string DataValueField
        {
            get;
            set;
        }

        public bool Enabled
        {
            get
            {
                object item = this.ViewState["enabled"];
                if (item == null)
                {
                    return true;
                }
                return (bool)item;
            }
            set
            {
                this.ViewState["enabled"] = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ListItemCollection Items
        {
            get
            {
                return this.items;
            }
        }

        public string OnClientClick { get; set; } 

        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        return i;
                    }
                }
                return 0;
            }
            set
            {
                this.SetSelectedIndex(value);
                if (this.SelectedIndexChanged != null)
                {
                    this.SelectedIndexChanged(this, new EventArgs());
                }
            }
        }

        public ListItem SelectedItem
        {
            get
            {
                int selectedIndex = this.SelectedIndex;
                if (selectedIndex < 0 || selectedIndex >= this.Items.Count)
                {
                    return null;
                }
                return this.Items[selectedIndex];
            }
        }

        public string SelectedValue
        {
            get
            {
                ListItem selectedItem = this.SelectedItem;
                if (selectedItem != null)
                {
                    return selectedItem.Value;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Value == value)
                    {
                        this.SelectedIndex = i;
                    }
                }
            }
        }

        public string Text
        {
            get
            {
                return this.SelectedValue;
            }
            set
            {
                this.SelectedValue = value;
            }
        }

        public BootstrapDropDownList()
        {
        }

        private void BindItems()
        {
            this.lit.InnerText = "...";
            if (this.Enabled)
            {
                HtmlGenericControl htmlGenericControl = new HtmlGenericControl("ul");
                htmlGenericControl.Attributes.Add("class", "dropdown-menu");
                htmlGenericControl.Attributes.Add("aria-labelledby", this.btn.ClientID);
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ListItem item = this.Items[i];
                    HtmlGenericControl htmlGenericControl1 = new HtmlGenericControl("li");
                    HtmlGenericControl text = new HtmlGenericControl("a");
                    htmlGenericControl1.Attributes.Add("value", item.Value);
                    text.InnerText = item.Text;
                    text.Attributes.Add("href", (this.AutoPostBack ? string.Format("javascript:__doPostBack(\"{0}\",\"\");", this.ddl.ClientID) : "javascript:void(0);"));
                    text.Attributes.Add("onclick", string.Concat("BootstrapDropDownListSelect(this); ", this.OnClientClick));
                    if (i == this.SelectedIndex)
                    {
                        this.lit.InnerText = item.Text;
                        htmlGenericControl1.Attributes.Add("class", "active");
                    }
                    htmlGenericControl1.Controls.Add(text);
                    htmlGenericControl.Controls.Add(htmlGenericControl1);
                }
                this.ddl.Controls.Add(htmlGenericControl);
            }
            else if (this.SelectedItem != null)
            {
                this.lit.InnerText = this.SelectedItem.Text;
            }
            this.ddl.Attributes.Add("class", string.Concat("dropdown", (string.IsNullOrEmpty(this.CssClass) ? "" : string.Concat(" ", this.CssClass))));
            this.btn.Attributes.Add("class", string.Concat("btn btn-default", (this.Enabled ? " dropdown-toggle" : "")));
            if (this.Enabled)
            {
                this.btn.Attributes.Remove("disabled");
                return;
            }
            this.btn.Attributes.Add("disabled", "");
        }

        public void ClearSelection()
        {
            this.SelectedIndex = -1;
        }

        public override void DataBind()
        {
            object value;
            object obj;
            if (this.DataSource != null && !string.IsNullOrEmpty(this.DataValueField) && !string.IsNullOrEmpty(this.DataTextField))
            {
                IEnumerable dataSource = this.DataSource as IEnumerable;
                if (dataSource != null)
                {
                    if (base.IsPostBack)
                    {
                        this.Items.Clear();
                    }
                    foreach (object obj1 in dataSource)
                    {
                        Type type = obj1.GetType();
                        BindingFlags bindingFlag = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
                        PropertyInfo property = type.GetProperty(this.DataValueField, bindingFlag);
                        if (property != null)
                        {
                            value = property.GetValue(obj1, null);
                        }
                        else
                        {
                            value = null;
                        }
                        object obj2 = value;
                        PropertyInfo propertyInfo = type.GetProperty(this.DataTextField, bindingFlag);
                        if (propertyInfo != null)
                        {
                            obj = propertyInfo.GetValue(obj1, null);
                        }
                        else
                        {
                            obj = null;
                        }
                        object obj3 = obj;
                        if (obj2 == null || obj3 == null)
                        {
                            continue;
                        }
                        this.Items.Add(new ListItem(obj3.ToString(), obj2.ToString()));
                    }
                }
                DataTable item = null;
                if (this.DataSource is DataSet)
                {
                    item = ((DataSet)this.DataSource).Tables[0];
                }
                if (this.DataSource is DataTable)
                {
                    item = this.DataSource as DataTable;
                }
                if (this.DataSource is DataView)
                {
                    item = ((DataView)this.DataSource).ToTable();
                }
                if (item != null)
                {
                    this.items.Clear();
                    foreach (DataRow row in item.Rows)
                    {
                        this.Items.Add(new ListItem(row[this.DataTextField].ToString(), row[this.DataValueField].ToString()));
                    }
                }
            }
            base.DataBind();
        }

        public string GetSelectedValueFromRequest()
        {
            return base.Request.Form[this.hdSelectedValue.Name];
        }

        protected void Initialization()
        {
            if (this.initialized)
            {
                return;
            }
            if (base.IsPostBack)
            {
                string value = this.hdSelectedIndex.Value;
                if (string.IsNullOrEmpty(value))
                {
                    value = base.Request.Form[this.hdSelectedIndex.Name];
                }
                int num = 0;
                if (int.TryParse(value, out num))
                {
                    this.SetSelectedIndex(num);
                }
            }
            this.initialized = true;
        }

        protected override void LoadViewState(object savedState)
        {
            object[] objArray = savedState as object[];
            if (objArray == null)
            {
                return;
            }
            if ((int)objArray.Length != 3)
            {
                return;
            }
            base.LoadViewState(objArray[0]);

            List<BootstrapDropDownListItem> bootstrapDropDownListItems = objArray[1] as List<BootstrapDropDownListItem>;
            if (bootstrapDropDownListItems != null && bootstrapDropDownListItems.Count > 0)
            {
                this.items.Clear();
                foreach (BootstrapDropDownListItem bootstrapDropDownListItem in bootstrapDropDownListItems)
                {
                    this.items.Add(new ListItem(bootstrapDropDownListItem.Text, bootstrapDropDownListItem.Value));
                }
            }
            if (objArray[2] is int)
            {
                this.SetSelectedIndex((int)objArray[2]);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.initialized = false;
            this.Page.PreLoad += new EventHandler(this.Page_PreLoad);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialization();
            ClientScriptManager clientScript = this.Page.ClientScript;
            clientScript.RegisterClientScriptInclude("jquery", base.ResolveUrl("~/JavaScript/jquery-1.12.3.min.js"));
            clientScript.RegisterClientScriptInclude("bootstrap", base.ResolveUrl("~/JavaScript/bootstrap/bootstrap.min.js"));
            clientScript.RegisterClientScriptBlock(base.GetType(), "BootstrapDropDownList", 
                "<script type=\"text/javascript\">\nfunction BootstrapDropDownListSelect(link)\n{\n    a = $(link); li = a.parent(); ul = li.parent(); ddl = ul.parent();\n    hd = ddl.children().eq(0); btn = ddl.children().eq(1); \n    lit = btn.children().eq(0); hdval = ddl.children().eq(2);\n    index = li.index();\n    itemvalue = li.attr('value');\n    lit.html(a.html());\n    ul.children().removeClass('active');\n    li.addClass('active');\n    hd.val(index);\n    hdval.val(itemvalue);\n}\n</script>");
            if (base.IsPostBack && this.SelectedIndexChanged != null && this.ViewState["index"] != null && (int)this.ViewState["index"] != this.SelectedIndex)
            {
                this.SelectedIndexChanged(this, new EventArgs());
            }
        }

        private void Page_PreLoad(object sender, EventArgs e)
        {
            this.Initialization();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SolidCPControlBase solidCPControlBase;
            HtmlInputHidden str;
            HtmlInputHidden htmlInputHidden;
            string str1;
            string item = base.Attributes["resourcekey"];
            if (!string.IsNullOrEmpty(item))
            {
                Control parent = this;
                do
                {
                    parent = parent.Parent;
                    if (parent == null)
                    {
                        this.BindItems();
                        str = this.hdSelectedIndex;
                        str.Value = this.SelectedIndex.ToString();
                        htmlInputHidden = this.hdSelectedValue;
                        str1 = (this.SelectedValue == null ? "" : this.SelectedValue);
                        htmlInputHidden.Value = str1;
                        this.ViewState["index"] = this.SelectedIndex;
                        return;
                    }
                    solidCPControlBase = parent as SolidCPControlBase;
                }
                while (solidCPControlBase == null);
                solidCPControlBase.LocalizeListItems(item, this.Items);
            }
            this.BindItems();
            str = this.hdSelectedIndex;
            str.Value = this.SelectedIndex.ToString();
            htmlInputHidden = this.hdSelectedValue;
            str1 = (this.SelectedValue == null ? "" : this.SelectedValue);
            htmlInputHidden.Value = str1;
            this.ViewState["index"] = this.SelectedIndex;
        }

        protected override object SaveViewState()
        {
            object[] selectedIndex = new object[] { base.SaveViewState(), null, null };
            List<BootstrapDropDownListItem> bootstrapDropDownListItems = new List<BootstrapDropDownListItem>();
            foreach (ListItem item in this.Items)
            {
                bootstrapDropDownListItems.Add(new BootstrapDropDownListItem()
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }
            selectedIndex[1] = bootstrapDropDownListItems;
            selectedIndex[2] = this.SelectedIndex;
            return selectedIndex;
        }

        public void SetSelectedIndex(int value)
        {
            if (value >= this.Items.Count)
            {
                return;
            }
            foreach (object item in this.Items)
            {
                ((ListItem)item).Selected = false;
            }
            if (value >= 0)
            {
                this.Items[value].Selected = true;
            }
        }

        public event EventHandler SelectedIndexChanged;
    }
}
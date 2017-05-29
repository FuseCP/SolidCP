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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;


namespace SolidCP.Portal.UserControls
{
    public abstract class DomainListControlBase : SolidCPControlBase
    {
        # region Properties

        protected abstract Button AddButton { get; }
        protected abstract GridView Grid { get; }

        public Boolean DisplayNames
        {
            get { return Grid.Columns[ 0 ].Visible; }
            set { Grid.Columns[ 0 ].Visible = value; }
        }


        public String Value
        {
            get
            {
            var items = CollectFormData( false );
            return String.Join( ";", items.ToArray() );
            }

            set
            {
            var items = new List<String>();
            if ( !String.IsNullOrEmpty( value ) )
                {
                var parts = value.Split( ';' );
                items.AddRange( from part in parts where part.Trim() != "" select part.Trim() );
                }

            // save items
            _loaded_items = items.ToArray();

            if ( IsPostBack )
                {
                BindItems( _loaded_items );
                }
            }
        }

        # endregion


        protected void Page_Load( Object sender, EventArgs e )
        {
            if ( !IsPostBack )
                {
                BindItems( _loaded_items ); // empty list
                }
        }


        private void BindItems( IEnumerable items )
        {
            Grid.DataSource = items;
            Grid.DataBind();
        }


        public List<String> CollectFormData( Boolean include_empty )
        {
            var items = new List<String>();
            foreach ( GridViewRow row in Grid.Rows )
                {
                var txt_name = (TextBox)row.FindControl( _txt_control_name );
                var val = txt_name.Text.Trim();

                if ( include_empty || "" != val )
                    {
                    items.Add( val );
                    }
                }

            return items;
        }


        # region Events

        protected void BtnAddClick( Object sender, EventArgs e)
        {
            var items = CollectFormData( true );

            // add empty string
            items.Add( "" );

            // bind items
            BindItems( items.ToArray() );
        }


        protected void ListRowCommand( Object sender, GridViewCommandEventArgs e )
        {
            if ( "delete_item" != e.CommandName )
                {
                return;
                }

            var items = CollectFormData(true);

            // remove error
            items.RemoveAt( Utils.ParseInt( e.CommandArgument, 0 ) );

            // bind items
            BindItems(items.ToArray());
        }


        protected void ListRowDataBound( Object sender, GridViewRowEventArgs e )
        {
            var lbl_name = (Label)e.Row.FindControl( _lbl_control_name );
            var txt_name = (TextBox)e.Row.FindControl( _txt_control_name );
            var cmd_delete = (CPCC.StyleButton)e.Row.FindControl( _delete_control_name );

            if ( null == lbl_name )
                {
                return;
                }

            var val = (String)e.Row.DataItem;
            txt_name.Text = val;

            var pos = ( e.Row.RowIndex < 2 ) 
                ? e.Row.RowIndex.ToString( CultureInfo.InvariantCulture ) 
                : "";
            lbl_name.Text = GetLocalizedString( "Item" + pos + ".Text" );

            cmd_delete.CommandArgument = e.Row.RowIndex.ToString( CultureInfo.InvariantCulture );
        }

        # endregion


        # region Fields

        protected String[] _loaded_items = new String[] {};
        
        protected String _txt_control_name;
        protected String _lbl_control_name;
        protected String _delete_control_name;

        # endregion
    }
}

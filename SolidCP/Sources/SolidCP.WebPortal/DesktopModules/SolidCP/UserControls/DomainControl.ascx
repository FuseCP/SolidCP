<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainControl.ascx.cs" Inherits="SolidCP.Portal.UserControls.DomainControl" %>

<div class="form-group">
     <asp:Label ID="lblAddress" runat="server" Text="Domain Name" CssClass="col-sm-2"  AssociatedControlID="txtDomainName"></asp:Label>
     <div class="col-sm-10 form-inline">
         <asp:TextBox ID="txtDomainName" runat="server" CssClass="form-control" OnTextChanged="txtDomainName_TextChanged"></asp:TextBox>
     <asp:Literal runat="server" ID="SubDomainSeparator" Visible="False">.</asp:Literal>
<asp:DropDownList ID="DomainsList" Runat="server" CssClass="form-control" DataTextField="DomainName" DataValueField="DomainName" Visible="False"></asp:DropDownList>
     </div>
 </div>
<asp:RequiredFieldValidator id="DomainRequiredValidator" runat="server" meta:resourcekey="DomainRequiredValidator"
    ControlToValidate="txtDomainName" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
<asp:CustomValidator id="DomainFormatValidator" runat="server" meta:resourcekey="DomainFormatValidator" EnableClientScript="False" ValidateEmptyText="False"
	ControlToValidate="txtDomainName" Display="Dynamic" SetFocusOnError="true" OnServerValidate="DomainFormatValidator_ServerValidate"></asp:CustomValidator>


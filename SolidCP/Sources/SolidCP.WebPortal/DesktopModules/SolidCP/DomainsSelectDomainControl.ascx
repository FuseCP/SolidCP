<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainsSelectDomainControl.ascx.cs" Inherits="SolidCP.Portal.DomainsSelectDomainControl" %>


<asp:DropDownList id="ddlDomains" runat="server" CssClass="form-control" DataTextField="DomainName" DataValueField="DomainID" ></asp:DropDownList>
<asp:RequiredFieldValidator id="valRequireDomain" runat="server" ErrorMessage="Select domain"
	ControlToValidate="ddlDomains" Display="Dynamic" meta:resourcekey="valRequireDomain"></asp:RequiredFieldValidator>


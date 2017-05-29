<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailEditAddress.ascx.cs" Inherits="SolidCP.Portal.MailEditAddress" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="dnc" TagName="SelectDomain" Src="DomainsSelectDomainControl.ascx" %>

<div class="form-group">
    <asp:Label ID="lblEmailAddress" runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtName">
        <asp:Localize ID="locAccount" runat="server" meta:resourcekey="lblEmailAddress" Text="E-mail Address: *" />
    </asp:Label>
    <div id="EditEmailPanel" runat="server" class="col-sm-8">
        <div class="input-group">
            <span class="input-group-addon"><i class="fa fa-envelope-o" aria-hidden="true"></i></span>
            <uc2:UsernameControl ID="txtName" runat="server" />
            <span class="input-group-addon">
                <asp:Literal ID="litAt" runat="server" Text="@" /></span>
            <dnc:SelectDomain ID="domainsSelectDomainControl" runat="server" HideDomainPointers="true" HideInstantAlias="false" HideMailDomainPointers="true" HideIdnDomains="True"></dnc:SelectDomain>
        </div>
    </div>
    <div id="DisplayEmailPanel" runat="server" class="col-sm-8">
        <h5><strong><asp:Label ID="litName" runat="server" Visible="False"></asp:Label></strong></h5>
    </div>
</div>



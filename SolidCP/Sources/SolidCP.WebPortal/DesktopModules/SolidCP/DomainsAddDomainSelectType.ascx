<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainsAddDomainSelectType.ascx.cs" Inherits="SolidCP.Portal.DomainsAddDomainSelectType" EnableViewState="false" %>

<div class="panel-body form-horizontal">

    <p>
        <asp:Localize ID="IntroPar" runat="server" meta:resourcekey="IntroPar" />
    </p>
    <div class="row form-group">
         <div class="col-sm-2"><asp:HyperLink ID="DomainLink" CssClass="btn btn-primary" runat="server"><i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="DomainLink"/></asp:HyperLink></div>
         <div class="col-sm-10"><asp:Localize ID="DomainDescription" runat="server" meta:resourcekey="DomainDescription" /></div>
     </div>
     <div class="row form-group">
         <div class="col-sm-2"><asp:HyperLink ID="SubDomainLink" CssClass="btn btn-primary" runat="server"><i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="SubDomainLink"/></asp:HyperLink></div>
         <div class="col-sm-10"><asp:Localize ID="SubDomainDescription" runat="server" meta:resourcekey="SubDomainDescription" /></div>
     </div>
    <p id="ProviderSubDomainPanel" runat="server">
        <b><asp:HyperLink ID="ProviderSubDomainLink" runat="server" meta:resourcekey="ProviderSubDomainLink">Provider Sub-domain</asp:HyperLink></b><br />
        <asp:Localize ID="ProviderSubDomainDescription" runat="server" meta:resourcekey="ProviderSubDomainDescription" /><br /><br />
    </p>
<!--    
    <p>
        <b><asp:HyperLink ID="DomainPointerLink" runat="server" meta:resourcekey="DomainPointerLink">Domain Alias</asp:HyperLink></b><br />
        <asp:Localize ID="DomainPointerDescription" runat="server" meta:resourcekey="DomainPointerDescription" />
    </p>
-->
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
</div>
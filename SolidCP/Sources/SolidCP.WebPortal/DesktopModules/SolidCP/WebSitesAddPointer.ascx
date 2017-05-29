<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesAddPointer.ascx.cs" Inherits="SolidCP.Portal.WebSitesAddPointer" %>
<%@ Register Src="DomainsSelectDomainControl.ascx" TagName="DomainsSelectDomainControl" TagPrefix="uc1" %>
<div class="panel-body form-horizontal">
<div class="form-group">
    <asp:Label ID="lblDomainName" runat="server" meta:resourcekey="lblDomainName" Text="Domain Name:" CssClass="col-sm-2"  AssociatedControlID="txtHostName"></asp:Label>
     <div class="col-sm-10 form-inline">
			<asp:TextBox ID="txtHostName" runat="server" CssClass="form-control" MaxLength="64"></asp:TextBox>
            <asp:Label ID="lblTheDotInTheMiddle" runat="server" meta:resourcekey="lblTheDotInTheMiddle" Text=" . "></asp:Label>
            <uc1:DomainsSelectDomainControl ID="domainsSelectDomainControl" runat="server" CssClass="form-control" HideWebSites="false" HideDomainPointers="true" HideInstantAlias="true"/>
            <asp:RegularExpressionValidator ID="valRequireCorrectHostName" runat="server"
	                ErrorMessage="Enter valid hostname" ControlToValidate="txtHostName" Display="Dynamic"
	                meta:resourcekey="valRequireCorrectHostName" ValidationExpression="^([0-9a-zæöøåüA-ZÆÖØÅÜ])*[0-9a-zæöøåüA-ZÆÖØÅÜ]+$" SetFocusOnError="True"></asp:RegularExpressionValidator>
</div>
</div>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAdd"/> </CPCC:StyleButton>
</div>
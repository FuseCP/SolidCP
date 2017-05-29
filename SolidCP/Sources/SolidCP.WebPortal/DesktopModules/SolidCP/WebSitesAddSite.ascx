<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesAddSite.ascx.cs" Inherits="SolidCP.Portal.WebSitesAddSite" %>
<%@ Register Src="DomainsSelectDomainControl.ascx" TagName="DomainsSelectDomainControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">
<table cellSpacing="0" cellPadding="4">
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblDomainName" runat="server" meta:resourcekey="lblDomainName" Text="Domain name:"></asp:Label>
		</td>
		<td>
            <div class="form-inline">
			<asp:TextBox ID="txtHostName" runat="server" CssClass="form-control" MaxLength="64" Text="www"></asp:TextBox><asp:Label ID="lblTheDotInTheMiddle" runat="server" meta:resourcekey="lblTheDotInTheMiddle" Text=" . "></asp:Label><uc1:DomainsSelectDomainControl ID="domainsSelectDomainControl" runat="server" HideWebSites="false" HideDomainPointers="true" HideInstantAlias="true"/>
            <asp:RequiredFieldValidator ID="valRequireHostName" runat="server" meta:resourcekey="valRequireHostName" ControlToValidate="txtHostName"
	            ErrorMessage="Enter hostname" ValidationGroup="CreateSite" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valRequireCorrectHostName" runat="server"
	                ErrorMessage="Enter valid hostname" ControlToValidate="txtHostName" Display="Dynamic"
	                meta:resourcekey="valRequireCorrectHostName" ValidationExpression="^([0-9a-zæöøåüA-ZÆÖØÅÜ])*[0-9a-zæöøåüA-ZÆÖØÅÜ]+$" SetFocusOnError="True"></asp:RegularExpressionValidator>
                </div>
		</td>
	</tr>
	<tr>
        <td class="Normal" nowrap rowspan="2"></td>
        <td class="Normal">
            <asp:CheckBox ID="chkIgnoreGlobalDNSRecords" runat="server" meta:resourcekey="chkIgnoreGlobalDNSRecords"
                Text="Include Zone Template" Checked="True" />
        </td>
	</tr>
	<tr>
        <td class="Normal">
  			<div class="Small WrapText" style="padding-top: 10px;">
			    <asp:Label ID="lblIgnoreGlobalDNSRecords" runat="server" meta:resourcekey="lblIPHelp2" Text="If you need your site..."></asp:Label>
			</div>
        </td>
	</tr>

	<tr>
		<td>
		    <br/>
		</td>
	</tr>
	<tr id="rowSiteIP" runat="server">
		<td class="SubHead" vAlign="top">
		    <asp:Label ID="lblIPAddress" runat="server" meta:resourcekey="lblIPAddress" Text="IP address:"></asp:Label>
		</td>
		<td class="Normal">
			<table cellpadding="3">
				<tr>
					<td><asp:RadioButton ID="rbSharedIP" Runat="server" meta:resourcekey="rbSharedIP" Text="Shared (recommended)" AutoPostBack="True" CssClass="Normal"
							GroupName="IP" Checked="True" OnCheckedChanged="rbIP_CheckedChanged"></asp:RadioButton></td>
				</tr>
				<tr>
					<td><asp:RadioButton ID="rbDedicatedIP" Runat="server" meta:resourcekey="rbDedicatedIP" Text="Dedicated" AutoPostBack="True" CssClass="Normal"
							GroupName="IP" OnCheckedChanged="rbIP_CheckedChanged"></asp:RadioButton></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr id="rowDedicatedIP" runat="server">
		<td></td>
		<td><asp:dropdownlist id="ddlIpAddresses" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist>
			<asp:RequiredFieldValidator id="valRequireIP" runat="server" meta:resourcekey="valRequireIP" CssClass="NormalBold" ErrorMessage="Please select web site IP address"
				Display="Dynamic" ControlToValidate="ddlIpAddresses"></asp:RequiredFieldValidator><br/>
			<div class="Small" style="padding-top: 10px;">
			    <asp:Label ID="lblIPHelp" runat="server" meta:resourcekey="lblIPHelp" Text="If you need your site..."></asp:Label>
			</div>
		</td>
	</tr>
</table>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Creating web site...');"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
</div>
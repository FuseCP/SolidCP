<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersAddService.ascx.cs" Inherits="SolidCP.Portal.ServersAddService" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">
    <uc1:ServerHeaderControl id="ServerHeaderControl" runat="server">
    </uc1:ServerHeaderControl>
    <br />
    <table cellSpacing="0" cellPadding="5" width="100%">
	    <tr>
		    <td class="SubHead" noWrap width="200"><asp:Label ID="lblServiceGroupName" runat="server" meta:resourcekey="lblServiceGroupName" Text="Service group name:"></asp:Label></td>
		    <td class="NormalBold" width="100%">
			    <asp:Literal ID="litGroupName" Runat="server"></asp:Literal>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead" style="height: 34px"><asp:Label ID="lblServiceName" runat="server" meta:resourcekey="lblServiceName" Text="Service name:"></asp:Label></td>
		    <td class="NormalBold" style="height: 34px;">
			    <asp:TextBox id="serviceName" runat="server" CssClass="form-control" style="width: 300px; height: 30px;"></asp:TextBox>
			    <asp:RequiredFieldValidator id="serviceNameValidator" meta:resourcekey="serviceNameValidator" runat="server" ErrorMessage="Please specify service name" ControlToValidate="serviceName"
				    Display="Dynamic"></asp:RequiredFieldValidator>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead" valign="top">
			    <asp:Label ID="lblServiceProvider" runat="server" meta:resourcekey="lblServiceProvider" Text="Service provider:"></asp:Label></td>
		    <td class="NormalBold" valign="top">
			    <asp:DropDownList id="ddlProviders" CssClass="NormalTextBox" runat="server" DataTextField="DisplayName"
				    DataValueField="ProviderID"></asp:DropDownList>
			    <asp:RequiredFieldValidator id="serviceValidator" meta:resourcekey="serviceValidator" runat="server" Display="Dynamic" ControlToValidate="ddlProviders"
				    ErrorMessage="Please select service provider"></asp:RequiredFieldValidator></td>
	    </tr>
    </table>
</div>
<div class="panel-footer text-right">    
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Adding Service...');"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
</div>
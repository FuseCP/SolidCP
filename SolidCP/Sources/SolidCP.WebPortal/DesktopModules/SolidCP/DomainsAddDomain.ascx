<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainsAddDomain.ascx.cs" Inherits="SolidCP.Portal.DomainsAddDomain" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/DomainControl.ascx" TagName="DomainControl" TagPrefix="scp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagPrefix="scp" TagName="CollapsiblePanel" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<asp:ValidationSummary ID="summary" runat="server" ShowMessageBox="true" ShowSummary="true" ValidationGroup="Domain" />

<div id="DomainPanel" runat="server" style="padding: 15px 0 15px 5px;">
        <scp:DomainControl ID="DomainName" runat="server" RequiredEnabled="True" ValidationGroup="Domain"></scp:DomainControl>
</div>
<div class="panel-body">
    <scp:CollapsiblePanel id="OptionsPanelHeader" runat="server"
        TargetControlID="OptionsPanel" resourcekey="OptionsPanelHeader" Text="Provisioning options">
    </scp:CollapsiblePanel>
    <asp:Panel ID="OptionsPanel" runat="server">
        
        <br />
        <asp:Panel id="CreateSolidCP" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="CreateWebSite" runat="server" meta:resourcekey="CreateWebSite" Text="Create Web Site" CssClass="input-group" Checked="true" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeCreateWebSite" runat="server" meta:resourcekey="DescribeCreateWebSite">Description...</asp:Localize>
            </div>
            <div class="form-inline" style="padding-left: 20px;">
		        <asp:Label ID="lblHostName" runat="server" meta:resourcekey="lblHostName" Text="Host name:"></asp:Label>
			    <asp:TextBox ID="txtHostName" runat="server" CssClass="form-control" Text="www"></asp:TextBox>
            </div>
        </asp:Panel>

        <asp:Panel id="PointSolidCP" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="PointWebSite" runat="server" meta:resourcekey="PointWebSite" Text="Assign to Web Site" CssClass="input-group"
                AutoPostBack="true" /><br />
            <div style="padding-left: 20px;">
                <asp:DropDownList ID="WebSitesList" Runat="server" CssClass="form-control" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            </div>
        </asp:Panel>
        
        <asp:Panel id="PointMailDomainPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="PointMailDomain" runat="server" meta:resourcekey="PointMailDomain" Text="Assign to Mail Domain" CssClass="input-group"
                AutoPostBack="true" /><br />
            <div style="padding-left: 20px;">
                <asp:DropDownList ID="MailDomainsList" Runat="server" CssClass="form-control" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            </div>
        </asp:Panel>
        
        <asp:Panel id="EnableDnsPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="EnableDns" runat="server" meta:resourcekey="EnableDns" Text="Enable DNS" CssClass="input-group"
                Checked="true" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeEnableDns" runat="server" meta:resourcekey="DescribeEnableDns">Description...</asp:Localize>
            </div>
        </asp:Panel>
<!--        
        <asp:Panel id="InstantAliasPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="CreateInstantAlias" runat="server" meta:resourcekey="CreateInstantAlias"
                Text="Create Instant Alias" CssClass="Checkbox Bold" Checked="true" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeCreateInstantAlias" runat="server" meta:resourcekey="DescribeCreateInstantAlias">Description...</asp:Localize>
            </div>
        </asp:Panel>
-->        
        <asp:Panel id="AllowSubDomainsPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="AllowSubDomains" runat="server" meta:resourcekey="AllowSubDomains" Text="Allow sub-domains" CssClass="input-group" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeAllowSubDomains" runat="server" meta:resourcekey="DescribeAllowSubDomains">Description...</asp:Localize>
            </div>
        </asp:Panel>
        
    </asp:Panel>

</div>

<div class="panel-footer text-right">
     <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
         <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/>
 	</CPCC:StyleButton>
     <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Adding Domain...');" ValidationGroup="Domain"> 
         <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAdd"/> 
    </CPCC:StyleButton>
</div>
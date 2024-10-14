<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditServer.ascx.cs" Inherits="SolidCP.Portal.ServersEditServer" %>
<%@ Register Src="ServerVLANsControl.ascx" TagName="ServerVLANsControl" TagPrefix="uc5" %>
<%@ Register Src="ServerServicesControl.ascx" TagName="ServerServicesControl" TagPrefix="uc4" %>
<%@ Register Src="ServerIPAddressesControl.ascx" TagName="ServerIPAddressesControl" TagPrefix="uc2" %>
<%@ Register Src="ServerDnsRecordsControl.ascx" TagName="ServerDnsRecordsControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/ServerPasswordControl.ascx" TagName="ServerPasswordControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="ProductVersion" Src="SkinControls/ProductVersion.ascx" %>
<%@ Import Namespace="SolidCP.Portal" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />
<asp:ValidationSummary ID="summary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="Server" />

<section>
    <div class="panel-body">
        <div class="row">
            <div class="col-md-8">
                <table width="100%">
                    <tr>
                        <td class="Normal" width="100%">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="VirtualServerNameValidator" runat="server" ControlToValidate="txtName"
                                ValidationGroup="Server" meta:resourcekey="ServerNameValidator">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal">
                            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control"
                                Width="100%" Rows="3" TextMode="MultiLine">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <scp:CollapsiblePanel ID="ConnectionHeader" runat="server" IsCollapsed="true"
                    TargetControlID="ConnectionPanel" ResourceKey="ConnectionHeader" Text="Connection Settings"></scp:CollapsiblePanel>
                <asp:Panel ID="ConnectionPanel" runat="server" Height="0" Style="overflow: hidden;">
                    <table width="100%">
                        <tr>
                            <td class="SubHead" style="width: 150px">
                                <asp:Label ID="lblServerUrl" runat="server" meta:resourcekey="lblServerUrl"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" style="width: 150px">
                                <asp:Label ID="lblNewPassword" runat="server" meta:resourcekey="lblNewPassword"></asp:Label>
                            </td>
                            <td class="Normal">
                                <uc1:ServerPasswordControl ID="serverPassword" runat="server" ValidationGroup="ServerPassword" width="100%"></uc1:ServerPasswordControl>
                            </td>
                        </tr>
                        <tr>
                            <td class="Normal"></td>
                            <td class="Normal">
                                <asp:Button ID="btnChangeServerPassword" meta:resourcekey="btnChangeServerPassword" runat="server" CssClass="btn btn-primary" CausesValidation="true" OnClick="btnChangeServerPassword_Click" ValidationGroup="ServerPassword" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <scp:CollapsiblePanel ID="ADHeader" runat="server" IsCollapsed="true"
                    TargetControlID="ADPanel" ResourceKey="ADHeader" Text="Active Directory Settings"></scp:CollapsiblePanel>
                <asp:Panel ID="ADPanel" runat="server" Height="0" Style="overflow: hidden;">
                    <table>
                        <tr>
                            <td class="SubHead" valign="top" width="150px">
                                <asp:Label ID="lblSecurityMode" runat="server" meta:resourcekey="lblSecurityMode"></asp:Label>
                            </td>
                            <td class="Normal" valign="top">
                                <asp:RadioButtonList ID="rbUsersCreationMode" runat="server" AutoPostBack="True" resourcekey="rbUsersCreationMode" CssClass="Normal" onchange="AccountChange();">
                                    <asp:ListItem Value="0">LocalAccounts</asp:ListItem>
                                    <asp:ListItem Value="1">ADAccounts</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trAuthType" runat="server">
                            <td class="SubHead">
                                <asp:Label ID="lblAuthType" runat="server" meta:resourcekey="lblAuthType"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:DropDownList ID="ddlAdAuthType" runat="server" AutoPostBack="True" meta:resourcekey="ddlAdAuthType" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="None">None</asp:ListItem>
                                    <asp:ListItem Value="Secure">Secure</asp:ListItem>
                                    <asp:ListItem Value="Delegation">Delegation</asp:ListItem>
                                    <asp:ListItem Value="Anonymous">Anonymous</asp:ListItem>

                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr id="trUseAdParentDomain" runat="server">
                            <td class="SubHead">
                                <asp:Label ID="lblAdUseParentDomain" runat="server" meta:resourcekey="lblAdUseParentDomain"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:CheckBox ID="chkUseAdParentDomain" runat="server" AutoPostBack="true" OnCheckedChanged="chkUseAdParentDomain_StateChanged" />
                            </td>
                        </tr>

                        <tr id="trParentDomain" runat="server">
                            <td class="SubHead">
                                <asp:Label ID="lblAdParentDomain" runat="server" meta:resourcekey="lblAdParentDomain"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:TextBox ID="txtAdParentDomain" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </td>
                        </tr>

                        <tr id="trParentDomainController" runat="server">
                            <td class="SubHead">
                                <asp:Label ID="lblAdParentDomainController" runat="server" meta:resourcekey="lblAdParentDomainController"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:TextBox ID="txtAdParentDomainController" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </td>
                        </tr>

                        <tr id="trAddDomain" runat="server">
                            <td class="SubHead">
                                <asp:Label ID="lblAdDomain" runat="server" meta:resourcekey="lblAdDomain"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:TextBox ID="txtDomainName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trAdUserName" runat="server">
                            <td class="SubHead">
                                <asp:Label ID="lblAdUsername" runat="server" meta:resourcekey="lblAdUsername"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:TextBox ID="txtAdUsername" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trAdPassword" runat="server">
                            <td class="SubHead" valign="top">
                                <asp:Label ID="lblAdPassword" runat="server" meta:resourcekey="lblAdPassword"></asp:Label>
                            </td>
                            <td class="Normal">
                                <uc1:ServerPasswordControl ID="adPassword" runat="server"
                                    ValidationEnabled="false" ValidationGroup="ADPassword" onkeyup="change(this,'btnChangeADPassword');"></uc1:ServerPasswordControl>
                            </td>
                        </tr>
                        <tr id="trAdButton" runat="server">
                            <td class="Normal"></td>
                            <td class="Normal">
                                <asp:Button ID="btnChangeADPassword" meta:resourcekey="btnChangeADPassword" runat="server" CssClass="btn btn-primary" CausesValidation="true" OnClick="btnChangeADPassword_Click" ValidationGroup="ADPassword" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <scp:CollapsiblePanel ID="PreviewDomainHeader" runat="server" IsCollapsed="true"
                    TargetControlID="PreviewDomainPanel" ResourceKey="PreviewDomainHeader" Text="Preview Domain"></scp:CollapsiblePanel>
                <asp:Panel ID="PreviewDomainPanel" runat="server" Height="0" Style="overflow: hidden;">
                    <table width="100%">
                        <tr>
                            <td class="Normal">
                                <div class="form-inline">
                                    customerdomain.com.&nbsp;<asp:TextBox ID="txtPreviewDomain" runat="server" CssClass="form-control" CausesValidation="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="DomainFormatValidator" ValidationGroup="Server" runat="server" meta:resourcekey="DomainFormatValidator"
                                        ControlToValidate="txtPreviewDomain" Display="Dynamic" SetFocusOnError="true"
                                        ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,15}$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <scp:CollapsiblePanel ID="IPAddressesHeader" runat="server" IsCollapsed="true"
                    TargetControlID="IPAddressesPanel" ResourceKey="IPAddressesHeader" Text="IP Addresses"></scp:CollapsiblePanel>
                <asp:Panel ID="IPAddressesPanel" runat="server" Height="0" Style="overflow: hidden;">
                    <table width="100%">
                        <tr>
                            <td>
                                <uc2:ServerIPAddressesControl ID="ServerIPAddressesControl1" runat="server"></uc2:ServerIPAddressesControl>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <scp:CollapsiblePanel ID="VLANsHeader" runat="server" IsCollapsed="true"
                    TargetControlID="VLANsPanel" ResourceKey="VLANsHeader" Text="Private Network VLANs"></scp:CollapsiblePanel>
                <asp:Panel ID="VLANsPanel" runat="server" Height="0" Style="overflow: hidden;">
                    <table width="100%">
                        <tr>
                            <td>
                                <uc5:ServerVLANsControl ID="ServerVLANsControl1" runat="server"></uc5:ServerVLANsControl>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <scp:CollapsiblePanel ID="ServicesHeader" runat="server"
                    TargetControlID="ServicesPanel" ResourceKey="ServicesHeader" Text="Services"></scp:CollapsiblePanel>
                <asp:Panel ID="ServicesPanel" runat="server" Height="0" Style="overflow: hidden;">
                    <table width="100%">
                        <tr>
                            <td class="Normal">
                                <div>
                                    <asp:Button ID="btnDiscoverServices" meta:resourcekey="btnDiscoverServices" runat="server" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnDiscoverServices_Click"  OnClientClick="ShowProgressDialog('Search installed software...');" Text = "'Search installed software" />
                                </div>      
                                <div>&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc4:ServerServicesControl ID="ServerServicesControl" runat="server"></uc4:ServerServicesControl>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <scp:CollapsiblePanel ID="DnsRecordsHeader" runat="server" IsCollapsed="true"
                    TargetControlID="DnsRecordsPanel" ResourceKey="DnsRecordsHeader" Text="DNS Records Template"></scp:CollapsiblePanel>
                <asp:Panel ID="DnsRecordsPanel" runat="server" Height="0" Style="overflow: hidden;">
                    <table width="100%">
                        <tr>
                            <td>
                                <uc3:ServerDnsRecordsControl ID="ServerDnsRecordsControl1" runat="server"></uc3:ServerDnsRecordsControl>

                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div class="col-md-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-wrench">&nbsp;</i>&nbsp;<asp:Label ID="lblServerTools" runat="server" meta:resourcekey="lblServerTools" Text="Server Tools"></asp:Label></h3>
                    </div>
                    <ul class="list-group">
                        <asp:Panel ID="pnTerminalPanel" runat="server">
                            <li class="list-group-item">
                                <asp:HyperLink ID="lnkTerminalSessions" runat="server"
                                    meta:resourcekey="lnkTerminalConnections" Text="Remote Desktop Sessions">
                                </asp:HyperLink>
                            </li>
                        </asp:Panel>
                        <asp:Panel ID="pnWindowsServices" runat="server">
                            <li class="list-group-item">
                                <asp:HyperLink ID="lnkWindowsServices" runat="server"
                                    meta:resourcekey="lnkWindowsServices" Text="Windows Services">
                                </asp:HyperLink>
                            </li>
                        </asp:Panel>
                        <asp:Panel ID="pnUnixServices" runat="server" Visible="false">
                            <li class="list-group-item">
                                <asp:HyperLink ID="lnkUnixServices" runat="server"
                                    meta:resourcekey="lnkUnixServices" Text="System Services">
                                </asp:HyperLink>
                            </li>
                        </asp:Panel>
                        <li class="list-group-item">
                            <asp:HyperLink ID="lnkWindowsProcesses" runat="server"
                                meta:resourcekey="lnkWindowsProcesses" Text="System Processes">
                            </asp:HyperLink>
                        </li>
                        <li class="list-group-item">
                            <asp:HyperLink ID="lnkEventViewer" runat="server"
                                meta:resourcekey="lnkEventViewer" Text="Event Viewer">
                            </asp:HyperLink>

                        </li>
                        <asp:Panel ID="pnPlatformPanel" runat="server">
                            <li class="list-group-item">
                                <asp:HyperLink ID="lnkPlatformInstaller" runat="server"
                                    meta:resourcekey="lnkPlatformInstaller" Text="Web Platform Installer">
                                </asp:HyperLink>
                            </li>
                        </asp:Panel>
                        <li class="list-group-item">
                            <asp:HyperLink ID="lnkServerReboot" runat="server"
                                meta:resourcekey="lnkServerReboot" Text="Server Reboot">
                            </asp:HyperLink>
                        </li>
                    </ul>
                </div>
                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-medkit">&nbsp;</i>&nbsp;<asp:Label ID="lblRecoveryTools" runat="server" meta:resourcekey="lblRecoveryTools" Text="Server Recovery"></asp:Label></h3>
                    </div>
                    <ul class="list-group">
                        <li class="list-group-item">
                            <asp:HyperLink ID="lnkBackup" runat="server"
                                meta:resourcekey="lnkBackup" Text="Backup">
                            </asp:HyperLink>
                        </li>
                        <li class="list-group-item">
                            <asp:HyperLink ID="lnkRestore" runat="server"
                                meta:resourcekey="lnkRestore" Text="Restore">
                            </asp:HyperLink>
                        </li>

                    </ul>
                </div>
                <br />
                <div class="panel panel-primary">

                    <ul class="list-group">

                        <li class="list-group-item">
                            <asp:Label ID="lblServerVersion" runat="server" meta:resourcekey="lblServerVersion" Text="SolidCP Server Version"></asp:Label>
                            :<br />
                            <asp:Localize ID="locVersion" runat="server" meta:resourcekey="locVersion" />
                            <asp:Label ID="scpVersion" runat="server" />
                        </li>
                        <li class="list-group-item">
                            <asp:Label ID="lblServerFilePath" runat="server" meta:resourcekey="lblServerFilePath" Text="SolidCP Server Filepath"></asp:Label>
                            :<br />
                            <asp:Localize ID="locFilepath" runat="server" meta:resourcekey="locFilepath" />
                            <asp:Label ID="scpFilepath" runat="server" />
                        </li>

                    </ul>
                </div>
                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-microchip">&nbsp;</i>&nbsp;<asp:Label ID="lblServerRAM" runat="server" meta:resourcekey="lblServerRAM" Text="Server RAM"></asp:Label></h3>
                    </div>
                    <ul class="list-group">
                        <li class="list-group-item">
                            <asp:Label ID="lblFreeMemory" runat="server" meta:resourcekey="lblFreeMemory" Text="Free Memory"></asp:Label>
                            :
                <asp:Localize ID="locFreeMemory" runat="server" meta:resourcekey="locFreeMemory" />
                            <asp:Label ID="freeMemory" runat="server" />
                            (MB)
                        </li>
                        <li class="list-group-item">
                            <asp:Label ID="lblTotalMemory" runat="server" meta:resourcekey="lblTotalMemory" Text="Total Memory"></asp:Label>
                            :
                <asp:Localize ID="locTotalMemory" runat="server" meta:resourcekey="locTotalMemory" />
                            <asp:Label ID="totalMemory" runat="server" />
                            (MB)
                        </li>
                        <li class="list-group-item">Usage:
                            <scp:Gauge ID="ramGauge" runat="server" Progress="0" Total="100" />
                        </li>
                    </ul>
                </div>

            </div>
        </div>

    </div>
</section>
<div class="panel-footer text-right">
    <CPCC:StyleButton ID="btnDelete" runat="server" CausesValidation="false" CssClass="btn btn-danger" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete server?');"><i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText" /></CPCC:StyleButton>
    <CPCC:StyleButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-warning" OnClick="btnCancel_Click"><i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel" /></CPCC:StyleButton>
    <CPCC:StyleButton ID="btnUpdate" runat="server" ValidationGroup="Server" CssClass="btn btn-success" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Updating Server...');"><span class="fa fa-refresh">&nbsp;</span>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText" /></CPCC:StyleButton>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcImportServer.ascx.cs" Inherits="SolidCP.Portal.VPS.VdcImportServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="AddServer48" runat="server" />
				    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Import VPS"></asp:Localize>
			    </div>
			    <div class="panel-body form-horizontal">
    			    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">  
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="ImportWizard" ShowMessageBox="True" ShowSummary="False" />
                        
                    
                    <table cellpadding="3">
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locHyperVService" runat="server" meta:resourcekey="locHyperVService" Text="Hyper-V Service:"></asp:Localize>
                            </td>
                            <td>
                                <asp:DropDownList ID="HyperVServices" runat="server" CssClass="NormalTextBox"
                                    DataValueField="ServiceID" DataTextField="FullServiceName" AutoPostBack="true"
                                    onselectedindexchanged="HyperVServices_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequireHyperVService" runat="server"
                                    ControlToValidate="HyperVServices" ValidationGroup="ImportWizard" meta:resourcekey="RequireHyperVService"
                                    Display="Dynamic" SetFocusOnError="true" Text="*">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locVirtualMachine" runat="server" meta:resourcekey="locVirtualMachine" Text="Virtual machine:"></asp:Localize>
                            </td>
                            <td>
                                <asp:DropDownList ID="VirtualMachines" runat="server" CssClass="NormalTextBox"
                                    DataValueField="VirtualMachineId" DataTextField="Name" AutoPostBack="true"
                                    onselectedindexchanged="VirtualMachines_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredVirtualMachine" runat="server"
                                    ControlToValidate="VirtualMachines" ValidationGroup="ImportWizard" meta:resourcekey="RequiredVirtualMachine"
                                    Display="Dynamic" SetFocusOnError="true" Text="*">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    
                    <scp:CollapsiblePanel id="secOsTemplate" runat="server"
                        TargetControlID="OsTemplatePanel" meta:resourcekey="secOsTemplate" Text="OS Template">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="OsTemplatePanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize ID="locOsTemplate" runat="server" meta:resourcekey="locOsTemplate" Text="OS Template:"></asp:Localize>
                                </td>
                                <td>
                                    <asp:DropDownList ID="OsTemplates" runat="server" CssClass="NormalTextBox"
                                        DataValueField="Path" DataTextField="Name"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredOsTemplate" runat="server"
                                        ControlToValidate="OsTemplates" ValidationGroup="ImportWizard" meta:resourcekey="RequiredOsTemplate"
                                        Display="Dynamic" SetFocusOnError="true" Text="*">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="EnableRemoteDesktop" runat="server" AutoPostBack="true"
                                        meta:resourcekey="EnableRemoteDesktop" Text="Enable Remote Desktop Web Connection" />
                                </td>
                            </tr>
                            <tr id="AdminPasswordPanel" runat="server" visible="false">
                                <td class="FormLabel150" valign="top">
                                    <asp:Localize ID="locAdminPassword" runat="server" meta:resourcekey="locAdminPassword" Text="Administrator password:"></asp:Localize>
                                </td>
                                <td>
                                    <asp:TextBox ID="adminPassword" runat="server" TextMode="Password" CssClass="NormalTextBox"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredAdminPassword" runat="server"
                                        ControlToValidate="adminPassword" ValidationGroup="ImportWizard" meta:resourcekey="RequiredAdminPassword"
                                        Display="Dynamic" SetFocusOnError="true" Text="*">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <asp:Panel ID="VirtualMachinePanel" runat="server">
                        <scp:CollapsiblePanel id="secConfiguration" runat="server"
                            TargetControlID="ConfigurationPanel" meta:resourcekey="secConfiguration" Text="Configuration">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="ConfigurationPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <table cellpadding="4">
                                <tr>
                                    <td class="FormLabel150">
                                        <asp:Localize ID="locCPU" runat="server" meta:resourcekey="locCPU" Text="CPU:"></asp:Localize>
                                    </td>
                                    <td class="NormalBold">
                                        <asp:Literal ID="CpuCores" runat="server" Text="0"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Localize ID="locRAM" runat="server" meta:resourcekey="locRAM" Text="RAM:"></asp:Localize>
                                    </td>
                                    <td class="NormalBold">
                                        <asp:Literal ID="RamSize" runat="server" Text="0"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Localize ID="locHDD" runat="server" meta:resourcekey="locHDD" Text="HDD:"></asp:Localize>
                                    </td>
                                    <td class="NormalBold">
                                        <asp:Literal ID="HddSize" runat="server" Text="0"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Localize ID="locVhdPath" runat="server" meta:resourcekey="locVhdPath" Text="VHD location:"></asp:Localize>
                                    </td>
                                    <td class="NormalBold">
                                        <asp:Literal ID="VhdPath" runat="server" Text="0"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secBios" runat="server"
                            TargetControlID="BiosPanel" meta:resourcekey="secBios" Text="BIOS">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="BiosPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <table cellpadding="4" width="100%">
                                <tr>
                                    <td style="width:30%;">
                                        <scp:CheckBoxOption id="BootFromCd" runat="server" Value="False" />
                                        <asp:Localize ID="locBootFromCd" runat="server" meta:resourcekey="locBootFromCd"></asp:Localize>
                                    </td>
                                    <td>
                                        <scp:CheckBoxOption id="NumLockEnabled" runat="server" Value="False" />
                                        <asp:Localize ID="locNumLockEnabled" runat="server" meta:resourcekey="locNumLockEnabled"></asp:Localize>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secDvd" runat="server"
                            TargetControlID="DvdPanel" meta:resourcekey="secDvd" Text="DVD">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="DvdPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <table cellpadding="4">
                                <tr>
                                    <td>
                                        <scp:CheckBoxOption id="DvdInstalled" runat="server" Value="False" />
                                        <asp:Localize ID="locDvdInstalled" runat="server" meta:resourcekey="locDvdInstalled"></asp:Localize>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secAllowedActions" runat="server"
                            TargetControlID="AllowedActionsPanel" meta:resourcekey="secAllowedActions" Text="Allowed Actions">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="AllowedActionsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <table cellpadding="4" width="100%">
                                <tr>
                                    <td style="width:30%;">
                                        <asp:CheckBox ID="AllowStartShutdown" runat="server" meta:resourcekey="AllowStartShutdown" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="AllowReboot" runat="server" meta:resourcekey="AllowReboot" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="AllowPause" runat="server" meta:resourcekey="AllowPause" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="AllowReset" runat="server" meta:resourcekey="AllowReset" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secExternalNetwork" runat="server"
                            TargetControlID="ExternalNetworkPanel" meta:resourcekey="secExternalNetwork" Text="External Network">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="ExternalNetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <table width="100%">
                                <tr>
                                    <td class="FormLabel150">
                                        <asp:Localize ID="locExternalAdapter" runat="server" meta:resourcekey="locExternalAdapter" Text="Connected NIC:"></asp:Localize>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ExternalAdapters" runat="server" CssClass="NormalTextBox"
                                            DataValueField="MacAddress" DataTextField="Name" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="ExternalAddressesRow" runat="server">
                                    <td valign="top">
                                        <asp:Localize ID="locExternalAddresses" runat="server" meta:resourcekey="locExternalAddresses" Text="Assign IP addresses:"></asp:Localize>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="ExternalAddresses" runat="server" Rows="5"
                                            Width="220" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="RequiredExternalAddresses" runat="server"
                                            ControlToValidate="ExternalAddresses" ValidationGroup="ImportWizard" meta:resourcekey="RequiredExternalAddresses"
                                            Display="Dynamic" SetFocusOnError="true" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secManagementNetwork" runat="server"
                            TargetControlID="ManagementNetworkPanel" meta:resourcekey="secManagementNetwork" Text="Management Network">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="ManagementNetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <table width="100%">
                                <tr>
                                    <td class="FormLabel150">
                                        <asp:Localize ID="locManagementAdapter" runat="server" meta:resourcekey="locManagementAdapter" Text="Connected NIC:"></asp:Localize>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ManagementAdapters" runat="server" CssClass="NormalTextBox"
                                            DataValueField="MacAddress" DataTextField="Name" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="ManagementAddressesRow" runat="server">
                                    <td valign="top">
                                        <asp:Localize ID="locManagementAddresses" runat="server" meta:resourcekey="locManagementAddresses" Text="Assign IP addresses:"></asp:Localize>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="ManagementAddresses" runat="server" Rows="5"
                                            Width="220" SelectionMode="Single"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="RequiredManagementAddresses" runat="server"
                                            ControlToValidate="ManagementAddresses" ValidationGroup="ImportWizard" meta:resourcekey="RequiredManagementAddresses"
                                            Display="Dynamic" SetFocusOnError="true" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <p>
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnImport" CssClass="btn btn-success" runat="server" OnClick="btnImport_Click" ValidationGroup="ImportWizard"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnImportText"/> </CPCC:StyleButton>
                    </p>
			    </div>
                </div>
                </div>
                </div>
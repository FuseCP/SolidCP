<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsEditConfiguration.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsEditConfiguration" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="DynamicMemoryControl" Src="UserControls/DynamicMemoryControl.ascx" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_config" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
		            <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server" meta:resourcekey="locSubTitle" Text="Edit Configuration" />
		            </p>

                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Vps" ShowMessageBox="True" ShowSummary="False" />
                    
                    <wsp:CollapsiblePanel id="secResources" runat="server"
                        TargetControlID="ResourcesPanel" meta:resourcekey="secResources" Text="Resources">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="ResourcesPanel" runat="server" Height="0" style="overflow:hidden;padding:10px;width:400px;">
                        <table cellpadding="3">
                            <tr>
                                <td style="width:60px;"><asp:Label ID="lblCpu" runat="server" AssociatedControlID="ddlCpu"
                                        meta:resourcekey="lblCpu" Text="CPU:" CssClass="Medium" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlCpu" runat="server" CssClass="HugeTextBox">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:Localize ID="locCores" runat="server" meta:resourcekey="locCores" Text="cores"/></td>
                            </tr>
                        </table>
                        <table cellpadding="3">
                            <tr>
                                <td style="width:60px;"><asp:Label ID="lblRam" runat="server" AssociatedControlID="txtRam"
                                        meta:resourcekey="lblRam" Text="RAM:" CssClass="Medium" /></td>
                                <td>
                                    <asp:TextBox ID="txtRam" runat="server" CssClass="HugeTextBox" Width="70"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireRamValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtRam" meta:resourcekey="RequireRamValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                </td>
                                <td><asp:Localize ID="locMB" runat="server" meta:resourcekey="locMB" Text="MB"/></td>
                            </tr>
                        </table>
                        <table cellpadding="3">
                            <tr>
                                <td style="width:60px;"><asp:Label ID="lblHdd" runat="server" AssociatedControlID="txtHdd"
                                        meta:resourcekey="lblHdd" Text="HDD:" CssClass="Medium" /></td>
                                <td>
                                    <asp:TextBox ID="txtHdd" runat="server" CssClass="HugeTextBox" Width="70"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireHddValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtHdd" meta:resourcekey="RequireHddValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                </td>
                                <td><asp:Localize ID="locGB" runat="server" meta:resourcekey="locGB" Text="GB"/></td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <wsp:DynamicMemoryControl runat="server" ID="DynamicMemorySetting" Mode="Display"/>

                    <wsp:CollapsiblePanel id="secSnapshots" runat="server"
                        TargetControlID="SnapshotsPanel" meta:resourcekey="secSnapshots" Text="Snapshots">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="SnapshotsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table>
                            <tr>
                                <td class="FormLabel150"><asp:Localize ID="locSnapshots" runat="server"
                                    meta:resourcekey="locSnapshots" Text="Number of snapshots:"></asp:Localize></td>
                                <td>
                                    <asp:TextBox ID="txtSnapshots" runat="server" CssClass="NormalTextBox" Width="50"></asp:TextBox>
                                    
                                    <asp:RequiredFieldValidator ID="SnapshotsValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtSnapshots" meta:resourcekey="SnapshotsValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:CollapsiblePanel id="secDvd" runat="server"
                        TargetControlID="DvdPanel" meta:resourcekey="secDvd" Text="DVD">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="DvdPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkDvdInstalled" runat="server" Checked="true"
                                        Text="DVD drive installed" meta:resourcekey="chkDvdInstalled" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:CollapsiblePanel id="secBios" runat="server"
                        TargetControlID="BiosPanel" meta:resourcekey="secBios" Text="BIOS">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="BiosPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table>
                            <tr>
                                <td style="width:200px;">
                                    <asp:CheckBox ID="chkBootFromCd" runat="server" Text="Boot from CD" meta:resourcekey="chkBootFromCd" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:CollapsiblePanel id="secActions" runat="server"
                        TargetControlID="ActionsPanel" meta:resourcekey="secActions" Text="Allowed actions">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="ActionsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table style="width:400px;">
                            <tr>
                                <td style="width:200px;">
                                    <asp:CheckBox ID="chkStartShutdown" runat="server" Text="Start, Turn off and Shutdown" meta:resourcekey="chkStartShutdown" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkReset" runat="server" Text="Reset" meta:resourcekey="chkReset" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkPauseResume" runat="server" Text="Pause, Resume" meta:resourcekey="chkPauseResume" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkReinstall" runat="server" Text="Re-install" meta:resourcekey="chkReinstall" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkReboot" runat="server" Text="Reboot" meta:resourcekey="chkReboot" />
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:CollapsiblePanel id="secNetwork" runat="server"
                        TargetControlID="NetworkPanel" meta:resourcekey="secNetwork" Text="Network">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="NetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkExternalNetworkEnabled" runat="server"
                                             meta:resourcekey="chkExternalNetworkEnabled" Text="External network enabled" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkPrivateNetworkEnabled" runat="server"
                                                    meta:resourcekey="chkPrivateNetworkEnabled" Text="Private network enabled" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <p>
                        <asp:Button ID="btnUpdate" runat="server" meta:resourcekey="btnUpdate"
                            ValidationGroup="Vps" Text="Update" CssClass="Button1" 
                            onclick="btnUpdate_Click" />
                        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel"
                            CausesValidation="false" Text="Cancel" CssClass="Button1" 
                            onclick="btnCancel_Click" />
                    </p>       

			    </div>
		    </div>
	    </div>

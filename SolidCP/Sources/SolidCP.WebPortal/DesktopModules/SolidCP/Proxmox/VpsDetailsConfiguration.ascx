<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsConfiguration.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsConfiguration" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="Generation" Src="UserControls/Generation.ascx" %>
<%@ Register TagPrefix="wsp" TagName="DynamicMemoryControl" Src="UserControls/DynamicMemoryControl.ascx" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_config" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                   
                    <wsp:CollapsiblePanel id="secSoftware" runat="server"
                        TargetControlID="SoftwarePanel" meta:resourcekey="secSoftware" Text="Software">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="SoftwarePanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td><asp:Localize ID="locOperatingSystem" runat="server"
                                    meta:resourcekey="locOperatingSystem" Text="Operating system:"></asp:Localize></td>
                                <td>
                                    <asp:Literal ID="litOperatingSystem" runat="server" Text="[OS]"></asp:Literal>
                                </td>
                            </tr>


                        </table>
                    </asp:Panel>
                    
                    <wsp:Generation runat="server" ID="GenerationSetting" Mode="Display"/>

                    <wsp:CollapsiblePanel id="secResources" runat="server"
                        TargetControlID="ResourcesPanel" meta:resourcekey="secResources" Text="Resources">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="ResourcesPanel" runat="server" Height="0" style="overflow:hidden;padding:10px;width:400px;">
                        <table cellspacing="5">
                            <tr>
                                <td class="Medium"><asp:Localize ID="lblCpu" runat="server"
                                        meta:resourcekey="lblCpu" Text="CPU:" /></td>
                                <td class="MediumBold">
                                    <asp:Literal ID="litCpu" runat="server" Text="[cpu]"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="5">
                            <tr>
                                <td class="Medium"><asp:Localize ID="lblRam" runat="server"
                                        meta:resourcekey="lblRam" Text="RAM:" /></td>
                                <td class="MediumBold">
                                    <asp:Literal ID="litRam" runat="server" Text="[ram]"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="5">
                            <tr>
                                <td class="Medium"><asp:Localize ID="lblHdd" runat="server"
                                        meta:resourcekey="lblHdd" Text="HDD:" /></td>
                                <td class="MediumBold">
                                    <asp:Literal ID="litHdd" runat="server" Text="[hdd]"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:DynamicMemoryControl runat="server" ID="DynamicMemorySetting" Mode="Display"/>

                    <wsp:CollapsiblePanel id="secSnapshots" runat="server"
                        TargetControlID="SnapshotsPanel" meta:resourcekey="secSnapshots" Text="Snapshots">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="SnapshotsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td><asp:Localize ID="locSnapshots" runat="server"
                                    meta:resourcekey="locSnapshots" Text="Number of snapshots:"></asp:Localize></td>
                                <td>
                                    <asp:Literal ID="litSnapshots" runat="server" Text="[num]"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:CollapsiblePanel id="secDvd" runat="server"
                        TargetControlID="DvdPanel" meta:resourcekey="secDvd" Text="DVD">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="DvdPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td>
                                    <wsp:CheckBoxOption id="optionDvdInstalled" runat="server"
                                        Text="DVD Drive installed" meta:resourcekey="optionDvdInstalled" Value="True" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:CollapsiblePanel id="secBios" runat="server"
                        TargetControlID="BiosPanel" meta:resourcekey="secBios" Text="BIOS">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="BiosPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td style="width:200px;">
                                    <wsp:CheckBoxOption id="optionBootFromCD" runat="server"
                                        Text="Boot from CD" meta:resourcekey="optionBootFromCD" Value="True" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <wsp:CollapsiblePanel id="secActions" runat="server"
                        TargetControlID="ActionsPanel" meta:resourcekey="secActions" Text="Allowed actions">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="ActionsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table style="width:400px;" cellspacing="5">
                            <tr>
                                <td style="width:200px;">
                                    <wsp:CheckBoxOption id="optionStartShutdown" runat="server"
                                        Text="Start, Turn off and Shutdown" meta:resourcekey="optionStartShutdown" Value="True" />
                                </td>
                                <td>
                                    <wsp:CheckBoxOption id="optionReset" runat="server"
                                        Text="Reset" meta:resourcekey="optionReset" Value="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <wsp:CheckBoxOption id="optionPauseResume" runat="server"
                                        Text="Pause, Resume" meta:resourcekey="optionPauseResume" Value="False" />
                                </td>
                                <td>
                                    <wsp:CheckBoxOption id="optionReinstall" runat="server"
                                        Text="Re-install" meta:resourcekey="optionReinstall" Value="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <wsp:CheckBoxOption id="optionReboot" runat="server"
                                        Text="Reboot" meta:resourcekey="optionReboot" Value="True" />
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
                                <td><wsp:CheckBoxOption id="optionExternalNetwork" runat="server"
                                        Text="External network enabled" meta:resourcekey="optionExternalNetwork" Value="True" />
                                </td>
                            </tr>
                            <tr>
                                <td><wsp:CheckBoxOption id="optionPrivateNetwork" runat="server"
                                        Text="Private network enabled" meta:resourcekey="optionPrivateNetwork" Value="True" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <p style="padding: 5px;">
                        <asp:Button ID="btnEdit" runat="server" CssClass="Button1"
			                 meta:resourcekey="btnEdit" Text="Edit configuration" CausesValidation="false" 
                            onclick="btnEdit_Click" />
                    </p>


			    </div>
		    </div>
	    </div>
    	
<asp:Panel ID="ChangePasswordPanel" runat="server" CssClass="Popup" style="display:none;">
	<table class="Popup-Header" cellpadding="0" cellspacing="0">
		<tr>
			<td class="Popup-HeaderLeft"></td>
			<td class="Popup-HeaderTitle">
				<asp:Localize ID="locChangePassword" runat="server" Text="Change Administrator Password"
				    meta:resourcekey="locChangePassword"></asp:Localize>
			</td>
			<td class="Popup-HeaderRight"></td>
		</tr>
	</table>
	<div class="Popup-Content">
		<div class="Popup-Body">
			<br />
			
			<table cellspacing="7" style="margin-left:20px;">
			    <tr>
			        <td>
			            <asp:Localize ID="locNewPassword" runat="server" Text="Enter new password:"
				            meta:resourcekey="locNewPassword"></asp:Localize>
			        </td>
			    </tr>
			    <tr>
			        <td>
			            <wsp:PasswordControl id="password" runat="server"
			                ValidationGroup="ChangePassword"></wsp:PasswordControl>
			        </td>
			    </tr>
			</table>
			
                                                
			<br />
		</div>
		
		<div class="FormFooter">
		    <asp:Button ID="btnChangePassword" runat="server" CssClass="Button1"
		        meta:resourcekey="btnChangePassword" Text="Change password" 
                ValidationGroup="ChangePassword" onclick="btnChangePassword_Click" />
		        
			<asp:Button ID="btnCancelChangePassword" runat="server" CssClass="Button1"
			    meta:resourcekey="btnCancelChangePassword" Text="Cancel" CausesValidation="false" />
		</div>
	</div>
</asp:Panel>

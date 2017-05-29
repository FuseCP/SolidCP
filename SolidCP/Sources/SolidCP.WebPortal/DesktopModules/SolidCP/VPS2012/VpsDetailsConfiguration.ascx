<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsConfiguration.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsConfiguration" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="Generation" Src="UserControls/Generation.ascx" %>
<%@ Register TagPrefix="scp" TagName="DynamicMemoryControl" Src="UserControls/DynamicMemoryControl.ascx" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_config" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                   
                    <scp:CollapsiblePanel id="secSoftware" runat="server"
                        TargetControlID="SoftwarePanel" meta:resourcekey="secSoftware" Text="Software">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="SoftwarePanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td><asp:Localize ID="locOperatingSystem" runat="server"
                                    meta:resourcekey="locOperatingSystem" Text="Operating system:"></asp:Localize></td>
                                <td>
                                    <asp:Literal ID="litOperatingSystem" runat="server" Text="[OS]"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locAdministratorPassword" runat="server"
                                    meta:resourcekey="locAdministratorPassword" Text="Administrator password:"></asp:Localize></td>
                                <td>
                                    ********
                                    <CPCC:StyleButton ID="btnChangePasswordPopup" runat="server" CausesValidation="false"
                                        Text="Change" meta:resourcekey="btnChangePasswordPopup"></CPCC:StyleButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <scp:Generation runat="server" ID="GenerationSetting" Mode="Display"/>

                    <scp:CollapsiblePanel id="secResources" runat="server"
                        TargetControlID="ResourcesPanel" meta:resourcekey="secResources" Text="Resources">
                    </scp:CollapsiblePanel>
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
                    
                    <scp:DynamicMemoryControl runat="server" ID="DynamicMemorySetting" Mode="Display"/>

                    <scp:CollapsiblePanel id="secSnapshots" runat="server"
                        TargetControlID="SnapshotsPanel" meta:resourcekey="secSnapshots" Text="Snapshots">
                    </scp:CollapsiblePanel>
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
                    
                    <scp:CollapsiblePanel id="secDvd" runat="server"
                        TargetControlID="DvdPanel" meta:resourcekey="secDvd" Text="DVD">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="DvdPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td>
                                    <scp:CheckBoxOption id="optionDvdInstalled" runat="server"
                                        Text="DVD Drive installed" meta:resourcekey="optionDvdInstalled" Value="True" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secBios" runat="server"
                        TargetControlID="BiosPanel" meta:resourcekey="secBios" Text="BIOS">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="BiosPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td style="width:200px;">
                                    <scp:CheckBoxOption id="optionBootFromCD" runat="server"
                                        Text="Boot from CD" meta:resourcekey="optionBootFromCD" Value="True" />
                                </td>
                                <td>
                                    <scp:CheckBoxOption id="optionNumLock" runat="server"
                                        Text="Num Lock enabled" meta:resourcekey="optionNumLock" Value="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secActions" runat="server"
                        TargetControlID="ActionsPanel" meta:resourcekey="secActions" Text="Allowed actions">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="ActionsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table style="width:400px;" cellspacing="5">
                            <tr>
                                <td style="width:200px;">
                                    <scp:CheckBoxOption id="optionStartShutdown" runat="server"
                                        Text="Start, Turn off and Shutdown" meta:resourcekey="optionStartShutdown" Value="True" />
                                </td>
                                <td>
                                    <scp:CheckBoxOption id="optionReset" runat="server"
                                        Text="Reset" meta:resourcekey="optionReset" Value="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <scp:CheckBoxOption id="optionPauseResume" runat="server"
                                        Text="Pause, Resume" meta:resourcekey="optionPauseResume" Value="False" />
                                </td>
                                <td>
                                    <scp:CheckBoxOption id="optionReinstall" runat="server"
                                        Text="Re-install" meta:resourcekey="optionReinstall" Value="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <scp:CheckBoxOption id="optionReboot" runat="server"
                                        Text="Reboot" meta:resourcekey="optionReboot" Value="True" />
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secNetwork" runat="server"
                        TargetControlID="NetworkPanel" meta:resourcekey="secNetwork" Text="Network">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="NetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <table cellspacing="5">
                            <tr>
                                <td><scp:CheckBoxOption id="optionExternalNetwork" runat="server"
                                        Text="External network enabled" meta:resourcekey="optionExternalNetwork" Value="True" />
                                </td>
                            </tr>
                            <tr>
                                <td><scp:CheckBoxOption id="optionPrivateNetwork" runat="server"
                                        Text="Private network enabled" meta:resourcekey="optionPrivateNetwork" Value="True" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                    <p style="padding: 5px;">
                        <CPCC:StyleButton id="btnEdit" CssClass="btn btn-success" runat="server" OnClick="btnEdit_Click" CausesValidation="false"> <i class="fa fa-pencil">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnEditText"/> </CPCC:StyleButton>
                    </p>


			    </div>
		    </div>
	    </div>
    	
<asp:Panel ID="ChangePasswordPanel" runat="server" style="display:none;">
	<div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-i-cursor"></i>  <asp:Localize ID="locChangePassword" runat="server" Text="Change Administrator Password" meta:resourcekey="locChangePassword"></asp:Localize></h3>
			</div>
                    <div class="widget-content Popup">
			<table cellspacing="7" style="margin-left:20px;">
			    <tr>
			        <td>
			            <asp:Localize ID="locNewPassword" runat="server" Text="Enter new password:"
				            meta:resourcekey="locNewPassword"></asp:Localize>
			        </td>
			    </tr>
			    <tr>
			        <td>
			            <scp:PasswordControl id="password" runat="server"
			                ValidationGroup="ChangePassword"></scp:PasswordControl>
			        </td>
			    </tr>
			</table>                     
			</div>
					<div class="popup-buttons text-right">
		    <CPCC:StyleButton id="btnCancelChangePassword" CssClass="btn btn-warning" runat="server" CausesValidation="false"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelChangePasswordText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnChangePassword" CssClass="btn btn-primary" runat="server" OnClick="btnChangePassword_Click" ValidationGroup="ChangePassword"> <i class="fa fa-key">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnChangePasswordText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ChangePasswordModal" runat="server" BehaviorID="PasswordModal"
	TargetControlID="btnChangePasswordPopup" PopupControlID="ChangePasswordPanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelChangePassword" />
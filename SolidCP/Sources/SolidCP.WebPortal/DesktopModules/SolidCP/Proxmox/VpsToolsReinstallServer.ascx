<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsToolsReinstallServer.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsToolsReinstallServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    	
	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_tools" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Reinstall" ShowMessageBox="True" ShowSummary="False" />
                    
		            <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server"
		                    meta:resourcekey="locSubTitle" Text="Re-install VPS Server" />
		            </p>
                    <p>
                        <asp:Localize ID="locDescription" runat="server"
		                    meta:resourcekey="locDescription" Text="This wizard will re-create VPS with the same configuration settings from scratch and then apply current OS template." />
                    </p>
                    <p>
                        <asp:CheckBox ID="chkConfirmReinstall" runat="server" CssClass="NormalBold"
				                    meta:resourcekey="chkConfirmReinstall" Text="Yes, I confirm re-installation of this VPS" />
                    </p>
				    
				    <table cellspacing="5">
				        <tr>
				            <td valign="top">
				                <asp:Localize ID="locPassword" runat="server"
		                            meta:resourcekey="locPassword" Text="New administrator password:" />
				            </td>
				            <td>
				                 <wsp:PasswordControl id="password" runat="server"
			                        ValidationGroup="Reinstall"></wsp:PasswordControl>
				            </td>
				        </tr>
				        <tr>
				            <td colspan="2">
				                <asp:CheckBox ID="chkPreserveExistingFiles" runat="server" CssClass="NormalBold"
				                    meta:resourcekey="chkPreserveExistingFiles" Text="Save existing VPS hard drive files" />
				                <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				                <asp:Localize ID="locPreserveHelp" runat="server"
		                            meta:resourcekey="locPreserveHelp" Text="All files from existing hard drive will be copied to &quot;old&quot; disk folder on hard drive of new VPS." />
				            </td>
				        </tr>
				    </table>
				    <br />
                    <table cellspacing="5" id="AdminOptionsPanel" runat="server">
				        <tr>
				            <td>
				                <asp:CheckBox ID="chkSaveVhd" runat="server"
				                    meta:resourcekey="chkSaveVhd" Text="Do not delete VPS virtual hard drive file" />
				            </td>
				        </tr>
				        <tr>
				            <td>
				                <asp:CheckBox ID="chkExport" runat="server"
				                    meta:resourcekey="chkExport" Text="Export VPS before re-installation to the following folder:" />
				            </td>
				        </tr>
				        <tr>
				            <td style="padding-left:20px;">
				                <asp:TextBox ID="txtExportPath" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
				                
				                <asp:RequiredFieldValidator ID="ExportPathValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtExportPath" meta:resourcekey="ExportPathValidator" SetFocusOnError="true"
                                        ValidationGroup="Reinstall">*</asp:RequiredFieldValidator>
				            </td>
				        </tr>
				    </table>
				    
                    <p>
                        <asp:Button ID="btnReinstall" runat="server" meta:resourcekey="btnReinstall"
                            ValidationGroup="Reinstall" Text="Re-install" CssClass="Button1" 
                            onclick="btnUpdate_Click" />
                            
                        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel"
                            CausesValidation="false" Text="Cancel" CssClass="Button1" 
                            onclick="btnCancel_Click" />
                    </p>
			    </div>
		    </div>
	    </div>

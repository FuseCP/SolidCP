<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsToolsDeleteServer.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsToolsDeleteServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Tools" ShowMessageBox="True" ShowSummary="False" />
                    
		            <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server"
		                    meta:resourcekey="locSubTitle" Text="Delete VPS Server" />
		            </p>
                    <p>
                        <asp:Localize ID="locDescription" runat="server"
		                    meta:resourcekey="locDescription" Text="This wizard will delete VPS and all its contents from the virtualization server." />
                    </p>
                    <p>
                        <asp:CheckBox ID="chkConfirmDelete" runat="server" CssClass="NormalBold"
				                    meta:resourcekey="chkConfirmDelete" Text="Yes, I confirm deletion of this VPS" />
                    </p>
				    
                    <fieldset id="AdminOptionsPanel" runat="server">
                        <legend>
                            <asp:Localize ID="locAdminOptions" runat="server" meta:resourcekey="locAdminOptions" Text="Administrator options"></asp:Localize>
                        </legend>

                            <table cellspacing="5">
				                <tr>
				                    <td>
				                        <asp:CheckBox ID="chkSaveFiles" runat="server"
				                            meta:resourcekey="chkSaveFiles" Text="Do not delete VPS files (virtual hard disk, snapshots)" />
				                    </td>
				                </tr>
				                <tr>
				                    <td>
				                        <asp:CheckBox ID="chkExport" runat="server" AutoPostBack="true"
				                            meta:resourcekey="chkExport" Text="Export VPS before deletion to the following folder:" />
				                    </td>
				                </tr>
				                <tr>
				                    <td style="padding-left:20px;">
				                        <asp:TextBox ID="txtExportPath" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
        				                
				                        <asp:RequiredFieldValidator ID="ExportPathValidator" runat="server" Text="*" Display="Dynamic"
                                                ControlToValidate="txtExportPath" meta:resourcekey="ExportPathValidator" SetFocusOnError="true"
                                                ValidationGroup="Tools">*</asp:RequiredFieldValidator>
				                    </td>
				                </tr>
				            </table>
				     </fieldset>
				    
                    <p>
                        <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete"
                            ValidationGroup="Tools" Text="Delete" CssClass="Button1" 
                            onclick="btnDelete_Click" />
                        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel"
                            CausesValidation="false" Text="Cancel" CssClass="Button1" 
                            onclick="btnCancel_Click" />
                    </p>
			    </div>
		    </div>
	    </div>
    	
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStorageFolderSettingsOwaEditing.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.EnterpriseStorageFolderSettingsOwaEditing" %>


<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnterpriseStorageOwaUsersList.ascx" TagName="OwaUsers" TagPrefix="scp"%>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnterpriseStorageEditFolderTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeList48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Folder"></asp:Localize>

					<asp:Literal ID="litFolderName" runat="server" Text="Folder" />
                        </h3>
                </div>
				<div class="panel-body form-horizontal">
                        <div class="nav nav-tabs" style="padding-bottom:7px !important;">
				            <scp:CollectionTabs id="tabs" runat="server" SelectedTab="enterprisestorage_folder_settings_owa_editing" />
                        </div>
                        <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <scp:CollapsiblePanel id="colOwaEditing" runat="server"
                        TargetControlID="panelFolderPermissions" meta:resourcekey="colOwaEditing" Text="">
                    </scp:CollapsiblePanel>		
                    
                     <asp:Panel runat="server" ID="panelFolderPermissions">                                                
					    <table>
						    <tr>
							    <td colspan="2">
                                    <fieldset id="OwaUsersPanel" runat="server">
                                        <legend><asp:Localize ID="locOwaEditingSection" runat="server" meta:resourcekey="locOwaEditingSection" Text="Users And Groups"></asp:Localize></legend>
                                        <scp:OwaUsers id="owaUsers" runat="server" />
                                    </fieldset>
						    </tr>
					        <tr><td>&nbsp;</td></tr>

					    </table>
                    </asp:Panel>
				</div>
                </div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditFolder"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
					    <asp:ValidationSummary ID="valSummary" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditFolder" />
				    </div>
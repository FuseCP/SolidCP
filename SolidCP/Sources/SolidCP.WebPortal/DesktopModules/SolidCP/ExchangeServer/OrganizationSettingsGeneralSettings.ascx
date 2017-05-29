<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationSettingsGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationSettingsGeneralSettings" %>



<%@ Register Src="UserControls/OrganizationSettingsTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />
				<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="Organization48" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Settings"></asp:Localize>

                    <asp:Literal ID="litOrganizationName" runat="server" Text="Organization" />
                        </h3>
                </div>
                <div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:CollectionTabs ID="tabs" runat="server" SelectedTab="organization_settings_general_settings" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox ID="messageBox" runat="server" />
                            <scp:CollapsiblePanel ID="colGeneralSettings" runat="server" TargetControlID="panelGeneralSettings" meta:ResourceKey="colGeneralSettings" Text="General settings"></scp:CollapsiblePanel>

                            <asp:Panel runat="server" ID="panelGeneralSettings">
                                <table id="GenerralSettignsTable" runat="server" cellpadding="2">
                                    <tr>
                                        <td class="Normal" style="width: 150px;">
                                            <asp:Label ID="lblOrganizationLogoUrl" runat="server"
                                                meta:resourcekey="lblOrganizationLogoUrl" Text="Minimum length:"></asp:Label></td>
                                        <td class="Normal">
                                            <asp:TextBox ID="txtOrganizationLogoUrl" runat="server" CssClass="form-control" Width="400px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="valRequireMinLength" runat="server" ControlToValidate="txtOrganizationLogoUrl" meta:resourcekey="valRequireOrganizationLogoUrl"
                                                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
                                          </td>
                                    </tr>
    
                                </table>
                            </asp:Panel>


                </div>
                    </div>
                    <div class="panel-footer text-right">
                        <scp:ItemButtonPanel ID="buttonPanel" runat="server" ValidationGroup="SettingsEditor"
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
                    </div>

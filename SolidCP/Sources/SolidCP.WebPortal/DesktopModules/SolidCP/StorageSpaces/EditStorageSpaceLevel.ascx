<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditStorageSpaceLevel.ascx.cs" Inherits="SolidCP.Portal.StorageSpaces.EditStorageSpaceLevel" %>

<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/StorageSpaceLevelResourceGroups.ascx" TagName="ResourceGroups" TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />


        <div class="panel-body form-horizontal">
            <scp:SimpleMessageBox ID="messageBox" runat="server" />

            <scp:CollapsiblePanel ID="colSsLevelGeneralSettings" runat="server"
                TargetControlID="panelSsLevelGeneralSettings" meta:ResourceKey="colSsLevelGeneralSettings"></scp:CollapsiblePanel>

            <asp:Panel runat="server" ID="panelSsLevelGeneralSettings">
                <div style="padding: 10px;">
                    <table>
                        <tr>
                            <td class="Label" style="width: 260px;">
                                <asp:Localize ID="locName" runat="server" meta:resourcekey="locName"></asp:Localize>
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox" />
                                <asp:RequiredFieldValidator runat="server" ID="valReqTxtName" ControlToValidate="txtName" meta:resourcekey="valReqTxtName" ErrorMessage="*" ValidationGroup="SaveSsLevel" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" style="width: 260px;">
                                <asp:Localize ID="locDescription" runat="server" meta:resourcekey="locDescription"></asp:Localize>
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" Rows="4" />
                                <asp:RequiredFieldValidator runat="server" ID="valReqTxtDescription" ControlToValidate="txtDescription" meta:resourcekey="valReqTxtDescription" ErrorMessage="*" ValidationGroup="SaveSsLevel" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </asp:Panel>

            <scp:CollapsiblePanel ID="colSsLevelServices" runat="server"
                TargetControlID="panelSsLevelServices" meta:ResourceKey="colSsLevelServices"></scp:CollapsiblePanel>

            <asp:Panel runat="server" ID="panelSsLevelServices">
                <table>
                    <tr>
                        <td colspan="2">
                            <fieldset id="Fieldset1" runat="server">
                                <legend>
                                    <asp:Localize ID="locResourceGroups" runat="server" meta:resourcekey="locResourceGroups" Text="Allowed Resource Groups"></asp:Localize></legend>
                                <scp:ResourceGroups ID="resourceGroups" runat="server" />
                            </fieldset>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>

                </table>
            </asp:Panel>

        </div>
            <div class="panel-footer text-right">
                <scp:ItemButtonPanel ID="buttonPanel" runat="server" ValidationGroup="SaveSsLevel"
                    OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
            </div>
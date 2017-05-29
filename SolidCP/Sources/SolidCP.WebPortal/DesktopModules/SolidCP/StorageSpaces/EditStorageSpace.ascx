<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditStorageSpace.ascx.cs" Inherits="SolidCP.Portal.StorageSpaces.EditStorageSpace" %>



<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/StorageSpaceLevelResourceGroups.ascx" TagName="ResourceGroups" TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />


        <div class="panel-body form-horizontal">
            <scp:SimpleMessageBox ID="messageBox" runat="server" />

            <scp:CollapsiblePanel ID="colStorageSpaceGeneralSettings" runat="server"
                TargetControlID="panelStorageSpaceGeneralSettings" meta:ResourceKey="colStorageSpaceGeneralSettings">
            </scp:CollapsiblePanel>

            <asp:Panel runat="server" ID="panelStorageSpaceGeneralSettings">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div style="padding: 10px;">
                            <table>
                                <tr>
                                    <td class="Label" style="width: 260px;">
                                        <asp:Localize ID="locName" runat="server" meta:resourcekey="locName"></asp:Localize>
                                    </td>
                                    <td style="width: 250px;">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                                        <asp:RequiredFieldValidator runat="server" ID="valReqTxtName" ControlToValidate="txtName" meta:resourcekey="valReqTxtName" ErrorMessage="*" ValidationGroup="SaveSpaceStorage" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" style="width: 200px;">
                                        <asp:Localize ID="lblStorageService" runat="server" meta:resourcekey="lblStorageService" />
                                    <td style="width: 200px;">
                                        <asp:DropDownList ID="ddlStorageService" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlStorageService_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:RequiredFieldValidator ID="valReqStorageService" runat="server" meta:resourcekey="valReqStorageService" ControlToValidate="ddlStorageService"
                                            ErrorMessage="Please select storage space service" ValidationGroup="SaveSpaceStorage" Display="Dynamic" Text="*" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" style="width: 260px;">
                                        <asp:Localize ID="locPath" runat="server" meta:resourcekey="locPath"></asp:Localize>
                                    </td>
                                    <td style="width: 250px;">
                                        <asp:TreeView ID="FoldersTree" runat="server" OnTreeNodePopulate="FoldersTree_OnTreeNodePopulate" />
                                        <asp:CustomValidator ID="valRequireFolder" runat="server" meta:resourcekey="valRequireFolder"
                                            ErrorMessage="sym" Text="Please select a folder" Display="Dynamic" ValidationGroup="SaveSpaceStorage"
                                            ClientValidationFunction="ClientValidateTreeView" OnServerValidate="valRequireFolder_ServerValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="valPathIsInUse" runat="server" meta:resourcekey="valPathIsInUse"
                                            ErrorMessage="sym" Text="Path is already in use" Display="Dynamic" ValidationGroup="SaveSpaceStorage"
                                            OnServerValidate="valPathIsInUseFolder_ServerValidate"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" style="width: 200px;">
                                        <asp:Localize ID="lblSsLevel" runat="server" meta:resourcekey="lblSsLevel" />
                                    <td style="width: 200px;">
                                        <asp:DropDownList ID="ddlSsLevel" runat="server" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="valReqSsLevel" runat="server" meta:resourcekey="valReqSsLevel" ControlToValidate="ddlSsLevel"
                                            ErrorMessage="Please select storage space level" ValidationGroup="SaveSpaceStorage" Display="Dynamic" Text="*" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label">
                                        <asp:Localize ID="locStorageSize" runat="server" meta:resourcekey="locStorageSize" Text="Storage Limit Size (Gb):"></asp:Localize></td>
                                    <td>
                                        <asp:TextBox ID="txtStorageSize" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="valRequireFolderSize" runat="server" meta:resourcekey="valRequireStorageSize" ControlToValidate="txtStorageSize"
                                            ErrorMessage="Enter Storage Size" ValidationGroup="SaveSpaceStorage" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        
                                        <%--
                                            01.09.2015 roland.breitschaft@x-company.de 
                                            Problem: Portal will raise an Error for the Range-Validator. It could not convert the double-Value
                                            Fix: Set the minimum Value to 0                                            
                                            --%>

                                        <%--<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtStorageSize" meta:resourcekey="rangeStorageSize" MaximumValue="99999999" MinimumValue="0.01" Type="Double"
                                            ValidationGroup="SaveSpaceStorage" Display="Dynamic" Text="*" SetFocusOnError="True" />--%>
                                        <asp:RangeValidator ID="rangeStorageSize" runat="server" ControlToValidate="txtStorageSize" meta:resourcekey="rangeStorageSize" MaximumValue="99999999" MinimumValue="0" Type="Double"
                                            ValidationGroup="SaveSpaceStorage" Display="Dynamic" Text="*" SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label">
                                        <asp:Localize ID="locQuotaType" runat="server" meta:resourcekey="locQuotaType" Text="Quota Type:"></asp:Localize></td>
                                    <td class="FormRBtnL">
                                        <asp:RadioButton ID="rbtnQuotaSoft" runat="server" meta:resourcekey="rbtnQuotaSoft" Text="Soft" GroupName="QuotaType" Checked="true" />
                                        <asp:RadioButton ID="rbtnQuotaHard" runat="server" meta:resourcekey="rbtnQuotaHard" Text="Hard" GroupName="QuotaType" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label">
                                        <asp:Localize ID="locIsDisabled" runat="server" meta:resourcekey="locIsDisabled" Text="Disable new folder creation:"></asp:Localize></td>
                                    <td class="FormRBtnL">
                                        <asp:CheckBox runat="server" ID="chkIsDisabled" meta:resourcekey="chkIsDisabled" Text="Yes" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </asp:Panel>


        </div>
            <div class="panel-footer text-right">
                <scp:ItemButtonPanel ID="buttonPanel" runat="server" ValidationGroup="SaveSpaceStorage"
                    OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
            </div>
<script type="text/javascript">
    // if you use jQuery, you can load them when dom is read.
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequest);
    });


    function EndRequest(sender, args) {
        CloseProgressDialog();
    }
</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsEditor.ascx.cs" Inherits="SolidCP.Portal.SpaceSettingsEditor" %>
<div class="panel-body form-horizontal">
    <asp:UpdatePanel runat="server" ID="updatePanelUsers">
        <ContentTemplate>
        
            <asp:DropDownList ID="ddlOverride" runat="server" CssClass="NormalTextBox"
                    resourcekey="ddlOverride" AutoPostBack="true" OnSelectedIndexChanged="ddlOverride_SelectedIndexChanged">
                <asp:ListItem>UseHost</asp:ListItem>
                <asp:ListItem>OverrideHost</asp:ListItem>
            </asp:DropDownList>
            
            <br />
            <br />
            <asp:PlaceHolder ID="settingsPlace" runat="server"></asp:PlaceHolder>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="SettingsEditor"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSave"/> </CPCC:StyleButton>
</div>
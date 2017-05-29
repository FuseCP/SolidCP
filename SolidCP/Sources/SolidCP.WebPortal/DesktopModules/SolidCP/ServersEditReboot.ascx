<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditReboot.ascx.cs" Inherits="SolidCP.Portal.ServersEditReboot" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>

<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server"/>
<div class="panel-body form-horizontal">
    <div class="RedBorderFillBox">
        <asp:Button ID="btnReboot" runat="server" meta:resourcekey="btnReboot" Text="Reboot Server"
			CssClass="Button2" OnClientClick="return confirm('Continue with Server reboot?');" OnClick="btnReboot_Click" />
    </div>
</div>


<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
</div>
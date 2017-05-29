<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailEnable_EditAccount.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MailEnable_EditAccount" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<table width="100%">
	<tr>
		<td class="SubHead" style="width:150px;"><asp:Label ID="lblReplyTo" runat="server" meta:resourcekey="lblReplyTo" Text="Reply to address:"></asp:Label></td>
		<td class="normal">
			<asp:TextBox id="txtReplyTo" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
		</td>
	</tr>
</table>


<scp:CollapsiblePanel id="secAutoresponder" runat="server"
    TargetControlID="AutoresponderPanel" meta:resourcekey="secAutoresponder" Text="Autoresponder">
</scp:CollapsiblePanel>
<asp:Panel ID="AutoresponderPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	    <tr>
		    <td class="SubHead" style="width:150px;"><asp:Label ID="lblResponderEnabled" runat="server" meta:resourcekey="lblResponderEnabled" Text="Enable autoresponder:"></asp:Label></td>
		    <td class="normal">
			    <asp:CheckBox ID="chkResponderEnabled" Runat="server" meta:resourcekey="chkResponderEnabled" Text="Yes"></asp:CheckBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"><asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label></td>
		    <td class="normal">
			    <asp:TextBox id="txtSubject" runat="server" Width="400px" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"><asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessage" Text="Message:"></asp:Label></td>
		    <td class="normal">
			    <asp:TextBox id="txtMessage" runat="server" Width="400px" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secForwarding" runat="server"
    TargetControlID="ForwardingPanel" meta:resourcekey="secForwarding" Text="Mail Forwarding">
</scp:CollapsiblePanel>
<asp:Panel ID="ForwardingPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	    <tr>
		    <td class="SubHead" style="width:150px;"><asp:Label ID="lblForwardTo" runat="server" meta:resourcekey="lblForwardTo" Text="Forward mail to address:"></asp:Label></td>
		    <td class="normal">
			    <asp:TextBox id="txtForward" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
    </table>
</asp:Panel>
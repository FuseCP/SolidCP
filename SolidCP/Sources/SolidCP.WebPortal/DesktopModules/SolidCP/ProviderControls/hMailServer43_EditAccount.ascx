<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="hMailServer43_EditAccount.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.hMailServer43_EditAccount" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secPersonalInfo" runat="server"
    TargetControlID="PersonalInfoPanel" meta:resourcekey="secPersonalInfo" Text="Personal Information">
</scp:CollapsiblePanel>

<asp:Panel ID="PersonalInfoPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First Name:"></asp:Label>
        </td>
        <td class="normal" width="100%">
            <asp:TextBox ID="txtFirstName" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" Text="Last Name:"></asp:Label>
        </td>
        <td class="normal" valign="top">
            <asp:TextBox ID="txtLastName" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secAutoresponder" runat="server"
    TargetControlID="AutoresponderPanel" meta:resourcekey="secAutoresponder" Text="Autoresponder">
</scp:CollapsiblePanel>

<asp:Panel ID="AutoresponderPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	    <tr>
		    <td class="SubHead" width="200" nowrap><asp:Label ID="lblResponderEnabled" runat="server" meta:resourcekey="lblResponderEnabled" Text="Autoresponder Enabled:"></asp:Label></td>
		    <td class="normal" width="100%">
			    <asp:CheckBox ID="chkResponderEnabled" Runat="server" meta:resourcekey="chkResponderEnabled" Text="Yes"></asp:CheckBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"><asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label></td>
		    <td class="normal" vAlign="top">
			    <asp:TextBox id="txtSubject" runat="server" Width="400px" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead" vAlign="top"><asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessage" Text="Message:"></asp:Label></td>
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
		    <td class="SubHead" width="200" nowrap><asp:Label ID="lblForwardTo" runat="server" meta:resourcekey="lblForwardTo" Text="Forward mail to address:"></asp:Label></td>
		    <td class="normal" width="100%" valign="top">
			    <asp:TextBox id="txtForward" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
        <tr>
            <td class="SubHead">
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkOriginalMessage" runat="server" meta:resourcekey="chkOriginalMessage"
                    Text="Keep original Message"></asp:CheckBox>
            </td>
        </tr>
    </table>
</asp:Panel>

<scp:Collapsiblepanel id="Signature" runat="server" targetcontrolid="SignaturePanel"
    meta:resourcekey="Signature" ></scp:collapsiblepanel>
<asp:Panel runat="server" ID="SignaturePanel">
   <table width="100%">
      <tr>
      	<td style="width:150px;">
		    <asp:Label ID="lblSignatureEnabled" runat="server" meta:resourcekey="lblSignatureEnabled" Text="Signature Enabled:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="cbSignatureEnabled" Runat="server" meta:resourcekey="cbHeader" Text="Yes"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
	    <td></td>
	    <td class="SubHead">
		    <asp:Label ID="lblPlainSignature" runat="server" meta:resourcekey="lblPlainSignature" Text="Plain Text Signature"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
		 </td>
		<td class="normal">
			<asp:TextBox ID="txtPlainSignature" runat="server" TextMode="MultiLine" Rows="7" Width="300px"></asp:TextBox>
		</td>
	</tr>
	<tr>
	    <td></td>
		<td class="SubHead">
		    <asp:Label ID="lblHtmlSignature" runat="server" meta:resourcekey="lblHtmlSignature" Text="HTML Signature"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
		 </td>
		<td class="normal">
			<asp:TextBox ID="txtHtmlSignature" runat="server" TextMode="MultiLine" Rows="7" Width="300px"></asp:TextBox>
		</td>
	</tr>
   </table>
 </asp:Panel>
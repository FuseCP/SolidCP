<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="hMailServer5_EditAccount.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.hMailServer5_EditAccount" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secStatusInfo" runat="server" IsCollapsed="True" 
    TargetControlID="StatusInfoPanel" meta:resourcekey="secStatusInfo" Text="Account Information" >
</scp:CollapsiblePanel>

<asp:Panel ID="StatusInfoPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	<tr>
		<td class="SubHead"  style="width:150px;" nowrap><asp:Label ID="lblEnabled" runat="server" meta:resourcekey="lblEnabled" Text="Mailbox Enabled:"></asp:Label></td>
		<td class="normal">
			<asp:CheckBox ID="chkEnabled" Runat="server" meta:resourcekey="chkEnabled" Text="Yes" Checked="true"></asp:CheckBox>
		</td>
	</tr>	
    <tr>
        <td class="SubHead"  style="width:150px;" nowrap>
            <asp:Label ID="lblSize" runat="server" meta:resourcekey="lblSize" Text="Size:"></asp:Label>
        </td>
        <td class="normal">
            <asp:Label ID="lblSizeInfo" runat="server">0</asp:Label>
        </td>
    </tr>
    <tr>
        <td class="SubHead"  style="width:150px;" nowrap>
            <asp:Label ID="lblQuotaUsed" runat="server" meta:resourcekey="lblQuotaUsed" Text="Quota Used:"></asp:Label>
        </td>
        <td class="normal">
            <asp:Label ID="lblQuotaUsedInfo" runat="server">0</asp:Label>
        </td>
    </tr>
    <tr>
        <td class="SubHead"  style="width:150px;" nowrap>
            <asp:Label ID="lblLastLoginDate" runat="server" meta:resourcekey="lblLastLoginDate" Text="Last Login Date:"></asp:Label>
        </td>
        <td class="normal">
            <asp:Label ID="lblLastLoginDateInfo" runat="server">n/a</asp:Label>
        </td>
    </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secPersonalInfo" runat="server"
    TargetControlID="PersonalInfoPanel" meta:resourcekey="secPersonalInfo" Text="Personal Information">
</scp:CollapsiblePanel>

<asp:Panel ID="PersonalInfoPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	    <tr>
        <td class="SubHead"  style="width:150px;" nowrap>
            <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First Name:"></asp:Label>
        </td>
        <td class="normal">
            <asp:TextBox ID="txtFirstName" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead"  style="width:150px;">
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
		    <td class="SubHead"  style="width:150px;" nowrap><asp:Label ID="lblResponderEnabled" runat="server" meta:resourcekey="lblResponderEnabled" Text="Autoresponder Enabled:"></asp:Label></td>
		    <td class="normal">
			    <asp:CheckBox ID="chkResponderEnabled" Runat="server" meta:resourcekey="chkResponderEnabled" Text="Yes"></asp:CheckBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead" style="width:150px;"><asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label></td>
		    <td class="normal" vAlign="top">
			    <asp:TextBox id="txtSubject" runat="server" Width="400px" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"  style="width:150px;" vAlign="top"><asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessage" Text="Message:"></asp:Label></td>
		    <td class="normal">
			    <asp:TextBox id="txtMessage" runat="server" Width="400px" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"  style="width:150px;" nowrap><asp:Label ID="lblResponderExpires" runat="server" meta:resourcekey="lblResponderExpires" Text="Autoresponder Expires:"></asp:Label></td>
		    <td class="normal">
			    <asp:CheckBox ID="chkResponderExpires" Runat="server" meta:resourcekey="chkResponderExpires" Text="Yes"></asp:CheckBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"  style="width:150px;" nowrap><asp:Label ID="lblResponderExpireDate" runat="server" meta:resourcekey="lblResponderExpireDate" Text="Autoresponder Expire Date:"></asp:Label></td>
		    <td class="normal">
			    <asp:TextBox ID="txtResponderExireDate" Runat="server" Text="" CssClass="form-control"></asp:TextBox>
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
		    <td class="SubHead"  style="width:150px;" nowrap><asp:Label ID="lblForwardingEnabled" runat="server" meta:resourcekey="lblForwardingEnabled" Text="Forwarding Enabled:"></asp:Label></td>
		    <td class="normal">
			    <asp:CheckBox ID="chkForwardingEnabled" Runat="server" meta:resourcekey="chkForwardingEnabled" Text="Yes"></asp:CheckBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"  style="width:150px;" nowrap><asp:Label ID="lblForwardTo" runat="server" meta:resourcekey="lblForwardTo" Text="Forward mail to address:"></asp:Label></td>
		    <td class="normal" valign="top">
			    <asp:TextBox id="txtForward" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
        <tr>
            <td class="SubHead" style="width:150px;">
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuspendOverusedSpaces.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.SuspendOverusedSpaces" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>


<scp:CollapsiblePanel id="clpOverusageDefinitionHeader" runat="server"
    TargetControlID="pnlOverusageDefinition" resourcekey="clpOverusageDefinitionHeader" Text="Space overused when">
</scp:CollapsiblePanel>
<asp:Panel ID="pnlOverusageDefinition" runat="server" CssClass="Normal">
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:CheckBox ID="cbxSuspendWhenDiskSpaceOverused" runat="server" meta:resourcekey="cbxSuspendWhenDiskSpaceOverused" Text="Disk space usage greater than defined below threshold" />
   				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:CheckBox ID="cbxSuspendWhenBandwidthOverused" runat="server" meta:resourcekey="cbxSuspendWhenBandwidthOverused" Text="Bandwidth usage greater than defined below threshold" />
				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblThreshold" runat="server" meta:resourcekey="lblThreshold" Text="usage greater than defined below threshold."></asp:Label> 
				</td>
			</tr>
		</table>
</asp:Panel>

	<br />


<scp:CollapsiblePanel id="clpWarningSettingsHeader" runat="server"
    TargetControlID="pnlWarningSettings" resourcekey="clpWarningSettingsHeader" Text="Issue warning">
</scp:CollapsiblePanel>
<asp:Panel ID="pnlWarningSettings" runat="server" CssClass="Normal">
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:CheckBox ID="cbxDoSendWarning" runat="server" meta:resourcekey="cbxDoSendWarning" Text="Send email notification when usage exceeds" />
   				</td>
   				<td class="SubHead" width="100%">
   					<asp:TextBox ID="txtWarningThreshold" runat="server" Width="30" CssClass="form-control" MaxLength="1000"></asp:TextBox>% 
   					<asp:CompareValidator ID="valWarningThreshold" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="txtWarningThreshold" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
   					<asp:Label ID="lblWarningThreshold" runat="server" meta:resourcekey="lblWarningThreshold" Text="threshold."></asp:Label>
   				</td>
			</tr>
		</table>
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblWarningMailFrom" runat="server" meta:resourcekey="lblWarningMailFrom" Text="Mail From:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtWarningMailFrom" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
	   				<asp:RegularExpressionValidator ID="warningMailFromRegExValidator" runat="server" ControlToValidate="txtWarningMailFrom" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblWarningMailBcc" runat="server" meta:resourcekey="lblWarningMailBcc" Text="BCC:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtWarningMailBcc" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
	   				<asp:RegularExpressionValidator ID="warningMailBccRegExValidator" runat="server" ControlToValidate="txtWarningMailBcc" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblWarningMailSubject" runat="server" meta:resourcekey="lblWarningMailSubject" Text="Mail Subject:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtWarningMailSubject" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td colspan="2" class="SubHead" nowrap>
					<asp:Label ID="lblWarningMailBody" runat="server" meta:resourcekey="lblWarningMailBody" Text="Mail Body:"></asp:Label>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:TextBox ID="txtWarningMailBody" runat="server" Width="95%" CssClass="form-control" TextMode="MultiLine" Rows="10" MaxLength="1000"></asp:TextBox>
					<br />
					<asp:Label ID="lblWarningMailBodyHint" runat="server" meta:resourcekey="lblWarningMailBodyHint" Visible="true">
						([threshold], [date], [usage], [space], [customer] variables are supported)
					</asp:Label>
				</td>
			</tr>
		</table>
	
</asp:Panel>

	<br />

<scp:CollapsiblePanel id="clpSuspensionSettingsHeader" runat="server"
    TargetControlID="pnlSuspensionSettings" resourcekey="clpSuspensionSettingsHeader" Text="Suspend space">
</scp:CollapsiblePanel>
<asp:Panel ID="pnlSuspensionSettings" runat="server" CssClass="Normal">
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblWhenUsageThresholdExceeds" runat="server" meta:resourcekey="lblWhenUsageThresholdExceeds" Text="When usage exceeds"></asp:Label>
				</td>
   				<td class="SubHead" width="100%">
	   				<asp:TextBox ID="txtSuspensionThreshold" runat="server" Width="30" CssClass="form-control" MaxLength="1000"></asp:TextBox>%
	   				<asp:CompareValidator ID="valSuspensionThreshold" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="txtSuspensionThreshold" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
   					<asp:Label ID="lblSuspensionThreshold" runat="server" meta:resourcekey="lblWarningThreshold" Text="threshold."></asp:Label>
   				</td>
   			</tr>
   			<tr>
				<td class="SubHead" nowrap colspan="2">
					<asp:CheckBox ID="cbxDoSendSuspension" runat="server" meta:resourcekey="cbxDoSendSuspension" Text="Send email notification" />
   				</td>
			</tr>
   			<tr>
				<td class="SubHead" nowrap colspan="2">
					<asp:CheckBox ID="cbxDoSuspend" runat="server" meta:resourcekey="cbxDoSuspend" Text="Suspend space" />
   				</td>
			</tr>
		</table>
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblSuspensionMailFrom" runat="server" meta:resourcekey="lblSuspensionMailFrom" Text="Mail From:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtSuspensionMailFrom" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
	   				<asp:RegularExpressionValidator ID="suspensionMailFromRegExValidator" runat="server" ControlToValidate="txtSuspensionMailFrom" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblSuspensionMailBcc" runat="server" meta:resourcekey="lblSuspensionMailBcc" Text="BCC:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtSuspensionMailBcc" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
	   				<asp:RegularExpressionValidator ID="suspensionMailBccRegExValidator" runat="server" ControlToValidate="txtSuspensionMailBcc" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblSuspensionMailSubject" runat="server" meta:resourcekey="lblSuspensionMailSubject" Text="Mail Subject:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtSuspensionMailSubject" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td colspan="2" class="SubHead" nowrap>
					<asp:Label ID="lblSuspensionMailBody" runat="server" meta:resourcekey="lblSuspensionMailBody" Text="Mail Body:"></asp:Label>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:TextBox ID="txtSuspensionMailBody" runat="server" Width="95%" CssClass="form-control" TextMode="MultiLine" Rows="10" MaxLength="1000"></asp:TextBox>
					<br />
					<asp:Label ID="lblSuspensionMailBodyHint" runat="server" meta:resourcekey="lblSuspensionMailBodyHint" Visible="true">
						([threshold], [date], [usage], [space], [customer] variables are supported)
					</asp:Label>
				</td>
			</tr>
		</table>
		
</asp:Panel>




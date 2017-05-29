<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotifyOverusedDatabases.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.NotifyOverusedDatabases" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>


<scp:CollapsiblePanel id="clpOverusageDefinitionHeader" runat="server"
    TargetControlID="pnlOverusageDefinition" resourcekey="clpOverusageDefinitionHeader" Text="Notify for">
</scp:CollapsiblePanel>
<asp:Panel ID="pnlOverusageDefinition" runat="server" CssClass="Normal">
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:CheckBox ID="cbxMSSQLOverused" runat="server" meta:resourcekey="cbxMSSQLOverused" Text="Microsoft SQL usage greater than defined below threshold" />
   				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:CheckBox ID="cbxMYSQLOverused" runat="server" meta:resourcekey="cbxMYSQLOverused" Text="MySQL usage greater than defined below threshold" />
				</td>
			</tr>
            <tr>
				<td class="SubHead" nowrap>
					<asp:CheckBox ID="cbxMARIADBOverused" runat="server" meta:resourcekey="cbxMARIADBOverused" Text="MariaDB usage greater than defined below threshold" />
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

<scp:CollapsiblePanel id="clpOverusedSettingsHeader" runat="server"
    TargetControlID="pnlOverusedSettings" resourcekey="clpOverusedSettingsHeader" Text="Issue Overused">
</scp:CollapsiblePanel>
<asp:Panel ID="pnlOverusedSettings" runat="server" CssClass="Normal">
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblWhenUsageThresholdExceeds" runat="server" meta:resourcekey="lblWhenUsageThresholdExceeds" Text="When usage exceeds"></asp:Label>
				</td>
   				<td class="SubHead" width="100%">
	   				<asp:TextBox ID="txtOverusedThreshold" runat="server" Width="30" CssClass="form-control" MaxLength="1000"></asp:TextBox>%
	   				<asp:CompareValidator ID="valOverusedThreshold" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="txtOverusedThreshold" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
   					<asp:Label ID="lblOverusedThreshold" runat="server" meta:resourcekey="lblWarningThreshold" Text="threshold."></asp:Label>
   				</td>
   			</tr>
   			<tr>
				<td class="SubHead" nowrap colspan="2">
					<asp:CheckBox ID="cbxDoSendOverused" runat="server" meta:resourcekey="cbxDoSendOverused" Text="Send email notification" />
   				</td>
			</tr>
		</table>
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblOverusedMailFrom" runat="server" meta:resourcekey="lblOverusedMailFrom" Text="Mail From:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtOverusedMailFrom" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
	   				<asp:RegularExpressionValidator ID="overusedMailFromRegExValidator" runat="server" ControlToValidate="txtOverusedMailFrom" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblOverusedMailBcc" runat="server" meta:resourcekey="lblOverusedMailBcc" Text="BCC:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtOverusedMailBcc" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
	   				<asp:RegularExpressionValidator ID="overusedMailBccRegExValidator" runat="server" ControlToValidate="txtOverusedMailBcc" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
				</td>
			</tr>
			<tr>
				<td class="SubHead" nowrap>
					<asp:Label ID="lblOverusedMailSubject" runat="server" meta:resourcekey="lblOverusedMailSubject" Text="Mail Subject:"></asp:Label>
				</td>
				<td class="Normal" width="100%">
   					<asp:TextBox ID="txtOverusedMailSubject" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td colspan="2" class="SubHead" nowrap>
					<asp:Label ID="lblOverusedMailBody" runat="server" meta:resourcekey="lblOverusedMailBody" Text="Mail Body:"></asp:Label>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:TextBox ID="txtOverusedMailBody" runat="server" Width="95%" CssClass="form-control" TextMode="MultiLine" Rows="10" MaxLength="1000"></asp:TextBox>
					<br />
					<asp:Label ID="lblOverusedMailBodyHint" runat="server" meta:resourcekey="lblOverusedMailBodyHint" Visible="true">
						([threshold], [date], [usage], [space], [customer] variables are supported)
					</asp:Label>
				</td>
			</tr>
		</table>
		
</asp:Panel>




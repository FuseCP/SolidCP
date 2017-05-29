<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailForwardingsEditForwarding.ascx.cs"
	Inherits="SolidCP.Portal.MailForwardingsEditForwarding" %>
<%@ Register TagPrefix="dnc" TagName="EmailAddress" Src="MailEditAddress.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<script type="text/javascript">

	function confirmation() {
		if (!confirm("Are you sure you want to delete this Mail Alias?")) return false; else ShowProgressDialog('Deleting Mail Alias...');
	}
</script>
<div class="panel-body form-horizontal">
	<dnc:EmailAddress id="emailAddress" runat="server">
	</dnc:EmailAddress>
	<table cellspacing="0" cellpadding="3" width="100%">
		<tr>
			<td class="SubHead" style="width: 150px;">
				<asp:Label ID="lblForwardsToEmail" runat="server" meta:resourcekey="lblForwardsToEmail"
					Text="Forwards to e-mail:"></asp:Label>
			</td>
			<td class="normal">
				<asp:TextBox ID="txtForwardTo" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
				<asp:RequiredFieldValidator ID="valtxtForwardTo" runat="server" ErrorMessage="*" meta:resourcekey="valRequireEmail" 
					ControlToValidate="txtForwardTo" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
	</table>
	<asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirmation();"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Saving Mail Alias...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
</div>

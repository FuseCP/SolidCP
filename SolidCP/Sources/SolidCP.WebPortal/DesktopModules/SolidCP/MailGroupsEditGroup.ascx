<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailGroupsEditGroup.ascx.cs" Inherits="SolidCP.Portal.MailGroupsEditGroup" %>
<%@ Register TagPrefix="dnc" TagName="EmailAddress" Src="MailEditAddress.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this Mail Group?")) return false; else ShowProgressDialog('Deleting Mail Group...');
}
</script>

<div class="panel-body form-horizontal">

    <dnc:EmailAddress id="emailAddress" runat="server"></dnc:EmailAddress>
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>

</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirmation();"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Saving Mail Group...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText" /> </CPCC:StyleButton>
</div>
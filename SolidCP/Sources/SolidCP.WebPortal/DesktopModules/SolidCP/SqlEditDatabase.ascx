<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlEditDatabase.ascx.cs" Inherits="SolidCP.Portal.SqlEditDatabase" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this Database?")) return false; else ShowProgressDialog('Deleting Database...');
}
</script>

<div class="panel-body form-horizontal">
	<table cellspacing="0" cellpadding="3" width="100%">
		<tr>
			<td class="SubHead" style="width: 150px;"><asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName" Text="Database name:"></asp:Label></td>
			<td class="NormalBold">
                <uc2:UsernameControl ID="usernameControl" runat="server" />
			</td>
		</tr>
        <tr>
            <td class="SubHead" style="width: 150px;"><asp:Label ID="lblDBInternalServer" runat="server" meta:resourcekey="lblDBInternalServer" Text="Internal Server:"></asp:Label></td>
			<td class="Normal"><asp:Literal ID="litDBInternalServer" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <td class="SubHead" style="width: 150px;"><asp:Label ID="lblDBExternalServer" runat="server" meta:resourcekey="lblDBExternalServer" Text="External Server:"></asp:Label></td>		
			<td class="Normal"><asp:Literal ID="litDBExternalServer" runat="server"></asp:Literal></td>
        </tr>
	</table>
	<br />
	
    <scp:CollapsiblePanel id="secUsers" runat="server"
        TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Database Users">
    </scp:CollapsiblePanel>
    <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
	    <table cellspacing="0" cellpadding="3" width="100%">
		    <tr>
			    <td colspan="2">
				    <asp:CheckBoxList ID="dlUsers" runat="server" CssClass="NormalBold" DataTextField="Name" DataValueField="Name"
					    RepeatColumns="2" CellPadding="3"></asp:CheckBoxList>
			    </td>
		    </tr>
	    </table>
	</asp:Panel>
	
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>

</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirmation();"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Saving Database...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
</div>
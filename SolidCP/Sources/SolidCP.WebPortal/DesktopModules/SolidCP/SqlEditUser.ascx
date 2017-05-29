<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlEditUser.ascx.cs" Inherits="SolidCP.Portal.SqlEditUser" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this User?")) return false; else ShowProgressDialog('Deleting User...');
}
</script>

<div class="panel-body form-horizontal">
    <table cellSpacing="0" cellPadding="3" width="100%">
        <tr>
            <td class="SubHead" style="width: 150px;"><asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label></td>
            <td class="NormalBold">
                <uc2:UsernameControl ID="usernameControl" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <uc3:PasswordControl ID="passwordControl" runat="server" />
	        </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
    <br />
    
    <scp:CollapsiblePanel id="secUsers" runat="server"
        TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Databases">
    </scp:CollapsiblePanel>
    <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
        <table id="tblDatabases" runat="server" cellSpacing="0" cellPadding="3" width="100%">
            <tr>
                <td colspan="2">
	                <asp:CheckBoxList id="dlDatabases" runat="server" CellPadding="3" RepeatColumns="2" CssClass="NormalBold"
                        DataTextField="Name" DataValueField="Name"></asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirmation();"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Saving User...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>&nbsp;
</div>
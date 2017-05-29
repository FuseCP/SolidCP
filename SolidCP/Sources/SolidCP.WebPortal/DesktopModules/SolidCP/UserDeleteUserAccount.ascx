<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDeleteUserAccount.ascx.cs" Inherits="SolidCP.Portal.UserDeleteUserAccount" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">
<table width="400">
	<tr>
		<td class="Normal">
			<asp:Label ID="lblAccountContains" runat="server" meta:resourcekey="lblAccountContains" Text="The account contains ..."></asp:Label>
			<br/>
			<br/>
			<asp:GridView ID="gvPackages" Runat="server" Width="100%" AutoGenerateColumns="False"
			    EmptyDataText="gvPackages"
			    CssSelectorClass="NormalGridView">
				<columns>
					<asp:BoundField DataField="PackageName" HeaderText="gvPackagesPackage"></asp:BoundField>
				</columns>
			</asp:GridView>
			<br/>
		</td>
	</tr>
	<tr>
		<td class="Normal">
		    <asp:Label ID="lblWarning" runat="server" meta:resourcekey="lblWarning" Text="Deleting this user also..."></asp:Label>
			<br/>
			<br/>
		</td>
	</tr>
	<tr>
		<td class="Normal">
			<asp:CheckBox ID="chkConfirm" Runat="server" meta:resourcekey="chkConfirm" Text="Yes, I understand ..."></asp:CheckBox>
			<br/>
		</td>
	</tr>
</table>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="ShowProgressDialog('Deleting...');"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditService.ascx.cs"
	Inherits="SolidCP.Portal.ServersEditService" %>
<%@ Register Src="GlobalDnsRecordsControl.ascx" TagName="GlobalDnsRecordsControl"
	TagPrefix="scp" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<script type="text/javascript">

	function confirmation() {
		if (!confirm("Are you sure you want to delete this Service?")) return false; else ShowProgressDialog('Deleting Service...');
	}
</script>
<div class="panel-body form-horizontal">
	<scp:ServerHeaderControl ID="ServerHeaderControl" runat="server" />
	<table cellspacing="0" cellpadding="2" width="100%">
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Label ID="lblGroup" runat="server" meta:resourcekey="lblGroup" Text="Group:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:Literal ID="litGroup" runat="server"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="SubHead" height="24">
				<asp:Label ID="lblProvider" runat="server" meta:resourcekey="lblProvider" Text="Provider:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:Literal ID="litProvider" runat="server"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblName" runat="server" meta:resourcekey="lblName" Text="Name:"></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="txtServiceName" runat="server" CssClass="form-control" Width="300px"
					MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="vldServiceNameRequired"
						runat="server" CssClass="NormalBold" ErrorMessage="Please enter service name"
						ControlToValidate="txtServiceName" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblComments" runat="server" meta:resourcekey="lblComments" Text="Comments:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:TextBox ID="txtComments" runat="server" Width="300px" CssClass="NormalTextBox"
					TextMode="MultiLine" Rows="3"></asp:TextBox>
			</td>
		</tr>
		<tr id="rowInstallResults" runat="server">
			<td class="NormalError" colspan="2">
				<asp:Label ID="lblServiceInstallationResults" runat="server" meta:resourcekey="lblServiceInstallationResults"
					Text="Service installation results:"></asp:Label>
				<asp:BulletedList ID="blInstallResults" runat="server">
				</asp:BulletedList>
			</td>
		</tr>
	</table>

    <h3><asp:Localize ID="SettingsHeader" runat="server" meta:resourcekey="SettingsHeader" Text="Settings"></asp:Localize></h3>
    <asp:PlaceHolder ID="serviceProps" runat="server"></asp:PlaceHolder>

    <scp:CollapsiblePanel id="DnsRecrodsHeader" runat="server" TargetControlID="DnsRecrodsPanel"
		resourcekey="DnsRecrodsHeader" Text="DNS Records Template">
	</scp:CollapsiblePanel>
	<asp:Panel ID="DnsRecrodsPanel" runat="server">
					<scp:GlobalDnsRecordsControl ID="GlobalDnsRecordsControl" runat="server" IPServerIDParam="ServerID" ServiceIdParam="ServiceID" />
	</asp:Panel>
	<asp:Panel ID="pnlQuota" runat="server" Visible="false">
		<table id="tblQuota" runat="server" width="100%" visible="false">
			<tr>
				<td width="200" nowrap class="SubHead">
					<asp:Label ID="lblQuotaName" runat="server"></asp:Label>
				</td>
				<td width="100%" class="Normal">
					<asp:TextBox ID="txtQuotaValue" runat="server" CssClass="NormalTextBox" Width="100px">0</asp:TextBox>
				</td>
			</tr>
		</table>
		<table id="tblCluster" runat="server" visible="false" width="100%">
			<tr>
				<td width="200" nowrap class="SubHead">
					<asp:Label ID="lblClusters" runat="server" meta:resourcekey="lblClusters"></asp:Label>
				</td>
				<td width="100%" class="Normal">
					<asp:DropDownList ID="ddlClusters" runat="server" Width="200px" CssClass="NormalTextBox"
						DataTextField="ClusterName" DataValueField="ClusterID">
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="valCluster" runat="server" ControlToValidate="ddlClusters"
						ErrorMessage="*" ValidationGroup="SelectCluster"></asp:RequiredFieldValidator>
					<CPCC:StyleButton ID="cmdDeleteCluster" runat="server" ValidationGroup="SelectCluster"
						OnClick="cmdDeleteCluster_Click">Delete Cluster</CPCC:StyleButton>
				</td>
			</tr>
			<tr>
				<td class="SubHead">
					<asp:Label ID="lblNewCluster" runat="server" meta:resourcekey="lblNewCluster"></asp:Label>
				</td>
				<td class="Normal">
					<asp:TextBox ID="txtClusterName" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
					<asp:RequiredFieldValidator ID="valNewCluster" runat="server" ControlToValidate="txtClusterName"
						ErrorMessage="*" ValidationGroup="NewCluster"></asp:RequiredFieldValidator>
					<CPCC:StyleButton ID="cmdAddCluster" runat="server" ValidationGroup="NewCluster" OnClick="cmdAddCluster_Click">Add Cluster</CPCC:StyleButton>
				</td>
			</tr>
		</table>
	</asp:Panel>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirmation();"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Updating Service...');"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/> </CPCC:StyleButton>
</div>

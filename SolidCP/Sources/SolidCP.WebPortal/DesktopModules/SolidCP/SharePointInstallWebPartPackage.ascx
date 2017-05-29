<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointInstallWebPartPackage.ascx.cs" Inherits="SolidCP.Portal.SharePointInstallWebPartPackage" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<div class="panel-body form-horizontal">
<table cellSpacing="0" cellPadding="5" width="100%">
	<tr>
		<td class="Huge" colspan="2"><asp:Literal id="litSiteName" runat="server"></asp:Literal></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead" vAlign="top" noWrap width="200"><asp:Label ID="lblRestoreFrom" runat="server" meta:resourcekey="lblRestoreFrom" Text="Restore From:"></asp:Label></td>
		<td class="normal" width="100%">
			<table width=100%>
				<tr>
					<td class="Normal"><asp:radiobutton id="radioUpload" meta:resourcekey="radioUpload" Checked="True" GroupName="media" Text="Uploaded File" Runat="server"
							AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged"></asp:radiobutton></td>
				</tr>
				<tr>
					<td class="Normal"><asp:radiobutton id="radioFile" meta:resourcekey="radioFile" GroupName="media" Text="Hosting Space File" Runat="server"
							AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged"></asp:radiobutton></td>
				</tr>
				<tr>
					<td class="Normal" id="cellUploadFile" runat="server">
						<table width=100%>
							<tr>
								<td>
                                    <asp:FileUpload ID="uploadFile" runat="server" Width="300px" /></td>
							</tr>
							<tr>
								<td class="Small" nowrap><asp:Label ID="lblAllowedFiles1" runat="server" meta:resourcekey="lblAllowedFiles" Text=".ZIP, .BAK files are allowed"></asp:Label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="Normal" id="cellFile" runat="server">
						<table width=100%>
							<tr>
								<td>
                                    <uc1:FileLookup ID="fileLookup" runat="server" Width="300" IncludeFiles="true" />
                                </td>
							</tr>
							<tr>
								<td class="Small" nowrap><asp:Label ID="lblAllowedFiles2" runat="server" meta:resourcekey="lblAllowedFiles" Text=".ZIP, .BAK files are allowed"></asp:Label></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<br/>
		</td>
	</tr>
</table>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnInstall" CssClass="btn btn-success" runat="server" OnClick="btnInstall_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInstall"/> </CPCC:StyleButton>
</div>
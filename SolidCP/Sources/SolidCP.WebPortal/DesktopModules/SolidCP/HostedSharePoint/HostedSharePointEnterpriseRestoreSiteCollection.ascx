<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostedSharePointEnterpriseRestoreSiteCollection.ascx.cs"
	Inherits="SolidCP.Portal.HostedSharePointEnterpriseRestoreSiteCollection" %>
<%@ Register Src="../UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
	TagPrefix="scp" %>	
6  
<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %> 
 

<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

		
	
<div id="ExchangeContainer">
	<div class="Module">
		<div class="Left">
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="SharePointSiteCollection48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="SharePoint Site Collections"></asp:Localize>
				</div>
				<div class="panel-body form-horizontal">
					<scp:SimpleMessageBox id="messageBox" runat="server" />
						<table cellspacing="0" cellpadding="5" width="100%">
							<tr>
								<td class="Huge" colspan="2">
									<asp:Literal ID="litSiteCollectionName" runat="server"></asp:Literal></td>
							</tr>
							<tr>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td class="SubHead" valign="top" nowrap width="200">
									<asp:Label ID="lblRestoreFrom" runat="server" meta:resourcekey="lblRestoreFrom" Text="Restore From:"></asp:Label></td>
								<td class="normal" width="100%">
									<table width="100%">
										<tr>
											<td class="Normal">
												<asp:RadioButton ID="radioUpload" meta:resourcekey="radioUpload" Checked="True" GroupName="media"
													Text="Uploaded File" runat="server" AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged">
												</asp:RadioButton></td>
										</tr>
										<tr>
											<td class="Normal">
												<asp:RadioButton ID="radioFile" meta:resourcekey="radioFile" GroupName="media" Text="Hosting Space File"
													runat="server" AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged">
												</asp:RadioButton></td>
										</tr>
										<tr>
											<td class="Normal" id="cellUploadFile" runat="server">
												<table width="100%">
													<tr>
														<td>
															<asp:FileUpload ID="uploadFile" runat="server" Width="300px" /></td>
													</tr>
													<tr>
														<td class="Small" nowrap>
															<asp:Label ID="lblAllowedFiles1" runat="server" meta:resourcekey="lblAllowedFiles"
																Text=".ZIP, .BAK files are allowed"></asp:Label></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td class="Normal" id="cellFile" runat="server">
												<table width="100%">
													<tr>
														<td>
															<uc1:FileLookup ID="fileLookup" runat="server" Width="300" IncludeFiles="true" />
														</td>
													</tr>
													<tr>
														<td class="Small" nowrap>
															<asp:Label ID="lblAllowedFiles2" runat="server" meta:resourcekey="lblAllowedFiles"
																Text=".ZIP, .BAK files are allowed"></asp:Label></td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
									<br />
								</td>
							</tr>
						</table>
					<div class="panel-footer text-right">
						<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnRestore" CssClass="btn btn-success" runat="server" OnClick="btnRestore_Click" OnClientClick="ShowProgressDialog('Restoring site collection...');"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRestoreText"/> </CPCC:StyleButton>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>

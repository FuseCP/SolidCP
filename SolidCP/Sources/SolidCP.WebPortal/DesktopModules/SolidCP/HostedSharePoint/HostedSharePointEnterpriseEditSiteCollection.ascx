<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostedSharePointEnterpriseEditSiteCollection.ascx.cs" Inherits="SolidCP.Portal.HostedSharePointEnterpriseEditSiteCollection" %>
<%@ Register Src="../ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="../UserControls/AllocatePackageIPAddresses.ascx" TagName="SiteUrlBuilder" TagPrefix="scp" %>	
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>	
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register src="../UserControls/QuotaEditor.ascx" tagname="QuotaEditor" tagprefix="uc1" %>
<%@ Register Src="../DomainsSelectDomainControl.ascx" TagName="DomainsSelectDomainControl" TagPrefix="uc1" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Left">
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title"> 
					<asp:Image ID="Image1" SkinID="SharePointSiteCollection48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="SharePoint Site Collection"></asp:Localize>
                    </div>
				<div class="panel-body form-horizontal">
					<scp:SimpleMessageBox id="localMessageBox" runat="server">
                    </scp:SimpleMessageBox>					
					<table id="tblEditItem" runat="server" cellspacing="0" cellpadding="5" width="100%">
						<tr id="rowUrl">
							<td class="SubHead" nowrap width="200">
								<asp:Label ID="lblSiteCollectionUrl" runat="server" meta:resourcekey="lblSiteCollectionUrl"
									Text="Url:"></asp:Label>
							</td>
							<td width="100%" class="NormalBold">
                                <div class="form-inline">
			                    <asp:TextBox ID="txtHostName" runat="server" CssClass="form-control" MaxLength="64"></asp:TextBox>.<uc1:DomainsSelectDomainControl ID="domain" runat="server" HideWebSites="false" HideDomainPointers="true" HideInstantAlias="true"/>
                                <asp:RequiredFieldValidator ID="valRequireHostName" runat="server" meta:resourcekey="valRequireHostName" ControlToValidate="txtHostName"
	                                ErrorMessage="Enter hostname" ValidationGroup="CreateSite" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="valRequireCorrectHostName" runat="server"
	                                ErrorMessage="Enter valid hostname" ControlToValidate="txtHostName" Display="Dynamic"
	                                meta:resourcekey="valRequireCorrectHostName" ValidationExpression="^([0-9a-zA-Z])*[0-9a-zA-Z]+$" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    </div>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionOwner" runat="server" meta:resourcekey="lblSiteCollectionOwner"
									Text="Owner:"></asp:Label>
							</td>
							<td class="Normal">
								<scp:UserSelector id="userSelector" IncludeMailboxes="true" runat="server"/>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionLocaleID" runat="server" meta:resourcekey="lblSiteCollectionLocaleID"
									Text="Locale ID:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:DropDownList ID="ddlLocaleID" runat="server" CssClass="form-control" DataTextField="DisplayName" DataValueField="LCID" ></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblMaxStorage" runat="server" meta:resourcekey="lblMaxStorage"></asp:Label>
							</td>
							<td class="Normal">																
                                <uc1:QuotaEditor ID="maxStorage" runat="server" CssClass="form-control" QuotaTypeId="2" />
                                
                                </td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblWarningStorage" runat="server" meta:resourcekey="lblWarningStorage"></asp:Label>
							</td>
							<td class="Normal">																
                                <uc1:QuotaEditor ID="warningStorage" runat="server" QuotaTypeId="2" CssClass="form-control"/>                                
                                </td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" Text="Title:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireTitle" runat="server" ErrorMessage="*"
									ControlToValidate="txtTitle" ValidationGroup="CreateSiteCollection"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblDescription" runat="server" meta:resourcekey="lblDescription" Text="Description:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"
									TextMode="MultiLine" Rows="5"></asp:TextBox>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
									ControlToValidate="txtDescription" ValidationGroup="CreateSiteCollection"></asp:RequiredFieldValidator>
							</td>
						</tr>
					</table>
					<table id="tblViewItem" runat="server" cellspacing="0" cellpadding="5" width="100%">
						<tr>
							<td class="SubHead" nowrap width="200">
								<asp:Label ID="lblSiteCollectionUrl2" runat="server" meta:resourcekey="lblSiteCollectionUrl"
									Text="Url:"></asp:Label>
							</td>
							<td width="100%" class="NormalBold">
								<span class="Huge">
								<asp:HyperLink runat="server" ID="lnkUrl" />								</span>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionOwner2" runat="server" meta:resourcekey="lblSiteCollectionOwner"
									Text="Owner:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litSiteCollectionOwner" runat="server"></asp:Literal>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionLocaleID2" runat="server" meta:resourcekey="lblSiteCollectionLocaleID"
									Text="Locale ID:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litLocaleID" runat="server"></asp:Literal>
							</td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblMaxStorageView" runat="server" meta:resourcekey="lblMaxStorage"
									Text="Limit site storage to a maximum of:"></asp:Label>
							</td>
							<td class="Normal">								
								<uc1:QuotaEditor ID="editMaxStorage" runat="server" QuotaTypeId="2" CssClass="form-control"/>                                
								
							</td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblWarningStorageView" runat="server" meta:resourcekey="lblWarningStorage"
									Text="Send warning E-mail when site storage reaches:"></asp:Label>
							</td>
							<td class="Normal">								
								<uc1:QuotaEditor ID="editWarningStorage" runat="server" QuotaTypeId="2" CssClass="form-control"/>                                
							</td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblTitle2" runat="server" meta:resourcekey="lblTitle" Text="Title:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litTitle" runat="server"></asp:Literal>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblDescription2" runat="server" meta:resourcekey="lblDescription"
									Text="Description:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litDescription" runat="server"></asp:Literal>
							</td>
						</tr>
					</table>
					<table width="100%">
						<tr>
							<td>
								<scp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true" TargetControlID="ToolsPanel"
									meta:resourcekey="secMainTools" Text="SharePoint Site Collection Tools">
								</scp:CollapsiblePanel>
								<asp:Panel ID="ToolsPanel" runat="server" Height="0" Style="overflow: hidden;">
									<table id="tblMaintenance" runat="server" cellpadding="10">
										<tr>
											<td>
												<asp:LinkButton ID="btnBackup" runat="server" meta:resourcekey="btnBackup" CausesValidation="false"
													Text="Backup Site Collection" CssClass="btn btn-primary" OnClick="btnBackup_Click" />
											</td>
										</tr>
										<tr>
											<td>
												<asp:LinkButton ID="btnRestore" runat="server" meta:resourcekey="btnRestore" CausesValidation="false"
													Text="Restore Site Collection" CssClass="btn btn-warning" OnClick="btnRestore_Click" />
											</td>
										</tr>
									</table>
								</asp:Panel>
							</td>
						</tr>
					</table>
					<div class="panel-footer text-right">
						<CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete Site?');"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
						<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
						<CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" ValidationGroup="CreateSiteCollection"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/> </CPCC:StyleButton>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>

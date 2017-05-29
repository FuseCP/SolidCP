<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsExchangeMailboxPlansPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsExchangeMailboxPlansPolicy" %>
<%@ Register Src="ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="ExchangeServer/UserControls/DaysBox.ascx" TagName="DaysBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/QuotaEditor.ascx" TagName="QuotaEditor" TagPrefix="uc1" %>
<%@ Import Namespace="SolidCP.Portal" %>

    <scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    <scp:SimpleMessageBox id="messageBox" runat="server" />
	<asp:GridView id="gvMailboxPlans" runat="server"  EnableViewState="true" AutoGenerateColumns="false"
		Width="100%" EmptyDataText="gvMailboxPlans" CssSelectorClass="NormalGridView" OnRowCommand="gvMailboxPlan_RowCommand" >
		<Columns>
            <asp:TemplateField HeaderText="Edit">
                <ItemTemplate>
                    <asp:ImageButton ID="cmdEdit" runat="server" SkinID="EditSmall" CommandName="EditItem" AlternateText="Edit record" CommandArgument='<%# Eval("MailboxPlanId") %>' ></asp:ImageButton>
                </ItemTemplate>
             </asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>							        
					<asp:Image ID="img2" runat="server" Width="16px" Height="16px" ImageUrl='<%# GetPlanType((int)Eval("MailboxPlanType")) %>' ImageAlign="AbsMiddle" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Policy">
				<ItemStyle Width="70%"></ItemStyle>
				<ItemTemplate>
					<asp:Label id="lnkDisplayMailboxPlan" runat="server" EnableViewState="true" ><%# PortalAntiXSS.Encode((string)Eval("MailboxPlan"))%></asp:Label>
                 </ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField >
				<ItemStyle Width="15%"></ItemStyle>
				<ItemTemplate>
				    &nbsp;<label>
				        <input type="radio" name="DefaultMailboxPlan" value='<%# Eval("MailboxPlanId") %>' <%# IsChecked((bool) Eval("IsDefault")) %>/>
                        <asp:Label runat="server" meta:resourcekey="lblDefaultMailboxPlan" Text="Default"></asp:Label>
				    </label>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					    <asp:LinkButton id="imgDelMailboxPlan" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("MailboxPlanId") %>' OnClientClick="return confirm('Are you sure you want to delete selected plan?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
                        <asp:LinkButton id="btnStamp" CssClass="btn btn-warning" runat="server" CommandName="RestampItem" CommandArgument='<%# Eval("MailboxPlanId") %>' OnClientClick="if (confirm('Restamp mailboxes with this plan.\n\nAre you sure you want to restamp the mailboxes ?')) ShowProgressDialog('Stamping mailboxes, this might take a while ...'); else return false;"> <i class="fa fa-clone">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnStampText"/> </asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
                        <asp:LinkButton id="btnStampUnassigned" CssClass="btn btn-primary" runat="server" CommandName="StampUnassigned" CommandArgument='<%# Eval("MailboxPlanId") %>' OnClientClick="if (confirm('Stamp unassigned mailboxes with this mailbox plan.\n\nAre you sure you want to continue with this ?')) ShowProgressDialog('Applying mailbox plans, this might take a while ...'); else return false;"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnStampUnassignedText"/> </asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateField>

		</Columns>
	</asp:GridView>
    <br />
	<div style="text-align: center">
		<CPCC:StyleButton id="btnSetDefaultMailboxPlan" CssClass="btn btn-success" runat="server" OnClick="btnSetDefaultMailboxPlan_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetDefaultMailboxPlanText"/> </CPCC:StyleButton>
    </div>
    	<scp:CollapsiblePanel id="secMailboxPlan" runat="server"
            TargetControlID="MailboxPlan" meta:resourcekey="secMailboxPlan" Text="Mailboxplan">
        </scp:CollapsiblePanel>
        <asp:Panel ID="MailboxPlan" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td class="FormLabel200" align="right">
									
					</td>
					<td>
						<asp:TextBox ID="txtMailboxPlan" runat="server"  CssClass="form-control" 
                            ontextchanged="txtMailboxPlan_TextChanged" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireMailboxPlan" runat="server" meta:resourcekey="valRequireMailboxPlan" ControlToValidate="txtMailboxPlan"
						ErrorMessage="Enter name" ValidationGroup="CreateMailboxPlan" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
					</td>
				</tr>
			</table>
			<br />
		</asp:Panel>

		<scp:CollapsiblePanel id="secMailboxFeatures" runat="server"
            TargetControlID="MailboxFeatures" meta:resourcekey="secMailboxFeatures" Text="Mailbox Features">
        </scp:CollapsiblePanel>
        <asp:Panel ID="MailboxFeatures" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td>
						<asp:CheckBox ID="chkPOP3" runat="server" meta:resourcekey="chkPOP3" Text="POP3"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:CheckBox ID="chkIMAP" runat="server" meta:resourcekey="chkIMAP" Text="IMAP"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:CheckBox ID="chkOWA" runat="server" meta:resourcekey="chkOWA" Text="OWA/HTTP"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:CheckBox ID="chkMAPI" runat="server" meta:resourcekey="chkMAPI" Text="MAPI"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:CheckBox ID="chkActiveSync" runat="server" meta:resourcekey="chkActiveSync" Text="ActiveSync"></asp:CheckBox>
					</td>
				</tr>
			</table>
			<br />
		</asp:Panel>

		<scp:CollapsiblePanel id="secMailboxGeneral" runat="server"
            TargetControlID="MailboxGeneral" meta:resourcekey="secMailboxGeneral" Text="Mailbox General">
        </scp:CollapsiblePanel>
        <asp:Panel ID="MailboxGeneral" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td>
						<asp:CheckBox ID="chkHideFromAddressBook" runat="server" meta:resourcekey="chkHideFromAddressBook" Text="Hide from Addressbook"></asp:CheckBox>
					</td>
				</tr>
			</table>
			<br />
		</asp:Panel>
				
		<scp:CollapsiblePanel id="secStorageQuotas" runat="server"
            TargetControlID="StorageQuotas" meta:resourcekey="secStorageQuotas" Text="Storage Quotas">
        </scp:CollapsiblePanel>
        <asp:Panel ID="StorageQuotas" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locMailboxSize" runat="server" meta:resourcekey="locMailboxSize" Text="Mailbox size:"></asp:Localize></td>
					<td>
                        <div class="Right">
                            <uc1:QuotaEditor id="mailboxSize" runat="server"
                                QuotaTypeID="2"
                                QuotaValue="0"
                                ParentQuotaValue="-1"></uc1:QuotaEditor>
                        </div>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locMaxRecipients" runat="server" meta:resourcekey="locMaxRecipients" Text="Maximum Recipients:"></asp:Localize></td>
					<td>
                        <div class="Right">
                            <uc1:QuotaEditor id="maxRecipients" runat="server"
                                QuotaTypeID="2"
                                QuotaValue="0"
                                ParentQuotaValue="-1"></uc1:QuotaEditor>
                        </div>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locMaxSendMessageSizeKB" runat="server" meta:resourcekey="locMaxSendMessageSizeKB" Text="Maximum Send Message Size (Kb):"></asp:Localize></td>
					<td>
                        <div class="Right">
                            <uc1:QuotaEditor id="maxSendMessageSizeKB" runat="server"
                                QuotaTypeID="2"
                                QuotaValue="0"
                                ParentQuotaValue="-1"></uc1:QuotaEditor>
                        </div>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locMaxReceiveMessageSizeKB" runat="server" meta:resourcekey="locMaxReceiveMessageSizeKB" Text="Maximum Receive Message Size (Kb):"></asp:Localize></td>
					<td>
                        <div class="Right">
                            <uc1:QuotaEditor id="maxReceiveMessageSizeKB" runat="server"
                                QuotaTypeID="2"
                                QuotaValue="0"
                                ParentQuotaValue="-1"></uc1:QuotaEditor>
                        </div>
					</td>
				</tr>

				<tr>
					<td class="FormLabel200" colspan="2"><asp:Localize ID="locWhenSizeExceeds" runat="server" meta:resourcekey="locWhenSizeExceeds" Text="When the mailbox size exceeds the indicated amount:"></asp:Localize></td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locIssueWarning" runat="server" meta:resourcekey="locIssueWarning" Text="Issue warning at:"></asp:Localize></td>
					<td>
						<scp:SizeBox id="sizeIssueWarning" runat="server" ValidationGroup="CreateMailboxPlan" DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="true" RequireValidatorEnabled="true"/>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locProhibitSend" runat="server" meta:resourcekey="locProhibitSend" Text="Prohibit send at:"></asp:Localize></td>
					<td>
						<scp:SizeBox id="sizeProhibitSend" runat="server" ValidationGroup="CreateMailboxPlan"  DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="true" RequireValidatorEnabled="true"/>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locProhibitSendReceive" runat="server" meta:resourcekey="locProhibitSendReceive" Text="Prohibit send and receive at:"></asp:Localize></td>
					<td>
						<scp:SizeBox id="sizeProhibitSendReceive" runat="server" ValidationGroup="CreateMailboxPlan" DisplayUnitsKB=false DisplayUnitsMB="false" DisplayUnitsPct="true" RequireValidatorEnabled="true"/>
					</td>
				</tr>
			</table>
			<br />
		</asp:Panel>
					
					
		<scp:CollapsiblePanel id="secDeleteRetention" runat="server" TargetControlID="DeleteRetention" meta:resourcekey="secDeleteRetention" Text="Delete Item Retention">
        </scp:CollapsiblePanel>
        <asp:Panel ID="DeleteRetention" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locKeepDeletedItems" runat="server" meta:resourcekey="locKeepDeletedItems" Text="Keep deleted items for:"></asp:Localize></td>
					<td>
						<scp:DaysBox id="daysKeepDeletedItems" runat="server" ValidationGroup="CreateMailboxPlan" RequireValidatorEnabled="true"/>
					</td>
				</tr>
			</table>
			<br />
		</asp:Panel>

		<scp:CollapsiblePanel id="secLitigationHold" runat="server"
            TargetControlID="LitigationHold" meta:resourcekey="secLitigationHold" Text="LitigationHold">
        </scp:CollapsiblePanel>
        <asp:Panel ID="LitigationHold" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td>
						<asp:CheckBox ID="chkEnableLitigationHold" runat="server" meta:resourcekey="chkEnableLitigationHold" Text="Enabled Litigation Hold"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locRecoverableItemsSpace" runat="server" meta:resourcekey="locRecoverableItemsSpace" Text="Recoverable Items Space (MB):"></asp:Localize></td>
					<td>
                            <uc1:QuotaEditor id="recoverableItemsSpace" runat="server"
                                QuotaTypeID="2"
                                QuotaValue="0"
                                ParentQuotaValue="-1"></uc1:QuotaEditor>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locRecoverableItemsWarning" runat="server" meta:resourcekey="locRecoverableItemsWarning" Text="Issue warning at:"></asp:Localize></td>
					<td>
						<scp:SizeBox id="recoverableItemsWarning" runat="server" ValidationGroup="CreateMailboxPlan" DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="true" RequireValidatorEnabled="true"/>
					</td>
				</tr>
                <tr>
                    <td class="FormLabel200" align="right"><asp:Label ID="lblLitigationHoldUrl" runat="server" meta:resourcekey="lblLitigationHoldUrl" Text="Url:"></asp:Label></td>
                    <td class="Normal">
                        <asp:TextBox ID="txtLitigationHoldUrl" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="FormLabel200" align="right"><asp:Label ID="lblLitigationHoldMsg" runat="server" meta:resourcekey="lblLitigationHoldMsg" Text="Page Content:"></asp:Label></td>
                    <td class="Normal" valign=top>
                        <asp:TextBox ID="txtLitigationHoldMsg" runat="server" Rows="10" TextMode="MultiLine" Width="100%" CssClass="form-control" Wrap="False" MaxLength="511"></asp:TextBox></td>
                </tr>

			</table>
		</asp:Panel>

		<scp:CollapsiblePanel id="secArchiving" runat="server"
            TargetControlID="Archiving" meta:resourcekey="secArchiving" Text="Archiving">
        </scp:CollapsiblePanel>
        <asp:Panel ID="Archiving" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td class="FormLabel200">
						<asp:CheckBox ID="chkEnableArchiving" runat="server" meta:resourcekey="chkEnableArchiving" Text="Archiving"></asp:CheckBox>
					</td>
                    <td></td>
				</tr>
				<tr id="rowArchiving">
					<td class="FormLabel200" align="right"><asp:Localize ID="locArchiveQuota" runat="server" meta:resourcekey="locArchiveQuota" Text="Archive quota:"></asp:Localize></td>
					<td>
                        <div class="Right">
                            <uc1:QuotaEditor id="archiveQuota" runat="server"
                                QuotaTypeID="2"
                                QuotaValue="0"
                                ParentQuotaValue="-1"></uc1:QuotaEditor>
                        </div>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locArchiveWarningQuota" runat="server" meta:resourcekey="locArchiveWarningQuota" Text="Archive warning quota:"></asp:Localize></td>
					<td>
						<scp:SizeBox id="archiveWarningQuota" runat="server" DisplayUnitsKB="false" DisplayUnitsMB="false" DisplayUnitsPct="true" />
					</td>
				</tr>

			</table>
			<br />
		</asp:Panel>

        <scp:CollapsiblePanel id="secRetentionPolicyTags" runat="server"
            TargetControlID="RetentionPolicyTags" meta:resourcekey="secRetentionPolicyTags" Text="Retention policy tags">
        </scp:CollapsiblePanel>
        <asp:Panel ID="RetentionPolicyTags" runat="server" Height="0" style="overflow:hidden;">
            <asp:UpdatePanel ID="GeneralUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                <asp:GridView id="gvPolicy" runat="server"  EnableViewState="true" AutoGenerateColumns="false"
		        Width="100%" EmptyDataText="" CssSelectorClass="NormalGridView" OnRowCommand="gvPolicy_RowCommand" >
		        <Columns>
			        <asp:TemplateField HeaderText="Tag">
				        <ItemStyle Width="70%"></ItemStyle>
				        <ItemTemplate>
					        <asp:Label id="displayPolicy" runat="server" EnableViewState="true" ><%# PortalAntiXSS.Encode((string)Eval("TagName"))%></asp:Label>
                        </ItemTemplate>
			        </asp:TemplateField>
                    <asp:TemplateField>
				        <ItemTemplate>
					        <CPCC:StyleButton id="imgDelPolicy" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("TagId") %>' OnClientClick="return confirm('Are you sure you want to delete selected policy tag?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
				        </ItemTemplate>
			        </asp:TemplateField>
		        </Columns>
	            </asp:GridView>
                <br />

                <asp:DropDownList ID="ddTags" runat ="server"></asp:DropDownList>
                <asp:Button ID="bntAddTag" runat="server" CssClass="btn btn-primary" Text="Add tag" meta:resourcekey="bntAddTag" OnClick="bntAddTag_Click" CausesValidation="false"/>
                <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>


    <table>
        <tr>
            <td>
                <div class="FormButtonsBarClean">
                    <CPCC:StyleButton id="btnUpdateMailboxPlan" CssClass="btn btn-warning" runat="server" OnClick="btnUpdateMailboxPlan_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateMailboxPlanText"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnAddMailboxPlan" CssClass="btn btn-success" runat="server" OnClick="btnAddMailboxPlan_Click"> <i class="fa fa-file-text-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddMailboxPlanText"/> </CPCC:StyleButton>
                </div>
            </td>
        </tr>
    </table>

    <br />

    <asp:TextBox ID="txtStatus" runat="server" CssClass="TextBox400" MaxLength="128" ReadOnly="true"></asp:TextBox>
    
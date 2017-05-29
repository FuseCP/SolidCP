<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsLyncUserPlansPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsLyncUserPlansPolicy" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

    <scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    <scp:SimpleMessageBox id="messageBox" runat="server" />
	<asp:GridView id="gvPlans" runat="server"  EnableViewState="true" AutoGenerateColumns="false"
		Width="100%" EmptyDataText="gvPlans" CssSelectorClass="NormalGridView" OnRowCommand="gvPlan_RowCommand" >
		<Columns>
            <asp:TemplateField HeaderText="gvPlanEdit">
                <ItemTemplate>
                    <asp:ImageButton ID="cmdEdit" runat="server" SkinID="EditSmall" CommandName="EditItem" AlternateText="Edit record" CommandArgument='<%# Eval("LyncUserPlanId") %>' ></asp:ImageButton>
                </ItemTemplate>
             </asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>							        
					<asp:Image ID="img2" runat="server" Width="16px" Height="16px" ImageUrl='<%# GetPlanType((int)Eval("LyncUserPlanType")) %>' ImageAlign="AbsMiddle" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="gvPlan">
				<ItemStyle Width="70%"></ItemStyle>
				<ItemTemplate>
					<asp:Label id="lnkDisplayPlan" runat="server" EnableViewState="true" ><%# Eval("LyncUserPlanName")%></asp:Label>
                 </ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<CPCC:StyleButton id="imgDelPlan" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("LyncUserPlanId") %>' OnClientClick="return confirm('Are you sure you want to delete selected plan?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
                        <CPCC:StyleButton id="btnStamp" CssClass="btn btn-warning" runat="server" CommandName="RestampItem" CommandArgument='<%# Eval("LyncUserPlanId") %>' OnClientClick="if (confirm('Restamp lync user with this plan.\n\nAre you sure you want to restamp the lync users ?')) ShowProgressDialog('Stamping lync users, this might take a while ...'); else return false;"> <i class="fa fa-clone">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnStampText"/> </CPCC:StyleButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />

					<scp:CollapsiblePanel id="secPlan" runat="server"
                        TargetControlID="Plan" meta:resourcekey="secPlan" Text="Plan">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="Plan" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel200" align="right">
									
								</td>
							    <td>
									<asp:TextBox ID="txtPlan" runat="server"  CssClass="form-control" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequirePlan" runat="server" meta:resourcekey="valRequirePlan" ControlToValidate="txtPlan"
									ErrorMessage="Enter plan name" ValidationGroup="CreatePlan" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
								</td>
						    </tr>
					    </table>
					    <br />
					</asp:Panel>

					<scp:CollapsiblePanel id="secPlanFeatures" runat="server"
                        TargetControlID="PlanFeatures" meta:resourcekey="secPlanFeatures" Text="Plan Features">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="PlanFeatures" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkIM" runat="server" meta:resourcekey="chkIM" Text="Instant Messaging"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkMobility" runat="server" meta:resourcekey="chkMobility" Text="Mobility"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkConferencing" runat="server" meta:resourcekey="chkConferencing" Text="Conferencing"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkEnterpriseVoice" runat="server" meta:resourcekey="chkEnterpriseVoice" Text="Enterprise Voice"></asp:CheckBox>
							    </td>
						    </tr>
						</table>
						<br />
					</asp:Panel>


					<scp:CollapsiblePanel id="secPlanFeaturesFederation" runat="server"
                        TargetControlID="PlanFeaturesFederation" meta:resourcekey="secPlanFeaturesFederation" Text="Federation">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="PlanFeaturesFederation" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkFederation" runat="server" meta:resourcekey="chkFederation" Text="Federation"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkRemoteUserAccess" runat="server" meta:resourcekey="chkRemoteUserAccess" Text="Remote User access"></asp:CheckBox>
							    </td>
						    </tr>
						</table>
						<br />
					</asp:Panel>

					<scp:CollapsiblePanel id="secPlanFeaturesArchiving" runat="server"
                        TargetControlID="PlanFeaturesArchiving" meta:resourcekey="secPlanFeaturesArchiving" Text="Archiving">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="PlanFeaturesArchiving" runat="server" Height="0" style="overflow:hidden;">
					    <table>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locArchivingPolicy" meta:resourcekey="locArchivingPolicy" Text="Archiving Policy:" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddArchivingPolicy" runat="server"  CssClass="form-control"></asp:DropDownList>
                                </td>
                            </tr>
						</table>
						<br />
					</asp:Panel>

					<scp:CollapsiblePanel id="secPlanFeaturesMeeting" runat="server"
                        TargetControlID="PlanFeaturesMeeting" meta:resourcekey="secPlanFeaturesMeeting" Text="Meeting">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="PlanFeaturesMeeting" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkAllowOrganizeMeetingsWithExternalAnonymous" runat="server" meta:resourcekey="chkAllowOrganizeMeetingsWithExternalAnonymous" Text="Allow organize meetings with external anonymous participants"></asp:CheckBox>
							    </td>
						    </tr>
						</table>
						<br />
					</asp:Panel>

					<scp:CollapsiblePanel id="secPlanFeaturesTelephony" runat="server"
                        TargetControlID="PlanFeaturesTelephony" meta:resourcekey="secPlanFeaturesTelephony" Text="Telephony">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="PlanFeaturesTelephony" runat="server" Height="0" style="overflow:hidden;">
					    <table>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locTelephony" meta:resourcekey="locTelephony" Text="Telephony :" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddTelephony"  CssClass="form-control" runat="server" AutoPostBack="True">
                                        <asp:ListItem Value="0" Text="Audio/Video disabled" meta:resourcekey="ddlTelephonyDisabled" />
                                        <asp:ListItem Value="1" Text="PC-to-PC only" meta:resourcekey="ddlTelephonyPCtoPCOnly" />
                                        <asp:ListItem Value="2" Text="Enterprise voice" meta:resourcekey="ddlTelephonyEnterpriseVoice" />
                                        <asp:ListItem Value="3" Text="Remote call control" meta:resourcekey="ddlTelephonyRemoteCallControl" />
                                        <asp:ListItem Value="4" Text="Remote call control only" meta:resourcekey="ddlTelephonyRemoteCallControlOnly" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>

                        <asp:Panel runat="server" ID="pnEnterpriseVoice">
                        <table>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locTelephonyProvider" meta:resourcekey="locTelephonyProvider" Text="Telephony Provider :" />
                                </td>
                                <td>
                                    <div class="input-group col-sm-6">
                                    <asp:TextBox ID="tbTelephoneProvider" CssClass="form-control" runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                    <asp:Button runat="server" ID="btnAccept" Text="Accept" CssClass="btn btn-primary" OnClick="btnAccept_Click" OnClientClick="ShowProgressDialog('Loading...');" ValidationGroup="Accept"/>
                                    </span>
                                    </div>
                                    <asp:RequiredFieldValidator id="AcceptRequiredValidator" runat="server" ErrorMessage="Please enter provider name"
                                    ControlToValidate="tbTelephoneProvider" Display="Dynamic" ValidationGroup="Accept" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locDialPlan" meta:resourcekey="locDialPlan" Text="Dial Plan :" />
                                </td>
                                <td>        
                                    <asp:DropDownList ID="ddTelephonyDialPlanPolicy" CssClass="form-control" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locVoicePolicy" meta:resourcekey="locVoicePolicy" Text="Voice Policy :" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddTelephonyVoicePolicy" CssClass="form-control" runat="server"></asp:DropDownList>
                                </td>
                            </tr>

                        </table>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnServerURI">
                        <table>
                            <tr>
                                <td class="FormLabel150">
                                    <asp:Localize runat="server" ID="locServerURI" meta:resourcekey="locServerURI" Text="Server URI :" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbServerURI" CssClass="form-control" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

					<br />
					</asp:Panel>


			
<%-- Disable because not used
	<scp:CollapsiblePanel id="secEnterpriseVoice" runat="server"
        TargetControlID="EnterpriseVoice" meta:resourcekey="secEnterpriseVoice" Text="Enterprise Voice Policy">
    </scp:CollapsiblePanel>
    <asp:Panel ID="EnterpriseVoice" runat="server" Height="0" style="overflow:hidden;">
		<table>
			<tr>
				<td>
					<asp:RadioButton ID="chkNone" groupName="VoicePolicy" runat="server" meta:resourcekey="chkNone" Text="None"></asp:RadioButton>
				</td>
			</tr>

			<tr>
				<td>
					<asp:RadioButton ID="chkEmergency" groupName="VoicePolicy" runat="server" meta:resourcekey="chkEmergency" Text="Emergency Calls"></asp:RadioButton>
				</td>
			</tr>
			<tr>
				<td>
					<asp:RadioButton ID="chkNational" groupName="VoicePolicy" runat="server" meta:resourcekey="chkNational" Text="National Calls"></asp:RadioButton>
				</td>
			</tr>
			<tr>
				<td>
					<asp:RadioButton ID="chkMobile" groupName="VoicePolicy" runat="server" meta:resourcekey="chkMobile" Text="Mobile Calls"></asp:RadioButton>
				</td>
			</tr>
			<tr>
				<td>
					<asp:RadioButton ID="chkInternational" groupName="VoicePolicy" runat="server" meta:resourcekey="chkInternational" Text="International Calls"></asp:RadioButton>
				</td>
			</tr>


		</table>
		<br />
	</asp:Panel>
    --%>
					
	<br />


    <table>
        <tr>
            <td>
                <div class="FormButtonsBarClean">
                        <CPCC:StyleButton id="btnUpdatePlan" CssClass="btn btn-warning" runat="server" OnClick="btnUpdatePlan_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdatePlanText"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnAddPlan" CssClass="btn btn-success" runat="server" OnClick="btnAddPlan_Click"> <i class="fa fa-file-text-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddPlanText"/> </CPCC:StyleButton>
                </div>
            </td>
        </tr>
    </table>

    <br />

    <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" MaxLength="128" ReadOnly="true"></asp:TextBox>


    



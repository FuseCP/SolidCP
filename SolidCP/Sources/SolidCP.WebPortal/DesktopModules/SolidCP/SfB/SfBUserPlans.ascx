<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBUserPlans.ascx.cs" Inherits="SolidCP.Portal.SfB.SfBUserPlans" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/SfBUserPlanSelector.ascx" TagName="SfBUserPlanSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
					<asp:Image ID="Image1" SkinID="SfBUserPlan48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Domain Names"></asp:Localize>
				</div>
 <div class="FormButtonsBar right">
     <CPCC:StyleButton id="btnAddPlan" CssClass="btn btn-primary" runat="server" OnClick="btnAddPlan_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddPlanText"/> </CPCC:StyleButton>
     
     </div>
					<scp:SimpleMessageBox id="messageBox" runat="server" />
				    <asp:GridView ID="gvPlans" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvPlans" CssSelectorClass="NormalGridView" OnRowCommand="gvPlan_RowCommand">
					    <Columns>
						    <asp:TemplateField>
							    <ItemTemplate>							        
								    <asp:Image ID="img2" runat="server" Width="16px" Height="16px" ImageUrl='<%# GetPlanType((int)Eval("SfBUserPlanType")) %>' ImageAlign="AbsMiddle" />
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvPlan">
							    <ItemStyle Width="70%"></ItemStyle>
							    <ItemTemplate>
								    <asp:hyperlink id="lnkDisplayPlan" runat="server" EnableViewState="false"
									    NavigateUrl='<%# GetPlanDisplayUrl(Eval("SfBUserPlanId").ToString()) %>'>
									    <%# Eval("SfBUserPlanName")%> 
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvPlanDefault">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <input type="radio" name="DefaultPlan" value='<%# Eval("SfBUserPlanId") %>' <%# IsChecked((bool)Eval("IsDefault")) %> />
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField>
							    <ItemTemplate>
									<CPCC:StyleButton id="imgDelMailboxPlan" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("SfBUserPlanId") %>' OnClientClick="return confirm('Are you sure you want to delete selected plan?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
		
				    <div class="panel-body text-right">
				        <CPCC:StyleButton id="btnSetDefaultPlan" CssClass="btn btn-success" runat="server" OnClick="btnSetDefaultPlan_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetDefaultPlanText"/> </CPCC:StyleButton>
                    </div>
				     <div class="panel-body text-right">
                    <scp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true" TargetControlID="ToolsPanel" meta:resourcekey="secMainTools" Text="SfB user plan maintenance">
					</scp:CollapsiblePanel>
					<asp:Panel ID="ToolsPanel" runat="server" Height="0" Style="overflow: hidden;" CssClass="panel panel-default">
                        <div class="panel-body">
						<table id="tblMaintenance" runat="server" cellpadding="10" >
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="lblSourcePlan" runat="server" meta:resourcekey="locSourcePlan" Text="Replace"></asp:Localize></td>
					            <td>                                
                                    <scp:SfBUserPlanSelector ID="sfbUserPlanSelectorSource" runat="server" AddNone="true"/>
                                </td>
					        </tr>
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="lblTargetPlan" runat="server" meta:resourcekey="locTargetPlan" Text="With"></asp:Localize></td>
					            <td>                                
                                    <scp:SfBUserPlanSelector ID="sfbUserPlanSelectorTarget" runat="server" AddNone="false"/>
                                </td>
					        </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" MaxLength="128" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
						</table>
                            </div>
				        <div class="panel-footer text-right">
					        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick = "ShowProgressDialog('Stamping mailboxes, this might take a while ...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
				        </div>


					</asp:Panel>
                          </div>


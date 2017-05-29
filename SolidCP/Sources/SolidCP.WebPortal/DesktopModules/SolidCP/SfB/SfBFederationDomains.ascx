<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBFederationDomains.ascx.cs" Inherits="SolidCP.Portal.SfB.SfBFederationDomains" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
					<asp:Image ID="Image1" SkinID="SfBFederationDomains48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Domain Names"></asp:Localize>
				</div>
  <div class="FormButtonsBar right">
                        <CPCC:StyleButton id="btnAddDomain" CssClass="btn btn-success" runat="server" OnClick="btnAddDomain_Click"> <i class="fa fa-globe">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddDomainText"/> </CPCC:StyleButton>
                    </div>
			
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
                  

				    <asp:GridView ID="gvDomains" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvDomains" CssSelectorClass="NormalGridView" OnRowCommand="gvDomains_RowCommand">
					    <Columns>
						    <asp:TemplateField HeaderText="gvDomainsName">
							    <ItemStyle Width="70%"></ItemStyle>
							    <ItemTemplate>
								    <asp:hyperlink id="lnkEditZone" runat="server" EnableViewState="false">
									    <%# Eval("DomainName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField>
							    <ItemTemplate>
									<CPCC:StyleButton id="imgDelDomain" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("DomainName") %>' OnClientClick="return confirm('Are you sure you want to delete selected domain?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>

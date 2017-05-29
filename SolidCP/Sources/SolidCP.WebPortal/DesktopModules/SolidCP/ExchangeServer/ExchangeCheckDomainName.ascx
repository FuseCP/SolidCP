<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeCheckDomainName.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeCheckDomainName" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeDomainName48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Domain Names"></asp:Localize>
					-
					<asp:Literal ID="litDomainName" runat="server"></asp:Literal>
                        </h3>
				</div>

				<asp:Literal ID="TopComments" runat="server" meta:resourcekey="TopComments"></asp:Literal>

				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    <br />

				    <asp:GridView ID="gvObjects" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" CssSelectorClass="NormalGridView" OnRowCommand="gvObjects_RowCommand">
					    <Columns>
						    <asp:TemplateField HeaderText="gvObjectsDisplayName">
							    <ItemStyle Width="40%"></ItemStyle>
							    <ItemTemplate>
							        <asp:Image ID="img1" runat="server" ImageUrl='<%# GetObjectImage(Eval("ObjectName").ToString(),(int)Eval("ObjectType")) %>' ImageAlign="AbsMiddle" />
								    <asp:hyperlink id="lnk1" runat="server"
									    NavigateUrl='<%# GetEditUrl(Eval("ObjectName").ToString(),(int)Eval("ObjectType"),Eval("ObjectID").ToString(),Eval("OwnerID").ToString()) %>'>
									    <%# Eval("DisplayName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvObjectsObjectType">
							    <ItemStyle Width="40%"></ItemStyle>
							    <ItemTemplate>							        
									<%# GetObjectType(Eval("ObjectName").ToString(),(int)Eval("ObjectType")) %>
							    </ItemTemplate>
						    </asp:TemplateField>

						    <asp:TemplateField HeaderText="gvObjectsView">
							    <ItemStyle Width="10%"></ItemStyle>
							    <ItemTemplate>	
								    <asp:hyperlink id="lnk2" runat="server"
									    NavigateUrl='<%# GetEditUrl(Eval("ObjectName").ToString(),(int)Eval("ObjectType"),Eval("ObjectID").ToString(),Eval("OwnerID").ToString()) %>'>
									    <asp:Literal id="lnkView" runat="server" Text="View" meta:resourcekey="lnkView" />
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>

						    <asp:TemplateField HeaderText="gvObjectsDelete">
							    <ItemStyle Width="10%"></ItemStyle>
							    <ItemTemplate>							        
                                    <CPCC:StyleButton id="lnkDelete" runat="server" Text="Delete" meta:resourcekey="lnkDelete" 
                                        OnClientClick="if(!confirm('Are you sure you want to delete ?')) return false; else ShowProgressDialog('Deleting ...');"
                                        CommandName="DeleteItem" CommandArgument='<%# Eval("OwnerID").ToString() + "," + Eval("ObjectType").ToString() + "," + Eval("DisplayName") %>'
                                        Visible='<%# AllowDelete(Eval("ObjectName").ToString(), (int)Eval("ObjectType")) %>' />
							    </ItemTemplate>
						    </asp:TemplateField>


					    </Columns>
				    </asp:GridView>


				    <br />
              				</div>
				    <div class="panel-footer">
                        <CPCC:StyleButton id="btnBack" CssClass="btn btn-warning" runat="server" OnClick="btnBack_Click"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBack"/> </CPCC:StyleButton>
				    </div>
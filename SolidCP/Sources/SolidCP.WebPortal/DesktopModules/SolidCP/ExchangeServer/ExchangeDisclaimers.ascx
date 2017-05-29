<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDisclaimers.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeDisclaimers" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeDisclaimers48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Lists"></asp:Localize>
                        </h3>
				</div>
				<div class="FormButtonsBar right">
                            <CPCC:StyleButton id="btnCreateList" CssClass="btn btn-primary" runat="server" OnClick="btnCreateList_Click"> <i class="fa fa-file-text-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateList"/> </CPCC:StyleButton>
                        </div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    <asp:GridView ID="gvLists" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvLists" CssSelectorClass="NormalGridView"
					    OnRowCommand="gvLists_RowCommand" AllowPaging="True" AllowSorting="True"
					    PageSize="20">
					    <Columns>
						    <asp:TemplateField HeaderText="gvListsDisplayName" SortExpression="DisplayName">
							    <ItemStyle Width="80%"></ItemStyle>
							    <ItemTemplate>
								    <asp:hyperlink id="lnk1" runat="server"
									    NavigateUrl='<%# GetListEditUrl(Eval("ExchangeDisclaimerId").ToString()) %>'>
									    <%# Eval("DisclaimerName")%>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField>
							    <ItemTemplate>
								    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("ExchangeDisclaimerId") %>' OnClientClick="return confirm('Remove this item?');"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
				    <br />
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Distribution Lists Created:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <scp:QuotaViewer ID="listsQuota" runat="server" QuotaTypeId="2" />
				    
				    
				</div>
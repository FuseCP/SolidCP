<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBUsers.ascx.cs" Inherits="SolidCP.Portal.SfB.SfBUsers" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />


                <div class="panel-heading">
                    <asp:Image ID="Image1" SkinID="SfBUser" runat="server" />
                    <asp:Localize ID="locTitle" meta:resourcekey="locTitle" runat="server" Text="SfB Users"></asp:Localize>
                </div>
                <div class="FormButtonsBar right">
                            <CPCC:StyleButton id="btnCreateUser" CssClass="btn btn-success" runat="server" OnClick="btnCreateUser_Click"> <i class="fa fa-user-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateUserText"/> </CPCC:StyleButton>
                </div>
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <div class="row">
                       
 
                            <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-6 col-md-offset-6 text-right form-inline">
                                   <div class="form-group">
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"    
                                onselectedindexchanged="ddlPageSize_SelectedIndexChanged">   
                                    <asp:ListItem>10</asp:ListItem>   
                                    <asp:ListItem Selected="True">20</asp:ListItem>   
                                    <asp:ListItem>50</asp:ListItem>   
                                    <asp:ListItem>100</asp:ListItem>   
                                </asp:DropDownList> 
                                           </div> 
                                          <div class="form-group">
                                <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
                                    <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                                    <asp:ListItem Value="UserPrincipalName" meta:resourcekey="ddlSearchColumnUserPrincipalName">Email</asp:ListItem>
                                </asp:DropDownList>
                                                  </div> 
                                <div class="form-group">
                                            <div class="input-group">
                                            <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control"></asp:TextBox>

                                                <div class="input-group-btn">
                                                    <CPCC:StyleButton
                                                        ID="cmdSearch"
                                                        runat="server"
                                                        CausesValidation="false"
                                                        CssClass="btn btn-primary"
                                                    >
                                                        <i class="fa fa-search" aria-hidden="true"></i>
                                                    </CPCC:StyleButton>      
                                                </div>     
                                           </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                        Width="100%" DataSourceID="odsAccountsPaged" EmptyDataText="gvUsers" CssSelectorClass="NormalGridView"
                        meta:resourcekey="gvUsers" AllowPaging="true" AllowSorting="true" OnRowCommand="gvUsers_RowCommand" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderText="gvUsersDisplayName" meta:resourcekey="gvUsersDisplayName"
                                SortExpression="DisplayName">
                                <ItemStyle Width="50%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage() %>' ImageAlign="AbsMiddle" />
                                    <asp:HyperLink ID="lnk1" runat="server" NavigateUrl='<%# GetUserEditUrl(Eval("AccountId").ToString()) %>'> 
									<%# Eval("DisplayName") %>
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvUsersLogin" SortExpression="UserPrincipalName">
							    <ItemStyle ></ItemStyle>
							    <ItemTemplate>							        
								    <asp:hyperlink id="lnk2" runat="server"
									    NavigateUrl='<%# GetOrganizationUserEditUrl(Eval("AccountId").ToString()) %>'>
									    <%# Eval("UserPrincipalName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
                            <asp:BoundField HeaderText="gvUsersEmail" meta:resourcekey="gvUsersEmail" DataField="SipAddress" SortExpression="SipAddress" ItemStyle-Width="25%" />
                            <asp:BoundField HeaderText="gvSfBUserPlan" meta:resourcekey="gvSfBUserPlan" DataField="SfBUserPlanName" SortExpression="SfBUserPlanName" ItemStyle-Width="25%" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("AccountId") %>' OnClientClick="return confirm('Remove this item?');"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetSfBUsersPagedCount"
                        SelectMethod="GetSfBUsersPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.SfBHelper">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
                            <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                <div class="panel-footer">
                    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Users Created:"></asp:Localize>
                    &nbsp;&nbsp;&nbsp;
                    <scp:QuotaViewer ID="usersQuota" runat="server" QuotaTypeId="2" />
                </div>

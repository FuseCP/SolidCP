<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationDeletedUsers.ascx.cs" Inherits="SolidCP.Portal.HostedSolution.OrganizationDeletedUsers" %>

<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Users"></asp:Localize>
                        </h3>
				</div>

				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarCleanRight">
                            <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"    
                                onselectedindexchanged="ddlPageSize_SelectedIndexChanged">   
                                    <asp:ListItem>10</asp:ListItem>   
                                    <asp:ListItem Selected="True">20</asp:ListItem>   
                                    <asp:ListItem>50</asp:ListItem>   
                                    <asp:ListItem>100</asp:ListItem>   
                                </asp:DropDownList>  

                                <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
                                    <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                                    <asp:ListItem Value="PrimaryEmailAddress" meta:resourcekey="ddlSearchColumnEmail">Email</asp:ListItem>
                                    <asp:ListItem Value="AccountName" meta:resourcekey="ddlSearchColumnAccountName">AccountName</asp:ListItem>
                                    <asp:ListItem Value="SubscriberNumber" meta:resourcekey="ddlSearchColumnSubscriberNumber">Account Number</asp:ListItem>
                                    <asp:ListItem Value="UserPrincipalName" meta:resourcekey="ddlSearchColumnUserPrincipalName">Login</asp:ListItem>
                                </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
		                            CausesValidation="false"/>
                            </asp:Panel>
                        </div>
                    </div>

				    <asp:GridView ID="gvDeletedUsers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvDeletedUsers" CssSelectorClass="NormalGridView"
					    OnRowCommand="gvDeletedUsers_RowCommand" AllowPaging="True" AllowSorting="True"
					    DataSourceID="odsAccountsPaged" PageSize="20">
					    <Columns>
						    <asp:TemplateField HeaderText="gvDeletedUsersDisplayName" SortExpression="DisplayName">
							    <ItemStyle Width="25%"></ItemStyle>
							    <ItemTemplate>							        
								    <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("OriginAT"),(bool)Eval("User.IsVIP")) %>' ImageAlign="AbsMiddle"/>
								    <asp:hyperlink id="lnk1" runat="server"
									    NavigateUrl='<%# GetUserEditUrl(Eval("AccountId").ToString()) %>'>
									    <%# Eval("User.DisplayName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
                            <asp:BoundField HeaderText="gvDeletedUsersLogin" DataField="User.UserPrincipalName" SortExpression="UserPrincipalName" ItemStyle-Width="25%" />
                            <asp:TemplateField HeaderText="gvServiceLevel">
                                <ItemStyle Width="25%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label id="lbServLevel" runat="server" ToolTip = '<%# GetServiceLevel((int)Eval("User.LevelId")).LevelDescription%>'>
                                        <%# GetServiceLevel((int)Eval("User.LevelId")).LevelName%>
                                    </asp:Label>
							    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="gvDeletedUsersEmail" DataField="User.PrimaryEmailAddress" SortExpression="PrimaryEmailAddress" ItemStyle-Width="25%" />                            
                            <asp:BoundField HeaderText="gvSubscriberNumber" DataField="User.SubscriberNumber" ItemStyle-Width="20%" />						    
						    <asp:TemplateField ItemStyle-Wrap="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Image2" runat="server" Width="16px" Height="16px" ToolTip="Mail" ImageUrl='<%# GetMailImage((int)Eval("OriginAT")) %>' CommandName="OpenMailProperties" CommandArgument='<%# Eval("AccountId") %>' Enabled=<%# EnableMailImageButton((int)Eval("OriginAT")) %>/>
                                    <asp:ImageButton ID="Image3" runat="server" Width="16px" Height="16px" ToolTip="UC" ImageUrl='<%# GetOCSImage((bool)Eval("User.IsOCSUser"),(bool)Eval("User.IsLyncUser"),(bool)Eval("User.IsSfBUser")) %>' CommandName="OpenUCProperties" CommandArgument='<%# GetOCSArgument((int)Eval("AccountId"),(bool)Eval("User.IsOCSUser"),(bool)Eval("User.IsLyncUser"),(bool)Eval("User.IsSfBUser")) %>' Enabled=<%# EnableOCSImageButton((bool)Eval("User.IsOCSUser"),(bool)Eval("User.IsLyncUser"),(bool)Eval("User.IsSfBUser")) %>/>
                                    <asp:ImageButton ID="Image4" runat="server" Width="16px" Height="16px" ToolTip="BlackBerry" ImageUrl='<%# GetBlackBerryImage((bool)Eval("User.IsBlackBerryUser")) %>' CommandName="OpenBlackBerryProperties" CommandArgument='<%# Eval("AccountId") %>' Enabled=<%# EnableBlackBerryImageButton((bool)Eval("User.IsBlackBerryUser")) %>/>
                                    <asp:Image ID="Image5" runat="server" Width="16px" Height="16px" ToolTip="CRM" ImageUrl='<%# GetCRMImage((Guid)Eval("User.CrmUserId")) %>'  />
                                </ItemTemplate>
						    </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
					                <asp:HyperLink ID="lnkDownload" runat="server" Visible='<%# !(bool)Eval("IsArchiveEmpty") %>'
						                NavigateUrl='<%# (bool)Eval("IsArchiveEmpty") ? "#" : GetDownloadLink((int)Eval("AccountId"), Eval("FileName").ToString()) %>'>
                                        <%# Eval("FileName") %>
					                </asp:HyperLink>
							    </ItemTemplate>
                            </asp:TemplateField>
						    <asp:TemplateField>
							    <ItemTemplate>
								    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("AccountId") %>' OnClientClick="return confirm('Remove this item?');"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
							    </ItemTemplate>
                            </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
					<asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True"
							SelectCountMethod="GetOrganizationDeletedUsersPagedCount"
							SelectMethod="GetOrganizationDeletedUsersPaged"
							SortParameterName="sortColumn"
							TypeName="SolidCP.Portal.OrganizationsHelper"
							OnSelected="odsAccountsPaged_Selected">
						<SelectParameters>
							<asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />							
							<asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
							<asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
						</SelectParameters>
					</asp:ObjectDataSource>
				    <br />
                    <div>
				        <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Users Created:"></asp:Localize>
				        &nbsp;&nbsp;&nbsp;
				        <scp:QuotaViewer ID="deletedUsersQuota" runat="server" QuotaTypeId="2" />
                    </div>
				</div>
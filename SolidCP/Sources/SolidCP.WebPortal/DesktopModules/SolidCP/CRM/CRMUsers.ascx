<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMUsers.ascx.cs" Inherits="SolidCP.Portal.CRM.CRMUsers" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
			<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="CRMLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" Text="CRM Users"></asp:Localize>
                </h3>
                        </div>
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarCleanLeft">
                            <CPCC:StyleButton id="btnCreateUser" CssClass="btn btn-success" runat="server" OnClick="btnCreateUser_Click"> <i class="fa fa-user-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateUser"/> </CPCC:StyleButton>
                        </div>
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
                                </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
		                            CausesValidation="false"/>
                            </asp:Panel>
                        </div>
                    </div>

				    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvUsers"  meta:resourcekey="gvUsers" CssSelectorClass="NormalGridView"
					     AllowPaging="True" AllowSorting="True" DataSourceID="odsAccountsPaged" PageSize="20">
					    <Columns>						     						   						    
						    <asp:TemplateField HeaderText="gvUsersDisplayName" SortExpression="DisplayName">
							    <ItemStyle Width="50%"></ItemStyle>
							    <ItemTemplate>							        
								    <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
								    <asp:hyperlink id="lnk1" runat="server"
									    NavigateUrl='<%# GetUserEditUrl(Eval("AccountId").ToString()) %>'>
									    <%# Eval("DisplayName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:BoundField HeaderText="gvUsersEmail" DataField="PrimaryEmailAddress" SortExpression="PrimaryEmailAddress" ItemStyle-Width="50%" />						   
					    </Columns>
				    </asp:GridView>
					<asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True"
							SelectCountMethod="GetCRMUsersPagedCount"
							SelectMethod="GetCRMUsersPaged"
							SortParameterName="sortColumn"
							TypeName="SolidCP.Portal.CrmHelper"
							OnSelected="odsAccountsPaged_Selected">
						<SelectParameters>
							<asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />							
							<asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
							<asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
						</SelectParameters>
					</asp:ObjectDataSource>
				    <br />

                    <asp:Panel ID="CRM2011Panel" runat="server">
                        <table>
                        <tr>
                                <td align="right" nowrap>
				                    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Full licenses :"></asp:Localize>
                                </td>
                                <td>
            				        <scp:QuotaViewer ID="usersQuota" runat="server" QuotaTypeId="2"   />
                                </td>
                        </tr>
                        <tr>
                                <td align="right" nowrap>
            				        <asp:Localize ID="locLimitedQuota" runat="server" meta:resourcekey="locLimitedQuota" Text="Limited licenses :"></asp:Localize>
                                </td>
                                <td>
				                    <scp:QuotaViewer ID="limitedusersQuota" runat="server" QuotaTypeId="2"   />
                                </td>
                        </tr>
                        <tr>
                                <td align="right" nowrap>
            				        <asp:Localize ID="locESSQuota" runat="server" meta:resourcekey="locESSQuota" Text="ESS licenses :"></asp:Localize>
                                </td>
                                <td>
				                    <scp:QuotaViewer ID="essusersQuota" runat="server" QuotaTypeId="2"   />
                                </td>
                        </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="CRM2013Panel" runat="server">
                        <table>
                        <tr>
                                <td align="right" nowrap>
				                    <asp:Localize ID="Localize1" runat="server" meta:resourcekey="locQuota" Text="Professional licenses :"></asp:Localize>
                                </td>
                                <td>
            				        <scp:QuotaViewer ID="professionalusersQuota" runat="server" QuotaTypeId="2"   />
                                </td>
                        </tr>
                        <tr>
                                <td align="right" nowrap>
            				        <asp:Localize ID="locBasicQuota" runat="server" meta:resourcekey="locBasicQuota" Text="Basic licenses :"></asp:Localize>
                                </td>
                                <td>
				                    <scp:QuotaViewer ID="basicusersQuota" runat="server" QuotaTypeId="2"   />
                                </td>
                        </tr>
                        <tr>
                                <td align="right" nowrap>
            				        <asp:Localize ID="locEssentialQuota" runat="server" meta:resourcekey="locEssentialQuota" Text="Essential licenses :"></asp:Localize>
                                </td>
                                <td>
				                    <scp:QuotaViewer ID="essentialusersQuota" runat="server" QuotaTypeId="2"   />
                                </td>
                        </tr>
                        </table>
                    </asp:Panel>


				</div>                   
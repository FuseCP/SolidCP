<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlackBerryUsers.ascx.cs"
    Inherits="SolidCP.Portal.BlackBerry.BlackBerryUsers" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div id="ExchangeContainer">
    <div class="Module">
        <div class="Left">
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="BlackBerryUsersLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarClean">
                            <div class="FormButtonsBarCleanLeft">
                                <CPCC:StyleButton id="btnCreateNewBlackBerryUser" CssClass="btn btn-success" runat="server" OnClick="btnCreateNewBlackBerryUser_Click"> <i class="fa fa-user-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateNewBlackBerryUser"/> </CPCC:StyleButton>
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
                                    </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton
                                        ID="cmdSearch" runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
                                        CausesValidation="false" />
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="FormButtonsBarCleanRight">
                            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                                Width="100%" CssSelectorClass="NormalGridView" 
                                DataSourceID="odsAccountsPaged" meta:resourcekey="gvUsers"
                                AllowPaging="true" AllowSorting="true" PageSize="20">
                                <Columns>
                                    <asp:TemplateField HeaderText="gvUsersDisplayName" SortExpression="DisplayName">
                                        <ItemStyle Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>'
                                                ImageAlign="AbsMiddle" />
                                            <asp:HyperLink ID="lnk1" runat="server" NavigateUrl='<%# GetUserEditUrl(Eval("AccountId").ToString()) %>'> 
									    <%# Eval("DisplayName") %>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="gvUsersEmail" DataField="PrimaryEmailAddress" SortExpression="PrimaryEmailAddress"
                                        ItemStyle-Width="50%" />
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetBlackBerryUsersPagedCount"
                                SelectMethod="GetBlackBerryUsersPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.BlackBerryHelper">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
                                    <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
                                    <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <br />
                            <div class="FormButtonsBarCleanLeft">
                            
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Users Created:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;				    
                            <scp:QuotaViewer ID="usersQuota" runat="server" QuotaTypeId="2"   />				    				    
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
            </div>
        </div>
    </div>
</div>

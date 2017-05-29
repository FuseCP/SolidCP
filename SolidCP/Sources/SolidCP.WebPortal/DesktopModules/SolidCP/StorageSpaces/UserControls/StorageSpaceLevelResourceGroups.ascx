<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StorageSpaceLevelResourceGroups.ascx.cs" Inherits="SolidCP.Portal.StorageSpaces.UserControls.StorageSpaceLevelResourceGroups" %>

<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../../UserControls/CollapsiblePanel.ascx" %>

<asp:UpdatePanel ID="ResourceGroupsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <div class="FormButtonsBarClean">
            <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
        </div>
        <asp:GridView ID="gvResourceGroups" runat="server" meta:resourcekey="gvResourceGroups" AutoGenerateColumns="False"
            Width="600px" CssSelectorClass="NormalGridView" 
            DataKeyNames="GroupId">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="10px" />
                </asp:TemplateField>
                <asp:TemplateField meta:resourcekey="gvResourceGroupsName" >
                    <ItemStyle Width="96%" Wrap="false" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Literal ID="litGroupName" runat="server" Text='<%# LocalizeGroup(Eval("GroupName").ToString()) %>'></asp:Literal>
                        <asp:HiddenField ID="hdnGroupId" runat="server" Value='<%# Eval("GroupId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />


        <asp:Panel ID="AddAccountsPanel" runat="server" Style="display: none">
            <div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-plus"></i> <asp:Localize ID="headerAddResourceGroups" runat="server" meta:resourcekey="headerAddResourceGroups"></asp:Localize></h3>
                   </div>
                    <div class="widget-content">
                    <asp:UpdatePanel ID="AddAccountsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>

                            <div class="FormButtonsBarClean">
                                <div class="FormButtonsBarCleanRight">
                                    <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                                        <asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox>
                                        <asp:ImageButton ID="cmdSearch" runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
                                            CausesValidation="false" OnClick="cmdSearch_Click" />
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="Popup-Scroll">
                                <asp:GridView ID="gvPopupResourceGroups" runat="server" meta:resourcekey="gvPopupResourceGroups" AutoGenerateColumns="False"
                                    Width="100%" CssSelectorClass="NormalGridView"
                                    DataKeyNames="GroupId">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="gvResourceGroupsName">
                                            <ItemStyle Width="50%" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Literal ID="litGroupName" runat="server" Text='<%# LocalizeGroup(Eval("GroupName").ToString()) %>'></asp:Literal>
                                                <asp:HiddenField ID="hdnGroupId" runat="server" Value='<%# Eval("GroupId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br /><br />
                    <CPCC:StyleButton id="btnCancelAdd" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnAddSelected" CssClass="btn btn-success" runat="server" OnClick="btnAddSelected_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddSelectedText"/> </CPCC:StyleButton>
                </div>
            </div>
        </asp:Panel>

        <asp:Button ID="btnAddAccountsFake" runat="server" Style="display: none;" />
        <ajaxToolkit:ModalPopupExtender ID="AddAccountsModal" runat="server"
            TargetControlID="btnAddAccountsFake" PopupControlID="AddAccountsPanel"
            BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelAdd" />

    </ContentTemplate>
</asp:UpdatePanel>



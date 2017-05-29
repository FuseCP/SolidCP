<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceStorageLevelsList.ascx.cs" Inherits="SolidCP.Portal.StorageSpaces.SpaceStorageLevelsList" %>

<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="../UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />

<asp:UpdatePanel runat="server" ID="messageBoxPanel" UpdateMode="Conditional">
    <ContentTemplate>
        <scp:SimpleMessageBox ID="messageBox" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<div class="FormButtonsBar right">
    <CPCC:StyleButton ID="btnAddSsLevel" runat="server" CssClass="btn btn-primary" OnClick="btnAddSsLevel_Click" >
        <i class="fa fa-plus">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddSsLevel"/>
    </CPCC:StyleButton>
</div>
<div class="panel-body form-horizontal">
    <div class="row">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-6 col-md-offset-6 text-right form-inline">
            <asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem Selected="True">20</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="input-group-btn">
                        <CPCC:StyleButton ID="cmdSearch" runat="server" CausesValidation="false" CssClass="btn btn-primary">
                            <i class="fa fa-search" aria-hidden="true"></i>
                        </CPCC:StyleButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</div>

<asp:ObjectDataSource ID="odsSsLevelsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetStorageSpaceLevelsPagedCount"
    SelectMethod="GetStorageSpaceLevelsPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.SsHelper" OnSelected="odsRDSServersPaged_Selected">
    <SelectParameters>
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:GridView ID="gvSsLevels" runat="server" AutoGenerateColumns="False"
    AllowPaging="True" AllowSorting="True"
    CssSelectorClass="NormalGridView"
    OnRowCommand="gvSsLevels_RowCommand"
    DataSourceID="odsSsLevelsPaged" EnableViewState="False"
    EmptyDataText="gvRDSServers">
    <Columns>
        <asp:TemplateField SortExpression="Name" HeaderText="Level name">
            <HeaderStyle Wrap="false" />
            <ItemStyle Wrap="False" Width="50%" />
            <ItemTemplate>
                <CPCC:StyleButton OnClientClick="ShowProgressDialog('Loading ...');return true;" CommandName="EditSsLevel" CommandArgument='<%# Eval("Id")%>' ID="lbEditSsLevel" runat="server" Text='<%#Eval("Name") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Description" HeaderText="Description">
            <ItemStyle Width="35%" />
        </asp:BoundField>
        <asp:TemplateField>
            <ItemTemplate>
                <CPCC:StyleButton ID="lnkRemove" runat="server" Text="Remove" Visible='<%# CheckLevelIsInUse(Utils.ParseInt(Eval("Id"), -1)) == false %>'
                    CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>'
                    meta:resourcekey="cmdDelete" OnClientClick="return confirm('Are you sure you want to delete selected storage space level?');"></CPCC:StyleButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
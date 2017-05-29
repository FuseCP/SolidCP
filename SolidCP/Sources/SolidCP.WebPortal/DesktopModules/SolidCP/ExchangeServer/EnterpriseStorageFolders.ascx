<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStorageFolders.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.EnterpriseStorageFolders" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<script type="text/javascript">
    //<![CDATA[
    $(document).ready(function () {
        setTimeout(getFolderData, 3000);
    });
    //]]>
</script>

<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="imgESS" SkinID="EnterpriseStorageSpace48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Shared Folders"></asp:Localize>
    </h3>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton id="btnAddFolder" CssClass="btn btn-success" runat="server" OnClick="btnAddFolder_Click">
        <i class="fa fa-folder-o">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddFolderText"/>
    </CPCC:StyleButton>
</div>
<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="row">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-6 col-md-offset-6 text-right form-inline">
            <asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlPageSize_SelectedIndexChanged">
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
<asp:HiddenField runat="server" ID="hdnGridState" Value="false" />
<asp:HiddenField runat="server" ID="hdnItemId"  />
<asp:GridView ID="gvFolders" runat="server" AutoGenerateColumns="False" EnableViewState="true" Width="100%"
    EmptyDataText="gvFolders" CssSelectorClass="NormalGridView" OnRowCommand="gvFolders_RowCommand" AllowPaging="True"
    AllowSorting="True" DataSourceID="odsEnterpriseFoldersPaged" PageSize="20">
    <Columns>
        <asp:TemplateField>
            <ItemStyle Width="0%" />
            <ItemTemplate>                        
                <asp:HiddenField ID="hdnFolderName" runat="server" Value='<%# Eval("Name") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvFolderName" SortExpression="Name">
            <ItemStyle Width="20%"></ItemStyle>
            <ItemTemplate>
                <asp:hyperlink id="lnkFolderName" runat="server" NavigateUrl='<%# GetFolderEditUrl(Eval("Name").ToString()) %>'>
                    <%# Eval("Name") %>
                </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvFolderQuota" SortExpression="FRSMQuotaGB">
            <ItemStyle Width="15%"></ItemStyle>
            <ItemTemplate>
                <asp:Literal id="litFolderQuota" runat="server" Text='<%# ConvertMBytesToGB(Eval("FRSMQuotaMB")).ToString() + " Gb" %>'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvFolderSize" SortExpression="Size">
            <ItemStyle Width="15%"></ItemStyle>
            <ItemTemplate>
                <asp:Literal id="litFolderSize" runat="server" Text='...'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvFolderUrl">
            <ItemStyle Width="40%"></ItemStyle>
            <ItemTemplate>
                <asp:Literal id="litFolderUrl" runat="server" Text='<%# (Eval("UncPath") ?? Eval("Url")).ToString() %>'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvMappedDrive">
            <ItemStyle Width="10%"></ItemStyle>
            <ItemTemplate>
                <asp:Image ID="img1" runat="server" ImageUrl='<%# GetDriveImage() %>' ImageAlign="AbsMiddle"  style="display:none"/>
                <asp:Literal id="litMappedDrive" runat="server" Text='...'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <CPCC:StyleButton id="imgDelFolder" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("Name") %>' OnClientClick="return confirm('Confirming Deletion will result in the deletion of all files on this share.')">
                    &nbsp;<i class="fa fa-trash-o"></i>
                    &nbsp;
                </CPCC:StyleButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsEnterpriseFoldersPaged" runat="server" EnablePaging="True" SelectCountMethod="GetEnterpriseFoldersPagedCount"
    SelectMethod="GetEnterpriseFoldersPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.EnterpriseStorageHelper">
    <SelectParameters>
        <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>
<div class="panel-footer">
    <asp:Localize ID="locQuotaFolders" runat="server" meta:resourcekey="locQuotaFolders" Text="Total Folders Allocated:"></asp:Localize>
    &nbsp;&nbsp;&nbsp;
    <scp:QuotaViewer ID="foldersQuota" runat="server" QuotaTypeId="2" />
    <br />
    <asp:Localize ID="locQuotaSpace" runat="server" meta:resourcekey="locQuotaSpace" Text="Total Space Allocated (Gb):"></asp:Localize>
    &nbsp;&nbsp;&nbsp;
    <scp:QuotaViewer ID="spaceQuota" runat="server" QuotaTypeId="3" />
    <br />
    <asp:Localize ID="locQuotaAvailableSpace" runat="server" meta:resourcekey="locQuotaAvailableSpace" Text="Used Diskspace (Mb):"></asp:Localize>
    &nbsp;&nbsp;&nbsp;
    <scp:QuotaViewer ID="spaceAvailableQuota" runat="server" QuotaTypeId="2" />
</div>

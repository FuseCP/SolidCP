<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedSSLEditFolder.ascx.cs" Inherits="SolidCP.Portal.SharedSSLEditFolder" %>
<%@ Register Src="WebSitesExtensionsControl.ascx" TagName="WebSitesExtensionsControl" TagPrefix="uc6" %>
<%@ Register Src="WebSitesCustomErrorsControl.ascx" TagName="WebSitesCustomErrorsControl" TagPrefix="uc4" %>
<%@ Register Src="WebSitesMimeTypesControl.ascx" TagName="WebSitesMimeTypesControl" TagPrefix="uc5" %>
<%@ Register Src="WebSitesHomeFolderControl.ascx" TagName="WebSitesHomeFolderControl" TagPrefix="uc1" %>
<%@ Register Src="WebSitesCustomHeadersControl.ascx" TagName="WebSitesCustomHeadersControl" TagPrefix="uc6" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this Shared SSL Folder?")) return false; else ShowProgressDialog('Deleting Shared SSL Folder...');	
}
</script>

<div class="panel-body form-horizontal">
    <div class="FormRow">
        <asp:HyperLink ID="lnkSiteName" runat="server" CssClass="Big" NavigateUrl="#" Target="_blank">domain.com/vdir</asp:HyperLink>
    </div>
    <div class="FormRow widget">
        <div class="widget-header clearfix">
        <table width="100%" cellpadding="0" cellspacing="1">
            <tr>
                <td class="Tabs">
                    <asp:DataList ID="dlTabs" runat="server" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="dlTabs_SelectedIndexChanged" RepeatLayout="Flow">
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <CPCC:StyleButton ID="cmdSelectTab" runat="server" CommandName="select" CssClass="Tab">
                                <%# Eval("Name") %>
                            </CPCC:StyleButton>
                        </ItemTemplate>
                        <SelectedItemStyle Wrap="False" />
                        <SelectedItemTemplate>
                            <CPCC:StyleButton ID="cmdSelectTab" runat="server" CommandName="select" CssClass="ActiveTab">
                                <%# Eval("Name") %>
                            </CPCC:StyleButton>
                        </SelectedItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
            </div>
        <div class="widget-content tab-content">
       <div class="panel-body form-horizontal">
           <asp:MultiView ID="tabs" runat="server" ActiveViewIndex="0">
            <asp:View ID="tabHomeFolder" runat="server">
                <uc1:WebSitesHomeFolderControl ID="webSitesHomeFolderControl" runat="server" IsAppVirtualDirectory="true" />
            </asp:View>
            
            <asp:View ID="tabExtensions" runat="server">
                <uc6:WebSitesExtensionsControl ID="webSitesExtensionsControl" runat="server" IsAppVirtualDirectory="true" />
            </asp:View>
            
            <asp:View ID="tabErrors" runat="server">
                <uc4:WebSitesCustomErrorsControl ID="webSitesCustomErrorsControl" runat="server" />
            </asp:View>
            
            <asp:View ID="tabHeaders" runat="server">
                <uc6:WebSitesCustomHeadersControl ID="webSitesCustomHeadersControl" runat="server" />
            </asp:View>
            
            <asp:View ID="tabMimes" runat="server">
                <uc5:WebSitesMimeTypesControl ID="webSitesMimeTypesControl" runat="server" />
            </asp:View>
            
           </asp:MultiView>
       </div>
            </div>
    </div>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="confirmation();"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" useSubmitBehavior="false" ValidationGroup="Server" OnclientClick="ShowProgressDialog('Saving Shared SSL Folder...')"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/> </CPCC:StyleButton>
</div>
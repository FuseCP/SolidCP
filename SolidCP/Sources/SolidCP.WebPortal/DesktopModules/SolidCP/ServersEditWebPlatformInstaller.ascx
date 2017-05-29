<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditWebPlatformInstaller.ascx.cs" Inherits="SolidCP.Portal.ServersEditWebPlatformInstaller" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Register Src="UserControls/EditFeedsList.ascx" TagName="EditFeedsList" TagPrefix="uc6" %>


<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server">
</uc1:ServerHeaderControl>
<%--<asp:Button ID="btnSettings" runat="server" Text="Settings" 
    onclick="btnSettings_Click" />
<asp:UpdatePanel ID="SettingsPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">

    <ContentTemplate>
      	<uc6:EditFeedsList id="feedsList" runat="server" DisplayNames="false" />
    
        <asp:Button ID="Button1" runat="server" Text="Save"  />    
    </ContentTemplate>
</asp:UpdatePanel>
--%>

<style>
ul.WPIKeywordList {
    padding: 0;
}
ul.WPIKeywordList>li {
    padding: .4em .4em;
    margin-right: .4em;
    line-height: 2em;
    display: inline;
    list-style-type: none;
}
ul.WPIKeywordList li .selected 
{
    padding: .4em .4em;
    background-color: #E5F2FF;
    border: solid 1px #86B9F7;
    font-weight: bold;
}
ul.WPIKeywordList input {
    display: none;
}

ul.WPIKeywordList label {
    padding: 0 .1em;
    cursor: pointer;
    text-decoration: underline;
    text-decoration-color: #888;
}
ul.WPIKeywordList li .selected label {
    text-decoration: none;
}
.NormalGridView .AspNet-GridView table {
    border-top: solid 1px #ccc;
}
.NormalGridView .AspNet-GridView h3 {
    padding: 0;
    margin: 0;
}

.AspNet-GridView-Pagination a,
.AspNet-GridView-Pagination span {
    font-size: 16px !important;
    padding: 0 3px !important;
}

h2.ProductTitle {
    cursor: pointer;
}
.NormalBold {
    font-size: 125%;
}

.panel-footer text-right {
    padding: 15px 10px;
}

.panel-footer text-right h2,
.panel-footer text-right p {
    margin: .2em 0;
}

.WpiInstallInfo span,
.WpiInstallInfo a {
    display: block;
}

.ProductsSearchInput {
    margin: 0;
    vertical-align: middle;
}
.ProgressAnimation {
    margin: .5em 1em .5em 0;    
}
.Failed {
    color: #c00;
}
</style>

<script type="text/javascript">
    Sys.Application.add_init(function () {
        $('h2.ProductTitle').live('click', function () {
            var $cell = $(this).parent();
            $('.ProductMoreInfo', $cell).show();
            $('.ProductDescription', $cell).show();
            $('.ProductSummary', $cell).hide();
        });
        $('.ProductsSearchInput').focus();
        $('.ProductsSearchInput').live("keypress", function (e) {
            /* ENTER PRESSED*/
            if (e.keyCode == 13) {
                e.stopPropagation();
                e.preventDefault();

                $('.ProductsSearchButton')[0].click();

                return false;
            }
        });
    });
</script>


<asp:Panel runat="server" ID="CheckLoadUserProfilePanel" Visible="False">
    <div class="MessageBox Yellow">
        To continue "Load User Profile" setting for the current application pool must be enabled. 
        <br/>
        Enable this setting now? (May require relogin)        
        <br/>
        <br/>
        <asp:Button runat="server" ID="EnableLoadUserProfileButton" Text="Yes" OnClick="EnableLoadUserProfileButton_OnClick"/>
    </div>
</asp:Panel>


<asp:Panel ID="SearchPanel" class="panel-body form-horizontal" runat="server" DefaultButton="ProductSearchGo">
    <div class="row">
	    <div class="col-md-9">
            <div class="input-group">
                <asp:RadioButtonList ID="keywordsList" runat="server" 
                        RepeatLayout="UnorderedList" CssClass="WPIKeywordList panel-body form-horizontal" 
                        onselectedindexchanged="keywordsList_SelectedIndexChanged" 
                        AutoPostBack="True">
                </asp:RadioButtonList>
            </div>
        </div>
	    <div class="col-md-3">
            <div class="form-group">
                <div class="input-group">
                    <asp:TextBox ID="searchBox" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="input-group-btn">
                        <CPCC:StyleButton ID="ProductSearchGo" runat="server" CssClass="btn btn-primary" OnClick="SearchButton_Click">
                            <i class="fa fa-search" aria-hidden="true"></i>
                        </CPCC:StyleButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<div runat="server" ID="InstallButtons2" class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel2" CssClass="btn btn-warning" runat="server" onclick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel2Text"/>
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton id="btnInstall2" CssClass="btn btn-success" runat="server" OnClick="btnInstall_Click">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnInstall2Text"/>
    </CPCC:StyleButton>
</div>

<asp:UpdatePanel ID="ProductsPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <Triggers>
        <asp:AsyncPostBackTrigger runat="server" ControlID="BackToGalleryButton" EventName="Click"/>
    </Triggers>

    <ContentTemplate>
        
        <asp:GridView ID="gvWpiProducts" runat="server" AutoGenerateColumns="False"
            EmptyDataText="Products not found"
            CssSelectorClass="NormalGridView" EnableViewState="true" 
            OnRowCommand="gvWpiProducts_RowCommand"
            ShowHeader="False" 
            AllowPaging="True" onpageindexchanging="gvWpiProducts_PageIndexChanging"
            >
            <Columns>
                
                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="gvWPILogo" ItemStyle-Width="5%"  >
                    <ItemTemplate>
                        <asp:Image ID="icoLogo" runat="server" SkinID="Dvd48" src='<%# FixDefaultLogo((string)Eval("Logo")) %>' Width="48" Height="48" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Wrap="true" HeaderText="gvServicesName" ItemStyle-Width="85%">
                    <ItemTemplate>
                    <h2 class="ProductTitle Huge"><%# Eval("Title")%></h2>
                        <p class="ProductSummary"><%# Eval("Summary") %></p>
                        <p style="display: none" class="ProductDescription"><%# Eval("LongDescription")%></p>
                        
                        <p>
                        <asp:HyperLink ID="productLink" runat="server" NavigateUrl='<%# Eval("Link")%>' Target="_blank">More information</asp:HyperLink>
                        &nbsp;&middot;&nbsp;
                        <span>Version: <%# Eval("Version")%></span>
                        &nbsp;&middot;&nbsp;
                        <span>Published: <%# ((DateTime)Eval("Published")).ToShortDateString() %></span>
                        </p>
                        <p class="ProductMoreInfo" style="display: none">
                            Publisher: <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("AuthorUri")%>' Target="_blank"><%# Eval("Author")%></asp:HyperLink>
                            &nbsp;&middot;&nbsp;
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("EulaUrl")%>' Target="_blank">License terms</asp:HyperLink>
                        </p>
                        </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="gvInstall" ItemStyle-Width="10%">
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>

                    <asp:Label ID="LabelInstalled" runat="server" Text="installed" Visible='<%#  Eval("IsInstalled") %>'></asp:Label>
                                            
                    <asp:Button ID="btnAdd" runat="server"
                    CssClass='Button1'
                    Visible = '<%#  !(bool)Eval("IsInstalled") %>'
                    Text = '<%# IsAddedText((string)Eval("ProductID")) %>'
			        CommandArgument='<%# Eval("ProductID") %>'
			        CommandName="WpiAdd" 
                     />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

    </ContentTemplate>
    
</asp:UpdatePanel>

<div runat="server" ID="InstallButtons1" class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel1" CssClass="btn btn-warning" runat="server" onclick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel1Text"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnInstall1" CssClass="btn btn-success" runat="server" OnClick="btnInstall_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInstall1Text"/> </CPCC:StyleButton>
</div>

<asp:Panel ID="NoProductsSelectedPanel" runat="server" Visible="False">
    <div class="panel-body form-horizontal">
    <h2>No products selected to install</h2>
    <br/>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="NoProductsBackButton" CssClass="btn btn-warning" runat="server" OnClick="NoProductsBackButtonClick"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="NoProductsBackButtonText"/> </CPCC:StyleButton>
        <br/>
    </div>
</asp:Panel>

<asp:UpdatePanel ID="InstallPanel" runat="server" Visible="False" UpdateMode="Conditional" >
    <ContentTemplate>
        <div class="panel-body form-horizontal">
            <h2 class="NormalBold">Products to be installed</h2>
            <p>Review the following list of software to be installed and Windows components to be turned on.</p>
        </div>

		<asp:GridView ID="gvWpiInstall" runat="server" AutoGenerateColumns="False" AllowPaging="false" 
            EmptyDataText="No products selected to install"
            CssSelectorClass="NormalGridView" EnableViewState="true" 
            onrowdatabound="gvWpiInstall_RowDataBound"
            ShowHeader="False">
            <Columns>
              
                <asp:TemplateField ItemStyle-Wrap="true" HeaderText="gvServicesName" ItemStyle-Width="95%"  >
                    <ItemTemplate>
                    <h3><%# Eval("Title")%></h3>
                    <p class="WpiInstallInfo">
                        <asp:HyperLink ID="eulaLink" runat="server" NavigateUrl='<%# Eval("EulaUrl")%>' Target="_blank">License Agreement</asp:HyperLink>
                        <asp:HyperLink ID="productLink" runat="server" NavigateUrl='<%# Eval("DownloadedLocation")%>'  Target="_blank"><%# Eval("DownloadedLocation")%></asp:HyperLink>
                        <asp:Label ID="downloadSize" runat="server" Text="Download size:"></asp:Label> 
                    </p>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>

<div runat="server" ID="AceptDEclineButtons" Visible="False" class="panel-footer text-right">
    <p>
        Warning: During product installation your IIS process might be restarted depending on the package installed, 
        causing all users connected to the control panel to lose their session, requiring them to re-login after completion. 
        Please assure that no other users are processing requests before you start this process as it might cause corruption 
        if they are in the middle of an update.
    </p>
    <br/>
    <p>
        By clicking "I Accept", you agree to the license terms for the software listed above. If you do not agree
        to all of the license terms, click "I Decline".
    </p>
    <br/>
    <CPCC:StyleButton id="btnDecline" CssClass="btn btn-danger" runat="server" OnClick="btnDecline_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeclineText"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel3" CssClass="btn btn-warning" runat="server" onclick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel3Text"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnAccept" CssClass="btn btn-success" runat="server" OnClick="btnAccept_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAcceptText"/> </CPCC:StyleButton>
</div>

<asp:UpdatePanel ID="ProgressPanel" runat="server" Visible="False" UpdateMode="Conditional" >
    <ContentTemplate>
        <asp:Timer ID="ProgressTimer" runat="server" Enabled="False" Interval="3000" OnTick="ProgressTimerTick"></asp:Timer>
        <asp:Panel ID="ProgressMessagePanel" class="panel-body form-horizontal" runat="server">
            <h3 class="NormalBold">Selected products are being installed now:</h3>
            <br/>
            <asp:Image runat="server" ID="ProgressAnimation" ImageAlign="AbsMiddle" ImageUrl="" CssClass="ProgressAnimation"></asp:Image>
            <asp:Label ID="ProgressMessage" runat="server">initializing...</asp:Label>
            
            <br/>
            <br/>
            <CPCC:StyleButton id="CancelInstall" CssClass="btn btn-warning" runat="server" OnClick="CacnelInstallButtonClick" OnClientClick="return confirm('Are you sure to cancel installation?')"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelInstallText"/> </CPCC:StyleButton>
            <br/>
            <br/>
        </asp:Panel>
        <asp:Panel ID="InstallCompletedPanel" runat="server" Visible="False">
            <div class="panel-body form-horizontal">
                <p><asp:Label ID="LabelInstallationSuccess" runat="server" CssClass="NormalBold">Selected products are successfully installed</asp:Label>
                <asp:BulletedList ID="InstalledProductsList" runat="server" Visible="False">
                </asp:BulletedList>
                </p>
                <p><asp:Label ID="LabelInstallationFailed"  runat="server" Visible="False" CssClass="Failed">Some products installation are failed</asp:Label></p>
                <br/>
                <br/>
                <CPCC:StyleButton id="ShowLogsButton" CssClass="btn btn-success" runat="server" OnClick="ShowLogsButton_OnClick" Visible="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnShowLogsText"/> </CPCC:StyleButton>
                <asp:Panel runat="server" ID="WpiLogsPanel" Visible="False">
                    <pre runat="server" ID="WpiLogsPre" style="word-wrap: break-word;"></pre>
                </asp:Panel>
                <br/>
                <br/>
            </div>
            <div class="panel-footer text-right">
                <CPCC:StyleButton id="BackToGalleryButton" CssClass="btn btn-warning" runat="server" OnClick="BackToGalleryButtonClick"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBackToGalleryText"/> </CPCC:StyleButton>&nbsp;
                <CPCC:StyleButton id="btnBackToServer" CssClass="btn btn-warning" runat="server" OnClick="btnCancel_Click"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBackToServerText"/> </CPCC:StyleButton>
            </div>

        </asp:Panel>
    </ContentTemplate>
    
</asp:UpdatePanel>



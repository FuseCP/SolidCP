<%@ Control AutoEventWireup="true" %>
<%@ Register TagPrefix="scp" TagName="SiteFooter" Src="~/DesktopModules/SolidCP/SkinControls/SiteFooter.ascx" %>
<%@ Register TagPrefix="scp" TagName="TopMenu" Src="~/DesktopModules/SolidCP/SkinControls/TopMenu.ascx" %>
<%@ Register  TagPrefix="scp" TagName="Logo" Src="~/DesktopModules/SolidCP/SkinControls/Logo.ascx" %>
<%@ Register  TagPrefix="scp" TagName="SignedInUser" Src="~/DesktopModules/SolidCP/SkinControls/SignedInUser.ascx" %>
<%@ Register  TagPrefix="scp" TagName="GlobalSearch" Src="~/DesktopModules/SolidCP/SkinControls/GlobalSearch.ascx" %>
<%@ Register TagPrefix="scp" TagName="GlobalSearchTop" Src="~/DesktopModules/SolidCP/SkinControls/GlobalSearch.ascx" %>
<%@ Register  TagPrefix="scp" TagName="UserSpaceBreadcrumb" Src="~/DesktopModules/SolidCP/SkinControls/UserSpaceBreadcrumb.ascx" %>

<asp:ScriptManager ID="scriptManager" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    <Services>
        <asp:ServiceReference Path="~/DesktopModules/SolidCP/TaskManager.asmx" />
    </Services>
</asp:ScriptManager>
	<!-- WRAPPER -->
	<div class="wrapper" id="SkinOutline">
        <nav class="top-bar navbar-fixed-top" role="navigation">
            <div class="search-top" style="display:none;">
                <scp:GlobalSearch ID="GlobalSearchTop" runat="server" />
            </div>
			<div class="logo-area">
				<a class="btn btn-link btn-off-canvas pull-left"><i class="icon ion-navicon"></i></a>
				<div class="logo pull-left">
                    <scp:Logo ID="logo" runat="server" />
				</div>
			</div>
            <scp:SignedInUser ID="signedInUser1" runat="server" />
            <div class="hidden-xs">
            <scp:GlobalSearch ID="globalSearch" runat="server" />
            </div>
		</nav>
		<!-- END TOP NAV BAR -->
        <!-- COLUMN LEFT -->
		<div id="col-left" class="col-left">
			<div class="main-nav-wrapper">
				<nav id="main-nav" class="main-nav">
					<a href="#" id="btn-nav-sidebar-minified" class="btn btn-link btn-nav-sidebar-minified"><i class="icon ion-arrow-swap"></i></a>
                    <h3>
                   	Main Menu</h3>
                      <scp:TopMenu ID="leftMenu" runat="server" Align="left" />
            <asp:PlaceHolder ID="LeftPane" runat="server"></asp:PlaceHolder>
            <scp:TopMenu ID="rightMenu" runat="server" Align="right" />
				</nav>
			</div>
		</div>
		<!-- END COLUMN LEFT -->
        <!-- COLUMN RIGHT -->
		<div id="col-right" class="col-right ">
			<div class="container-fluid primary-content" id="SkinContent">
				<!-- PRIMARY CONTENT HEADING -->
				<div class="primary-content-heading clearfix">
					<scp:UserSpaceBreadcrumb ID="breadcrumb" runat="server"/>
        <div id="ContentOneColumn" class="row">
            <div id="Center" class="col-md-12">
                <asp:PlaceHolder ID="ContentPane" runat="server"></asp:PlaceHolder>
            </div>
        </div>
                    </div>
                				<!-- END PRIMARY CONTENT HEADING -->
				<div class="widget widget-no-header widget-transparent bottom-30px">
                    </div>
                </div>
    <div id="Footer" class="content-footer">
        <scp:SiteFooter ID="footer" runat="server" />
    </div>
            </div>
            </div>
	<!-- Javascript -->
	<script src="/JavaScript/jquery-2.1.0.min.js"></script>
	<script src="/JavaScript/bootstrap/bootstrap.js"></script>
	<script src="/JavaScript/scp-common.js"></script>
	<script src="/JavaScript/scp-charts.js"></script>
	<script src="/JavaScript/scp-elements.js"></script>
    <script src="/JavaScript/plugins/plugins.js"></script>
    <script src="/JavaScript/jquery-ui/jquery-ui-1.10.4.custom.min.js"></script>
	<script src="/JavaScript/jquery.matchHeight.js"></script>
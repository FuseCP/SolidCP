<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsTools.ascx.cs" Inherits="SolidCP.Portal.VPSForPC.VpsDetailsTools" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>


	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Server48" runat="server" />
				    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Tools" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_tools" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    <table cellspacing="15">
				        <%-- <tr>
				            <td>
				                <CPCC:StyleButton id="btnReinstall" CssClass="btn btn-success" runat="server" CausesValidation="false" onclick="btnReinstall_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnReinstallText"/> </CPCC:StyleButton>
				            </td>
				            <td>
				                <asp:Localize ID="locReinstall" runat="server" meta:resourcekey="locReinstall" Text="Performs..."></asp:Localize>
				            </td>
				        </tr>--%>
				        <tr>
				            <td>
				                <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="false" onclick="btnDelete_Click"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>
				            </td>
				            <td>
				                <asp:Localize ID="locDelete" runat="server" meta:resourcekey="locDelete" Text="Performs..."></asp:Localize>
				            </td>
				        </tr>
				    </table>
			    </div>
		    </div>
            </div>
            </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsCheckPoints.ascx.cs" Inherits="SolidCP.Portal.VPSForPC.VpsCheckPoints" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>

	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Monitoring48" runat="server" />
                    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Snapshots" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_checkpoints" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <asp:TreeView runat="server" ID="treeCheckPoints"></asp:TreeView>
                <div class="FormButtonsBar" >
                    <CPCC:StyleButton id="btnRestoreCheckPoint" CssClass="btn btn-warning" runat="server" OnClick="btnRestoreCheckPoint_Click" CausesValidation="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRestoreText"/> </CPCC:StyleButton>&nbsp;        
                    <CPCC:StyleButton id="btnCreateCheckPoint" CssClass="btn btn-success" runat="server" OnClick="btnCreateCheckPoint_Click" CausesValidation="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
                </div> 
            </div>
                    </div>
                    </div>
            </div>
	        <div class="Right">
		        <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
	        </div>
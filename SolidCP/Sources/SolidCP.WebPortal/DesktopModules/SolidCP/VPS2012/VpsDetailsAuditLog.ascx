<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsAuditLog.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsAuditLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/AuditLogControl.ascx" TagName="AuditLogControl" TagPrefix="scp" %>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_audit_log" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    <scp:AuditLogControl id="auditLog" runat="server" LogSource="VPS2012" />
				    
			    </div>
		    </div>
	    </div>

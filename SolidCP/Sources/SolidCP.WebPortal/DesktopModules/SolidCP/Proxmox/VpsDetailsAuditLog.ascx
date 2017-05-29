<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsAuditLog.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsAuditLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/AuditLogControl.ascx" TagName="AuditLogControl" TagPrefix="wsp" %>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_audit_log" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    <wsp:AuditLogControl id="auditLog" runat="server" LogSource="Proxmox" />
				    
			    </div>
		    </div>
	    </div>

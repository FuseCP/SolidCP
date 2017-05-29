<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAuditLog.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcAuditLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Register Src="../UserControls/AuditLogControl.ascx" TagName="AuditLogControl" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">

                    <scp:AuditLogControl id="auditLog" runat="server" LogSource="VPS2012" />

			    </div>
		    </div>
	    </div>

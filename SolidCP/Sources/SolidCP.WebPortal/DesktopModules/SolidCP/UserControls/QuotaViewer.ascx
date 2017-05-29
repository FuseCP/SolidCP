<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuotaViewer.ascx.cs" Inherits="SolidCP.Portal.QuotaViewer" %>
<%@ Register Src="Gauge.ascx" TagName="Gauge" TagPrefix="uc1" %>
<uc1:Gauge ID="gauge" runat="server"
    Progress='<%# Eval("QuotaUsedValue") %>'
    Total='<%# Eval("QuotaValue") %>' 
    Available='<%# Eval("QuotaAvailable") %>'/>
<asp:Label ID="litValue" runat="server"></asp:Label>
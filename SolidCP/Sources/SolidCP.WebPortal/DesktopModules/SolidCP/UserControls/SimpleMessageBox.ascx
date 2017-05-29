<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleMessageBox.ascx.cs" Inherits="SolidCP.Portal.UserControls.SimpleMessageBox" %>
<div id="divMessageBox" runat="server" class="MessageBox" visible="false">
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
    <asp:Literal ID="litDescription" runat="server"></asp:Literal>
</div>
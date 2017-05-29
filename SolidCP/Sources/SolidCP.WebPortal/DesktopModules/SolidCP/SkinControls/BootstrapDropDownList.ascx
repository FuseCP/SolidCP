<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BootstrapDropDownList.ascx.cs" Inherits="SolidCP.Portal.SkinControls.BootstrapDropDownList" %>

<div id="ddl" runat="server">
    <input type="hidden" id="hdSelectedIndex" runat="server" />
    <button ID="btn" runat="server" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
        <span id="lit" runat="server">...</span>
        <span class="caret"></span> 
    </button>
    <input type="hidden" id="hdSelectedValue" runat="server" />
</div>

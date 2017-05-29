<%@ Control language="C#" AutoEventWireup="false" %>
<div class="panel panel-default">
  <div class="panel-heading">
    <h2 class="panel-title">
    <asp:Image ID="imgModuleIcon" runat="server" alt="" />
    <asp:Label ID="lblModuleTitle" runat="server"></asp:Label>
    </h2>
  </div>
  <asp:PlaceHolder ID="ContentPane" runat="server"/>
</div>

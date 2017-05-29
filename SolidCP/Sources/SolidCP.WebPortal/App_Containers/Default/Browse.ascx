<%@ Control Language="C#" AutoEventWireup="false" %>
<div class="panel panel-default">
  <div class="panel-heading">
      <h3 class="panel-title">
            <asp:Image ID="imgModuleIcon" runat="server" alt="" />                
            <asp:Label ID="lblModuleTitle" runat="server"></asp:Label>
      </h3>
  </div>
  <asp:PlaceHolder ID="ContentPane" runat="server"/>
</div>
<%@ Control Language="C#" AutoEventWireup="false" %>
xxxx
<div class="RightContainer">
xxxxxx
    <div class="Top">
		<div class="Title">
			<asp:Image ID="imgModuleIcon" runat="server" ImageAlign="AbsMiddle" Width="48px" Height="48px" />
			&nbsp;<asp:Label ID="lblModuleTitle" runat="server" CssClass="Head"></asp:Label>
		</div>
    </div>
</div>
<div class="RightContainerContent">
    <asp:PlaceHolder ID="ContentPane" runat="server"/>
</div>
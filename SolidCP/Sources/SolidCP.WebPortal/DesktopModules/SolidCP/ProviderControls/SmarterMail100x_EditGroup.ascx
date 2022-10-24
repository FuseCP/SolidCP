<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail100x_EditGroup.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail100x_EditGroup" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<div class="form-group">
		    <asp:Label ID="lblGroupMembers" CssClass="control-label col-sm-2" runat="server" meta:resourcekey="lblGroupMembers" Text="Group e-mails:"></asp:Label>
		<div class="input-group col-sm-8">
			<dnc:EditItemsList id="mailEditItems" runat="server"></dnc:EditItemsList>
        </div>
</div>
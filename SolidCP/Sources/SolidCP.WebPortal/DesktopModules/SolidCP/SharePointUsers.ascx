<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointUsers.ascx.cs" Inherits="SolidCP.Portal.SharePointUsers" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="SolidCP.Providers.OS.SystemUser, SolidCP.Providers.Base"
    QuotaName="SharePoint.Users" />
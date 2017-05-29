<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointGroups.ascx.cs" Inherits="SolidCP.Portal.SharePointGroups" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="SolidCP.Providers.OS.SystemGroup, SolidCP.Providers.Base"
    QuotaName="SharePoint.Groups" />
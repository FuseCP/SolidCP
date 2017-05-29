<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSites.ascx.cs" Inherits="SolidCP.Portal.WebSites" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddWebSite"
    CreateControlID="add_site"
    GroupName="Web"
    TypeName="SolidCP.Providers.Web.WebSite, SolidCP.Providers.Base"
    QuotaName="Web.Sites" />
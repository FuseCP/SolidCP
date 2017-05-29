<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlDatabases.ascx.cs" Inherits="SolidCP.Portal.SqlDatabases" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddDatabase"
    CreateControlID="edit_item"
    GroupName="MsSQL2000"
    TypeName="SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base"
    QuotaName="MsSQL2000.Databases" />
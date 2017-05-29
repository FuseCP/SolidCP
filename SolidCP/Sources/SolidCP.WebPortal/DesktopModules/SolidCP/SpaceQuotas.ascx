<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceQuotas.ascx.cs" Inherits="SolidCP.Portal.SpaceQuotas" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>

<div class="panel-body form-horizontal">
<table  id="tblQuotas" runat="server" cellpadding="3">
    <tr ID="pnlDiskspace" runat="server">
        <td class="SubHead" nowrap><asp:Label runat="server" meta:resourcekey="lblDiskspace" Text="Diskspace, MB:"/></td>
        <td class="Normal"><scp:Quota ID="quotaDiskspace" runat="server" QuotaName="OS.Diskspace" DisplayGauge="True" />&nbsp;&nbsp;(<asp:HyperLink
				ID="lnkViewDiskspaceDetails" runat="server" Target="_blank" meta:resourcekey="GoToReportQuickLink" />)</td>
    </tr>
    <tr ID="pnlBandwidth" runat="server">
        <td class="SubHead" nowrap><asp:Label runat="server" meta:resourcekey="lblBandwidth" Text="Bandwidth, MB:"/></td>
        <td class="Normal"><scp:Quota ID="quotaBandwidth" runat="server" QuotaName="OS.Bandwidth" 
			DisplayGauge="True" />&nbsp;&nbsp;(<asp:HyperLink ID="lnkViewBandwidthDetails" runat="server"
				Target="_blank" meta:resourcekey="GoToReportQuickLink" />)</td>
    </tr>
    <tr ID="pnlDomains" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblDomains" runat="server" meta:resourcekey="lblDomains" Text="Domains:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaDomains" runat="server" QuotaName="OS.Domains" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlSubDomains" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblSubDomains" runat="server" meta:resourcekey="lblSubDomains" Text="Sub-Domains:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaSubDomains" runat="server" QuotaName="OS.SubDomains" DisplayGauge="True" /></td>
    </tr>

<%--    <tr ID="pnlDomainPointers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblDomainPointers" runat="server" meta:resourcekey="lblDomainPointers" Text="Domain Pointers:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaDomainPointers" runat="server" QuotaName="OS.DomainPointers" DisplayGauge="True" /></td>
    </tr>--%>

    <tr ID="pnlOrganizations" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblOrganizations" runat="server" meta:resourcekey="lblOrganizations" Text="Organizations:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaOrganizations" runat="server" QuotaName="HostedSolution.Organizations" DisplayGauge="True" /></td>
    </tr>

    <tr ID="pnlUserAccounts" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblUserAccounts" runat="server" meta:resourcekey="lblUserAccounts" Text="User Accounts:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaUserAccounts" runat="server" QuotaName="HostedSolution.Users" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlDeletedUsers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblDeletedUsers" runat="server" meta:resourcekey="lblDeletedUsers" Text="Deleted Users:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaDeletedUsers" runat="server" QuotaName="HostedSolution.DeletedUsers" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlExchangeAccounts" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblExchangeAccounts" runat="server" meta:resourcekey="lblExchangeAccounts" Text="Exchange Accounts:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaExchangeAccounts" runat="server" QuotaName="Exchange2007.Mailboxes" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlExchangeStorage" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblExchangeStorage" runat="server" meta:resourcekey="lblExchangeStorage" Text="Exchange Storage:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaExchangeStorage" runat="server" QuotaName="Exchange2007.DiskSpace" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlMailAccounts" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblMailAccounts" runat="server" meta:resourcekey="lblMailAccounts" Text="Mail Accounts:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaMailAccounts" runat="server" QuotaName="Mail.Accounts" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlOCSUsers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblOCSUsers" runat="server" meta:resourcekey="lblOCSUsers" Text="OCS Users:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaOCSUsers" runat="server" QuotaName="OCS.Users" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlLyncUsers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblLyncUsers" runat="server" meta:resourcekey="lblLyncUsers" Text="Lync Users:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaLyncUsers" runat="server" QuotaName="Lync.Users" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlLyncPhone" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblLyncPhone" runat="server" meta:resourcekey="lblLyncPhone" Text="Lync Phone Numbers:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaLyncPhone" runat="server" QuotaName="Lync.PhoneNumbers" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlSfBUsers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblSfBUsers" runat="server" meta:resourcekey="lblSfBUsers" Text="Skype for Business Users:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaSfBUsers" runat="server" QuotaName="SfB.Users" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlSfBPhone" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblSfBPhone" runat="server" meta:resourcekey="lblSfBPhone" Text="Skype for Business Phone Numbers:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaSfBPhone" runat="server" QuotaName="SfB.PhoneNumbers" DisplayGauge="True" /></td>
    </tr>

    <tr ID="pnlBlackBerryUsers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblBlackBerryUsers" runat="server" meta:resourcekey="lblBlackBerryUsers" Text="BlackBerry Users:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaBlackBerryUsers" runat="server" QuotaName="BlackBerry.Users" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlSharepointSites" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblSharepointSites" runat="server" meta:resourcekey="lblSharepointSites" Text="Sharepoint Foundation Sites:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaSharepointSites" runat="server" QuotaName="HostedSharePoint.Sites" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlEnterpriseSharepointSites" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblEnterpriseSharepointSites" runat="server" meta:resourcekey="lblEnterpriseSharepointSites" Text="Sharepoint Server Sites:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaEnterpriseSharepointSites" runat="server" QuotaName="HostedSharePointEnterprise.Sites" DisplayGauge="True" /></td>
    </tr>    

    <tr ID="pnlWebSites" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblWebSites" runat="server" meta:resourcekey="lblWebSites" Text="Web Sites:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaWebSites" runat="server" QuotaName="Web.Sites" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlFtpAccounts" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblFtpAccounts" runat="server" meta:resourcekey="lblFtpAccounts" Text="FTP Accounts:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaFtpAccounts" runat="server" QuotaName="FTP.Accounts" DisplayGauge="True"/></td>
    </tr>
    <tr ID="pnlDatabases" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblDatabases" runat="server" meta:resourcekey="lblDatabases" Text="Databases 2008:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaDatabases" runat="server" QuotaName="MsSQL2008.Databases" DisplayGauge="True"/></td>
    </tr>
    <tr ID="pnlMsSQL2014Databases" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblMsSQL2014Databases" runat="server" meta:resourcekey="lblMsSQL2014Databases" Text="MsSQL2014 Databases:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotamssql2014databases" runat="server" QuotaName="MsSQL2014.Databases" DisplayGauge="True"/></td>
    </tr>
    <tr ID="pnlMsSQL2016Databases" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblMsSQL2016Databases" runat="server" meta:resourcekey="lblMsSQL2016Databases" Text="MsSQL2016 Databases:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotamssql2016databases" runat="server" QuotaName="MsSQL2016.Users" DisplayGauge="True"/></td>
    </tr>
        <tr ID="pnlMySQL5Databases" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblMySQL5Databases" runat="server" meta:resourcekey="lblMySQL5Databases" Text="MySQL5 Databases:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotamysql5databases" runat="server" QuotaName="MySQL5.Databases" DisplayGauge="True"/></td>
    </tr>
        <tr ID="pnlMariaDBDatabases" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblMariaDBDatabases" runat="server" meta:resourcekey="lblMariaDBDatabases" Text="MariaDB Databases:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotamariadbdatabases" runat="server" QuotaName="MariaDB.Databases" DisplayGauge="True"/></td>
    </tr>

	<tr id="pnlHyperVForPC" runat="server">
		<td class="SubHead" nowrap><asp:Label ID="lblHyperVForPC" runat="server" meta:resourcekey="lblHyperVForPC" Text="Number of VM:" /></td>
		<td class="Normal"><scp:Quota ID="quotaNumberOfVm" runat="server" QuotaName="VPSForPC.ServersNumber" DisplayGauge="True" /></td>
	</tr>
    <tr id="pnlFolders" runat="server">
		<td class="SubHead" nowrap><asp:Label ID="lblFolders" runat="server" meta:resourcekey="lblFolders" Text="Folders:" /></td>
		<td class="Normal"><scp:Quota ID="quotaNumberOfFolders" runat="server" QuotaName="EnterpriseStorage.Folders" DisplayGauge="True" /></td>
	</tr>
    <tr id="pnlEnterpriseStorage" runat="server">
		<td class="SubHead" nowrap><asp:Label ID="lblEnterpriseStorage" runat="server" meta:resourcekey="lblEnterpriseStorage" Text="Cloud Folders:" /></td>
		<td class="Normal"><scp:Quota ID="quotaEnterpriseStorage" runat="server" QuotaName="EnterpriseStorage.DiskStorageSpace" DisplayGauge="True" /></td>
	</tr>
    <tr ID="pnlRdsCollections" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblRdsCollections" runat="server" meta:resourcekey="lblRdsCollections" Text="RDS Collections:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaRdsCollections" runat="server" QuotaName="RDS.Collections" DisplayGauge="True" /></td>
    </tr> 
    <tr ID="pnlRdsServers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblRdsServers" runat="server" meta:resourcekey="lblRdsServers" Text="RDS Servers:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaRdsServers" runat="server" QuotaName="RDS.Servers" DisplayGauge="True" /></td>
    </tr> 
    <tr ID="pnlRdsUsers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblRdsUsers" runat="server" meta:resourcekey="lblRdsUsers" Text="RDS Users:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotaRdsUsers" runat="server" QuotaName="RDS.Users" DisplayGauge="True" /></td>
    </tr>
     <tr ID="pnlVPS2012Servers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblVPS2012Servers" runat="server" meta:resourcekey="lblVPS2012Servers" Text="VPS 2012 Servers:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotavps2012servers" runat="server" QuotaName="VPS2012.ServersNumber" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlVPS2012RamQuota" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblVPS2012RamQuota" runat="server" meta:resourcekey="lblVPS2012RamQuota" Text="VPS 2012 Ram, MB:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotavps2012ramquota" runat="server" QuotaName="VPS2012.Ram" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlVPS2012HddQuota" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblVPS2012HddQuota" runat="server" meta:resourcekey="lblVPS2012HddQuota" Text="VPS 2012 Hdd, GB:"></asp:Label></td>
        <td class="Normal"><scp:Quota ID="quotavps2012hddquota" runat="server" QuotaName="VPS2012.Hdd" DisplayGauge="True" /></td>
    </tr>
</table>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnViewQuotas" CssClass="btn btn-success" runat="server" OnClick="btnViewQuotas_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnViewQuotasText"/> </CPCC:StyleButton>
</div>


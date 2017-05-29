<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationHome.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationHome" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
				<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="Organization48" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Welcome"></asp:Localize>
                        </h3>
                </div>
                <div class="panel-body form-horizontal">
                    <table>
                        <tr class="OrgStatsRow">
                            <td>
                                <asp:Label runat="server" ID="lblOrganizationName" meta:resourcekey="lblOrganizationName" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOrganizationNameValue" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td >
                                <asp:Label runat="server" meta:resourcekey="lblOrganizationID" ID="lblOrganizationID" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOrganizationIDValue" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td>
                                <asp:Label runat="server" meta:resourcekey="lblCreated" ID="lblCreated" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCreatedValue" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%">
                        <asp:Panel runat="server" ID="organizationStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locHeadStatistics" runat="server" meta:resourcekey="locHeadStatistics"
                                    Text="Organization Statistics"></asp:Localize>
                            </td>
                        </tr>
<!--
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkDomains" runat="server" meta:resourcekey="lnkDomains"></asp:HyperLink>
                            </td>
                            <td width="100%">
                                <scp:QuotaViewer ID="domainStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
-->
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkUsers" runat="server" meta:resourcekey="lnkUsers"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="userStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkDeletedUsers" runat="server" meta:resourcekey="lnkDeletedUsers"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="deletedUserStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow" id="securGroupsStat" runat="server">
                            <td class="OrgStatsQuota" nowrap >
                                <asp:HyperLink ID="lnkGroups" runat="server" meta:resourcekey="lnkGroups" Text="Groups:"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="groupStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="exchangeStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locExchangeStatistics" runat="server" meta:resourcekey="locExchangeStatistics" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkMailboxes" runat="server" meta:resourcekey="lnkMailboxes" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="mailboxesStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>

                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkSharedMailboxes" runat="server" meta:resourcekey="lnkSharedMailboxes" Text="Shared mailboxes" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="mailboxesSharedStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkResourceMailboxes" runat="server" meta:resourcekey="lnkResourceMailboxes" Text="Resource mailboxes" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="mailboxesResourceStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>

                        <tr class="OrgStatsRow" id="rowContacts" runat="server">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkContacts" runat="server" meta:resourcekey="lnkContacts"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="contactsStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow" id="rowLists" runat="server">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkLists" runat="server" meta:resourcekey="lnkLists"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="listsStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"  id="rowFolders" runat="server">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkFolders" runat="server" meta:resourcekey="lnkFolders"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="foldersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow" id="rowExchangeStorage" runat="server">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkExchangeStorage" runat="server" meta:resourcekey="lnkExchangeStorage"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="exchangeStorageStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow" id="rowExchangeLitigationHold" runat="server">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkExchangeLitigationHold" runat="server" meta:resourcekey="lnkExchangeLitigationHold"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="exchangeLitigationHoldStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow" id="rowExchangeArchiving" runat="server">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkExchangeArchiving" runat="server" meta:resourcekey="lnkExchangeArchiving">Archiving Storage (Mb):</asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="exchangeArchivingStatus" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>

                        </asp:Panel>

                        <asp:Panel runat="server" ID="besStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locBESStatistics" runat="server" meta:resourcekey="locBESStatistics" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkBESUsers" runat="server" meta:resourcekey="lnkBESUsers" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="besUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="sfbStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locSfBStatistics" runat="server" meta:resourcekey="locSfBStatistics" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkSfBUsers" runat="server" meta:resourcekey="lnkSfBUsers" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="sfbUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="lyncStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locLyncStatistics" runat="server" meta:resourcekey="locLyncStatistics" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkLyncUsers" runat="server" meta:resourcekey="lnkLyncUsers" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="lyncUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>



                        
                        <asp:Panel runat="server" ID="sharePointStatsPanel">
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locSharePoint" runat="server" meta:resourcekey="locSharePoint"
                                    Text="Organization Statistics"></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap> 
                                <asp:HyperLink ID="lnkSiteCollections" runat="server" meta:resourcekey="lnkSiteCollections"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="siteCollectionsStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="sharePointEnterpriseStatsPanel">
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locSharePointEnterprise" runat="server" meta:resourcekey="locSharePointEnterprise"
                                    Text="Organization Statistics"></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap> 
                                <asp:HyperLink ID="lnkEnterpriseSiteCollections" runat="server" meta:resourcekey="lnkSiteCollections"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="enterpriseSiteCollectionsStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>



                        <asp:Panel runat="server" ID="ocsStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locOCSStatistics" runat="server" meta:resourcekey="locOCSStatistics" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkOCSUsers" runat="server" meta:resourcekey="lnkOCSUsers" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="ocsUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>

                        
                        <asp:Panel runat="server" ID="crmStatsPanel">
                        <tr >
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locCRM" runat="server" meta:resourcekey="locCRM"
                                    Text="Organization Statistics"></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkCRMUsers" runat="server" meta:resourcekey="lnkCRMUsers" Text="Full licenses :"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="crmUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkLimitedCRMUsers" runat="server" meta:resourcekey="lnkLimitedCRMUsers" Text="Limited licenses :"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="crmLimitedUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkESSCRMUsers" runat="server" meta:resourcekey="lnkESSCRMUsers" Text="ESS licenses :"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="crmESSUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
				                <asp:HyperLink ID="lnkCRMDBSize" runat="server" meta:resourcekey="lnkCRMDBSize" Text="Storage size (MB):"></asp:HyperLink>
                            </td>
                            <td>
				                <scp:QuotaViewer ID="crmDBSize" runat="server" QuotaTypeId="2" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="crm2013StatsPanel">
                        <tr >
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locCRM2013" runat="server" meta:resourcekey="locCRM2013"
                                    Text="CRM 2013/2015"></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkProfessionalCRMUsers" runat="server" meta:resourcekey="lnkProfessionalCRMUsers" Text="Professional licenses :"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="crmProfessionalUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkBasicCRMUsers" runat="server" meta:resourcekey="lnkBasicCRMUsers" Text="Basic licenses :"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="crmBasicUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkEssentialCRMUsers" runat="server" meta:resourcekey="lnkEssentialCRMUsers" Text="Essential licenses :"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="crmEssentialUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
				                <asp:HyperLink ID="lnkCRM2013DBSize" runat="server" meta:resourcekey="lnkCRMDBSize" Text="Storage size (MB):"></asp:HyperLink>
                            </td>
                            <td>
				                <scp:QuotaViewer ID="crm2013DBSize" runat="server" QuotaTypeId="2" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>


                        <asp:Panel runat="server" ID="enterpriseStorageStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locEnterpriseStorage" runat="server" meta:resourcekey="locEnterpriseStorage" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkEnterpriseStorageSpace" runat="server" meta:resourcekey="lnkEnterpriseStorageSpace" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="enterpriseStorageSpaceStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkEnterpriseStorageFolders" runat="server" meta:resourcekey="lnkEnterpriseStorageFolders"></asp:HyperLink>
                            </td>
                            <td>
                                <scp:QuotaViewer ID="enterpriseStorageFoldersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="serviceLevelsStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locServiceLevels" runat="server" meta:resourcekey="locServiceLevels" ></asp:Localize>
                            </td>
                        </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="remoteDesktopStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locRemoteDesktop" runat="server" meta:resourcekey="locRemoteDesktop" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkRdsServers" runat="server" meta:resourcekey="lnkRdsServers" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="rdsServers" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkRdsCollections" runat="server" meta:resourcekey="lnkRdsCollections" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="rdsCollections" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                            <tr class="OrgStatsRow"> 
                            <td class="OrgStatsQuota" nowrap>
                                <asp:HyperLink ID="lnkRdsUsers" runat="server" meta:resourcekey="lnkRdsUsers" />
                            </td>
                            <td>
                                <scp:QuotaViewer ID="rdsUsers" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>
                    </table>
                  
                </div>

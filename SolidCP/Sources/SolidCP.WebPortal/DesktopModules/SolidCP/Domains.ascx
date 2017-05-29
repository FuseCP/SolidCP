<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Domains.ascx.cs" Inherits="SolidCP.Portal.Domains" %>
 <%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
 <%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="scp" %>
 <%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="scp" %>
 <%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="scp" %>
 <%@ Register Src="UserControls/DomainActions.ascx" TagName="DomainActions" TagPrefix="scp" %>
 <%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
 <scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<script type="text/javascript">
                var gridViewId = '<%# gvDomains.ClientID %>';
                function checkAll(selectAllCheckbox) {
                    //get all checkbox and select it
                    $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
                }
                function unCheckSelectAll(selectCheckbox) {
                    //if any item is unchecked, uncheck header checkbox as also
                    if (!selectCheckbox.checked)
                        $('th :checkbox', gridViewId).prop("checked", false);
                }
</script>

 <div class="FormButtonsBar right">
     <div class="right">
        <CPCC:StyleButton ID="btnAddDomain" runat="server" CssClass="btn btn-primary" OnClick="btnAddDomain_Click" >
            <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddDomain"/>
        </CPCC:StyleButton>
         &nbsp;<asp:CheckBox ID="chkRecursive" runat="server" Text="Show Nested Spaces Items" meta:resourcekey="chkRecursive" AutoPostBack="true" Checked="True" CssClass="Normal" />
     </div>
      </div>
     <div class="panel-body ">
         <div class="row">
         <div class="col-md-3">
             <scp:DomainActions ID="websiteActions" runat="server" GridViewID="gvDomains" CheckboxesName="chkSelectedIds" />
         </div>
              <div class="col-md-6">
 
              </div>
            <div class="col-md-3 text-right">
                <scp:SearchBox ID="searchBox" runat="server" />
             </div>
         </div>
     </div>
 
 
 <asp:GridView ID="gvDomains" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" DataSourceID="odsDomainsPaged"
     EmptyDataText="gvDomains" DataKeyNames="DomainID"
     CssSelectorClass="NormalGridView" AllowPaging="True" OnRowCommand="gvDomains_RowCommand">
     <Columns>
         <asp:TemplateField>
             <HeaderTemplate>
                 <asp:CheckBox ID="selectAll" Runat="server" onclick="checkAll(this);" CssClass="HeaderCheckbox"></asp:CheckBox>
             </HeaderTemplate>
 			<ItemTemplate>							        
 				<asp:CheckBox runat="server" ID="chkSelectedIds" onclick="unCheckSelectAll(this);" CssClass="GridCheckbox"></asp:CheckBox>
 			</ItemTemplate>
 		</asp:TemplateField>


         <asp:TemplateField SortExpression="DomainName" HeaderText="gvDomainsName">
             <ItemStyle Width="45%" Wrap="False"></ItemStyle>
             <ItemTemplate>
 	            <b><asp:hyperlink id=lnkEdit1 runat="server" CssClass="Medium"
 	                NavigateUrl='<%# GetItemEditUrl(Eval("PackageID"), Eval("DomainID")) %>'>
 		            <%# Eval("DomainName")%></asp:hyperlink>
 	            </b>
 	            <div runat="server" class="Small" style="margin-top:2px;" visible=' <%# Eval("MailDomainName") != DBNull.Value %>'>
                     <asp:Label ID="lblMailDomain" runat="server" meta:resourcekey="lblMailDomain" Text="Mail:"></asp:Label>
                     <b><%# Eval("MailDomainName")%></b>
 	            </div>
             </ItemTemplate>
         </asp:TemplateField>


         <asp:TemplateField HeaderText="gvDomainsExpirationDate">
             <ItemStyle Width="11%"></ItemStyle>
             <ItemTemplate>
 	            <%# GetDomainExpirationDate(Eval("ExpirationDate"), Eval("LastUpdateDate"))%>
             </ItemTemplate>
         </asp:TemplateField>


         <asp:TemplateField HeaderText="">
             <ItemStyle Width="5%"></ItemStyle>
             <ItemTemplate>
 	            <div style="display:inline-block" runat="server" Visible='<%# ShowDomainDnsInfo(Eval("ExpirationDate"), Eval("LastUpdateDate"), !(bool)Eval("IsSubDomain") && !(bool)Eval("IsInstantAlias") && !(bool)Eval("IsDomainPointer")) && !string.IsNullOrEmpty(GetDomainDnsRecords((int)Eval("DomainId"))) %>'>
                   <img style="border-width: 0px;" src="App_Themes/Default/Images/information_icon_small.gif" title="<%# GetDomainTooltip((int)Eval("DomainId"), Eval("RegistrarName") != DBNull.Value ? (string)Eval("RegistrarName"):string.Empty)  %>">
                 </div>
             </ItemTemplate>
         </asp:TemplateField>


         <asp:TemplateField HeaderText="gvDomainsType">
             <ItemStyle Width="30%"></ItemStyle>
             <ItemTemplate>
 	            <%# GetDomainTypeName((bool)Eval("IsSubDomain"), (bool)Eval("IsInstantAlias"), (bool)Eval("IsDomainPointer"))%>
             </ItemTemplate>
         </asp:TemplateField>


         <asp:TemplateField SortExpression="PackageName" HeaderText="gvDomainsSpace">
             <ItemStyle Width="30%"></ItemStyle>
             <ItemTemplate>
 	            <asp:hyperlink id="lnkEdit2" runat="server" EnableViewState="false"
 	                NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
 		            <%# Eval("PackageName") %>
 	            </asp:hyperlink>
             </ItemTemplate>
         </asp:TemplateField>


 		<asp:TemplateField SortExpression="Username" HeaderText="gvDomainsUser">
 			<ItemStyle Wrap="False" />
 			<ItemTemplate>
 				<asp:hyperlink id=lnkEdit3 runat="server" EnableViewState="false"
 				    NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
 					<%# Eval("Username") %>
 				</asp:hyperlink>
 			</ItemTemplate>
             <HeaderStyle Wrap="False" />
 		</asp:TemplateField>


         <asp:TemplateField SortExpression="ServerName" HeaderText="gvDomainsServer">
 			<ItemStyle Wrap="False" />
 			<ItemTemplate>
 				<asp:hyperlink id=lnkEdit4 runat="server" EnableViewState="false"
 				    NavigateUrl='<%# GetItemsPageUrl("ServerID", Eval("ServerID").ToString()) %>'>
 					<%# Eval("ServerName") %>
 				</asp:hyperlink>
 			</ItemTemplate>
         </asp:TemplateField>

         <asp:TemplateField>
			<ItemTemplate>
               <asp:Button ID="btnSE" runat="server" Text="go to SpamExperts" CssClass="btn btn-primary" OnClick="btnSE_Click" CommandName="GoToSpamExperts" CommandArgument='<%# Eval("DomainName") %>' />
			</ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField>
 			<ItemTemplate>
 				<CPCC:StyleButton ID="cmdDetach" runat="server" 
 					CommandName="Detach" CommandArgument='<%# Eval("DomainID") %>'
					CssClass="btn btn-default btn-sm" OnClientClick="return confirm('Remove this item?');">
                    <i class="fa fa-chain-broken">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="cmdDetachText"/>
				</CPCC:StyleButton>
 			</ItemTemplate>
         </asp:TemplateField>




     </Columns>
 	<PagerSettings Mode="NumericFirstLast" />
 </asp:GridView>
 <asp:ObjectDataSource ID="odsDomainsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetDomainsPagedCount"
     SelectMethod="GetDomainsPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.ServersHelper" OnSelected="odsDomainsPaged_Selected">
     <SelectParameters>
         <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="-1" />
         <asp:QueryStringParameter Name="serverId" QueryStringField="ServerID" DefaultValue="0" Type="Int32" />
         <asp:ControlParameter Name="recursive" ControlID="chkRecursive" PropertyName="Checked" DefaultValue="False" />
         <asp:ControlParameter Name="filterColumn" ControlID="searchBox" PropertyName="FilterColumn" />
         <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
     </SelectParameters>
 </asp:ObjectDataSource>
 
 	
 <div class="GridFooter">
     <table>
         <tr>
             <td><asp:Label ID="lblDomains" runat="server" meta:resourcekey="lblDomains" Text="Domains:" CssClass="NormalBold"></asp:Label></td>
             <td><scp:Quota ID="quotaDomains" runat="server" QuotaName="OS.Domains" /></td>
         </tr>
         <tr>
             <td><asp:Label ID="lblSubDomains" runat="server" meta:resourcekey="lblSubDomains" Text="Sub-Domains:" CssClass="NormalBold"></asp:Label></td>
             <td><scp:Quota ID="quotaSubDomains" runat="server" QuotaName="OS.SubDomains" /></td>
         </tr>
 <!--
         <tr>
	
             <td><asp:Label ID="lblDomainPointers" runat="server" meta:resourcekey="lblDomainPointers" Text="Domain Aliases:" CssClass="NormalBold"></asp:Label></td>
             <td><scp:Quota ID="quotaDomainPointers" runat="server" QuotaName="OS.DomainPointers" /></td>
         </tr>
 -->
     </table>
 </div>
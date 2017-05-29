<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDomainNames.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeDomainNames" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeDomainName48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Domain Names"></asp:Localize>
                    </h3>
				</div>
                    <div class="FormButtonsBar right">
                        <CPCC:StyleButton id="btnAddDomain" CssClass="btn btn-primary" runat="server" OnClick="btnAddDomain_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddDomain"/> </CPCC:StyleButton>
                    </div>
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    <asp:GridView ID="gvDomains" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvDomains" CssSelectorClass="NormalGridView" OnRowCommand="gvDomains_RowCommand">
					    <Columns>
						    <asp:TemplateField SortExpression="DomainName" HeaderText="gvDomainsName">
							    <ItemStyle Width="50%"></ItemStyle>
							    <ItemTemplate>
								    <asp:hyperlink id="lnkEditZone" runat="server" EnableViewState="false"
									    NavigateUrl='<%# GetDomainRecordsEditUrl(Eval("DomainID").ToString()) %>' Enabled="true">
									    <%# Eval("DomainName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
                            <asp:TemplateField HeaderText="gvDomainsType">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <asp:Label ID="Label1" Text='<%# Eval("DomainType") %>' runat="server"/>
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
                             <asp:TemplateField HeaderText="gvDomainsTypeChange">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <asp:Button ID="btnChangeDomain" text="Change" meta:resourcekey="btnChangeDomain" CssClass="btn btn-default btn-sm" runat="server" CommandName="Change" CommandArgument='<%# Eval("DomainId") + "|" + Eval("DomainType") %>'/>
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvDomainsDefault">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <input type="radio" name="DefaultDomain" value='<%# Eval("DomainId") %>' <%# IsChecked((bool)Eval("IsDefault")) %> />
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>                            
						    <asp:TemplateField>
							    <ItemTemplate>
									<CPCC:StyleButton id="imgDelDomain" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("DomainId") %>' Visible='<%# ((!(bool)Eval("IsDefault"))) && (!CheckDomainUsedByHostedOrganization(Eval("DomainID").ToString())) %>' OnClientClick="return confirm('Are you sure you want to delete selected domain?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>&nbsp;
                                    <CPCC:StyleButton ID="lnkViewUsage" CssClass="btn btn-primary" runat="server" Text="View Usage" Visible='<%# CheckDomainUsedByHostedOrganization(Eval("DomainID").ToString()) %>'
                                        CommandName="ViewUsage" CommandArgument='<%# Eval("DomainId") %>'
                                         />
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
				    <div class="panel-footer">
				   <div class="row">
                    <div class="col-md-6">
				 
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Domains Used:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <scp:QuotaViewer ID="domainsQuota" runat="server" QuotaTypeId="2" />
</div>
                         <div class="col-md-6 text-right">
				        <CPCC:StyleButton id="btnSetDefaultDomain" CssClass="btn btn-success" runat="server"  CausesValidation="false" OnClick="btnSetDefaultDomain_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetDefaultDomain"/> </CPCC:StyleButton>&nbsp;
                    </div>
                       </div>
				</div>
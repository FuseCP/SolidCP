<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangePublicFolderEmailAddresses.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangePublicFolderEmailAddresses" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="UserControls/PublicFolderTabs.ascx" TagName="PublicFolderTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

<script type="text/javascript">
                function checkAll(selectAllCheckbox) {
                    //get all checkbox and select it
                    $('td :checkbox').prop("checked", selectAllCheckbox.checked);
                }
                function unCheckSelectAll(selectCheckbox) {
                    //if any item is unchecked, uncheck header checkbox as also
                    if (!selectCheckbox.checked)
                        $('th :checkbox').prop("checked", false);
                }
</script>


				<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="ExchangePublicFolder48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Public Folder"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                    </h3>
                </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:PublicFolderTabs id="tabs" runat="server" SelectedTab="public_folder_addresses" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<fieldset>
					    <legend>
					       <h3><i class="fa fa-envelope-o"></i>  <asp:Label ID="lblAddEmail" runat="server" Text="Add New E-mail Address" meta:resourcekey="lblAddEmail" CssClass="NormalBold"></asp:Label></h3>
					    </legend>
					    <div class="row" style="padding:20px;max-width:1200px">
                           <div class="col-sm-2" style="line-height: 2.5;">
                               <asp:Localize ID="locAccount" runat="server" meta:resourcekey="locAccount" Text="E-mail Address: *"></asp:Localize></td>
						   </div>
                           <div class="input-group col-sm-10">
									<scp:EmailAddress id="email" runat="server" ValidationGroup="AddEmail"></scp:EmailAddress>
                                    <span class="input-group-btn"><CPCC:StyleButton id="btnAddEmail" CssClass="btn btn-success" runat="server" OnClick="btnAddEmail_Click" ValidationGroup="AddEmail"> <i class="fa fa-envelope">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddEmail"/> </CPCC:StyleButton></span>
                           </div>
					   </div>
					</fieldset>
					<br />
					<br />
					
					<scp:CollapsiblePanel id="secExistingAddresses" runat="server"
                        TargetControlID="ExistingAddresses" meta:resourcekey="secExistingAddresses" Text="Existing E-mail Addresses">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="ExistingAddresses" runat="server" Height="0" style="overflow:hidden;">
                        <br />
				        <asp:GridView ID="gvEmails" runat="server" AutoGenerateColumns="False"
					        Width="100%" EmptyDataText="gvEmails" CssSelectorClass="NormalGridView" DataKeyNames="EmailAddress">
					        <Columns>
					            <asp:TemplateField>
					                <HeaderTemplate>
					                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
					                </HeaderTemplate>
					                <ItemTemplate>
					                    <asp:CheckBox ID="chkSelect" runat="server" onclick="unCheckSelectAll(this);" Enabled='<%# !(bool)Eval("IsPrimary") %>' />
					                </ItemTemplate>
                                    <ItemStyle Width="10px" />
					            </asp:TemplateField>
						        <asp:TemplateField HeaderText="gvEmailAddress">
							        <ItemStyle Width="60%"></ItemStyle>
							        <ItemTemplate>
								        <%# Eval("EmailAddress") %>
							        </ItemTemplate>
						        </asp:TemplateField>
						        <asp:TemplateField HeaderText="gvPrimaryEmail">
							        <ItemTemplate>
							            <div style="text-align:center">
							                &nbsp;
								            <asp:Image ID="Image2" runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsPrimary") %>' />
								        </div>
							        </ItemTemplate>
						        </asp:TemplateField>
					        </Columns>
				        </asp:GridView>
				        
				        <br />
				        <asp:Localize ID="locTotal" runat="server" meta:resourcekey="locTotal" Text="Total E-mail Addresses:"></asp:Localize>
				        <asp:Label ID="lblTotal" runat="server" CssClass="NormalBold">1</asp:Label>
				        
					    <br />
					    <br />
                        <CPCC:StyleButton id="btnDeleteAddresses" CssClass="btn btn-danger" runat="server" OnClick="btnDeleteAddresses_Click" CausesValidation="false"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteAddresses"/> </CPCC:StyleButton>&nbsp;
				        <CPCC:StyleButton id="btnSetAsPrimary" CssClass="btn btn-success" runat="server"  CausesValidation="false" OnClick="btnSetAsPrimary_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetAsPrimary"/> </CPCC:StyleButton>
					</asp:Panel>					
					<br />
					<br />
                        </div>
				</div>
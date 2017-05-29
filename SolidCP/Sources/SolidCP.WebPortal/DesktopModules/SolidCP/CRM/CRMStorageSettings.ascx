<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMStorageSettings.ascx.cs" Inherits="SolidCP.Portal.CRMStorageSettings" %>
<%@ Register Src="../ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel"
    TagPrefix="scp" %>
<%@ Register Src="../ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="scp" %>

<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register src="../UserControls/QuotaEditor.ascx" tagname="QuotaEditor" tagprefix="uc1" %>



				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image2" SkinID="CRMLogo" runat="server" />
					<asp:Localize ID="Localize1" runat="server"  Text="CRM Organization"></asp:Localize>
				</div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />


				    
					<scp:CollapsiblePanel id="secStorageLimits" runat="server"
                        TargetControlID="StorageLimits" meta:resourcekey="secStorageLimits" >
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="StorageLimits" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    
                            <tr>
                                <td class="FormLabel200" align="right"><asp:Localize runat="server" meta:resourcekey="locUsageStorage" >Current usage (MB):</asp:Localize></td>
                                <td>
                                    <asp:Label ID="lblDBSize" runat="server" Text="0" /> of <asp:Label ID="lblMAXDBSize" runat="server" Text="0" />
                            </tr>
                            <tr>
                                <td class="FormLabel200" align="right"><asp:Localize runat="server" meta:resourcekey="locLimitStorage" >Maximum allowed (MB):</asp:Localize></td>
                                <td>
                                    <asp:Label ID="lblLimitDBSize" runat="server" Text="0" />
                            </tr>
						    <tr>
							    <td class="FormLabel200" align="right"><asp:Localize ID="locMaxStorage" runat="server" meta:resourcekey="locMaxStorage" >Reassign storage space (MB):</asp:Localize></td>
							    <td>                                    
									<uc1:QuotaEditor QuotaTypeId="2" ID="maxStorageSettingsValue" runat="server"/>
								</td>
						    </tr>
                            <!-- 
						    <tr>
							    <td class="FormLabel200" align="right"><asp:Localize ID="locWarningStorage" runat="server" meta:resourcekey="locWarningStorage" ></asp:Localize></td>
							    <td>
									<uc1:QuotaEditor  QuotaTypeId="2" ID="warningValue" runat="server" />
									
								</td>
						    </tr>
                            -->
					    </table>
					    <br />
					</asp:Panel>
                   									                    

				    
				</div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditStorageSettings"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSave"/> </CPCC:StyleButton>
				    </div>
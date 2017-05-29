<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostedSharePointEnterpriseStorageSettings.ascx.cs" Inherits="SolidCP.Portal.HostedSharePointEnterpriseStorageSettings" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel"
    TagPrefix="scp" %>
<%@ Register Src="../ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="scp" %>
<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register src="../UserControls/QuotaEditor.ascx" tagname="QuotaEditor" tagprefix="uc1" %>



<div id="ExchangeContainer">
	<div class="Module">
		<div class="Left">
        </div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeStorageConfig48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" ></asp:Localize>
				</div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
					<scp:CollapsiblePanel id="secStorageLimits" runat="server"
                        TargetControlID="StorageLimits" meta:resourcekey="secStorageLimits" >
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="StorageLimits" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    
						    <tr>
							    <td class="FormLabel200" align="right"><asp:Localize ID="locMaxStorage" runat="server" meta:resourcekey="locMaxStorage" ></asp:Localize></td>
							    <td>                                    
									<uc1:QuotaEditor QuotaTypeId="2" ID="maxStorageSettingsValue" runat="server" />                                    																	    
								</td>
						    </tr>
						    <tr>
							    <td class="FormLabel200" align="right"><asp:Localize ID="locWarningStorage" runat="server" meta:resourcekey="locWarningStorage" ></asp:Localize></td>
							    <td>
									<uc1:QuotaEditor  QuotaTypeId="2" ID="warningValue" runat="server" />
									
								</td>
						    </tr>
					    </table>
					    <br />
					</asp:Panel>
                   									                    
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-warning" runat="server" OnClick="btnSave_Click" ValidationGroup="EditStorageSettings"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>&nbsp;
						<CPCC:StyleButton id="btnSaveApply" CssClass="btn btn-success" runat="server" OnClick="btnSaveApply_Click" ValidationGroup="EditStorageSettings"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveApplyText"/> </CPCC:StyleButton>
						
				    </div>
				    
				</div>
			</div>
		</div>
	</div>
</div>
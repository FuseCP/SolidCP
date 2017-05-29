<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSEditUserExperience.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSEditUserExperience" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-heading">
    <asp:Image ID="imgEditRDSCollection" SkinID="EnterpriseRDSCollections48" runat="server" />
    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit RDS Collection"></asp:Localize>
    -
    <asp:Literal ID="litCollectionName" runat="server" Text="" />
</div>
<div class="panel-body form-horizontal">
    <scp:CollectionTabs id="tabs" runat="server" SelectedTab="rds_collection_user_experience" />
    <div class="panel panel-default tab-content">
        <div class="panel-body form-horizontal">
            <scp:SimpleMessageBox id="messageBox" runat="server" />
            <scp:CollapsiblePanel id="secTimeout" runat="server" TargetControlID="timeoutPanel" meta:resourcekey="secTimeout" Text="Lock Screen Timeout"/>
            <asp:Panel ID="timeoutPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="col">
                    <asp:DropDownList ID="ddTimeout" runat="server" CssClass="form-control"/>
                </div>
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbTimeoutUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbTimeoutAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secRunCommand" runat="server" TargetControlID="runCommandPanel" meta:resourcekey="secRunCommand" Text="Remove Run Command"/>
            <asp:Panel ID="runCommandPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbRunCommandUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbRunCommandAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secPowershellCommand" runat="server" TargetControlID="powershellCommandPanel" meta:resourcekey="secPowershellCommand" Text="Remove Powershell Command"/>
            <asp:Panel ID="powershellCommandPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbPowershellUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbPowershellAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secHideCDrive" runat="server" TargetControlID="hideCDrivePanel" meta:resourcekey="secHideCDrive" Text="Hide C: Drive"/>
            <asp:Panel ID="hideCDrivePanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="col">
                    <asp:DropDownList ID="ddHideCDrive" runat="server" CssClass="form-control">
                        <asp:ListItem Value="" Text="Do not restrict drives" />
                        <asp:ListItem Value="3" Text="Restrict A and B drives only" />
                        <asp:ListItem Value="4" Text="Restrict C drive only" />
                        <asp:ListItem Value="8" Text="Restrict D drive only" />
                        <asp:ListItem Value="7" Text="Restrict A, B and C drives only" />
                        <asp:ListItem Value="15" Text="Restrict A, B, C and D drives only" />
                        <asp:ListItem Value="67108863" Text="Restrict all drives" />
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbHideCDriveUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbHideCDriveAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secShutdown" runat="server" TargetControlID="shutdownPanel" meta:resourcekey="secShutdown" Text="Remove Shutdown and Restart"/>
            <asp:Panel ID="shutdownPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbShutdownUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbShutdownAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secTaskManager" runat="server" TargetControlID="taskManagerPanel" meta:resourcekey="secTaskManager" Text="Disable Task Manager"/>
            <asp:Panel ID="taskManagerPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbTaskManagerUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbTaskManagerAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secChangeDesktop" runat="server" TargetControlID="desktopPanel" meta:resourcekey="secChangeDesktop" Text="Changing Desktop Disabled"/>
            <asp:Panel ID="desktopPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbDesktopUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbDesktopAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secScreenSaver" runat="server" TargetControlID="screenSaverPanel" meta:resourcekey="secScreenSaver" Text="Disable Screen Saver"/>
            <asp:Panel ID="screenSaverPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbScreenSaverUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbScreenSaverAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secDriveSpace" runat="server" TargetControlID="driveSpacePanel" meta:resourcekey="secDriveSpace" Text="Drive Space Threshold"/>
            <asp:Panel ID="driveSpacePanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="col">
                    <asp:DropDownList ID="ddTreshold" runat="server" CssClass="form-control">
                        <asp:ListItem Value="" Text="None" />
                        <asp:ListItem Value="5" Text="5%" />
                        <asp:ListItem Value="10" Text="10%" />
                        <asp:ListItem Value="15" Text="15%" />
                        <asp:ListItem Value="20" Text="20%" />
                        <asp:ListItem Value="25" Text="25%" />
                        <asp:ListItem Value="30" Text="30%" />
                        <asp:ListItem Value="35" Text="35%" />
                        <asp:ListItem Value="40" Text="40%" />
                    </asp:DropDownList>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secViewSession" runat="server" TargetControlID="viewSessionPanel" meta:resourcekey="secViewSession" Text="View RDS Session without Users's Permission"/>
            <asp:Panel ID="viewSessionPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbViewSessionUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbViewSessionAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secControlSession" runat="server" TargetControlID="controlSessionPanel" meta:resourcekey="secControlSession" Text="Control RDS Session without Users's Permission"/>
            <asp:Panel ID="controlSessionPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbControlSessionUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbControlSessionAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <scp:CollapsiblePanel id="secDisableCmd" runat="server" TargetControlID="disableCmdPanel" meta:resourcekey="secDisableCmd" Text="Disable Command Prompt"/>
            <asp:Panel ID="disableCmdPanel" runat="server" Height="0" style="overflow:hidden;">
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Users" ID="cbDisableCmdUsers" meta:resourcekey="cbUsers" Checked="false" />
                        </div>
                        <div class="input-group">
                            <asp:CheckBox runat="server" Text="Administrators" meta:resourcekey="cbAdministrators" ID="cbDisableCmdAdministrators" Checked="false" />
                        </div>
                    </div>
                </div>
                <span class="alert alert-info col-xs-12">You may need to enable <strong>Command Prompt</strong> if RemoteApps in this collection won't load.</span>
            </asp:Panel>
		</div>
        <div class="text-right">
            <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="SaveRDSCollection" OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
        </div>
    </div>
</div>

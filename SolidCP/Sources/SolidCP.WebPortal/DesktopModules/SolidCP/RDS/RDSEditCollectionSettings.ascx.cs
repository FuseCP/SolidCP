using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.RemoteDesktopServices;

namespace SolidCP.Portal.RDS
{
    public partial class RDSEditCollectionSettings : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WriteScriptBlock();

            if (!IsPostBack)
            {
                RdsCollection collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                RdsCollectionSettings settings = ES.Services.RDS.GetRdsCollectionSettings(PanelRequest.CollectionID);
                collection.Settings = settings;

                if (collection.Settings == null)
                {
                    collection.Settings = new RdsCollectionSettings
                    {
                        DisconnectedSessionLimitMin = 0,
                        ActiveSessionLimitMin = 0,
                        IdleSessionLimitMin = 0,
                        BrokenConnectionAction = BrokenConnectionActionValues.Disconnect.ToString(),
                        AutomaticReconnectionEnabled = true,
                        TemporaryFoldersDeletedOnExit = true,
                        TemporaryFoldersPerSession = true,
                        ClientDeviceRedirectionOptions = string.Join(",", new List<string>
                        {
                            ClientDeviceRedirectionOptionValues.AudioVideoPlayBack.ToString(),
                            ClientDeviceRedirectionOptionValues.AudioRecording.ToString(),
                            ClientDeviceRedirectionOptionValues.SmartCard.ToString(),
                            ClientDeviceRedirectionOptionValues.Clipboard.ToString(),
                            ClientDeviceRedirectionOptionValues.Drive.ToString(),
                            ClientDeviceRedirectionOptionValues.PlugAndPlayDevice.ToString()
                        }.ToArray()),
                        ClientPrinterRedirected = true,
                        ClientPrinterAsDefault = true,
                        RDEasyPrintDriverEnabled = true,
                        MaxRedirectedMonitors = 16,
                        EncryptionLevel = EncryptionLevel.ClientCompatible.ToString(),
                        SecurityLayer = SecurityLayerValues.Negotiate.ToString(),
                        AuthenticateUsingNLA = true
                    };
                }

                litCollectionName.Text = collection.DisplayName;
                BindControls(collection);
            }
        }

        private void BindControls(RdsCollection collection)
        {
            slDisconnectedSessionLimit.SelectedLimit = collection.Settings.DisconnectedSessionLimitMin;
            slActiveSessionLimit.SelectedLimit = collection.Settings.ActiveSessionLimitMin;
            slIdleSessionLimit.SelectedLimit = collection.Settings.IdleSessionLimitMin;

            if (collection.Settings.BrokenConnectionAction == BrokenConnectionActionValues.Disconnect.ToString())
            {
                chDisconnect.Checked = true;
                chAutomaticReconnection.Enabled = true;
            }
            else
            {
                chEndSession.Checked = true;
                chAutomaticReconnection.Enabled = false;
            }

            chAutomaticReconnection.Checked = collection.Settings.AutomaticReconnectionEnabled;
            chDeleteOnExit.Checked = collection.Settings.TemporaryFoldersDeletedOnExit;
            chUseFolders.Checked = collection.Settings.TemporaryFoldersPerSession;

            if (collection.Settings.ClientDeviceRedirectionOptions != null)
            {
                chAudioVideo.Checked = collection.Settings.ClientDeviceRedirectionOptions.Contains(ClientDeviceRedirectionOptionValues.AudioVideoPlayBack.ToString());
                chAudioRecording.Checked = collection.Settings.ClientDeviceRedirectionOptions.Contains(ClientDeviceRedirectionOptionValues.AudioRecording.ToString());
                chDrives.Checked = collection.Settings.ClientDeviceRedirectionOptions.Contains(ClientDeviceRedirectionOptionValues.Drive.ToString());
                chSmartCards.Checked = collection.Settings.ClientDeviceRedirectionOptions.Contains(ClientDeviceRedirectionOptionValues.SmartCard.ToString());
                chPlugPlay.Checked = collection.Settings.ClientDeviceRedirectionOptions.Contains(ClientDeviceRedirectionOptionValues.PlugAndPlayDevice.ToString());
                chClipboard.Checked = collection.Settings.ClientDeviceRedirectionOptions.Contains(ClientDeviceRedirectionOptionValues.Clipboard.ToString());
            }

            chPrinterRedirection.Checked = collection.Settings.ClientPrinterRedirected;
            chDefaultDevice.Checked = collection.Settings.ClientPrinterAsDefault;
            chDefaultDevice.Enabled = collection.Settings.ClientPrinterRedirected;
            chEasyPrint.Checked = collection.Settings.RDEasyPrintDriverEnabled;
            chEasyPrint.Enabled = collection.Settings.ClientPrinterRedirected;
            tbMonitorsNumber.Text = collection.Settings.MaxRedirectedMonitors.ToString();
            cbAuthentication.Checked = collection.Settings.AuthenticateUsingNLA;
            ddSecurityLayer.SelectedValue = collection.Settings.SecurityLayer;
            ddEncryptionLevel.SelectedValue = collection.Settings.EncryptionLevel;
        }

        private bool EditCollectionSettings()
        {
            try
            {
                RdsCollection collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                collection.Settings.RdsCollectionId = collection.Id;
                collection.Settings = GetSettings(collection.Settings);                
                ES.Services.RDS.EditRdsCollectionSettings(PanelRequest.ItemID, collection);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSCOLLECTIONSETTINGS_NOT_UPDATES", ex);                
                return false;
            }

            return true;
        }

        private RdsCollectionSettings GetSettings(RdsCollectionSettings settings)
        {
            settings.DisconnectedSessionLimitMin = slDisconnectedSessionLimit.SelectedLimit;
            settings.ActiveSessionLimitMin = slActiveSessionLimit.SelectedLimit;
            settings.IdleSessionLimitMin = slIdleSessionLimit.SelectedLimit;
            settings.AutomaticReconnectionEnabled = chAutomaticReconnection.Checked;

            if (chDisconnect.Checked)
            {
                settings.BrokenConnectionAction = BrokenConnectionActionValues.Disconnect.ToString();
            }
            else
            {
                settings.BrokenConnectionAction = BrokenConnectionActionValues.LogOff.ToString();
            }

            settings.TemporaryFoldersDeletedOnExit = chDeleteOnExit.Checked;
            settings.TemporaryFoldersPerSession = chUseFolders.Checked;
            settings.ClientPrinterRedirected = chPrinterRedirection.Checked;
            settings.ClientPrinterAsDefault = chDefaultDevice.Checked;
            settings.RDEasyPrintDriverEnabled = chEasyPrint.Checked;
            settings.MaxRedirectedMonitors = Convert.ToInt32(tbMonitorsNumber.Text);

            List<string> redirectionOptions = new List<string>();

            if (chAudioVideo.Checked)
            {
                redirectionOptions.Add(ClientDeviceRedirectionOptionValues.AudioVideoPlayBack.ToString());
            }

            if (chAudioRecording.Checked)
            {
                redirectionOptions.Add(ClientDeviceRedirectionOptionValues.AudioRecording.ToString());
            }

            if (chSmartCards.Checked)
            {
                redirectionOptions.Add(ClientDeviceRedirectionOptionValues.SmartCard.ToString());
            }

            if (chPlugPlay.Checked)
            {
                redirectionOptions.Add(ClientDeviceRedirectionOptionValues.PlugAndPlayDevice.ToString());
            }

            if (chDrives.Checked)
            {
                redirectionOptions.Add(ClientDeviceRedirectionOptionValues.Drive.ToString());
            }

            if (chClipboard.Checked)
            {
                redirectionOptions.Add(ClientDeviceRedirectionOptionValues.Clipboard.ToString());
            }

            settings.ClientDeviceRedirectionOptions = string.Join(",", redirectionOptions.ToArray());
            settings.AuthenticateUsingNLA = cbAuthentication.Checked;
            settings.SecurityLayer = ddSecurityLayer.SelectedItem.Value;
            settings.EncryptionLevel = ddEncryptionLevel.SelectedItem.Value;            

            return settings;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            EditCollectionSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (EditCollectionSettings())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {                        
            chPrinterRedirection.Attributes["onclick"] = String.Format("TogglePrinterCheckboxes('{0}', '{1}', '{2}');", chPrinterRedirection.ClientID, chDefaultDevice.ClientID, chEasyPrint.ClientID);
            chDisconnect.Attributes["onclick"] = String.Format("EnableReconnectionCheckbox('{0}', true);", chAutomaticReconnection.ClientID);
            chEndSession.Attributes["onclick"] = String.Format("EnableReconnectionCheckbox('{0}', false);", chAutomaticReconnection.ClientID);
            base.OnPreRender(e);
        }

        private void WriteScriptBlock()
        {
            string scriptKey = "RdsSettingsScript";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptKey, @"<script language='javascript' type='text/javascript'>
                        function TogglePrinterCheckboxes(chkId, chDefaultId, chEasyId)
                        {   
                            var chPrinter = document.getElementById(chkId);
                            var chDefaultDevice = document.getElementById(chDefaultId);
                            chDefaultDevice.disabled = !chPrinter.checked;
                            var chEasyPrint = document.getElementById(chEasyId);
                            chEasyPrint.disabled = !chPrinter.checked;
                        }

                        function EnableReconnectionCheckbox(checkBoxId, enabled)
                        {                            
                            var checkBox = document.getElementById(checkBoxId);                            
                            checkBox.disabled = !enabled;
                        }

                        </script>");
            }            
        }
    }
}
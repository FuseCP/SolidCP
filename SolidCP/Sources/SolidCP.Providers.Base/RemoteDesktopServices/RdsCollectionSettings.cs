using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    [Serializable]
    public class RdsCollectionSettings
    {
        public int Id { get; set; }
        public int RdsCollectionId { get; set; }
        public int DisconnectedSessionLimitMin { get; set; }
        public int ActiveSessionLimitMin { get; set; }
        public int IdleSessionLimitMin { get; set; }
        public string BrokenConnectionAction { get; set; }
        public bool AutomaticReconnectionEnabled { get; set; }
        public bool TemporaryFoldersDeletedOnExit { get; set; }
        public bool TemporaryFoldersPerSession { get; set; }
        public string ClientDeviceRedirectionOptions { get; set; }
        public bool ClientPrinterRedirected { get; set; }
        public bool ClientPrinterAsDefault { get; set; }
        public bool RDEasyPrintDriverEnabled { get; set; }
        public int MaxRedirectedMonitors { get; set; }
        public string SecurityLayer { get; set; }
        public string EncryptionLevel { get; set; }
        public bool AuthenticateUsingNLA { get; set; }
    }
}

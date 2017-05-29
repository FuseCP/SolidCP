using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public enum BrokenConnectionActionValues
    {
        Disconnect,
        LogOff
    }

    public enum ClientDeviceRedirectionOptionValues
    {
        AudioVideoPlayBack,
        AudioRecording,
        SmartCard,
        PlugAndPlayDevice,
        Drive,
        Clipboard
    }

    public enum SecurityLayerValues
    {
        RDP,
        Negotiate,
        SSL
    }

    public enum EncryptionLevel
    {
        Low,
        ClientCompatible,
        High,
        FipsCompliant
    }

    public enum CommandLineSettings
    {
        Allow,
        DoNotAllow,
        Require
    }
}

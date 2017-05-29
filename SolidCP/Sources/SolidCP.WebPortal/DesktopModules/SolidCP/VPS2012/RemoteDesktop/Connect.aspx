<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Connect.aspx.cs" Inherits="SolidCP.Portal.VPS2012.RemoteDesktop.Connect" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>
        <asp:Literal ID="litServerName" runat="server"></asp:Literal>
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Remote Desktop Web Connection"></asp:Localize></title>
</head>
<body style="margin:0;">


<script language="vbscript" type="text/vbscript">
<!--
const L_FullScreenWarn1_Text = "Your current security settings do not allow automatically switching to fullscreen mode."
const L_FullScreenWarn2_Text = "You can use ctrl-alt-pause to toggle your remote desktop session to fullscreen mode"
const L_FullScreenTitle_Text = "Remote Desktop Web Connection "
const L_ErrMsg_Text         = "Error connecting to remote computer: "
const L_PlatformCheck_ErrorMessage = "The Remote Desktop Web connection ActiveX control can only run in the 32-bit version of Internet Explorer."

' error messages
const L_DisconnectedCaption_ErrorMessage =  "Remote Desktop Disconnected"
const L_ErrConnectCallFailed_ErrorMessage =  "An error occurred when the client attempted to connect to the remote computer. Check your system memory and then try the connection again."
const L_DisconnectRemoteByServer_ErrorMessage = "The remote computer has ended the connection."
const L_LowMemory_ErrorMessage = "The local computer is low on memory. Close some programs, and then connect to the remote computer again."
const L_SecurityErr_ErrorMessage = "Because of a security error, the client could not connect to the remote computer. Verify that you are logged on to the network, and then try connecting again."
const L_BadServerName_ErrorMessage = "The specified remote computer could not be found. Verify that you have typed the correct computer name or IP address, and then try connecting again."
const L_ConnectFailedProtocol_ErrorMessage = "Because of a protocol error, the client could not connect to the remote computer. Please try connecting to the remote computer again. If the client still fails to connect, contact your network administrator."
const L_CannotLoopBackConnect_ErrorMessage = "The client could not connect. You cannot connect to the console from a console session of the same computer."
const L_NetworkErr_ErrorMessage = "The connection was ended because of a network error. Please try connecting to the remote computer again."
const L_InternalErr_ErrorMessage = "An internal error has occurred."
const L_NotResponding_ErrorMessage = "The client could not connect to the remote computer. Remote connections might not be enabled or the computer might be too busy to accept new connections.It is also possible that network problems are preventing your connection.Please try connecting again later. If the problem continues to occur, contact your administrator."
const L_VersionMismatch_ErrorMessage = "Client and server versions do not match. Please upgrade your client software and then try connecting again."
const L_EncryptionError_ErrorMessage = "Because of an error in data encryption, this session will end. Please try connecting to the remote computer again."
const L_ProtocolErr_ErrorMessage = "Because of a protocol error, this session will be disconnected. Please try connecting to the remote computer again."
const L_IllegalServerName_ErrorMessage = "The specified computer name contains invalid characters. Please verify the name and try again."
const L_ConnectionTimeout_ErrorMessage = "The remote connection has timed out. Please try connecting to the remote computer again."
const L_DisconnectIdleTimeout_ErrorMessage = "The remote session was ended because the idle timeout limit was reached. This limit is set by the server administrator or by network policies."
const L_DisconnectLogonTimeout_ErrorMessage ="The remote session ended because the total logon time limit was reached. This limit is set by the server administrator or by network policies."
const L_ProtocolErrWITHCODE_ErrorMessage  = "Client disconnected due to internal protocol error: "
const L_LicensingTimeout_ErrorMessage = "A licensing error occurred while the client was attempting to connect (Licensing timed out).Please try connecting to the remote computer again."
const L_LicensingNegotFailed_ErrorMessage = "The remote computer disconnected the session because of an error in the licensing protocol.Please try connecting to the remote computer again or contact your server administrator."
const L_DisconnectRemoteByServerTool_ErrorMessage = "The remote session to the remote computer was ended by means of an administration tool.Your administrator might have ended your connection."
const L_LogoffRemoteByServer_ErrorMessage = "The remote session was disconnected because your session was logged off at the remote computer.Your administrator or another user might have ended your connection."
const L_DisconnectByOtherConnection_ErrorMessage = "The remote session was disconnected because another user has connected to the session."
const L_ConnectionBroken_ErrorMessage  = "The connection to the remote computer was broken. This may have been caused by a network error.Please try connecting to the remote computer again."
const L_ServerOutOfMemory_ErrorMessage = "The connection was disconnected because the remote computer is low on memory."
const L_LicenseInternal_ErrorMessage = "The remote session was disconnected because there was an internal error in the remote computer's licensing protocol."
const L_NoLicenseServer_ErrorMessage = "The remote session was disconnected because there are no Terminal Server License Servers available to provide a license.Please contact the server administrator."
const L_NoLicense_ErrorMessage = "The remote session was disconnected because there are no Terminal Server client access licenses available for this computer.Please contact the server administrator."
const L_LicenseBadClientMsg_ErrorMessage = "The remote session was disconnected because the remote computer received an invalid licensing message from this computer."
const L_LicenseHwidDoesntMatch_ErrorMessage = "The remote session was disconnected because the Terminal Server client access license stored on this computer has been modified."
const L_BadClientLicense_ErrorMessage = "The remote session was disconnected because the Terminal Server client access license stored on this computer is in an invalid format."
const L_LicenseCantFinishProtocol_ErrorMessage = "The remote session was disconnected because there were network problems during the licensing protocol.Please try connecting to the remote computer again."
const L_LicenseClientEndedProtocol_ErrorMessage = "The remote session was disconnected because the client prematurely ended the licensing protocol."
const L_LicenseBadClientEncryption_ErrorMessage = "The remote session was disconnected because a licensing message was incorrectly encrypted."
const L_CantUpgradeLicense_ErrorMessage = "The remote session was disconnected because the local computer's client access license could not be upgraded or renewed.Please contact the server administrator."
const L_LicenseNoRemoteConnections_ErrorMessage = "The remote session was disconnected because the remote computer is not licensed to accept remote connections.Please contact the server administrator."
const L_DecompressionFailed_ErrorMessage = "The remote session was disconnected because of a decompression failure at the client side. Please try connecting to the remote computer again."
const L_ServerDeniedConnection_ErrorMessage ="The client could not establish a connection to the remote computer.The most likely causes for this error are:1) Remote connections might not be enabled at the remote computer.2) The maximum number of connections was exceeded at the remote computer.3) A network error occurred while establishing the connection."

resWidth = 0
resHeight = 0
resolution = "<asp:Literal id="resolution" runat="server"/>"

serverName = "<asp:Literal id="serverName" runat="server"/>"
username = "<asp:Literal id="username" runat="server"/>"
domain = ""
password = "<asp:Literal id="password" runat="server"/>"

sub OnControlLoad
   set Control = Document.getElementById("MsRdpClient")
   if Not Control is Nothing then
      if Control.readyState = 4 then
         BtnConnect()
      end if
   end if
end sub


sub BtnConnect

   if not LCase(Navigator.CpuClass) = "x86" then
      msgbox L_PlatformCheck_ErrorMessage
   end if
   
   'Resolution
   MsRdpClient.FullScreen = FALSE
   select case resolution
   case "1"
      MsRdpClient.FullScreen = TRUE
      resWidth  = screen.width
      resHeight = screen.height
   case "2"
      resWidth  = "800"
      resHeight = "600"
   case "3"
      resWidth  = "1024"
      resHeight = "768"
   case "4"
      resWidth  = "1280"
      resHeight = "1024"
   end select
   
   'ResizeWindow resWidth, resHeight
   
    MsRdpClient.server = serverName
    MsRdpClient.UserName = username
    MsRdpClient.Domain = domain
    MsRdpClient.AdvancedSettings2.ClearTextPassword = password
  
    MsRdpClient.DesktopWidth = resWidth
    MsRdpClient.DesktopHeight = resHeight

    MsRdpClient.Width = resWidth
    MsRdpClient.Height = resHeight

    'Device redirection options
    MsRdpClient.AdvancedSettings2.RedirectDrives     = FALSE
    MsRdpClient.AdvancedSettings2.RedirectPrinters   = FALSE
    MsRdpClient.AdvancedSettings2.RedirectPorts      = FALSE
    MsRdpClient.AdvancedSettings2.RedirectSmartCards = FALSE

    'FullScreen title
    MsRdpClient.FullScreenTitle = L_FullScreenTitle_Text & "(" & serverName & ")"

    'Connect
    MsRdpClient.Connect
end sub

sub MsRdpClient_OnConnected()
   ' nothing to do
end sub

sub MsRdpClient_OnDisconnected(disconnectCode)

   extendedDiscReason = MsRdpClient.ExtendedDisconnectReason
   majorDiscReason = disconnectCode And &hFF

   if (disconnectCode = &hB08 or majorDiscReason = 2 or majorDiscReason = 1) and not (extendedDiscReason = 5) then
      window.close()
      exit sub
   end if
   
   errMsgText = L_DisconnectRemoteByServer_ErrorMessage
   if not extendedDiscReason = 0 then
      'Use the extended disconnect code
      select case extendedDiscReason
      case 0   errMsgText  = ""
      case 1   errMsgText  = L_DisconnectRemoteByServerTool_ErrorMessage
      case 2   errMsgText  = L_LogoffRemoteByServer_ErrorMessage
      case 3   errMsgText  = L_DisconnectIdleTimeout_ErrorMessage
      case 4   errMsgText  = L_DisconnectLogonTimeout_ErrorMessage
      case 5   errMsgText  = L_DisconnectByOtherConnection_ErrorMessage
      case 6   errMsgText  = L_ServerOutOfMemory_ErrorMessage
      case 7   errMsgText  = L_ServerDeniedConnection_ErrorMessage
      case 256 errMsgText  = L_LicenseInternal_ErrorMessage
      case 257 errMsgText  = L_NoLicenseServer_ErrorMessage
      case 258 errMsgText  = L_NoLicense_ErrorMessage
      case 259 errMsgText  = L_LicenseBadClientMsg_ErrorMessage
      case 260 errMsgText  = L_LicenseHwidDoesntMatch_ErrorMessage
      case 261 errMsgText  = L_BadClientLicense_ErrorMessage
      case 262 errMsgText  = L_LicenseCantFinishProtocol_ErrorMessage
      case 263 errMsgText  = L_LicenseClientEndedProtocol_ErrorMessage
      case 264 errMsgText  = L_LicenseBadClientEncryption_ErrorMessage
      case 265 errMsgText  = L_CantUpgradeLicense_ErrorMessage
      case 266 errMsgText  = L_LicenseNoRemoteConnections_ErrorMessage
      case else errMsgText = L_ErrMsg_Text
      end select
      if extendedDiscReason > 4096 then
         errMsgText = L_ProtocolErrWITHCODE_ErrorMessage  & errMsgText
      end if
   else
      ' no extended error information, use the disconnect code
      select case disconnectCode
      case 0   errMsgText  = L_ErrMsg_Text
      case 1   errMsgText  = L_ErrMsg_Text
      case 2   errMsgText  = L_ErrMsg_Text
      case 260 errMsgText  = L_BadServerName_ErrorMessage
      case 262 errMsgText  = L_LowMemory_ErrorMessage
      case 264 errMsgText  = L_ConnectionTimeout_ErrorMessage
      case 516 errMsgText  = L_NotResponding_ErrorMessage
      case 518 errMsgText  = L_LowMemory_ErrorMessage
      case 520 errMsgText  = L_BadServerName_ErrorMessage
      case 772 errMsgText  = L_NetworkErr_ErrorMessage
      case 774 errMsgText  = L_LowMemory_ErrorMessage
      case 776 errMsgText  = L_BadServerName_ErrorMessage
      case 1028 errMsgText = L_NetworkErr_ErrorMessage
      case 1030 errMsgText = L_SecurityErr_ErrorMessage
      case 1032 errMsgText = L_IllegalServerName_ErrorMessage
      case 1286 errMsgText = L_EncryptionError_ErrorMessage
      case 1288 errMsgText = L_BadServerName_ErrorMessage
      case 1540 errMsgText = L_BadServerName_ErrorMessage
      case 1542 errMsgText = L_SecurityErr_ErrorMessage
      case 1544 errMsgText = L_LowMemory_ErrorMessage
      case 1796 errMsgText = L_NotResponding_ErrorMessage
      case 1798 errMsgText = L_SecurityErr_ErrorMessage
      case 1800 errMsgText = L_CannotLoopBackConnect_ErrorMessage
      case 2052 errMsgText = L_BadServerName_ErrorMessage
      case 2056 errMsgText = L_LicensingNegotFailed_ErrorMessage
      case 2310 errMsgText = L_SecurityErr_ErrorMessage
      case 2566 errMsgText = L_SecurityErr_ErrorMessage
      case 2822 errMsgText = L_EncryptionError_ErrorMessage
      case 3078 errMsgText = L_EncryptionError_ErrorMessage
      case 3080 errMsgText = L_DecompressionFailed_ErrorMessage
      case 3334 errMsgText = L_ProtocolErr_ErrorMessage
      case 10500 errMsgText = L_ProtocolErr_ErrorMessage
      case else errMsgText = L_InternalErr_ErrorMessage
      end select
   end if
   
   msgbox errMsgText,0,L_DisconnectedCaption_ErrorMessage
   
   window.close()
   
end sub

-->
</script>

<object language="vbscript" id="MsRdpClient"
    onreadystatechange="OnControlLoad"
    classid="CLSID:9059f30f-4eb1-4bd2-9fdc-36f43a218f4a"
    codebase="msrdp.cab#version=5,1,2600,2180"
    width="100" height="100">
</object>

    <form id="AspForm" runat="server">
    </form>
</body>
</html>

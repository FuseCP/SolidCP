' Copyright (c) 2016, SolidCP
' SolidCP Is distributed under the Creative Commons Share-alike license
' 
' SolidCP Is a fork of WebsitePanel:
' Copyright (c) 2014, Outercurve Foundation.
' All rights reserved.
'
' Redistribution and use in source and binary forms, with or without modification,
' are permitted provided that the following conditions are met:
'
' - Redistributions of source code must  retain  the  above copyright notice, this
'   list of conditions and the following disclaimer.
'
' - Redistributions in binary form  must  reproduce the  above  copyright  notice,
'   this list of conditions  and  the  following  disclaimer in  the documentation
'   and/or other materials provided with the distribution.
'
' - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
'   contributors may be used to endorse or  promote  products  derived  from  this
'   software without specific prior written permission.
'
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
' ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
' WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
' DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
' ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
' (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
' LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
' ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
' SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

Imports SolidCP.Providers.Utils
Imports SolidCP.Server.Utils
Imports SolidCP.Providers.Common


Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Win32


Public Class MDaemon
    Inherits HostingServiceProviderBase
    Implements IMailServer

#Region "Constants"
    Public Const ALIAS_PROG_ID As String = "MDUserCOM.MDAlias"
    Public Const ALIAS_ITEM_PROG_ID As String = "MDUserCOM.MDAliasItem"
    Public Const LIST_PROG_ID As String = "MDUserCOM.MDList"
    Public Const LIST_MEMBER_PROG_ID As String = "MDUserCOM.MDListMember"
    Public Const MESSAGE_INFO_PROG_ID As String = "MDUserCOM.MDMessageInfo"
    Public Const RULE_PROG_ID As String = "MDUserCOM.MDRule"
    Public Const USER_PROG_ID As String = "MDUserCOM.MDUser"
    Public Const USER_INFO_PROG_ID As String = "MDUserCOM.MDUserInfo"
    Public Const GATEWAY_INFO_PROG_ID As String = "MDUserCOM.MDGateway"

    Private Const DefaultDomainMaxUsers As Integer = 0
    Private Const DefaultDomainMaxLists As Integer = 0
    Private Const DefaultDomainIP As String = "127.0.0.1"
    Private Const DefaultDomainMaxInactive As Integer = 0
    Private Const DefaultDomainMaxMessageAge As Integer = 0
    Private Const DefaultDomainMaxDeletedIMAPMessageAge As Integer = 0
    Private Const DefaultDomainBind As Boolean = False
    Private Const DefaultDomainRecurseIMAP As Boolean = False
    Private Const DefaultDomainEnableAntiVirus As Boolean = True
    Private Const DefaultDomainEnableAntiSpam As Boolean = True
#End Region

#Region "Properties"
    Public Property EnableIMAP() As Boolean
        Get
            Return Convert.ToBoolean(ProviderSettings("EnableIMAP"))
        End Get
        Set(ByVal value As Boolean)
            ProviderSettings.Settings("EnableIMAP") = value.ToString()
        End Set
    End Property

    Public Property EnablePOP() As Boolean
        Get
            Return Convert.ToBoolean(ProviderSettings("EnablePOP"))
        End Get
        Set(ByVal value As Boolean)
            ProviderSettings.Settings("EnablePOP") = value.ToString()
        End Set
    End Property
#End Region

#Region "Internal Classes"
    Class Service
        Public ComObject As Object
        Public Succeed As Boolean
    End Class
#End Region

#Region "Ctors"
    Public Sub New()

    End Sub
#End Region

#Region "Helper Methods"
    Protected Sub RefreshMailServerCache(ByRef service As Service)
        Try
            Dim appCachePath As String = service.ComObject.GetAppDir()

            If Not String.IsNullOrEmpty(appCachePath) Then
                appCachePath = Path.Combine(appCachePath, "ReloadCache.SEM")
                File.Create(appCachePath).Close()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Overridable Function LoadServiceProvider() As Service
        Dim result As Service = New Service()

        Try

            Dim comObject As Object = CreateObject(USER_PROG_ID)
            result.Succeed = comObject.LoadUserDll()

            If result.Succeed Then
                result.ComObject = comObject
            Else
                result.ComObject = Nothing
                Log.WriteInfo("MDUserCOM.LoadUserDll failed to initialize COM object.")
            End If

        Catch ex As Exception

            Log.WriteError("Couldn't create MDUserCOM.MDUser ActiveX object.", ex)

            result.ComObject = Nothing
            result.Succeed = True

        End Try

        Return result
    End Function

    Protected Overridable Sub UnloadServiceProvider(ByRef service As Service)
        If service.Succeed Then
            service.ComObject.FreeUserDll()
        End If
    End Sub

    Protected Overridable Function CreateMDAliasItem(ByRef service As Service) As Object
        Dim mdAlias As Object = Nothing

        If service.Succeed Then
            Try
                mdAlias = CreateObject(ALIAS_ITEM_PROG_ID)
            Catch ex As Exception
                Log.WriteError("Couldn't create MDUserCOM.MDAliasItem Acivex object.", ex)
            End Try
        End If

        Return mdAlias
    End Function


    Public Overrides Function Install() As String()

        Dim service As Service = LoadServiceProvider()
        Dim file As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_MDAEMONINIDB)

        Dim value As String = ProviderSettings.Settings.Item(Constants.RelayAliasedMail)
        Dim relayAliasedMail As Boolean = Boolean.Parse(value)
        Dim relayAliasedMailValue As String = "No"

        If relayAliasedMail Then
            relayAliasedMailValue = "Yes"
        End If

        WriteProfileString("SPECIAL", Constants.RelayAliasedMail, relayAliasedMailValue, file)
        Return MyBase.Install()

    End Function



    Protected Overridable Function CreateMDAlias(ByRef service As Service) As Object
        Dim mdAlias As Object = Nothing

        If service.Succeed Then
            Try
                mdAlias = CreateObject(ALIAS_PROG_ID)
            Catch ex As Exception
                Log.WriteError("Couldn't create MDUserCOM.MDAlias Acivex object.", ex)
            End Try
        End If

        Return mdAlias
    End Function

    Protected Overridable Function CreateMDList(ByRef service As Service, ByRef listName As String) As Object
        Dim mdList As Object = Nothing

        If service.Succeed Then
            Try
                mdList = CreateObject(LIST_PROG_ID)
                service.ComObject.InitListInfo(mdList, listName)
            Catch ex As Exception
                Log.WriteError("Couldn't create MDUserCOM.MDList ActiveX object.", ex)
            End Try
        End If

        Return mdList
    End Function

    Protected Overridable Function CreateMDUserInfo(ByRef service As Service) As Object
        Dim mdUserInfo As Object = Nothing

        Try
            mdUserInfo = CreateObject(USER_INFO_PROG_ID)
            service.ComObject.InitUserInfo(mdUserInfo)
        Catch ex As Exception
            Log.WriteError("Couldn't create MDUserCOM.MDUserInfo ActiveX object.", ex)
        End Try

        Return mdUserInfo
    End Function

    Protected Overridable Function CreateMDListMember(ByRef service As Service) As Object
        Dim mdListMember As Object = Nothing

        If service.Succeed Then
            Try
                mdListMember = CreateObject(LIST_MEMBER_PROG_ID)
            Catch ex As Exception
                Log.WriteError("Couldn't create MDUserCOM.MDListMember ActiveX object.", ex)
            End Try
        End If

        Return mdListMember
    End Function

    Protected Overridable Function LoadMDGateway(ByRef service As Service) As Object
        Dim mdGateway As Object = Nothing

        If service.Succeed Then
            Try
                mdGateway = CreateObject(GATEWAY_INFO_PROG_ID)
            Catch ex As Exception
                Log.WriteError("Couldn't create MDUserCOM.MDGateway ActiveX object.", ex)
            End Try
        End If

        Return mdGateway
    End Function

    Protected Sub UnloadMDGateway(ByRef service As Service, ByRef mdGateway As Object)
        If service.Succeed Then
            service.ComObject.FreeGateway(mdGateway)
        End If
    End Sub

    Protected Function GetEmailName(ByVal mailbox As String)
        Return mailbox.Substring(0, mailbox.IndexOf("@"))
    End Function

    Protected Function GetDomainName(ByVal mailbox As String)
        Return mailbox.Substring(mailbox.IndexOf("@") + 1)
    End Function

    Protected Overridable Function GetApplicationDir() As String
        Dim service As Service = LoadServiceProvider()

        Try

            Dim appDir As String = service.ComObject.GetAppDir()
            UnloadServiceProvider(service)
            Return appDir

        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get application dir", ex)
        End Try

    End Function


    Protected Overridable Sub PopulateUserInfo(ByRef account As MailAccount, ByRef mdUserInfo As Object)

        If Not account.ForwardingAddresses Is Nothing AndAlso account.ForwardingAddresses.Length > 0 Then
            If String.IsNullOrEmpty(account.FirstName) Then
                mdUserInfo.FwdAddress = String.Join(",", account.ForwardingAddresses)
                mdUserInfo.IsForwarding = True
                mdUserInfo.KeepForwardedMail = account.RetainLocalCopy
                mdUserInfo.FullName = "Mail Forwarding"

            Else
                mdUserInfo.FwdAddress = String.Join(",", account.ForwardingAddresses)
                mdUserInfo.IsForwarding = True
                mdUserInfo.KeepForwardedMail = account.RetainLocalCopy
                mdUserInfo.FullName = String.Concat(account.FirstName, " ", account.LastName)
            End If

        Else
            mdUserInfo.FullName = String.Concat(account.FirstName, " ", account.LastName)
            mdUserInfo.IsForwarding = False
        End If

        mdUserInfo.MailBox = GetEmailName(account.Name)
        mdUserInfo.Domain = GetDomainName(account.Name)
        mdUserInfo.Password = account.Password
        mdUserInfo.Email = account.Name

        ' TRUE if account is configured to auto-extract attachments
        mdUserInfo.AutoDecode = Convert.ToBoolean(account.Item("AutoDecode"))
        mdUserInfo.MailFormat = account.Item("MailFormat")

        mdUserInfo.HideFromEveryone = Convert.ToBoolean(account.Item("HideFromEveryone"))
        mdUserInfo.AllowChangeViaEmail = Convert.ToBoolean(account.Item("AllowChangeViaEmail"))
        mdUserInfo.CheckAddrBook = Convert.ToBoolean(account.Item("CheckAddrBook"))
        mdUserInfo.EncryptEmail = Convert.ToBoolean(account.Item("EncryptEmail"))
        mdUserInfo.UpdateAddrBook = Convert.ToBoolean(account.Item("UpdateAddrBook"))

        If account.MaxMailboxSize > 0 Then
            mdUserInfo.ApplyQuotas = True
            mdUserInfo.MaxDiskSpace = account.MaxMailboxSize
        End If
    End Sub

    Protected Sub PopulateGroupInfo(ByRef group As MailGroup, ByVal verify As Boolean)
        Dim service As Service = LoadServiceProvider()

        Try
            Dim mdList = MailGroupToMDList(service, group)

            Dim errorCode As Integer = MDaemonInterop.MDLISTERR_NOERROR
            ' Verify list before creation
            If verify Then
                errorCode = service.ComObject.VerifyListInfo(mdList)
            End If

            If errorCode = MDaemonInterop.MDLISTERR_NOERROR Then
                If Not service.ComObject.WriteList(mdList) Then
                    Throw New Exception("Could not write group to disk.")
                End If

                ClearListMembers(service, mdList)

                Dim member As String

                For Each member In group.Members
                    service.ComObject.ListAddMember(group.Name, member, String.Empty)
                Next member

                UnloadServiceProvider(service)
            Else
                Throw New Exception(String.Format("Could not verify group. Error code {0}", errorCode))
            End If

            ' force to refresh cache
            RefreshMailServerCache(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't populate group info.", ex)
        End Try
    End Sub 'PopulateGroupInfo

    Protected Overridable Sub ClearListMembers(ByRef service As Service, ByRef mdList As Object)
        Dim members As New ArrayList()

        Dim member As Object = CreateMDListMember(service)

        Dim [next] As Boolean = mdList.GetFirstMember(member)

        While [next]
            If Not (member.Email Is Nothing) And member.Email.Length > 0 Then
                members.Add(member.Email)
            End If
            [next] = mdList.GetNextMember(member)
        End While

        Dim email As String
        For Each email In members
            service.ComObject.ListRemoveMember(mdList.ListName, email)
        Next email
    End Sub 'ClearListMembers

    Protected Sub UpdateUserAccessInfo(ByVal userDbPath As String, ByRef account As MailAccount)
        Dim service As Service = LoadServiceProvider()

        Dim hUser As Integer = service.ComObject.GetByEmail(account.Name)

        If hUser = CInt(MD_HANDLE.MD_BADHANDLE) Then
            Throw New Exception(String.Format("Mailbox '{0}' not found.", account.Name))
        End If

        Dim mdUserInfo As Object = CreateMDUserInfo(service)

        PopulateUserInfo(account, mdUserInfo)
        service.ComObject.FilterUserInfo(mdUserInfo)

        Dim recordExists As Boolean

        Try
            If AccountExists(account.Name) Then

                Dim access As Long = 1

                If account.Enabled Then
                    If EnableIMAP And EnablePOP Then
                        access = 1
                    ElseIf EnablePOP Then
                        access = 2
                    ElseIf EnableIMAP Then
                        access = 3
                    End If
                Else
                    access = 4
                End If

                recordExists = True

                ' Update access info (aka access-type)
                mdUserInfo.AccessType = access

                ' Update mailbox size
                If account.MaxMailboxSize > 0 Then
                    mdUserInfo.ApplyQuotas = True
                    mdUserInfo.MaxDiskSpace = CType(account.MaxMailboxSize * 1000, Long)
                End If

                ' Send the changes
                Dim errorCode As Integer = service.ComObject.VerifyUserInfo(mdUserInfo, CInt(MD_VRFYFLAGS.MDUSERDLL_VRFYALL))
                If errorCode <> CInt(MD_ERROR.MDDLLERR_NOERROR) Then
                    Throw New Exception(String.Format("Could not validate account info. Please make sure that all entries are valid. Error code {0}", errorCode))
                End If

                If Not service.ComObject.SetUserInfo(hUser, mdUserInfo) Then
                    Throw New Exception(String.Format("Could not update  mailbox '{0}'", account.Name))
                End If

            End If

            ' Check whether a user record exists
            If Not recordExists Then
                Throw New Exception("Could not find mailbox info.")
            End If
        Catch ex As Exception
            Throw New Exception("Could not update mailbox access info.", ex)
        End Try
    End Sub

    Protected Sub UpdateUserResponderInfo(ByVal responderDbPath As String, ByVal mailbox As MailAccount)
        Try
            If Not mailbox.ResponderEnabled Then
                If Not File.Exists(responderDbPath) Then
                    Return
                End If
                If GetProfileSection(mailbox.Name, responderDbPath) <> 0 Then
                    If Not DeleteProfileSection(mailbox.Name, responderDbPath) Then
                        Throw New Exception("Could not delete profile section.")
                    End If
                End If
            Else
                If Not File.Exists(responderDbPath) Then
                    Dim stream As FileStream = File.Create(responderDbPath)
                    stream.Close()
                End If
                Dim responderFile As String = Path.Combine(GetApplicationDir(), mailbox.Name + ".rsp")
                If GetProfileSection(mailbox.Name, responderDbPath) <> 0 Then
                    responderFile = GetProfileString(mailbox.Name, "MailBackFile", responderFile, responderDbPath)
                End If
                ' update profile section
                WriteProfileString(mailbox.Name, "MailBackFile", responderFile, responderDbPath)
                WriteProfileString(mailbox.Name, "PassMessage", "No", responderDbPath)
                WriteProfileString(mailbox.Name, "ResponderSubject", mailbox.ResponderSubject, responderDbPath)
                ' update responder message body in responder file.
                Dim sw As New StreamWriter(responderFile)
                Try
                    sw.Write(mailbox.ResponderMessage)
                Finally
                    sw.Dispose()
                End Try
            End If
        Catch ex As Exception
            Throw New Exception("Can't update mailbox responder info.", ex)
        End Try
    End Sub 'UpdateUserResponderInfo

    Private Shared Sub WriteDomainInfo(ByVal domain As MailDomain, ByVal domainDbPath As String)
        ' Domain users quota
        WriteProfileString(domain.Name, "MaxUsers", domain.MaxDomainUsers.ToString(), domainDbPath)
        ' Domain lists quota
        WriteProfileString(domain.Name, "MaxLists", domain.MaxLists.ToString(), domainDbPath)

        Dim strIP As String = DefaultDomainIP
        If Not domain.ServerIP Is Nothing AndAlso domain.ServerIP.Length > 0 Then
            strIP = domain.ServerIP
        End If

        ' IP address
        WriteProfileString(domain.Name, "IP", strIP, domainDbPath)

        ' Delete accounts within this domain if inactive for XX days (0=never)
        WriteProfileString(domain.Name, "MaxInactive", domain.Item("MaxInactiveLimit"), domainDbPath)

        ' Delete messages kept by users within this domain if older than XX days (0=never)
        WriteProfileString(domain.Name, "MaxMessageAge", domain.Item("MaxMessageAge"), domainDbPath)

        ' Delete deleted IMAP messages in this domain older than XX days (0 = never)
        WriteProfileString(domain.Name, "MaxDeletedIMAPMessageAge", domain.Item("MaxDeletedImapMessageAge"), domainDbPath)

        ' Bind to this IP if you want to bind the secondary domain to its IP address
        WriteProfileString(domain.Name, "Bind", YesNoBooleanToString(domain.Item("BindIP")), domainDbPath)

        ' Delete old messages from IMAP folders as well
        WriteProfileString(domain.Name, "RecurseIMAP", YesNoBooleanToString(domain.Item("RecurseIMAP")), domainDbPath)

        ' If AntiVirus for MDaemon is installed, this option enables you the AntiVirus settings to be applied to the selected secondary domain
        WriteProfileString(domain.Name, "EnableAntiVirus", YesNoBooleanToString(domain.Item("EnableAntiVirus")), domainDbPath)

        ' If you want MDaemon's current Spam Filter settings to be applied to the selected secondary domain
        WriteProfileString(domain.Name, "EnableAntiSpam", YesNoBooleanToString(domain.Item("EnableAntiSpam")), domainDbPath)

    End Sub 'WriteDomainInfo

    Private Shared Function YesNoBooleanToString(ByVal val As Object) As String
        Dim resultStr As String = "No"

        If Not val Is Nothing Then
            Dim valueStr As String = val.ToString()

            Select Case valueStr
                Case "True"
                    resultStr = "Yes"
                Case "true"
                    resultStr = "Yes"
                Case "1"
                    resultStr = "Yes"
            End Select
        End If

        Return resultStr
    End Function 'YesNoBooleanToString

    Protected Overridable Function EmailExists(ByRef userName As String) As Boolean
        Dim service As Service = LoadServiceProvider()
        Dim exists As Boolean = False

        Try
            exists = service.ComObject.UserExists(userName)
            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception(String.Format("Can't check whether '{0}' exists", userName), ex)
        End Try

        Return exists
    End Function 'EmailExists

    Protected Overridable Function MailListExists(ByRef maillistName As String) As Boolean
        Return EmailExists(maillistName)
    End Function 'MaillistExists

    Protected Overridable Sub PopulateMailListInfo(ByRef list As MailList, ByRef verify As Boolean)
        Dim service As Service = LoadServiceProvider()

        Try
            Dim mdList As Object = CreateMDList(service, list.Name)

            mdList.CatalogName = String.Empty
            mdList.DefaultMode = 1
            mdList.DigestFlags = 0
            mdList.DigestMBF = "DIGEST"
            mdList.FooterFilePath = list.Item("FooterFilePath")
            mdList.HeaderFilePath = list.Item("HeaderFilePath")
            mdList.KillFilePath = list.Item("SuppressionFilePath")
            'flags
            mdList.ListFlags = MDaemonInterop.MDLIST_AUTOPRUNE Or MDaemonInterop.MDLIST_CRACKMESSAGE Or MDaemonInterop.MDLIST_FORCEUNIQUEID

            Select Case list.PostingMode
                Case PostingMode.MembersCanPost
                    mdList.ListFlags = mdList.ListFlags Or MDaemonInterop.MDLIST_PRIVATE
                Case PostingMode.PasswordProtectedPosting
                    mdList.ListFlags = mdList.ListFlags Or MDaemonInterop.MDLIST_PASSWORDPOST
            End Select

            If list.EnableSubjectPrefix Then
                mdList.ListFlags = mdList.ListFlags Or MDaemonInterop.MDLIST_LISTNAMEINSUBJECT
            End If
            If YesNoBooleanToString(list.Item("ShowThreadNumbersInSubject")) = "Yes" Then
                mdList.ListFlags = mdList.ListFlags Or MDaemonInterop.MDLIST_THREADNUMBINSUBJECT
            End If
            If list.Moderated Then
                mdList.ListFlags = mdList.ListFlags Or MDaemonInterop.MDLIST_MODERATED

                If String.IsNullOrEmpty(list.ModeratorAddress) Then
                    Throw New Exception("Mailing list is supposed to be as moderated, but moderator address not specified.")
                End If

                mdList.ModeratorEmail = list.ModeratorAddress
            End If

            Select Case list.ReplyToMode
                Case ReplyTo.RepliesToSender
                    mdList.ReplyAddress = String.Empty
                Case ReplyTo.RepliesToList
                    mdList.ReplyAddress = list.Name
            End Select




            mdList.ListName = list.Name
            mdList.ListPassword = list.Password
            mdList.MaxLineCount = 0 'DIGEST
            mdList.MaxMembers = Convert.ToInt32(list.Item("MaxMembers"))
            mdList.MaxMessageCount = 0 'DIGEST
            mdList.MaxMessageSize = list.MaxMessageSize
            mdList.NotificationEmail = String.Empty
            mdList.PrecedenceLevel = 60
            mdList.PublicFolderName = String.Empty
            mdList.RemoteHost = String.Empty

            mdList.RoutingLimit = 0
            mdList.SendNotesTo = String.Empty
            mdList.WelcomeFilePath = list.Item("WelcomeFilePath")

            Dim errorCode As Integer = MDaemonInterop.MDLISTERR_NOERROR

            If verify Then
                errorCode = service.ComObject.VerifyListInfo(mdList)
            End If

            If errorCode = MDaemonInterop.MDLISTERR_NOERROR Then
                If Not service.ComObject.WriteList(mdList) Then
                    Throw New Exception("Could not write mail list to disk.")
                End If

                ' Clear list
                ClearListMembers(service, mdList)

                Dim member As String
                For Each member In list.Members
                    service.ComObject.ListAddMember(list.Name, member, String.Empty)
                Next member

                ' force to refresh cache
                RefreshMailServerCache(service)

                UnloadServiceProvider(service)
            Else
                Throw New Exception(String.Format("Could not verify mail list. Error code {0}", errorCode))
            End If
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't populate mail list info.", ex)
        End Try
    End Sub 'PopulateMailListInfo

    Protected Overridable Sub DeleteAlias(ByVal email As String, ByVal [alias] As String)
        Dim service As Service = LoadServiceProvider()
        Try
            If Not service.ComObject.DeleteAlias([alias], email) Then
                Throw New Exception(String.Format("Alias {0} not found", [alias]))
            End If

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't delete alias", ex)
        End Try
    End Sub 'DeleteAlias

    Protected Overridable Function CreateMailboxItem(ByRef mdUserInfo As Object) As MailAccount
        Dim mailbox As New MailAccount()
        mailbox.Name = mdUserInfo.Email

        Dim names() As String = mdUserInfo.FullName.ToString().Split(New String() {" "}, StringSplitOptions.None)

        If names.Length = 2 Then
            mailbox.FirstName = names(0)
            mailbox.LastName = names(1)
        ElseIf names.Length = 1 Then
            mailbox.FirstName = names(0)
        End If

        mailbox.Password = mdUserInfo.Password
        mailbox.Item("Comments") = mdUserInfo.Comments
        mailbox.Item("AutoDecode") = Convert.ToBoolean(mdUserInfo.AutoDecode)
        mailbox.Item("MailFormat") = mdUserInfo.MailFormat
        mailbox.Item("HideFromEveryone") = Convert.ToBoolean(mdUserInfo.HideFromEveryone)
        mailbox.Item("AllowChangeViaEmail") = Convert.ToBoolean(mdUserInfo.AllowChangeViaEmail)
        mailbox.Item("CheckAddrBook") = Convert.ToBoolean(mdUserInfo.CheckAddrBook)
        mailbox.Item("EncryptEmail") = Convert.ToBoolean(mdUserInfo.EncryptEmail)
        mailbox.Item("UpdateAddrBook") = Convert.ToBoolean(mdUserInfo.UpdateAddrBook)
        'forwarding
        Dim isForwarding As Boolean = Convert.ToBoolean(mdUserInfo.IsForwarding)

        If isForwarding Then
            mailbox.DeleteOnForward = True
            mailbox.ForwardingAddresses = CStr(mdUserInfo.FwdAddress).Split(",".ToCharArray())
            mailbox.Item("FwdHost") = mdUserInfo.FwdHost
            mailbox.Item("FwdPort") = mdUserInfo.FwdPort
            mailbox.Item("FwdSendAs") = mdUserInfo.FwdSendAs
        End If

        mailbox.RetainLocalCopy = Convert.ToBoolean(mdUserInfo.KeepForwardedMail)


        mailbox.MaxMailboxSize = ParseLong(mdUserInfo.MaxDiskSpace, 0) / 1000

        Return mailbox
    End Function 'CreateMailboxItem

    Protected Overridable Sub PopulateMailboxAccessInfo(ByVal userDbPath As String, ByVal mailbox As MailAccount)
        Dim service As Service = LoadServiceProvider()

        Dim hUser As Integer = service.ComObject.GetByEmail(mailbox.Name)

        If hUser = CInt(MD_HANDLE.MD_BADHANDLE) Then
            Throw New Exception(String.Format("Mailbox '{0}' not found.", mailbox.Name))
        End If

        Dim mdUserInfo As Object = CreateMDUserInfo(service)

        PopulateUserInfo(mailbox, mdUserInfo)
        service.ComObject.FilterUserInfo(mdUserInfo)

        Dim recordExists As Boolean

        Try
            If AccountExists(mailbox.Name) Then

                Dim access As Long = 1

                Select Case mdUserInfo.AccessType
                    Case 1
                        mailbox.Enabled = True
                        mailbox.Item("EnableIMAP") = True
                        mailbox.Item("EnablePOP") = True

                    Case 2
                        mailbox.Enabled = True
                        mailbox.Item("EnableIMAP") = False
                        mailbox.Item("EnablePOP") = True

                    Case 3
                        mailbox.Enabled = True
                        mailbox.Item("EnableIMAP") = True
                        mailbox.Item("EnablePOP") = False

                    Case Else
                        mailbox.Enabled = False
                        mailbox.Item("EnableIMAP") = False
                        mailbox.Item("EnablePOP") = False

                End Select

                recordExists = True

            End If

            If Not recordExists Then
                Throw New Exception(String.Format("Could not find mailbox '{0}' info.", mailbox.Name))
            End If

        Catch ex As Exception
            Throw New Exception(String.Format("Could not read mailbox '{0}' access info.", mailbox.Name), ex)
        End Try
    End Sub 'PopulateMailboxAccessInfo

    Protected Overridable Sub PopulateMailboxResponderInfo(ByVal responderDbPath As String, ByVal mailbox As MailAccount)
        Try
            If Not File.Exists(responderDbPath) Then
                mailbox.ResponderEnabled = False
                mailbox.ResponderMessage = String.Empty
                Exit Sub
            End If

            Dim retVal As Integer = GetProfileSection(mailbox.Name, responderDbPath)

            If retVal = 0 Then
                mailbox.ResponderEnabled = False
                mailbox.ResponderMessage = String.Empty
            Else
                mailbox.ResponderEnabled = True
                Dim responderFile As String = GetProfileString(mailbox.Name, "MailBackFile", String.Empty, responderDbPath)
                mailbox.ResponderSubject = GetProfileString(mailbox.Name, "ResponderSubject", String.Empty, responderDbPath)

                If responderFile Is Nothing Or responderFile.Length = 0 Then
                    Throw New Exception("Responder file not specified.")
                End If

                If Not File.Exists(responderFile) Then
                    Throw New Exception("Responder file not found.")
                End If

                Using reader As New StreamReader(responderFile)
                    'Dim line As String = String.Empty

                    'Dim builder As StringBuilder = New StringBuilder()

                    'Do
                    '   line = reader.ReadLine()

                    '   If line Is Nothing Then
                    '       Continue Do
                    '   End If

                    '   builder.Append(line)
                    '   builder.Append(Environment.NewLine)
                    'Loop While Not line Is Nothing

                    'mailbox.ResponderMessage = builder.ToString()

                    mailbox.ResponderMessage = reader.ReadToEnd()
                End Using
            End If
        Catch ex As Exception
            Throw New Exception("Can't read mailbox responder info.", ex)
        End Try
    End Sub

    Protected Overridable Function CreateDomainItemFromProfile(ByVal domainName As String, ByVal profilePath As String) As MailDomain
        Dim item As New MailDomain()
        item.Name = domainName
        item.MaxDomainUsers = DefaultDomainMaxUsers
        item.MaxLists = DefaultDomainMaxLists
        item.ServerIP = DefaultDomainIP
        item.Item("MaxInactiveLimit") = DefaultDomainMaxInactive
        item.Item("MaxMessageAge") = DefaultDomainMaxMessageAge
        item.Item("MaxDeletedImapMessageAge") = DefaultDomainMaxDeletedIMAPMessageAge
        item.Item("BindIP") = DefaultDomainBind
        item.Item("RecurseIMAP") = DefaultDomainRecurseIMAP
        item.Item("EnableAntiVirus") = DefaultDomainEnableAntiVirus
        item.Item("EnableAntiSpam") = DefaultDomainEnableAntiSpam

        Dim retVal As Integer = GetProfileSection(domainName, profilePath)
        If retVal <> 0 Then
            Dim strMaxUsers As String = GetProfileString(domainName, "MaxUsers", DefaultDomainMaxUsers.ToString(), profilePath)
            item.MaxDomainUsers = ParseInt32(strMaxUsers, DefaultDomainMaxUsers)

            Dim strMaxLists As String = GetProfileString(domainName, "MaxLists", DefaultDomainMaxLists.ToString(), profilePath)
            item.MaxLists = ParseInt32(strMaxLists, DefaultDomainMaxLists)

            Dim strIP As String = GetProfileString(domainName, "IP", DefaultDomainIP, profilePath)
            item.ServerIP = strIP

            item.Item("MaxInactiveLimit") = GetProfileString(domainName, "MaxInactive", DefaultDomainMaxInactive.ToString(), profilePath)

            item.Item("MaxMessageAge") = GetProfileString(domainName, "MaxMessageAge", DefaultDomainMaxMessageAge.ToString(), profilePath)

            item.Item("MaxDeletedImapMessageAge") = GetProfileString(domainName, "MaxDeletedIMAPMessageAge", DefaultDomainMaxDeletedIMAPMessageAge.ToString(), profilePath)

            item.Item("BindIP") = GetProfileString(domainName, "Bind", YesNoBooleanToString(DefaultDomainBind), profilePath)

            item.Item("RecurseIMAP") = GetProfileString(domainName, "RecurseIMAP", YesNoBooleanToString(DefaultDomainRecurseIMAP), profilePath)

            item.Item("EnableAntiVirus") = GetProfileString(domainName, "EnableAntiVirus", YesNoBooleanToString(DefaultDomainEnableAntiVirus), profilePath)

            item.Item("EnableAntiSpam") = GetProfileString(domainName, "EnableAntiSpam", YesNoBooleanToString(DefaultDomainEnableAntiSpam), profilePath)
        End If
        Return item
    End Function 'CreateDomainItemFromProfile

    Protected Overridable Function MDListToMailGroup(ByRef mdList As Object) As MailGroup
        Dim group As New MailGroup()

        group.Name = mdList.ListName
        group.Item("CatalogName") = mdList.CatalogName

        group.Item("DefaultMode") = mdList.DefaultMode.ToString()
        group.Item("DigestFlags") = mdList.DigestFlags.ToString()
        group.Item("DigestMBF") = mdList.DigestMBF

        group.Item("FooterFilePath") = mdList.FooterFilePath
        group.Item("HeaderFilePath") = mdList.HeaderFilePath
        group.Item("KillFilePath") = mdList.KillFilePath
        group.Item("ListFlags") = mdList.ListFlags.ToString()
        group.Item("GroupPassword") = mdList.ListPassword
        group.Item("ModeratorEmail") = mdList.ModeratorEmail
        group.Item("NotificationEmail") = mdList.NotificationEmail

        group.Item("MaxLineCount") = mdList.MaxLineCount.ToString()
        group.Item("MaxMembers") = mdList.MaxMembers.ToString()
        group.Item("MaxMessageCount") = mdList.MaxMessageCount.ToString()
        group.Item("MaxMessageSize") = mdList.MaxMessageSize.ToString()

        'mdList.PrecedenceLevel = 50
        group.Item("PublicFolderName") = mdList.PublicFolderName
        group.Item("RemoteHost") = mdList.RemoteHost
        group.Item("ReplyAddress") = mdList.ReplyAddress
        group.Item("RoutingLimit") = mdList.RoutingLimit.ToString()
        group.Item("SendNotesTo") = mdList.SendNotesTo
        group.Item("WelcomeFilePath") = mdList.WelcomeFilePath

        Return group
    End Function

    Protected Overridable Function MailGroupToMDList(ByRef service As Service, ByRef group As MailGroup) As Object
        Dim mdList As Object = Nothing

        If service.Succeed Then
            mdList = CreateMDList(service, group.Name)
            mdList.CatalogName = group.Item("CatalogName")
            mdList.DefaultMode = ParseLong(group.Item("DefaultMode"), 1)
            mdList.DigestFlags = ParseLong(group.Item("DigestFlags"), 0)

            mdList.DigestMBF = "DIGEST"

            mdList.FooterFilePath = group.Item("FooterFilePath")
            mdList.HeaderFilePath = group.Item("HeaderFilePath")
            mdList.KillFilePath = group.Item("KillFilePath")

            mdList.ListFlags = MDaemonInterop.MDLIST_AUTOPRUNE Or MDaemonInterop.MDLIST_USELISTNAME Or MDaemonInterop.MDLIST_CRACKMESSAGE Or MDaemonInterop.MDLIST_FORCEUNIQUEID

            mdList.ListName = group.Name
            mdList.ListPassword = group.Item("GroupPassword")
            mdList.ModeratorEmail = group.Item("ModeratorEmail")
            mdList.NotificationEmail = group.Item("NotificationEmail")

            mdList.MaxLineCount = ParseLong(group.Item("MaxLineCount"), 0)
            mdList.MaxMembers = ParseLong(group.Item("MaxMembers"), 0)
            mdList.MaxMessageCount = ParseLong(group.Item("MaxMessageCount"), 0)
            mdList.MaxMessageSize = ParseLong(group.Item("MaxMessageSize"), 0)

            mdList.PrecedenceLevel = 50

            mdList.PublicFolderName = group.Item("PublicFolderName")
            mdList.RemoteHost = group.Item("RemoteHost")
            mdList.ReplyAddress = group.Item("ReplyAddress")
            mdList.RoutingLimit = ParseLong(group.Item("RoutingLimit"), 0)
            mdList.SendNotesTo = group.Item("SendNotesTo")
            mdList.WelcomeFilePath = group.Item("WelcomeFilePath")
        End If

        Return mdList
    End Function

    Protected Overridable Function CreateMailGroupItem(ByRef service As Service, ByRef mdList As Object) As MailGroup
        Dim group As MailGroup = Nothing

        group = MDListToMailGroup(mdList)

        Dim member As Object = CreateMDListMember(service)
        Dim members As New List(Of String)

        Dim [next] As Boolean = mdList.GetFirstMember(member)

        While [next]
            If Not member.Email Is Nothing And member.Email.Length > 0 Then
                members.Add(member.Email)
            End If
            [next] = mdList.GetNextMember(member)
        End While

        group.Members = members.ToArray()

        Return group
    End Function 'CreateMailGroupItem


    Protected Overridable Function CreateMailListItem(ByRef service As Service, ByRef mdList As Object) As MailList
        Dim list As New MailList()
        list.Name = mdList.ListName

        list.Password = mdList.ListPassword
        list.ModeratorAddress = mdList.ModeratorEmail
        list.MaxMessageSize = mdList.MaxMessageSize
        list.Item("MaxMembers") = mdList.MaxMembers

        list.Item("FooterFilePath") = mdList.FooterFilePath
        list.Item("HeaderFilePath") = mdList.HeaderFilePath
        list.Item("SuppressionFilePath") = mdList.KillFilePath
        list.Item("WelcomeFilePath") = mdList.WelcomeFilePath

        list.Moderated = (mdList.ListFlags And MDaemonInterop.MDLIST_MODERATED) = MDaemonInterop.MDLIST_MODERATED
        If (mdList.ListFlags And MDaemonInterop.MDLIST_PRIVATE) = MDaemonInterop.MDLIST_PRIVATE Then
            list.PostingMode = PostingMode.MembersCanPost
        ElseIf (mdList.ListFlags And MDaemonInterop.MDLIST_PASSWORDPOST) = MDaemonInterop.MDLIST_PASSWORDPOST Then
            list.PostingMode = PostingMode.PasswordProtectedPosting
        Else
            list.PostingMode = PostingMode.AnyoneCanPost
        End If



        If mdList.ReplyAddress.Length > 0 Then
            list.ReplyToMode = ReplyTo.RepliesToList
        ElseIf mdList.ReplyAddress.Length = 0 Then
            list.ReplyToMode = ReplyTo.RepliesToSender
        End If


        If (mdList.ListFlags And MDaemonInterop.MDLIST_LISTNAMEINSUBJECT) = MDaemonInterop.MDLIST_LISTNAMEINSUBJECT Then
            list.Item("ShowNameInSubject") = "True"
        End If

        If (mdList.ListFlags And MDaemonInterop.MDLIST_THREADNUMBINSUBJECT) = MDaemonInterop.MDLIST_THREADNUMBINSUBJECT Then
            list.Item("ShowThreadNumbersInSubject") = "True"
        End If

        If service.Succeed Then
            Dim member As Object = CreateMDListMember(service)
            Dim members As New List(Of String)
            Dim [next] As Boolean = mdList.GetFirstMember(member)

            While [next]
                If Not (member.Email Is Nothing) And member.Email.Length > 0 Then
                    members.Add(member.Email)
                End If
                [next] = mdList.GetNextMember(member)
            End While

            list.Members = members.ToArray()
        End If

        Return list
    End Function 'CreateMailListItem

    Protected Overridable Function GetAllLists(ByRef service As Service) As String()
        Dim arrayList As New List(Of String)
        Dim objects As Object = Nothing

        Try
            If service.Succeed Then
                service.ComObject.GetMailingLists(objects)
                Dim lists As Array = TryCast(objects, Array)

                If lists Is Nothing Then
                    Exit Try
                End If

                Dim list As Object

                For Each list In lists
                    Dim listName As String = list.ToString()
                    If listName Is Nothing Then
                        Continue For
                    End If
                    If listName.Length = 0 Then
                        Continue For
                    End If

                    arrayList.Add(listName)
                Next list
            End If
        Catch ex As Exception
            Throw New Exception("Can't get the list of mailing lists", ex)
        End Try

        Return arrayList.ToArray()
    End Function 'GetAllLists
#End Region

#Region "Convert Routines"
    Private Shared Function ParseLong(ByVal strValue As String, ByVal defaultValue As Long) As Long
        Dim ret As Long = defaultValue
        Try
            ret = Long.Parse(strValue)
        Catch
        End Try
        Return ret
    End Function

    Private Shared Function ParseInt32(ByVal strValue As String, ByVal defaultValue As Integer) As Integer
        Dim ret As Integer = defaultValue
        Try
            ret = Int32.Parse(strValue)
        Catch
        End Try
        Return ret
    End Function 'ParseInt32

    Private Shared Function ParseYesNoBoolean(ByVal strValue As String, ByVal defaultValue As Boolean) As Boolean
        Dim ret As Boolean = defaultValue
        Dim val As String = strValue.Trim().ToLower()
        If val = "yes" Then
            ret = True
        Else
            If val = "no" Then
                ret = False
            End If
        End If
        Return ret
    End Function 'ParseYesNoBoolean
#End Region

#Region "Profile Section"
    Private Shared Function DeleteProfileSection(ByVal section As String, ByVal file As String) As Boolean
        Dim key As String = Nothing
        Dim strValue As String = Nothing
        Dim retVal As Integer = MDaemonInterop.WritePrivateProfileString(section, key, strValue, file)
        Return retVal <> 0
    End Function 'DeleteProfileSection

    Private Shared Function WriteProfileSection(ByVal section As String, ByVal file As String) As Boolean
        Dim content As String = String.Empty
        Dim retVal As Integer = MDaemonInterop.WritePrivateProfileSection(section, content, file)
        Return retVal <> 0
    End Function 'WriteProfileSection

    Private Shared Function WriteProfileString(ByVal section As String, ByVal key As String, ByVal strValue As String, ByVal file As String) As Boolean
        Dim retVal As Integer = MDaemonInterop.WritePrivateProfileString(section, key, strValue, file)
        Return retVal <> 0
    End Function 'WriteProfileString

    Private Shared Function GetProfileString(ByVal section As String, ByVal key As String, ByVal defaultValue As String, ByVal file As String) As String
        Dim retVal As Integer = 0
        Dim retString As New String(" "c, &H100)
        retVal = MDaemonInterop.GetPrivateProfileString(section, key, defaultValue, retString, &HFF, file)
        If retVal = 0 Then
            retString = Nothing
        Else
            retString = Trim(retString).Replace(CStr(ControlChars.NullChar), "")
        End If
        Return retString
    End Function 'GetProfileString

    Private Shared Function GetProfileSection(ByVal section As String, ByVal file As String) As Integer
        Dim retVal As Integer = 0
        Dim retString As String = String.Empty
        retVal = MDaemonInterop.GetPrivateProfileSection(section, retString, &HFF, file)
        Return retVal
    End Function 'GetProfileSection
#End Region

    Public Function AccountExists(ByVal mailboxName As String) As Boolean Implements IMailServer.AccountExists
        Dim service As Service = LoadServiceProvider()
        Dim exists As Boolean = False

        If service.Succeed Then
            exists = service.ComObject.UserExists(mailboxName)
        End If

        UnloadServiceProvider(service)

        Return exists
    End Function

    Public Sub AddDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.AddDomainAlias
        Dim service As Service = LoadServiceProvider()

        Try
            aliasName = String.Concat("*@", aliasName)
            domainName = String.Concat("*@", domainName)

            Dim succeed As Boolean = service.ComObject.CreateAlias(domainName, aliasName)

            If Not succeed Then
                Throw New Exception("Can't add domain alias.")
            End If

            ' force to refresh cache
            RefreshMailServerCache(service)

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)

            Throw New Exception("Can't add domain alias.", ex)
        End Try

    End Sub

    Public Sub CreateAccount(ByVal mailbox As MailAccount) Implements IMailServer.CreateAccount
        Dim service As Service = LoadServiceProvider()

        Try
            Dim mdUserInfo As Object = CreateMDUserInfo(service)
            PopulateUserInfo(mailbox, mdUserInfo)
            service.ComObject.FilterUserInfo(mdUserInfo)
            Dim errorCode As Integer = service.ComObject.AddUser(mdUserInfo)

            Select Case CType(errorCode, MD_ERROR)
                Case MD_ERROR.MDDLLERR_NOERROR
                    Dim userDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_USERLISTDB)

                    UpdateUserAccessInfo(userDbPath, mailbox)

                    Dim responderDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_AUTORESPDB)

                    UpdateUserResponderInfo(responderDbPath, mailbox)
                    'service.ComObject.ReloadUsers()
                Case MD_ERROR.MDDLLERR_USEREXISTS
                    Throw New Exception("Mailbox is already registered.")
                Case Else
                    Throw New Exception(String.Format("Could not add user. Error code {0}", errorCode))
            End Select

            ' force to refresh cache
            'RefreshMailServerCache(service)

            ' force to reload DLL data
            'service.ComObject.ReloadUsers()

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't create mailbox", ex)
        End Try
    End Sub

    Public Sub CreateDomain(ByVal domain As MailDomain) Implements IMailServer.CreateDomain
        Dim service As Service = LoadServiceProvider()

        Try
            Dim domainDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_DOMAINDB)
            Dim retVal As Integer = GetProfileSection(domain.Name, domainDbPath)
            If retVal <> 0 Then
                Throw New Exception(String.Format("Domain '{0}' already exists on the server", domain.Name))
            End If
            If Not WriteProfileSection(domain.Name, domainDbPath) Then
                Throw New Exception(String.Format("Could not create profile section in '{0}' file.", domainDbPath))
            End If
            WriteDomainInfo(domain, domainDbPath)

            ' force to refresh cache
            RefreshMailServerCache(service)

            ' force to reload DLL data
            service.ComObject.ReloadUsers()

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't create domain", ex)
        End Try
    End Sub

    Public Sub CreateGroup(ByVal group As MailGroup) Implements IMailServer.CreateGroup
        Try
            If GroupExists(group.Name) Then
                Throw New Exception(String.Format("Group {0} already exists.", group.Name))
            End If
            PopulateGroupInfo(group, True)
        Catch ex As Exception
            Throw New Exception("Can't create group.", ex)
        End Try
    End Sub

    Public Sub CreateList(ByVal maillist As MailList) Implements IMailServer.CreateList
        Try
            If MailListExists(maillist.Name) Then
                Throw New Exception(String.Format("Mail list {0} already exists.", maillist.Name))
            End If

            PopulateMailListInfo(maillist, False)
        Catch ex As Exception
            Throw New Exception("Can't create mail list.", ex)
        End Try
    End Sub

    Public Sub DeleteAccount(ByVal mailboxName As String) Implements IMailServer.DeleteAccount
        Dim service As Service = LoadServiceProvider()
        Try
            Dim res As Boolean = service.ComObject.DeleteUser(mailboxName, CInt(MD_DELFLAGS.MDUSERDLL_DDELETEALL))
            If Not res Then
                Throw New Exception("Can't delete mailbox")
            End If

            UnloadServiceProvider(service)

            ' force to refresh cache
            RefreshMailServerCache(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't delete mailbox", ex)
        End Try
    End Sub

    Public Function MailAliasExists(ByVal mailAliasName As String) As Boolean Implements IMailServer.MailAliasExists
        Dim path As String = GetAppFolderPath() + "Alias.dat"
        Dim split As String()
        Using sr As StreamReader = New StreamReader(path)
            Dim line As String
            Do
                line = sr.ReadLine()
                If (Not String.IsNullOrEmpty(line)) Then
                    split = line.Split(New [Char]() {"="c})
                Else
                    Continue Do
                End If
                If mailAliasName.Equals(split(0).Trim) Then
                    Return True
                End If
            Loop Until line Is Nothing
            sr.Close()
        End Using
        Return False
    End Function

    Public Function GetMailAliases(ByVal domainName As String) As MailAlias() Implements IMailServer.GetMailAliases
        Dim aliases As List(Of MailAlias) = New List(Of MailAlias)
        Dim path As String = GetAppFolderPath() + "Alias.dat"
        Dim split As String()
        Using sr As StreamReader = New StreamReader(path)
            Dim line As String
            Do
                line = sr.ReadLine()
                If (Not String.IsNullOrEmpty(line)) Then
                    split = line.Split(New [Char]() {"="c})
                Else
                    Continue Do
                End If
                If domainName.Equals(GetDomainName(split(0).Trim)) Then
                    Dim mailAlias As New MailAlias()
                    mailAlias.Name = split(0).Trim
                    mailAlias.ForwardTo = split(1).Trim
                    aliases.Add(mailAlias)
                End If
            Loop Until line Is Nothing
            sr.Close()
        End Using


        Return aliases.ToArray
    End Function

    Public Function GetMailAlias(ByVal mailAliasName As String) As MailAlias Implements IMailServer.GetMailAlias
        Dim mailAlias As New MailAlias
        Dim newMailAlias As New MailAlias

        If AccountExists(mailAliasName) Then
            Try
                Dim mailAccount As MailAccount = GetAccount(mailAliasName)
                newMailAlias.Name = mailAccount.Name
                newMailAlias.ForwardTo = mailAccount.ForwardingAddresses(0)
                'delete incorrect account
                DeleteAccount(mailAliasName)
                'recreate mail alias 
                CreateMailAlias(newMailAlias)
                Return newMailAlias
            Catch ex As Exception
                'do nothing
            End Try
        End If

        Dim path As String = GetAppFolderPath() + "Alias.dat"
        Dim split As String()
        Using sr As StreamReader = New StreamReader(path)
            Dim line As String
            Do
                line = sr.ReadLine()
                If (Not String.IsNullOrEmpty(line)) Then
                    split = line.Split(New [Char]() {"="c})
                Else
                    Continue Do
                End If
                If mailAliasName.Equals(split(0).Trim) Then
                    mailAlias.Name = split(0).Trim
                    mailAlias.ForwardTo = split(1).Trim
                    Exit Do
                End If
            Loop Until line Is Nothing
            sr.Close()
        End Using
        Return mailAlias
    End Function

    Public Sub CreateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.CreateMailAlias

        Dim service As Service = LoadServiceProvider()
        Dim succeed As Boolean = service.ComObject.CreateAlias(mailAlias.ForwardTo, mailAlias.Name)

        If Not succeed Then
            Throw New Exception(String.Format("Could not create mail alias {0}", mailAlias.Name))
        End If
    End Sub

    Public Sub UpdateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.UpdateMailAlias
        DeleteMailAlias(mailAlias.Name)
        'recreate alias
        CreateMailAlias(mailAlias)
    End Sub

    Public Sub DeleteMailAlias(ByVal mailAliasName As String) Implements IMailServer.DeleteMailAlias
        Dim service As Service = LoadServiceProvider()
        Dim mailAlias As MailAlias = GetMailAlias(mailAliasName)

        Dim succeed As Boolean = service.ComObject.DeleteAlias(mailAlias.ForwardTo, mailAlias.Name)

        If Not succeed Then
            Throw New Exception(String.Format("Could not delete mail alias {0}", mailAlias.Name))
        End If
    End Sub

    Public Sub DeleteDomain(ByVal domainName As String) Implements IMailServer.DeleteDomain
        Dim service As Service = LoadServiceProvider()

        Try
            service.ComObject.DeleteDomain(domainName)

            ' force to refresh cache
            RefreshMailServerCache(service)

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't delete domain", ex)
        End Try
    End Sub

    Public Overrides Sub DeleteServiceItems(ByVal items() As ServiceProviderItem)

        For Each item As ServiceProviderItem In items

            If (TypeOf item Is MailDomain) Then

                Try
                    DeleteDomain(item.Name)
                Catch ex As Exception
                    Log.WriteError(String.Format("Error deleting '{0}' SmarterMail domain", item.Name), ex)
                End Try

            End If

        Next

    End Sub


    Public Sub DeleteDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.DeleteDomainAlias
        Try
            If Not DomainAliasExists(domainName, aliasName) Then
                Throw New Exception(String.Format("Domain alias {0} does not exist", aliasName))
            End If
            Dim [alias] As String = "*@" + aliasName
            Dim domain As String = "*@" + domainName
            DeleteAlias(domain, [alias])
        Catch ex As Exception
            Throw New Exception("Can't delete domain alias", ex)
        End Try
    End Sub

    Public Sub DeleteGroup(ByVal groupName As String) Implements IMailServer.DeleteGroup
        DeleteList(groupName)
    End Sub

    Public Sub DeleteList(ByVal maillistName As String) Implements IMailServer.DeleteList
        Dim service As Service = LoadServiceProvider()

        Try
            service.ComObject.DeleteList(maillistName)
            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't delete list", ex)
        End Try

    End Sub

    Public Function DomainAliasExists(ByVal domainName As String, ByVal aliasName As String) As Boolean Implements IMailServer.DomainAliasExists
        Dim exists As Boolean = False

        Try
            Dim aliases As String() = GetDomainAliases(domainName)
            Dim [alias] As String
            For Each [alias] In aliases
                If String.Compare([alias], aliasName, True) = 0 Then
                    exists = True
                    Exit For
                End If
            Next [alias]
        Catch ex As Exception
            Throw New Exception("Can't check whether domain alias exists", ex)
        End Try

        Return exists
    End Function

    Public Function DomainExists(ByVal domainName As String) As Boolean Implements IMailServer.DomainExists
        Try
            Dim ret As Boolean = False
            Dim domains() As String = GetDomains()
            Dim domain As String
            For Each domain In domains
                If String.Compare(domain, domainName, True) = 0 Then
                    ret = True
                    Exit For
                End If
            Next domain
            Return ret
        Catch ex As Exception
            Throw New Exception("Can't check whether domain exists", ex)
        End Try
    End Function

    Public Function GetDomains() As String() Implements IMailServer.GetDomains
        Dim service As Service = LoadServiceProvider()

        Try
            Dim domainsCount As Integer = service.ComObject.GetDomainCount()
            Dim domains() As Object = New Object() {domainsCount}
            service.ComObject.GetDomainNames(domains)

            Dim ret As New ArrayList()
            Dim domain As Object
            For Each domain In domains
                ret.Add(domain.ToString())
            Next domain

            UnloadServiceProvider(service)

            Return CType(ret.ToArray(GetType(String)), String())
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get the list of domains", ex)
        End Try
    End Function 'GetDomains

    Public Function GetAccount(ByVal mailboxName As String) As MailAccount Implements IMailServer.GetAccount
        Dim service As Service = LoadServiceProvider()
        Dim mailbox As MailAccount = Nothing

        Try
            Dim hUser As Integer = service.ComObject.GetByEmail(mailboxName)

            If hUser = CInt(MD_HANDLE.MD_BADHANDLE) Then
                Throw New Exception(String.Format("Mailbox '{0}' not found.", mailboxName))
            End If

            Dim mdUserInfo As Object = CreateMDUserInfo(service)
            service.ComObject.GetUserInfo(hUser, mdUserInfo)

            mailbox = CreateMailboxItem(mdUserInfo)

            Dim userDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_USERLISTDB)
            PopulateMailboxAccessInfo(userDbPath, mailbox)

            Dim responderDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_AUTORESPDB)
            PopulateMailboxResponderInfo(responderDbPath, mailbox)

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get mailbox", ex)
        End Try

        Return mailbox
    End Function

    Public Function GetAccounts(ByVal domainName As String) As MailAccount() Implements IMailServer.GetAccounts
        Dim service As Service = LoadServiceProvider()
        Dim accounts As List(Of MailAccount) = New List(Of MailAccount)

        Try
            Dim badHandle As Integer = CType(MD_HANDLE.MD_BADHANDLE, Integer)
            Dim hUser As Integer = service.ComObject.FindFirst()

            Do
                If hUser = badHandle Then
                    Continue Do
                End If

                Dim domain As String = service.ComObject.GetDomain(hUser)
                If String.Compare(domain, domainName, True) = 0 Then
                    Dim mailbox As String = service.ComObject.GetEmail(hUser)
                    Dim fullname As String = service.ComObject.GetFullName(hUser)

                    Dim account As MailAccount = New MailAccount()
                    account.Name = mailbox
                    Dim names() As String = fullname.Split(New String() {" "}, StringSplitOptions.None)

                    If names.Length = 2 Then
                        account.FirstName = names(0)
                        account.LastName = names(1)
                    ElseIf names.Length = 1 Then
                        account.FirstName = names(0)
                    End If

                    accounts.Add(account)
                End If
                hUser = service.ComObject.FindNext(hUser)
            Loop While Not hUser = badHandle

            service.ComObject.FindClose()
            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get the list of domain users", ex)
        End Try

        Return accounts.ToArray()
    End Function

    Public Function GetDomain(ByVal domainName As String) As MailDomain Implements IMailServer.GetDomain
        Dim service As Service = LoadServiceProvider()
        Dim domainItem As MailDomain = Nothing

        Try
            If Not DomainExists(domainName) Then
                Throw New Exception(String.Format("Domain '{0}' does not exist", domainName))
            End If

            Dim domainDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_DOMAINDB)
            domainItem = CreateDomainItemFromProfile(domainName, domainDbPath)

            ' read catch-all and abuse accounts
            Dim postmasterAlias As String = String.Concat("Postmaster@", domainItem.Name)
            Dim abuseAlias As String = String.Concat("Abuse@", domainItem.Name)

            Dim mdAlias As Object = CreateMDAlias(service)
            Dim aliasItem As Object = CreateMDAliasItem(service)
            Dim result As Boolean = mdAlias.GetFirstAlias(aliasItem)

            While result
                If String.Compare(postmasterAlias, aliasItem.Alias, True) = 0 Then
                    domainItem.CatchAllAccount = GetEmailName(aliasItem.Email)
                ElseIf String.Compare(abuseAlias, aliasItem.Alias, True) = 0 Then
                    domainItem.AbuseAccount = GetEmailName(aliasItem.Email)
                End If

                If Not String.IsNullOrEmpty(domainItem.CatchAllAccount) AndAlso Not String.IsNullOrEmpty(domainItem.AbuseAccount) Then
                    Exit While
                End If

                result = mdAlias.GetNextAlias(aliasItem)
            End While

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get domain info", ex)
        End Try

        Return domainItem
    End Function

    Public Function GetDomainAliases(ByVal domainName As String) As String() Implements IMailServer.GetDomainAliases
        Dim service As Service = LoadServiceProvider()
        Dim aliases As New ArrayList()
        Dim domainPattern As String = "*@" + domainName

        Try
            Dim mdAlias As Object = CreateMDAlias(service)
            Dim aliasItem As Object = CreateMDAliasItem(service)
            Dim result As Boolean = mdAlias.GetFirstAlias(aliasItem)
            While result
                Dim domain As String = aliasItem.Email
                If String.Compare(domain, domainPattern, True) = 0 Then
                    Dim [alias] As String = GetDomainName(aliasItem.Alias)
                    aliases.Add([alias])
                End If
                result = mdAlias.GetNextAlias(aliasItem)
            End While
            UnloadServiceProvider(service)
            Return CType(aliases.ToArray(GetType(String)), String())
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get the list of domain aliases", ex)
        End Try
    End Function

    Public Function GetGroup(ByVal groupName As String) As MailGroup Implements IMailServer.GetGroup
        Dim service As Service = LoadServiceProvider()
        Dim mailGroup As MailGroup = Nothing

        Try
            Dim mdList As Object = CreateMDList(service, groupName)
            mailGroup = CreateMailGroupItem(service, mdList)
            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get group", ex)
        End Try

        Return mailGroup
    End Function

    Public Function GetGroups(ByVal domainName As String) As MailGroup() Implements IMailServer.GetGroups
        Dim service As Service = LoadServiceProvider()
        Dim groups As New List(Of MailGroup)

        Try
            Dim lists As String() = GetAllLists(service)
            Dim mdList As Object = Nothing
            Dim listName As String

            For Each listName In lists
                Dim domain As String = GetDomainName(listName)
                If String.Compare(domain, domainName, True) <> 0 Then
                    Continue For
                End If

                mdList = CreateMDList(service, listName)
                If mdList.PrecedenceLevel = 50 Then
                    Dim item As New MailGroup()
                    item.Name = listName
                    groups.Add(item)
                End If
            Next listName

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get the list of domain groups", ex)
        End Try

        Return groups.ToArray()
    End Function

    Public Function GetList(ByVal maillistName As String) As MailList Implements IMailServer.GetList
        Dim service As Service = LoadServiceProvider()
        Dim mailList As MailList = Nothing

        Try
            Dim mdList As Object = CreateMDList(service, maillistName)
            mailList = CreateMailListItem(service, mdList)

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get mail list", ex)
        End Try

        Return mailList
    End Function

    Public Function GetLists(ByVal domainName As String) As MailList() Implements IMailServer.GetLists
        Dim service As Service = LoadServiceProvider()
        Dim arrayList As New List(Of MailList)

        Try
            Dim lists As String() = GetAllLists(service)
            Dim mdList As Object = Nothing
            Dim listName As String

            For Each listName In lists
                Dim domain As String = GetDomainName(listName)
                If String.Compare(domain, domainName, True) <> 0 Then
                    Continue For
                End If

                mdList = CreateMDList(service, listName)
                If mdList.PrecedenceLevel <> 50 Then
                    Dim item As New MailList()
                    item.Name = listName
                    arrayList.Add(item)
                End If
            Next listName

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't get the list of mailing lists.", ex)
        End Try

        Return arrayList.ToArray()
    End Function

    Public Function GroupExists(ByVal groupName As String) As Boolean Implements IMailServer.GroupExists
        Return EmailExists(groupName)
    End Function

    Public Function ListExists(ByVal maillistName As String) As Boolean Implements IMailServer.ListExists
        Return EmailExists(maillistName)
    End Function

    Public Sub UpdateAccount(ByVal mailbox As MailAccount) Implements IMailServer.UpdateAccount
        Dim service As Service = LoadServiceProvider()

        Try
            Dim hUser As Integer = service.ComObject.GetByEmail(mailbox.Name)

            If hUser = CInt(MD_HANDLE.MD_BADHANDLE) Then
                Throw New Exception(String.Format("Mailbox '{0}' not found.", mailbox.Name))
            End If

            Dim mdUserInfo As Object = CreateMDUserInfo(service)

            PopulateUserInfo(mailbox, mdUserInfo)
            service.ComObject.FilterUserInfo(mdUserInfo)

            If mailbox.Enabled = True Then
                mdUserInfo.AccessType = "Y"c
            Else
                mdUserInfo.AccessType = "C"c
            End If

            Dim errorCode As Integer = service.ComObject.VerifyUserInfo(mdUserInfo, CInt(MD_VRFYFLAGS.MDUSERDLL_VRFYALL))
            If errorCode <> CInt(MD_ERROR.MDDLLERR_NOERROR) Then
                Throw New Exception(String.Format("Could not validate account info. Please make sure that all entries are valid. Error code {0}", errorCode))
            End If

            If Not service.ComObject.SetUserInfo(hUser, mdUserInfo) Then
                Throw New Exception(String.Format("Could not update  mailbox '{0}'", mailbox.Name))
            End If

            Dim userDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_USERLISTDB)
            UpdateUserAccessInfo(userDbPath, mailbox)

            Dim responderDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_AUTORESPDB)
            UpdateUserResponderInfo(responderDbPath, mailbox)

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception(String.Format("Could not update mailbox '{0}'", mailbox.Name), ex)
        End Try
    End Sub

    Public Sub UpdateDomain(ByVal domain As MailDomain) Implements IMailServer.UpdateDomain
        Dim service As Service = LoadServiceProvider()

        Try
            Dim postmaster As String = String.Concat(domain.CatchAllAccount, "@", domain.Name)
            Dim abuse As String = String.Concat(domain.AbuseAccount, "@", domain.Name)

            Dim postmasterAlias As String = String.Concat("Postmaster@", domain.Name)
            Dim abuseAlias As String = String.Concat("Abuse@", domain.Name)

            Dim pmOldEmail As String = Nothing
            Dim abOldEmail As String = Nothing

            Dim mdAlias As Object = CreateMDAlias(service)
            Dim aliasItem As Object = CreateMDAliasItem(service)
            Dim result As Boolean = mdAlias.GetFirstAlias(aliasItem)

            While result
                If String.Compare(postmasterAlias, aliasItem.Alias, True) = 0 Then
                    pmOldEmail = aliasItem.Email
                ElseIf String.Compare(abuseAlias, aliasItem.Alias, True) = 0 Then
                    abOldEmail = aliasItem.Email
                End If

                If Not String.IsNullOrEmpty(pmOldEmail) AndAlso Not String.IsNullOrEmpty(abOldEmail) Then
                    Exit While
                End If

                result = mdAlias.GetNextAlias(aliasItem)
            End While

            ' cleanup postmaster alias
            If Not String.IsNullOrEmpty(pmOldEmail) Then
                service.ComObject.DeleteAlias(postmasterAlias, pmOldEmail)
            End If

            ' cleanup abuse alias
            If Not String.IsNullOrEmpty(abOldEmail) Then
                service.ComObject.DeleteAlias(abuseAlias, abOldEmail)
            End If

            If Not service.ComObject.CreateAlias(postmaster, postmasterAlias) Then
                Throw New Exception("Couldn't assign domain postmaster account.")
            End If

            If Not service.ComObject.CreateAlias(abuse, abuseAlias) Then
                Throw New Exception("Couldn't assign domain abuse account.")
            End If

            Dim domainDbPath As String = service.ComObject.GetDBPath(MDaemonInterop.MDUSERDLL_DOMAINDB)
            Dim retVal As Integer = GetProfileSection(domain.Name, domainDbPath)

            If retVal = 0 Then
                If Not WriteProfileSection(domain.Name, domainDbPath) Then
                    Throw New Exception(String.Format("Could not create profile section in '{0}' file.", domainDbPath))
                End If
            End If

            WriteDomainInfo(domain, domainDbPath)

            ' force to refresh cache
            RefreshMailServerCache(service)

            'reload domains
            service.ComObject.ReloadUsers()

            UnloadServiceProvider(service)
        Catch ex As Exception
            UnloadServiceProvider(service)
            Throw New Exception("Can't update domain", ex)
        End Try
    End Sub

    Public Sub UpdateGroup(ByVal group As MailGroup) Implements IMailServer.UpdateGroup
        Try
            If Not GroupExists(group.Name) Then
                Throw New Exception(String.Format("Group {0} does not exists.", group.Name))
            End If
            PopulateGroupInfo(group, False)
        Catch ex As Exception
            Throw New Exception("Can't update group.", ex)
        End Try
    End Sub

    Public Sub UpdateList(ByVal maillist As MailList) Implements IMailServer.UpdateList
        Try
            If Not GroupExists(maillist.Name) Then
                Throw New Exception(String.Format("Mail list {0} does not exists.", maillist.Name))
            End If
            PopulateMailListInfo(maillist, False)
        Catch ex As Exception
            Throw New Exception("Can't update mail list.", ex)
        End Try
    End Sub

#Region "HostingServiceProvider methods"
    Public Overrides Sub ChangeServiceItemsState(ByVal items() As ServiceProviderItem, ByVal enabled As Boolean)
        For Each item As ServiceProviderItem In items
            If TypeOf item Is MailDomain Then
                Try
                    ' get mail domain accounts
                    Dim accounts As MailAccount() = Me.GetAccounts(item.Name)
                    ' disable each mail account
                    For Each account As MailAccount In accounts
                        account.Enabled = enabled
                        ' change service item state
                        UpdateAccount(account)
                    Next
                Catch ex As Exception
                    Log.WriteError(String.Format("Error switching '{0}' mail domain", item.Name), ex)
                End Try
            End If
        Next
    End Sub
#End Region

    Public Function GetAppFolderPath() As String
        Dim uninstalString As String = ""
        Dim returnPath As String = ""
        Dim key32bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\MDaemon Server")
        If (key32bit IsNot Nothing) Then
            uninstalString = CStr(key32bit.GetValue("UninstallString"))
        Else
            Dim key64bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\MDaemon Server")
            If (key64bit IsNot Nothing) Then
                uninstalString = CStr(key32bit.GetValue("UninstallString"))
            Else
                Return "C:\MDaemon\App\"
            End If
        End If
        If [String].IsNullOrEmpty(uninstalString) = False Then
            Dim split As String() = uninstalString.Split(New [Char]() {" "c})
            returnPath = split(0).Remove(split(0).LastIndexOf("\") + 1)
        End If

        If [String].IsNullOrEmpty(uninstalString) = False Then
            Return returnPath
        End If
        Return "C:\MDaemon\App\"
    End Function

    Public Overrides Function IsInstalled() As Boolean
        Dim version As String = ""
        Dim key32bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\MDaemon Server")
        If (key32bit IsNot Nothing) Then
            version = CStr(key32bit.GetValue("DisplayVersion"))
        Else
            Dim key64bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\MDaemon Server")
            If (key64bit IsNot Nothing) Then
                version = CStr(key64bit.GetValue("DisplayVersion"))
            Else
                Return False
            End If
        End If
        If [String].IsNullOrEmpty(version) = False Then
            Dim split As String() = version.Split(New [Char]() {"."c})
            Return split(0).Equals("9") Or split(0).Equals("10") Or split(0).Equals("11")
        Else
            Return False
        End If
    End Function

End Class
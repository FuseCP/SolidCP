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

Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports System.Diagnostics
Imports Microsoft.Win32
Imports SolidCP.Providers.Utils
Imports SolidCP.Server.Utils

Public Class Merak
    Inherits HostingServiceProviderBase
    Implements IMailServer

    Const API_PROGID As String = "IceWarpServer.APIObject"
    Const DOMAIN_PROGID As String = "IceWarpServer.DomainObject"
    Const ACCOUNT_PROGID As String = "IceWarpServer.AccountObject"

    Friend ReadOnly Property AccountType As MailType
        Get
            Dim mType As MailType = MailType.POP

            Try
                mType = CType(ProviderSettings("ServerAccountType"), MailType)
            Catch ex As Exception

            End Try

            Return mType
        End Get
    End Property

    Private Function GetDomainName(ByVal email As String) As String
        Return email.Substring((email.IndexOf("@"c) + 1))
    End Function 'GetDomainName


    Private Function GetEmailAlias(ByVal email As String) As String
        If String.IsNullOrEmpty(email) Then
            Return email
        ElseIf email.IndexOf("@"c) = -1 Then
            Return email
        End If

        Return email.Substring(0, email.IndexOf("@"c))
    End Function 'GetEmailAlias


    Private ReadOnly Property ConfigPath() As String
        Get
            Dim apiObject = CreateObject(API_PROGID)
            Return CStr(apiObject.GetProperty(MerakInterop.C_ConfigPath))
        End Get
    End Property

    Private ReadOnly Property MailPath() As String
        Get
            Dim apiObject = CreateObject(API_PROGID)

            Try
                Return CStr(apiObject.GetProperty(MerakInterop.C_System_Storage_Dir_MailPath))
            Catch ex1 As Exception
                'Log.WriteError(ex1)
                Try
                    Return CStr(apiObject.GetProperty(MerakInterop.C_MailPath))
                Catch ex2 As Exception
                    'Log.WriteError(ex2)
                    Try
                        Return CStr(apiObject.GetProperty(MerakInterop.C_XMailPath))
                    Catch ex3 As Exception
                        Log.WriteError(ex3)
                        Throw New Exception("Couldn't obtain mail folder path (version compatibility issue).", ex3)
                    End Try
                End Try
            End Try
        End Get
    End Property


    Private ReadOnly Property MailListTypeIdentifier() As String
        Get
            Dim one As Char = Chr(1) 'ToDo: Signed Bytes not supported
            Return New String(New Char() {one, ControlChars.NullChar, ControlChars.NullChar, ControlChars.NullChar})
        End Get
    End Property


    Private Const AutoRespondFolderName As String = "autoresp"
    Private Const SubjectKeywordStart As String = "$$SetSubject "
    Private Const SubjectKeywordEnd As String = "$$"
    Private Const GroupIdentifier As String = "CBDAAFA13E3C440C994A28F51CC834D2"


#Region "Domains"
    Private Sub CheckIfDomainExists(ByVal domainName As String)

        If Utils.IsStringNullOrEmpty(domainName, True) Or Not DomainExists(domainName) Then
            Throw New ArgumentException("Specified domain does not exist!")
        End If
    End Sub 'CheckIfDomainExists


    Private Function OpenDomain(ByVal domainName As String) As Object

        Dim apiObject = CreateObject(API_PROGID)

        Return apiObject.OpenDomain(domainName)
    End Function 'OpenDomain


    Private Sub SaveDomain(ByVal domainObject As Object)
        If Not domainObject.Save() Then
            Throw New ApplicationException("Failed to save new domain!")
        End If
    End Sub 'SaveDomain

    Public Overridable Function GetNumberOfDomains() As Integer
        Dim apiObject = CreateObject(API_PROGID)
        Return apiObject.GetDomainCount()
    End Function 'GetNumberOfDomains


    Public Overridable Function DomainExists(ByVal domainName As String) As Boolean Implements IMailServer.DomainExists
        Return Not (OpenDomain(domainName) Is Nothing)
    End Function 'DomainExists

    Public Overridable Function GetDomains() As String() Implements IMailServer.GetDomains
        Dim apiObject = CreateObject(API_PROGID)

        Dim domainList As String = apiObject.GetDomainList()

        Dim domains As New List(Of String)

        Dim indexOf As Integer = domainList.IndexOf(";")

        While (indexOf > -1)
            Dim domain As String = domainList.Substring(0, indexOf)

            If Not String.IsNullOrEmpty(domain) Then
                domains.Add(domain.Trim())
            End If

            domainList = domainList.Substring(indexOf + 1)

            indexOf = domainList.IndexOf(";")
        End While

        Return domains.ToArray()
    End Function

    Public Overridable Function GetDomain(ByVal domainName As String) As MailDomain Implements IMailServer.GetDomain
        CheckIfDomainExists(domainName)

        Dim domainObjectClass = OpenDomain(domainName)

        Dim domainItem As New MailDomain

        domainItem.Name = domainObjectClass.Name
        domainItem.PostmasterAccount = GetEmailAlias(CStr(domainObjectClass.GetProperty("D_AdminEmail")))
        domainItem.CatchAllAccount = GetEmailAlias(CStr(domainObjectClass.GetProperty("D_UnknownForwardTo")))
        domainItem.AbuseAccount = ""

        domainItem.Enabled = CInt(domainObjectClass.GetProperty("D_DisableLogin")) = 0
        domainItem.MaxMailboxSizeInMB = CInt(domainObjectClass.GetProperty("D_UserMailbox")) / 1024

        'Dim abuseEmail As String = "abuse@" + domainName

        'If MailboxExists(abuseEmail) Then
        '    Dim mailbox As MailAccount = GetMailboxInternal(abuseEmail)
        '    domainItem.AbuseAccount = mailbox.ForwardingAddresses(0)
        'Else
        '    domainItem.AbuseAccount = String.Empty
        'End If
        'Dim sw As New StreamWriter("c:\merak.txt")
        'sw.WriteLine("D_AdminEmail: " & domainObjectClass.GetProperty(MerakInterop.D_AdminEmail))
        'sw.WriteLine("D_AccountNumber: " & domainObjectClass.GetProperty(MerakInterop.D_AccountNumber))
        'sw.WriteLine("D_Description: " & domainObjectClass.GetProperty(MerakInterop.D_Description))
        'sw.WriteLine("D_DomainValue: " & domainObjectClass.GetProperty(MerakInterop.D_DomainValue))
        'sw.WriteLine("D_InfoToAdmin: " & domainObjectClass.GetProperty(MerakInterop.D_InfoToAdmin))
        'sw.WriteLine("D_PostMaster: " & domainObjectClass.GetProperty(MerakInterop.D_PostMaster))
        'sw.WriteLine("D_Type: " & domainObjectClass.GetProperty(MerakInterop.D_Type))
        'sw.WriteLine("D_UnknownForwardTo: " & domainObjectClass.GetProperty(MerakInterop.D_UnknownForwardTo))
        'sw.WriteLine("D_UnknownUsersForward: " & domainObjectClass.GetProperty(MerakInterop.D_UnknownUsersForward))
        'sw.Close()


        Return domainItem
    End Function 'GetDomain

    Public Overridable Sub CreateDomain(ByVal domain As MailDomain) Implements IMailServer.CreateDomain
        If Utils.IsStringNullOrEmpty(domain.Name, True) Then
            Throw New ArgumentNullException("domain.Name")
        End If
        Dim domainObject = CreateObject(DOMAIN_PROGID)

        If Not domainObject.[New](domain.Name) Then
            Throw New ApplicationException("Failed to create domain!")
        End If

        SaveDomain(domainObject)

        UpdateDomain(domain)
    End Sub 'CreateDomain

    Public Overridable Sub UpdateDomain(ByVal domain As MailDomain) Implements IMailServer.UpdateDomain
        CheckIfDomainExists(domain.Name)

        'open domain to edit
        Dim domainObject = OpenDomain(domain.Name)

        domainObject.SetProperty("D_AdminEmail", _
            IIf(String.IsNullOrEmpty(domain.PostmasterAccount), "", domain.PostmasterAccount & "@" & domain.Name))


        If String.IsNullOrEmpty(domain.CatchAllAccount) Then
            domainObject.SetProperty("D_UnknownForwardTo", "")
            domainObject.SetProperty("D_UnknownUsersType", 0)
            'domainObject.SetProperty(MerakInterop.D_UnknownForwardTo, 0)
        Else
            domainObject.SetProperty("D_UnknownForwardTo", domain.CatchAllAccount & "@" & domain.Name)
            domainObject.SetProperty("D_UnknownUsersType", 1)
            'domainObject.SetProperty(MerakInterop.D_UnknownForwardTo, 1)
        End If

        ' enable user limits
        Dim apiObject = CreateObject(API_PROGID)
        apiObject.SetProperty(MerakInterop.C_Config_UseDomainLimits, 1)

        domainObject.SetProperty(MerakInterop.D_DisableLogin, IIf(domain.Enabled, 0, 1))
        domainObject.SetProperty("D_UserMailbox", domain.MaxMailboxSizeInMB * 1024)


        'create abuse account mailbox
        'If Not Utils.IsStringNullOrEmpty(domain.AbuseAccount, True) Then
        '    Dim abuseName As String = "abuse@" + domain.Name

        '    Dim mailbox As New MailAccount
        '    mailbox.Name = abuseName
        '    mailbox.Enabled = True

        '    If String.Compare(domain.AbuseAccount, abuseName, False) <> 0 Then
        '        'done redirect to itself
        '        mailbox.ForwardingAddresses = New String() {domain.AbuseAccount}
        '    End If
        '    If Not MailboxExists(abuseName) Then
        '        CreateMailbox(mailbox)
        '    Else
        '        UpdateMailbox(mailbox)
        '    End If
        'End If
        SaveDomain(domainObject)
    End Sub 'UpdateDomain

    Public Overridable Sub DeleteDomain(ByVal domainName As String) Implements IMailServer.DeleteDomain
        CheckIfDomainExists(domainName)
        Dim objDomain = OpenDomain(domainName)
        objDomain.Delete()
    End Sub 'DeleteDomain

#End Region

#Region "Domain Aliases"
    Public Overridable Function DomainAliasExists(ByVal domainName As String, ByVal aliasName As String) As Boolean _
        Implements IMailServer.DomainAliasExists
        If DomainExists(aliasName) Then
            Dim domainObject = OpenDomain(aliasName)
            If CInt(domainObject.GetProperty(MerakInterop.D_Type)) = 2 And String.Compare(CStr(domainObject.GetProperty(MerakInterop.D_DomainValue)), domainName, False) = 0 Then
                Return True
            End If
        End If
        Return False
    End Function 'DomainAliasExists

    Public Overridable Function GetDomainAliases(ByVal domainName As String) As String() Implements IMailServer.GetDomainAliases
        Dim domainNameCollection As New List(Of String)
        Dim numberOfDomains As Integer = GetNumberOfDomains()

        Dim apiObject = CreateObject(API_PROGID)

        Dim i As Integer
        For i = 0 To numberOfDomains - 1
            Dim aliasName As String = apiObject.GetDomain(i)
            If DomainAliasExists(domainName, aliasName) Then
                domainNameCollection.Add(aliasName)
            End If
        Next i
        Return domainNameCollection.ToArray()
    End Function 'GetDomainAliases

    Public Overridable Sub AddDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.AddDomainAlias
        Dim MailDomain As New MailDomain
        MailDomain.Name = aliasName
        CreateDomain(MailDomain)
        Dim domainObject = OpenDomain(aliasName)
        domainObject.SetProperty(MerakInterop.D_Type, 2)
        domainObject.SetProperty(MerakInterop.D_DomainValue, domainName)
        SaveDomain(domainObject)
    End Sub 'AddDomainAlias

    Public Overridable Sub DeleteDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.DeleteDomainAlias
        DeleteDomain(aliasName)
    End Sub 'DeleteDomainAlias
#End Region

#Region "Accounts"
    Public Overridable Function AccountExists(ByVal mailboxName As String) As Boolean Implements IMailServer.AccountExists
        Dim accountObject = CreateObject(ACCOUNT_PROGID)

        If Not accountObject.Open(mailboxName) Or MerakInterop.GetIntSettingValue(CStr(accountObject.GetProperty(MerakInterop.U_Type))) <> 0 Or String.Compare(CStr(accountObject.GetProperty("U_Comment")), GroupIdentifier, False) = 0 Then
            'maillist?
            'group?
            Return False
        End If

        Return True
    End Function 'MailboxExists

    Public Overridable Function GetAccounts(ByVal domainName As String) As MailAccount() Implements IMailServer.GetAccounts
        Dim domainObject = OpenDomain(domainName)
        Dim emailList As String = domainObject.GetAccountList()
        Dim emails As String() = emailList.Split(";"c)

        If emails.Length > 0 Then
            Dim mailboxes As New ArrayList

            If emails.Length > 1 Then
                Dim i As Integer
                For i = 0 To emails.Length - 1
                    If emails(i) = ControlChars.NullChar Then
                        GoTo ContinueFor1
                    End If
                    Dim mailboxName As String = emails(i) + "@" + domainName

                    If AccountExists(mailboxName) Then
                        Dim mailaccount = OpenMailbox(mailboxName)
                        If mailaccount.GetProperty(MerakInterop.U_UseRemoteAddress).Equals(0) Then
                            mailboxes.Add(GetAccount(mailboxName))
                        End If
                    End If
ContinueFor1:
                Next i
            End If
            Return CType(mailboxes.ToArray(GetType(MailAccount)), MailAccount())
        End If
        Return New MailAccount(0) {}
    End Function 'GetMailboxes


    Private Function GetAccountInternal(ByVal mailboxName As String) As MailAccount

        Dim accountObject = OpenMailbox(mailboxName)

        Dim MailAccount As New MailAccount
        MailAccount.Name = mailboxName

        'If checkDomain Then
        'Dim domainItem As MailDomain = GetDomain(accountObject.Domain)
        'MailAccount.Postmaster = String.Compare(mailboxName, domainItem.PostmasterAccount, False) = 0
        'MailAccount.CatchAll = String.Compare(mailboxName, domainItem.CatchAllAccount, False) = 0
        'MailAccount.Abuse = String.Compare(mailboxName, domainItem.AbuseAccount, False) = 0
        'End If

        MailAccount.Enabled = CInt(accountObject.GetProperty(MerakInterop.U_AccountDisabled)) = 0
        MailAccount.MaxMailboxSize = CInt(accountObject.GetProperty(MerakInterop.U_MaxBoxSize)) / 1024
        MailAccount.Password = CStr(accountObject.GetProperty(MerakInterop.U_Password))
        MailAccount.FirstName = CStr(accountObject.GetProperty("U_Name"))

        Dim redirect As String = CStr(accountObject.GetProperty("U_ForwardTo"))
        If Not (redirect Is Nothing) Then
            MailAccount.ForwardingAddresses = redirect.Split(";"c)
        End If

        MailAccount.DeleteOnForward = CInt(accountObject.GetProperty(MerakInterop.U_DeleteOlder)) = 1

        Dim respondStatus As Integer = CInt(accountObject.GetProperty(MerakInterop.U_Respond))
        MailAccount.ResponderEnabled = respondStatus > 0

        'if (MailAccount.ResponderEnabled)
        '	{
        Dim responderSubject, responderMessage As String
        responderSubject = ""
        responderMessage = ""
        ParseResponderMessage(accountObject, GetDomainName(MailAccount.Name), responderSubject, responderMessage)

        If Not (responderSubject Is Nothing) Then
            MailAccount.ResponderSubject = responderSubject
            MailAccount.ResponderMessage = responderMessage.Trim(ControlChars.Cr, ControlChars.Lf)
        End If '	}

        'MailAccount.Unlimited = CInt(accountObject.GetProperty(MerakInterop.U_MaxBox)) = 0

        Return MailAccount
    End Function 'GetMailboxInternal

    Public Overridable Function GetAccount(ByVal mailboxName As String) As MailAccount Implements IMailServer.GetAccount
        Return GetAccountInternal(mailboxName)
    End Function 'GetMailbox

    Public Overridable Sub CreateAccount(ByVal mailbox As MailAccount) Implements IMailServer.CreateAccount
        If Utils.IsStringNullOrEmpty(mailbox.Name, True) Then
            Throw New ArgumentNullException("mailbox.Name")
        End If

        If Not DomainExists(GetDomainName(mailbox.Name)) Then
            Throw New ArgumentException("Cannot create mailbox, because the domain part is invalid!", "mailbox.Name")
        End If

        Dim domainName As String = GetDomainName(mailbox.Name)
        Dim domainObject = OpenDomain(domainName)

        'AccountObjectClass accountObject = (AccountObjectClass) domainObject.NewAccount(mailbox.Name);
        Dim accountObject = CreateObject(ACCOUNT_PROGID)
        accountObject.[New](mailbox.Name)

        ' Account type
        accountObject.SetProperty(MerakInterop.U_AccountType, AccountType)

        SaveMailbox(accountObject)

        UpdateAccount(mailbox)

        SaveDomain(domainObject)

    End Sub 'CreateMailbox

    'fix for Alberto
    Public Overridable Sub UpdateAccountName(ByVal mailAccount As MailAccount)
        If Utils.IsStringNullOrEmpty(mailAccount.Name, True) Or Not AccountExists(mailAccount.Name) Then
            Throw New ArgumentException("The mailbox name is empty or mailbox does not exist")
        End If
        Dim accountObject = OpenMailbox(mailAccount.Name)
        If Utils.IsStringNullOrEmpty(accountObject.GetProperty("U_Name"), True) Then
            accountObject.SetProperty("U_Name", GetEmailAlias(mailAccount.Name))
        End If
        SaveMailbox(accountObject)

    End Sub

    Public Overridable Sub UpdateAllAccounts()

        Dim domainsList As Array
        Dim accountsList As Array
        domainsList = GetDomains()

        Dim domain As String
        Dim account As MailAccount
        Dim i As Integer = 0
        For Each domain In domainsList

            accountsList = GetAccounts(domain)
            If accountsList.Length <> 0 Then
                For Each account In accountsList
                    i = i + 1
                    UpdateAccountName(account)
                    Log.WriteInfo(account.Name + " Updated" + i.ToString)
                Next account
            End If
        Next domain



    End Sub

    Public Overridable Sub UpdateAccount(ByVal mailbox As MailAccount) Implements IMailServer.UpdateAccount

        If Utils.IsStringNullOrEmpty(mailbox.Name, True) Or Not AccountExists(mailbox.Name) Then
            Throw New ArgumentException("The mailbox name is empty or mailbox does not exist")
        End If

        Dim accountObject = OpenMailbox(mailbox.Name)
        accountObject.SetProperty(MerakInterop.U_AccountDisabled, IIf(mailbox.Enabled, 0, 1)) 'ToDo: Unsupported feature: conditional (?) operator.

        'mail box size
        accountObject.SetProperty(MerakInterop.U_MaxBox, IIf(mailbox.MaxMailboxSize = 0, 0, 1))
        accountObject.SetProperty(MerakInterop.U_MaxBoxSize, mailbox.MaxMailboxSize * 1024)

        'accountObject.SetProperty(MerakInterop.U_Password, mailbox.Password)
        accountObject.SetProperty("U_Password", mailbox.Password)
        accountObject.SetProperty("U_Name", mailbox.FirstName)
        accountObject.SetProperty(MerakInterop.U_Respond, IIf(mailbox.ResponderEnabled, 1, 0)) 'ToDo: Unsupported feature: conditional (?) operator.

        If Not (mailbox.ForwardingAddresses Is Nothing) And mailbox.ForwardingAddresses.Length > 0 Then
            accountObject.SetProperty("U_ForwardTo", Utils.ConcatStrings(mailbox.ForwardingAddresses, ";"c))
        Else
            accountObject.SetProperty("U_ForwardTo", "")
        End If

        ' delete on forward
        accountObject.SetProperty(MerakInterop.U_DeleteOlder, IIf(mailbox.DeleteOnForward, 1, 0))
        accountObject.SetProperty(MerakInterop.U_DeleteOlderDays, 0)

        If mailbox.ResponderEnabled Then
            UpdateResponderMessage(accountObject, mailbox.Name, mailbox.ResponderSubject, mailbox.ResponderMessage)
        End If
        SaveMailbox(accountObject)
    End Sub 'UpdateMailbox

    Public Overridable Sub DeleteAccount(ByVal mailboxName As String) Implements IMailServer.DeleteAccount
        If Utils.IsStringNullOrEmpty(mailboxName, True) Then
            Throw New ArgumentNullException("mailboxName")
        End If
        Dim accountObject = OpenMailbox(mailboxName)

        If Not accountObject.Delete() Then
            Throw New ApplicationException("Failed to delete account object!")
        End If
    End Sub

    Public Overridable Function MailAliasExists(ByVal mailAliasName As String) As Boolean Implements IMailServer.MailAliasExists

        Dim accountObject = CreateObject(ACCOUNT_PROGID)

        If Not accountObject.Open(mailAliasName) Or MerakInterop.GetIntSettingValue(CStr(accountObject.GetProperty(MerakInterop.U_Type))) <> 0 Or String.Compare(CStr(accountObject.GetProperty("U_Comment")), GroupIdentifier, False) = 0 Then
            If accountObject.GetProperty(MerakInterop.U_UseRemoteAddress) <> 1 Then
                Return False
            End If
        End If

        Return True

    End Function

    Public Function GetMailAliases(ByVal domainName As String) As MailAlias() Implements IMailServer.GetMailAliases
        Dim domainObject = OpenDomain(domainName)
        Dim emailList As String = domainObject.GetAccountList()
        Dim emails As String() = emailList.Split(";"c)

        If emails.Length > 0 Then
            Dim mailAliases As New ArrayList

            If emails.Length > 1 Then
                Dim i As Integer
                For i = 0 To emails.Length - 1
                    If emails(i) = ControlChars.NullChar Then
                        GoTo ContinueFor1
                    End If
                    Dim mailboxName As String = emails(i) + "@" + domainName

                    If AccountExists(mailboxName) Then
                        Dim mailAlias = OpenMailbox(mailboxName)
                        If mailAlias.GetProperty(MerakInterop.U_UseRemoteAddress).Equals(1) Then
                            mailAliases.Add(GetMailAlias(mailboxName))
                        End If
                    End If
ContinueFor1:
                Next i
            End If
            Return CType(mailAliases.ToArray(GetType(MailAlias)), MailAlias())
        End If
        Return New MailAlias(0) {}
    End Function

    Public Function GetMailAlias(ByVal mailAliasName As String) As MailAlias Implements IMailServer.GetMailAlias
        Return GetMailAliasInternal(mailAliasName)
    End Function


    Private Function GetMailAliasInternal(ByVal mailAliasName As String) As MailAlias
        Dim accountObject = OpenMailbox(mailAliasName)
        If accountObject.GetProperty("U_UseRemoteAddress").Equals(0) Then
            accountObject.SetProperty("U_UseRemoteAddress", 1)
        End If
        Dim mailAlias As New MailAlias
        mailAlias.Name = mailAliasName
        'get mail Alias Remote Address
        mailAlias.ForwardTo = accountObject.GetProperty("U_ForwardTo")
        SaveMailbox(accountObject)
        Return mailAlias
        'End If
    End Function 'GetMailAliasInternal

    Public Sub CreateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.CreateMailAlias

        If Utils.IsStringNullOrEmpty(mailAlias.Name, True) Then
            Throw New ArgumentNullException(mailAlias.Name)
        End If

        If Not DomainExists(GetDomainName(mailAlias.Name)) Then
            Throw New ArgumentException("Cannot create mailAlias, because the domain part is invalid!", mailAlias.Name)
        End If

        Dim domainName As String = GetDomainName(mailAlias.Name)
        Dim domainObject = OpenDomain(domainName)

        Dim accountObject = CreateObject(ACCOUNT_PROGID)
        accountObject.[New](mailAlias.Name)
        ' Account type
        accountObject.SetProperty(MerakInterop.U_AccountType, AccountType)

        SaveMailbox(accountObject)

        UpdateMailAlias(mailAlias)

        SaveDomain(domainObject)

    End Sub

    Public Sub UpdateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.UpdateMailAlias

        If Utils.IsStringNullOrEmpty(mailAlias.Name, True) Or Not AccountExists(mailAlias.Name) Then
            Throw New ArgumentException("The mailbox name is empty or mailbox does not exist")
        End If

        Dim accountObject = OpenMailbox(mailAlias.Name)
        'set MailBox as a Remote Mailbox
        accountObject.SetProperty("U_UseRemoteAddress", 1)
        'set Remote Address
        accountObject.SetProperty("U_ForwardTo", mailAlias.ForwardTo)
        SaveMailbox(accountObject)

    End Sub

    Public Sub DeleteMailAlias(ByVal mailAliasName As String) Implements IMailServer.DeleteMailAlias
        DeleteAccount(mailAliasName)
    End Sub

    'DeleteMailbox

    Private Function OpenMailbox(ByVal mailboxName As String) As Object

        Dim accountObject = CreateObject(ACCOUNT_PROGID)

        If Not accountObject.Open(mailboxName) Then
            Throw New ApplicationException(String.Format("Failed to open account {0}", mailboxName))
        End If
        Return accountObject
    End Function 'OpenMailbox

    Private Function OpenMailbox_1(ByVal mailboxName As String) As Service
        Dim objService As New Service()

        Try
            objService.ComObject = CreateObject(ACCOUNT_PROGID)

            If objService.ComObject.Open(mailboxName) Then
                objService.Succeed = True
            End If
        Catch ex As Exception
            objService.Succeed = False
        End Try

        Return objService
    End Function


    Private Sub SaveMailbox(ByVal accountObject As Object)
        If Not accountObject.Save() Then
            Throw New ApplicationException(String.Format("Failed to save account"))
        End If
    End Sub 'SaveMailbox


    Private Sub ParseResponderMessage(ByVal accountObject As Object, ByVal domainName As String, ByRef subject As String, ByRef message As String)
        Dim fileName As String = CStr(accountObject.GetProperty(MerakInterop.U_RespondWith))
        Dim filePath As String = String.Format("{0}{1}\{2}\{3}", ConfigPath, domainName, AutoRespondFolderName, fileName)

        subject = String.Empty
        message = String.Empty

        If File.Exists(filePath) Then
            Dim fileContent As String = String.Empty
            Dim streamReader As New StreamReader(filePath)
            Try
                fileContent = streamReader.ReadToEnd()
            Finally
                streamReader.Close()
            End Try

            If Not Utils.IsStringEmpty(fileContent, True) Then
                Dim indexSubjectStart As Integer = fileContent.IndexOf(SubjectKeywordStart)
                If indexSubjectStart > -1 Then
                    Dim indexSubjectEnd As Integer = fileContent.IndexOf(SubjectKeywordEnd, indexSubjectStart + 2)

                    If indexSubjectEnd > -1 Then
                        Dim subjectStartLength As Integer = SubjectKeywordStart.Length
                        Dim subjectLength As Integer = indexSubjectEnd - indexSubjectStart - subjectStartLength
                        subject = fileContent.Substring(indexSubjectStart + subjectStartLength, subjectLength)
                        fileContent = fileContent.Remove(indexSubjectStart, indexSubjectEnd - indexSubjectStart + SubjectKeywordEnd.Length)
                    End If
                End If
                message = fileContent
            End If
        End If
    End Sub 'ParseResponderMessage


    Private Sub UpdateResponderMessage(ByVal accountObject As Object, ByVal mailboxName As String, ByVal subject As String, ByVal message As String)
        Dim fileName As String = CStr(accountObject.GetProperty(MerakInterop.U_RespondWith))
        Dim [alias] As String = GetEmailAlias(mailboxName)
        Dim domainName As String = GetDomainName(mailboxName)

        If Utils.IsStringNullOrEmpty(fileName, True) Then

            fileName = [alias] + ".txt"
            accountObject.SetProperty(MerakInterop.U_RespondWith, fileName)
        End If

        Dim dirPath As String = String.Format("{0}{1}\{2}", ConfigPath, domainName, AutoRespondFolderName)

        If Not Directory.Exists(dirPath) Then
            Directory.CreateDirectory(dirPath)
        End If

        Dim filePath As String = Path.Combine(dirPath, fileName)


        Dim stream As FileStream = File.Open(filePath, FileMode.OpenOrCreate)
        Try
            Dim writer As New StreamWriter(stream)
            Try
                If Not Utils.IsStringNullOrEmpty(subject, True) Then
                    writer.WriteLine(String.Concat(SubjectKeywordStart, subject, SubjectKeywordEnd))
                End If
                writer.WriteLine(message)
                writer.Flush()
                writer.Close()
            Finally
                writer.Close()
            End Try
        Finally
            stream.Close()
        End Try
        SaveMailbox(accountObject)
    End Sub 'UpdateResponderMessage
#End Region

#Region "Groups"
    Public Overridable Function GroupExists(ByVal groupName As String) As Boolean Implements IMailServer.GroupExists
        Dim accountObject = CreateObject(ACCOUNT_PROGID)

        If Not accountObject.Open(groupName) Then
            Return False
        Else
            If (accountObject.GetProperty("U_Type").Equals(7)) Then
                Return True
            End If
        End If
        Return False
    End Function 'GroupExists

    Public Overridable Function GetGroups(ByVal domainName As String) As MailGroup() Implements IMailServer.GetGroups
        Dim domainObject = OpenDomain(domainName)
        Dim emailList As String = domainObject.GetAccountList()
        Dim emails As String() = emailList.Split(";"c)

        If emails.Length > 0 Then
            Dim mailgroupes As New ArrayList

            If emails.Length > 1 Then
                Dim i As Integer
                For i = 0 To emails.Length - 1
                    If emails(i) = ControlChars.NullChar Then
                        GoTo ContinueFor1
                    End If
                    Dim mailgroupName As String = emails(i) + "@" + domainName
                    Dim accountObject = OpenMailbox(mailgroupName)
                    If GroupExists(mailgroupName) And accountObject.GetProperty("U_Type").Equals(7) Then
                        mailgroupes.Add(GetGroup(mailgroupName))
                    End If
ContinueFor1:
                Next i
            End If
            Return CType(mailgroupes.ToArray(GetType(MailGroup)), MailGroup())
        End If
        Return New MailGroup(0) {}
    End Function 'GetGroups

    Public Overridable Function GetGroup(ByVal groupName As String) As MailGroup Implements IMailServer.GetGroup
        If GroupExists(groupName) Then
            Dim accountObject = OpenMailbox(groupName)

            Dim group As New MailGroup
            group.Name = groupName
            group.Enabled = CInt(accountObject.GetProperty(MerakInterop.U_AccountDisabled)) = 0
            group.Members = GetMaillistMembers(accountObject, group.Name, GetDomainName(group.Name))
            Return group
        End If

        Throw New ArgumentException(String.Format("The group with the specified name:{0} does not exist!", groupName))
    End Function 'GetGroup

    Public Overridable Sub CreateGroup(ByVal group As MailGroup) Implements IMailServer.CreateGroup

        If Utils.IsStringNullOrEmpty(group.Name, True) Then
            Throw New ArgumentNullException(group.Name)
        End If

        If Not DomainExists(GetDomainName(group.Name)) Then
            Throw New ArgumentException("Cannot create Group, because the domain part is invalid!", group.Name)
        End If

        Dim domainName As String = GetDomainName(group.Name)
        Dim domainObject = OpenDomain(domainName)

        Dim accountObject = CreateObject(ACCOUNT_PROGID)
        accountObject.[New](group.Name)
        accountObject.SetProperty("U_Type", 7)
        accountObject.Save()
        SaveMailbox(accountObject)
        SaveDomain(domainObject)

        UpdateGroup(group)

    End Sub 'CreateGroup


    Public Overridable Sub UpdateGroup(ByVal group As MailGroup) Implements IMailServer.UpdateGroup
        If GroupExists(group.Name) Then
            Dim accountObject = OpenMailbox(group.Name)
            accountObject.SetProperty("G_Description", GetEmailAlias(group.Name))
            accountObject.SetProperty("G_GroupwareShared", 1)
            'implement members here.
            UpdateMailGroupMembers(group.Name, GetDomainName(group.Name), group.Members)
            If Not (group.Members Is Nothing) And group.Members.Length > 0 Then
                UpdateMailGroupMembers(group.Name, GetDomainName(group.Name), group.Members)
            End If
            accountObject.Save()
        Else
            Throw New ArgumentException(String.Format("Group {0} does not exist", group.Name))
        End If
    End Sub 'UpdateGroup

    Public Overridable Sub DeleteGroup(ByVal groupName As String) Implements IMailServer.DeleteGroup
        If GroupExists(groupName) Then
            Dim accountObject = OpenMailbox(groupName)

            If Not accountObject.Delete() Then
                Throw New ApplicationException("Failed to delete group object!")
            End If
        Else
            Throw New ArgumentException(String.Format("Group {0} does not exist", groupName))
        End If
    End Sub 'DeleteGroup
#End Region

#Region "Lists"
    '/ <summary>
    '/ Determines if Merak maillist exists
    '/ </summary>
    '/ <param name="maillistName">Maillist name</param>
    '/ <returns>True if maillist exists, false otherwise</returns>
    Public Overridable Function ListExists(ByVal maillistName As String) As Boolean Implements IMailServer.ListExists
        Dim accountObject = CreateObject(ACCOUNT_PROGID)

        If Not accountObject.Open(maillistName) Or MerakInterop.GetIntSettingValue(CStr(accountObject.GetProperty(MerakInterop.U_Type))) = 0 Or String.Compare(CStr(accountObject.GetProperty("U_Comment")), GroupIdentifier, False) = 0 Then
            'mailbox?
            'group?
            Return False
        End If

        Return True
    End Function 'MaillistExists

    Public Overridable Function GetLists(ByVal domainName As String) As MailList() Implements IMailServer.GetLists
        Dim domainObject = OpenDomain(domainName)
        Dim emailList As String = domainObject.GetAccountList()
        Dim emails As String() = emailList.Split(";"c)

        If emails.Length > 0 Then
            Dim maillistes As New ArrayList

            If emails.Length > 1 Then
                Dim i As Integer
                For i = 0 To emails.Length - 1
                    If emails(i) = ControlChars.NullChar Then
                        GoTo ContinueFor1
                    End If
                    Dim maillistName As String = emails(i) + "@" + domainName
                    Dim accountObject = OpenMailbox(maillistName)
                    If ListExists(maillistName) And accountObject.GetProperty("U_Type").Equals(1) Then
                        maillistes.Add(GetList(maillistName))
                    End If
ContinueFor1:
                Next i
            End If
            Return CType(maillistes.ToArray(GetType(MailList)), MailList())
        End If
        Return New MailList(0) {}
    End Function 'GetMaillists

    Public Overridable Function GetList(ByVal maillistName As String) As MailList Implements IMailServer.GetList
        Dim accountObject = OpenMailbox(maillistName)

        Dim item As New MailList
        item.Name = maillistName
        item.Description = CStr(accountObject.GetProperty("U_Name"))
        'item.Enabled 
        item.Moderated = Not CInt(accountObject.GetProperty(MerakInterop.M_Moderated)) = 0
        'item.ModeratorAddress = CStr(accountObject.GetProperty(MerakInterop.M_OwnerAddress))
        'item.Password = CStr(accountObject.GetProperty(MerakInterop.M_ModeratedPassword))
        item.MaxMessageSizeEnabled = Not CInt(accountObject.GetProperty("M_MaxList")) = 0
        item.MaxMessageSize = CInt(accountObject.GetProperty("M_MaxListSize"))
        item.MaxRecipientsPerMessage = CInt(accountObject.GetProperty("M_MaxMembers"))
        item.ModeratorAddress = CStr(accountObject.GetProperty(MerakInterop.M_OwnerAddress))

        If CInt(accountObject.GetProperty("M_MembersOnly")) = 1 Then
            item.PostingMode = PostingMode.MembersCanPost
        Else
            item.PostingMode = PostingMode.AnyoneCanPost
        End If

        Dim mMode As Integer = CInt(accountObject.GetProperty("M_Moderated"))

        Select Case mMode
            Case 0 ' Not password Protected
                item.Password = CStr(accountObject.GetProperty(MerakInterop.M_ModeratedPassword))
                item.PasswordProtection = PasswordProtection.NoProtection
                item.Moderated = True
            Case 1 ' Client moderated
                item.Password = CStr(accountObject.GetProperty(MerakInterop.M_ModeratedPassword))
                item.PasswordProtection = PasswordProtection.ClientModerated
                item.Moderated = True
            Case 2 ' Server moderated
                item.Password = CStr(accountObject.GetProperty(MerakInterop.M_ModeratedPassword))
                item.PasswordProtection = PasswordProtection.ServerModerated
                item.Moderated = True
        End Select

        Dim setSender As Integer = CInt(accountObject.GetProperty(MerakInterop.M_SetSender))
        Dim setValue As Integer = CInt(accountObject.GetProperty(MerakInterop.M_SetValue))
        'Dim replyToMode As Integer = CInt(accountObject.GetProperty(MerakInterop.M_ReplyTo))

        'switching fields
        accountObject.SetProperty(MerakInterop.M_SwitchFields, 1)
        'Dim setFromTo As Integer = CInt(accountObject.GetProperty(MerakInterop.M_SetFromTo))
        accountObject.Save()

        'string replyToValue = (string) accountObject.GetProperty (MerakInterop.M_SetFromToValue);
        'If replyToMode = 0 And setFromTo = 0 Then
        '    item.ReplyToMode = ReplyTo.RepliesToList
        'Else
        '    If setFromTo = 1 Then
        '        item.ReplyToMode = ReplyTo.RepliesToModerator
        '    Else
        '        item.ReplyToMode = ReplyTo.RepliesToSender
        '    End If
        'End If

        If setSender = 2 Then
            item.ReplyToMode = ReplyTo.RepliesToSender
        Else
            If setValue = 2 Then
                item.ReplyToMode = ReplyTo.RepliesToModerator
            Else
                item.ReplyToMode = ReplyTo.RepliesToList
            End If
        End If

        item.Members = GetMaillistMembers(accountObject, maillistName, GetDomainName(maillistName))
        Return item
    End Function 'GetMaillist

    Public Overridable Sub CreateList(ByVal maillist As MailList) Implements IMailServer.CreateList
        If Utils.IsStringNullOrEmpty(maillist.Name, True) Then
            Throw New ArgumentNullException("maillist.Name")
        End If

        If Not DomainExists(GetDomainName(maillist.Name)) Then
            Throw New ArgumentException("Cannot create maillist, because the domain part is invalid!", "maillist.Name")
        End If

        Dim domainName As String = GetDomainName(maillist.Name)
        Dim domainObject = OpenDomain(domainName)

        'AccountObjectClass accountObject = (AccountObjectClass) domainObject.NewAccount(maillist.Name);
        Dim accountObject = CreateObject(ACCOUNT_PROGID)
        accountObject.[New](maillist.Name)
        accountObject.SetProperty("U_Type", 1)
        accountObject.Save()

        SaveMailbox(accountObject)
        SaveDomain(domainObject)

        UpdateMaillist(maillist)
    End Sub 'CreateMaillist

    Public Overridable Sub UpdateMaillist(ByVal maillist As MailList) Implements IMailServer.UpdateList

        If Utils.IsStringNullOrEmpty(maillist.Name, True) Or Not ListExists(maillist.Name) Then
            Throw New ArgumentException("The maillist name is empty or maillist does not exist")
        End If

        Dim accountObject = OpenMailbox(maillist.Name)

        accountObject.SetProperty("L_Name", maillist.Description)

        accountObject.SetProperty("M_MaxList", IIf(maillist.MaxMessageSizeEnabled, 1, 0))
        accountObject.SetProperty("M_MaxListSize", maillist.MaxMessageSize)
        accountObject.SetProperty("M_MaxMembers", maillist.MaxRecipientsPerMessage)
        accountObject.SetProperty(MerakInterop.M_Moderated, IIf(maillist.Moderated, 1, 0))
        accountObject.SetProperty(MerakInterop.M_OwnerAddress, maillist.ModeratorAddress)



        Select Case maillist.PasswordProtection
            Case PasswordProtection.NoProtection
                accountObject.SetProperty("M_Moderated", 0)
                accountObject.SetProperty("M_ModeratedPassword", maillist.Password)
            Case PasswordProtection.ClientModerated
                accountObject.SetProperty("M_Moderated", 1)
                accountObject.SetProperty("M_ModeratedPassword", maillist.Password)
            Case PasswordProtection.ServerModerated
                accountObject.SetProperty("M_Moderated", 2)
                accountObject.SetProperty("M_ModeratedPassword", maillist.Password)
        End Select

        'Select Case maillist.

        accountObject.SetProperty("M_MembersOnly", IIf(maillist.PostingMode = PostingMode.MembersCanPost, 1, 0))

        Select Case maillist.ReplyToMode
            Case ReplyTo.RepliesToList
                ' Earlier server versions
                accountObject.SetProperty(MerakInterop.M_ReplyTo, 0)
                ' Merak 8.3.8
                accountObject.SetProperty(MerakInterop.M_SetValue, 0)
                accountObject.SetProperty(MerakInterop.M_HeaderValue, String.Empty)
                accountObject.SetProperty(MerakInterop.M_SetSender, 0)

                'accountObject.SetProperty (MerakInterop.M_SetFromTo, 0);
            Case ReplyTo.RepliesToSender
                ' Earlier server versions
                accountObject.SetProperty(MerakInterop.M_ReplyTo, 1)
                ' Merak 8.3.8
                accountObject.SetProperty(MerakInterop.M_SetSender, 2)
                accountObject.SetProperty(MerakInterop.M_SetValue, 0)
                accountObject.SetProperty(MerakInterop.M_HeaderValue, String.Empty)

                'accountObject.SetProperty (MerakInterop.M_SetFromTo, 0);
            Case ReplyTo.RepliesToModerator
                ' Earlier server versions
                accountObject.SetProperty(MerakInterop.M_SwitchFields, 1)
                accountObject.SetProperty(MerakInterop.M_SetFromTo, 1)
                accountObject.SetProperty(MerakInterop.M_SetFromToValue, maillist.ModeratorAddress)
                ' Merak 8.3.8
                accountObject.SetProperty(MerakInterop.M_SetSender, 0)
                accountObject.SetProperty(MerakInterop.M_SetValue, 2)
                accountObject.SetProperty(MerakInterop.M_HeaderValue, String.Concat("|", maillist.ModeratorAddress))
        End Select

        ' mail list members
        UpdateMaillistMembers(maillist.Name, GetDomainName(maillist.Name), maillist.Members)
        accountObject.SetProperty(MerakInterop.M_MailingListFile, _
            GetDomainName(maillist.Name) & "\" & GetEmailAlias(maillist.Name) + ".txt")

        SaveMailbox(accountObject)
    End Sub 'UpdateMaillist

    Public Overridable Sub DeleteMaillist(ByVal maillistName As String) Implements IMailServer.DeleteList
        If Utils.IsStringNullOrEmpty(maillistName, True) Or Not ListExists(maillistName) Then
            Throw New ArgumentException("The mailbox name is empty or maillist does not exist")
        End If
        Dim accountObject = OpenMailbox(maillistName)

        If Not accountObject.Delete() Then
            Throw New ArgumentException("Failed to delete maillist")
        End If
    End Sub 'DeleteMaillist

    Private Function GetMaillistMembers(ByVal accountObject As Object, ByVal listName As String, ByVal domainName As String) As String() '
        'ToDo: Error processing original source shown below
        '
        '
        '--^--- Unexpected pre-processor directive
        Dim dirPath As String = Path.Combine(ConfigPath, domainName)
        Dim filePath As String = Path.Combine(dirPath, GetEmailAlias(listName) + ".txt")

        Dim maillistMembers As New StringCollection

        'maillistMembers.Add(CStr(accountObject.GetProperty(MerakInterop.M_MailingListFile)))

        If File.Exists(filePath) Then
            Dim fs As FileStream = File.Open(filePath, FileMode.Open)
            Try
                Dim sr As New StreamReader(fs)
                Try
                    Dim line As String = sr.ReadLine()
                    While Not (line Is Nothing)
                        If Not Utils.IsStringEmpty(line, True) Then
                            Dim stopIndex As Integer = line.IndexOfAny(New Char() {" "c, ";"c})

                            If stopIndex > -1 Then
                                maillistMembers.Add(line.Substring(0, stopIndex))
                            Else
                                maillistMembers.Add(line)
                            End If
                            line = sr.ReadLine()
                        End If
                    End While
                Finally
                    sr.Close()
                End Try
            Finally
                fs.Close()
            End Try

            Dim ret(maillistMembers.Count - 1) As String
            maillistMembers.CopyTo(ret, 0)
            Return ret
        End If
        Return New String(0) {}
    End Function 'GetMaillistMembers




    Private Sub UpdateMailGroupMembers(ByVal groupName As String, ByVal domainName As String, ByVal members() As String)
        Dim dirPath As String = Path.Combine(ConfigPath, domainName)
        Dim filePath As String = Path.Combine(dirPath, GetEmailAlias(groupName) + ".txt")

        If Not Directory.Exists(dirPath) Then
            Directory.CreateDirectory(dirPath)
        End If

        Dim fs As FileStream = File.Open(filePath, FileMode.OpenOrCreate)
        'clean all old members
        Try
            fs.SetLength(0)
        Catch ex As Exception
            Log.WriteError(String.Format("Error deleting old mail list memebers  for '{0}' mail list", groupName), ex)
        Finally
            fs.Close()
        End Try
        'reopen file for new members
        fs = File.Open(filePath, FileMode.OpenOrCreate)
        Try
            Dim sw As New StreamWriter(fs)
            Try
                Dim str As String
                For Each str In members
                    sw.WriteLine(str)
                Next str
                sw.Flush()
                sw.Close()
            Catch ex As Exception
                Log.WriteError(String.Format("Unable to update new mail group members  for '{0}' group", groupName), ex)
            Finally
                sw.Close()
            End Try
        Finally
            fs.Close()
        End Try
    End Sub 'UpdateMaillistMembers


    Private Sub UpdateMaillistMembers(ByVal listName As String, ByVal domainName As String, ByVal members() As String)
        Dim dirPath As String = Path.Combine(ConfigPath, domainName)
        Dim filePath As String = Path.Combine(dirPath, GetEmailAlias(listName) + ".txt")

        If Not Directory.Exists(dirPath) Then
            Directory.CreateDirectory(dirPath)
        End If

        Dim fs As FileStream = File.Open(filePath, FileMode.OpenOrCreate)
        'clean all old members
        Try
            fs.SetLength(0)
        Catch ex As Exception
            Log.WriteError(String.Format("Error deleting old mail list members  for '{0}' mail list", listName), ex)
        Finally
            fs.Close()
        End Try
        'reopen file for new members
        fs = File.Open(filePath, FileMode.OpenOrCreate)
        Try
            Dim sw As New StreamWriter(fs)
            Try
                Dim str As String
                For Each str In members
                    sw.WriteLine(str)
                Next str
                sw.Flush()
                sw.Close()
            Catch ex As Exception
                Log.WriteError(String.Format("Unable to update new mail list memebers  for '{0}' mail list", listName), ex)
            Finally
                sw.Close()
            End Try
        Finally
            fs.Close()
        End Try
    End Sub 'UpdateMaillistMembers
#End Region

#Region "HostingServiceProvider methods"

    Public Overrides Sub ChangeServiceItemsState(ByVal items() As ServiceProviderItem, ByVal enabled As Boolean)
        Dim item As ServiceProviderItem
        For Each item In items
            If TypeOf item Is MailDomain Then
                Try
                    Dim domain As MailDomain = GetDomain(item.Name)
                    domain.Enabled = enabled
                    UpdateDomain(domain)
                Catch ex As Exception
                    Log.WriteError(String.Format("Error switching '{0}' mail domain", item.Name), ex)
                End Try
            End If
        Next
    End Sub

    Public Overrides Sub DeleteServiceItems(ByVal items() As ServiceProviderItem)
        For Each item As ServiceProviderItem In items
            If TypeOf item Is MailDomain Then
                Try
                    DeleteDomain(item.Name)
                Catch ex As Exception
                    Log.WriteError(String.Format("Error deleting '{0}' mail domain", item.Name), ex)
                End Try
            End If
        Next
    End Sub

    Public Overrides Function GetServiceItemsBandwidth(items() As ServiceProviderItem, since As Date) As ServiceProviderItemBandwidth()
        Dim itemsBandwidth As List(Of ServiceProviderItemBandwidth) = New List(Of ServiceProviderItemBandwidth)
        Dim item As ServiceProviderItem

        For Each item In items
            If TypeOf item Is MailAccount Then
                Dim name As String = item.Name

                Dim bandwidth As New ServiceProviderItemBandwidth()

                bandwidth.ItemId = item.Id
                bandwidth.Days = GetDailyStatistics(since, name)

                itemsBandwidth.Add(bandwidth)

            End If
        Next item
        Return itemsBandwidth.ToArray()

    End Function

    Public Function GetDailyStatistics(since As Date, mailAccount As String) As DailyStatistics()
        Dim days As List(Of DailyStatistics) = New List(Of DailyStatistics)


        Dim currentDate As DateTime = since
        Dim now As DateTime = DateTime.Now


        While currentDate < now
            days.Add(GetSingleDayBandwidthStatistics(currentDate, mailAccount))

            ' advance day
            currentDate = currentDate.AddDays(1)
        End While


        Return days.ToArray()
    End Function


    Public Function GetSingleDayBandwidthStatistics(day As Date, mailAccount As String) As DailyStatistics
        Const ReceivedAmmountPositionInString As Int32 = 18
        Const SentOutAmmountPositionInString As Int32 = 22

        Dim apiObject = CreateObject(API_PROGID)
        Dim result As New DailyStatistics()
        Dim formatedDate As String
        formatedDate = Format(day, "yyyy/MM/dd")

        result.Day = day.Day
        result.Month = day.Month
        result.Year = day.Year
        result.BytesReceived = 0
        result.BytesSent = 0

        Try
            Dim apiResult As String = apiObject.GetUserStatistics(formatedDate, formatedDate, mailAccount)

            Dim apiResultSplit As String() = apiResult.Split(",")

            result.BytesReceived = apiResultSplit(ReceivedAmmountPositionInString)
            result.BytesSent = apiResultSplit(SentOutAmmountPositionInString)
        Catch ex As Exception
            Log.WriteError(String.Format("Merak: Error calculating '{0}' bandwidth at {1}", mailAccount, day.ToShortDateString()), ex)
        End Try

        Return result
    End Function

    Public Overrides Function GetServiceItemsDiskSpace(ByVal items() As ServiceProviderItem) As ServiceProviderItemDiskSpace()
        Dim itemsDiskspace As List(Of ServiceProviderItemDiskSpace) = New List(Of ServiceProviderItemDiskSpace)
        Dim item As ServiceProviderItem
        For Each item In items
            If TypeOf item Is MailAccount Then

                ' get mailbox size
                Dim name As String = item.Name
                Dim objAccount As Service = OpenMailbox_1(name)

                If Not objAccount.Succeed Then
                    Continue For
                End If

                Dim mailboxPath As String = CStr(objAccount.ComObject.GetProperty(MerakInterop.U_MailBoxPath))
                mailboxPath = Path.Combine(MailPath, mailboxPath)

                Log.WriteStart([String].Format("Calculating '{0}' folder size", mailboxPath))

                ' calculate disk space
                Try
                    Dim diskspace As New ServiceProviderItemDiskSpace()
                    diskspace.ItemId = item.Id
                    diskspace.DiskSpace = FileUtils.CalculateFolderSize(mailboxPath)
                    itemsDiskspace.Add(diskspace)

                    Log.WriteEnd([String].Format("Calculating '{0}' folder size", mailboxPath))
                Catch ex As Exception
                    Log.WriteError(String.Format("Merak: Error calculating '{0}' folder diskspace", mailboxPath), ex)
                End Try
            End If
        Next item
        Return itemsDiskspace.ToArray()
    End Function

    Private Function GetMailboxPath(ByVal mailboxName As String) As String
        Dim dirPath As String = Path.Combine(MailPath, GetDomainName(mailboxName))
        Return Path.Combine(dirPath, GetEmailAlias(mailboxName))
    End Function

    Public Overrides Function IsInstalled() As Boolean
        Dim version As String = ""
        Dim key32bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\IceWarp\Merak Mail Server")
        If (key32bit IsNot Nothing) Then
            version = CStr(key32bit.GetValue("Version"))
        Else
            Dim key64bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\IceWarp\Merak Mail Server")
            If (key64bit IsNot Nothing) Then
                version = CStr(key64bit.GetValue("Version"))
            Else
                Return False
            End If
        End If
        If [String].IsNullOrEmpty(version) = False Then
            Dim split As String() = version.Split(New [Char]() {"."c})
            Return split(0).Equals("8") Or split(0).Equals("9")
        Else
            Return False
        End If
    End Function

#End Region

    '/ Provides constant values for easier interoperability with Merak COM library
    '/ </summary>
    Private Class MerakInterop

        Public Shared Function GetIntSettingValue(ByVal val As String) As Integer
            Return Convert.ToInt32(val.Chars(0))
        End Function 'GetIntSettingValue

        Public Const D_Type As String = "D_Type"
        Public Const D_DomainValue As String = "D_DomainValue"

        Public Const D_Description As Integer = &H0
        Public Const D_PostMaster As Integer = &H4
        Public Const D_AdminEmail As Integer = &H5
        Public Const D_UnknownUsersType As Integer = &H6
        Public Const D_UnknownForwardTo As Integer = &H7
        Public Const D_InfoToAdmin As Integer = &H8
        Public Const D_AccountNumber As Integer = &H9
        Public Const D_DisableLogin As Integer = &H10

        ' Account 
        Public Const U_Type As Integer = &H0
        Public Const U_AntiSpamIndex As Integer = &H1
        Public Const U_Name As Integer = &H2
        Public Const U_Alias As Integer = &H3
        Public Const U_Comment As Integer = &H5


        ' Merak 8.3.8
        Public Const U_AccountType As String = "U_AccountType" ' (atPOP3, atIMAPPOP3, atIMAP)
        Public Const U_MailBoxPath As String = "U_MailBoxPath"
        Public Const M_SetValue As String = "M_SetValue"
        Public Const M_HeaderValue As String = "M_HeaderValue"
        Public Const M_SetSender As String = "M_SetSender"

        Public Const U_Mailbox As Integer = &H10
        Public Const U_AccountDisabled As Integer = &H11
        Public Const U_AccountValid As Integer = &H12
        Public Const U_AccountValidTill As Integer = &H13
        Public Const U_AllowRemote As Integer = &H15
        Public Const U_ValidityReport As Integer = &H16
        Public Const U_ValidityReportDays As Integer = &H17
        Public Const U_NTPassword As Integer = &H18
        Public Const U_IMAP As Integer = &H19
        Public Const U_IMAPMailbox As Integer = &H1A
        Public Const U_MaxMessageSize As Integer = &H1B
        Public Const U_DontShowMessages As Integer = &H1C
        Public Const U_AnyPassword As Integer = &H1D
        Public Const U_ETRN As Integer = &H1E
        Public Const U_DeleteExpire As Integer = &H1F
        Public Const U_NULL As Integer = &H20
        Public Const U_Password As Integer = &H21
        Public Const U_NTPasswordValue As Integer = &H22
        Public Const U_DomainAdminIndex As Integer = &H23
        Public Const U_DomainAdmin As Integer = &H24
        'Public Const U_MailBoxPath As Integer = &H25
        Public Const U_Admin As Integer = &H26
        Public Const U_MaxBox As Integer = &H27
        Public Const U_MaxBoxSize As Integer = &H28
        Public Const U_ForceFrom As Integer = &H29

        '/ <summary>
        '/ (rNone, rAll, rOnce, rPeriod)
        '/ </summary>
        Public Const U_Respond As Integer = &H2A
        Public Const U_OnlyLocalDomain As Integer = &H2B
        Public Const U_UseRemoteAddress As Integer = &H2C
        Public Const U_ForwardTo As Integer = &H2D
        Public Const U_RespondWith As Integer = &H2E
        Public Const U_MailIn As Integer = &H2F
        Public Const U_MailOut As Integer = &H30
        Public Const U_ValidReport As Integer = &H31
        Public Const U_DeleteOlder As Integer = &H32
        Public Const U_DeleteOlderDays As Integer = &H33
        Public Const U_ForwardOlder As Integer = &H34
        Public Const U_ForwardOlderDays As Integer = &H35
        Public Const U_ForwardOlderTo As Integer = &H36
        Public Const U_RemoteAddress As Integer = &H37
        Public Const U_ForceFromAddress As Integer = &H38
        Public Const U_MegabyteSendLimit As Integer = &H39
        Public Const U_NumberSendLimit As Integer = &H3A
        Public Const U_NoMailList As Integer = &H3B
        Public Const U_AlternateEmail As Integer = &H4D
        Public Const U_E_Application As Integer = &H50
        Public Const U_E_Parameters As Integer = &H51
        Public Const U_E_ExecForwardCopy As Integer = &H52

        Public Const M_OwnerAddress As Integer = &H70
        Public Const M_CopyToOwner As Integer = &H71
        Public Const M_DigestConfirmed As Integer = &H72
        Public Const M_MailingListFile As Integer = &H73
        Public Const M_ListServer As Integer = &H74
        Public Const M_SendToSender As Integer = &H75
        Public Const M_SubListFile As Integer = &H76
        Public Const M_SendAllLists As Integer = &H77
        Public Const M_HeaderFile As Integer = &H78
        Public Const M_FooterFile As Integer = &H79
        Public Const M_Moderated As Integer = &H7A
        Public Const M_ModeratedPassword As Integer = &H7B
        Public Const M_DenyEXPNList As Integer = &H7C
        Public Const M_MaxList As Integer = &H7D
        Public Const M_MaxListSize As Integer = &H7E
        Public Const M_SetFromTo As Integer = &H7F
        Public Const M_SetFromToValue As Integer = &H80
        Public Const M_HelpFile As Integer = &H81
        Public Const M_MembersOnly As Integer = &H82
        Public Const M_ReplyTo As Integer = &H83
        Public Const M_SwitchFields As Integer = &H84
        Public Const M_AddToSubject As Integer = &H85
        Public Const M_JoinR As Integer = &H86
        Public Const M_LeaveR As Integer = &H87
        Public Const M_ListsR As Integer = &H88
        Public Const M_WhichR As Integer = &H89
        Public Const M_ReviewR As Integer = &H8A
        Public Const M_ListSubject As Integer = &H8B
        Public Const M_ListSender As Integer = &H8C
        Public Const M_SeparateTo As Integer = &H8D
        Public Const M_ServerModerated As Integer = &H8E

        Public Const S_MailAddress As Integer = &HA0
        Public Const S_SMSIntoSubject As Integer = &HA1
        Public Const S_Size As Integer = &HA2
        Public Const S_SendTo As Integer = &HA3
        Public Const S_SendFrom As Integer = &HA4
        Public Const S_SendSubject As Integer = &HA5
        Public Const S_SendBody As Integer = &HA6
        Public Const S_SendDateTime As Integer = &HA7
        Public Const S_Send As Integer = &HA8
        Public Const S_SMSForwardCopy As Integer = &HA9
        Public Const S_SkipAttach As Integer = &HAA
        Public Const S_SMSSender As Integer = &HAB
        Public Const S_SMSCount As Integer = &HAC
        Public Const S_SMSFilterFile As Integer = &HAD

        Public Const R_Activity As Integer = &HC0
        Public Const R_ActivityValue As Integer = &HC1
        Public Const R_FilterFile As Integer = &HC2
        Public Const R_ExternalFilter As Integer = &HC3
        Public Const R_ExternalFilterFile As Integer = &HC4
        Public Const R_ExternalDomain As Integer = &HC5
        Public Const R_SaveTo As Integer = &HC6
        Public Const R_ExternalFilterType As Integer = &HC7

        ' Config
        Public Const C_XHostName As Integer = &H0
        Public Const C_XDNS As Integer = &H1
        Public Const C_XRelay As Integer = &H2
        Public Const C_XLicense As Integer = &H3
        Public Const C_XTempPath As Integer = &H4
        'Public Const C_XMailPath As Integer = &H5
        Public Const C_XLogPath As Integer = &H6

        Public Const C_Version As String = "C_Version"
        Public Const C_Name As Integer = &HB
        Public Const C_SessionTimeOut As Integer = &HC
        Public Const C_ResponseDelay As Integer = &HD
        Public Const C_VirusType As Integer = &HE
        Public Const C_VirusAllParts As Integer = &HF
        Public Const C_VirusPlugIn As Integer = &H10
        Public Const C_LDAPSSLPort As Integer = &H11
        Public Const C_LDAPPort As Integer = &H12
        Public Const C_SMTPPortMore As Integer = &H13
        Public Const C_NoUIDLPrefix As Integer = &H14
        Public Const C_ServerType As Integer = &H15
        Public Const C_TempPath As Integer = &H16
        ' Merak 8.0.3
        Public Const C_MailPath As String = "C_MailPath"
        ' Merak 8.5.x-x
        Public Const C_System_Storage_Dir_MailPath As String = "C_System_Storage_Dir_MailPath" 'Integer = &H17
        ' Merak ???
        Public Const C_XMailPath As String = "C_XMailPath"

        Public Const C_LogPath As Integer = &H18
        Public Const C_Config_Ports_POP3 As Integer = &H19
        Public Const C_Config_Ports_POP3SSL As Integer = &H1A
        Public Const C_Config_Ports_SMTP As Integer = &H1B
        Public Const C_Config_Ports_IMAPSSL As Integer = &H1C
        Public Const C_Config_Ports_TarpittingPeriod As Integer = &H1D
        Public Const C_Config_Ports_Control As Integer = &H1E
        Public Const C_Config_Ports_IMAP As Integer = &H1F
        Public Const C_Config_Ports_ControlSSL As Integer = &H20
        Public Const C_Config_Ports_MinOlder As Integer = &H21
        Public Const C_Config_AutoStart_POP3 As Integer = &H22
        Public Const C_Config_AutoStart_SMTP As Integer = &H23
        Public Const C_Config_AutoStart_Control As Integer = &H24
        Public Const C_Config_AutoStart_TarpittingCount As Integer = &H25
        Public Const C_Config_System_RelayMailServer As Integer = &H26
        Public Const C_Config_System_BindIP As Integer = &H27
        Public Const C_Config_System_DNSServer As Integer = &H28
        Public Const C_Config_System_UseDNS As Integer = &H29
        Public Const C_Config_System_DenyESMTP As Integer = &H2A
        Public Const C_Config_System_DenyVRFY As Integer = &H2B
        Public Const C_Config_System_DenyEXPN As Integer = &H2C
        Public Const C_Config_System_DenyTelnet As Integer = &H2D
        Public Const C_Config_System_WatchDogSMTP As Integer = &H2E
        Public Const C_Config_System_WatchDogPOP3 As Integer = &H2F
        Public Const C_Config_System_RejectrDNS As Integer = &H30
        Public Const C_Config_System_WatchDogInterval As Integer = &H31
        Public Const C_Config_System_VirusScan As Integer = &H32
        Public Const C_Config_System_UseQuarantine As Integer = &H33
        Public Const C_Config_System_DeleteOlder As Integer = &H34
        Public Const C_Config_System_DiskMonitorActive As Integer = &H35
        Public Const C_Config_System_GreetingRequired As Integer = &H36
        Public Const C_Config_System_DenyWeb As Integer = &H37
        Public Const C_Config_System_MaxConnections As Integer = &H38
        Public Const C_Config_System_MaxRecipients As Integer = &H39
        Public Const C_Config_System_DomainListing As Integer = &H3A
        Public Const C_Config_System_SMTPSSL As Integer = &H3B
        Public Const C_Config_System_HeaderFooter As Integer = &H3C
        Public Const C_Config_System_SMTPScan As Integer = &H3D
        Public Const C_Config_System_RBL As Integer = &H3E
        Public Const C_Config_System_DiskQuota As Integer = &H3F
        Public Const C_Config_System_SafeConfirm As Integer = &H40
        Public Const C_Config_System_NoDomainAdminMailbox As Integer = &H41
        Public Const C_Config_System_NoShowControl As Integer = &H42
        Public Const C_Config_System_NoShowSMTP As Integer = &H43
        Public Const C_Config_System_NoShowPOP3 As Integer = &H44
        Public Const C_Config_System_UseWelcome As Integer = &H45
        Public Const C_Config_System_UseBindIP As Integer = &H46
        Public Const C_Config_System_VirusInfoAdmin As Integer = &H47
        Public Const C_Config_System_DNSTimeout As Integer = &H48
        Public Const C_Config_System_DaemonAlias As Integer = &H49
        Public Const C_Config_System_DaemonName As Integer = &H4A
        Public Const C_Config_System_VirusInfoRecipient As Integer = &H4B
        Public Const C_Config_System_DisableCPU As Integer = &H4C
        Public Const C_Config_System_UseDefaults As Integer = &H4D
        Public Const C_Config_System_MaxBadCommands As Integer = &H4E
        Public Const C_Config_System_POP3Max As Integer = &H4F
        Public Const C_Config_System_SMTPMax As Integer = &H50
        Public Const C_Config_System_SMTPCache As Integer = &H51
        Public Const C_Config_System_POP3Cache As Integer = &H52
        Public Const C_Config_System_ControlCache As Integer = &H53
        Public Const C_Config_System_BackLog As Integer = &H54
        Public Const C_Config_System_DiskMonitorSize As Integer = &H55
        Public Const C_Config_Exchange_MaxData As Integer = &H56
        Public Const C_Config_Exchange_MaxDataSize As Integer = &H57
        Public Const C_Config_Exchange_SearchForAliasInDomains As Integer = &H58
        Public Const C_Config_Exchange_HeaderFunctions As Integer = &H59
        Public Const C_Config_Exchange_NoAutoResponderFor As Integer = &H5A
        Public Const C_Config_Firewall_Active As Integer = &H5B
        Public Const C_Config_Firewall_SMTP_Grant As Integer = &H5C
        Public Const C_Config_Firewall_SMTP_List As Integer = &H5D
        Public Const C_Config_Firewall_POP3_Grant As Integer = &H5E
        Public Const C_Config_Firewall_POP3_List As Integer = &H5F
        Public Const C_Config_Firewall_ODBC_VirusInfoSender As Integer = &H60
        Public Const C_Config_Firewall_ODBC_ConnectionString As Integer = &H61
        Public Const C_Config_Firewall_Control_Grant As Integer = &H62
        Public Const C_Config_Firewall_Control_List As Integer = &H63
        Public Const C_Config_EventLog_SMTP As Integer = &H64
        Public Const C_Config_EventLog_More_OutgoingPacketDelay As Integer = &H65
        Public Const C_Config_EventLog_More_IncomingPacketDelay As Integer = &H66
        Public Const C_Config_EventLog_More_LoggingCache As Integer = &H67
        Public Const C_Config_EventLog_More_DialOnDemandExceed As Integer = &H68
        Public Const C_Config_EventLog_POP3 As Integer = &H69
        Public Const C_Config_EventLog_BadMailAddr As Integer = &H6A
        Public Const C_Config_EventLog_ODBCLoggingConnection As Integer = &H6B
        Public Const C_Config_EventLog_ODBCLogging As Integer = &H6C
        Public Const C_Config_EventLog_LDAPActive As Integer = &H6D
        Public Const C_Config_EventLog_Control As Integer = &H6E
        Public Const C_Config_EventLog_BackupActive As Integer = &H6F
        Public Const C_Config_EventLog_DialOnDemandHeader As Integer = &H70
        Public Const C_Config_EventLog_DialOnDemand As Integer = &H71
        Public Const C_Config_EventLog_WebAdminSecure As Integer = &H72
        Public Const C_Config_EventLog_EmailLogin As Integer = &H73
        Public Const C_Config_EventLog_UserStat As Integer = &H74
        Public Const C_Config_EventLog_DBSetting As Integer = &H75
        Public Const C_Config_EventLog_AllowSMTPAuth As Integer = &H76
        Public Const C_Config_EventLog_ContentFilter As Integer = &H77
        Public Const C_Config_EventLog_HideIP As Integer = &H78
        Public Const C_Config_EventLog_ConvertAuth As Integer = &H79
        Public Const C_Config_EventLog_DisableMailFile As Integer = &H7A
        Public Const C_Config_EventLog_OutputDebug As Integer = &H7B
        Public Const C_Config_DiskMonitor_ReportAddress As Integer = &H7C
        Public Const C_Config_DiskMonitor_QuarantineAddress As Integer = &H7D
        Public Const C_Config_Connect_DialUp As Integer = &H7E
        Public Const C_Config_Connect_EntryName As Integer = &H7F
        Public Const C_Config_Connect_Username As Integer = &H80
        Public Const C_Config_Connect_Password As Integer = &H81
        Public Const C_Config_Connect_HangUpAfter As Integer = &H82
        Public Const C_Config_Delivery_UseDoNotRelay As Integer = &H83
        Public Const C_Config_Delivery_AllowRelayTo As Integer = &H84
        Public Const C_Config_Delivery_UseETRN As Integer = &H85
        Public Const C_Config_Delivery_ETRNCount As Integer = &H86
        Public Const C_Config_Delivery_UseTLSSSL As Integer = &H87
        Public Const C_Config_Delivery_Tarpitting As Integer = &H88
        Public Const C_Config_Delivery_UseSMTP As Integer = &H89
        Public Const C_Config_Delivery_LongDomain As Integer = &H8A
        Public Const C_Config_Delivery_RejectMX As Integer = &H8B
        Public Const C_Config_Delivery_POP3SMTPValue As Integer = &H8C
        Public Const C_Config_Delivery_POP3SMTPRelay As Integer = &H8D
        Public Const C_Config_Delivery_GlobalFilter As Integer = &H8E
        Public Const C_Config_Delivery_UndelivAdmin As Integer = &H8F
        Public Const C_Config_Delivery_MaxHopCount As Integer = &H90
        Public Const C_Config_Delivery_RelayLocal As Integer = &H91
        Public Const C_Config_Delivery_UndeliverableWarning As Integer = &H92
        Public Const C_Config_Delivery_UndeliverableAfter As Integer = &H93
        Public Const C_Config_ETRNConfig As Integer = &H94
        Public Const C_Config_ConnectSchedule As Integer = &H95
        Public Const C_Config_AtomicClockSync As Integer = &H96
        Public Const C_Config_AVLogging As Integer = &H97
        Public Const C_Config_AVDeleteOlder As Integer = &H98
        Public Const C_Config_AutoArchive As Integer = &H99
        Public Const C_Config_ArchivePath As Integer = &H9A
        Public Const C_Config_PassPolicy As Integer = &H9B ' Int
        Public Const C_Config_PassPolicyLength As Integer = &H9C ' Int
        Public Const C_Config_PassPolicyDigits As Integer = &H9D ' Int
        Public Const C_Config_AVCleanIfPossible As Integer = &H9E ' Int
        Public Const C_Config_NoExternalAV As Integer = &H9F ' Int
        Public Const C_Config_UseDomainLimits As Integer = &HA0 ' Int
        Public Const C_Config_DomainIPShielding As Integer = &HA1 ' Int
        Public Const C_Config_UpgradeValue As Integer = &HA2 ' Int
        Public Const C_Config_AntivirusLicense As Integer = &HA3 ' String
        Public Const C_Config_IMLicense As Integer = &HA4 ' String
        Public Const C_Config_IASLicense As Integer = &HA5 ' String
        Public Const C_Config_GWLicense As Integer = &HA6 ' String
        Public Const C_Config_EnsimLicense As Integer = &HA7 ' String
        Public Const C_Config_WebMailLicense As Integer = &HA8 ' String
        Public Const C_ServerLicense As Integer = &HA9 ' String
        Public Const C_ConfigPath As String = "C_ConfigPath" ' String
        Public Const C_XMLLicense As Integer = &HAB ' String
        Public Const C_AVPassword As Integer = &HAC ' Bool
        Public Const C_IDNSupport As Integer = &HAD ' Bool
        Public Const C_WatchDogCalendar As Integer = &HAE ' Bool
        Public Const C_DisableSSLTLS As Integer = &HAF ' Bool
        Public Const C_ODBCMultithreaded As Integer = &HB0 ' Bool
        Public Const C_CalendarLog As Integer = &HB1 ' Int
        Public Const C_RelayTarpit As Integer = &HB2 ' Bool
        Public Const C_DomainLiterals As Integer = &HB3 ' Bool
        Public Const C_EnableIPv6 As Integer = &HB4 ' Bool
        Public Const C_ChallengeMode As Integer = &HB5 ' Int
        Public Const C_RejectLocalUnauthorized As Integer = &HB6 ' Bool
        Public Const C_PassPolicyUserAlias As Integer = &HB7 ' Bool
        Public Const C_CalendarServerPort As Integer = &HB8 ' Int
        Public Const C_CalendarNotActive As Integer = &HB9 ' Bool
        Public Const C_ArchiveDeleteOlder As Integer = &HBA ' Int
        Public Const C_AVLast As Integer = &HBB ' Bool
        Public Const C_MIASMode As Integer = &HBC ' Int
        Public Const C_IMServerPort As Integer = &HBD ' Int
        Public Const C_NoRetryQueue As Integer = &HBE ' Bool
        Public Const C_CloseRBLConn As Integer = &HBF ' Bool
        Public Const C_BlockIPValue As Integer = &HC0 ' Int
        Public Const C_BlockIPTarpitting As Integer = &HC1 ' Bool
        Public Const C_DeliverEmailOnce As Integer = &HC2 ' Bool
        Public Const C_OverrideGlobalLimits As Integer = &HC3 ' Bool
        Public Const C_IMArchiveDeleteDays As Integer = &HC4 ' Int
        Public Const C_IMArchive As Integer = &HC5 ' Bool
        Public Const C_ExternalDelivery As Integer = &HC6 ' Bool
        Public Const C_MXReconnectFailure As Integer = &HC7 ' Bool
        Public Const C_TimeServer As Integer = &HC8 ' Bool
        Public Const C_AVEveryHour As Integer = &HC9 ' Int
        Public Const C_Tunnel As Integer = &HCA ' Bool
        Public Const C_EnableChangePass As Integer = &HCB ' Bool
        Public Const C_NoLicenseBackup As Integer = &HCC ' Bool
        Public Const C_ODBCPasswordEncryption As Integer = &HCD ' Bool
        Public Const C_PermanentSessions As Integer = &HCE ' Bool
        Public Const C_WatchDogIM As Integer = &HCF ' Bool
        Public Const C_RequireAuth As Integer = &HD0 ' Bool
        Public Const C_IMServerFileTransfer As Integer = &HD1 ' Bool
        Public Const C_ServiceID As Integer = &HD2 ' String
        Public Const C_WarnMailboxSize As Integer = &HD3 ' Int
        Public Const C_SMTPLFDotLF As Integer = &HD4 ' Bool
        Public Const C_IMDisabled As Integer = &HD5 ' Bool
        Public Const C_LDAPShared As Integer = &HD6 ' Bool
        Public Const C_IMJabberDomains As Integer = &HD7 ' Bool
        Public Const C_IMAutoStart As Integer = &HD8 ' Bool
        Public Const C_AVAttachmantesQuarantine As Integer = &HD9 ' Bool
        Public Const C_IMUntrustedHosts As Integer = &HDA ' Bool
        Public Const C_IMEnableRegister As Integer = &HDB ' Bool
        Public Const C_IMEnableVersion As Integer = &HDC ' Date
        Public Const C_IMThreadCache As Integer = &HDE ' Int
        Public Const C_NoShowIM As Integer = &HDF ' Bool
        Public Const C_IMLogging As Integer = &HE0 ' Int
        Public Const C_IMPort As Integer = &HE1 ' Int
        Public Const C_IMSSLPort As Integer = &HE2 ' Int
        Public Const C_DomainIPShield As Integer = &HE3 ' Bool
        Public Const C_MemoryModeCache As Integer = &HE4 ' Int
        Public Const C_NoExternalAV As Integer = &HE5 ' Bool
        Public Const C_DNSCache As Integer = &HE6 ' Bool
        Public Const C_AVScanMode As Integer = &HE7 ' Int Bit 0 - All,Selected, Bit 1 = Outgoing, Bit 2,3 - Except, Domains, Accounts
        Public Const C_AVUpdateNotWeekdays As Integer = &HE9 ' Int
        Public Const C_AVActiveUpdate As Integer = &HEA ' Bool
        Public Const C_AVUpdateTime As Integer = &HEB ' Int
        Public Const C_AVActiveAddress As Integer = &HEC ' String
        Public Const C_IMMode As Integer = &HED ' Int
        Public Const C_GroupWareMode As Integer = &HEE ' Int
        Public Const C_MonitorCPUUsagePerc As Integer = &HEF ' Int
        Public Const C_MonitorCPUUsagePeriod As Integer = &HF0 ' Int
        Public Const C_MonitorFreeMem As Integer = &HF1 ' Int
        Public Const C_UseDomainExpiration As Integer = &HF2 ' Bool
        Public Const C_LoginPolicy As Integer = &HF3 ' Bool
        Public Const C_LoginPolicyAttempts As Integer = &HF4 ' Int
        Public Const C_LoginPolicyMinutes As Integer = &HF5 ' Int
        Public Const C_CloseMaxMessageSize As Integer = &HF6 ' Bool
        Public Const C_PassPolicyNonAlphaNum As Integer = &HF7 ' Int
        Public Const C_BanPassRetrieval As Integer = &HF8 ' Bool
        Public Const C_AllowAdminPass As Integer = &HF9 ' Bool
        Public Const C_PassExpire As Integer = &HFA ' Bool
        Public Const C_PassExpireDays As Integer = &HFB ' Int
        Public Const C_EnableLocalDelivery As Integer = &HFC ' Bool
        Public Const C_SpamEngine As Integer = &HFD ' Bool
        Public Const C_BackupMailDir As Integer = &HFE ' Bool
        Public Const C_FirewallIMGrant As Integer = &HFF ' Bool
        Public Const C_FirewallIMList As Integer = &H100 ' String
        Public Const C_FirewallGWGrant As Integer = &H101 ' Bool
        Public Const C_FirewallGWList As Integer = &H102 ' String
        Public Const C_DS_SMTP As Integer = &H103 ' Bool
        Public Const C_DS_POP3 As Integer = &H104 ' Bool
        Public Const C_DS_IMAP As Integer = &H105 ' Bool
        Public Const C_DS_IM As Integer = &H106 ' Bool
        Public Const C_DS_GW As Integer = &H107 ' Bool
        Public Const C_DS_Control As Integer = &H108 ' Bool
        Public Const C_NoTruncatedDeliveryMessage As Integer = &H109 ' Bool
        Public Const C_InstallPath As Integer = &H10A ' String
        Public Const C_AVBlockFiles As Integer = &H10B ' Bool
        Public Const C_FTPPort As Integer = &H10C ' Int
        Public Const C_FTPSSLPort As Integer = &H10D ' Int
        Public Const C_FTPLogging As Integer = &H10E ' Int
        Public Const C_IMAPLogging As Integer = &H10F ' Int
        Public Const C_LDAPLogging As Integer = &H110 ' Int
        Public Const C_AVThreadLock As Integer = &H111 ' Int
    End Class 'MerakInterop

End Class

Public Class Utils

    Public Shared Function IsStringEmpty(ByVal stringToTest As String, ByVal trimmed As Boolean) As Boolean
        If stringToTest Is Nothing Then
            Throw New ArgumentNullException("stringToTest")
        End If

        If trimmed Then
            Return stringToTest.Trim().Length = 0
        Else
            Return stringToTest.Length = 0
        End If
    End Function 'IsStringEmpty

    Public Shared Function IsStringNullOrEmpty(ByVal stringToTest As String, ByVal trimmed As Boolean) As Boolean
        If stringToTest Is Nothing Then
            Return True
        End If
        Return IsStringEmpty(stringToTest, trimmed)
    End Function 'IsStringNullOrEmpty

    Public Shared Function ParseDouble(ByVal str As String, ByVal defaultValue As Double) As Double
        If Not IsStringNullOrEmpty(str, True) Then
            Try
                Return [Double].Parse(str)
            Catch
            End Try
        End If
        Return defaultValue
    End Function 'ParseDouble

    Public Shared Function ParseInt32(ByVal str As String, ByVal defaultValue As Integer) As Integer
        If Not IsStringNullOrEmpty(str, True) Then
            Try
                Return Int32.Parse(str)
            Catch
            End Try
        End If
        Return defaultValue
    End Function 'ParseInt32

    Public Shared Function ParseBoolean(ByVal str As String, ByVal defaultValue As Boolean) As Boolean
        If Not IsStringNullOrEmpty(str, True) Then
            Try
                Return Boolean.Parse(str)
            Catch
            End Try
        End If
        Return defaultValue
    End Function 'ParseBoolean

    Public Shared Function ConcatStrings(ByVal strs() As String, ByVal sep As Char) As String
        Dim sb As New StringBuilder
        Dim s As String
        For Each s In strs
            sb.Append(s)
            sb.Append(sep)
        Next s
        If strs.Length > 0 Then
            sb.Remove(sb.Length - 1, 1)
        End If
        Return sb.ToString()
    End Function 'ConcatStrings
End Class 'Utils

Friend Class Service
    Public ComObject As Object
    Public Succeed As Boolean
End Class

Friend Enum MailType
    POP = 0
    IMAP_POP = 1
    IMAP = 2
End Enum

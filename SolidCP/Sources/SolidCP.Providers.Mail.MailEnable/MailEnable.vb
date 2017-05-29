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
Imports SolidCP.Providers.Utils.LogParser
Imports SolidCP.Server.Utils

Public Class MailEnable
    Inherits HostingServiceProviderBase
    Implements IMailServer

#Region "Domains"

    Public Overridable Function GetDomains() As String() Implements IMailServer.GetDomains

        Dim domainList As List(Of String) = New List(Of String)
        Dim po As New SolidCP.Providers.Mail.MailEnablePostoffice

        po.Account = ""
        po.Name = ""
        po.Status = -1
        If po.FindFirstPostoffice() = 1 Then
            Do
                domainList.Add(po.Name)
                po.Account = ""
                po.Name = ""
                po.Status = -1
            Loop While po.FindNextPostoffice() = 1
        End If

        Return domainList.ToArray()

    End Function

    Public Overridable Function DomainExists(ByVal domainName As String) As Boolean Implements IMailServer.DomainExists
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain

        ResetDomain(domain)
        domain.AccountName = domainName
        domain.DomainName = domainName

        Return (domain.GetDomain() = 1)
    End Function

    Public Overridable Function GetDomain(ByVal domainName As String) As MailDomain Implements IMailServer.GetDomain
        Dim info As MailDomain = Nothing
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain

        ResetDomain(domain)
        domain.AccountName = domainName
        domain.DomainName = domainName

        If (domain.GetDomain() = 1) Then
            info = GetMailDomainInfo(domain)
        Else
            Return Nothing
        End If

        Return info
    End Function

    Public Overridable Sub CreateDomain(ByVal domainInfo As MailDomain) Implements IMailServer.CreateDomain
        'create a new postoffice for each account
        Dim postoffice As New SolidCP.Providers.Mail.MailEnablePostoffice
        postoffice.Account = domainInfo.Name
        postoffice.Name = domainInfo.Name
        postoffice.Status = IIf((domainInfo.Enabled), 1, 0)

        If (postoffice.AddPostoffice() <> 1) Then
            Throw New Exception("Postoffice creation failedNot ")
        End If

        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain
        domain.AccountName = domainInfo.Name
        domain.DomainName = domainInfo.Name
        domain.DomainRedirectionHosts = domainInfo.RedirectionHosts
        domain.DomainRedirectionStatus = IIf((domainInfo.RedirectionActive), 1, 0)
        domain.Status = IIf((domainInfo.Enabled), 1, 0)

        If (domain.AddDomain() <> 1) Then
            Throw New Exception("Domain creation failedNot ")
        End If

        Dim blackListedDomain As String
        For Each blackListedDomain In domainInfo.BlackList

            Dim blacklist As New SolidCP.Providers.Mail.MailEnableDomainBlacklist
            blacklist.Account = domainInfo.Name
            blacklist.Status = 1
            blacklist.TargetDomainName = domainInfo.Name
            blacklist.BannedDomainName = blackListedDomain
            If (blacklist.AddBlacklist() <> 1) Then
                Throw New Exception("Although the domain was created, blacklist creation failed")
            End If
        Next
    End Sub

    Public Overridable Sub UpdateDomain(ByVal info As MailDomain) Implements IMailServer.UpdateDomain
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain
        domain.AccountName = info.Name
        domain.DomainName = info.Name
        domain.DomainRedirectionHosts = String.Empty
        domain.DomainRedirectionStatus = -1
        domain.Host = String.Empty
        domain.Status = -1

        If (domain.GetDomain() = 1) Then

            Dim newStatus As Integer = IIf(info.Enabled, 1, 0)
            Dim newRedirectionStatus As Integer = IIf(info.RedirectionActive, 1, 0)

            'redirection status has 3 states, so we don't use redirectionaction
            '0=off, 1=on, 2=on for authenticated only
            If info("MailEnable_SmartHostEnabled") Then
                If info("MailEnable_SmartHostAuth") Then
                    newRedirectionStatus = 2
                Else
                    newRedirectionStatus = 1
                End If
            Else
                newRedirectionStatus = 0
            End If

            domain.EditDomain( _
                               info.Name, _
                               newStatus, _
                               newRedirectionStatus, _
                               info.RedirectionHosts, _
                               info.Name)

            '
            ' Update the Catch All Account
            '
            Dim oAddressMap As New SolidCP.Providers.Mail.MailEnableAddressMap

            oAddressMap.Account = info.Name ' account
            oAddressMap.DestinationAddress = ""
            oAddressMap.SourceAddress = "[SMTP:*@" & info.Name & "]"
            oAddressMap.Scope = ""

            If info.CatchAllAccount = "" Then
                ' things are tricky here because we want to change it so we know what we are deleting
                oAddressMap.SourceAddress = "[SMTP:*@" & info.Name & "]"
                oAddressMap.DestinationAddress = ""
                oAddressMap.Scope = ""
                '
                ' Change its value if it exists
                '
                If oAddressMap.EditAddressMap(info.Name, "[DELETE:ME]", "[DELETE:ME]", "0", oAddressMap.Status) Then
                    oAddressMap.Account = info.Name
                    oAddressMap.DestinationAddress = "[DELETE:ME]"
                    oAddressMap.SourceAddress = "[DELETE:ME]"
                    oAddressMap.Scope = "0"
                    Dim Result As Integer = oAddressMap.RemoveAddressMap
                End If
            Else
                Dim NewAccount As String = info.Name
                Dim NewSourceAddress As String = "[SMTP:*@" & info.Name & "]"
                Dim NewDestinationAddress As String = "[SF:" & info.Name & "/" & GetMailboxName(info.CatchAllAccount) & "]"
                Dim NewScope As String = "0"

                If oAddressMap.EditAddressMap(NewAccount, NewSourceAddress, NewDestinationAddress, NewScope, 0) <> 1 Then
                    '
                    ' We need to add it because there was not one defined  
                    '
                    oAddressMap.SourceAddress = "[SMTP:*@" & info.Name & "]"
                    oAddressMap.DestinationAddress = "[SF:" & info.Name & "/" & GetMailboxName(info.CatchAllAccount) & "]"
                    oAddressMap.Scope = "0"
                    oAddressMap.Account = info.Name
                    oAddressMap.AddAddressMap()
                End If
            End If

            '
            ' Update the Postmaster Account
            '
            oAddressMap.Account = info.Name
            oAddressMap.DestinationAddress = ""
            oAddressMap.SourceAddress = "[SMTP:Postmaster@" & info.Name & "]"
            oAddressMap.Scope = ""
            If (info.PostmasterAccount = "") Then
                oAddressMap.SourceAddress = "[SMTP:Postmaster@" & info.Name & "]"
                oAddressMap.DestinationAddress = ""
                oAddressMap.Scope = ""
                Dim Result As Integer = oAddressMap.RemoveAddressMap
            Else
                Dim NewAccount = info.Name
                Dim NewSourceAddress = "[SMTP:postmaster@" & info.Name & "]"
                Dim NewDestinationAddress = "[SF:" & info.Name & "/" & GetMailboxName(info.PostmasterAccount) & "]"
                Dim NewScope = "0"
                If oAddressMap.EditAddressMap(NewAccount, NewSourceAddress, NewDestinationAddress, NewScope, 0) <> 1 Then
                    '
                    ' We need to add it because there was not one defined  
                    '
                    oAddressMap.SourceAddress = "[SMTP:Postmaster@" & info.Name & "]"
                    oAddressMap.DestinationAddress = "[SF:" & info.Name & "/" & GetMailboxName(info.PostmasterAccount) & "]"
                    oAddressMap.Scope = "0"
                    oAddressMap.Account = info.Name
                    oAddressMap.AddAddressMap()
                End If
            End If

            '
            ' Update the Abuse Account
            '
            oAddressMap.Account = info.Name
            oAddressMap.DestinationAddress = ""
            oAddressMap.SourceAddress = "[SMTP:abuse@" & info.Name & "]"
            oAddressMap.Scope = ""
            If (info.AbuseAccount = "") Then
                oAddressMap.SourceAddress = "[SMTP:abuse@" & info.Name & "]"
                oAddressMap.DestinationAddress = ""
                oAddressMap.Scope = ""
                Dim Result = oAddressMap.RemoveAddressMap
            Else
                Dim NewAccount = info.Name
                Dim NewSourceAddress = "[SMTP:Abuse@" & info.Name & "]"
                Dim NewDestinationAddress = "[SF:" & info.Name & "/" & GetMailboxName(info.AbuseAccount) & "]"
                Dim NewScope = "0"
                If oAddressMap.EditAddressMap(NewAccount, NewSourceAddress, NewDestinationAddress, NewScope, 0) <> 1 Then
                    '
                    ' We need to add it because there was not one defined  
                    '
                    oAddressMap.SourceAddress = "[SMTP:abuse@" & info.Name & "]"
                    oAddressMap.DestinationAddress = "[SF:" & info.Name & "/" & GetMailboxName(info.AbuseAccount) & "]"
                    oAddressMap.Scope = "0"
                    oAddressMap.Account = info.Name
                    oAddressMap.AddAddressMap()
                End If
            End If


            'edit blacklists 
            'delete all the blacklists
            Dim blacklist As New SolidCP.Providers.Mail.MailEnableDomainBlacklist
            ResetBlacklist(blacklist)
            blacklist.Account = info.Name
            blacklist.TargetDomainName = info.Name

            While blacklist.FindFirstBlacklist() = 1
                ' remove blacklist
                blacklist.RemoveBlacklist()

                ' initialize blacklist again
                blacklist = New SolidCP.Providers.Mail.MailEnableDomainBlacklist
                ResetBlacklist(blacklist)
                blacklist.Account = info.Name
                blacklist.TargetDomainName = info.Name
            End While

            'add new blacklists
            Dim blacklistedDomainName As String
            For Each blacklistedDomainName In info.BlackList
                blacklist = New SolidCP.Providers.Mail.MailEnableDomainBlacklist
                blacklist.Account = info.Name
                blacklist.TargetDomainName = info.Name
                blacklist.BannedDomainName = blacklistedDomainName
                blacklist.Status = IIf((info.Enabled), 1, 0)

                ' add blacklist
                blacklist.AddBlacklist()
            Next


        End If
    End Sub

    Public Overridable Sub DeleteDomain(ByVal domainName As String) Implements IMailServer.DeleteDomain
        'delete all postoffice logins
        Dim login As New SolidCP.Providers.Mail.MailEnableLogin
        ResetLogin(login)
        login.Account = domainName
        login.RemoveLogin()

        'delete all the mailboxes
        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox

        ResetMailbox(mailbox)
        mailbox.Postoffice = domainName
        mailbox.RemoveMailbox()

        ' delete mailling lists
        Dim lists() As MailList = GetLists(domainName)
        Dim list As MailList
        For Each list In lists
            ' remove list members
            Dim listMember As New SolidCP.Providers.Mail.MailEnableListMember
            listMember.AccountName = domainName
            listMember.ListName = GetMailboxName(list.Name)
            listMember.Address = ""
            listMember.ListMemberType = -1
            listMember.Status = -1
            listMember.RemoveListMember()

            ' delete maillist
            Dim mailList As New SolidCP.Providers.Mail.MailEnableList
            ResetMaillist(mailList)
            mailList.AccountName = domainName
            mailList.ListName = GetMailboxName(list.Name)
            mailList.RemoveList()
        Next

        ' delete groups
        Dim groups() As MailGroup = GetGroups(domainName)
        Dim group As MailGroup
        For Each group In groups
            ' remove group members
            Dim groupMember As New SolidCP.Providers.Mail.MailEnableGroupMember
            groupMember.Postoffice = domainName
            groupMember.Mailbox = GetMailboxName(group.Name)
            groupMember.Address = ""
            groupMember.RemoveGroupMember()

            ' delete group
            Dim objGroup As New SolidCP.Providers.Mail.MailEnableGroup
            ResetGroup(objGroup)
            objGroup.Postoffice = domainName
            objGroup.GroupName = GetMailboxName(group.Name)
            objGroup.RemoveGroup()
        Next

        'delete all address mappings
        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap
        ResetAddressMap(map)
        map.Account = domainName
        map.RemoveAddressMap(True)

        'delete all the blacklists
        Dim blacklist As New SolidCP.Providers.Mail.MailEnableDomainBlacklist
        ResetBlacklist(blacklist)
        blacklist.Account = domainName
        blacklist.RemoveBlacklist()

        'delete all domains
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain
        ResetDomain(domain)
        domain.AccountName = domainName
        domain.RemoveDomain()

        'delete postoffice
        Dim po As New SolidCP.Providers.Mail.MailEnablePostoffice
        po.Account = domainName
        po.Name = domainName
        po.Host = ""
        po.Status = -1

        If (po.RemovePostoffice() <> 1) Then
            Throw New Exception(String.Format("Could  not remove postoffice '{0}'", domainName))
        End If

        ' delete postoffice directory
        DeletePostofficeDirectory(domainName)
    End Sub

    Private Function GetMailDomainInfo(ByVal domain As Object) As MailDomain

        Dim info As MailDomain = New MailDomain

        info.Name = domain.DomainName
        info.RedirectionHosts = domain.DomainRedirectionHosts
        info.RedirectionActive = (domain.DomainRedirectionStatus = 1)

        If domain.DomainRedirectionStatus = 2 Then
            info("MailEnable_SmartHostAuth") = True
            info("MailEnable_SmartHostEnabled") = True
        ElseIf domain.DomainRedirectionStatus = 1 Then
            info("MailEnable_SmartHostEnabled") = True
            info("MailEnable_SmartHostAuth") = False
        Else
            info("MailEnable_SmartHostEnabled") = false
            info("MailEnable_SmartHostAuth") = False
        End If

        info.Enabled = (domain.Status = 1)

        '
        ' We need to get the catch all account for the domain
        '
        Dim oAddressMap As New SolidCP.Providers.Mail.MailEnableAddressMap
        oAddressMap.Account = info.Name
        oAddressMap.DestinationAddress = ""
        oAddressMap.SourceAddress = "[SMTP:*@" & info.Name & "]"
        oAddressMap.Scope = ""
        If oAddressMap.GetAddressMap = 1 Then
            Dim FrmCatchAllAccount = Mid(oAddressMap.DestinationAddress, InStr(1, oAddressMap.DestinationAddress, "/") + 1)
            info.CatchAllAccount = Left(FrmCatchAllAccount, Len(FrmCatchAllAccount) - 1)
        Else
            info.CatchAllAccount = ""
        End If
        '
        ' Get the Postmaster Address Map
        '
        oAddressMap.Account = info.Name
        oAddressMap.DestinationAddress = ""
        oAddressMap.SourceAddress = "[SMTP:postmaster@" & info.Name & "]"
        oAddressMap.Scope = ""
        If oAddressMap.GetAddressMap = 1 Then
            Dim FrmPostmasterAccount = Mid(oAddressMap.DestinationAddress, InStr(1, oAddressMap.DestinationAddress, "/") + 1)
            info.PostmasterAccount = Left(FrmPostmasterAccount, Len(FrmPostmasterAccount) - 1)
        Else
            info.PostmasterAccount = ""
        End If
        '
        ' Get the Abuse Address Map
        '
        oAddressMap.Account = info.Name
        oAddressMap.DestinationAddress = ""
        oAddressMap.SourceAddress = "[SMTP:abuse@" & info.Name & "]"
        oAddressMap.Scope = ""
        If oAddressMap.GetAddressMap = 1 Then
            Dim FrmAbuseAccount = Mid(oAddressMap.DestinationAddress, InStr(1, oAddressMap.DestinationAddress, "/") + 1)
            info.AbuseAccount = Left(FrmAbuseAccount, Len(FrmAbuseAccount) - 1)
        Else
            info.AbuseAccount = "(None)"
        End If
        oAddressMap = Nothing

        'getting black mail list
        Dim blacklists As ArrayList = New ArrayList

        Dim blacklist As New SolidCP.Providers.Mail.MailEnableDomainBlacklist
        blacklist.Account = domain.AccountName
        blacklist.Host = domain.Host
        blacklist.TargetDomainName = domain.DomainName
        blacklist.Status = -1
        blacklist.BannedDomainName = ""

        If blacklist.FindFirstBlacklist() = 1 Then
            blacklists.Add(blacklist.BannedDomainName)
        End If

        blacklist.Status = -1
        blacklist.BannedDomainName = ""

        While blacklist.FindNextBlacklist() = 1
            blacklists.Add(blacklist.BannedDomainName)
            blacklist.Status = -1
            blacklist.BannedDomainName = ""
        End While

        info.BlackList = CType(blacklists.ToArray(GetType(String)), String())

        Return info

    End Function
#End Region

#Region "Domain Aliases"
    Public Overridable Function DomainAliasExists(ByVal domainName As String, ByVal aliasName As String) As Boolean Implements IMailServer.DomainAliasExists
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain

        ResetDomain(domain)
        domain.AccountName = domainName
        domain.DomainName = aliasName

        Return (domain.GetDomain() = 1)
    End Function

    Public Overridable Function GetDomainAliases(ByVal domainName As String) As String() Implements IMailServer.GetDomainAliases
        Dim aliases As List(Of String) = New List(Of String)
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain

        ResetDomain(domain)
        domain.AccountName = domainName

        If (domain.FindFirstDomain() = 1) Then
            If domain.DomainName.ToLower() <> domainName.ToLower() Then
                aliases.Add(domain.DomainName)
            End If
            ResetDomain(domain)
            domain.AccountName = domainName
        End If

        While domain.FindNextDomain() = 1
            If domain.DomainName.ToLower() <> domainName.ToLower() Then
                aliases.Add(domain.DomainName)
            End If
            ResetDomain(domain)
            domain.AccountName = domainName
        End While

        Return aliases.ToArray()
    End Function

    Public Overridable Sub AddDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.AddDomainAlias
        ' add new domain
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain
        domain.AccountName = domainName
        domain.DomainName = aliasName
        domain.DomainRedirectionHosts = ""
        domain.DomainRedirectionStatus = 0 ' disabled
        domain.Status = 1 ' enabled

        If (domain.AddDomain() <> 1) Then
            Throw New Exception("Can't create domain alias")
        End If

        ' add address maps
        ' get current "main domain" address mappings
        Dim srcAddr As String = "@" + domainName + "]"
        Dim maps As ArrayList = New ArrayList
        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap

        ResetAddressMap(map)
        map.Account = domainName

        If (map.FindFirstAddressMap() = 1) Then
            Do
                If map.SourceAddress.ToLower().IndexOf(srcAddr) > -1 Then
                    maps.Add(New String() {map.DestinationAddress, map.SourceAddress})
                End If

                ResetAddressMap(map)
                map.Account = domainName
            Loop While (map.FindNextAddressMap() = 1)
        End If

        Dim mapInfo() As String
        For Each mapInfo In maps
            Dim userName As String = mapInfo(1).Substring(6, mapInfo(1).IndexOf("@") - 6)
            ResetAddressMap(map)
            map.Account = domainName
            map.DestinationAddress = mapInfo(0)
            map.SourceAddress = String.Format("[SMTP:{0}]", userName + "@" + aliasName)

            map.AddAddressMap()
        Next
    End Sub

    Public Overridable Sub DeleteDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.DeleteDomainAlias
        'delete all address mappings
        Dim addr As String = "@" + aliasName.ToLower() + "]"
        Dim maps As ArrayList = New ArrayList
        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap
        ResetAddressMap(map)
        map.Account = domainName
        map.SourceAddress = "[SMTP:*@" + aliasName + "]"
        map.RemoveAddressMap(True)

        'delete all the blacklists
        Dim blacklist As New SolidCP.Providers.Mail.MailEnableDomainBlacklist
        ResetBlacklist(blacklist)
        blacklist.Account = domainName
        blacklist.TargetDomainName = aliasName
        blacklist.RemoveBlacklist()

        'delete all domains
        Dim domain As New SolidCP.Providers.Mail.MailEnableDomain
        ResetDomain(domain)
        domain.AccountName = domainName
        domain.DomainName = aliasName
        domain.RemoveDomain()
    End Sub
#End Region

#Region "Accounts"

    Public Overridable Function GetAccounts(ByVal domainName As String) As MailAccount() Implements IMailServer.GetAccounts

        Dim mailboxes As List(Of MailAccount) = New List(Of MailAccount)
        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox

        ResetMailbox(mailbox)
        mailbox.Postoffice = domainName
        mailbox.Size = -4                   'we use -4 since this prevents the function from calculating quotas, which is slow

        If mailbox.FindFirstMailbox() = 1 Then
            Do
                ' add mailbox
                Dim account As MailAccount = GetMailboxInfo(mailbox)
                If (Not account.DeleteOnForward) Then
                    mailboxes.Add(account)
                End If
                ResetMailbox(mailbox)
                mailbox.Postoffice = domainName
                mailbox.Size = -4
            Loop While mailbox.FindNextMailbox() = 1
        End If

        Return mailboxes.ToArray()
    End Function

    Public Overridable Function GetAccount(ByVal mailboxName As String) As MailAccount Implements IMailServer.GetAccount
        Dim info As MailAccount = Nothing
        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox
        ResetMailbox(mailbox)
        mailbox.Postoffice = GetDomainName(mailboxName)
        mailbox.MailboxName = GetMailboxName(mailboxName)

        If (mailbox.GetMailbox() <> 1) Then
            Throw New Exception("Could not find the mailbox")
        End If

        info = GetMailboxInfo(mailbox)

        ' read autoresponder info
        ReadMailboxAutoresponderFile(info)
        Return info
    End Function

    Private Function GetMailboxInfo(ByVal mailbox As SolidCP.Providers.Mail.MailEnableMailbox) As MailAccount
        Dim info As MailAccount = New MailAccount
        info.MaxMailboxSize = IIf(mailbox.Limit = -1, 0, mailbox.Limit / 1024)
        info.Name = mailbox.MailboxName + "@" + mailbox.Postoffice

        Dim redirectAddrs As ArrayList = New ArrayList
        Dim smtpAddress As String
        For Each smtpAddress In mailbox.RedirectAddress.Split(";"c)
            If (smtpAddress.Trim() <> String.Empty) Then
                redirectAddrs.Add(GetEmailString(smtpAddress))
            End If
        Next

        info.ForwardingAddresses = CType(redirectAddrs.ToArray(GetType(String)), String())

        Dim st As Integer = mailbox.RedirectStatus
        Dim ra As String = mailbox.RedirectAddress

        'info.RedirectionActive = (mailbox.RedirectStatus > 0)
        info.DeleteOnForward = (mailbox.RedirectStatus.Equals(1))
        info.Enabled = (mailbox.Status = 1)

        info.ResponderEnabled = mailbox.GetAutoResponderStatus()
        info.ResponderMessage = mailbox.GetAutoResponderContents()
        info.ReplyTo = GetMailBoxReplyToAddress(info.Name)

        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap
        map.Account = info.Name
        map.DestinationAddress = String.Format("[SF:{0}/{1}]", info.Name, info.Name)
        map.SourceAddress = ""

        Dim login As New SolidCP.Providers.Mail.MailEnableLogin
        ResetLogin(login)
        login.Account = mailbox.Postoffice
        login.UserName = info.Name

        If (login.GetLogin() = 1) Then
            info.Password = login.Password
            'info.MailboxRights = CType([Enum].Parse(GetType(MailboxRights), login.Rights, True), MailboxRights)
        End If

        Return info
    End Function

    Private Function GetMailAliasInfo(ByVal mailbox As SolidCP.Providers.Mail.MailEnableMailbox) As MailAlias
        Dim info As MailAlias = New MailAlias
        info.Name = mailbox.MailboxName + "@" + mailbox.Postoffice

        Dim redirectAddrs As ArrayList = New ArrayList
        Dim smtpAddress As String
        For Each smtpAddress In mailbox.RedirectAddress.Split(";"c)
            If (smtpAddress.Trim() <> String.Empty) Then
                redirectAddrs.Add(GetEmailString(smtpAddress))
            End If
        Next
        info.ForwardingAddresses = CType(redirectAddrs.ToArray(GetType(String)), String())
        If (Not (info.ForwardingAddresses Is Nothing) And info.ForwardingAddresses.Length > 0) Then
            info.ForwardTo = info.ForwardingAddresses(0)
        End If
        Dim st As Integer = mailbox.RedirectStatus
        Dim ra As String = mailbox.RedirectAddress

        'info.RedirectionActive = (mailbox.RedirectStatus > 0)
        info.DeleteOnForward = (mailbox.RedirectStatus.Equals(1))
        info.Enabled = (mailbox.Status = 1)

        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap
        map.Account = info.Name
        map.DestinationAddress = String.Format("[SF:{0}/{1}]", info.Name, info.Name)
        map.SourceAddress = ""

        Dim login As New SolidCP.Providers.Mail.MailEnableLogin
        ResetLogin(login)
        login.Account = mailbox.Postoffice
        login.UserName = info.Name

        If (login.GetLogin() = 1) Then
            info.Password = login.Password
            'info.MailboxRights = CType([Enum].Parse(GetType(MailboxRights), login.Rights, True), MailboxRights)
        End If

        Return info
    End Function

    Public Overridable Function AccountExists(ByVal mailboxName As String) As Boolean Implements IMailServer.AccountExists
        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox
        ResetMailbox(mailbox)
        mailbox.Postoffice = GetDomainName(mailboxName)
        mailbox.MailboxName = GetMailboxName(mailboxName)

        Return (mailbox.GetMailbox() = 1)
    End Function

    Public Overridable Sub CreateAccount(ByVal info As MailAccount) Implements IMailServer.CreateAccount
        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox
        Dim domainName As String = GetDomainName(info.Name)
        Dim mailboxName As String = GetMailboxName(info.Name)
        mailbox.Postoffice = domainName

        mailbox.Limit = IIf(info.MaxMailboxSize = 0, -1, info.MaxMailboxSize * 1024) ' convert to kilobytes
        mailbox.MailboxName = GetMailboxName(info.Name)

        If info.ForwardingAddresses Is Nothing Then
            info.ForwardingAddresses = New String() {}
        End If

        Dim sbAddresses As StringBuilder = New StringBuilder
        Dim address As String
        For Each address In info.ForwardingAddresses
            sbAddresses.AppendFormat("[SMTP:{0}];", address)
        Next
        Dim redirectAddrs As String = sbAddresses.ToString()
        If redirectAddrs.Length > 0 Then
            redirectAddrs = redirectAddrs.Substring(0, redirectAddrs.Length - 1)
        End If
        mailbox.RedirectAddress = redirectAddrs

        Dim redirectStatus As Integer = 0
        If (Not (info.ForwardingAddresses Is Nothing) And info.ForwardingAddresses.Length > 0) Then
            redirectStatus = 1

            If (Not info.DeleteOnForward) Then
                redirectStatus = redirectStatus + 1
            End If
        End If

        mailbox.RedirectStatus = redirectStatus
        mailbox.Status = IIf(info.Enabled, 1, 0)

        If (mailbox.AddMailbox() <> 1) Then
            Throw New Exception(" ' AddMailbox ' method Returned 0 ")
        End If

        mailbox.SetAutoResponderStatus(info.ResponderEnabled)
        mailbox.SetAutoResponderContents("", info.ResponderMessage)
        mailbox = Nothing

        ' create address maps for all domains
        Dim destinationAddress As String = String.Format("[SF:{0}/{1}]", domainName, mailboxName)
        CreateAddressMapsForAllDomains(domainName, mailboxName, destinationAddress)

        ' create login
        Dim login As New SolidCP.Providers.Mail.MailEnableLogin
        login.Account = domainName
        login.Password = info.Password
        login.Status = IIf(info.Enabled, 1, 0)

        login.UserName = info.Name
        login.Rights = "USER" ' info.MailboxRights.ToString()

        If (login.AddLogin() <> 1) Then
            Throw New Exception("AddLogin method Returned 0 ")
        End If

        ' change mailbox in order to set autoresponder
        UpdateAccount(info)
    End Sub

    Public Overridable Sub UpdateAccount(ByVal info As MailAccount) Implements IMailServer.UpdateAccount
        ' change mailbox
        Dim domainName As String = GetDomainName(info.Name)
        Dim mailboxName As String = GetMailboxName(info.Name)

        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox
        mailbox.Postoffice = domainName
        mailbox.MailboxName = mailboxName

        If info.ForwardingAddresses Is Nothing Then
            info.ForwardingAddresses = New String() {}
        End If

        If (mailbox.GetMailbox() <> 1) Then
            Throw New Exception("Can't find specified mailboxNot ")
        End If

        Dim sbAddresses As StringBuilder = New StringBuilder
        Dim address As String
        For Each address In info.ForwardingAddresses
            sbAddresses.AppendFormat("[SMTP:{0}];", address)
        Next
        Dim redirectAddrs As String = sbAddresses.ToString()
        If redirectAddrs.Length > 0 Then
            redirectAddrs = redirectAddrs.Substring(0, redirectAddrs.Length - 1)
        End If

        Dim redirectStatus As Integer = 0
        If (Not (info.ForwardingAddresses Is Nothing) And info.ForwardingAddresses.Length > 0) Then
            redirectStatus = 1

            If (Not info.DeleteOnForward) Then
                redirectStatus = redirectStatus + 1
            End If
        End If

        mailbox.EditMailbox(domainName, _
         mailboxName, _
         redirectAddrs, _
         redirectStatus, _
         IIf(info.Enabled, 1, 0), _
         IIf(info.MaxMailboxSize = 0, -1, info.MaxMailboxSize * 1024), _
         0)

        If (String.IsNullOrEmpty(info.ReplyTo) = False) Then
            SetMailBoxReplyToAddress(info.Name, info.ReplyTo)
        Else
            SetMailBoxReplyToAddress(info.Name, "")
        End If

        mailbox.SetAutoResponderStatus(info.ResponderEnabled)
        mailbox.SetAutoResponderContents("", info.ResponderMessage)
        mailbox = Nothing

        'mail Alias random password 
        If (String.IsNullOrEmpty(info.Password)) Then
            info.Password = Guid.NewGuid().ToString("N").Substring(0, 12)
        End If

        ' change login password
        If (info.Password.Length > 0) Then
            Dim login As New SolidCP.Providers.Mail.MailEnableLogin
            ResetLogin(login)
            login.Account = domainName
            login.UserName = info.Name

            If (login.GetLogin() <> 1) Then
                Throw New Exception("Can't find specified loginNot ")
            End If

            login.EditLogin(login.UserName, _
             IIf(info.Enabled, 1, 0), _
             info.Password, _
             domainName, _
             "", _
             0, 0, 0, "USER") ' USER | ADMIN | SYSADMIN
        End If

        ' build autoresponder file
        WriteMailboxAutoresponderFile(info)


    End Sub

    Public Overridable Sub DeleteAccount(ByVal name As String) Implements IMailServer.DeleteAccount
        Dim domainName As String = GetDomainName(name)
        Dim mailboxName As String = GetMailboxName(name)

        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox
        ResetMailbox(mailbox)
        mailbox.Postoffice = domainName
        mailbox.MailboxName = mailboxName

        If (mailbox.RemoveMailbox() <> 1) Then
            Throw New Exception(String.Format("Could not delete mailbox '{0}'", mailboxName))
        End If

        'delete the login for this mailbox
        Dim login As New SolidCP.Providers.Mail.MailEnableLogin
        ResetLogin(login)
        login.Account = domainName
        login.UserName = name

        If (login.RemoveLogin() <> 1) Then
            Throw New Exception(String.Format("Could not delete login '{0}'", name))
        End If


        'delete the address map for this mailbox
        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap
        ResetAddressMap(map)
        map.Account = domainName
        map.DestinationAddress = String.Format("[SF:{0}/{1}]", domainName, mailboxName)
        map.RemoveAddressMap()

        ' delete account folder
        DeleteMailBoxDirectory(name)
    End Sub

    Public Function MailAliasExists(ByVal mailAliasName As String) As Boolean Implements IMailServer.MailAliasExists
        Return AccountExists(mailAliasName)
    End Function

    Public Function GetMailAliases(ByVal domainName As String) As MailAlias() Implements IMailServer.GetMailAliases

        Dim mailAliases As List(Of MailAlias) = New List(Of MailAlias)
        Dim mailbox As New SolidCP.Providers.Mail.MailEnableMailbox

        ResetMailbox(mailbox)
        mailbox.Postoffice = domainName
        mailbox.Size = -4

        If mailbox.FindFirstMailbox() = 1 Then
            Do
                Dim mailAlias As MailAlias = GetMailAliasInfo(mailbox)
                If (mailAlias.DeleteOnForward) Then
                    mailAliases.Add(mailAlias)
                End If
                ResetMailbox(mailbox)
                mailbox.Postoffice = domainName
                mailbox.Size = -4
            Loop While mailbox.FindNextMailbox() = 1
        End If

        Return mailAliases.ToArray()
    End Function


    Public Function GetMailAlias(ByVal mailAliasName As String) As MailAlias Implements IMailServer.GetMailAlias
        Dim info As MailAlias = Nothing
        Dim mailAlias As New SolidCP.Providers.Mail.MailEnableMailbox
        ResetMailbox(mailAlias)
        mailAlias.Postoffice = GetDomainName(mailAliasName)
        mailAlias.MailboxName = GetMailboxName(mailAliasName)

        If (mailAlias.GetMailbox() <> 1) Then
            Throw New Exception("Could not find the mailbox")
        End If

        info = GetMailAliasInfo(mailAlias)

        ' read autoresponder info
        'ReadMailboxAutoresponderFile(info)
        Return info
    End Function

    Public Sub CreateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.CreateMailAlias
        CreateAccount(mailAlias)
    End Sub

    Public Sub UpdateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.UpdateMailAlias
        UpdateAccount(mailAlias)
    End Sub

    Public Sub DeleteMailAlias(ByVal mailAliasName As String) Implements IMailServer.DeleteMailAlias
        DeleteAccount(mailAliasName)
    End Sub

#End Region

#Region "Groups"
    ' ============================
    '       GROUPS
    ' ============================

    Public Overridable Function GroupExists(ByVal groupName As String) As Boolean Implements IMailServer.GroupExists
        Dim group As New SolidCP.Providers.Mail.MailEnableGroup
        ResetGroup(group)
        group.Postoffice = GetDomainName(groupName)
        group.GroupName = GetMailboxName(groupName)

        Return (group.GetGroup() = 1)
    End Function

    Public Overridable Function GetGroup(ByVal groupName As String) As MailGroup Implements IMailServer.GetGroup
        Dim objGroup As New SolidCP.Providers.Mail.MailEnableGroup
        ResetGroup(objGroup)
        objGroup.Postoffice = GetDomainName(groupName)
        objGroup.GroupName = GetMailboxName(groupName)

        If (objGroup.GetGroup() = 1) Then
            Return GetGroupInfo(objGroup)
        End If

        Return Nothing
    End Function

    Public Overridable Function GetGroups(ByVal domainName As String) As MailGroup() Implements IMailServer.GetGroups
        Dim groups As List(Of MailGroup) = New List(Of MailGroup)

        Dim objGroup As New SolidCP.Providers.Mail.MailEnableGroup
        ResetGroup(objGroup)
        objGroup.Postoffice = domainName

        If objGroup.FindFirstGroup() = 1 Then
            Do
                ' add group
                groups.Add(GetGroupInfo(objGroup))

                ResetGroup(objGroup)
                objGroup.Postoffice = domainName
            Loop While objGroup.FindNextGroup() = 1
        End If

        Return groups.ToArray()
    End Function

    Public Overridable Sub CreateGroup(ByVal group As MailGroup) Implements IMailServer.CreateGroup
        Dim domainName As String = GetDomainName(group.Name)
        Dim groupName As String = GetMailboxName(group.Name)

        ' process parameters
        If group.Members Is Nothing Then
            group.Members = New String() {}
        End If

        Dim objGroup As New SolidCP.Providers.Mail.MailEnableGroup
        ResetGroup(objGroup)

        objGroup.Postoffice = domainName
        objGroup.GroupName = groupName
        objGroup.RecipientAddress = String.Format("[SF:{0}/{1}]", domainName, groupName)
        objGroup.GroupStatus = IIf(group.Enabled, 1, 0)

        If (objGroup.AddGroup() = 1) Then
            ' add address maps
            CreateAddressMapsForAllDomains(domainName, groupName, objGroup.RecipientAddress)

            ' add group members
            Dim member As String
            For Each member In group.Members
                Dim groupMember As New SolidCP.Providers.Mail.MailEnableGroupMember
                groupMember.Postoffice = domainName
                groupMember.Address = String.Format("[SMTP:{0}]", member)
                groupMember.Mailbox = groupName

                If (groupMember.AddGroupMember() <> 1) Then
                    Throw New Exception("Group member creation failed")
                End If
            Next
        End If
    End Sub

    Public Overridable Sub UpdateGroup(ByVal group As MailGroup) Implements IMailServer.UpdateGroup
        Dim domainName As String = GetDomainName(group.Name)
        Dim groupName As String = GetMailboxName(group.Name)

        ' process parameters
        If group.Members Is Nothing Then
            group.Members = New String() {}
        End If

        Dim objGroup As New SolidCP.Providers.Mail.MailEnableGroup
        ResetGroup(objGroup)

        objGroup.Postoffice = domainName
        objGroup.GroupName = groupName

        If (objGroup.GetGroup() <> 1) Then
            Throw New Exception("Can't find specified group")
        End If

        objGroup.EditGroup( _
            String.Format("[SF:{0}/{1}]", domainName, groupName), _
            domainName, _
            groupName, _
            "", _
            IIf(group.Enabled, 1, 0))

        'delete group members
        Dim objMember As New SolidCP.Providers.Mail.MailEnableGroupMember
        objMember.Postoffice = domainName
        objMember.Mailbox = groupName
        objMember.Address = ""
        objMember.RemoveGroupMember()

        ' add group members
        Dim member As String
        For Each member In group.Members
            Dim groupMember As New SolidCP.Providers.Mail.MailEnableGroupMember
            groupMember.Postoffice = domainName
            groupMember.Address = String.Format("[SMTP:{0}]", member)
            groupMember.Mailbox = groupName

            If (groupMember.AddGroupMember() <> 1) Then
                Throw New Exception("Group member creation failed")
            End If
        Next
    End Sub

    Public Overridable Sub DeleteGroup(ByVal name As String) Implements IMailServer.DeleteGroup
        Dim domainName As String = GetDomainName(name)
        Dim groupName As String = GetMailboxName(name)

        ' remove group
        Dim objGroup As New SolidCP.Providers.Mail.MailEnableGroup
        ResetGroup(objGroup)
        objGroup.Postoffice = domainName
        objGroup.GroupName = groupName
        objGroup.RemoveGroup()

        'delete group members
        Dim objMember As New SolidCP.Providers.Mail.MailEnableGroupMember
        objMember.Postoffice = domainName
        objMember.Mailbox = groupName
        objMember.Address = ""
        objMember.RemoveGroupMember()

        ' delete address maps
        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap
        ResetAddressMap(map)
        map.Account = domainName
        map.DestinationAddress = String.Format("[SF:{0}/{1}]", domainName, groupName)
        map.RemoveAddressMap()
    End Sub
#End Region

#Region "Lists"

    Public Overridable Function GetList(ByVal maillistName As String) As MailList Implements IMailServer.GetList
        Dim mailList As New SolidCP.Providers.Mail.MailEnableList
        ResetMaillist(mailList)
        mailList.AccountName = GetDomainName(maillistName)
        mailList.ListName = GetMailboxName(maillistName)

        If (mailList.GetList() = 1) Then
            Return GetMaillistInfo(mailList)
        End If

        Return Nothing
    End Function

    Public Overridable Function GetLists(ByVal domainName As String) As MailList() Implements IMailServer.GetLists
        Dim maillists As List(Of MailList) = New List(Of MailList)

        Try
            Dim mailList As New SolidCP.Providers.Mail.MailEnableList
            ResetMaillist(mailList)
            mailList.AccountName = domainName

            If mailList.FindFirstList() = 1 Then
                Do
                    ' add mailist
                    maillists.Add(GetMaillistInfo(mailList))

                    ResetMaillist(mailList)
                    mailList.AccountName = domainName
                Loop While mailList.FindNextList() = 1
            End If

        Catch ex As Exception
            Throw New Exception("Can't get domain maillists", ex)
        End Try

        Return maillists.ToArray()
    End Function

    Private Function GetMaillistInfo(ByVal mailList As Object) As MailList
        Dim info As MailList = New MailList
        info.Name = mailList.ListName + "@" + mailList.AccountName
        info.Description = mailList.Description
        info.Enabled = (mailList.ListStatus = 1)
        info.Moderated = (mailList.ListType = 1)
        info.ModeratorAddress = GetEmailString(mailList.ModeratorAddress)
        info.Password = mailList.Password
        info.PostingMode = CType(mailList.PostingMode, PostingMode)
        info.ReplyToMode = CType(mailList.ReplyToMode, ReplyTo)
        info.PrefixOption = CType(mailList.SubjectPrefixStatus, PrefixOption)
        info.SubjectPrefix = mailList.SubjectPrefix
        info.AttachHeader = mailList.HeaderAnnotationStatus
        info.AttachFooter = mailList.FooterAnnotationStatus
        info.TextHeader = GetMailListHeaderFooter(mailList.ListName, mailList.AccountName, 1)
        info.TextFooter = GetMailListHeaderFooter(mailList.ListName, mailList.AccountName, 2)
        info.HtmlHeader = GetMailListHeaderFooter(mailList.ListName, mailList.AccountName, 3)
        info.HtmlFooter = GetMailListHeaderFooter(mailList.ListName, mailList.AccountName, 4)

        'retrieve maillist members
        Dim listMembers As ArrayList = GetMailListMembers(info.Name)
        info.Members = CType(listMembers.ToArray(GetType(String)), String())
        ResetMaillist(mailList)

        Return info
    End Function

    Private Function GetMailListMembers(ByVal name As String) As ArrayList
        Dim members As ArrayList = New ArrayList
        Dim domainName As String = GetDomainName(name)
        Dim mailListName As String = GetMailboxName(name)

        Dim listMember As New SolidCP.Providers.Mail.MailEnableListMember
        listMember.AccountName = domainName
        listMember.ListName = mailListName
        listMember.Address = ""
        listMember.Host = ""
        listMember.ListMemberType = -1
        listMember.Status = -1

        If listMember.FindFirstListMember() = 1 Then
            Do
                ' add member
                members.Add(GetEmailString(listMember.Address))

                ' reset list member
                listMember.AccountName = domainName
                listMember.ListName = mailListName
                listMember.Address = ""
                listMember.Host = ""
                listMember.ListMemberType = -1
                listMember.Status = -1
            Loop While listMember.FindNextListMember() = 1
        End If

        Return members
    End Function

    Public Overridable Function ListExists(ByVal maillistName As String) As Boolean Implements IMailServer.ListExists
        Dim mailList As New SolidCP.Providers.Mail.MailEnableList
        ResetMaillist(mailList)
        mailList.AccountName = GetDomainName(maillistName)
        mailList.ListName = GetMailboxName(maillistName)

        Return (mailList.GetList() = 1)
    End Function

    Public Overridable Sub CreateList(ByVal info As MailList) Implements IMailServer.CreateList
        Dim domainName As String = GetDomainName(info.Name)
        Dim maillistName As String = GetMailboxName(info.Name)


        ' process parameters
        If info.Members Is Nothing Then
            info.Members = New String() {}
        End If

        Dim mailList As New SolidCP.Providers.Mail.MailEnableList
        ResetMaillist(mailList)

        mailList.AccountName = domainName
        mailList.ListName = maillistName
        mailList.Description = info.Description
        mailList.ListStatus = IIf(info.Enabled, 1, 0)
        mailList.ListType = IIf(info.Moderated, 1, 0)


        mailList.ListAddress = String.Format("[SMTP:{0}]", info.Name + "@" + domainName)
        mailList.ModeratorAddress = String.Format("[SMTP:{0}]", info.ModeratorAddress)
        mailList.Password = info.Password
        mailList.PostingMode = CType(info.PostingMode, Int32)
        mailList.ReplyToMode = CType(info.ReplyToMode, Int32)
        mailList.SubjectPrefixStatus = CType(info.PrefixOption, Int32)
        mailList.SubjectPrefix = info.SubjectPrefix

        If (mailList.AddList() <> 1) Then
            Throw New Exception("Mail list creation failed")
        End If

        ' create address maps for all domains
        Dim destinationAddress As String = String.Format("[LS:{0}/{1}]", domainName, maillistName)
        CreateAddressMapsForAllDomains(domainName, maillistName, destinationAddress)

        'create mail list members
        Dim member As String
        For Each member In info.Members
            Dim listMember As New SolidCP.Providers.Mail.MailEnableListMember
            listMember.AccountName = domainName
            listMember.Address = String.Format("[SMTP:{0}]", member)
            listMember.ListMemberType = 0
            listMember.ListName = maillistName
            listMember.Status = 1

            If (listMember.AddListMember() <> 1) Then
                Throw New Exception("List member creation failed")
            End If
        Next

        'set Header and Footer for mail list
        'SetMailListHeaderFooter(maillistName, domainName, info.Header, info.Footer)

    End Sub

    Public Overridable Sub UpdateList(ByVal info As MailList) Implements IMailServer.UpdateList
        Dim domainName As String = GetDomainName(info.Name)
        Dim maillistName As String = GetMailboxName(info.Name)

        ' process parameters
        If info.Members Is Nothing Then
            info.Members = New String() {}
        End If

        Dim mailList As New SolidCP.Providers.Mail.MailEnableList
        ResetMaillist(mailList)

        mailList.AccountName = domainName
        mailList.ListName = maillistName

        If (mailList.GetList() <> 1) Then
            Throw New Exception("Can't find specified maillistNot ")
        End If


        mailList.EditList( _
         info.Description, _
         domainName, _
         maillistName, _
         IIf(info.Moderated, 1, 0), _
         IIf(info.Enabled, 1, 0), _
         info.AttachHeader, _
         String.Empty, _
         info.AttachFooter, _
         String.Empty, _
         String.Format("[SMTP:{0}]", info.ModeratorAddress), _
         String.Format("[SMTP:{0}]", info.Name), _
         0, _
         String.Empty, _
         0, _
         String.Empty, _
         0, _
         String.Empty, _
         CType(info.PrefixOption, Int32), _
         info.SubjectPrefix, _
         String.Empty, _
         0, _
         String.Empty, _
         0, _
         String.Empty, _
         CType(info.ReplyToMode, Int32), _
         -1, _
         CType(info.PostingMode, Int32), _
         -1, _
         -1, _
         info.Password, _
         -1, _
         String.Empty, _
         -1, _
         -1, _
         -1, _
         -1, _
         -1, _
         -1, _
         -1)

        'delete list members
        Dim listMember As New SolidCP.Providers.Mail.MailEnableListMember
        listMember.AccountName = domainName
        listMember.ListName = maillistName
        listMember.Address = ""
        listMember.ListMemberType = -1
        listMember.Status = -1
        listMember.RemoveListMember()

        'create mail list members
        Dim member As String
        For Each member In info.Members
            listMember = New SolidCP.Providers.Mail.MailEnableListMember
            listMember.AccountName = domainName
            listMember.ListName = maillistName
            listMember.Address = String.Format("[SMTP:{0}]", member)
            listMember.ListMemberType = 0
            listMember.Status = 1

            If (listMember.AddListMember() <> 1) Then
                Throw New Exception("List member creation failed")
            End If
        Next

        'update Header and Footer for mail list
        SetMailListHeaderFooter(maillistName, domainName, info.TextHeader, info.TextFooter, info.HtmlHeader, info.HtmlFooter)

    End Sub
#End Region

#Region "Private Helper methods"
    Public Sub DeleteList(ByVal name As String) Implements IMailServer.DeleteList
        Dim domainName As String = GetDomainName(name)
        Dim maillistName As String = GetMailboxName(name)

        ' remove mailing list
        Dim mailList As New SolidCP.Providers.Mail.MailEnableList
        ResetMaillist(mailList)
        mailList.ListName = maillistName
        mailList.AccountName = domainName
        mailList.RemoveList()

        ' delete list members
        Dim listMember As New SolidCP.Providers.Mail.MailEnableListMember
        listMember.AccountName = domainName
        listMember.ListName = maillistName
        listMember.Address = ""
        listMember.Host = ""
        listMember.ListMemberType = -1
        listMember.Status = -1
        listMember.RemoveListMember()

        ' delete address maps
        Dim map As New SolidCP.Providers.Mail.MailEnableAddressMap
        ResetAddressMap(map)
        map.Account = domainName
        map.DestinationAddress = String.Format("[LS:{0}/{1}]", domainName, maillistName)
        map.RemoveAddressMap()
    End Sub

    Private Sub ResetDomain(ByVal domain As Object)
        domain.AccountName = ""
        domain.DomainName = ""
        domain.DomainRedirectionHosts = ""
        domain.Host = ""
        domain.Status = -1
        domain.DomainRedirectionStatus = -1
    End Sub

    Private Sub ResetAddressMap(ByVal map As Object)
        map.Account = ""
        map.DestinationAddress = ""
        map.Host = ""
        map.SourceAddress = ""
        map.Scope = ""
    End Sub

    Private Sub ResetBlacklist(ByVal blacklist As Object)
        blacklist.Account = ""
        blacklist.TargetDomainName = ""
        blacklist.BannedDomainName = ""
        blacklist.Host = ""
        blacklist.Status = -1
    End Sub

    Private Sub ResetLogin(ByVal login As Object)
        login.Account = ""
        login.UserName = ""
        login.Description = ""
        login.Host = ""
        login.Password = ""
        login.UserName = ""
        login.LastAttempt = -1
        login.LastSuccessfulLogin = -1
        login.LoginAttempts = -1
        login.Status = -1
    End Sub

    Private Sub ResetMailbox(ByVal mailbox As SolidCP.Providers.Mail.MailEnableMailbox)
        mailbox.Postoffice = ""
        mailbox.Host = ""
        mailbox.MailboxName = ""
        mailbox.RedirectAddress = ""
        mailbox.Limit = -1
        mailbox.RedirectStatus = -1
        mailbox.Size = -1
        mailbox.Status = -1
    End Sub

    Private Sub ResetMaillist(ByVal maillist As Object)
        maillist.AccountName = ""
        maillist.Description = ""
        maillist.DigestMailbox = ""
        maillist.FooterAnnotation = ""
        maillist.HeaderAnnotation = ""
        maillist.HelpMessageFile = ""
        maillist.Host = ""
        maillist.ListAddress = ""
        maillist.ListName = ""
        maillist.ModeratorAddress = ""
        maillist.Owner = ""
        maillist.Password = ""
        maillist.RemovalMessageFile = ""
        maillist.SubjectPrefix = ""
        maillist.SubjectSuffix = ""
        maillist.SubscribeMessageFile = ""
        maillist.UnsubscribeMessageFile = ""

        maillist.AuthenticationMode = -1
        maillist.DigestAnnotationMode = -1
        maillist.DigestAttachmentMode = -1
        maillist.DigestMessageSeparationMode = -1
        maillist.DigestMode = -1
        maillist.DigestSchedulingInterval = -1
        maillist.DigestSchedulingMode = -1
        maillist.DigestSchedulingStatus = -1
        maillist.FooterAnnotationStatus = -1
        maillist.FromAddressMode = -1
        maillist.HeaderAnnotationStatus = -1
        maillist.HelpMessageFileStatus = -1
        maillist.ListStatus = -1
        maillist.ListType = -1
        maillist.MaxMessageSize = -1
        maillist.PostingMode = -1
        maillist.RemovalMessageFileStatus = -1
        maillist.ReplyToMode = -1
        maillist.SubjectPrefixStatus = -1
        maillist.SubjectSuffixStatus = -1
        maillist.SubscribeMessageFileStatus = -1
        maillist.SubScriptionMode = -1
        maillist.UnsubscribeMessageFileStatus = -1
    End Sub

    Private Sub ResetGroup(ByVal group As Object)
        group.Postoffice = ""
        group.RecipientAddress = ""
        group.GroupName = ""
        group.GroupStatus = -1
    End Sub

    Private Function GetGroupInfo(ByVal objGroup As Object) As MailGroup
        Dim group As MailGroup = New MailGroup
        group.Name = objGroup.GroupName + "@" + objGroup.Postoffice
        group.Enabled = (objGroup.GroupStatus = 1)

        ' retrieve group members
        Dim members As ArrayList = GetGroupMembers(group.Name)
        group.Members = CType(members.ToArray(GetType(String)), String())

        Return group
    End Function

    Private Function GetGroupMembers(ByVal name As String) As ArrayList
        Dim members As ArrayList = New ArrayList

        Dim domainName As String = GetDomainName(name)
        Dim groupName As String = GetMailboxName(name)

        Dim groupMember As New SolidCP.Providers.Mail.MailEnableGroupMember
        groupMember.Postoffice = domainName
        groupMember.Mailbox = groupName
        groupMember.Address = ""

        If groupMember.FindFirstGroupMember() = 1 Then
            Do
                ' add member
                members.Add(GetEmailString(groupMember.Address))

                ' reset group member
                groupMember.Postoffice = domainName
                groupMember.Mailbox = groupName
                groupMember.Address = ""
            Loop While groupMember.FindNextGroupMember() = 1
        End If

        Return members
    End Function

    Private Sub CreateAddressMapsForAllDomains(ByVal domainName As String, ByVal aliasName As String, ByVal targetAddress As String)
        Dim oDomain As New SolidCP.Providers.Mail.MailEnableDomain
        Dim oAddressMap As New SolidCP.Providers.Mail.MailEnableAddressMap
        oDomain.AccountName = domainName
        oDomain.DomainName = ""
        oDomain.Status = -1
        oDomain.DomainRedirectionHosts = ""
        oDomain.DomainRedirectionStatus = -1
        If oDomain.FindFirstDomain = 1 Then
            Do
                Dim MappedAddress As String = "[SMTP:" & aliasName & "@" & oDomain.DomainName & "]"
                oAddressMap.Account = domainName
                oAddressMap.DestinationAddress = targetAddress
                oAddressMap.SourceAddress = MappedAddress
                oAddressMap.Scope = 0
                If oAddressMap.AddAddressMap = 1 Then
                    '
                    ' Address Map was added too
                    '
                End If
                oDomain.AccountName = domainName
                oDomain.DomainName = ""
                oDomain.Status = -1
                oDomain.DomainRedirectionHosts = ""
                oDomain.DomainRedirectionStatus = -1
            Loop While (oDomain.FindNextDomain = 1)
        End If
        oDomain = Nothing
        oAddressMap = Nothing
    End Sub


    Private Function GetEmailString(ByVal smtpEmailString As String) As String
        If (smtpEmailString = "") Then
            Return ""
        End If
        Dim ret As String = smtpEmailString.Remove(0, "[SMTP:".Length)
        ret = ret.Remove(ret.Length - 1, 1)
        Return ret
    End Function

    Private Function GetMailboxName(ByVal email As String) As String
        If (email.IndexOf("@") = -1) Then
            Return email
        End If
        Return email.Substring(0, email.IndexOf("@"))
    End Function

    Private Function GetDomainName(ByVal email As String) As String
        Return email.Substring(email.IndexOf("@") + 1)
    End Function
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

    Public Overrides Function GetServiceItemsDiskSpace(ByVal items() As ServiceProviderItem) As ServiceProviderItemDiskSpace()
        Dim itemsDiskspace As List(Of ServiceProviderItemDiskSpace) = New List(Of ServiceProviderItemDiskSpace)

        ' update items with diskspace
        Dim item As ServiceProviderItem
        For Each item In items
            If TypeOf item Is MailAccount Then
                Try
                    ' get mailbox size
                    Dim name As String = item.Name

                    ' try to get MailEnable postoffices path
                    Dim poPath As String = GetPostofficesPath()
                    If poPath Is Nothing Then
                        Continue For
                    End If
                    Dim mailboxName As String = name.Substring(0, name.IndexOf("@"))
                    Dim domainName As String = name.Substring((name.IndexOf("@") + 1))

                    Dim mailboxPath As String = [String].Format("{0}\{1}\Mailroot\{2}", poPath, domainName, mailboxName)

                    ' calculate disk space
                    Dim diskspace As New ServiceProviderItemDiskSpace()
                    diskspace.ItemId = item.Id
                    diskspace.DiskSpace = FileUtils.CalculateFolderSize(mailboxPath)
                    itemsDiskspace.Add(diskspace)
                Catch ex As Exception
                    Log.WriteError("Error calculating disk space for mail account: " + item.Name, ex)
                End Try
            End If
        Next item

        Return itemsDiskspace.ToArray()
    End Function

    Public Overrides Function GetServiceItemsBandwidth(ByVal items() As ServiceProviderItem, ByVal since As Date) As ServiceProviderItemBandwidth()
        Dim itemsBandwidth(items.Length) As ServiceProviderItemBandwidth

        Dim logsPath As String = GetLoggingPath()
        If logsPath Is Nothing Then
            Return Nothing
        End If
        ' calculate bandwidth for mail enable
        ' parse mail logs
        Dim parser As New LogParser("Mail", "mailenable_pop", Path.Combine(logsPath, "pop"), "account")
        parser.ParseLogs(Of LogReader)()

        parser = New LogParser("Mail", "mailenable_smtp", Path.Combine(logsPath, "smtp"), "account")
        parser.ParseLogs(Of MELogReader)()


        ' update items with diskspace
        Dim i As Integer
        For i = 0 To items.Length - 1
            Dim item As ServiceProviderItem = items(i)

            ' create new bandwidth object
            itemsBandwidth(i) = New ServiceProviderItemBandwidth()
            itemsBandwidth(i).ItemId = item.Id
            itemsBandwidth(i).Days = New DailyStatistics(0) {}

            If TypeOf item Is MailDomain Then
                Try
                    ' get daily statistics
                    itemsBandwidth(i).Days = parser.GetDailyStatistics(since, New String() {item.Name})
                Catch ex As Exception
                    Log.WriteError("Error calculating bandwidth for mail domain: " + item.Name, ex)
                End Try
            End If
        Next i
        Return itemsBandwidth
    End Function

    Private Sub DeleteMailBoxDirectory(ByVal name As String)
        ' try to get MailEnable postoffices path
        Dim poPath As String = GetPostofficesPath()
        If poPath Is Nothing Then
            Return
        End If
        Dim mailboxName As String = name.Substring(0, name.IndexOf("@"))
        Dim domainName As String = name.Substring((name.IndexOf("@") + 1))

        Dim mailboxPath As String = [String].Format("{0}\{1}\Mailroot\{2}", poPath, domainName, mailboxName)

        Try
            FileUtils.DeleteFile(mailboxPath)
        Catch
        End Try
    End Sub

    Private Sub DeletePostofficeDirectory(ByVal domainName As String)
        ' try to get MailEnable postoffices path
        Dim poPath As String = GetPostofficesPath()
        If poPath Is Nothing Then
            Return
        End If
        Dim postofficePath As String = [String].Format("{0}\{1}", poPath, domainName)

        Try
            FileUtils.DeleteFile(postofficePath)
        Catch
        End Try
    End Sub


    Private Sub ReadMailboxAutoresponderFile(ByVal mailbox As MailAccount)
        ' try to get MailEnable postoffices path
        Dim poPath As String = GetPostofficesPath()
        If poPath Is Nothing Then
            Return
        End If
        Dim mailboxName As String = mailbox.Name.Substring(0, mailbox.Name.IndexOf("@"))
        Dim domainName As String = mailbox.Name.Substring((mailbox.Name.IndexOf("@") + 1))

        ' build autoresponder path
        Dim responderPath As String = [String].Format("{0}\{1}\Mailroot\{2}\AUTORESPOND", poPath, domainName, mailboxName)

        ' read responder configuration file
        Dim respFile As String = responderPath + ".CF_"
        If Not File.Exists(respFile) Then
            Return
        End If
        Dim reader As New StreamReader(respFile)
        Dim content As String = reader.ReadToEnd()
        reader.Close()

        ' parse configuration file
        Dim subjectToken As String = "Subject: "
        Dim replyToken As String = "Reply-To: "
        Dim idx As Integer = content.IndexOf(subjectToken, 0)
        If idx <> -1 Then
            ' extract subject line
            mailbox.ResponderSubject = content.Substring(idx + subjectToken.Length, content.IndexOf(ControlChars.Lf, idx) - (idx + subjectToken.Length + 1))
        End If

        idx = content.IndexOf(replyToken, 0)
        If idx <> -1 Then
            ' extract reply-to line
            mailbox.ReplyTo = content.Substring(idx + replyToken.Length + 1, content.IndexOf(ControlChars.Lf, idx) - (idx + replyToken.Length + 3))
        End If
    End Sub


    Private Sub WriteMailboxAutoresponderFile(ByVal mailbox As MailAccount)
        ' try to get MailEnable postoffices path
        Dim poPath As String = GetPostofficesPath()
        If poPath Is Nothing Then
            Return
        End If
        Dim mailboxName As String = mailbox.Name.Substring(0, mailbox.Name.IndexOf("@"))
        Dim domainName As String = mailbox.Name.Substring((mailbox.Name.IndexOf("@") + 1))

        ' build autoresponder path
        Dim responderPath As String = [String].Format("{0}\{1}\Mailroot\{2}\AUTORESPOND", poPath, domainName, mailboxName)

        Dim responderContent As String = [String].Format("From: ""{0}"" <{1}>" & vbCrLf & _
"Subject: {2}" & vbCrLf & _
"Reply-To: <{3}>" & vbCrLf & _
"Return-Path: <{4}>" & vbCrLf & vbCrLf & _
"{5}", mailbox.Name, mailbox.Name, mailbox.ResponderSubject, mailbox.ReplyTo, mailbox.ReplyTo, mailbox.ResponderMessage)

        ' write file
        Dim writer As New StreamWriter(responderPath + ".CF_")
        writer.Write(responderContent)
        writer.Close()

        If mailbox.ResponderEnabled Then
            ' write actual responder file
            writer = New StreamWriter(responderPath + ".CFG")
            writer.Write(responderContent)
            writer.Close()
        End If
    End Sub

    Private Sub SetMailBoxReplyToAddress(ByVal mailbox As String, ByVal replyToAddress As String)

        Dim oMEAOSO As New SolidCP.Providers.Mail.MailEnableOption

        With oMEAOSO
            .Scope = 2
            .Query = GetDomainName(mailbox) & "/" & GetMailboxName(mailbox)
            .ValueName = "ReplyAddress"
            .Value = replyToAddress
            .SetOption()
        End With

    End Sub

    Private Function GetMailBoxReplyToAddress(ByVal mailbox As String)

        Dim oMEAOSO As New SolidCP.Providers.Mail.MailEnableOption

        With oMEAOSO
            .Scope = 2
            .Query = GetDomainName(mailbox) & "/" & GetMailboxName(mailbox)
            .ValueName = "ReplyAddress"
            .GetOption()
            Return .Value
        End With

    End Function

    Function GetMailEnableRegistryItem(item As String) As String

        Dim key As RegistryKey

        If IntPtr.Size > 4 Then
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Mail Enable\Mail Enable")
        Else
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Mail Enable\Mail Enable")
        End If

        Return CStr(key.GetValue(item))

    End Function


    Shared Function GetPostofficesPath() As String

        Dim oLocal As New MailEnable
        Return oLocal.GetMailEnableRegistryItem("Mail Root")

    End Function


    Private Function GetInstallPath() As String

        Return GetMailEnableRegistryItem("Program Directory")

    End Function


    Private Sub SetMailListHeaderFooter(ByVal listname As String, ByVal postofficeName As String, ByVal headerText As String, ByVal footerText As String, ByVal headerHtml As String, ByVal footerHtml As String)

        Dim headerTextPath As String = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-HEADER.txt", listname))
        Dim footerTextPath As String = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-FOOTER.txt", listname))
        Dim headerHtmlPath As String = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-HEADER.htm", listname))
        Dim footerHtmlPath As String = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-FOOTER.htm", listname))

        'write Header/Footer for plain text messages
        If File.Exists(headerTextPath) And [String].IsNullOrEmpty(headerText) Then
            File.Delete(headerTextPath)
        ElseIf Not File.Exists(headerTextPath) Then
            Dim oWrite As StreamWriter = File.CreateText(headerTextPath)
            oWrite.Write(headerText)
            oWrite.Close()
        Else
            Dim textFileStream As New IO.FileStream(headerTextPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
            Dim myFileWriter As New IO.StreamWriter(textFileStream)
            myFileWriter.Write(headerText)
            myFileWriter.Close()
        End If

        If File.Exists(footerTextPath) And [String].IsNullOrEmpty(footerText) Then
            File.Delete(footerTextPath)

        ElseIf Not File.Exists(headerTextPath) Then
            Dim oWrite As StreamWriter = File.CreateText(footerTextPath)
            oWrite.Write(footerText)
            oWrite.Close()
        Else
            Dim textFileStream As New IO.FileStream(footerTextPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
            Dim myFileWriter As New IO.StreamWriter(textFileStream)
            myFileWriter.Write(footerText)
            myFileWriter.Close()
        End If

        'write Header/Footer for HTML messages
        If File.Exists(headerHtmlPath) And [String].IsNullOrEmpty(headerHtml) Then
            File.Delete(headerHtmlPath)
        ElseIf Not File.Exists(headerHtmlPath) Then
            Dim oWrite As StreamWriter = File.CreateText(headerHtmlPath)
            oWrite.Write(headerHtml)
            oWrite.Close()
        Else
            Dim textFileStream As New IO.FileStream(headerHtmlPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
            Dim myFileWriter As New IO.StreamWriter(textFileStream)
            myFileWriter.Write(headerHtml)
            myFileWriter.Close()
        End If

        If File.Exists(footerHtmlPath) And [String].IsNullOrEmpty(footerHtml) Then
            File.Delete(footerHtmlPath)

        ElseIf Not File.Exists(headerHtmlPath) Then
            Dim oWrite As StreamWriter = File.CreateText(footerHtmlPath)
            oWrite.Write(footerHtml)
            oWrite.Close()
        Else
            Dim textFileStream As New IO.FileStream(footerHtmlPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
            Dim myFileWriter As New IO.StreamWriter(textFileStream)
            myFileWriter.Write(footerHtml)
            myFileWriter.Close()
        End If

    End Sub

    Private Function GetMailListHeaderFooter(ByVal listname As String, ByVal postofficeName As String, ByVal type As Integer) As String
        'type 1 - headerText
        'type 2 - footerText
        'type 3 - headerHtml
        'type 4 - footerHtml
        Dim filepath As String = String.Empty
        Dim data As String = String.Empty

        If type.Equals(1) Then
            filepath = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-HEADER.txt", listname))
        ElseIf type.Equals(2) Then
            filepath = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-FOOTER.txt", listname))
        End If

        If type.Equals(3) Then
            filepath = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-HEADER.htm", listname))
        ElseIf type.Equals(4) Then
            filepath = Path.Combine(GetAnnotationPath(postofficeName), String.Format("{0}-FOOTER.htm", listname))
        End If

        If File.Exists(filepath) Then
            Dim textFileStream As New IO.FileStream(filepath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
            Dim myFileReader As New IO.StreamReader(textFileStream)
            data = myFileReader.ReadToEnd()
            myFileReader.Close()
        Else
            Return data
        End If
        Return data
    End Function

    Private Function GetAnnotationPath(ByVal postOfficeName As String) As String

        'the annotation paths are in the configuration directory

        Dim programPath As String = ""
        Dim key As RegistryKey

        If IntPtr.Size > 4 Then
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Mail Enable\Mail Enable")
        Else
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Mail Enable\Mail Enable")
        End If

        If (key Is Nothing) Then          
            Return String.Empty
        Else
            programPath = CStr(key.GetValue("Configuration Directory"))
            Return Path.Combine(programPath, String.Format("Postoffices\{0}\ANNOTATIONS", postOfficeName))
        End If

    End Function

    Private Function NonCString(ByVal InString As String) As String
        Dim NTPos As Integer
        NTPos = InStr(1, InString, Chr(0), CompareMethod.Binary)
        If NTPos > 0 Then
            NonCString = Left(InString, NTPos - 1)
        Else
            NonCString = InString
        End If
    End Function

    Private Function CString(ByVal InString As String) As String
        CString = InString & Chr(0)
    End Function

    Private Function GetLoggingPath() As String

        Dim programPath As String = ""
        Dim key As RegistryKey

        If IntPtr.Size > 4 Then
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Mail Enable\Mail Enable")
        Else
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Mail Enable\Mail Enable")
        End If

        If (key Is Nothing) Then
            Return String.Empty
        Else
            Return CStr(key.GetValue("W3C Logging Directory"))
        End If

    End Function

#End Region

    Public Overrides Function IsInstalled() As Boolean

        Dim version As String = ""
        Dim key As RegistryKey

        If IntPtr.Size > 4 Then
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Mail Enable\Mail Enable")
        Else
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Mail Enable\Mail Enable")
        End If

        version = CStr(key.GetValue("Enterprise Version"))
        If (version Is Nothing) Then
            version = CStr(key.GetValue("Version"))
            If (version Is Nothing Or version.Equals("0")) Then
                version = CStr(key.GetValue("Professional Version"))
            End If
        End If

        If [String].IsNullOrEmpty(version) = False Then
            'all versions of MailEnable will be compatible with this, so we are just checking to see if there is a version number
            'future versions aim to retain compatibility
            Return True
        Else
            Return False
        End If

    End Function

End Class

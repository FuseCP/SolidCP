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

Imports SolidCP.Providers
Imports SolidCP.Providers.Mail
Imports SolidCP.Providers.Utils
Imports SolidCP.Server.Utils
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

#Region "Internal classes"
Friend Class Service
    Public ComObject As Object
    Public Succeed As Boolean

    Private Sub New()

    End Sub

    Friend Shared Function LoadServiceFactory(ByVal serviceProgId As String) As Service
        Dim service As New Service()

        Try
            Dim comObject As Object = CreateObject(serviceProgId)

            If Not comObject Is Nothing Then
                service.ComObject = comObject
                service.Succeed = True
            Else
                service.ComObject = Nothing
                service.Succeed = False
                Log.WriteInfo(String.Format("Failed to load {0} ActiveX object.", serviceProgId))
            End If

        Catch ex As Exception
            service.ComObject = Nothing
            service.Succeed = False

            Log.WriteError(String.Format("Couldn't create {0} ActiveX object.", serviceProgId), ex)
        End Try

        Return service
    End Function
End Class
#End Region


Public Class ArgoMail
    Inherits HostingServiceProviderBase
    Implements IMailServer

#Region "Prog ID Constants"

    Public Const ADDRESS_ITEM_PROG_ID As String = "MailServerX.AddressItem"
    Public Const ADDRESS_LIST_PROG_ID As String = "MailServerX.AddressList"
    Public Const ADMIN_PROG_ID As String = "MailServerX.Admin"
    Public Const DISTRIB_LIST_PROG_ID As String = "MailServerX.DistribList"
    Public Const DISTRIB_LISTS_PROG_ID As String = "MailServerX.DistribLists"
    Public Const LINES_PROG_ID As String = "MailServerX.Lines"
    Public Const LOCAL_DOMAIN_PROG_ID As String = "MailServerX.LocalDomain"
    Public Const LOCAL_DOMAINS_PROG_ID As String = "MailServerX.LocalDomains"
    Public Const MAIL_BAG_PROG_ID As String = "MailServerX.MailBag"
    Public Const MAIL_BAGS_PROG_ID As String = "MailServerX.MailBags"
    Public Const MAIL_BOX_PROG_ID As String = "MailServerX.MailBox"
    Public Const MAIL_MESSAGE_PROG_ID As String = "MailServerX.MailMessage"
    Public Const SEND_MAIL_PROG_ID As String = "MailServerX.SendMail"
    Public Const SEND_MAIL1_PROG_ID As String = "MailServerX.SendMail1"
    Public Const USER_PROG_ID As String = "MailServerX.User"
    Public Const USERS_PROG_ID As String = "MailServerX.Users"
    Public Const WHITE_LIST_PROG_ID As String = "MailServerX.WhiteList"

#End Region

#Region "Helper routines"
    
    Friend Shared Function LoadUsersService() As Service
        Return Service.LoadServiceFactory(USERS_PROG_ID)
    End Function

    Friend Shared Function LoadLocalDomainsService() As Service
        Return Service.LoadServiceFactory(LOCAL_DOMAINS_PROG_ID)
    End Function

    Friend Shared Function LoadDistribListsService() As Service
        Return Service.LoadServiceFactory(DISTRIB_LISTS_PROG_ID)
    End Function

    Friend Shared Function CreateLocalDomainObject() As Object
        Return Service.LoadServiceFactory(LOCAL_DOMAIN_PROG_ID).ComObject
    End Function

    Friend Shared Function CreateUserObject() As Object
        Return Service.LoadServiceFactory(USER_PROG_ID).ComObject
    End Function

    Friend Shared Function CreateLinesObject() As Object
        Return Service.LoadServiceFactory(LINES_PROG_ID).ComObject
    End Function

    Friend Shared Function CreateDistribListObject() As Object
        Return Service.LoadServiceFactory(DISTRIB_LIST_PROG_ID).ComObject
    End Function

#End Region

#Region "Helper routines"

    Protected Overridable Function MailboxExists(mailboxName As String) As Boolean
        Dim service As Service = LoadUsersService()
        Dim exists As Boolean = True

        Try
            If service.Succeed Then
                exists = service.ComObject.UserExists(mailboxName)
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain mailbox info.", ex)
        End Try

        Return exists
    End Function 'MailboxExists

    Protected Overridable Sub CreateMailbox(mailboxName As String, mailBoxDesc As String)
        Dim service As Service = LoadUsersService()

        Try
            If service.Succeed Then
                Dim user As Object = CreateUserObject()

                user.UserName = mailboxName
                user.RealName = mailBoxDesc
                user.Password = ""
                user.Active = True
                user.AutoResponderEnabled = False
                user.AutoResponderSubject = ""
                user.ForwardAddress = ""
                user.MailboxSize = 0
                user.KeepCopies = False

                service.ComObject.Add(user)
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't create mailbox item.", ex)
        End Try
    End Sub 'CreateMailbox


    Protected Overridable Function ReadMailBox(ByRef user As Object) As MailAccount
        Dim objMailboxItem As New MailAccount()

        Dim names() As String

        If Not String.IsNullOrEmpty(user.RealName) Then
            names = user.RealName.Split(" "c)

            If names.Length > 2 Then
                objMailboxItem.FirstName = names(0) + names(1)
                objMailboxItem.LastName = names(2)
            Else
                If names.Length > 0 Then
                    objMailboxItem.LastName = names(1)
                End If
            End If
        End If

        objMailboxItem.Name = user.UserName
        objMailboxItem.Password = user.Password
        objMailboxItem.Enabled = user.Active
        objMailboxItem.ResponderEnabled = user.AutoResponderEnabled
        objMailboxItem.ResponderSubject = user.AutoResponderSubject

        If user.AutoResponderData.Count > 0 Then
            objMailboxItem.ResponderMessage = user.AutoResponderData.Items(0)
        Else
            objMailboxItem.ResponderMessage = ""
        End If

        If user.ForwardAddress <> "" Then
            Dim addresses As New List(Of String)
            Dim forwardings() As String = CStr(user.ForwardAddress).Split(",".ToCharArray())

            For Each forwarding As String In forwardings
                addresses.Add(forwarding)
            Next forwarding

            objMailboxItem.ForwardingAddresses = addresses.ToArray()
        End If

        objMailboxItem.ReplyTo = user.ReturnAddress
        objMailboxItem.MaxMailboxSize = user.MailboxSize

        Return objMailboxItem
    End Function 'ReadMailBox

    Protected Overridable Function ConvertToMailGroup(ByRef objGroup As Object) As MailGroup
        Dim group As MailGroup = Nothing

        If objGroup.Members.Count > 0 Then
            group = New MailGroup()

            group.Name = objGroup.Name
            Dim members As New List(Of String)

            For i As Integer = 0 To objGroup.Members.Count - 1
                members.Add(objGroup.Members.Items(i))
            Next i

            group.Members = members.ToArray()
        End If

        Return group
    End Function

    Private Function ConvertToMailList(ByRef objMailList As ArgoMailListItem) As MailList
        Dim mailList As MailList = Nothing

        If Not objMailList Is Nothing Then
            mailList = New MailList()

            mailList.Item("MailListAccount") = objMailList.Account
            mailList.Description = objMailList.Desription
            mailList.Enabled = True
            mailList.Moderated = objMailList.RequireMemberShip
            mailList.Name = objMailList.Name

            If objMailList.RepliesGoToSender = True Then
                mailList.ReplyToMode = ReplyTo.RepliesToSender
            Else
                mailList.ReplyToMode = ReplyTo.RepliesToList
            End If

            If objMailList.ListISClosed Then
                mailList.PostingMode = PostingMode.MembersCanPost
            Else
                mailList.PostingMode = PostingMode.AnyoneCanPost
            End If

            If Not objMailList.Members Is Nothing Then
                Dim members As New List(Of String)
                For i As Integer = 0 To objMailList.Members.Length - 1
                    If Not objMailList.Members(i) Is Nothing Then
                        members.Add(objMailList.Members(i))
                    End If
                Next i
                mailList.Members = members.ToArray()
            End If
        End If

        Return mailList
    End Function

    Protected Function GetEmailName(ByVal mailbox As String)
        Return mailbox.Substring(0, mailbox.IndexOf("@"))
    End Function

    Protected Function GetDomainName(ByVal mailbox As String)
        Return mailbox.Substring(mailbox.IndexOf("@") + 1)
    End Function
#End Region


#Region "IMailServer members"
    Public Function AccountExists(ByVal mailboxName As String) As Boolean Implements SolidCP.Providers.Mail.IMailServer.AccountExists
        Dim service As Service = LoadUsersService()
        Dim exists As Boolean = False

        If service.Succeed Then
            exists = service.ComObject.UserExists(mailboxName)
        End If

        Return exists
    End Function

    Public Sub AddDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements SolidCP.Providers.Mail.IMailServer.AddDomainAlias
        Dim service As Service = LoadLocalDomainsService()

        Try
            If service.Succeed Then
                Dim domainIndex As Integer = service.ComObject.IndexOf(domainName)

                If domainIndex >= 0 Then
                    Dim domain As Object = service.ComObject.Items(domainIndex)
                    
                    Dim aliases As Object = domain.Aliases
                    aliases.Add(aliasName)
                    domain.Aliases = aliases

                    service.ComObject.Items(domainIndex) = domain
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't add domain alias.", ex)
        End Try
    End Sub

    Public Sub CreateAccount(ByVal mailbox As SolidCP.Providers.Mail.MailAccount) Implements SolidCP.Providers.Mail.IMailServer.CreateAccount
        Dim service As Service = LoadUsersService()

        Try
            If service.Succeed Then
                Dim user As Object = CreateUserObject()
                Dim respMsgLines As Object = CreateLinesObject()

                Dim iSize As Integer = mailbox.MaxMailboxSize

                If Not String.IsNullOrEmpty(mailbox.Item("MaxMailboxSizeInMB")) Then
                    iSize = Convert.ToInt32(mailbox.Item("MaxMailboxSizeInMB"))
                End If 

                user.UserName = mailbox.Name
                user.RealName = string.Concat(mailbox.FirstName, " ", mailbox.LastName)
                user.Password = mailbox.Password
                user.Active = mailbox.Enabled

                If mailbox.ResponderEnabled Then
                    user.AutoResponderEnabled = True
                    user.AutoResponderSubject = mailbox.ResponderSubject
                    respMsgLines.Add(mailbox.ResponderMessage)
                    user.AutoResponderData = respMsgLines
                End If

                If mailbox.ForwardingAddresses Is Nothing Then
                    user.ForwardAddress = String.Empty
                ElseIf mailbox.ForwardingAddresses.Length > 0 Then
                    user.ForwardAddress = String.Join(",", mailbox.ForwardingAddresses)
                Else
                    user.ForwardAddress = String.Empty
                End If

                user.ReturnAddress = mailbox.ReplyTo
                user.MailboxSize = iSize
                user.KeepCopies = Not mailbox.DeleteOnForward

                service.ComObject.Add(user)
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't create mailbox account.", ex)
        End Try
    End Sub

    Public Sub CreateDomain(ByVal domain As SolidCP.Providers.Mail.MailDomain) Implements SolidCP.Providers.Mail.IMailServer.CreateDomain
        Dim service As Service = LoadLocalDomainsService()

        Try
            If service.Succeed Then
                Dim localDomain As Object = CreateLocalDomainObject()

                localDomain.DiskQuota = domain.MaxDomainSizeInMB
                localDomain.Name = domain.Name
                localDomain.MaxAccounts = domain.MaxDomainUsers
                localDomain.MaxDistribLists = domain.MaxLists
                localDomain.AllowDistribLists = True

                service.ComObject.Add(localDomain)

				' Create postmaster account
				Dim postmaster As New MailAccount()
				postmaster.DeleteOnForward = True
				postmaster.Name = String.Concat("postmaster@", domain.Name)
				postmaster.Enabled = False

				CreateAccount(postmaster)
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't create domain.", ex)
        End Try
    End Sub

    Public Sub CreateGroup(ByVal group As SolidCP.Providers.Mail.MailGroup) Implements SolidCP.Providers.Mail.IMailServer.CreateGroup
        Dim service As Service = LoadDistribListsService()

        Try
            If service.Succeed Then
                Dim distList As Object = CreateDistribListObject()
                Dim listMembers As Object = CreateLinesObject()

                distList.Name = group.Name
                
                Dim member As String
                For Each member In group.Members
                    listMembers.Add(member)
                Next member

                distList.Members = listMembers
                service.ComObject.Add(distList)
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't create mail group.", ex)
        End Try
    End Sub

    Public Sub CreateList(ByVal maillist As SolidCP.Providers.Mail.MailList) Implements SolidCP.Providers.Mail.IMailServer.CreateList
        Try
            Dim listAccount As String = maillist.Name

            If String.IsNullOrEmpty(listAccount) Then
                Throw New Exception("Please provide mail list account.")
            End If

            If Not MailboxExists(listAccount) Then
                CreateMailbox(listAccount, "MailList Account")
            End If

            Dim lists As New ArgoMailLists()
            Dim newItem As New ArgoMailListItem()

            newItem.Name = GetEmailName(maillist.Name)
            newItem.Account = listAccount

            If Not String.IsNullOrEmpty(maillist.Description) Then
                newItem.Desription = maillist.Description.Replace(ControlChars.Cr + ControlChars.Lf, ControlChars.Lf)
            End If

            newItem.ID = "ML00"

            If maillist.PostingMode = PostingMode.MembersCanPost Then
                newItem.ListISClosed = True
            Else
                newItem.ListISClosed = False
            End If

            If Not maillist.Members Is Nothing Then
                newItem.Count = maillist.Members.Length
                Dim members As New List(Of String)

                For i As Integer = 0 To maillist.Members.Length - 1
                    members.Add(maillist.Members(i))
                Next i

                newItem.Members = members.ToArray()
            End If

            'If maillist.Moderated Then
            '   newItem..Name = maillist.ModeratorAddress
            'End If

            If maillist.ReplyToMode = ReplyTo.RepliesToSender Then
                newItem.RepliesGoToSender = True
            Else
                newItem.RepliesGoToSender = False
            End If

            newItem.RequireMemberShip = (maillist.PostingMode = PostingMode.MembersCanPost)

            lists.Add(newItem)
        Catch ex As Exception
            Log.WriteError("Couldn't create mail list.", ex)
        End Try
    End Sub

    Public Sub DeleteAccount(ByVal mailboxName As String) Implements SolidCP.Providers.Mail.IMailServer.DeleteAccount
        Dim service As Service = LoadUsersService()

        Try
            If service.Succeed Then
                If service.ComObject.UserExists(mailboxName) Then
                    service.ComObject.Delete(mailboxName)
                End If 
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't delete a mailbox.", ex)
        End Try
    End Sub

    Public Function MailAliasExists(ByVal mailAliasName As String) As Boolean Implements IMailServer.MailAliasExists
        Throw New System.NotImplementedException()
    End Function

    Public Function GetMailAliases(ByVal domainName As String) As MailAlias() Implements IMailServer.GetMailAliases
        Throw New System.NotImplementedException()
    End Function

    Public Function GetMailAlias(ByVal mailAliasName As String) As MailAlias Implements IMailServer.GetMailAlias
        Throw New System.NotImplementedException()
    End Function

    Public Sub CreateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.CreateMailAlias
        Throw New System.NotImplementedException()
    End Sub

    Public Sub UpdateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.UpdateMailAlias
        Throw New System.NotImplementedException()
    End Sub

    Public Sub DeleteMailAlias(ByVal mailAliasName As String) Implements IMailServer.DeleteMailAlias
        Throw New System.NotImplementedException()
    End Sub

    Public Sub DeleteDomain(ByVal domainName As String) Implements SolidCP.Providers.Mail.IMailServer.DeleteDomain
        Dim service As Service = LoadLocalDomainsService()

        Try
            If service.Succeed Then
                service.ComObject.Delete(domainName)
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't delete a domain.", ex)
        End Try
    End Sub

    Public Sub DeleteDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements SolidCP.Providers.Mail.IMailServer.DeleteDomainAlias
        Dim service As Service = LoadLocalDomainsService()

        Try
            If service.Succeed Then
                Dim localDomain As Object = CreateLocalDomainObject()
                Dim domainIndex As Integer = service.ComObject.IndexOf(domainName)

                If domainIndex >= 0 Then
                    Dim deleteIndex As Integer = -1
                    localDomain = service.ComObject.Items(domainIndex)

                    If localDomain.Aliases.Count > 0 Then
                        Dim index As Integer
                        For index = 0 To localDomain.Aliases.Count - 1
                            If localDomain.Aliases.Items(index) = aliasName Then
                                deleteIndex = index
                                Exit For
                            End If
                        Next index

                        If deleteIndex >= 0 Then
                            Dim aliases As Object = CreateLinesObject()

                            aliases = localDomain.Aliases
                            aliases.Delete(deleteIndex)
                            localDomain.Aliases = aliases

                            service.ComObject.Items(domainIndex) = localDomain
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't delete a domain alias.", ex)
        End Try
    End Sub

    Public Sub DeleteGroup(ByVal groupName As String) Implements SolidCP.Providers.Mail.IMailServer.DeleteGroup
        Dim service As Service = LoadDistribListsService()

        Try
            If service.Succeed Then
                If service.ComObject.IndexOf(groupName) >= 0 Then
                    service.ComObject.Delete(groupName)
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't delete a mail group.", ex)
        End Try
    End Sub

    Public Sub DeleteList(ByVal maillistName As String) Implements SolidCP.Providers.Mail.IMailServer.DeleteList
        Dim userService As Service = LoadUsersService()

        Try
            Dim listService As New ArgoMailLists()
            Dim mailList As ArgoMailListItem = listService.GetItem(maillistName)

            If Not mailList Is Nothing And userService.Succeed Then
                ' remove list account first
                If userService.ComObject.UserExists(mailList.Account) Then
                    userService.ComObject.Delete(mailList.Account)
                End If

                ' remove mail list physically
                listService.Delete(maillistName)
            End If

        Catch ex As Exception
            Log.WriteError("Couldn't delete a mail list.", ex)
        End Try
    End Sub

    Public Function DomainAliasExists(ByVal domainName As String, ByVal aliasName As String) As Boolean Implements SolidCP.Providers.Mail.IMailServer.DomainAliasExists
        Dim service As Service = LoadLocalDomainsService()
        Dim exists As Boolean = False

        Try
            If service.Succeed Then
                Dim localDomain As Object = CreateLocalDomainObject()
                Dim domainIndex As Integer = service.ComObject.IndexOf(domainName)

                If domainIndex >= 0 Then
                    localDomain = service.ComObject.Items(domainIndex)
                    If localDomain.Aliases.Count > 0 Then
                        Dim index As Integer
                        For index = 0 To localDomain.Aliases.Count - 1
                            If localDomain.Aliases.Items(index) = aliasName Then
                                exists = True
                                Exit For
                            End If
                        Next index
                    End If
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't check whether the domain alias exists.", ex)
        End Try

        Return exists
    End Function

    Public Function DomainExists(ByVal domainName As String) As Boolean Implements SolidCP.Providers.Mail.IMailServer.DomainExists
        Dim service As Service = LoadLocalDomainsService()
        Dim exists As Boolean = False

        Try
            If service.Succeed Then
                Dim domainIndex As Integer = service.ComObject.IndexOf(domainName)
                If domainIndex >= 0 Then
                    exists = True
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain domain information.", ex)
        End Try

        Return exists
    End Function

    Public Function GetAccount(ByVal mailboxName As String) As SolidCP.Providers.Mail.MailAccount Implements SolidCP.Providers.Mail.IMailServer.GetAccount
        Dim service As Service = LoadUsersService()
        Dim objMailboxItem As MailAccount = Nothing

        Try
            If service.Succeed Then
                Dim user As Object = Nothing

                If service.ComObject.UserExists(mailboxName) Then
                    user = service.ComObject.GetUserByName(mailboxName)

                    objMailboxItem = ReadMailBox(user)
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't get mail account.", ex)
        End Try

        Return objMailboxItem
    End Function

    Public Function GetAccounts(ByVal domainName As String) As SolidCP.Providers.Mail.MailAccount() Implements SolidCP.Providers.Mail.IMailServer.GetAccounts
        Dim domainService As Service = LoadLocalDomainsService()
        Dim userService As Service = LoadUsersService()
        Dim accounts As New List(Of MailAccount)

        Try
            If domainService.Succeed Then
                Dim domainIndex As Integer = domainService.ComObject.IndexOf(domainName)

                If domainIndex >= 0 Then
                    If userService.Succeed Then
                        Dim users As New List(Of Object)

                        For i As Integer = 0 To userService.ComObject.Count - 1
                            Dim user As Object = userService.ComObject.Items(i)
                            If user.UserName.IndexOf(domainName) >= 0 Then
                                users.Add(user)
                            End If
                        Next i

                        For Each user As Object In users
                            accounts.Add(ReadMailBox(user))
                        Next user
                    End If
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't read dommain mail accounts.", ex)
        End Try

        Return accounts.ToArray()
	End Function

	Public Overridable Function GetDomains() As String() Implements IMailServer.GetDomains
		Dim domainsService As Service = LoadLocalDomainsService()

		If domainsService.Succeed Then
			Dim domains As New List(Of String)

			For Index As Integer = 1 To domainsService.ComObject.Count - 1
				domains.Add(domainsService.ComObject.Items(Index).Name)
			Next

			Return domains.ToArray()
		End If

		Return Nothing
	End Function

    Public Function GetDomain(ByVal domainName As String) As SolidCP.Providers.Mail.MailDomain Implements SolidCP.Providers.Mail.IMailServer.GetDomain
        Dim domainService As Service = LoadLocalDomainsService()
        Dim domain As MailDomain = Nothing

        Try
            If domainService.Succeed Then
                Dim domainIndex As Integer = domainService.ComObject.IndexOf(domainName)
                If domainIndex >= 0 Then
                    Dim objDomain = domainService.ComObject.Items(domainIndex)

                    domain = New MailDomain()
                    domain.Enabled = True
                    domain.MaxDomainUsers = objDomain.MaxAccounts
                    domain.MaxLists = objDomain.MaxDistribLists

                    'If Not (this.Settings("MaxMailboxSizeInMB") Is Nothing) Then
                    '    domItem.MaxMailboxSizeInMB = Convert.ToInt32(settings("MaxMailboxSizeInMB"))
                    'End If
                    domain.Name = objDomain.Name

					Dim postmaster As MailAccount = GetAccount(String.Concat("postmaster@", domain.Name))
					domain.CatchAllAccount = GetEmailName(postmaster.ForwardingAddresses(0))
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't get domain you've specified.", ex)
        End Try

        Return domain
    End Function

    Public Function GetDomainAliases(ByVal domainName As String) As String() Implements SolidCP.Providers.Mail.IMailServer.GetDomainAliases
        Dim domainService As Service = LoadLocalDomainsService()
        Dim aliases As New List(Of String)

        Try
            If domainService.Succeed Then
                Dim domainIndex As Integer = domainService.ComObject.IndexOf(domainName)
                If domainIndex >= 0 Then
                    Dim domain As Object = domainService.ComObject.Items(domainIndex)
                    If domain.Aliases.Count > 0 Then
                        For i As Integer = 0 To domain.Aliases.Count - 1
                            aliases.Add(domain.Aliases.Items(i))
                        Next i
                    End If
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain domain aliases.", ex)
        End Try

        Return aliases.ToArray()
    End Function

    Public Function GetGroup(ByVal groupName As String) As SolidCP.Providers.Mail.MailGroup Implements SolidCP.Providers.Mail.IMailServer.GetGroup
        Dim groupService As Service = LoadDistribListsService()
        Dim group As MailGroup = Nothing

        Try
            If groupService.Succeed Then
                Dim groupIndex As Integer = groupService.ComObject.IndexOf(groupName)
                If groupIndex >= 0 Then
                    Dim objGroup As Object = groupService.ComObject.Items(groupIndex)
                    group = ConvertToMailGroup(objGroup)
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain group you've specified.", ex)
        End Try

        Return group
    End Function

    Public Function GetGroups(ByVal domainName As String) As SolidCP.Providers.Mail.MailGroup() Implements SolidCP.Providers.Mail.IMailServer.GetGroups
        Dim domainService As Service = LoadLocalDomainsService()
        Dim groupService As Service = LoadDistribListsService()
        Dim groups As New List(Of MailGroup)

        Try
            If domainService.Succeed Then
                Dim domainIndex As Integer = domainService.ComObject.IndexOf(domainName)

                If domainIndex >= 0 Then
                    If groupService.Succeed Then
                        If groupService.ComObject.Count > 0 Then
                            For i As Integer = 0 To groupService.ComObject.Count - 1
                                Dim objGroup As Object = groupService.ComObject.Items(i)
                                If objGroup.Name.IndexOf(domainName) >= 0 Then
                                    groups.Add(ConvertToMailGroup(objGroup))
                                End If
                            Next i
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain domain groups.", ex)
        End Try

        Return groups.ToArray()
    End Function

    Public Function GetList(ByVal maillistName As String) As SolidCP.Providers.Mail.MailList Implements SolidCP.Providers.Mail.IMailServer.GetList
        Dim mailList As MailList = Nothing

        Try
            Dim lists As New ArgoMailLists()
            Dim idx As Integer = lists.IndexOf(maillistName)

            If idx >= 0 Then
                mailList = ConvertToMailList(lists.Items(idx))
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain mail list.", ex)
        End Try

        Return mailList
    End Function

    Public Function GetLists(ByVal domainName As String) As SolidCP.Providers.Mail.MailList() Implements SolidCP.Providers.Mail.IMailServer.GetLists
        Dim mailLists As List(Of MailList) = Nothing

        Try
            Dim lists As New ArgoMailLists()

            If lists.Items.Count > 0 Then

                mailLists = New List(Of MailList)

                For Each item As ArgoMailListItem In lists.Items
                    If item.Account.IndexOf(domainName) > -1 Then
                        mailLists.Add(ConvertToMailList(item))
                    End If
                Next item
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain mail lists.", ex)
        End Try

        Return mailLists.ToArray()
    End Function

    Public Function GroupExists(ByVal groupName As String) As Boolean Implements SolidCP.Providers.Mail.IMailServer.GroupExists
        Dim groupService As Service = LoadDistribListsService()
        Dim exists As Boolean = False

        Try
            If groupService.Succeed Then
                Dim groupIndex As Integer = groupService.ComObject.IndexOf(groupName)
                exists = groupIndex >= 0
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't obtain mail group.", ex)
        End Try

        Return exists
    End Function

    Public Function ListExists(ByVal maillistName As String) As Boolean Implements SolidCP.Providers.Mail.IMailServer.ListExists
        Dim exists As Boolean = False

        Try
            Dim lists As New ArgoMailLists()

            Dim idx As Integer = lists.IndexOf(maillistName)
            exists = idx >= 0
        Catch ex As Exception
            Log.WriteError("Couldn't obtain mail list.", ex)
        End Try

        Return exists
    End Function

    Public Sub UpdateAccount(ByVal mailbox As SolidCP.Providers.Mail.MailAccount) Implements SolidCP.Providers.Mail.IMailServer.UpdateAccount
        Dim userService As Service = LoadUsersService()

        Try
            If userService.Succeed Then
                If userService.ComObject.UserExists(mailbox.Name) Then
                    Dim boxIdx As Integer
                    Dim respMsgLines As Object = CreateLinesObject()

                    Dim user As Object = userService.ComObject.GetUserByName(mailbox.Name)
                    boxIdx = userService.ComObject.IndexOf(mailbox.Name)

                    Dim iSize As Integer = mailbox.MaxMailboxSize
                    'If Not (settings("MaxMailboxSizeInMB") Is Nothing) Then
                    '    iSize = Convert.ToInt32(settings("MaxMailboxSizeInMB"))
                    'End If
                    user.RealName = mailbox.FirstName + " " + mailbox.LastName
                    user.Password = mailbox.Password
                    user.Active = mailbox.Enabled
                    user.AutoResponderEnabled = mailbox.ResponderEnabled
                    user.AutoResponderSubject = mailbox.ResponderSubject
                    respMsgLines.Add(mailbox.ResponderMessage)
                    user.AutoResponderData = respMsgLines

                    If Not mailbox.ForwardingAddresses Is Nothing Then
                        If mailbox.ForwardingAddresses.Length > 0 Then
                            user.ForwardAddress = [String].Join(", ", mailbox.ForwardingAddresses)
                        Else
                            user.ForwardAddress = ""
                        End If
                    End If

                    user.ReturnAddress = mailbox.ReplyTo
                    user.MailboxSize = iSize
                    user.KeepCopies = Not mailbox.DeleteOnForward

                    userService.ComObject.Items(boxIdx) = user
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't update mailbox.", ex)
        End Try
    End Sub

    Public Sub UpdateDomain(ByVal domain As SolidCP.Providers.Mail.MailDomain) Implements SolidCP.Providers.Mail.IMailServer.UpdateDomain
        Dim domainService As Service = LoadLocalDomainsService()

        Try
            If domainService.Succeed Then
                Dim domainIndex As Integer = domainService.ComObject.IndexOf(domain.Name)
                If domainIndex >= 0 Then
                    Dim objDomain As Object = domainService.ComObject.Items(domainIndex)

                    objDomain.MaxAccounts = domain.MaxDomainUsers
                    objDomain.MaxDistribLists = domain.MaxLists

                    domainService.ComObject.Items(domainIndex) = objDomain

					If Not String.IsNullOrEmpty(domain.CatchAllAccount) Then
						Dim postmaster As New MailAccount()
						postmaster.Enabled = True
						postmaster.Name = String.Concat("postmaster@", domain.Name)
						postmaster.DeleteOnForward = True
						postmaster.ForwardingAddresses = New String() {String.Concat(domain.CatchAllAccount, "@", domain.Name)}
						UpdateAccount(postmaster)
					End If
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't update domain.", ex)
        End Try
    End Sub

    Public Sub UpdateGroup(ByVal group As SolidCP.Providers.Mail.MailGroup) Implements SolidCP.Providers.Mail.IMailServer.UpdateGroup
        Dim groupService As Service = LoadDistribListsService()

        Try
            If groupService.Succeed Then
                Dim groupIndex As Integer = groupService.ComObject.IndexOf(group.Name)
                If groupIndex >= 0 Then
                    Dim objGroup As Object = groupService.ComObject.Items(groupIndex)
                    Dim objLines As Object = CreateLinesObject()

                    For Each member As String In group.Members
                        objLines.Add(member)
                    Next member

                    objGroup.Members = objLines

                    groupService.ComObject.Items(groupIndex) = objGroup
                End If
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't update mail group.", ex)
        End Try
    End Sub

    Public Sub UpdateList(ByVal maillist As SolidCP.Providers.Mail.MailList) Implements SolidCP.Providers.Mail.IMailServer.UpdateList
        Try
            Dim lists As ArgoMailLists = New ArgoMailLists()
            Dim updItem As ArgoMailListItem = lists.GetItem(maillist.Name)

            If Not updItem Is Nothing Then
                Dim listAccount As String = maillist.Name

                If Not MailboxExists(listAccount) Then
                    CreateMailbox(listAccount, "MailList Account")
                End If

                updItem.Name = GetEmailName(maillist.Name)
                updItem.Account = listAccount

                If Not String.IsNullOrEmpty(maillist.Description) Then
                    updItem.Desription = maillist.Description.Replace(ControlChars.Cr + ControlChars.Lf, ControlChars.Lf)
                End If

                If maillist.PostingMode = PostingMode.MembersCanPost Then
                    updItem.ListISClosed = True
                Else
                    updItem.ListISClosed = False
                End If

                If Not maillist.Members Is Nothing Then
                    Dim members As New List(Of String)

                    For i As Integer = 0 To maillist.Members.Length - 1
                        If Not String.IsNullOrEmpty(maillist.Members(i)) Then
                            members.Add(maillist.Members(i))
                        End If
                    Next i

                    updItem.Count = members.Count
                    updItem.Members = members.ToArray()
                End If

                If maillist.ReplyToMode = ReplyTo.RepliesToSender Then
                    updItem.RepliesGoToSender = True
                Else
                    updItem.RepliesGoToSender = False
                End If

                updItem.RequireMemberShip = maillist.Moderated

                lists.Update(updItem)
            End If
        Catch ex As Exception
            Log.WriteError("Couldn't update mail list.", ex)
        End Try
    End Sub
#End Region

#Region "HostingServiceProviderBase"
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

	Public Overrides Function IsInstalled() As Boolean
		Return True
	End Function



    ' Public Overrides Function IsInstalled() As Boolean

    'Dim productName As String = ""
    'Dim productVersion As String = ""

    '    Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")

    '   Dim names As String() = key.GetSubKeyNames

    '      For Each name As String In names
    ' Dim subkey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + name)
    '        If CStr(subkey.GetValue("DisplayName")) IsNot Nothing Then
    '           productName = CStr(subkey.GetValue("DisplayName"))
    '      End If
    '     If productName IsNot Nothing Then
    '        If productName.Equals("ArGoSoft Mail Server .NET") Then
    '           productVersion = CStr(subkey.GetValue("DisplayVersion"))
    '          Exit For
    '     End If
    'End If
    'Next name

    'If [String].IsNullOrEmpty(productVersion) = False Then
    'Dim split As String() = productVersion.Split(New [Char]() {"."c})
    '       Return split(0).Equals("1")
    '  Else
    '
    '       key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall")
    '      names = key.GetSubKeyNames
    '
    '       For Each name As String In names
    'Dim subkey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + name)
    '          If CStr(subkey.GetValue("DisplayName")) IsNot Nothing Then
    '               productName = CStr(subkey.GetValue("DisplayName"))
    '         End If
    '        If productName IsNot Nothing Then
    '           If productName.Equals("ArGoSoft Mail Server .NET") Then
    '              productVersion = CStr(subkey.GetValue("DisplayVersion"))
    '             Exit For
    '               End If
    '          End If
    '     Next name
    '
    '       If [String].IsNullOrEmpty(productVersion) = False Then
    'Dim split As String() = productVersion.Split(New [Char]() {"."c})
    '           Return split(0).Equals("1")
    '      Else
    '          Return False
    '      End If
    '  End If


    '    End Function

End Class
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
Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports Microsoft.Win32
Imports SolidCP.Server.Utils

Public Class hMailServer
    Inherits HostingServiceProviderBase
    Implements IMailServer

	Private Const APPLICATION_PROG_ID As String = "hMailServer.Application"
	Private Const SolidCP_RULE_NAME As String = "SolidCP"
	Private Const MAIL_GROUP_RULE As String = "Mail Group Rule"
	Private Const FORWARDING_RULE As String = "Forwarding Rule"

	Protected Overridable ReadOnly Property hMailServer() As Object
		Get
			Return CreateObject(APPLICATION_PROG_ID)
		End Get
	End Property

	Class Service
		Public ComObject As Object
		Public Succeed As Boolean
	End Class

#Region "Private Helper methods"
	Private Function CheckAccountIsGroup(ByVal objAccount As Object) As Boolean
		If objAccount.Rules.Count > 0 Then
			' check rule actions
			Dim objRule As Object = objAccount.Rules.Item(0)
			' first read rule name
			If String.Compare(objRule.Name, MAIL_GROUP_RULE, True) = 0 Then
				Return True
			Else ' read rule actions
				For j As Integer = 0 To objRule.Actions.Count - 1
					If objRule.Actions.Item(j).Type = 1 Then 'eRADeleteEmail
						Return True
					End If
				Next
			End If
		End If
		Return False
	End Function

	Private Function GetAccountForwardings(ByVal objAccount As Object) As String()
		Dim forwardings As List(Of String) = New List(Of String)
		If objAccount.Rules.Count > 0 Then
			' check rule actions
			Dim objRule As Object = objAccount.Rules.Item(0)
			Dim j As Integer
			For j = 0 To objRule.Actions.Count - 1
				If objRule.Actions.Item(j).Type = 2 Then 'eRAForwardEmail
					forwardings.Add(objRule.Actions.Item(j).To)
				End If
			Next
		End If
		Return forwardings.ToArray()
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

	Private Function GetUtilitiesObject() As Service
		' find existing domain
		Dim objDomain As New Service()
		objDomain.Succeed = False

		Try
			objDomain.ComObject = hMailServer.Utilities
			objDomain.Succeed = True
		Catch ex As Exception
			Log.WriteError("Couldn't create hMailServer.Application ActiveX object.", ex)
		End Try

		Return objDomain
	End Function

	Private Function GetDomainObject(ByVal domainName As String) As Service
		' find existing domain
		Dim objDomain As New Service()
		objDomain.Succeed = False

		Try
			objDomain.ComObject = hMailServer.Domains.ItemByName(domainName)
			objDomain.Succeed = True
		Catch ex As Exception
			Log.WriteError("Couldn't create hMailServer.Application ActiveX object.", ex)
		End Try

		Return objDomain
	End Function

	Private Function GetDomainsObject() As Service
		' find existing domain
		Dim objDomain As New Service()
		objDomain.Succeed = False

		Try
			objDomain.ComObject = hMailServer.Domains
			objDomain.Succeed = True
		Catch ex As Exception
			Log.WriteError("Couldn't create hMailServer.Application ActiveX object.", ex)
		End Try

		Return objDomain
	End Function

	Protected Overridable Function ConvertToMailList(ByRef objMailList As Object) As MailList
		Dim mailList As New MailList()

		mailList.Enabled = objMailList.Active
		mailList.Name = objMailList.Address
		mailList.RequireSmtpAuthentication = objMailList.RequireSMTPAuth

		'If objMailList.RequireSMTPAuth Then
		'mailList.PostingMode = PostingMode.MembersCanPost
		'ElseIf Not String.IsNullOrEmpty(objMailList.RequireSenderAddress) Then
		'mailList.PostingMode = PostingMode.ModeratorCanPost
		'mailList.ModeratorAddress = objMailList.RequireSenderAddress
		'Else
		'mailList.PostingMode = PostingMode.AnyoneCanPost
		'End If

		If objMailList.Mode = 1 Then
			mailList.PostingMode = PostingMode.MembersCanPost
		ElseIf Not String.IsNullOrEmpty(objMailList.RequireSenderAddress) And (objMailList.Mode = 2) Then
			mailList.PostingMode = PostingMode.ModeratorCanPost
			mailList.ModeratorAddress = objMailList.RequireSenderAddress
		Else
			mailList.PostingMode = PostingMode.AnyoneCanPost
		End If

		' load list members
		Dim membersCount As Integer = objMailList.Recipients.Count - 1

		Dim objRecipient As Object
		If membersCount > 0 Then
			mailList.Members = New String(membersCount) {}
			For index As Integer = 0 To membersCount
				objRecipient = objMailList.Recipients.Item(index)
				mailList.Members(index) = objRecipient.RecipientAddress
			Next index
		Else
			'case when list has one member
			If membersCount = 0 Then
				mailList.Members = New String(1) {}
				objRecipient = objMailList.Recipients.Item(0)
				mailList.Members(0) = objRecipient.RecipientAddress
			End If
		End If
		'membersCount = -1 - list does not have members

		Return mailList
	End Function

	Protected Overridable Function ConvertToMailGroup(ByRef objMailGroup As Object) As MailGroup
		Dim mailGroup As New MailGroup()

		mailGroup.Name = objMailGroup.Address
		mailGroup.Enabled = objMailGroup.Active
		'mailGroup.DiskSpace = objMailGroup.MaxSize

		Dim objGroupRules As Object = objMailGroup.Rules
		Dim rulesCount As Integer = objGroupRules.Count - 1

		Dim objGroupRule As Object = Nothing
		' find mail group rule
		For i As Integer = 0 To rulesCount
			objGroupRule = objMailGroup.Rules.Item(i)
			If String.Compare(objGroupRule.Name, MAIL_GROUP_RULE, True) = 0 Then
				Exit For
			End If
		Next i

		If Not objGroupRule Is Nothing Then
			Dim groupMembers As New List(Of String)
			Dim actionsCount As Integer = objGroupRule.Actions.Count - 1

			' copy group members
			For i As Integer = 0 To actionsCount
				Dim objRuleAction As Object = objGroupRule.Actions.Item(i)
				If objRuleAction.Type = 2 Then 'eRAForwardEmail
					groupMembers.Add(objRuleAction.To)
				End If
			Next i

			mailGroup.Members = New String(groupMembers.Count) {}
			groupMembers.CopyTo(mailGroup.Members)
		End If

		Return mailGroup
	End Function
#End Region

#Region "IMailServer members"
	Public Function AccountExists(ByVal mailboxName As String) As Boolean Implements IMailServer.AccountExists
		' find existing domain
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailboxName))

		If objDomain.Succeed Then
			' find existing account
			Dim i As Integer
			For i = 0 To objDomain.ComObject.Accounts.Count - 1
				If String.Compare(objDomain.ComObject.Accounts.Item(i).Address, mailboxName, True) = 0 Then
					Return True
				End If
			Next
		End If

		Return False
	End Function

	Public Sub AddDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.AddDomainAlias
		' find existing domain
		Dim objDomain As Service = GetDomainObject(domainName)

		If objDomain.Succeed Then
			' add domain alias
			Dim objAlias As Object = objDomain.ComObject.DomainAliases.Add()
			objAlias.DomainID = objDomain.ComObject.ID
			objAlias.AliasName = aliasName
			objAlias.Save()
			objDomain.ComObject.DomainAliases.Refresh()
		End If
	End Sub

	Public Sub CreateAccount(ByVal mailbox As MailAccount) Implements IMailServer.CreateAccount
		' find existing domain
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailbox.Name))

		If objDomain.Succeed Then
			' add account
			Dim objAccount As Object = objDomain.ComObject.Accounts.Add()
			objAccount.DomainID = objDomain.ComObject.ID
			objAccount.Address = mailbox.Name
			objAccount.Active = mailbox.Enabled
			objAccount.Password = mailbox.Password
			objAccount.MaxSize = mailbox.MaxMailboxSize
			objAccount.PersonFirstName = mailbox.FirstName
			objAccount.PersonLastName = mailbox.FirstName
			objAccount.SignatureEnabled = mailbox.SignatureEnabled
			objAccount.SignaturePlainText = mailbox.Signature
			objAccount.SignatureHTML = mailbox.SignatureHTML

			If mailbox.ResponderEnabled Then
				objAccount.VacationMessageIsOn = True
				objAccount.VacationSubject = mailbox.ResponderSubject
				objAccount.VacationMessage = mailbox.ResponderMessage
			End If

			'set forwarding address
			If mailbox.ForwardingAddresses.Length > 0 Then
				objAccount.ForwardAddress = mailbox.ForwardingAddresses(0)
				objAccount.ForwardEnabled = True
				objAccount.ForwardKeepOriginal = mailbox.RetainLocalCopy
			End If

			objAccount.Save()

			' set account rules
			SetAccountRules(mailbox, objAccount)


		End If
	End Sub

	Private Sub SetAccountRules(ByVal mailbox As MailAccount, ByVal objAccount As Object)

		' check for default SolidCP rule
		Dim ruleExists As Boolean = False
		Dim objRule As Object = Nothing

		For i As Integer = 0 To objAccount.Rules.Count - 1
			objRule = objAccount.Rules.Item(i)
			If String.Compare(objRule.Name, SolidCP_RULE_NAME, True) = 0 Then
				ruleExists = True
				Exit For
			End If
		Next i

		If ruleExists Then
			' delete rule
			objAccount.Rules.DeleteByDBID(objRule.ID)
		End If

		If Not mailbox.ForwardingAddresses Is Nothing _
		 And mailbox.ForwardingAddresses.Length > 0 Then
			' create rule

			' add "default" rule
			objRule = objAccount.Rules.Add()
			objRule.AccountID = objAccount.ID
			objRule.Active = True
			objRule.Name = SolidCP_RULE_NAME
			objRule.Save()

			' Add criteria
			Dim objCriteria As Object = objRule.Criterias.Add()
			objCriteria.RuleID = objRule.ID
			objCriteria.PredefinedField = 6	' hMailServer.eRulePredefinedField.eFTMessageSize
			objCriteria.MatchType = 4 ' hMailServer.eRuleMatchType.eMTGreaterThan
			objCriteria.MatchValue = "0"
			objCriteria.Save()

			' add forwarding addresses
			Dim forwarding As String
			For Each forwarding In mailbox.ForwardingAddresses
				Dim objRuleAction As Object = objRule.Actions.Add()
				objRuleAction.RuleID = objRule.ID
				objRuleAction.Type = 2 'eRAForwardEmail 
				objRuleAction.To = forwarding
				objRuleAction.Save()
			Next

			If mailbox.DeleteOnForward Then
				Dim objRuleAction As Object = objRule.Actions.Add()
				objRuleAction.RuleID = objRule.ID
				objRuleAction.RuleID = 1 'eRADeleteEmail
				objRuleAction.Save()
			End If
		End If
	End Sub

	Public Sub CreateDomain(ByVal domain As MailDomain) Implements IMailServer.CreateDomain
		Dim objDomain As New Service()

		objDomain.ComObject = hMailServer.Domains.Add()
		objDomain.ComObject.Name = domain.Name
		objDomain.ComObject.Active = domain.Enabled
		objDomain.ComObject.Postmaster = domain.CatchAllAccount
		objDomain.ComObject.Save()
	End Sub

	Public Sub CreateGroup(ByVal group As MailGroup) Implements IMailServer.CreateGroup
		Dim objDomain As Service = GetDomainObject(GetDomainName(group.Name))
		Dim objGroup As Object = Nothing

		If objDomain.Succeed Then
			Dim Length As Integer = objDomain.ComObject.Accounts.Count - 1

			' check whether a group is already created
			For index As Integer = 0 To Length
				Dim objAccount As Object = objDomain.ComObject.Accounts.Item(index)
				If CheckAccountIsGroup(objAccount) Then
					If String.Compare(objAccount.Address, group.Name, True) = 0 Then
						objGroup = objAccount
						Exit For
					End If
				End If
			Next index

			' throw an exception
			If Not objGroup Is Nothing Then
				Throw New Exception("Group is already exsists.")
			End If

			objGroup = objDomain.ComObject.Accounts.Add()
			objGroup.DomainID = objDomain.ComObject.ID
			objGroup.Address = group.Name
			objGroup.Active = group.Enabled
			objGroup.AdminLevel = 0	' hAdminLevelNormal

			' group should be empty
			'objGroup.MaxSize = group.DiskSpace
			objGroup.VacationMessageIsOn = False
			objGroup.VacationSubject = String.Empty
			objGroup.VacationMessage = String.Empty
			objGroup.Save()

			' Create mail group rule
			Dim objRule As Object = objGroup.Rules.Add()
			objRule.AccountID = objGroup.ID
			objRule.Active = True
			objRule.Name = MAIL_GROUP_RULE
			objRule.Save()

			' Add criteria
			Dim objCriteria As Object = objRule.Criterias.Add()
			objCriteria.RuleID = objRule.ID
			objCriteria.PredefinedField = 6	' hMailServer.eRulePredefinedField.eFTMessageSize
			objCriteria.MatchType = 4 ' hMailServer.eRuleMatchType.eMTGreaterThan
			objCriteria.MatchValue = "0"
			objCriteria.Save()

			' create group members
			If Not group.Members Is Nothing Then
				For Each member As String In group.Members
					Dim objGroupMemberAction As Object = objRule.Actions.Add()
					objGroupMemberAction.RuleID = objRule.ID
					objGroupMemberAction.Type = 2 'eRAForwardEmail
					objGroupMemberAction.To = member
					objGroupMemberAction.Save()
				Next member
			End If

			' Add delete mail action
			Dim objGroupAction As Object = objRule.Actions.Add()
			objGroupAction.RuleID = objRule.ID
			objGroupAction.Type = 1	'eRADeleteEmail
			objGroupAction.Save()

		End If
	End Sub

	Public Sub CreateList(ByVal maillist As MailList) Implements IMailServer.CreateList
		Dim objDomain As Service = GetDomainObject(GetDomainName(maillist.Name))

		If objDomain.Succeed Then
			Dim objMailList As Object = objDomain.ComObject.DistributionLists.Add()
			objMailList.Active = maillist.Enabled
			objMailList.Address = maillist.Name

			Select Case maillist.PostingMode
				Case PostingMode.MembersCanPost
					objMailList.RequireSMTPAuth = True
				Case PostingMode.ModeratorCanPost
					If String.IsNullOrEmpty(maillist.ModeratorAddress) Then
						Throw New Exception("List moderator address doesn't specified.")
					End If
					objMailList.RequireSenderAddress = maillist.ModeratorAddress
			End Select

			objMailList.Save()

			' save list members
			If Not maillist.Members Is Nothing Then
				For Each member As String In maillist.Members
					Dim objRecipient As Object = objMailList.Recipients.Add()
					objRecipient.RecipientAddress = member
					objRecipient.Save()
				Next member
			End If
		End If
	End Sub

	Public Sub DeleteAccount(ByVal mailboxName As String) Implements IMailServer.DeleteAccount
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailboxName))

		If objDomain.Succeed Then
			Dim index As Integer

			' find and remove account if exists
			For index = 0 To objDomain.ComObject.Accounts.Count - 1
				Dim objAccount As Object = objDomain.ComObject.Accounts.Item(index)
				If String.Compare(objAccount.Address, mailboxName, True) = 0 Then
					objDomain.ComObject.Accounts.Delete(index)
					Exit For
				End If
			Next

			' find and remove alias if exists
			For index = 0 To objDomain.ComObject.Aliases.Count - 1
				Dim objAlias As Object = objDomain.ComObject.Aliases.Item(index)
				If String.Compare(objAlias.Name, mailboxName, True) = 0 Then
					objDomain.ComObject.Aliases.Delete(index)
					Exit For
				End If
			Next
		End If
	End Sub

	Public Function MailAliasExists(ByVal mailAliasName As String) As Boolean Implements IMailServer.MailAliasExists
		' find existing domain
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailAliasName))

		If objDomain.Succeed Then
			Try
				' find existing account
				Dim i As Integer
				For i = 0 To objDomain.ComObject.Aliases.Count - 1
					If String.Compare(objDomain.ComObject.Aliases.Item(i).Address, mailAliasName, True) = 0 Then
						Return True
					End If
				Next
			Catch ex As Exception
				Log.WriteError("Couldn't determine if mail alias exists.", ex)
			End Try
		End If

		Return False
	End Function

	Public Function GetMailAliases(ByVal domainName As String) As MailAlias() Implements IMailServer.GetMailAliases
		Dim aliases As New List(Of MailAlias)

		' find existing domain
		Dim objDomain As Service = GetDomainObject(domainName)

		If objDomain.Succeed Then
			Try
				' get all domain accounts
				Dim i As Integer
				For i = 0 To objDomain.ComObject.Aliases.Count - 1
					Dim objAccount As Object = objDomain.ComObject.Aliases.Item(i)


					' get account details
					Dim mailAlias As MailAlias = New MailAlias()
					mailAlias.Name = objAccount.Name
					mailAlias.Enabled = objAccount.Active
					mailAlias.ForwardTo = objAccount.Value
					aliases.Add(mailAlias)
				Next
			Catch ex As Exception
				Log.WriteError("Couldn't get mail aliases.", ex)
			End Try
		End If

		Return aliases.ToArray()
	End Function

	Public Function GetMailAlias(ByVal mailAliasName As String) As MailAlias Implements IMailServer.GetMailAlias
		'recreate alias if it was created incorrectly before
		Dim mailAlias As New MailAlias
		Dim newMailAlias As New MailAlias

		If AccountExists(mailAliasName) Then
			Dim mailAccount As MailAccount = GetAccount(mailAliasName)
			newMailAlias.Name = mailAccount.Name
			newMailAlias.ForwardTo = mailAccount.ForwardingAddresses(0)
			'delete incorrect account
			DeleteAccount(mailAliasName)
			'recreate mail alias 
			CreateMailAlias(newMailAlias)
			Return newMailAlias
		End If

		' find existing domain
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailAliasName))

		If objDomain.Succeed Then
			Try
				' find through all domain accounts
				For i As Integer = 0 To objDomain.ComObject.Aliases.Count - 1
					Dim objAccount As Object = objDomain.ComObject.Aliases.Item(i)

					If String.Compare(objAccount.Name, mailAliasName, True) = 0 Then
						' check if this is a Group
						'If CheckAccountIsGroup(objAccount) Then
						'Continue For
						'End If

						' get account details
						mailAlias.Name = objAccount.Name
						mailAlias.Enabled = objAccount.Active
						mailAlias.ForwardTo = objAccount.Value
						Return mailAlias
					End If
				Next
			Catch ex As Exception
				Log.WriteError("Couldn't get mail alias.", ex)
			End Try
		End If

		Return Nothing
	End Function

	Public Sub CreateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.CreateMailAlias
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailAlias.Name))
		If objDomain.Succeed Then
			Try
				' add alias
				Dim objAlias As Object = objDomain.ComObject.Aliases.Add()
				objAlias.DomainID = objDomain.ComObject.ID
				objAlias.Name = mailAlias.Name
				objAlias.Active = True
				objAlias.Value = mailAlias.ForwardTo
				objAlias.Save()
			Catch ex As Exception
				Log.WriteError("Couldn't create mail alias.", ex)
			End Try
		End If
	End Sub

	Public Sub UpdateMailAlias(ByVal mailAlias As MailAlias) Implements IMailServer.UpdateMailAlias
		' find existing domain
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailAlias.Name))

		If objDomain.Succeed Then
			Try
				' find through all domain accounts
				For i As Integer = 0 To objDomain.ComObject.Aliases.Count - 1
					Dim objAccount As Object = objDomain.ComObject.Aliases.Item(i)

					If String.Compare(objAccount.Name, mailAlias.Name, True) = 0 Then
						'Fix mail alias is disabled in hMail Server when update it in SCP
						objAccount.Active = True
						objAccount.Value = mailAlias.ForwardTo
						objAccount.Save()
					End If
				Next
			Catch ex As Exception
				Log.WriteError("Couldn't update mail alias.", ex)
			End Try
		End If
	End Sub

	Public Sub DeleteMailAlias(ByVal mailAliasName As String) Implements IMailServer.DeleteMailAlias
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailAliasName))
		If objDomain.Succeed Then
			Try
				Dim index As Integer
				' find and remove alias if exists
				For index = 0 To objDomain.ComObject.Aliases.Count - 1
					Dim objAlias As Object = objDomain.ComObject.Aliases.Item(index)
					If String.Compare(objAlias.Name, mailAliasName, True) = 0 Then
						objDomain.ComObject.Aliases.Delete(index)
						Exit For
					End If
				Next
			Catch ex As Exception
				Log.WriteError("Couldn't delete mail alias.", ex)
			End Try
		End If
	End Sub

	Public Sub DeleteDomain(ByVal domainName As String) Implements IMailServer.DeleteDomain
		' find existing domain
		Dim objDomain As Object
		Try
			objDomain = hMailServer.Domains.ItemByName(domainName)
		Catch
			Throw New Exception("Specified mail domain does not exists")
		End Try

		' delete domain
		objDomain.Delete()
	End Sub

	Public Sub DeleteDomainAlias(ByVal domainName As String, ByVal aliasName As String) Implements IMailServer.DeleteDomainAlias
		' find existing domain
		Dim objDomain As Service = GetDomainObject(domainName)

		If objDomain.Succeed Then
			Dim i As Integer
			For i = 0 To objDomain.ComObject.DomainAliases.Count - 1
				If String.Compare(objDomain.ComObject.DomainAliases.Item(i).AliasName, aliasName, True) = 0 Then
					objDomain.ComObject.DomainAliases.Delete(i)
					Return
				End If
			Next
		End If
	End Sub

	Public Sub DeleteGroup(ByVal groupName As String) Implements IMailServer.DeleteGroup
		DeleteAccount(groupName)
	End Sub

	Public Sub DeleteList(ByVal maillistName As String) Implements IMailServer.DeleteList
		Dim objDomain As Service = GetDomainObject(GetDomainName(maillistName))

		If objDomain.Succeed Then
			Dim objMailList = objDomain.ComObject.DistributionLists.ItemByAddress(maillistName)

			If Not objMailList Is Nothing Then
				objMailList.Delete()
			End If
		End If
	End Sub

	Public Function DomainAliasExists(ByVal domainName As String, ByVal aliasName As String) As Boolean Implements IMailServer.DomainAliasExists
		' find existing domain
		Dim objDomain As Service = GetDomainObject(domainName)

		If objDomain.Succeed Then
			' check aliases
			Dim i As Integer
			For i = 0 To objDomain.ComObject.DomainAliases.Count - 1
				If String.Compare(objDomain.ComObject.DomainAliases.Item(i).AliasName, aliasName, True) = 0 Then
					Return True
				End If
			Next
		End If

		Return False
	End Function

	Public Function DomainExists(ByVal domainName As String) As Boolean Implements IMailServer.DomainExists
		Dim objDomain As Service = GetDomainObject(domainName)

		Return objDomain.Succeed
	End Function

	Public Function GetAccount(ByVal mailboxName As String) As MailAccount Implements IMailServer.GetAccount
		' find existing domain
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailboxName))

		If objDomain.Succeed Then
			' find through all domain accounts
			For i As Integer = 0 To objDomain.ComObject.Accounts.Count - 1
				Dim objAccount As Object = objDomain.ComObject.Accounts.Item(i)

				If String.Compare(objAccount.Address, mailboxName, True) = 0 Then
					' check if this is a Group
					If CheckAccountIsGroup(objAccount) Then
						Continue For
					End If

					' get account details
					Dim account As MailAccount = New MailAccount()
					account.Name = objAccount.Address
					account.FirstName = objAccount.PersonFirstName
					account.LastName = objAccount.PersonLastName
					account.Enabled = objAccount.Active
					account.MaxMailboxSize = objAccount.MaxSize
					account.Password = objAccount.Password
					account.ResponderEnabled = objAccount.VacationMessageIsOn
					account.ResponderSubject = objAccount.VacationSubject
					account.ResponderMessage = objAccount.VacationMessage
					Dim forwardings As List(Of String) = New List(Of String)
					forwardings.Add(objAccount.ForwardAddress)
					account.ForwardingAddresses = forwardings.ToArray
					account.RetainLocalCopy = objAccount.ForwardKeepOriginal
					'Signature
					account.SignatureEnabled = objAccount.SignatureEnabled
					account.Signature = objAccount.SignaturePlainText
					account.SignatureHTML = objAccount.SignatureHTML
					Return account
				End If
			Next

			' find through forwardings (hMail aliases)
			For i As Integer = 0 To objDomain.ComObject.Aliases.Count - 1
				Dim objAlias As Object = objDomain.ComObject.Aliases.Item(i)
				If String.Compare(objAlias.Name, mailboxName, True) = 0 Then
					Dim account As MailAccount = New MailAccount()
					account.Name = objAlias.Name
					account.Enabled = objAlias.Active
					account.ForwardingAddresses = New String() {objAlias.Value}
					account.DeleteOnForward = True
					Return account
				End If
			Next
		End If

		Return Nothing
	End Function

	Public Function GetAccounts(ByVal domainName As String) As MailAccount() Implements IMailServer.GetAccounts
		Dim accounts As New List(Of MailAccount)

		' find existing domain
		Dim objDomain As Service = GetDomainObject(domainName)

		If objDomain.Succeed Then
			' get all domain accounts
			Dim i As Integer
			For i = 0 To objDomain.ComObject.Accounts.Count - 1
				Dim objAccount As Object = objDomain.ComObject.Accounts.Item(i)

				If CheckAccountIsGroup(objAccount) Then
					Continue For
				End If

				' get account details
				Dim account As MailAccount = New MailAccount()
				account.Name = objAccount.Address
				account.FirstName = objAccount.PersonFirstName
				account.LastName = objAccount.PersonLastName
				account.Enabled = objAccount.Active
				account.MaxMailboxSize = objAccount.MaxSize
				account.Password = objAccount.Password
				account.ResponderEnabled = objAccount.VacationMessageIsOn
				account.ResponderSubject = objAccount.VacationSubject
				account.ResponderMessage = objAccount.VacationMessage
				Dim forwardings As List(Of String) = New List(Of String)
				forwardings.Add(objAccount.ForwardAddress)
				account.ForwardingAddresses = forwardings.ToArray
				account.RetainLocalCopy = objAccount.ForwardKeepOriginal
				'Signature
				account.SignatureEnabled = objAccount.SignatureEnabled
				account.Signature = objAccount.SignaturePlainText
				account.SignatureHTML = objAccount.SignatureHTML
				accounts.Add(account)
			Next
		End If

		Return accounts.ToArray()
	End Function

	Public Overridable Function GetDomains() As String() Implements IMailServer.GetDomains
		Dim objDomains As Service = GetDomainsObject()

		If objDomains.Succeed Then
			Dim domains As New List(Of String)

			For Index As Integer = 0 To objDomains.ComObject.Count - 1
				domains.Add(objDomains.ComObject.Item(Index).Name)
			Next

			Return domains.ToArray()
		End If

		Return Nothing
	End Function

	Public Function GetDomain(ByVal domainName As String) As MailDomain Implements IMailServer.GetDomain
		Dim objDomain As Service = GetDomainObject(domainName)

		If objDomain.Succeed Then
			Dim domain As MailDomain = New MailDomain()
			domain.Name = objDomain.ComObject.Name
			domain.Enabled = objDomain.ComObject.Active
			domain.CatchAllAccount = GetMailboxName(objDomain.ComObject.Postmaster)
			Return domain
		End If

		Return Nothing
	End Function

	Public Function GetDomainAliases(ByVal domainName As String) As String() Implements IMailServer.GetDomainAliases
		' find existing domain
		Dim objDomain As Service = GetDomainObject(domainName)
		Dim aliases As New List(Of String)

		If objDomain.Succeed Then
			For i As Integer = 0 To objDomain.ComObject.DomainAliases.Count - 1
				aliases.Add(objDomain.ComObject.DomainAliases.Item(i).AliasName)
			Next
		End If

		Return aliases.ToArray()
	End Function

	Public Function GetGroup(ByVal groupName As String) As MailGroup Implements IMailServer.GetGroup
		Dim objDomain As Service = GetDomainObject(GetDomainName(groupName))
		Dim mailGroup As MailGroup = Nothing

		If objDomain.Succeed Then
			Dim mailboxCount As Integer = objDomain.ComObject.Accounts.Count - 1

			For i As Integer = 0 To mailboxCount
				Dim objAccount As Object = objDomain.ComObject.Accounts.Item(i)

				If String.Compare(objAccount.Address, groupName, True) = 0 Then
					If CheckAccountIsGroup(objAccount) Then
						mailGroup = ConvertToMailGroup(objAccount)
						Exit For
					End If
				End If
			Next i
		End If

		Return mailGroup
	End Function

	Public Function GetGroups(ByVal domainName As String) As MailGroup() Implements IMailServer.GetGroups
		Dim objDomain As Service = GetDomainObject(domainName)
		Dim mailGroups As New List(Of MailGroup)

		If objDomain.Succeed Then
			Dim Count As Integer = objDomain.ComObject.Accounts.Count - 1
			For I As Integer = 0 To Count
				Dim objAccount As Object = objDomain.ComObject.Accounts.Item(I)
				If CheckAccountIsGroup(objAccount) Then
					mailGroups.Add(ConvertToMailGroup(objAccount))
				End If
			Next I
		End If

		Return mailGroups.ToArray()
	End Function

	Public Function GetList(ByVal maillistName As String) As MailList Implements IMailServer.GetList
		Dim objDomain As Service = GetDomainObject(GetDomainName(maillistName))
		Dim mailList As MailList = Nothing

		If objDomain.Succeed Then
			Dim objMailList As Object = objDomain.ComObject.DistributionLists.ItemByAddress(maillistName)

			If Not objMailList Is Nothing Then
				mailList = ConvertToMailList(objMailList)
			End If
		End If

		Return mailList
	End Function

	Public Function GetLists(ByVal domainName As String) As MailList() Implements IMailServer.GetLists
		Dim objDomain As Service = GetDomainObject(domainName)

		Dim lists As New List(Of MailList)

		If objDomain.Succeed Then
			Dim mailListCount As Integer = objDomain.ComObject.DistributionLists.Count - 1

			For index As Integer = 0 To mailListCount
				Dim objMailList As Object = objDomain.ComObject.DistributionLists.Item(index)
				lists.Add(ConvertToMailList(objMailList))
			Next index
		End If

		Return lists.ToArray()
	End Function

	Public Function GroupExists(ByVal groupName As String) As Boolean Implements IMailServer.GroupExists
		Dim objDomain As Service = GetDomainObject(GetDomainName(groupName))
		Dim exists As Boolean = False

		If objDomain.Succeed Then
			Try
				Dim objAccount As Object = objDomain.ComObject.Accounts.ItemByAddress(groupName)
				exists = CheckAccountIsGroup(objAccount)
			Catch ex As Exception
				Log.WriteError("Couldn't find mail group.", ex)
			End Try
		End If

		Return exists
	End Function

	Public Function ListExists(ByVal maillistName As String) As Boolean Implements IMailServer.ListExists
		Dim objDomain As Service = GetDomainObject(GetDomainName(maillistName))
		Dim exists As Boolean = False

		If objDomain.Succeed Then
			Try
				Dim objMailList As Object = objDomain.ComObject.DistributionLists.ItemByAddress(maillistName)
				exists = True
			Catch ex As Exception
				Log.WriteError("Couldn't find mail list.", ex)
			End Try
		End If

		Return exists
	End Function

	Public Sub UpdateAccount(ByVal mailbox As MailAccount) Implements IMailServer.UpdateAccount
		Dim objDomain As Service = GetDomainObject(GetDomainName(mailbox.Name))

		If objDomain.Succeed Then
			Try
				' update account
				Dim objAccount As Object = objDomain.ComObject.Accounts.ItemByAddress(mailbox.Name)
				objAccount.Active = mailbox.Enabled
				objAccount.Password = mailbox.Password
				objAccount.MaxSize = mailbox.MaxMailboxSize
				objAccount.VacationMessageIsOn = mailbox.ResponderEnabled
				objAccount.VacationSubject = mailbox.ResponderSubject
				objAccount.VacationMessage = mailbox.ResponderMessage
				'Personal Information
				objAccount.PersonFirstName = mailbox.FirstName
				objAccount.PersonLastName = mailbox.LastName
				'Signature
				objAccount.SignatureEnabled = mailbox.SignatureEnabled
				objAccount.SignaturePlainText = mailbox.Signature
				objAccount.SignatureHTML = mailbox.SignatureHTML

				If mailbox.ForwardingAddresses.Length > 0 Then
					objAccount.ForwardAddress = mailbox.ForwardingAddresses(0)
					objAccount.ForwardKeepOriginal = mailbox.RetainLocalCopy
					objAccount.ForwardEnabled = True
				End If

				objAccount.Save()

				' set account rules
				SetAccountRules(mailbox, objAccount)

			Catch ex As Exception
				Log.WriteError("Couldn't update an account.", ex)
			End Try
		End If
	End Sub

	Public Sub UpdateDomain(ByVal domain As MailDomain) Implements IMailServer.UpdateDomain
		' find existing domain
		Dim objDomain As Service = GetDomainObject(domain.Name)

		If objDomain.Succeed Then
			' update domain
			objDomain.ComObject.Name = domain.Name
			objDomain.ComObject.Active = domain.Enabled
			objDomain.ComObject.Postmaster = String.Concat(domain.CatchAllAccount, "@", domain.Name)
			objDomain.ComObject.Save()
		End If
	End Sub

	Public Sub UpdateGroup(ByVal group As MailGroup) Implements IMailServer.UpdateGroup
		Dim objDomain As Service = GetDomainObject(GetDomainName(group.Name))

		If objDomain.Succeed Then
			Try
				Dim objGroup As Object = objDomain.ComObject.Accounts.ItemByAddress(group.Name)
				If CheckAccountIsGroup(objGroup) Then
					objGroup.Active = group.Enabled

					' group should be empty
					'objGroup.MaxSize = group.DiskSpace
					objGroup.VacationMessageIsOn = False
					objGroup.VacationSubject = String.Empty
					objGroup.VacationMessage = String.Empty
					objGroup.Save()

					Dim groupRuleExists As Boolean = False
					Dim objRule As Object = Nothing

					For i As Integer = 0 To objGroup.Rules.Count - 1
						objRule = objGroup.Rules.Item(i)
						If String.Compare(objRule.Name, MAIL_GROUP_RULE, True) = 0 Then
							groupRuleExists = True
							Exit For
						End If
					Next i

					If Not groupRuleExists Then
						' Create mail group rule
						objRule = objGroup.Rules.Add()
						objRule.AccountID = objGroup.ID
						objRule.Active = True
						objRule.Name = MAIL_GROUP_RULE
						objRule.Save()
					End If

					Dim criteriaExists As Boolean = False
					Dim objCriteria As Object
					' Check for the criteria
					For i As Integer = 0 To objRule.Criterias.Count - 1
						objCriteria = objRule.Criterias.Item(i)

						If objCriteria.PredefinedField = 6 And objCriteria.MatchType = 4 And objCriteria.MatchValue = "0" Then
							criteriaExists = True
							Exit For
						End If
					Next i

					If Not criteriaExists Then
						' Add criteria
						objCriteria = objRule.Criterias.Add()
						objCriteria.RuleID = objRule.ID
						objCriteria.PredefinedField = 6	' hMailServer.eRulePredefinedField.eFTMessageSize
						objCriteria.MatchType = 4 ' hMailServer.eRuleMatchType.eMTGreaterThan
						objCriteria.MatchValue = "0"
						objCriteria.Save()
					End If

					' cleanup previous rule actions
					Do
						objRule.Actions.Delete(0)
					Loop While objRule.Actions.Count > 0

					' Add delete mail action
					Dim objGroupAction As Object = objRule.Actions.Add()
					objGroupAction.RuleID = objRule.ID
					objGroupAction.Type = 1	'eRADeleteEmail
					objGroupAction.Save()

					' create group members
					If Not group.Members Is Nothing Then
						For Each member As String In group.Members
							Dim objGroupMemberAction As Object = objRule.Actions.Add()
							objGroupMemberAction.RuleID = objRule.ID
							objGroupMemberAction.Type = 2 'eRAForwardEmail
							objGroupMemberAction.To = member
							objGroupMemberAction.Save()
						Next member
					End If
				End If
			Catch ex As Exception
				Log.WriteError("Couldn't update specified mail group.", ex)
			End Try
		End If
	End Sub

	Public Sub UpdateList(ByVal maillist As MailList) Implements IMailServer.UpdateList
		Dim objDomain As Service = GetDomainObject(GetDomainName(maillist.Name))

		If objDomain.Succeed Then
			Try
				Dim objMailList As Object = objDomain.ComObject.DistributionLists.ItemByAddress(maillist.Name)
				objMailList.Active = maillist.Enabled

				objMailList.RequireSMTPAuth = maillist.RequireSmtpAuthentication

				Select Case maillist.PostingMode
					Case PostingMode.MembersCanPost
						objMailList.Mode = 1
					Case PostingMode.ModeratorCanPost
						If String.IsNullOrEmpty(maillist.ModeratorAddress) Then
							Throw New Exception("List moderator address doesn't specified.")
						End If
						objMailList.RequireSenderAddress = maillist.ModeratorAddress
						objMailList.Mode = 2
					Case PostingMode.AnyoneCanPost
						objMailList.Mode = 3
				End Select

				objMailList.Save()

				Dim count As Integer = objMailList.Recipients.Count

				' cleanup list members
				' check if list has members to avoid Invalid Index exception
				If objMailList.Recipients.Count > 0 Then
					For i As Integer = 0 To objMailList.Recipients.Count - 1
						Dim objRecipient As Object = objMailList.Recipients.Item(0)
						objRecipient.Delete()
					Next i
				End If


				' save list members
				If Not maillist.Members Is Nothing Then
					For Each member As String In maillist.Members
						Dim objRecipient As Object = objMailList.Recipients.Add()
						objRecipient.RecipientAddress = member
						objRecipient.Save()
					Next member
				End If

			Catch ex As Exception
				Log.WriteError("Couldn't update a mail list.", ex)
			End Try
		End If
	End Sub
#End Region

#Region "HostingServiceProviderBase"

	Public Overrides Sub ChangeServiceItemsState(ByVal items() As ServiceProviderItem, ByVal enabled As Boolean)
		For Each item As ServiceProviderItem In items
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
#End Region

    Public Overrides Function IsInstalled() As Boolean
        Dim displayName As String = ""
        Dim version As String = ""
        Dim key32bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\hMailServer_is1")
        If (key32bit IsNot Nothing) Then
            displayName = CStr(key32bit.GetValue("DisplayName"))
            Dim split As String() = displayName.Split(New [Char]() {" "c})
            version = split(1)
        Else
            Dim key64bit As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\hMailServer_is1")
            If (key64bit IsNot Nothing) Then
                displayName = CStr(key64bit.GetValue("DisplayName"))
                Dim split As String() = displayName.Split(New [Char]() {" "c})
                version = split(1)
            Else
                Return False
            End If
        End If
        If [String].IsNullOrEmpty(version) = False Then
            Dim split As String() = version.Split(New [Char]() {"."c})
            Return (split(0).Equals("4")) And (split(1).Equals("2"))
        Else
            Return False
        End If
    End Function

End Class
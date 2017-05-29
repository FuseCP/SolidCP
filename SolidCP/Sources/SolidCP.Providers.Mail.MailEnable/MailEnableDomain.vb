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

Option Strict Off
Option Explicit On

Namespace SolidCP.Providers.Mail

    Public Class MailEnableDomain
        Inherits MarshalByRefObject

        Private DomainNameVal As String
        Private StatusVal As Integer
        Private DomainRedirectionStatusVal As Integer
        Private DomainRedirectionHostsVal As String
        Private AccountNameVal As String
        Private HostVal As String
        Private RetainModeVal As Integer                    ' this is used to hold messages waiting for pickup by issued ETRN
        Private PollForMessagesVal As Integer               ' this indicates that we need to poll a host for messages for this domain
        Private UpStreamHostVal As String                ' this is the address of the host to poll if we are set to poll a remote host
        Private PollIntervalVal As Integer                  ' this is the frequency in minutes that we need to poll the remote host
        Private AliasModeVal As Integer
        Private AliasNameVal As String

        Private Structure ISMTPDOMAINTYPE
            <VBFixedString(512), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=512)> Public DomainName As String
            Public Status As Integer
            Public DomainRedirectionStatus As Integer
            <VBFixedString(2048), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=2048)> Public DomainRedirectionHosts As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public AccountName As String
            Public RetainMode As Integer ' this is used to hold messages waiting for pickup by issued ETRN
            Public PollForMessages As Integer               ' this indicates that we need to poll a host for messages for this domain
            <VBFixedString(512), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=512)> Public UpStreamHost As String          ' this is the address of the host to poll if we are set to poll a remote host
            Public PollInterval As Integer                  ' this is the frequency in minutes that we need to poll the remote host
            Public AliasMode As Integer
            <VBFixedString(512), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=512)> Public AliasName As String
        End Structure

        Private Declare Function SMTPDomainAdd Lib "MEAISM.DLL" (ByRef SMTPDomain As ISMTPDOMAINTYPE) As Integer
        Private Declare Function SMTPDomainGet Lib "MEAISM.DLL" (ByRef SMTPDomainCriteria As ISMTPDOMAINTYPE) As Integer
        Private Declare Function SMTPDomainEdit Lib "MEAISM.DLL" (ByRef SMTPDomainCriteria As ISMTPDOMAINTYPE, ByRef SMTPDomainData As ISMTPDOMAINTYPE) As Integer
        Private Declare Function SMTPDomainRemove Lib "MEAISM.DLL" (ByRef SMTPDomain As ISMTPDOMAINTYPE) As Integer
        Private Declare Function SMTPDomainFindFirst Lib "MEAISM.DLL" (ByRef SMTPDomain As ISMTPDOMAINTYPE) As Integer
        Private Declare Function SMTPDomainFindNext Lib "MEAISM.DLL" (ByRef SMTPDomain As ISMTPDOMAINTYPE) As Integer
        Private Declare Function SetCurrentHost Lib "MEAISM.DLL" (ByVal CurrentHost As String) As Integer

        Public Function SetHost() As Integer
            SetHost = SetCurrentHost(Host)
        End Function

        Public Function AddDomain() As Integer

            Dim CDomain As ISMTPDOMAINTYPE

            CDomain.AliasMode = 0
            CDomain.AliasName = ""
            CDomain.PollForMessages = 0
            CDomain.PollInterval = 0
            CDomain.RetainMode = 0
            CDomain.UpStreamHost = ""

            CDomain.AccountName = AccountName
            CDomain.DomainName = DomainName
            CDomain.DomainRedirectionHosts = DomainRedirectionHosts
            CDomain.DomainRedirectionStatus = DomainRedirectionStatus
            CDomain.Status = Status

            AddDomain = SMTPDomainAdd(CDomain)

            AccountName = CDomain.AccountName
            DomainName = CDomain.DomainName
            DomainRedirectionHosts = CDomain.DomainRedirectionHosts
            DomainRedirectionStatus = CDomain.DomainRedirectionStatus
            Status = CDomain.Status

            'Console.WriteLine("AddDomain called on server")

        End Function


        Public Function GetDomain() As Integer

            Dim CDomain As ISMTPDOMAINTYPE
            CDomain.AliasMode = -1
            CDomain.AliasName = ""
            CDomain.PollForMessages = -1
            CDomain.PollInterval = -1
            CDomain.RetainMode = -1
            CDomain.UpStreamHost = ""

            CDomain.AccountName = AccountName
            CDomain.DomainName = DomainName
            CDomain.DomainRedirectionHosts = DomainRedirectionHosts
            CDomain.DomainRedirectionStatus = DomainRedirectionStatus
            CDomain.Status = Status

            GetDomain = SMTPDomainGet(CDomain)

            AccountName = CDomain.AccountName
            DomainName = CDomain.DomainName
            DomainRedirectionHosts = CDomain.DomainRedirectionHosts
            DomainRedirectionStatus = CDomain.DomainRedirectionStatus
            Status = CDomain.Status

        End Function


        Public Function EditDomain(ByVal NewDomainName As String, ByVal NewStatus As Integer, ByVal NewDomainRedirectionStatus As Integer, ByVal NewDomainRedirectionHosts As String, ByVal NewAccountName As String) As Integer

            Dim CDomain As ISMTPDOMAINTYPE
            CDomain.AliasMode = -1
            CDomain.AliasName = ""
            CDomain.PollForMessages = -1
            CDomain.PollInterval = -1
            CDomain.RetainMode = -1
            CDomain.UpStreamHost = ""
            Dim CDomainData As ISMTPDOMAINTYPE

            CDomain.AccountName = AccountName
            CDomain.DomainName = DomainName
            CDomain.DomainRedirectionHosts = DomainRedirectionHosts
            CDomain.DomainRedirectionStatus = DomainRedirectionStatus
            CDomain.Status = Status
            CDomainData.AccountName = NewAccountName
            CDomainData.DomainName = NewDomainName
            CDomainData.DomainRedirectionHosts = NewDomainRedirectionHosts
            CDomainData.DomainRedirectionStatus = NewDomainRedirectionStatus
            CDomainData.Status = NewStatus

            CDomainData.AliasMode = 0
            CDomainData.AliasName = ""
            CDomainData.PollForMessages = 0
            CDomainData.PollInterval = 0
            CDomainData.RetainMode = 0
            CDomainData.UpStreamHost = ""

            EditDomain = SMTPDomainEdit(CDomain, CDomainData)

            AccountName = CDomain.AccountName
            DomainName = CDomain.DomainName
            DomainRedirectionHosts = CDomain.DomainRedirectionHosts
            DomainRedirectionStatus = CDomain.DomainRedirectionStatus
            Status = CDomain.Status

        End Function


        Public Function RemoveDomain() As Integer

            Dim CDomain As ISMTPDOMAINTYPE
            CDomain.AliasMode = -1
            CDomain.AliasName = ""
            CDomain.PollForMessages = -1
            CDomain.PollInterval = -1
            CDomain.RetainMode = -1
            CDomain.UpStreamHost = ""

            CDomain.AccountName = AccountName
            CDomain.DomainName = DomainName
            CDomain.DomainRedirectionHosts = DomainRedirectionHosts
            CDomain.DomainRedirectionStatus = DomainRedirectionStatus
            CDomain.Status = Status

            RemoveDomain = SMTPDomainRemove(CDomain)

            AccountName = CDomain.AccountName
            DomainName = CDomain.DomainName
            DomainRedirectionHosts = CDomain.DomainRedirectionHosts
            DomainRedirectionStatus = CDomain.DomainRedirectionStatus
            Status = CDomain.Status

        End Function


        Public Function FindFirstDomain() As Integer

            Dim CDomain As ISMTPDOMAINTYPE
            CDomain.AliasMode = -1
            CDomain.AliasName = ""
            CDomain.PollForMessages = -1
            CDomain.PollInterval = -1
            CDomain.RetainMode = -1
            CDomain.UpStreamHost = ""

            CDomain.AccountName = AccountName
            CDomain.DomainName = DomainName
            CDomain.DomainRedirectionHosts = DomainRedirectionHosts
            CDomain.DomainRedirectionStatus = DomainRedirectionStatus
            CDomain.Status = Status

            FindFirstDomain = SMTPDomainFindFirst(CDomain)

            AccountName = CDomain.AccountName
            DomainName = CDomain.DomainName
            DomainRedirectionHosts = CDomain.DomainRedirectionHosts
            DomainRedirectionStatus = CDomain.DomainRedirectionStatus
            Status = CDomain.Status

        End Function


        Public Function FindNextDomain() As Integer

            Dim CDomain As ISMTPDOMAINTYPE

            CDomain.AccountName = AccountName
            CDomain.DomainName = DomainName
            CDomain.DomainRedirectionHosts = DomainRedirectionHosts
            CDomain.DomainRedirectionStatus = DomainRedirectionStatus
            CDomain.Status = Status


            CDomain.AliasMode = -1
            CDomain.AliasName = ""
            CDomain.PollForMessages = -1
            CDomain.PollInterval = -1
            CDomain.RetainMode = -1
            CDomain.UpStreamHost = ""

            FindNextDomain = SMTPDomainFindNext(CDomain)

            AccountName = CDomain.AccountName
            DomainName = CDomain.DomainName
            DomainRedirectionHosts = CDomain.DomainRedirectionHosts
            DomainRedirectionStatus = CDomain.DomainRedirectionStatus
            Status = CDomain.Status

        End Function

        Public Property DomainName() As String
            Get
                Return Me.DomainNameVal
            End Get
            Set(ByVal Value As String)
                Me.DomainNameVal = Value
            End Set
        End Property

        Public Property Status() As Integer
            Get
                Return Me.StatusVal
            End Get
            Set(ByVal Value As Integer)
                Me.StatusVal = Value
            End Set
        End Property

        Public Property DomainRedirectionStatus() As Integer
            Get
                Return Me.DomainRedirectionStatusVal
            End Get
            Set(ByVal Value As Integer)
                Me.DomainRedirectionStatusVal = Value
            End Set
        End Property

        Public Property DomainRedirectionHosts() As String
            Get
                Return Me.DomainRedirectionHostsVal
            End Get
            Set(ByVal Value As String)
                Me.DomainRedirectionHostsVal = Value
            End Set
        End Property

        Public Property AccountName() As String
            Get
                Return Me.AccountNameVal
            End Get
            Set(ByVal Value As String)
                Me.AccountNameVal = Value
            End Set
        End Property

        Public Property Host() As String
            Get
                Return Me.HostVal
            End Get
            Set(ByVal Value As String)
                Me.HostVal = Value
            End Set
        End Property

        Public Property RetainMode() As Integer
            Get
                Return Me.RetainModeVal
            End Get
            Set(ByVal Value As Integer)
                Me.RetainModeVal = Value
            End Set
        End Property

        Public Property PollForMessages() As Integer
            Get
                Return Me.PollForMessagesVal
            End Get
            Set(ByVal Value As Integer)
                Me.PollForMessagesVal = Value
            End Set
        End Property

        Public Property UpStreamHost() As String
            Get
                Return Me.UpStreamHostVal
            End Get
            Set(ByVal Value As String)
                Me.UpStreamHostVal = Value
            End Set
        End Property

        Public Property PollInterval() As Integer
            Get
                Return Me.PollIntervalVal
            End Get
            Set(ByVal Value As Integer)
                Me.PollIntervalVal = Value
            End Set
        End Property

        Public Property AliasMode() As Integer
            Get
                Return Me.AliasModeVal
            End Get
            Set(ByVal Value As Integer)
                Me.AliasModeVal = Value
            End Set
        End Property

        Public Property AliasName() As String
            Get
                Return Me.AliasNameVal
            End Get
            Set(ByVal Value As String)
                Me.AliasNameVal = Value
            End Set
        End Property

        Private Function CString(ByVal InString As String) As String
            CString = InString & Chr(0)
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

        Public Function Exists(ByVal DomainName As String) As Boolean

            Dim CDomain As New ISMTPDOMAINTYPE

            CDomain.AccountName = CString("")
            CDomain.DomainName = CString(DomainName)
            CDomain.DomainRedirectionHosts = CString("")
            CDomain.DomainRedirectionStatus = -1
            CDomain.Status = -1

            Exists = (SMTPDomainGet(CDomain) = 1)

        End Function

        Private Sub AddDataTableColumns(ByRef oTable As DataTable)
            oTable.Columns.Add("DomainName", GetType(String))
            oTable.Columns.Add("Status", GetType(Long))
            oTable.Columns.Add("RedirectionStatus", GetType(Long))
            oTable.Columns.Add("RedirectionHosts", GetType(String))
        End Sub

    End Class
End Namespace

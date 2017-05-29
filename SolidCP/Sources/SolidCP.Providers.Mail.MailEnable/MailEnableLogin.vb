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

Namespace SolidCP.Providers.Mail

    Public Class MailEnableLogin
        Inherits MarshalByRefObject

        Private UserNameVal As String
        Private StatusVal As Long
        Private PasswordVal As String
        Private AccountVal As String
        Private DescriptionVal As String
        Private LoginAttemptsVal As Long
        Private LastAttemptVal As Long
        Private LastSuccessfulLoginVal As Long
        Private RightsVal As String
        Private HostVal As String

        Private Structure IAUTHENTRYTYPE
            <VBFixedString(64), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=64)> Public UserName As String
            Public Status As Integer
            <VBFixedString(64), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=64)> Public Password As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public Account As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public Rights As String
            <VBFixedString(1024), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=1024)> Public Description As String
            Public LoginAttempts As Integer
            Public LastAttempt As Integer
            Public LastSuccessfulLogin As Integer
        End Structure

        Private Declare Function GetLastProviderErrorCode Lib "MEAIAU.DLL" Alias "GetLastErrorCode" () As Integer
        Private Declare Function LoginGet Lib "MEAIAU.DLL" (ByRef lpLogin As IAUTHENTRYTYPE) As Integer
        Private Declare Function LoginFindFirst Lib "MEAIAU.DLL" (ByRef lpLogin As IAUTHENTRYTYPE) As Integer
        Private Declare Function LoginFindNext Lib "MEAIAU.DLL" (ByRef lpLogin As IAUTHENTRYTYPE) As Integer
        Private Declare Function LoginAdd Lib "MEAIAU.DLL" (ByRef lpLogin As IAUTHENTRYTYPE) As Integer
        Private Declare Function LoginEdit Lib "MEAIAU.DLL" (ByRef TargetLogin As IAUTHENTRYTYPE, ByRef NewLogin As IAUTHENTRYTYPE) As Integer
        Private Declare Function LoginRemove Lib "MEAIAU.DLL" (ByRef lpLogin As IAUTHENTRYTYPE) As Integer
        Private Declare Function SetCurrentHost Lib "MEAIAU.DLL" (ByVal CurrentHost As String) As Integer

        Public Function GetLastErrorCode() As Integer
            Return GetLastProviderErrorCode()
        End Function

        Public Function SetHost() As Integer
            SetHost = SetCurrentHost(Host)
        End Function

        Public Function GetLogin() As Integer
            Dim CLogin As IAUTHENTRYTYPE
            CLogin.UserName = UserName
            CLogin.Password = Password
            CLogin.Status = Status
            CLogin.Account = Account
            CLogin.Description = Description
            CLogin.Rights = Rights
            GetLogin = LoginGet(CLogin)
            UserName = CLogin.UserName
            Password = CLogin.Password
            Status = CLogin.Status
            Account = CLogin.Account
            Description = CLogin.Description
            Rights = CLogin.Rights
        End Function

        Public Function FindFirstLogin() As Integer

            Dim CLogin As IAUTHENTRYTYPE

            CLogin.UserName = UserName
            CLogin.Password = Password
            CLogin.Status = Status
            CLogin.Account = Account
            CLogin.Description = Description
            CLogin.Rights = Rights
            FindFirstLogin = LoginFindFirst(CLogin)
            UserName = CLogin.UserName
            Password = CLogin.Password
            Status = CLogin.Status
            Account = CLogin.Account
            Description = CLogin.Description
            Rights = CLogin.Rights
        End Function

        Public Function FindNextLogin() As Integer
            Dim CLogin As IAUTHENTRYTYPE
            CLogin.UserName = UserName
            CLogin.Password = Password
            CLogin.Status = Status
            CLogin.Account = Account
            CLogin.Description = Description
            CLogin.Rights = Rights
            FindNextLogin = LoginFindNext(CLogin)
            UserName = CLogin.UserName
            Password = CLogin.Password
            Status = CLogin.Status
            Account = CLogin.Account
            Description = CLogin.Description
            Rights = CLogin.Rights
        End Function

        Public Function AddLogin() As Integer
            Dim CLogin As IAUTHENTRYTYPE
            CLogin.UserName = UserName
            CLogin.Password = Password
            CLogin.Status = Status
            CLogin.Account = Account
            CLogin.Description = Description
            CLogin.Rights = Rights
            AddLogin = LoginAdd(CLogin)
            UserName = CLogin.UserName
            Password = CLogin.Password
            Status = CLogin.Status
            Account = CLogin.Account
            Description = CLogin.Description
            Rights = CLogin.Rights
        End Function

        Public Function RemoveLogin() As Integer
            Dim CLogin As IAUTHENTRYTYPE
            CLogin.UserName = UserName
            CLogin.Password = Password
            CLogin.Status = Status
            CLogin.Account = Account
            CLogin.Description = Description
            CLogin.Rights = Rights
            RemoveLogin = LoginRemove(CLogin)
        End Function

        Public Function EditLogin(ByVal NewUserName As String, ByVal NewStatus As Long, ByVal NewPassword As String, ByVal NewAccount As String, ByVal NewDescription As String, ByVal NewLoginAttempts As Long, ByVal NewLastAttempt As Long, ByVal NewLastSuccessfulLogin As Long, ByVal NewRights As String) As Integer
            Dim CLogin As IAUTHENTRYTYPE
            Dim CLoginData As IAUTHENTRYTYPE
            CLogin.UserName = UserName
            CLogin.Password = Password
            CLogin.Status = Status
            CLogin.Account = Account
            CLogin.Description = Description
            CLogin.Rights = Rights
            '
            CLoginData.UserName = NewUserName
            CLoginData.Password = NewPassword
            CLoginData.Status = NewStatus
            CLoginData.Account = NewAccount
            CLoginData.Description = NewDescription
            CLoginData.Rights = NewRights
            EditLogin = LoginEdit(CLogin, CLoginData)
        End Function

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

        Public Function Exists(ByVal sUsername As String) As Boolean
            Dim CLogin As IAUTHENTRYTYPE
            CLogin.UserName = CString(sUsername)
            CLogin.Password = CString("")
            CLogin.Status = -1
            CLogin.Account = CString("")
            CLogin.Description = CString("")
            CLogin.Rights = CString("")
            Exists = (LoginGet(CLogin) = 1)
        End Function


        Public Property UserName() As String
            Get
                Return Me.UserNameVal
            End Get
            Set(ByVal value As String)
                Me.UserNameVal = value
            End Set
        End Property

        Public Property Status() As Long
            Get
                Return Me.StatusVal
            End Get
            Set(ByVal value As Long)
                Me.StatusVal = value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return Me.PasswordVal
            End Get
            Set(ByVal value As String)
                Me.PasswordVal = value
            End Set
        End Property

        Public Property Account() As String
            Get
                Return Me.AccountVal
            End Get
            Set(ByVal value As String)
                Me.AccountVal = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Me.DescriptionVal
            End Get
            Set(ByVal value As String)
                Me.DescriptionVal = value
            End Set
        End Property

        Public Property LoginAttempts() As Long
            Get
                Return Me.LoginAttemptsVal
            End Get
            Set(ByVal value As Long)
                Me.LoginAttemptsVal = value
            End Set
        End Property

        Public Property LastAttempt() As Long
            Get
                Return Me.LastAttemptVal
            End Get
            Set(ByVal value As Long)
                Me.LastAttemptVal = value
            End Set
        End Property

        Public Property LastSuccessfulLogin() As Long
            Get
                Return Me.LastSuccessfulLoginVal
            End Get
            Set(ByVal value As Long)
                Me.LastSuccessfulLoginVal = value
            End Set
        End Property

        Public Property Rights() As String
            Get
                Return Me.RightsVal
            End Get
            Set(ByVal value As String)
                Me.RightsVal = value
            End Set
        End Property

        Public Property Host() As String
            Get
                Return Me.HostVal
            End Get
            Set(ByVal value As String)
                Me.HostVal = value
            End Set
        End Property
    End Class

End Namespace

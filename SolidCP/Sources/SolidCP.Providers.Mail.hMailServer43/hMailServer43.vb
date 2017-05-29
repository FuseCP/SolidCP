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

Public Class hMailServer43
	Inherits hMailServer

#Region "Public Properties"
	Public ReadOnly Property AdminUsername() As String
		Get
			Return ProviderSettings("AdminUsername")
		End Get
	End Property

	Public ReadOnly Property AdminPassword() As String
		Get
			Return ProviderSettings("AdminPassword")
		End Get
	End Property
#End Region

	Protected Overrides ReadOnly Property hMailServer() As Object
		Get
			Dim svc As Object = MyBase.hMailServer

			' Authenticate API
			Dim account As Object = svc.Authenticate(AdminUsername, AdminPassword)
			If account Is Nothing Then
				Throw New Exception("Could not authenticate using administrator credentials provided")
			End If

			Return svc
		End Get
    End Property

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
            Return (split(0).Equals("4")) And (Integer.Parse(split(1)) > 2)
        Else
            Return False
        End If
    End Function


End Class

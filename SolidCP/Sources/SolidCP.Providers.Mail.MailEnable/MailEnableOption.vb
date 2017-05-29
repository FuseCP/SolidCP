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

    Public Class MailEnableOption
        Inherits MarshalByRefObject

        Private ScopeVal As Integer
        Private QueryVal As String
        Private ValueNameVal As String
        Private ValueVal As String
        Private HostVal As String

        Private Declare Function SystemGetOption Lib "MEAISO.DLL" Alias "GetOption" (ByVal Scope As Integer, ByVal Query As String, ByVal ValueName As String, ByVal ReturnValue As String) As Int32
        Private Declare Function SystemSetOption Lib "MEAISO.DLL" Alias "SetOption" (ByVal Scope As Integer, ByVal Query As String, ByVal ValueName As String, ByVal ReturnValue As String) As Int32
        Private Declare Function SetCurrentHost Lib "MEAISO.DLL" (ByVal CurrentHost As String) As Int32

        Private Shared Function NonCString(ByVal InString As String) As String
            Dim NTPos As Integer
            NTPos = InStr(1, InString, Chr(0), CompareMethod.Binary)
            If NTPos > 0 Then
                NonCString = Left(InString, NTPos - 1)
            Else
                NonCString = InString
            End If
        End Function

        Private Shared Function CString(ByVal InString As String) As String
            CString = InString & Chr(0)
        End Function

        Public Function SetHost() As Integer
            SetHost = SetCurrentHost(Host)
        End Function

        Private Function ValidData() As Boolean
            If Len(Query) >= 1024 Then ValidData = False : Exit Function
            If Len(ValueName) >= 1024 Then ValidData = False : Exit Function
            If Len(Value) >= 1024 Then ValidData = False : Exit Function
            ValidData = True
        End Function


        Public Function GetOption() As Integer
            If Not ValidData() Then GetOption = 0 : Exit Function

            Value = Space(2048)
            Try
                GetOption = SystemGetOption(Scope, Query, ValueName, Value)
                Value = NonCString(Value)
            Catch ex As Exception
                GetOption = 0
                Value = String.Empty
            End Try
        End Function

        Public Function SetOption() As Integer
            If Not ValidData() Then SetOption = 0 : Exit Function

            SetOption = SystemSetOption(Scope, Query, ValueName, Value)
        End Function


        Public Property Scope() As Integer
            Get
                Return Me.ScopeVal
            End Get
            Set(ByVal value As Integer)
                Me.ScopeVal = value
            End Set
        End Property

        Public Property Query() As String
            Get
                Return Me.QueryVal
            End Get
            Set(ByVal value As String)
                Me.QueryVal = value
            End Set
        End Property

        Public Property ValueName() As String
            Get
                Return Me.ValueNameVal
            End Get
            Set(ByVal value As String)
                Me.ValueNameVal = value
            End Set
        End Property

        Public Property Value() As String
            Get
                Return Me.ValueVal
            End Get
            Set(ByVal value As String)
                Me.ValueVal = value
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


        Public Shared Function GetSystemOption(ByRef lScope As String, ByRef sQuery As String, ByRef sValueName As String, Optional ByVal DefaultValue As String = "") As String

            Dim oMEAOSO As New MailEnableOption

            On Error Resume Next
            oMEAOSO.Scope = lScope
            oMEAOSO.Query = sQuery
            oMEAOSO.ValueName = sValueName
            If oMEAOSO.GetOption() <> 0 Then
                GetSystemOption = oMEAOSO.Value
            Else
                GetSystemOption = DefaultValue
            End If
            oMEAOSO = Nothing

        End Function

        Public Shared Function SetSystemOption(ByRef lScope As String, ByRef sQuery As String, ByRef sValueName As String, ByRef sValue As String) As Integer

            On Error Resume Next

            If Not IsNothing(sValue) Then
                Dim oMEAOSO As New MailEnableOption

                oMEAOSO.Scope = lScope
                oMEAOSO.Query = sQuery
                oMEAOSO.ValueName = sValueName
                oMEAOSO.Value = sValue
                oMEAOSO.SetOption()
                SetSystemOption = True

                oMEAOSO = Nothing
                Return 1
            End If

            Return 0

        End Function

        Public Shared Function GetRemoteRegistryString(ByVal HostName As String, ByVal ParentKey As String, ByVal RegistryKey As String, ByRef KeyValue As String) As Integer
            If HostName Is Nothing Then
                HostName = ""
            End If
            Dim LocalValue As String = Space(4096)
            Dim Result As Integer = _GetRemoteRegistryString(HostName, ParentKey, RegistryKey, LocalValue)
            If Result = 1 Then
                KeyValue = NonCString(LocalValue)
            End If
            Return Result
        End Function

        Private Declare Function GetRemoteRegistryWord Lib "MEAISO.DLL" (ByVal HostName As String, ByVal ParentKey As String, ByVal RegistryKey As String, ByVal KeyValue As Integer) As Integer
        Private Declare Function SetRemoteRegistryWord Lib "MEAISO.DLL" (ByVal HostName As String, ByVal ParentKey As String, ByVal RegistryKey As String, ByVal KeyValue As Integer) As Integer
        Private Declare Function SetRemoteRegistryString Lib "MEAISO.DLL" (ByVal HostName As String, ByVal ParentKey As String, ByVal RegistryKey As String, ByVal KeyValue As String) As Integer
        Private Declare Function _GetRemoteRegistryString Lib "MEAISO.DLL" Alias "GetRemoteRegistryString" (ByVal HostName As String, ByVal ParentKey As String, ByVal RegistryKey As String, ByVal KeyValue As String) As Integer

    End Class

End Namespace

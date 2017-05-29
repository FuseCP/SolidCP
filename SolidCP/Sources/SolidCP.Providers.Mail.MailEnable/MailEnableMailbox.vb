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

Imports System.IO
Imports Microsoft.Win32

Namespace SolidCP.Providers.Mail

    Public Class MailEnableMailbox
        Inherits MarshalByRefObject

        Private PostofficeVal As String
        Private MailboxNameVal As String
        Private RedirectAddressVal As String
        Private RedirectStatusVal As Long
        Private StatusVal As Long
        Private LimitVal As Long
        Private SizeVal As Long
        Private HostVal As String


        Private Structure IMAILBOXTYPE
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public Postoffice As String
            <VBFixedString(64), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=64)> Public Mailbox As String
            <VBFixedString(512), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=512)> Public RedirectAddress As String
            Public RedirectStatus As Integer
            Public Status As Integer
            Public Limit As Integer
            Public Size As Integer
        End Structure


        Private Declare Function MailboxGet Lib "MEAIPO.DLL" (ByRef lpMailbox As IMAILBOXTYPE) As Integer
        Private Declare Function MailboxGetLength Lib "MEAIPO.DLL" (ByRef lpMailbox As IMAILBOXTYPE) As Integer
        Private Declare Function MailboxFindFirst Lib "MEAIPO.DLL" (ByRef lpMailbox As IMAILBOXTYPE) As Integer
        Private Declare Function MailboxFindNext Lib "MEAIPO.DLL" (ByRef lpMailbox As IMAILBOXTYPE) As Integer
        Private Declare Function MailboxAdd Lib "MEAIPO.DLL" (ByRef lpMailbox As IMAILBOXTYPE) As Integer
        Private Declare Function MailboxEdit Lib "MEAIPO.DLL" (ByRef TargetMailbox As IMAILBOXTYPE, ByRef NewMailbox As IMAILBOXTYPE) As Integer
        Private Declare Function MailboxRemove Lib "MEAIPO.DLL" (ByRef lpMailbox As IMAILBOXTYPE) As Integer

        Private Declare Function SetCurrentHost Lib "MEAIPO.DLL" (ByVal CurrentHost As String) As Integer

        Private Structure IANNOTATIONTYPE
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public AnnotationName As String
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public AccountName As String
            <VBFixedString(8192), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=8192)> Public AnnotationText As String
        End Structure

        Private Declare Function AnnotationGet Lib "MEAILS.DLL" (ByRef lpAnnotation As IANNOTATIONTYPE) As Integer
        Private Declare Function AnnotationAdd Lib "MEAILS.DLL" (ByRef lpAnnotation As IANNOTATIONTYPE) As Integer
        Private Declare Function AnnotationRemove Lib "MEAILS.DLL" (ByRef lpAnnotation As IANNOTATIONTYPE) As Integer


        Public Function SetHost() As Integer
            SetHost = SetCurrentHost(Host)
        End Function

        Public Function FindFirstMailbox() As Integer
            Dim CMailBox As IMAILBOXTYPE
            CMailBox.Mailbox = MailboxName
            CMailBox.Postoffice = Postoffice
            CMailBox.RedirectAddress = RedirectAddress
            CMailBox.RedirectStatus = RedirectStatus
            CMailBox.Status = Status
            CMailBox.Limit = Limit
            CMailBox.Size = Size
            FindFirstMailbox = MailboxFindFirst(CMailBox)
            MailboxName = CMailBox.Mailbox
            Postoffice = CMailBox.Postoffice
            RedirectAddress = CMailBox.RedirectAddress
            RedirectStatus = CMailBox.RedirectStatus
            Status = CMailBox.Status
            Limit = CMailBox.Limit
            Size = CMailBox.Size
        End Function


        Public Function FindNextMailbox() As Integer
            Dim CMailBox As IMAILBOXTYPE
            CMailBox.Mailbox = MailboxName
            CMailBox.Postoffice = Postoffice
            CMailBox.RedirectAddress = RedirectAddress
            CMailBox.RedirectStatus = RedirectStatus
            CMailBox.Status = Status
            CMailBox.Limit = Limit
            CMailBox.Size = Size
            FindNextMailbox = MailboxFindNext(CMailBox)
            MailboxName = CMailBox.Mailbox
            Postoffice = CMailBox.Postoffice
            RedirectAddress = CMailBox.RedirectAddress
            RedirectStatus = CMailBox.RedirectStatus
            Status = CMailBox.Status
            Limit = CMailBox.Limit
            Size = CMailBox.Size
        End Function

        Public Function AddMailbox() As Integer
            Dim CMailBox As IMAILBOXTYPE
            CMailBox.Mailbox = MailboxName
            CMailBox.Postoffice = Postoffice
            CMailBox.RedirectAddress = RedirectAddress
            CMailBox.RedirectStatus = RedirectStatus
            CMailBox.Status = Status
            CMailBox.Limit = Limit
            CMailBox.Size = Size
            AddMailbox = MailboxAdd(CMailBox)
            MailboxName = CMailBox.Mailbox
            Postoffice = CMailBox.Postoffice
            RedirectAddress = CMailBox.RedirectAddress
            RedirectStatus = CMailBox.RedirectStatus
            Status = CMailBox.Status
            Limit = CMailBox.Limit
            Size = CMailBox.Size
        End Function

        Public Function GetMailbox() As Integer
            Dim CMailBox As IMAILBOXTYPE
            CMailBox.Mailbox = MailboxName
            CMailBox.Postoffice = Postoffice
            CMailBox.RedirectAddress = RedirectAddress
            CMailBox.RedirectStatus = RedirectStatus
            CMailBox.Status = Status
            CMailBox.Limit = Limit
            CMailBox.Size = Size
            GetMailbox = MailboxGet(CMailBox)
            MailboxName = CMailBox.Mailbox
            Postoffice = CMailBox.Postoffice
            RedirectAddress = CMailBox.RedirectAddress
            RedirectStatus = CMailBox.RedirectStatus
            Status = CMailBox.Status
            Limit = CMailBox.Limit
            Size = CMailBox.Size
        End Function


        Public Function RemoveMailbox() As Integer
            Dim CMailBox As IMAILBOXTYPE
            CMailBox.Mailbox = MailboxName
            CMailBox.Postoffice = Postoffice
            CMailBox.RedirectAddress = RedirectAddress
            CMailBox.RedirectStatus = RedirectStatus
            CMailBox.Status = Status
            CMailBox.Limit = Limit
            CMailBox.Size = Size
            RemoveMailbox = MailboxRemove(CMailBox)
            MailboxName = CMailBox.Mailbox
            Postoffice = CMailBox.Postoffice
            RedirectAddress = CMailBox.RedirectAddress
            RedirectStatus = CMailBox.RedirectStatus
            Status = CMailBox.Status
            Limit = CMailBox.Limit
            Size = CMailBox.Size
        End Function


        Public Function EditMailbox(ByVal NewPostoffice As String, ByVal NewMailbox As String, ByVal NewRedirectAddress As String, ByVal NewRedirectStatus As Long, ByVal NewStatus As Long, ByVal NewLimit As Long, ByVal NewSize As Long) As Integer
            Dim CMailBox As IMAILBOXTYPE
            Dim CMailBoxData As IMAILBOXTYPE
            ' Get the Find Stuff Set up
            CMailBox.Mailbox = MailboxName
            CMailBox.Postoffice = Postoffice
            CMailBox.RedirectAddress = RedirectAddress
            CMailBox.RedirectStatus = RedirectStatus
            CMailBox.Status = Status
            CMailBox.Limit = Limit
            CMailBox.Size = Size
            ' Get the Data Set up
            CMailBoxData.Mailbox = NewMailbox
            CMailBoxData.Postoffice = NewPostoffice
            CMailBoxData.RedirectAddress = NewRedirectAddress
            CMailBoxData.RedirectStatus = NewRedirectStatus
            CMailBoxData.Status = NewStatus
            CMailBoxData.Limit = NewLimit
            CMailBoxData.Size = NewSize
            EditMailbox = MailboxEdit(CMailBox, CMailBoxData)
            MailboxName = CMailBox.Mailbox
            Postoffice = CMailBox.Postoffice
            RedirectAddress = CMailBox.RedirectAddress
            RedirectStatus = CMailBox.RedirectStatus

        End Function

        Public Function GetLength() As Integer
            Dim CMailBox As IMAILBOXTYPE
            CMailBox.Mailbox = MailboxName
            CMailBox.Postoffice = Postoffice
            CMailBox.RedirectAddress = RedirectAddress
            CMailBox.RedirectStatus = RedirectStatus
            CMailBox.Status = Status
            CMailBox.Limit = Limit
            CMailBox.Size = Size
            Return MailboxGetLength(CMailBox)
        End Function


        Public Function SetAutoResponderStatus(ByVal bEnabled As Boolean) As Integer
            Return SetAutoResponderStatus(Me.Postoffice, Me.MailboxName, bEnabled)
        End Function

        Public Shared Function SetAutoResponderStatus(ByVal sPostoffice As String, ByVal sMailbox As String, ByVal bEnabled As Boolean) As Integer
            '
            ' This function copies the Auto Responder Config File to indicate Enabled.
            ' It deletes the CFG file to Disable
            '
            Dim SourcePath As String
            Dim TargetPath As String

            On Error Resume Next

            If (bEnabled) Then
                '
                ' Copy AUTORESPOND.CF_ to AUTORESPOND.CFG
                '
                SourcePath = MailEnable.GetPostofficesPath() & "\" & sPostoffice & "\MAILROOT\" & sMailbox & "\AUTORESPOND.CF_"
                TargetPath = MailEnable.GetPostofficesPath() & "\" & sPostoffice & "\MAILROOT\" & sMailbox & "\AUTORESPOND.CFG"

                If Not File.Exists(SourcePath) Then
                    Return 1
                End If

                File.Copy(SourcePath, TargetPath, True)
                SetAutoResponderStatus = 1
            Else
                SourcePath = MailEnable.GetPostofficesPath() & "\" & sPostoffice & "\MAILROOT\" & sMailbox & "\AUTORESPOND.CFG"

                If Not File.Exists(SourcePath) Then
                    Return 1
                End If

                File.Delete(SourcePath)
                SetAutoResponderStatus = 1
            End If

        End Function


        Public Function SetAutoResponderContents(ByVal Headers As String, ByVal MessageBody As String) As Integer
            Return SetAutoResponderContents(Me.Postoffice, Me.MailboxName, Headers, MessageBody)
        End Function

        Public Shared Function SetAutoResponderContents(ByVal sPostoffice As String, ByVal sMailbox As String, ByVal Headers As String, ByVal MessageBody As String) As Integer
            '
            ' This function sets the contents of the autoresponse.
            ' The SetAutoResponderStatus routine must be called after this to make the changes effective
            '
            Dim SourcePath As String

            SourcePath = MailEnable.GetPostofficesPath() & "\" & sPostoffice & "\MAILROOT\" & sMailbox & "\AUTORESPOND.CF_"

            On Error Resume Next

            Dim oTS As StreamWriter = New StreamWriter(SourcePath, False)

            If Err.Number = 0 Then
                oTS.Write(Headers & vbCrLf & vbCrLf & MessageBody)
                oTS.Close()
                SetAutoResponderContents = 1
            Else
                SetAutoResponderContents = 0
            End If

        End Function

        Public Function GetAutoResponderSubject() As String
            '
            ' This Routine returns the current AutoResponder Subject
            '
            Dim FileContents As String
            Dim DelimiterPos As Long
            Dim StartPos As Long
            Dim EndPos As Long
            Dim SubjectOffset As Long
            Dim SourcePath As String

            SourcePath = MailEnable.GetPostofficesPath() & "\" & Me.Postoffice & "\MAILROOT\" & Me.MailboxName & "\AUTORESPOND.CF_"

            On Error Resume Next

            If Not File.Exists(SourcePath) Then
                Return ""
            End If

            Dim oTS As StreamReader = New StreamReader(SourcePath)

            FileContents = oTS.ReadToEnd
            oTS.Close()

            DelimiterPos = InStr(1, FileContents, "Subject:")

            If DelimiterPos > 0 Then
                SubjectOffset = Len("Subject: ")
                StartPos = DelimiterPos + SubjectOffset
                EndPos = InStr(DelimiterPos, FileContents, vbCrLf, )
                GetAutoResponderSubject = Mid(FileContents, StartPos, (EndPos - StartPos))
            Else
                GetAutoResponderSubject = FileContents
            End If

        End Function

        Public Function GetAutoResponderContents() As String
            '
            ' This Routine returns the current AutoResponder Message
            '
            Dim FileContents As String
            Dim DelimiterPos As Long
            Dim SourcePath As String

            SourcePath = MailEnable.GetPostofficesPath() & "\" & Me.Postoffice & "\MAILROOT\" & Me.MailboxName & "\AUTORESPOND.CF_"

            On Error Resume Next

            If Not File.Exists(SourcePath) Then
                Return ""
            End If

            Dim oTS As StreamReader = New StreamReader(SourcePath)

            FileContents = oTS.ReadToEnd
            oTS.Close()

            DelimiterPos = InStr(1, FileContents, vbCrLf & vbCrLf)

            If DelimiterPos > 0 Then
                ' It has headers
                ' Headers = Mid(FileContents, 1, DelimiterPos + Len(vbCrLf & vbCrLf))
                GetAutoResponderContents = Mid$(FileContents, DelimiterPos + Len(vbCrLf & vbCrLf))
            Else
                'Headers = ""
                GetAutoResponderContents = FileContents
            End If

        End Function

        Public Function GetAutoResponderStatus() As Boolean
            '
            ' This Routine returns the current AutoResponder Status
            '
            Dim SourcePath As String

            SourcePath = MailEnable.GetPostofficesPath() & "\" & Me.Postoffice & "\MAILROOT\" & Me.MailboxName & "\AUTORESPOND.CFG"

            On Error Resume Next

            GetAutoResponderStatus = System.IO.File.Exists(SourcePath)

        End Function

      

        Function SetSignature(ByVal Postoffice As String, ByVal Mailbox As String, ByVal SignitureText As String) As Integer
            Dim CAnnotation As IANNOTATIONTYPE
            CAnnotation.AccountName = Postoffice
            CAnnotation.AnnotationName = Mailbox & "-AUTOSIG"
            CAnnotation.AnnotationText = SignitureText
            SetSignature = AnnotationAdd(CAnnotation)
        End Function

        Function GetSignature(ByVal Postoffice As String, ByVal Mailbox As String) As String

            Dim CAnnotation As IANNOTATIONTYPE

            CAnnotation.AccountName = Postoffice
            CAnnotation.AnnotationName = Mailbox & "-AUTOSIG"
            CAnnotation.AnnotationText = ""

            If (AnnotationGet(CAnnotation) = 1) Then
                GetSignature = CAnnotation.AnnotationText
            Else
                GetSignature = ""
            End If
        End Function


        Public Property Postoffice() As String
            Get
                Return Me.PostofficeVal
            End Get
            Set(ByVal value As String)
                Me.PostofficeVal = value
            End Set
        End Property

        Public Property MailboxName() As String
            Get
                Return Me.MailboxNameVal
            End Get
            Set(ByVal value As String)
                Me.MailboxNameVal = value
            End Set
        End Property

        Public Property RedirectAddress() As String
            Get
                Return Me.RedirectAddressVal
            End Get
            Set(ByVal value As String)
                Me.RedirectAddressVal = value
            End Set
        End Property

        Public Property RedirectStatus() As Long
            Get
                Return Me.RedirectStatusVal
            End Get
            Set(ByVal value As Long)
                Me.RedirectStatusVal = value
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

        Public Property Limit() As Long
            Get
                Return Me.LimitVal
            End Get
            Set(ByVal value As Long)
                Me.LimitVal = value
            End Set
        End Property

        Public Property Size() As Long
            Get
                Return Me.SizeVal
            End Get
            Set(ByVal value As Long)
                Me.SizeVal = value
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
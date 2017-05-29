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

    Public Class MailEnableList
        Inherits MarshalByRefObject

        Private DescriptionVal As String
        Private AccountNameVal As String
        Private ListNameVal As String
        Private ListTypeVal As Long
        Private ListStatusVal As Long
        Private ModeratorAddressVal As String
        Private HeaderAnnotationStatusVal As Long
        Private HeaderAnnotationVal As String
        Private FooterAnnotationStatusVal As Long
        Private FooterAnnotationVal As String
        Private ListAddressVal As String

        '
        ' New Fields
        '
        Private SubscribeMessageFileStatusVal As Long
        Private SubscribeMessageFileVal As String
        Private UnsubscribeMessageFileStatusVal As Long
        Private UnsubscribeMessageFileVal As String
        Private SubjectSuffixStatusVal As Long
        Private SubjectSuffixVal As String
        Private SubjectPrefixStatusVal As Long
        Private SubjectPrefixVal As String
        Private OwnerVal As String
        Private HelpMessageFileStatusVal As Long
        Private HelpMessageFileVal As String
        Private RemovalMessageFileStatusVal As Long
        Private RemovalMessageFileVal As String
        Private ReplyToModeVal As Long
        Private MaxMessageSizeVal As Long
        Private PostingModeVal As Long
        Private SubScriptionModeVal As Long
        Private AuthenticationModeVal As Long
        Private PasswordVal As String
        Private DigestModeVal As Long
        Private DigestMailboxVal As String
        Private DigestAnnotationModeVal As Long
        Private DigestAttachmentModeVal As Long
        Private DigestMessageSeparationModeVal As Long
        Private DigestSchedulingStatusVal As Long
        Private DigestSchedulingModeVal As Long
        Private DigestSchedulingIntervalVal As Long
        Private FromAddressModeVal As Long

        '
        ' Host Fields
        '
        Public Host As String

        Private Structure ILISTTYPE
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public ListName As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public AccountName As String
            Public ListType As Integer
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public ModeratorAddress As String
            Public ListStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public Description As String
            Public HeaderAnnotationStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public HeaderAnnotation As String
            Public FooterAnnotationStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public FooterAnnotation As String
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public ListAddress As String
            '
            ' New feilds
            '
            Public SubscribeMessageFileStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public SubscribeMessageFile As String
            Public UnsubscribeMessageFileStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public UnsubscribeMessageFile As String
            Public SubjectSuffixStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public SubjectSuffix As String
            Public SubjectPrefixStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public SubjectPrefix As String
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public Owner As String
            Public HelpMessageFileStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public HelpMessageFile As String
            Public RemovalMessageFileStatus As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public RemovalMessageFile As String
            Public ReplyToMode As Integer
            Public MaxMessageSize As Integer
            Public PostingMode As Integer
            Public SubScriptionMode As Integer
            Public AuthenticationMode As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public Password As String
            Public DigestMode As Integer
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public DigestMailbox As String
            Public DigestAnnotationMode As Integer
            Public DigestAttachmentMode As Integer
            Public DigestMessageSeparationMode As Integer
            Public DigestSchedulingStatus As Integer
            Public DigestSchedulingMode As Integer
            Public DigestSchedulingInterval As Integer
            Public FromAddressMode As Integer
        End Structure


        Private Declare Function ListGet Lib "MEAILS.DLL" (ByRef lpList As ILISTTYPE) As Integer
        Private Declare Function ListFindFirst Lib "MEAILS.DLL" (ByRef lpList As ILISTTYPE) As Integer
        Private Declare Function ListFindNext Lib "MEAILS.DLL" (ByRef lpList As ILISTTYPE) As Integer
        Private Declare Function ListAdd Lib "MEAILS.DLL" (ByRef lpList As ILISTTYPE) As Integer
        Private Declare Function ListEdit Lib "MEAILS.DLL" (ByRef TargetList As ILISTTYPE, ByRef NewList As ILISTTYPE) As Integer
        Private Declare Function ListRemove Lib "MEAILS.DLL" (ByRef lpList As ILISTTYPE) As Integer
        Private Declare Function SetCurrentHost Lib "MEAILS.DLL" (ByVal CurrentHost As String) As Integer


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

        Public Function FindFirstList() As Integer
            Dim CList As ILISTTYPE
            CList.ListType = ListType
            CList.AccountName = AccountName
            CList.ListName = ListName
            CList.Description = Description
            CList.ListStatus = ListStatus
            CList.HeaderAnnotationStatus = HeaderAnnotationStatus
            CList.HeaderAnnotation = HeaderAnnotation
            CList.FooterAnnotationStatus = FooterAnnotationStatus
            CList.FooterAnnotation = FooterAnnotation
            CList.ModeratorAddress = ModeratorAddress
            CList.ListAddress = ListAddress
            CList.SubscribeMessageFileStatus = SubscribeMessageFileStatus
            CList.SubscribeMessageFile = SubscribeMessageFile
            CList.UnsubscribeMessageFileStatus = UnsubscribeMessageFileStatus
            CList.UnsubscribeMessageFile = UnsubscribeMessageFile
            CList.SubjectSuffixStatus = SubjectSuffixStatus
            CList.SubjectSuffix = SubjectSuffix
            CList.SubjectPrefixStatus = SubjectPrefixStatus
            CList.SubjectPrefix = SubjectPrefix
            CList.Owner = Owner
            CList.HelpMessageFileStatus = HelpMessageFileStatus
            CList.HelpMessageFile = HelpMessageFile
            CList.RemovalMessageFileStatus = RemovalMessageFileStatus
            CList.RemovalMessageFile = RemovalMessageFile
            CList.ReplyToMode = ReplyToMode
            CList.MaxMessageSize = MaxMessageSize
            CList.PostingMode = PostingMode
            CList.SubScriptionMode = SubScriptionMode
            CList.AuthenticationMode = AuthenticationMode
            CList.Password = Password
            CList.DigestMode = DigestMode
            CList.DigestMailbox = DigestMailbox
            CList.DigestAnnotationMode = DigestAnnotationMode
            CList.DigestAttachmentMode = DigestAttachmentMode
            CList.DigestMessageSeparationMode = DigestMessageSeparationMode
            CList.DigestSchedulingStatus = DigestSchedulingStatus
            CList.DigestSchedulingMode = DigestSchedulingMode
            CList.DigestSchedulingInterval = DigestSchedulingInterval
            CList.FromAddressMode = FromAddressMode
            '
            ' Make Call
            '
            FindFirstList = ListFindFirst(CList)
            ListType = CList.ListType
            AccountName = CList.AccountName
            ListName = CList.ListName
            Description = CList.Description
            ListStatus = CList.ListStatus
            HeaderAnnotationStatus = CList.HeaderAnnotationStatus
            HeaderAnnotation = CList.HeaderAnnotation
            FooterAnnotationStatus = CList.FooterAnnotationStatus
            FooterAnnotation = CList.FooterAnnotation
            ModeratorAddress = CList.ModeratorAddress
            ListAddress = CList.ListAddress
            SubscribeMessageFileStatus = CList.SubscribeMessageFileStatus
            SubscribeMessageFile = CList.SubscribeMessageFile
            UnsubscribeMessageFileStatus = CList.UnsubscribeMessageFileStatus
            UnsubscribeMessageFile = CList.UnsubscribeMessageFile
            SubjectSuffixStatus = CList.SubjectSuffixStatus
            SubjectSuffix = CList.SubjectSuffix
            SubjectPrefixStatus = CList.SubjectPrefixStatus
            SubjectPrefix = CList.SubjectPrefix
            Owner = CList.Owner
            HelpMessageFileStatus = CList.HelpMessageFileStatus
            HelpMessageFile = CList.HelpMessageFile
            RemovalMessageFileStatus = CList.RemovalMessageFileStatus
            RemovalMessageFile = CList.RemovalMessageFile
            ReplyToMode = CList.ReplyToMode
            MaxMessageSize = CList.MaxMessageSize
            PostingMode = CList.PostingMode
            SubScriptionMode = CList.SubScriptionMode
            AuthenticationMode = CList.AuthenticationMode
            Password = CList.Password
            DigestMode = CList.DigestMode
            DigestMailbox = CList.DigestMailbox
            DigestAnnotationMode = CList.DigestAnnotationMode
            DigestAttachmentMode = CList.DigestAttachmentMode
            DigestMessageSeparationMode = CList.DigestMessageSeparationMode
            DigestSchedulingStatus = CList.DigestSchedulingStatus
            DigestSchedulingMode = CList.DigestSchedulingMode
            DigestSchedulingInterval = CList.DigestSchedulingInterval
            FromAddressMode = CList.FromAddressMode
        End Function
        Public Function FindNextList() As Integer
            Dim CList As ILISTTYPE
            CList.ListType = ListType
            CList.AccountName = AccountName
            CList.ListName = ListName
            CList.Description = Description
            CList.ListStatus = ListStatus
            CList.HeaderAnnotationStatus = HeaderAnnotationStatus
            CList.HeaderAnnotation = HeaderAnnotation
            CList.FooterAnnotationStatus = FooterAnnotationStatus
            CList.FooterAnnotation = FooterAnnotation
            CList.ModeratorAddress = ModeratorAddress
            CList.ListAddress = ListAddress
            CList.SubscribeMessageFileStatus = SubscribeMessageFileStatus
            CList.SubscribeMessageFile = SubscribeMessageFile
            CList.UnsubscribeMessageFileStatus = UnsubscribeMessageFileStatus
            CList.UnsubscribeMessageFile = UnsubscribeMessageFile
            CList.SubjectSuffixStatus = SubjectSuffixStatus
            CList.SubjectSuffix = SubjectSuffix
            CList.SubjectPrefixStatus = SubjectPrefixStatus
            CList.SubjectPrefix = SubjectPrefix
            CList.Owner = Owner
            CList.HelpMessageFileStatus = HelpMessageFileStatus
            CList.HelpMessageFile = HelpMessageFile
            CList.RemovalMessageFileStatus = RemovalMessageFileStatus
            CList.RemovalMessageFile = RemovalMessageFile
            CList.ReplyToMode = ReplyToMode
            CList.MaxMessageSize = MaxMessageSize
            CList.PostingMode = PostingMode
            CList.SubScriptionMode = SubScriptionMode
            CList.AuthenticationMode = AuthenticationMode
            CList.Password = Password
            CList.DigestMode = DigestMode
            CList.DigestMailbox = DigestMailbox
            CList.DigestAnnotationMode = DigestAnnotationMode
            CList.DigestAttachmentMode = DigestAttachmentMode
            CList.DigestMessageSeparationMode = DigestMessageSeparationMode
            CList.DigestSchedulingStatus = DigestSchedulingStatus
            CList.DigestSchedulingMode = DigestSchedulingMode
            CList.DigestSchedulingInterval = DigestSchedulingInterval
            CList.FromAddressMode = FromAddressMode
            '
            ' Make Call
            '
            FindNextList = ListFindNext(CList)
            ListType = CList.ListType
            AccountName = CList.AccountName
            ListName = CList.ListName
            Description = CList.Description
            ListStatus = CList.ListStatus
            HeaderAnnotationStatus = CList.HeaderAnnotationStatus
            HeaderAnnotation = CList.HeaderAnnotation
            FooterAnnotationStatus = CList.FooterAnnotationStatus
            FooterAnnotation = CList.FooterAnnotation
            ModeratorAddress = CList.ModeratorAddress
            ListAddress = CList.ListAddress
            SubscribeMessageFileStatus = CList.SubscribeMessageFileStatus
            SubscribeMessageFile = CList.SubscribeMessageFile
            UnsubscribeMessageFileStatus = CList.UnsubscribeMessageFileStatus
            UnsubscribeMessageFile = CList.UnsubscribeMessageFile
            SubjectSuffixStatus = CList.SubjectSuffixStatus
            SubjectSuffix = CList.SubjectSuffix
            SubjectPrefixStatus = CList.SubjectPrefixStatus
            SubjectPrefix = CList.SubjectPrefix
            Owner = CList.Owner
            HelpMessageFileStatus = CList.HelpMessageFileStatus
            HelpMessageFile = CList.HelpMessageFile
            RemovalMessageFileStatus = CList.RemovalMessageFileStatus
            RemovalMessageFile = CList.RemovalMessageFile
            ReplyToMode = CList.ReplyToMode
            MaxMessageSize = CList.MaxMessageSize
            PostingMode = CList.PostingMode
            SubScriptionMode = CList.SubScriptionMode
            AuthenticationMode = CList.AuthenticationMode
            Password = CList.Password
            DigestMode = CList.DigestMode
            DigestMailbox = CList.DigestMailbox
            DigestAnnotationMode = CList.DigestAnnotationMode
            DigestAttachmentMode = CList.DigestAttachmentMode
            DigestMessageSeparationMode = CList.DigestMessageSeparationMode
            DigestSchedulingStatus = CList.DigestSchedulingStatus
            DigestSchedulingMode = CList.DigestSchedulingMode
            DigestSchedulingInterval = CList.DigestSchedulingInterval
            FromAddressMode = CList.FromAddressMode
        End Function

        Public Function AddList() As Integer
            Dim CList As ILISTTYPE
            CList.ListType = ListType
            CList.AccountName = AccountName
            CList.ListName = ListName
            CList.Description = Description
            CList.ListStatus = ListStatus
            CList.HeaderAnnotationStatus = HeaderAnnotationStatus
            CList.HeaderAnnotation = HeaderAnnotation
            CList.FooterAnnotationStatus = FooterAnnotationStatus
            CList.FooterAnnotation = FooterAnnotation
            CList.ModeratorAddress = ModeratorAddress
            CList.ListAddress = ListAddress
            CList.SubscribeMessageFileStatus = SubscribeMessageFileStatus
            CList.SubscribeMessageFile = SubscribeMessageFile
            CList.UnsubscribeMessageFileStatus = UnsubscribeMessageFileStatus
            CList.UnsubscribeMessageFile = UnsubscribeMessageFile
            CList.SubjectSuffixStatus = SubjectSuffixStatus
            CList.SubjectSuffix = SubjectSuffix
            CList.SubjectPrefixStatus = SubjectPrefixStatus
            CList.SubjectPrefix = SubjectPrefix
            CList.Owner = Owner
            CList.HelpMessageFileStatus = HelpMessageFileStatus
            CList.HelpMessageFile = HelpMessageFile
            CList.RemovalMessageFileStatus = RemovalMessageFileStatus
            CList.RemovalMessageFile = RemovalMessageFile
            CList.ReplyToMode = ReplyToMode
            CList.MaxMessageSize = MaxMessageSize
            CList.PostingMode = PostingMode
            CList.SubScriptionMode = SubScriptionMode
            CList.AuthenticationMode = AuthenticationMode
            CList.Password = Password
            CList.DigestMode = DigestMode
            CList.DigestMailbox = DigestMailbox
            CList.DigestAnnotationMode = DigestAnnotationMode
            CList.DigestAttachmentMode = DigestAttachmentMode
            CList.DigestMessageSeparationMode = DigestMessageSeparationMode
            CList.DigestSchedulingStatus = DigestSchedulingStatus
            CList.DigestSchedulingMode = DigestSchedulingMode
            CList.DigestSchedulingInterval = DigestSchedulingInterval
            CList.FromAddressMode = FromAddressMode
            '
            ' Make Call
            '
            AddList = ListAdd(CList)
            ListType = CList.ListType
            AccountName = CList.AccountName
            ListName = CList.ListName
            Description = CList.Description
            ListStatus = CList.ListStatus
            HeaderAnnotationStatus = CList.HeaderAnnotationStatus
            HeaderAnnotation = CList.HeaderAnnotation
            FooterAnnotationStatus = CList.FooterAnnotationStatus
            FooterAnnotation = CList.FooterAnnotation
            ModeratorAddress = CList.ModeratorAddress
            ListAddress = CList.ListAddress
            SubscribeMessageFileStatus = CList.SubscribeMessageFileStatus
            SubscribeMessageFile = CList.SubscribeMessageFile
            UnsubscribeMessageFileStatus = CList.UnsubscribeMessageFileStatus
            UnsubscribeMessageFile = CList.UnsubscribeMessageFile
            SubjectSuffixStatus = CList.SubjectSuffixStatus
            SubjectSuffix = CList.SubjectSuffix
            SubjectPrefixStatus = CList.SubjectPrefixStatus
            SubjectPrefix = CList.SubjectPrefix
            Owner = CList.Owner
            HelpMessageFileStatus = CList.HelpMessageFileStatus
            HelpMessageFile = CList.HelpMessageFile
            RemovalMessageFileStatus = CList.RemovalMessageFileStatus
            RemovalMessageFile = CList.RemovalMessageFile
            ReplyToMode = CList.ReplyToMode
            MaxMessageSize = CList.MaxMessageSize
            PostingMode = CList.PostingMode
            SubScriptionMode = CList.SubScriptionMode
            AuthenticationMode = CList.AuthenticationMode
            Password = CList.Password
            DigestMode = CList.DigestMode
            DigestMailbox = CList.DigestMailbox
            DigestAnnotationMode = CList.DigestAnnotationMode
            DigestAttachmentMode = CList.DigestAttachmentMode
            DigestMessageSeparationMode = CList.DigestMessageSeparationMode
            DigestSchedulingStatus = CList.DigestSchedulingStatus
            DigestSchedulingMode = CList.DigestSchedulingMode
            DigestSchedulingInterval = CList.DigestSchedulingInterval
            FromAddressMode = CList.FromAddressMode
        End Function

        Public Function GetList() As Integer
            Dim CList As ILISTTYPE
            CList.ListType = ListType
            CList.AccountName = AccountName
            CList.ListName = ListName
            CList.Description = Description
            CList.ListStatus = ListStatus
            CList.HeaderAnnotationStatus = HeaderAnnotationStatus
            CList.HeaderAnnotation = HeaderAnnotation
            CList.FooterAnnotationStatus = FooterAnnotationStatus
            CList.FooterAnnotation = FooterAnnotation
            CList.ModeratorAddress = ModeratorAddress
            CList.ListAddress = ListAddress
            CList.SubscribeMessageFileStatus = SubscribeMessageFileStatus
            CList.SubscribeMessageFile = SubscribeMessageFile
            CList.UnsubscribeMessageFileStatus = UnsubscribeMessageFileStatus
            CList.UnsubscribeMessageFile = UnsubscribeMessageFile
            CList.SubjectSuffixStatus = SubjectSuffixStatus
            CList.SubjectSuffix = SubjectSuffix
            CList.SubjectPrefixStatus = SubjectPrefixStatus
            CList.SubjectPrefix = SubjectPrefix
            CList.Owner = Owner
            CList.HelpMessageFileStatus = HelpMessageFileStatus
            CList.HelpMessageFile = HelpMessageFile
            CList.RemovalMessageFileStatus = RemovalMessageFileStatus
            CList.RemovalMessageFile = RemovalMessageFile
            CList.ReplyToMode = ReplyToMode
            CList.MaxMessageSize = MaxMessageSize
            CList.PostingMode = PostingMode
            CList.SubScriptionMode = SubScriptionMode
            CList.AuthenticationMode = AuthenticationMode
            CList.Password = Password
            CList.DigestMode = DigestMode
            CList.DigestMailbox = DigestMailbox
            CList.DigestAnnotationMode = DigestAnnotationMode
            CList.DigestAttachmentMode = DigestAttachmentMode
            CList.DigestMessageSeparationMode = DigestMessageSeparationMode
            CList.DigestSchedulingStatus = DigestSchedulingStatus
            CList.DigestSchedulingMode = DigestSchedulingMode
            CList.DigestSchedulingInterval = DigestSchedulingInterval
            CList.FromAddressMode = FromAddressMode
            '
            ' Make Call
            '
            GetList = ListGet(CList)
            ListType = CList.ListType
            AccountName = CList.AccountName
            ListName = CList.ListName
            Description = CList.Description
            ListStatus = CList.ListStatus
            HeaderAnnotationStatus = CList.HeaderAnnotationStatus
            HeaderAnnotation = CList.HeaderAnnotation
            FooterAnnotationStatus = CList.FooterAnnotationStatus
            FooterAnnotation = CList.FooterAnnotation
            ModeratorAddress = CList.ModeratorAddress
            ListAddress = CList.ListAddress
            'new
            SubscribeMessageFileStatus = CList.SubscribeMessageFileStatus
            SubscribeMessageFile = CList.SubscribeMessageFile
            UnsubscribeMessageFileStatus = CList.UnsubscribeMessageFileStatus
            UnsubscribeMessageFile = CList.UnsubscribeMessageFile
            SubjectSuffixStatus = CList.SubjectSuffixStatus
            SubjectSuffix = CList.SubjectSuffix
            SubjectPrefixStatus = CList.SubjectPrefixStatus
            SubjectPrefix = CList.SubjectPrefix
            Owner = CList.Owner
            HelpMessageFileStatus = CList.HelpMessageFileStatus
            HelpMessageFile = CList.HelpMessageFile
            RemovalMessageFileStatus = CList.RemovalMessageFileStatus
            RemovalMessageFile = CList.RemovalMessageFile
            ReplyToMode = CList.ReplyToMode
            MaxMessageSize = CList.MaxMessageSize
            PostingMode = CList.PostingMode
            SubScriptionMode = CList.SubScriptionMode
            AuthenticationMode = CList.AuthenticationMode
            Password = CList.Password
            DigestMode = CList.DigestMode
            DigestMailbox = CList.DigestMailbox
            DigestAnnotationMode = CList.DigestAnnotationMode
            DigestAttachmentMode = CList.DigestAttachmentMode
            DigestMessageSeparationMode = CList.DigestMessageSeparationMode
            DigestSchedulingStatus = CList.DigestSchedulingStatus
            DigestSchedulingMode = CList.DigestSchedulingMode
            DigestSchedulingInterval = CList.DigestSchedulingInterval
            FromAddressMode = CList.FromAddressMode
        End Function
        Public Function RemoveList() As Integer
            Dim CList As ILISTTYPE
            CList.ListType = ListType
            CList.AccountName = AccountName
            CList.ListName = ListName
            CList.Description = Description
            CList.ListStatus = ListStatus
            CList.HeaderAnnotationStatus = HeaderAnnotationStatus
            CList.HeaderAnnotation = HeaderAnnotation
            CList.FooterAnnotationStatus = FooterAnnotationStatus
            CList.FooterAnnotation = FooterAnnotation
            CList.ModeratorAddress = ModeratorAddress
            CList.ListAddress = ListAddress
            CList.SubscribeMessageFileStatus = SubscribeMessageFileStatus
            CList.SubscribeMessageFile = SubscribeMessageFile
            CList.UnsubscribeMessageFileStatus = UnsubscribeMessageFileStatus
            CList.UnsubscribeMessageFile = UnsubscribeMessageFile
            CList.SubjectSuffixStatus = SubjectSuffixStatus
            CList.SubjectSuffix = SubjectSuffix
            CList.SubjectPrefixStatus = SubjectPrefixStatus
            CList.SubjectPrefix = SubjectPrefix
            CList.Owner = Owner
            CList.HelpMessageFileStatus = HelpMessageFileStatus
            CList.HelpMessageFile = HelpMessageFile
            CList.RemovalMessageFileStatus = RemovalMessageFileStatus
            CList.RemovalMessageFile = RemovalMessageFile
            CList.ReplyToMode = ReplyToMode
            CList.MaxMessageSize = MaxMessageSize
            CList.PostingMode = PostingMode
            CList.SubScriptionMode = SubScriptionMode
            CList.AuthenticationMode = AuthenticationMode
            CList.Password = Password
            CList.DigestMode = DigestMode
            CList.DigestMailbox = DigestMailbox
            CList.DigestAnnotationMode = DigestAnnotationMode
            CList.DigestAttachmentMode = DigestAttachmentMode
            CList.DigestMessageSeparationMode = DigestMessageSeparationMode
            CList.DigestSchedulingStatus = DigestSchedulingStatus
            CList.DigestSchedulingMode = DigestSchedulingMode
            CList.DigestSchedulingInterval = DigestSchedulingInterval
            CList.FromAddressMode = FromAddressMode
            '
            ' Make Call
            '
            RemoveList = ListRemove(CList)
            ListType = CList.ListType
            AccountName = CList.AccountName
            ListName = CList.ListName
            Description = CList.Description
            ListStatus = CList.ListStatus
            HeaderAnnotationStatus = CList.HeaderAnnotationStatus
            HeaderAnnotation = CList.HeaderAnnotation
            FooterAnnotationStatus = CList.FooterAnnotationStatus
            FooterAnnotation = CList.FooterAnnotation
            ModeratorAddress = CList.ModeratorAddress
            ListAddress = CList.ListAddress
            SubscribeMessageFileStatus = CList.SubscribeMessageFileStatus
            SubscribeMessageFile = CList.SubscribeMessageFile
            UnsubscribeMessageFileStatus = CList.UnsubscribeMessageFileStatus
            UnsubscribeMessageFile = CList.UnsubscribeMessageFile
            SubjectSuffixStatus = CList.SubjectSuffixStatus
            SubjectSuffix = CList.SubjectSuffix
            SubjectPrefixStatus = CList.SubjectPrefixStatus
            SubjectPrefix = CList.SubjectPrefix
            Owner = CList.Owner
            HelpMessageFileStatus = CList.HelpMessageFileStatus
            HelpMessageFile = CList.HelpMessageFile
            RemovalMessageFileStatus = CList.RemovalMessageFileStatus
            RemovalMessageFile = CList.RemovalMessageFile
            ReplyToMode = CList.ReplyToMode
            MaxMessageSize = CList.MaxMessageSize
            PostingMode = CList.PostingMode
            SubScriptionMode = CList.SubScriptionMode
            AuthenticationMode = CList.AuthenticationMode
            Password = CList.Password
            DigestMode = CList.DigestMode
            DigestMailbox = CList.DigestMailbox
            DigestAnnotationMode = CList.DigestAnnotationMode
            DigestAttachmentMode = CList.DigestAttachmentMode
            DigestMessageSeparationMode = CList.DigestMessageSeparationMode
            DigestSchedulingStatus = CList.DigestSchedulingStatus
            DigestSchedulingMode = CList.DigestSchedulingMode
            DigestSchedulingInterval = CList.DigestSchedulingInterval
            FromAddressMode = CList.FromAddressMode
        End Function

        Public Function EditList(ByVal NewDescription As String, ByVal NewAccountName As String, ByVal NewListName As String, ByVal NewListType As Long, ByVal NewListStatus As Long, ByVal NewHeaderAnnotationStatus As Long, ByVal NewHeaderAnnotation As String, ByVal NewFooterAnnotationStatus As Long, ByVal NewFooterAnnotation As String, ByVal NewModeratorAddress As String, ByVal NewListAddress As String, _
        Optional ByVal NewSubscribeMessageFileStatus As Long = -1, Optional ByVal NewSubscribeMessageFile As String = "(Nil)", Optional ByVal NewUnsubscribeMessageFileStatus As Long = -1, Optional ByVal NewUnsubscribeMessageFile As String = "(Nil)", Optional ByVal NewSubjectSuffixStatus As Long = -1, Optional ByVal NewSubjectSuffix As String = "(Nil)", Optional ByVal NewSubjectPrefixStatus As Long = -1, Optional ByVal NewSubjectPrefix As String = "(Nil)", Optional ByVal NewOwner As String = "(Nil)", Optional ByVal NewHelpMessageFileStatus As Long = -1, _
        Optional ByVal NewHelpMessageFile As String = "(Nil)", Optional ByVal NewRemovalMessageFileStatus As Long = -1, Optional ByVal NewRemovalMessageFile As String = "(Nil)", Optional ByVal NewReplyToMode As Long = -1, Optional ByVal NewMaxMessageSize As Long = -1, Optional ByVal NewPostingMode As Long = -1, Optional ByVal NewSubScriptionMode As Long = -1, Optional ByVal NewAuthenticationMode As Long = -1, Optional ByVal NewPassword As String = "(Nil)", Optional ByVal NewDigestMode As Long = -1, Optional ByVal NewDigestMailbox As String = "(Nil)", _
        Optional ByVal NewDigestAnnotationMode As Long = -1, Optional ByVal NewDigestAttachmentMode As Long = -1, Optional ByVal NewDigestMessageSeparationMode As Long = -1, Optional ByVal NewDigestSchedulingStatus As Long = -1, _
        Optional ByVal NewDigestSchedulingMode As Long = -1, Optional ByVal NewDigestSchedulingInterval As Long = -1, Optional ByVal NewFromAddressMode As Long = -1) As Integer
            '
            ' hmm - if we pass these with default values of Blank, then how will we know whether or not to clear then or not?
            '
            '
            Dim CList As ILISTTYPE
            Dim CListData As ILISTTYPE
            ' Get the Find Stuff Set up
            CList.ListType = ListType
            CList.ListName = ListName
            CList.AccountName = AccountName
            CList.Description = Description
            CList.ListStatus = ListStatus
            CList.HeaderAnnotationStatus = HeaderAnnotationStatus
            CList.HeaderAnnotation = HeaderAnnotation
            CList.FooterAnnotationStatus = FooterAnnotationStatus
            CList.FooterAnnotation = FooterAnnotation
            CList.ModeratorAddress = ModeratorAddress
            CList.ListAddress = ListAddress
            CList.SubscribeMessageFileStatus = SubscribeMessageFileStatus
            CList.SubscribeMessageFile = SubscribeMessageFile
            CList.UnsubscribeMessageFileStatus = UnsubscribeMessageFileStatus
            CList.UnsubscribeMessageFile = UnsubscribeMessageFile
            CList.SubjectSuffixStatus = SubjectSuffixStatus
            CList.SubjectSuffix = SubjectSuffix
            CList.SubjectPrefixStatus = SubjectPrefixStatus
            CList.SubjectPrefix = SubjectPrefix
            CList.Owner = Owner
            CList.HelpMessageFileStatus = HelpMessageFileStatus
            CList.HelpMessageFile = HelpMessageFile
            CList.RemovalMessageFileStatus = RemovalMessageFileStatus
            CList.RemovalMessageFile = RemovalMessageFile
            CList.ReplyToMode = ReplyToMode
            CList.MaxMessageSize = MaxMessageSize
            CList.PostingMode = PostingMode
            CList.SubScriptionMode = SubScriptionMode
            CList.AuthenticationMode = AuthenticationMode
            CList.Password = Password
            CList.DigestMode = DigestMode
            CList.DigestMailbox = DigestMailbox
            CList.DigestAnnotationMode = DigestAnnotationMode
            CList.DigestAttachmentMode = DigestAttachmentMode
            CList.DigestMessageSeparationMode = DigestMessageSeparationMode
            CList.DigestSchedulingStatus = DigestSchedulingStatus
            CList.DigestSchedulingMode = DigestSchedulingMode
            CList.DigestSchedulingInterval = DigestSchedulingInterval
            CList.FromAddressMode = FromAddressMode
            ' Get the Data Set up
            CListData.ListType = NewListType
            CListData.ListName = NewListName
            CListData.AccountName = NewAccountName
            CListData.Description = NewDescription
            CListData.ListStatus = NewListStatus 'NewHeaderAnnotationStatus As Long, NewHeaderAnnotation As String, NewFooterAnnotationStatus As Long, NewFooterAnnotation As String,NewModeratorAddress As String
            CListData.HeaderAnnotationStatus = NewHeaderAnnotationStatus
            CListData.HeaderAnnotation = NewHeaderAnnotation
            CListData.FooterAnnotationStatus = NewFooterAnnotationStatus
            CListData.FooterAnnotation = NewFooterAnnotation
            CListData.ModeratorAddress = NewModeratorAddress
            CListData.ListAddress = NewListAddress
            CListData.SubscribeMessageFileStatus = NewSubscribeMessageFileStatus
            CListData.SubscribeMessageFile = NewSubscribeMessageFile
            CListData.UnsubscribeMessageFileStatus = NewUnsubscribeMessageFileStatus
            CListData.UnsubscribeMessageFile = NewUnsubscribeMessageFile
            CListData.SubjectSuffixStatus = NewSubjectSuffixStatus
            CListData.SubjectSuffix = NewSubjectSuffix
            CListData.SubjectPrefixStatus = NewSubjectPrefixStatus
            CListData.SubjectPrefix = NewSubjectPrefix
            CListData.Owner = NewOwner
            CListData.HelpMessageFileStatus = NewHelpMessageFileStatus
            CListData.HelpMessageFile = NewHelpMessageFile
            CListData.RemovalMessageFileStatus = NewRemovalMessageFileStatus
            CListData.RemovalMessageFile = NewRemovalMessageFile
            CListData.ReplyToMode = NewReplyToMode
            CListData.MaxMessageSize = NewMaxMessageSize
            CListData.PostingMode = NewPostingMode
            CListData.SubScriptionMode = NewSubScriptionMode
            CListData.AuthenticationMode = NewAuthenticationMode
            CListData.Password = NewPassword
            CListData.DigestMode = NewDigestMode
            CListData.DigestMailbox = NewDigestMailbox
            CListData.DigestAnnotationMode = NewDigestAnnotationMode
            CListData.DigestAttachmentMode = NewDigestAttachmentMode
            CListData.DigestMessageSeparationMode = NewDigestMessageSeparationMode
            CListData.DigestSchedulingStatus = NewDigestSchedulingStatus
            CListData.DigestSchedulingMode = NewDigestSchedulingMode
            CListData.DigestSchedulingInterval = NewDigestSchedulingInterval
            CListData.FromAddressMode = NewFromAddressMode
            '
            ' Make the Call
            '
            EditList = ListEdit(CList, CListData)
            ListType = CListData.ListType
            AccountName = CListData.AccountName
            ListName = CListData.ListName
            Description = CListData.Description
            ListStatus = CList.ListStatus
            HeaderAnnotationStatus = CList.HeaderAnnotationStatus
            HeaderAnnotation = CList.HeaderAnnotation
            FooterAnnotationStatus = CList.FooterAnnotationStatus
            FooterAnnotation = CList.FooterAnnotation
            ModeratorAddress = CList.ModeratorAddress
            ListAddress = CList.ListAddress
            SubscribeMessageFileStatus = CList.SubscribeMessageFileStatus
            SubscribeMessageFile = CList.SubscribeMessageFile
            UnsubscribeMessageFileStatus = CList.UnsubscribeMessageFileStatus
            UnsubscribeMessageFile = CList.UnsubscribeMessageFile
            SubjectSuffixStatus = CList.SubjectSuffixStatus
            SubjectSuffix = CList.SubjectSuffix
            SubjectPrefixStatus = CList.SubjectPrefixStatus
            SubjectPrefix = CList.SubjectPrefix
            Owner = CList.Owner
            HelpMessageFileStatus = CList.HelpMessageFileStatus
            HelpMessageFile = CList.HelpMessageFile
            RemovalMessageFileStatus = CList.RemovalMessageFileStatus
            RemovalMessageFile = CList.RemovalMessageFile
            ReplyToMode = CList.ReplyToMode
            MaxMessageSize = CList.MaxMessageSize
            PostingMode = CList.PostingMode
            SubScriptionMode = CList.SubScriptionMode
            AuthenticationMode = CList.AuthenticationMode
            Password = CList.Password
            DigestMode = CList.DigestMode
            DigestMailbox = CList.DigestMailbox
            DigestAnnotationMode = CList.DigestAnnotationMode
            DigestAttachmentMode = CList.DigestAttachmentMode
            DigestMessageSeparationMode = CList.DigestMessageSeparationMode
            DigestSchedulingStatus = CList.DigestSchedulingStatus
            DigestSchedulingMode = CList.DigestSchedulingMode
            DigestSchedulingInterval = CList.DigestSchedulingInterval
            FromAddressMode = CList.FromAddressMode
        End Function
        '
        ' We need to wrapper these routines
        '
        Function SetHeader(ByVal Postoffice As String, ByVal List As String, ByVal ListHeader As String) As Integer

            Dim CAnnotation As IANNOTATIONTYPE

            CAnnotation.AccountName = Postoffice
            CAnnotation.AnnotationName = List & "-HEADER"
            CAnnotation.AnnotationText = ListHeader
            SetHeader = AnnotationAdd(CAnnotation)

        End Function

        Function GetHeader(ByVal Postoffice As String, ByVal List As String) As String

            Dim CAnnotation As IANNOTATIONTYPE
            CAnnotation.AnnotationText = ""

            CAnnotation.AccountName = Postoffice
            CAnnotation.AnnotationName = List & "-HEADER"
            If (AnnotationGet(CAnnotation) = 1) Then
                GetHeader = CAnnotation.AnnotationText
            Else
                GetHeader = ""
            End If

        End Function

        Function SetFooter(ByVal Postoffice As String, ByVal List As String, ByVal ListFooter As String) As Integer

            Dim CAnnotation As IANNOTATIONTYPE

            CAnnotation.AccountName = Postoffice
            CAnnotation.AnnotationName = List & "-FOOTER"
            CAnnotation.AnnotationText = ListFooter
            SetFooter = AnnotationAdd(CAnnotation)

        End Function

        Public Function GetFooter(ByVal Postoffice As String, ByVal List As String) As String

            Dim CAnnotation As IANNOTATIONTYPE

            CAnnotation.AccountName = Postoffice
            CAnnotation.AnnotationName = List & "-FOOTER"
            CAnnotation.AnnotationText = ""

            If (AnnotationGet(CAnnotation) = 1) Then
                GetFooter = CAnnotation.AnnotationText
            Else
                GetFooter = ""
            End If

        End Function

        Private Sub AddDataTableColumns(ByRef oTable As DataTable)
            oTable.Columns.Add("ListName", GetType(String))
            oTable.Columns.Add("ListStatus", GetType(Long))
            oTable.Columns.Add("Description", GetType(String))
            oTable.Columns.Add("ListType", GetType(Long))
        End Sub


        Public Property Description() As String
            Get
                Return Me.DescriptionVal
            End Get
            Set(ByVal value As String)
                Me.DescriptionVal = value
            End Set
        End Property

        Public Property AccountName() As String
            Get
                Return Me.AccountNameVal
            End Get
            Set(ByVal value As String)
                Me.AccountNameVal = value
            End Set
        End Property

        Public Property ListName() As String
            Get
                Return Me.ListNameVal
            End Get
            Set(ByVal value As String)
                Me.ListNameVal = value
            End Set
        End Property

        Public Property ListType() As Long
            Get
                Return Me.ListTypeVal
            End Get
            Set(ByVal value As Long)
                Me.ListTypeVal = value
            End Set
        End Property

        Public Property ListStatus() As Long
            Get
                Return Me.ListStatusVal
            End Get
            Set(ByVal value As Long)
                Me.ListStatusVal = value
            End Set
        End Property

        Public Property ModeratorAddress() As String
            Get
                Return Me.ModeratorAddressVal
            End Get
            Set(ByVal value As String)
                Me.ModeratorAddressVal = value
            End Set
        End Property

        Public Property HeaderAnnotationStatus() As Long
            Get
                Return Me.HeaderAnnotationStatusVal
            End Get
            Set(ByVal value As Long)
                Me.HeaderAnnotationStatusVal = value
            End Set
        End Property

        Public Property HeaderAnnotation() As String
            Get
                Return Me.HeaderAnnotationVal
            End Get
            Set(ByVal value As String)
                Me.HeaderAnnotationVal = value
            End Set
        End Property

        Public Property FooterAnnotationStatus() As Long
            Get
                Return Me.FooterAnnotationStatusVal
            End Get
            Set(ByVal value As Long)
                Me.FooterAnnotationStatusVal = value
            End Set
        End Property

        Public Property FooterAnnotation() As String
            Get
                Return Me.FooterAnnotationVal
            End Get
            Set(ByVal value As String)
                Me.FooterAnnotationVal = value
            End Set
        End Property

        Public Property ListAddress() As String
            Get
                Return Me.ListAddressVal
            End Get
            Set(ByVal value As String)
                Me.ListAddressVal = value
            End Set
        End Property

        Public Property SubscribeMessageFileStatus() As Long
            Get
                Return Me.SubscribeMessageFileStatusVal
            End Get
            Set(ByVal value As Long)
                Me.SubscribeMessageFileStatusVal = value
            End Set
        End Property

        Public Property SubscribeMessageFile() As String
            Get
                Return Me.SubscribeMessageFileVal
            End Get
            Set(ByVal value As String)
                Me.SubscribeMessageFileVal = value
            End Set
        End Property

        Public Property UnsubscribeMessageFileStatus() As Long
            Get
                Return Me.UnsubscribeMessageFileStatusVal
            End Get
            Set(ByVal value As Long)
                Me.UnsubscribeMessageFileStatusVal = value
            End Set
        End Property

        Public Property UnsubscribeMessageFile() As String
            Get
                Return Me.UnsubscribeMessageFileVal
            End Get
            Set(ByVal value As String)
                Me.UnsubscribeMessageFileVal = value
            End Set
        End Property

        Public Property SubjectSuffixStatus() As Long
            Get
                Return Me.SubjectSuffixStatusVal
            End Get
            Set(ByVal value As Long)
                Me.SubjectSuffixStatusVal = value
            End Set
        End Property

        Public Property SubjectSuffix() As String
            Get
                Return Me.SubjectSuffixVal
            End Get
            Set(ByVal value As String)
                Me.SubjectSuffixVal = value
            End Set
        End Property

        Public Property SubjectPrefixStatus() As Long
            Get
                Return Me.SubjectPrefixStatusVal
            End Get
            Set(ByVal value As Long)
                Me.SubjectPrefixStatusVal = value
            End Set
        End Property

        Public Property SubjectPrefix() As String
            Get
                Return Me.SubjectPrefixVal
            End Get
            Set(ByVal value As String)
                Me.SubjectPrefixVal = value
            End Set
        End Property

        Public Property Owner() As String
            Get
                Return Me.OwnerVal
            End Get
            Set(ByVal value As String)
                Me.OwnerVal = value
            End Set
        End Property

        Public Property HelpMessageFileStatus() As Long
            Get
                Return Me.HelpMessageFileStatusVal
            End Get
            Set(ByVal value As Long)
                Me.HelpMessageFileStatusVal = value
            End Set
        End Property

        Public Property HelpMessageFile() As String
            Get
                Return Me.HelpMessageFileVal
            End Get
            Set(ByVal value As String)
                Me.HelpMessageFileVal = value
            End Set
        End Property

        Public Property RemovalMessageFileStatus() As Long
            Get
                Return Me.RemovalMessageFileStatusVal
            End Get
            Set(ByVal value As Long)
                Me.RemovalMessageFileStatusVal = value
            End Set
        End Property

        Public Property RemovalMessageFile() As String
            Get
                Return Me.RemovalMessageFileVal
            End Get
            Set(ByVal value As String)
                Me.RemovalMessageFileVal = value
            End Set
        End Property

        Public Property ReplyToMode() As Long
            Get
                Return Me.ReplyToModeVal
            End Get
            Set(ByVal value As Long)
                Me.ReplyToModeVal = value
            End Set
        End Property

        Public Property MaxMessageSize() As Long
            Get
                Return Me.MaxMessageSizeVal
            End Get
            Set(ByVal value As Long)
                Me.MaxMessageSizeVal = value
            End Set
        End Property

        Public Property PostingMode() As Long
            Get
                Return Me.PostingModeVal
            End Get
            Set(ByVal value As Long)
                Me.PostingModeVal = value
            End Set
        End Property

        Public Property SubScriptionMode() As Long
            Get
                Return Me.SubScriptionModeVal
            End Get
            Set(ByVal value As Long)
                Me.SubScriptionModeVal = value
            End Set
        End Property

        Public Property AuthenticationMode() As Long
            Get
                Return Me.AuthenticationModeVal
            End Get
            Set(ByVal value As Long)
                Me.AuthenticationModeVal = value
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

        Public Property DigestMode() As Long
            Get
                Return Me.DigestModeVal
            End Get
            Set(ByVal value As Long)
                Me.DigestModeVal = value
            End Set
        End Property

        Public Property DigestMailbox() As String
            Get
                Return Me.DigestMailboxVal
            End Get
            Set(ByVal value As String)
                Me.DigestMailboxVal = value
            End Set
        End Property

        Public Property DigestAnnotationMode() As Long
            Get
                Return Me.DigestAnnotationModeVal
            End Get
            Set(ByVal value As Long)
                Me.DigestAnnotationModeVal = value
            End Set
        End Property

        Public Property DigestAttachmentMode() As Long
            Get
                Return Me.DigestAttachmentModeVal
            End Get
            Set(ByVal value As Long)
                Me.DigestAttachmentModeVal = value
            End Set
        End Property

        Public Property DigestMessageSeparationMode() As Long
            Get
                Return Me.DigestMessageSeparationModeVal
            End Get
            Set(ByVal value As Long)
                Me.DigestMessageSeparationModeVal = value
            End Set
        End Property

        Public Property DigestSchedulingStatus() As Long
            Get
                Return Me.DigestSchedulingStatusVal
            End Get
            Set(ByVal value As Long)
                Me.DigestSchedulingStatusVal = value
            End Set
        End Property

        Public Property DigestSchedulingMode() As Long
            Get
                Return Me.DigestSchedulingModeVal
            End Get
            Set(ByVal value As Long)
                Me.DigestSchedulingModeVal = value
            End Set
        End Property

        Public Property DigestSchedulingInterval() As Long
            Get
                Return Me.DigestSchedulingIntervalVal
            End Get
            Set(ByVal value As Long)
                Me.DigestSchedulingIntervalVal = value
            End Set
        End Property

        Public Property FromAddressMode() As Long
            Get
                Return Me.FromAddressModeVal
            End Get
            Set(ByVal value As Long)
                Me.FromAddressModeVal = value
            End Set
        End Property

    End Class

End Namespace

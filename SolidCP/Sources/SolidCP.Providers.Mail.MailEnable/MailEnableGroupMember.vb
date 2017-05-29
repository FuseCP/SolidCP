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

    Public Class MailEnableGroupMember
        Inherits MarshalByRefObject

        Private AddressVal As String
        Private PostofficeVal As String
        Private MailboxVal As String
        Private HostVal As String

        Private Structure IGROUPMEMBERTYPE
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public Address As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public Postoffice As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public Mailbox As String
        End Structure

        Private Declare Function GroupMemberGet Lib "MEAIPO.DLL" (ByRef lpGroupMember As IGROUPMEMBERTYPE) As Integer
        Private Declare Function GroupMemberFindFirst Lib "MEAIPO.DLL" (ByRef lpGroupMember As IGROUPMEMBERTYPE) As Integer
        Private Declare Function GroupMemberFindNext Lib "MEAIPO.DLL" (ByRef lpGroupMember As IGROUPMEMBERTYPE) As Integer
        Private Declare Function GroupMemberAdd Lib "MEAIPO.DLL" (ByRef lpGroupMember As IGROUPMEMBERTYPE) As Integer
        Private Declare Function GroupMemberEdit Lib "MEAIPO.DLL" (ByRef TargetGroupMember As IGROUPMEMBERTYPE, ByRef NewGroupMember As IGROUPMEMBERTYPE) As Integer
        Private Declare Function GroupMemberRemove Lib "MEAIPO.DLL" (ByRef lpGroupMember As IGROUPMEMBERTYPE) As Integer
        Private Declare Function SetCurrentHost Lib "MEAIPO.DLL" (ByVal CurrentHost As String) As Integer

        Public Function SetHost() As Integer
            SetHost = SetCurrentHost(Host)
        End Function


        Public Function FindFirstGroupMember() As Integer
            Dim CGroupMember As IGROUPMEMBERTYPE

            CGroupMember.Address = Address
            CGroupMember.Postoffice = Postoffice
            CGroupMember.Mailbox = Mailbox
            FindFirstGroupMember = GroupMemberFindFirst(CGroupMember)
            Address = CGroupMember.Address
            Postoffice = CGroupMember.Postoffice
            Mailbox = CGroupMember.Mailbox

        End Function

        Public Function FindNextGroupMember() As Integer
            Dim CGroupMember As IGROUPMEMBERTYPE
            CGroupMember.Address = Address
            CGroupMember.Postoffice = Postoffice
            CGroupMember.Mailbox = Mailbox
            FindNextGroupMember = GroupMemberFindNext(CGroupMember)
            Address = CGroupMember.Address
            Postoffice = CGroupMember.Postoffice
            Mailbox = CGroupMember.Mailbox
        End Function

        Public Function AddGroupMember() As Integer
            Dim CGroupMember As IGROUPMEMBERTYPE
            CGroupMember.Address = Address
            CGroupMember.Postoffice = Postoffice
            CGroupMember.Mailbox = Mailbox
            AddGroupMember = GroupMemberAdd(CGroupMember)
            Address = CGroupMember.Address
            Postoffice = CGroupMember.Postoffice
            Mailbox = CGroupMember.Mailbox
        End Function

        Public Function GetGroupMember() As Integer
            Dim CGroupMember As IGROUPMEMBERTYPE
            CGroupMember.Address = Address
            CGroupMember.Postoffice = Postoffice
            CGroupMember.Mailbox = Mailbox
            GetGroupMember = GroupMemberGet(CGroupMember)
            Address = CGroupMember.Address
            Postoffice = CGroupMember.Postoffice
            Mailbox = CGroupMember.Mailbox
        End Function
        Public Function RemoveGroupMember() As Integer
            Dim CGroupMember As IGROUPMEMBERTYPE
            CGroupMember.Address = Address
            CGroupMember.Postoffice = Postoffice
            CGroupMember.Mailbox = Mailbox
            RemoveGroupMember = GroupMemberRemove(CGroupMember)
            Address = CGroupMember.Address
            Postoffice = CGroupMember.Postoffice
            Mailbox = CGroupMember.Mailbox
        End Function
        Public Function EditGroupMember(ByVal NewAddress As String, ByVal NewPostoffice As String, ByVal NewMailbox As String) As Integer

            Dim CGroupMember As IGROUPMEMBERTYPE
            Dim CGroupMemberData As IGROUPMEMBERTYPE
            ' Get the Find Stuff Set up
            CGroupMember.Address = Address
            CGroupMember.Postoffice = Postoffice
            CGroupMember.Mailbox = Mailbox
            ' Get the Data Set up
            CGroupMemberData.Address = NewAddress
            CGroupMemberData.Postoffice = NewPostoffice
            CGroupMemberData.Mailbox = NewMailbox
            EditGroupMember = GroupMemberEdit(CGroupMember, CGroupMemberData)

            CGroupMemberData.Address = CGroupMemberData.Address
            CGroupMemberData.Postoffice = CGroupMemberData.Postoffice
            CGroupMemberData.Mailbox = CGroupMemberData.Mailbox
        End Function

        Public Property Address() As String
            Get
                Return Me.AddressVal
            End Get
            Set(ByVal value As String)
                Me.AddressVal = value
            End Set
        End Property

        Public Property Postoffice() As String
            Get
                Return Me.PostofficeVal
            End Get
            Set(ByVal value As String)
                Me.PostofficeVal = value
            End Set
        End Property

        Public Property Mailbox() As String
            Get
                Return Me.MailboxVal
            End Get
            Set(ByVal value As String)
                Me.MailboxVal = value
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

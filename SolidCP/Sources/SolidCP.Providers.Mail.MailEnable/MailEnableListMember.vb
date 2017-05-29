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

    Public Class MailEnableListMember
        Inherits MarshalByRefObject

        Private AddressVal As String
        Private AccountNameVal As String
        Private ListNameVal As String
        Private ListMemberTypeVal As Long
        Private StatusVal As Long
        Private HostVal As String


        Private Structure ILISTMEMBERTYPE
            <VBFixedString(256), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=256)> Public Address As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public AccountName As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public ListName As String
            Public Role As Integer
            Public Status As Integer
        End Structure


        Private Declare Function ListMemberGet Lib "MEAILS.DLL" (ByRef lpListMember As ILISTMEMBERTYPE) As Integer
        Private Declare Function ListMemberFindFirst Lib "MEAILS.DLL" (ByRef lpListMember As ILISTMEMBERTYPE) As Integer
        Private Declare Function ListMemberFindNext Lib "MEAILS.DLL" (ByRef lpListMember As ILISTMEMBERTYPE) As Integer
        Private Declare Function ListMemberAdd Lib "MEAILS.DLL" (ByRef lpListMember As ILISTMEMBERTYPE) As Integer
        Private Declare Function ListMemberEdit Lib "MEAILS.DLL" (ByRef TargetListMember As ILISTMEMBERTYPE, ByRef NewListMember As ILISTMEMBERTYPE) As Integer
        Private Declare Function ListMemberRemove Lib "MEAILS.DLL" (ByRef lpListMember As ILISTMEMBERTYPE) As Integer
        Private Declare Function SetCurrentHost Lib "MEAILS.DLL" (ByVal CurrentHost As String) As Integer

        Public Function SetHost() As Integer
            SetHost = SetCurrentHost(Host)
        End Function


        Public Function FindFirstListMember() As Integer
            Dim CListMember As ILISTMEMBERTYPE
            CListMember.Address = Address
            CListMember.AccountName = AccountName
            CListMember.ListName = ListName
            FindFirstListMember = ListMemberFindFirst(CListMember)
            Address = CListMember.Address
            AccountName = CListMember.AccountName
            ListName = CListMember.ListName
        End Function

        Public Function FindNextListMember() As Integer
            Dim CListMember As ILISTMEMBERTYPE
            CListMember.Address = Address
            CListMember.AccountName = AccountName
            CListMember.ListName = ListName
            FindNextListMember = ListMemberFindNext(CListMember)
            Address = CListMember.Address
            AccountName = CListMember.AccountName
            ListName = CListMember.ListName
        End Function

        Public Function AddListMember() As Integer
            Dim CListMember As ILISTMEMBERTYPE
            CListMember.Address = Address
            CListMember.AccountName = AccountName
            CListMember.ListName = ListName
            AddListMember = ListMemberAdd(CListMember)
            Address = CListMember.Address
            AccountName = CListMember.AccountName
            ListName = CListMember.ListName
        End Function

        Public Function GetListMember() As Integer
            Dim CListMember As ILISTMEMBERTYPE
            CListMember.Address = Address
            CListMember.AccountName = AccountName
            CListMember.ListName = ListName
            GetListMember = ListMemberGet(CListMember)
            Address = CListMember.Address
            AccountName = CListMember.AccountName
            ListName = CListMember.ListName
        End Function
        Public Function RemoveListMember() As Integer
            Dim CListMember As ILISTMEMBERTYPE
            CListMember.Address = Address
            CListMember.AccountName = AccountName
            CListMember.ListName = ListName
            RemoveListMember = ListMemberRemove(CListMember)
            Address = CListMember.Address
            AccountName = CListMember.AccountName
            ListName = CListMember.ListName
        End Function

        Public Function EditListMember(ByVal NewAddress, ByVal NewAccountName, ByVal NewListName, ByVal NewListMemberType, ByVal NewStatus) As Integer
            Dim CListMember As ILISTMEMBERTYPE
            Dim CListMemberData As ILISTMEMBERTYPE
            ' Get the Find Stuff Set up
            CListMember.Address = Address
            CListMember.AccountName = AccountName
            CListMember.ListName = ListName
            ' Get the Data Set up
            CListMemberData.Address = NewAddress
            CListMemberData.AccountName = NewAccountName
            CListMemberData.ListName = NewListName
            EditListMember = ListMemberEdit(CListMember, CListMemberData)

            NewAddress = CListMemberData.Address
            NewAccountName = CListMemberData.AccountName
            NewListName = CListMemberData.ListName
        End Function

        Public Property Address() As String
            Get
                Return Me.AddressVal
            End Get
            Set(ByVal value As String)
                Me.AddressVal = value
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

        Public Property ListMemberType() As Long
            Get
                Return Me.ListMemberTypeVal
            End Get
            Set(ByVal value As Long)
                Me.ListMemberTypeVal = value
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

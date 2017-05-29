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

    Public Class MailEnableGroup
        Inherits MarshalByRefObject

        Private RecipientAddressVal As String
        Private PostofficeVal As String
        Private GroupNameVal As String
        Private GroupFileVal As String
        Private GroupStatusVal As Integer
        Private HostVal As String

        Private Structure IGROUPTYPE
            <VBFixedString(1024), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=1024)> Public RecipientAddress As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public Postoffice As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public GroupName As String
            <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public GroupFile As String
            Public GroupStatus As Integer
        End Structure

        Private Declare Function GroupGet Lib "MEAIPO.DLL" (ByRef lpGroup As IGROUPTYPE) As Integer
        Private Declare Function GroupFindFirst Lib "MEAIPO.DLL" (ByRef lpGroup As IGROUPTYPE) As Integer
        Private Declare Function GroupFindNext Lib "MEAIPO.DLL" (ByRef lpGroup As IGROUPTYPE) As Integer
        Private Declare Function GroupAdd Lib "MEAIPO.DLL" (ByRef lpGroup As IGROUPTYPE) As Integer
        Private Declare Function GroupEdit Lib "MEAIPO.DLL" (ByRef TargetGroup As IGROUPTYPE, ByRef NewGroup As IGROUPTYPE) As Integer
        Private Declare Function GroupRemove Lib "MEAIPO.DLL" (ByRef lpGroup As IGROUPTYPE) As Integer
        Private Declare Function SetCurrentHost Lib "MEAIPO.DLL" (ByVal CurrentHost As String) As Integer

        Public Function SetHost() As Integer
            SetHost = SetCurrentHost(Host)
        End Function

        Public Function FindFirstGroup() As Integer
            Dim CGroup As IGROUPTYPE
            CGroup.GroupFile = GroupFile
            CGroup.Postoffice = Postoffice
            CGroup.GroupName = GroupName
            CGroup.RecipientAddress = RecipientAddress
            CGroup.GroupStatus = GroupStatus
            FindFirstGroup = GroupFindFirst(CGroup)
            GroupFile = CGroup.GroupFile
            Postoffice = CGroup.Postoffice
            GroupName = CGroup.GroupName
            RecipientAddress = CGroup.RecipientAddress
            GroupStatus = CGroup.GroupStatus
        End Function
        Public Function FindNextGroup() As Integer
            Dim CGroup As IGROUPTYPE
            CGroup.GroupFile = GroupFile
            CGroup.Postoffice = Postoffice
            CGroup.GroupName = GroupName
            CGroup.RecipientAddress = RecipientAddress
            CGroup.GroupStatus = GroupStatus
            FindNextGroup = GroupFindNext(CGroup)
            GroupFile = CGroup.GroupFile
            Postoffice = CGroup.Postoffice
            GroupName = CGroup.GroupName
            RecipientAddress = CGroup.RecipientAddress
            GroupStatus = CGroup.GroupStatus
        End Function

        Public Function AddGroup() As Integer
            Dim CGroup As IGROUPTYPE
            CGroup.GroupFile = GroupFile
            CGroup.Postoffice = Postoffice
            CGroup.GroupName = GroupName
            CGroup.RecipientAddress = RecipientAddress
            CGroup.GroupStatus = GroupStatus
            AddGroup = GroupAdd(CGroup)
            GroupFile = CGroup.GroupFile
            Postoffice = CGroup.Postoffice
            GroupName = CGroup.GroupName
            RecipientAddress = CGroup.RecipientAddress
            GroupStatus = CGroup.GroupStatus
        End Function

        Public Function GetGroup() As Integer
            Dim CGroup As IGROUPTYPE
            CGroup.GroupFile = GroupFile
            CGroup.Postoffice = Postoffice
            CGroup.GroupName = GroupName
            CGroup.RecipientAddress = RecipientAddress
            CGroup.GroupStatus = GroupStatus
            GetGroup = GroupGet(CGroup)
            GroupFile = CGroup.GroupFile
            Postoffice = CGroup.Postoffice
            GroupName = CGroup.GroupName
            RecipientAddress = CGroup.RecipientAddress
            GroupStatus = CGroup.GroupStatus
        End Function
        Public Function RemoveGroup() As Integer
            Dim CGroup As IGROUPTYPE
            CGroup.GroupFile = GroupFile
            CGroup.Postoffice = Postoffice
            CGroup.GroupName = GroupName
            CGroup.RecipientAddress = RecipientAddress
            CGroup.GroupStatus = GroupStatus
            RemoveGroup = GroupRemove(CGroup)
            GroupFile = CGroup.GroupFile
            Postoffice = CGroup.Postoffice
            GroupName = CGroup.GroupName
            RecipientAddress = CGroup.RecipientAddress
            GroupStatus = CGroup.GroupStatus
        End Function

        Public Function EditGroup(ByVal NewRecipientAddress As String, ByVal NewPostoffice As String, ByVal NewGroupName As String, ByVal NewGroupFile As String, ByVal NewGroupStatus As Integer) As Integer
            Dim CGroup As IGROUPTYPE
            Dim CGroupData As IGROUPTYPE
            ' Get the Find Stuff Set up
            CGroup.GroupFile = GroupFile
            CGroup.GroupName = GroupName
            CGroup.Postoffice = Postoffice
            CGroup.RecipientAddress = RecipientAddress
            CGroup.GroupStatus = GroupStatus
            ' Get the Data Set up
            CGroupData.GroupFile = NewGroupFile
            CGroupData.GroupName = NewGroupName
            CGroupData.Postoffice = NewPostoffice
            CGroupData.RecipientAddress = NewRecipientAddress
            CGroupData.GroupStatus = NewGroupStatus

            EditGroup = GroupEdit(CGroup, CGroupData)
            GroupFile = CGroupData.GroupFile
            Postoffice = CGroupData.Postoffice
            GroupName = CGroupData.GroupName
            RecipientAddress = CGroupData.RecipientAddress
            GroupStatus = CGroup.GroupStatus
        End Function

        Private Sub AddDataTableColumns(ByRef oTable As DataTable)
            oTable.Columns.Add("GroupName", GetType(String))
            oTable.Columns.Add("GroupStatus", GetType(Long))
            oTable.Columns.Add("RecipientAddress", GetType(String))
        End Sub

        Public Property RecipientAddress() As String
            Get
                Return Me.RecipientAddressVal
            End Get
            Set(ByVal value As String)
                Me.RecipientAddressVal = value
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

        Public Property GroupName() As String
            Get
                Return Me.GroupNameVal
            End Get
            Set(ByVal value As String)
                Me.GroupNameVal = value
            End Set
        End Property

        Public Property GroupFile() As String
            Get
                Return Me.GroupFileVal
            End Get
            Set(ByVal value As String)
                Me.GroupFileVal = value
            End Set
        End Property

        Public Property GroupStatus() As Integer
            Get
                Return Me.GroupStatusVal
            End Get
            Set(ByVal value As Integer)
                Me.GroupStatusVal = value
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
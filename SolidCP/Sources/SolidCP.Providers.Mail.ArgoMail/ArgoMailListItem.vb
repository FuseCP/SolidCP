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


Friend Class ArgoMailListItem
    Private _Id As String
    Private _listItemName As String
    Private _Account As String
    Private _iDescriptionLines As Integer
    Private _desription As String
    Private _iMembersCount As Integer
    Private _members() As String
    Private _bListISClosed As Boolean
    Private _bRepliesGoToSender As Boolean
    Private _bRequireMemberShip As Boolean
    Private _diskSpace As Long


    Public Sub New()
    End Sub 'New

    Public Property ID() As String
        Get
            Return _Id
        End Get

        Set(ByVal value As String)
            _Id = Value
        End Set
    End Property


    Public Property Name() As String
        Get
            Return _listItemName
        End Get

        Set(ByVal value As String)
            _listItemName = value
        End Set
    End Property


    Public Property Account() As String
        Get
            Return _Account
        End Get

        Set(ByVal value As String)
            _Account = Value
        End Set
    End Property


    Public Property DescriptionLines() As Integer
        Get
            Return _iDescriptionLines
        End Get

        Set(ByVal value As Integer)
            _iDescriptionLines = Value
        End Set
    End Property


    Public Property Desription() As String
        Get
            Return _desription
        End Get

        Set(ByVal value As String)
            _desription = Value
        End Set
    End Property


    Public Property Count() As Integer
        Get
            Return _iMembersCount
        End Get

        Set(ByVal value As Integer)
            _iMembersCount = Value
        End Set
    End Property


    Public Property Members() As String()
        Get
            Return _members
        End Get

        Set(ByVal value As String())
            _members = Value
        End Set
    End Property


    Public Property ListISClosed() As Boolean
        Get
            Return _bListISClosed
        End Get

        Set(ByVal value As Boolean)
            _bListISClosed = Value
        End Set
    End Property


    Public Property RepliesGoToSender() As Boolean
        Get
            Return _bRepliesGoToSender
        End Get

        Set(ByVal value As Boolean)
            _bRepliesGoToSender = Value
        End Set
    End Property


    Public Property RequireMemberShip() As Boolean
        Get
            Return _bRequireMemberShip
        End Get

        Set(ByVal value As Boolean)
            _bRequireMemberShip = Value
        End Set
    End Property


    Public Property DiskSpace() As Long
        Get
            Return _diskSpace
        End Get
        Set(ByVal value As Long)
            _diskSpace = Value
        End Set
    End Property
End Class 'ArgoMailListItem
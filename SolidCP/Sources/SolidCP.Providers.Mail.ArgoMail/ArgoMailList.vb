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

Imports Microsoft.Win32
Imports System.IO
Imports System.Security.AccessControl


Friend Class ArgoMailLists
    Private _mailListPath As String = ""
    Private mailListItems As New List(Of ArgoMailListItem)

    Public Sub New()
        Dim locKey As RegistryKey = Registry.LocalMachine
        Dim argoKey As RegistryKey = locKey.OpenSubKey("SOFTWARE\ArGoSoft\Mail Server\Setup", False)

        If Not (argoKey Is Nothing) Then
            Dim argoInst As String = CStr(argoKey.GetValue("Program Path"))
            If argoInst <> "" Then
                _mailListPath = argoInst + "_maillists\"
                ReadLists()
            End If
        End If
    End Sub 'New


    Public Sub New(ByVal loadLists As Boolean)
        Dim locKey As RegistryKey = Registry.LocalMachine
        Dim argoKey As RegistryKey = locKey.OpenSubKey("SOFTWARE\ArGoSoft\Mail Server\Setup", False)

        If Not argoKey Is Nothing Then
            Dim argoInst As String = CStr(argoKey.GetValue("Program Path"))
            If Not String.IsNullOrEmpty(argoInst) Then
                _mailListPath = argoInst + "_maillists\"
                If loadLists Then
                    ReadLists()
                End If
            End If
        End If
    End Sub 'New 

    Public Property Items() As List(Of ArgoMailListItem)
        Get
            Return mailListItems
        End Get
        Set(ByVal value As List(Of ArgoMailListItem))
            mailListItems = Value
        End Set
    End Property

    Public Property ListPath() As String
        Get
            Return _mailListPath
        End Get
        Set(ByVal value As String)
            _mailListPath = Value
        End Set
    End Property

    Public Sub Add(ByRef item As ArgoMailListItem)
        Try
            Dim sFile As String = _mailListPath + item.Name
            AddListItem(item, sFile)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub 'Add


    Public Sub Update(ByVal item As ArgoMailListItem)
        Try
            Dim sFile As String = _mailListPath + item.Name
            AddListItem(item, sFile)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub 'Update


    Public Sub Delete(ByVal listName As String)
        Dim sFile As String = Nothing
        Dim item As ArgoMailListItem = FindItem(listName)
        If Not (item Is Nothing) Then
            sFile = _mailListPath + item.Name
            File.Delete(sFile)
        End If
    End Sub 'Delete


    Public Function IndexOf(ByVal listName As String) As Integer
        Dim item As ArgoMailListItem = FindItem(listName)
        If item Is Nothing Then
            Return -1
        Else
            Return mailListItems.IndexOf(item)
        End If
    End Function 'IndexOf

    Public Overloads Function GetItem(ByVal listName As String) As ArgoMailListItem
        Return FindItem(listName)
    End Function 'GetItem


    Public Overloads Function GetItem(ByVal index As Integer) As ArgoMailListItem
        If index <= mailListItems.Count Then
            Return mailListItems(index)
        Else
            Return Nothing
        End If
    End Function 'GetItem

    Public Function TotalDiskSpace(ByVal domainName As String) As Long
        Dim lTotSpace As Long = 0
        Dim alAcc As ArrayList = DomainAccounts(domainName)
        Dim user As Object = ArgoMail.CreateUserObject()

        For Each user In alAcc
            Dim idx As Integer = user.UserName.IndexOf("@")
            If idx >= 0 Then
                lTotSpace += CalcDiskSpace((_mailListPath + "_users\" + domainName + "\" + user.UserName.Substring(0, idx) + "\Inbox\"))
            End If
        Next user
        Return lTotSpace
    End Function 'TotalDiskSpace


    Public Function MailBoxDiskSpace(ByVal mailBox As String) As Long
        Dim lTotSpace As Long = 0
        Dim idx As Integer = mailBox.IndexOf("@")
        If idx >= 0 Then
            Dim account As String = mailBox.Substring(0, idx)
            Dim domain As String = mailBox.Substring((idx + 1))
            lTotSpace += CalcDiskSpace((_mailListPath + "_users\" + domain + "\" + account + "\Inbox\"))
        End If
        Return lTotSpace
    End Function 'MailBoxDiskSpace


    Private Sub ReadLists()
        Dim aFiles As String() = Directory.GetFiles(_mailListPath, "*.")
        Dim read As StreamReader = Nothing

        Try
            Dim file As String
            For Each file In aFiles
                Dim newlist As New ArgoMailListItem()

                read = New StreamReader(file)
                Dim data As String

                data = read.ReadLine()
                If Not (data Is Nothing) Then
                    newlist.ID = data
                End If
                data = read.ReadLine()
                If Not (data Is Nothing) Then
                    newlist.Name = data
                End If
                data = read.ReadLine()
                If Not (data Is Nothing) Then
                    newlist.Account = data
                End If
                data = read.ReadLine()
                If Not (data Is Nothing) Then
                    newlist.DescriptionLines = Convert.ToInt32(data)
                    If newlist.DescriptionLines > 0 Then
                        Dim idx As Integer
                        For idx = 0 To newlist.DescriptionLines - 1
                            data = read.ReadLine()
                            If Not (data Is Nothing) Then
                                newlist.Desription = String.Concat(newlist.Desription, data)
                            End If
                        Next idx
                    Else
                        newlist.Desription = ""
                    End If
                End If

                data = read.ReadLine()
                If Not (data Is Nothing) Then
                    newlist.Count = Convert.ToInt32(data)
                    newlist.Members = New String(newlist.Count) {}
                    If newlist.Count > 0 Then
                        Dim idx As Integer
                        For idx = 0 To newlist.Count - 1
                            data = read.ReadLine()
                            If Not (data Is Nothing) Then
                                newlist.Members(idx) = data
                            End If
                        Next idx
                    End If
                End If
                data = read.ReadLine()
                If Not data Is Nothing Then
                    If data <> "0" Then
                        newlist.ListISClosed = True
                    Else
                        newlist.ListISClosed = False
                    End If
                End If

                data = read.ReadLine()
                If Not data Is Nothing Then
                    If data <> "0" Then
                        newlist.RepliesGoToSender = True
                    Else
                        newlist.RepliesGoToSender = False
                    End If
                End If

                data = read.ReadLine()
                If Not (data Is Nothing) Then
                    If data <> "0" Then
                        newlist.RequireMemberShip = True
                    Else
                        newlist.RequireMemberShip = False
                    End If
                End If
                newlist.DiskSpace = read.BaseStream.Length

                mailListItems.Add(newlist)
            Next file
        Catch

        Finally
            If Not (read Is Nothing) Then
                read.Close()
            End If
        End Try
    End Sub 'ReadLists

    Private Sub AddListItem(ByVal item As ArgoMailListItem, ByVal sFile As String)
        Dim writer As StreamWriter = Nothing

        Try
            writer = New StreamWriter(sFile)
            writer.WriteLine(item.ID)
            writer.WriteLine(item.Name)
            writer.WriteLine(item.Account)

            If String.IsNullOrEmpty(item.Desription) Then
                item.Desription = String.Empty
            End If

            Dim aDesc As String() = item.Desription.TrimEnd(ControlChars.Lf).Split(ControlChars.Lf)
            item.DescriptionLines = aDesc.Length
            writer.WriteLine(item.DescriptionLines.ToString())
            If item.DescriptionLines > 0 Then
                Dim idx As Integer
                For idx = 0 To item.DescriptionLines - 1
                    writer.WriteLine(aDesc(idx).TrimEnd(ControlChars.Cr))
                Next idx
            Else
                If item.Desription <> "" Then
                    writer.WriteLine(item.Desription)
                End If
            End If
            writer.WriteLine(item.Count.ToString())
            If item.Count > 0 Then
                Dim idx As Integer
                For idx = 0 To item.Count - 1
                    writer.WriteLine(item.Members(idx))
                Next idx
            End If
            If item.ListISClosed Then
                writer.WriteLine("1")
            Else
                writer.WriteLine("0")
            End If
            If item.RepliesGoToSender Then
                writer.WriteLine("1")
            Else
                writer.WriteLine("0")
            End If
            If item.RequireMemberShip Then
                writer.WriteLine("1")
            Else
                writer.WriteLine("0")
            End If
        Catch ex As Exception

            Throw ex
        Finally
            If Not (writer Is Nothing) Then
                writer.Close()
            End If
        End Try
    End Sub 'AddListItem

    Private Function FindItem(ByVal listName As String) As ArgoMailListItem
        Dim item As ArgoMailListItem
        For Each item In mailListItems
            If item.Name = listName Then
                Return item
            End If
            If item.Account = listName Then
                Return item
            End If
        Next item
        Return Nothing
    End Function 'FindItem


    Private Function CalcDiskSpace(ByVal sPath As String) As Long
        Dim lDiskSpace As Long = 0
        Dim aFiles As String() = Directory.GetFiles(sPath, "*.eml")

        Dim file As String
        For Each file In aFiles
            Dim inf As New FileInfo(file)
            lDiskSpace += inf.Length
        Next file
        Return lDiskSpace
    End Function 'CalcDiskSpace


    Private Function DomainAccounts(ByVal domainName As String) As ArrayList
        Dim alUsers As New ArrayList()
        Dim domainService As Service = ArgoMail.LoadLocalDomainsService()

        If domainService.Succeed Then
            Dim domainIndex As Integer = domainService.ComObject.IndexOf(domainName)

            If domainIndex >= 0 Then
                Dim userService As Service = ArgoMail.LoadUsersService()
                If userService.Succeed Then
                    If userService.ComObject.Count > 0 Then
                        Dim user As Object = Nothing
                        Dim index As Integer

                        For index = 0 To userService.ComObject.Count - 1
                            user = userService.ComObject.Items(index)
                            If user.UserName.IndexOf(domainName) >= 0 Then
                                alUsers.Add(user)
                            End If
                        Next index
                    End If
                End If
            End If
        End If

        Return alUsers
    End Function 'DomainAccounts
End Class 'ArgoMailLists

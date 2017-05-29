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

Imports System.Runtime.InteropServices


Public Class MDaemonInterop

#Region "Constants"
    Friend Const MDUSERDLL_MDAEMONINIDB As Integer = 0
    Friend Const MDUSERDLL_DOMAINDB As Integer = 1
    Friend Const MDUSERDLL_HELPDB As Integer = 2
    Friend Const MDUSERDLL_DELWARNDB As Integer = 3
    Friend Const MDUSERDLL_DELERRDB As Integer = 4
    Friend Const MDUSERDLL_RECEIPTDB As Integer = 5
    Friend Const MDUSERDLL_NOCOMMANDDB As Integer = 6
    Friend Const MDUSERDLL_NOSUCHUSERDB As Integer = 7
    Friend Const MDUSERDLL_ACCTINFODB As Integer = 8
    Friend Const MDUSERDLL_WELCOMEDB As Integer = 9
    Friend Const MDUSERDLL_LOCATIONSDB As Integer = 10
    Friend Const MDUSERDLL_MIMETYPEDB As Integer = 11
    Friend Const MDUSERDLL_ALIASDB As Integer = 12
    Friend Const MDUSERDLL_TRANSLATEDB As Integer = 13
    Friend Const MDUSERDLL_TRANSEXCPTDB As Integer = 14
    Friend Const MDUSERDLL_WEBACCESSDB As Integer = 15
    Friend Const MDUSERDLL_RFC822TMPDB As Integer = 16
    Friend Const MDUSERDLL_DIGESTTMPDB As Integer = 17
    Friend Const MDUSERDLL_IPSHIELDDB As Integer = 18
    Friend Const MDUSERDLL_FWDDB As Integer = 19
    Friend Const MDUSERDLL_SIGDB As Integer = 20
    Friend Const MDUSERDLL_MAILFORMATDB As Integer = 21
    Friend Const MDUSERDLL_AUTORESPDB As Integer = 22
    Friend Const MDUSERDLL_GATEWAYDB As Integer = 23
    Friend Const MDUSERDLL_REFUSALDB As Integer = 24
    Friend Const MDUSERDLL_LOCALONLYDB As Integer = 25
    Friend Const MDUSERDLL_MULTIPOPDB As Integer = 26
    Friend Const MDUSERDLL_IPCACHEDB As Integer = 27
    Friend Const MDUSERDLL_MXCACHEDB As Integer = 28
    Friend Const MDUSERDLL_NOCACHEDB As Integer = 29
    Friend Const MDUSERDLL_PRIORITYDB As Integer = 30
    Friend Const MDUSERDLL_EXCEPTIONDB As Integer = 31
    Friend Const MDUSERDLL_DELUNLESSDB As Integer = 32
    Friend Const MDUSERDLL_FWDUNLESSDB As Integer = 33
    Friend Const MDUSERDLL_DVUNLESSDB As Integer = 34
    Friend Const MDUSERDLL_MSGIDDB As Integer = 35
    Friend Const MDUSERDLL_LANDOMAINDB As Integer = 36
    Friend Const MDUSERDLL_IPSCREENDB As Integer = 37
    Friend Const MDUSERDLL_RELAYDB As Integer = 38
    Friend Const MDUSERDLL_AUTHDB As Integer = 39
    Friend Const MDUSERDLL_USERLISTDB As Integer = 40
    Friend Const MDUSERDLL_DOMAINPOPDB As Integer = 41
    Friend Const MDUSERDLL_SPAMBLOCKERDB As Integer = 42
    Friend Const MDUSERDLL_SPAMEXCEPTDB As Integer = 43
    Friend Const MDUSERDLL_SPAMCACHEDB As Integer = 44
    Friend Const MDUSERDLL_LDAPDB As Integer = 45
    Friend Const MDUSERDLL_SCHEDULEDB As Integer = 46
    Friend Const MDUSERDLL_CFILTERDB As Integer = 47
    Friend Const MDUSERDLL_POLICYDB As Integer = 48
    Friend Const MDUSERDLL_DPOPXTRADB As Integer = 49
    Friend Const MDUSERDLL_VARIABLEDB As Integer = 50
    Friend Const MDUSERDLL_AVSCHEDULEDB As Integer = 51
    Friend Const MDUSERDLL_GUARDIANDB As Integer = 52
    Friend Const MDUSERDLL_CENSORDB As Integer = 53
    Friend Const MDUSERDLL_CFADMDB As Integer = 54
    Friend Const MDUSERDLL_CFRECDB As Integer = 55
    Friend Const MDUSERDLL_CFSNDDB As Integer = 56
    Friend Const MDUSERDLL_CFVIRADMDB As Integer = 57
    Friend Const MDUSERDLL_CFVIRRECDB As Integer = 58
    Friend Const MDUSERDLL_CFVIRSNDDB As Integer = 59
    Friend Const MDUSERDLL_CFVIRWRNDB As Integer = 60
    Friend Const MDUSERDLL_POPLOCKDB As Integer = 61
    Friend Const MDUSERDLL_CFVIRUPDATEDB As Integer = 62
    Friend Const MDUSERDLL_CFCOMPRESSDB As Integer = 63
    Friend Const MDUSERDLL_CFDELFILESDB As Integer = 64
    Friend Const MDUSERDLL_CFDOMAINEXCDB As Integer = 65
    Friend Const MDUSERDLL_CFEXCLUDESDB As Integer = 66
    Friend Const MDUSERDLL_CALENDARDB As Integer = 67
    Friend Const MDUSERDLL_REMINDERSDB As Integer = 68
    Friend Const MDUSERDLL_CALREMINDDB As Integer = 69
    Friend Const MDUSERDLL_LISTPRUNEDB As Integer = 70
    Friend Const MDUSERDLL_QCOUNTSDB As Integer = 71
    Friend Const MDUSERDLL_OVERQUOTADB As Integer = 72
    Friend Const MDUSERDLL_PFDATADB As Integer = 73
    Friend Const MDUSERDLL_ACLSHLOOKUPDB As Integer = 74
    Friend Const MDUSERDLL_RECENTDB As Integer = 75
    Friend Const MDUSERDLL_TARPITDB As Integer = 76
    Friend Const MDUSERDLL_CFRULESDB As Integer = 77
    Friend Const MDUSERDLL_CFBOUNCEDB As Integer = 78
    Friend Const MDUSERDLL_AUTORESPEXCEPTDB As Integer = 79
    Friend Const MDUSERDLL_ASSCHEDULEDB As Integer = 80
    Friend Const MDUSERDLL_CFSAUPDATEDB As Integer = 81
    Friend Const MDUSERDLL_TARPITCONNECTDB As Integer = 82
    Friend Const MDUSERDLL_CFSYSRULESDB As Integer = 83
    Friend Const MDUSERDLL_NOTARPITDB As Integer = 84
    Friend Const MDUSERDLL_SPFCACHEDB As Integer = 85
    Friend Const MDUSERDLL_SPFEXCEPTDB As Integer = 86
    Friend Const MDUSERDLL_REVERSEEXCEPTDB As Integer = 87
    Friend Const MDUSERDLL_DKVRFYEXCEPTDB As Integer = 88
    Friend Const MDUSERDLL_DKCACHEDB As Integer = 89
    Friend Const MDUSERDLL_DKSIGNLISTDB As Integer = 90
    Friend Const MDUSERDLL_HCMINTLISTDB As Integer = 91
    Friend Const MDUSERDLL_TASKREMINDDB As Integer = 92
    Friend Const MDUSERDLL_LDAPCACHEDB As Integer = 93
    Friend Const MDUSERDLL_GREYLISTDB As Integer = 94
    Friend Const MDUSERDLL_NOGREYLISTDB As Integer = 95
    Friend Const MDUSERDLL_ALLDB As Integer = 96 ' This must be last and represents all of the above files
    Friend Const MDUSERDLL_WORLDCLIENTINIDB As Integer = 100 ' not cached
    Friend Const MDUSERDLL_WORLDCLIENTDOMAINDB As Integer = 101 ' not cached
    Friend Const MDUSERDLL_GROUPWAREUSRDB As Integer = 102 ' not cached
    Friend Const MDUSERDLL_CFSPAMEXCLUDEDB As Integer = 103 ' not cached
    Friend Const MDUSERDLL_SUBCONFDB As Integer = 104 ' not cached
    Friend Const MDUSERDLL_UNSUBCONFDB As Integer = 105 ' not cached
    Friend Const MDLISTERR_NOERROR As Integer = 1
    Friend Const MDLISTERR_INVALIDLISTNAME As Integer = 2
    Friend Const MDLISTERR_INVALIDEMAILADDRESS As Integer = 3
    Friend Const MDLISTERR_INVALIDREMOTEHOST As Integer = 4

    Friend Const MDLIST_NORMAL As Integer = 1
    Friend Const MDLIST_POSTONLY As Integer = 2
    Friend Const MDLIST_READONLY As Integer = 3
    Friend Const MDLIST_DIGEST As Integer = 4

    Friend Const MDLIST_ENABLEDIGESTS As Integer = &H1
    Friend Const MDLIST_FORCEDIGESTUSE As Integer = &H2
    Friend Const MDLIST_HTMLDIGESTS As Integer = &H4
    Friend Const MDLIST_ARCHIVEDIGESTS As Integer = &H8
    Friend Const MDLIST_NINE As Integer = &H10
    Friend Const MDLIST_TWELVE As Integer = &H20
    Friend Const MDLIST_THREE As Integer = &H40
    Friend Const MDLIST_SIX As Integer = &H80
    Friend Const MDLIST_AM As Integer = &H100
    Friend Const MDLIST_PM As Integer = &H200

    Friend Const MDLIST_AUTOPRUNE As Integer = &H1
    Friend Const MDLIST_PRIVATE As Integer = &H2
    Friend Const MDLIST_ALLOWEXPN As Integer = &H4
    Friend Const MDLIST_LISTNAMEINSUBJECT As Integer = &H8
    Friend Const MDLIST_THREADNUMBINSUBJECT As Integer = &H10
    Friend Const MDLIST_USEMEMBERNAMES As Integer = &H20
    Friend Const MDLIST_USELISTNAME As Integer = &H40
    Friend Const MDLIST_USESTANDARDNAME As Integer = &H80
    Friend Const MDLIST_INSERTCAPTION As Integer = &H100
    Friend Const MDLIST_CRACKMESSAGE As Integer = &H200
    Friend Const MDLIST_FORCEUNIQUEID As Integer = &H400
    Friend Const MDLIST_IGNORERCPTERRORS As Integer = &H800
    Friend Const MDLIST_MODERATED As Integer = &H1000
    Friend Const MDLIST_SUBSCRIBENOTE As Integer = &H2000
    Friend Const MDLIST_UNSUBSCRIBENOTE As Integer = &H4000
    Friend Const MDLIST_MSGTOOBIGNOTE As Integer = &H8000
    Friend Const MDLIST_INFORMNONMEMBER As Integer = &H10000
    Friend Const MDLIST_SENDSTATUSMESSAGES As Integer = &H20000
    Friend Const MDLIST_ALLOWSUBSCRIBE As Integer = &H40000
    Friend Const MDLIST_AUTHSUBSCRIBE As Integer = &H80000
    Friend Const MDLIST_AUTHAUTOSUBSCRIBE As Integer = &H100000
    Friend Const MDLIST_ALLOWUNSUBSCRIBE As Integer = &H200000
    Friend Const MDLIST_AUTHUNSUBSCRIBE As Integer = &H400000
    Friend Const MDLIST_AUTHAUTOUNSUBSCRIBE As Integer = &H800000
    Friend Const MDLIST_PASSWORDPOST As Integer = &H1000000
    Friend Const MDLIST_USEPUBLICFOLDER As Integer = &H2000000
    Friend Const MDLIST_HIDEFROMADDRESSBOOK As Integer = &H4000000
#End Region

    <DllImport("kernel32", EntryPoint:="GetPrivateProfileSectionW", CharSet:=CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Public Shared Function GetPrivateProfileSection(<MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpAppName As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpReturnedString As String, ByVal nSize As Integer, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpFileName As String) As Integer
    End Function


    <DllImport("kernel32", EntryPoint:="GetPrivateProfileStringW", CharSet:=CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Public Shared Function GetPrivateProfileString(<MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpApplicationName As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpKeyName As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpDefault As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpReturnedString As String, ByVal nSize As Integer, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpFileName As String) As Integer
    End Function


    <DllImport("kernel32", EntryPoint:="WritePrivateProfileSectionA", CharSet:=CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Public Shared Function WritePrivateProfileSection(<MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpAppName As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpString As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpFileName As String) As Integer
    End Function

    <DllImport("kernel32", EntryPoint:="WritePrivateProfileStringW", CharSet:=CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Public Shared Function WritePrivateProfileString(<MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpApplicationName As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpKeyName As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpString As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpFileName As String) As Integer
    End Function
End Class

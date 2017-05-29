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

Friend Enum MD_ERROR
    MDDLLERR_NOTFOUND = -1
    MDDLLERR_NOERROR = 0
    MDDLLERR_MBXHASDOMAIN = 1324
    MDDLLERR_BASE = 1324
    MDDLLERR_USEREXISTS = 1325
    MDDLLERR_INVALIDFULLNAME = 1326
    MDDLLERR_INVALIDMAILBOX = 1327
    MDDLLERR_INVALIDMAILDIR = 1328
    MDDLLERR_INVALIDPASSWORD = 1329
    MDDLLERR_INVALIDFWD = 1330
    MDDLLERR_POSTMASTER = 1331
    MDDLLERR_LOGONINUSE = 1332
    MDDLLERR_INVALIDCNTQUOTA = 1333
    MDDLLERR_INVALIDDISKQUOTA = 1334
    MDDLLERR_CANTCREATEMAILDIR = 1336
    MDDLLERR_CANTCREATEFILEDIR = 1337
    MDDLLERR_TOOMANYACCOUNTS = 1338
    MDDLLERR_INVALIDRECORD = 1339
    MDDLLERR_MISSINGTO = 1340
    MDDLLERR_MISSINGFROM = 1341
    MDDLLERR_MISSINGBODY = 1342
    MDDLLERR_MISSINGBODYFILE = 1343
    MDDLLERR_MISSINGATTACHMENTFILE = 1344
    MDDLLERR_MISSINGRAWPATH = 1345
    MDDLLERR_CANTGENRAWFILENAME = 1346
    MDDLLERR_CANTLOCKRAWFILE = 1347
    MDDLLERR_CANTCREATERAWFILE = 1348
    MDDLLERR_CANTACCESSBODYFILE = 1349
    MDDLLERR_LDAP_BASE = 1600
End Enum
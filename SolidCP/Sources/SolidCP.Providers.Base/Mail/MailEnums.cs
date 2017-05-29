// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;

namespace SolidCP.Providers.Mail
{
	/// <summary>
	/// Summary description for MailboxRights.
	/// </summary>
	public enum ReplyTo 
	{ 
		RepliesToList = 0, 
		RepliesToSender = 1, 
		RepliesToModerator = 2
    }

    #region MailEnable
    public enum PostingMode
	{ 
		MembersCanPost = 0,
		AnyoneCanPost = 1,
		PasswordProtectedPosting = 2,
        ModeratorCanPost = 3
	}

    public enum PrefixOption
    {
        Default = 0,
        Altered = 1,
        CustomPrefix = 2
    }
    #endregion

    #region Merak
    public enum PasswordProtection
    {
        NoProtection = 0,
        ClientModerated = 1,
        ServerModerated = 2
    }

    #endregion

    #region IceWarp

    public enum IceWarpListMembersSource
    {
        MembersInFile = 0,
        AllDomainUsers = 1,
        AllDomainAdmins = 3
    }

    public enum IceWarpListFromAndReplyToHeader
    {
        NoChange = 0,
        SetToSender = 1,
        SetToValue = 2
    }

    public enum IceWarpListOriginator
    {
        Blank = 0,
        Sender = 1,
        Owner = 2
    }

    [Flags]
    public enum IceWarpListDefaultRights
    {
        Receive = 1,
        Post = 2,
        Digest = 4
    }

    public enum IceWarpListConfirmSubscription
    {
        None = 0,
        User = 1,
        Owner = 2
    }
    #endregion
}

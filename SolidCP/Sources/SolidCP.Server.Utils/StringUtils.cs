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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.Providers.Utils
{
    public class StringUtils
    {
        public static string ReplaceStringVariable(string str, string variable, string value)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(value))
                return str;

            Regex re = new Regex("\\[" + variable + "\\]+", RegexOptions.IgnoreCase);
            return re.Replace(str, value);
        }

        public static string CleanIdentifier(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            return Regex.Replace(str, "\\W+", "_", RegexOptions.Compiled);
        }

        public static string CleanupASCIIControlCharacters(string s)
        {
            byte[] invalidCharacters = { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0xB,
                             0xC, 0xE, 0xF, 0x10, 0x11, 0x12, 0x14, 0x15, 0x16,
                             0x17, 0x18, 0x1A, 0x1B, 0x1E, 0x1F, 0x7F };

            byte[] sanitizedBytes = (from a in Encoding.UTF8.GetBytes(s)
                                     where !invalidCharacters.Contains(a)
                                     select a).ToArray();

            return Encoding.UTF8.GetString(sanitizedBytes);
        }
    }
}
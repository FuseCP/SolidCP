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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SolidCP.Providers.Utils
{
    public class JsonUtils
    {
        /// <summary>
        /// A brute force convertion to Json. Any serialization Exceptions are ignored. Formatting: Indented
        /// </summary>
        /// <remarks>
        /// See also
        /// <seealso cref="Objects.CloneBySerialization{T}"/>  //todo alternatieven bespreken
        /// <seealso cref="Objects.CloneBySerializationSimple{T}"/>
        /// </remarks>
        public static string ConvertToJsonForLogging<T>(T source, bool indent = true, string method = "")
        {
            try
            {
                if (source is JObject retval) // no need to convert a JObject
                    return retval.ToString();

                // Create settings to suppress any serialization Exceptions
                var settings = new JsonSerializerSettings
                {
                    Error = (serializer, err) => err.ErrorContext.Handled = true,
                    Formatting = (indent ? Formatting.Indented : Formatting.None)
                };

                // Serialze the sourceObject
                return JsonConvert.SerializeObject(source, settings);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"[{method}] ConvertToJson failed. Error: {ex.Message}");
            }
        }
    }
}
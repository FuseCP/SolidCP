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
using System.Security.AccessControl;
using System.Text;
using Microsoft.Web.Administration;

namespace SolidCP.Providers.Web.Handlers
{
	internal sealed class HandlerAction : ConfigurationElement
	{
		// Fields
		private static readonly string ModulesAttribute = "modules";
		private static readonly string NameAttribute = "name";
		private static readonly string PathAttribute = "path";
		private static readonly string PreConditionAttribute = "preCondition";
		private static readonly string RequireAccessAttribute = "requireAccess";
		private static readonly string ResourceTypeAttribute = "resourceType";
		private static readonly string ScriptProcessorAttribute = "scriptProcessor";
		private static readonly string TypeAttribute = "type";
		private static readonly string VerbAttribute = "verb";

		// Properties
		public string Modules
		{
			get
			{
				return (string)base[ModulesAttribute];
			}
			set
			{
				base[ModulesAttribute] = value;
			}
		}

		public string Name
		{
			get
			{
				return (string)base[NameAttribute];
			}
			set
			{
				base[NameAttribute] = value;
			}
		}

		public string Path
		{
			get
			{
				return (string)base[PathAttribute];
			}
			set
			{
				base[PathAttribute] = value;
			}
		}

		public string PreCondition
		{
			get
			{
				return (string)base[PreConditionAttribute];
			}
			set
			{
				base[PreConditionAttribute] = value;
			}
		}

		public HandlerRequiredAccess RequireAccess
		{
			get
			{
				return (HandlerRequiredAccess)base[RequireAccessAttribute];
			}
			set
			{
				base[RequireAccessAttribute] = (int)value;
			}
		}

		public ResourceType ResourceType
		{
			get
			{
				return (ResourceType)base[ResourceTypeAttribute];
			}
			set
			{
				base[ResourceTypeAttribute] = (int)value;
			}
		}

		public string ScriptProcessor
		{
			get
			{
				return (string)base[ScriptProcessorAttribute];
			}
			set
			{
				base[ScriptProcessorAttribute] = value;
			}
		}

		public string Type
		{
			get
			{
				return (string)base[TypeAttribute];
			}
			set
			{
				base[TypeAttribute] = value;
			}
		}

		public string Verb
		{
			get
			{
				return (string)base[VerbAttribute];
			}
			set
			{
				base[VerbAttribute] = value;
			}
		}
	}

 

}

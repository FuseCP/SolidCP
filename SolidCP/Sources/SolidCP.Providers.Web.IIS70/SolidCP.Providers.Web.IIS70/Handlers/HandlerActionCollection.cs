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
using System.Text;
using Microsoft.Web.Administration;

namespace SolidCP.Providers.Web.Handlers
{
	internal sealed class HandlerActionCollection : ConfigurationElementCollectionBase<HandlerAction>
	{
		// Methods
		public HandlerAction AddAt(int index, string name, string path, string verb)
		{
			HandlerAction element = base.CreateElement();
			element.Name = name;
			element.Path = path;
			element.Verb = verb;
			return base.AddAt(index, element);
		}

		public HandlerAction AddCopy(HandlerAction action)
		{
			HandlerAction destination = base.CreateElement();
			CopyInfo(action, destination);
			return base.Add(destination);
		}

		public HandlerAction AddCopyAt(int index, HandlerAction action)
		{
			HandlerAction destination = base.CreateElement();
			CopyInfo(action, destination);
			return base.AddAt(index, destination);
		}

		private static void CopyInfo(HandlerAction source, HandlerAction destination)
		{
			destination.Name = source.Name;
			destination.Modules = source.Modules;
			destination.Path = source.Path;
			destination.PreCondition = source.PreCondition;
			destination.RequireAccess = source.RequireAccess;
			destination.ResourceType = source.ResourceType;
			destination.ScriptProcessor = source.ScriptProcessor;
			destination.Type = source.Type;
			destination.Verb = source.Verb;
		}

		protected override HandlerAction CreateNewElement(string elementTagName)
		{
			return new HandlerAction();
		}

		// Properties
		public new HandlerAction this[string name]
		{
			get
			{
				for (int i = 0; i < base.Count; i++)
				{
					HandlerAction action = base[i];
					if (string.Equals(action.Name, name, StringComparison.OrdinalIgnoreCase))
					{
						return action;
					}
				}
				return null;
			}
		}
	}


}

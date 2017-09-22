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

using System.Linq;

namespace SolidCP.Providers.Web.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Web.Administration;
    using SolidCP.Providers.Web.Iis.Common;
    using Microsoft.Web.Management.Server;
	using SolidCP.Providers.Utils;
	using SolidCP.Server.Utils;

	internal sealed class HandlersModuleService : ConfigurationModuleService
	{
        public void AddFastCgiApplication(string processorPath, string arguments)
        {
			//
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				FastCgiSection section = (FastCgiSection)config.GetSection(Constants.FactCgiSection, typeof(FastCgiSection));

				FastCgiApplicationCollection applications = section.Applications;
				//
				if (applications[processorPath, arguments] == null)
				{
					applications.Add(processorPath, arguments);
				}

				//
				srvman.CommitChanges();
			}
        }

        public void AddIsapiAndCgiRestriction(string processorPath, bool allowed)
        {
			//
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				IsapiCgiRestrictionSection section = (IsapiCgiRestrictionSection)config.GetSection(
					"system.webServer/security/isapiCgiRestriction", typeof(IsapiCgiRestrictionSection));
				
				//
				IsapiCgiRestrictionCollection isapiCgiRestrictions = section.IsapiCgiRestrictions;
				//
				if (isapiCgiRestrictions[processorPath] == null)
				{
					isapiCgiRestrictions.Add(processorPath, true);
				}
				else
				{
					isapiCgiRestrictions[processorPath].Allowed = true;
				}

				//
				srvman.CommitChanges();
			}
        }

		public void SetHandlersAccessPolicy(ServerManager srvman, string fqPath, HandlerAccessPolicy policy)
		{
			var config = srvman.GetWebConfiguration(fqPath);
			//
			HandlersSection section = (HandlersSection)config.GetSection(Constants.HandlersSection, typeof(HandlersSection));
			//
			section.AccessPolicy = policy;
		}

		public HandlerAccessPolicy GetHandlersAccessPolicy(ServerManager srvman, string fqPath)
		{
			var config = srvman.GetWebConfiguration(fqPath);
			//
			HandlersSection section = (HandlersSection)config.GetSection(Constants.HandlersSection, typeof(HandlersSection));
			//
			return section.AccessPolicy;
		}

		/// <summary>
		/// Adds non existent script maps.
		/// </summary>
		/// <param name="installedHandlers">Already installed scrip maps.</param>
		/// <param name="extensions">Extensions to check.</param>
		/// <param name="processor">Extensions processor.</param>
		internal void AddScriptMaps(WebAppVirtualDirectory virtualDir,
			IEnumerable<string> extensions, string processor, string scName, string scModule)
		{
			// Empty processor is out of interest...
			if (String.IsNullOrEmpty(processor))
				return;
			// This section helps to overcome "legacy" issues
            //using (var srvman = GetServerManager())
            //{
            //    var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
            //    //
            //    var handlersSection = config.GetSection(Constants.HandlersSection);
            //    // Do a complete section cleanup
            //    handlersSection.RevertToParent();
            //    //
            //    srvman.CommitChanges();
            //}
			//
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var handlersSection = config.GetSection(Constants.HandlersSection, virtualDir.FullQualifiedPath);

				var handlersCollection = handlersSection.GetCollection();
				// Iterate over extensions in order to setup non-existent handlers
				foreach (string extension in extensions)
				{
					var extParts = extension.Split(',');
					var path = extParts[0];
					var existentHandler = FindHandlerAction(handlersCollection, path, processor);
					// No need to add an existing handler 
					if (existentHandler != null)
						continue;
					// Create a new handler
					var handler = handlersCollection.CreateElement();
					// build script mapping name
					var scriptMappingName = String.Format(scName, path);
					//
					handler["name"] = scriptMappingName;
					handler["path"] = "*" + path;
					handler["verb"] = "GET,HEAD,POST,DEBUG";
					handler["scriptProcessor"] = processor;
					handler["resourceType"] = ResourceType.File;
					handler["modules"] = scModule;
					// add handler
					handlersCollection.AddAt(0, handler);
				}
				//
				srvman.CommitChanges();
			}
			// Allow a script module...
			switch (scModule)
			{
				case Constants.CgiModule: // Allow either ISAPI or CGI module
				case Constants.IsapiModule:
					AddIsapiAndCgiRestriction(processor, true);
					break;
				case Constants.FastCgiModule: // Allow FastCGI module
					AddFastCgiApplication(processor, String.Empty);
					break;
				default:
					Log.WriteWarning("Unknown Script Module has been requested to allow: {0};", scModule);
					break;
			}
		}

	    internal void InheritScriptMapsFromParent(string fqPath)
		{
			if (String.IsNullOrEmpty(fqPath))
				return;
			//
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var handlersSection = config.GetSection(Constants.HandlersSection, fqPath);
				//
				handlersSection.RevertToParent();
				//
				srvman.CommitChanges();
			}
		}

		internal void RemoveScriptMaps(WebAppVirtualDirectory virtualDir, IEnumerable<string> extensions, string processor)
		{
			if (String.IsNullOrEmpty(processor))
				return;
			//
			if (virtualDir == null)
				return;
			//
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var handlersSection = config.GetSection(Constants.HandlersSection, virtualDir.FullQualifiedPath);
				var handlersCollection = handlersSection.GetCollection();
				//
				foreach (string extension in extensions)
				{
					var extParts = extension.Split(',');
					var path = extParts[0];
					var existentHandler = FindHandlerAction(handlersCollection, path, processor);
					// remove handler if exists
					if (existentHandler != null)
						handlersCollection.Remove(existentHandler);
				}
				//
				srvman.CommitChanges();
			}
		}

		private ConfigurationElement FindHandlerAction(ConfigurationElementCollection handlers, string path, string processor)
		{
			foreach (ConfigurationElement action in handlers)
			{
				// match handler path mapping
				bool pathMatches = (String.Compare((string)action["path"], path, true) == 0)
					|| (String.Compare((string)action["path"], String.Format("*{0}", path), true) == 0);
				// match handler processor
				bool processorMatches = (String.Compare(FileUtils.EvaluateSystemVariables((string)action["scriptProcessor"]), 
					FileUtils.EvaluateSystemVariables(processor), true) == 0);
				// return handler action when match is exact
				if (pathMatches && processorMatches)
					return action;
			}
			// 
			return null;
		}


        internal void CopyInheritedHandlers(string siteName, string vDirPath)
	    {
            if (string.IsNullOrEmpty(siteName))
            {
                return;
            }

            if (string.IsNullOrEmpty(vDirPath))
            {
                vDirPath = "/";
            }

	        using (var srvman = GetServerManager())
	        {
				var config = srvman.GetWebConfiguration(siteName, vDirPath);

                var handlersSection = (HandlersSection)config.GetSection(Constants.HandlersSection, typeof(HandlersSection));

	            var handlersCollection = handlersSection.Handlers;

	            var list = new HandlerAction[handlersCollection.Count];
	            ((System.Collections.ICollection) handlersCollection).CopyTo(list, 0);
                
	            handlersCollection.Clear();

	            foreach (var handler in list)
	            {
	                handlersCollection.AddCopy(handler);
	            }

                srvman.CommitChanges();
	        }
	    }
	}
}

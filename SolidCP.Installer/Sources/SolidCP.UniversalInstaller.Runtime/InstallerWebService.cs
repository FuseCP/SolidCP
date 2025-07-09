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
using System.Net;
using System.ServiceModel;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.UniversalInstaller
{
	public class InstallerWebService : InstallerService_SoapClient, IInstallerWebService
	{
		public InstallerWebService(string url) : base(
			new BasicHttpBinding() { MaxReceivedMessageSize = Core.SetupLoader.ChunkSize * 2 },
			new EndpointAddress(url)) { }

		private Stream XStream(XElement xml)
		{
			var stream = new MemoryStream();
			var writer = XmlWriter.Create(stream);
			xml.WriteTo(writer);
			writer.Flush();
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}
		protected DataSet DataSet(ArrayOfXElement xml)
		{
			if (xml == null) return null;

			var set = new DataSet();
			using (var stream = XStream(xml.Nodes[0]))
			{
				set.ReadXmlSchema(stream);
			}
			using (var stream = XStream(xml.Nodes[1]))
			{
				set.ReadXml(stream);
			}
			return set;
		}

		protected List<T> FromResultCollection<T>(ArrayOfXElement xml) where T : class
			=> ObjectUtils.CreateListFromDataSet<T>(DataSet(xml));
		protected T FromResult<T>(ArrayOfXElement xml) where T : class => FromResultCollection<T>(xml).FirstOrDefault();

		public new ComponentUpdateInfo GetComponentUpdate(string componentCode, string release)
			=> FromResult<ComponentUpdateInfo>(base.GetComponentUpdate(componentCode, release));
		public new async Task<ComponentUpdateInfo> GetComponentUpdateAsync(string componentCode, string release)
			=> FromResult<ComponentUpdateInfo>(await base.GetComponentUpdateAsync(componentCode, release));

		private void Filter(List<ComponentInfo> result)
		{
			// exclude Standalone vs. other components
			if (Installer.Current.Settings.Installer.InstalledComponents.Any(c => c.ComponentCode == "standalone"))
			{
				result.RemoveAll(c => c.ComponentCode == "enterprise server" || c.ComponentCode == "serverunix" || c.ComponentCode == "server" ||
						c.ComponentCode == "portal" || c.ComponentCode == "WebDavPortal" || c.ComponentCode == "serveraspv2");
			}
			else if (Installer.Current.Settings.Installer.InstalledComponents.Any(c => c.ComponentCode == "enterprise server" || c.ComponentCode == "serverunix" || c.ComponentCode == "server" ||
						c.ComponentCode == "portal" || c.ComponentCode == "WebDavPortal" || c.ComponentCode == "serveraspv2"))
			{
				result.RemoveAll(c => c.ComponentCode == "standalone");
			}
		}
		public new List<ComponentInfo> GetAvailableComponents()
		{
			var result = FromResultCollection<ComponentInfo>(base.GetAvailableComponents());
			foreach (var component in result) component.VersionName = component.Version.ToString(3);
			Filter(result);
			return result;
		}
		public new async Task<List<ComponentInfo>> GetAvailableComponentsAsync()
		{
			var result = FromResultCollection<ComponentInfo>(await base.GetAvailableComponentsAsync());
			foreach (var component in result) component.VersionName = component.Version.ToString(3);
			Filter(result);
			return result;
		}
		public new ComponentUpdateInfo GetLatestComponentUpdate(string componentCode) => FromResult<ComponentUpdateInfo>(base.GetLatestComponentUpdate(componentCode));
		public new async Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode) => FromResult<ComponentUpdateInfo>(await base.GetLatestComponentUpdateAsync(componentCode));
		public new ReleaseFileInfo GetReleaseFileInfo(string componentCode, string version) => FromResultCollection<ReleaseFileInfo>(base.GetReleaseFileInfo(componentCode, version)).Single();
		public new async Task<ReleaseFileInfo> GetReleaseFileInfoAsync(string componentCode, string version) => FromResultCollection<ReleaseFileInfo>(await base.GetReleaseFileInfoAsync(componentCode, version)).Single();
		public new async Task<byte[]> GetFileChunkAsync(string file, int offset, int size)
			=> (await base.GetFileChunkAsync(file, offset, size)).GetFileChunkResult;

	}
}

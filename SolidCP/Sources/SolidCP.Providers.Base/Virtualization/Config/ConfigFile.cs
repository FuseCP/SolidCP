using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SolidCP.Providers.Virtualization
{
    public class ConfigFile
    {
        const string resultTemplate = @"
<?xml version=""1.0""?>
<items>
  {0}
</items>";

        const string itemTemplate = @"
  <item path=""{0}"" legacyNetworkAdapter=""{1}"" remoteDesktop=""{2}"" processVolume=""{3}"">
    <name>{4}</name>
    <description>{5}</description>
    <DeployScriptParams>{10}</DeployScriptParams>
    <provisioning>
      {6}
      <vmconfig computerName=""{7}"" administratorPassword=""{8}"" networkAdapters=""{9}"" />
    </provisioning>
  </item>";

        const string sysprepTemplate = @"<sysprep file=""{0}""/>";

        public ConfigFile(string xml)
        {
            Xml = xml;
            Load(xml);
        }

        public ConfigFile(LibraryItem[] libraryItems)
        {
            LibraryItems = libraryItems;
            Load(libraryItems);
        }


        public LibraryItem[] LibraryItems { get; private set; }

        public string Xml { get; private set; }


        private void Load(string xmlText)
        {
            // create list
            List<LibraryItem> items = new List<LibraryItem>();

            if (string.IsNullOrEmpty(xmlText))
            {
                LibraryItems = items.ToArray();
                return;
            }

            // load xml
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlText);

            XmlNodeList nodeItems = xml.SelectNodes("/items/item");

            foreach (XmlNode nodeItem in nodeItems)
            {
                LibraryItem item = new LibraryItem();
                if (nodeItem.Attributes["path"] != null)
                    item.Path = nodeItem.Attributes["path"].Value;

                // optional attributes
                if (nodeItem.Attributes["diskSize"] != null)
                    item.DiskSize = Int32.Parse(nodeItem.Attributes["diskSize"].Value);

                if (nodeItem.Attributes["DeployScriptParams"] != null)
                    item.DeployScriptParams = nodeItem.Attributes["DeployScriptParams"].Value;

                if (nodeItem.Attributes["legacyNetworkAdapter"] != null)
                    item.LegacyNetworkAdapter = Boolean.Parse(nodeItem.Attributes["legacyNetworkAdapter"].Value);

                item.ProcessVolume = 0; // process (extend and sysprep) 1st volume by default
                if (nodeItem.Attributes["processVolume"] != null)
                    item.ProcessVolume = Int32.Parse(nodeItem.Attributes["processVolume"].Value);

                if (nodeItem.Attributes["remoteDesktop"] != null)
                    item.RemoteDesktop = Boolean.Parse(nodeItem.Attributes["remoteDesktop"].Value);

                // inner nodes
                item.Name = nodeItem.SelectSingleNode("name").InnerText;
                var descriptionNode = nodeItem.SelectSingleNode("description");
                if (descriptionNode != null)
                    item.Description = descriptionNode.InnerText;

                var DeployScriptParamsNode = nodeItem.SelectSingleNode("DeployScriptParams");
                if (DeployScriptParamsNode != null)
                    item.DeployScriptParams = DeployScriptParamsNode.InnerText;


                // sysprep files
                XmlNodeList nodesSyspep = nodeItem.SelectNodes("provisioning/sysprep");
                List<string> sysprepFiles = new List<string>();
                foreach (XmlNode nodeSyspep in nodesSyspep)
                {
                    if (nodeSyspep.Attributes["file"] != null)
                        sysprepFiles.Add(nodeSyspep.Attributes["file"].Value);
                }
                item.SysprepFiles = sysprepFiles.ToArray();

                // vmconfig
                XmlNode nodeVmConfig = nodeItem.SelectSingleNode("provisioning/vmconfig");
                if (nodeVmConfig != null)
                {
                    if (nodeVmConfig.Attributes["computerName"] != null)
                        item.ProvisionComputerName = Boolean.Parse(nodeVmConfig.Attributes["computerName"].Value);

                    if (nodeVmConfig.Attributes["administratorPassword"] != null)
                        item.ProvisionAdministratorPassword =
                            Boolean.Parse(nodeVmConfig.Attributes["administratorPassword"].Value);

                    if (nodeVmConfig.Attributes["networkAdapters"] != null)
                        item.ProvisionNetworkAdapters = Boolean.Parse(nodeVmConfig.Attributes["networkAdapters"].Value);
                }

                items.Add(item);
            }

            LibraryItems = items.ToArray();
        }

        private void Load(LibraryItem[] libraryItems)
        {
            List<string> items = new List<string>();

            foreach (var libraryItem in libraryItems)
            {
                var sysprep = "";

                if (libraryItem.SysprepFiles != null && libraryItem.SysprepFiles.Any())
                    sysprep = string.Join("", libraryItem.SysprepFiles.Select(s => string.Format(sysprepTemplate, s)).ToArray());

                items.Add(string.Format(itemTemplate, libraryItem.Path, libraryItem.LegacyNetworkAdapter,
                    libraryItem.RemoteDesktop, libraryItem.ProcessVolume, libraryItem.Name, libraryItem.Description,
                    sysprep, libraryItem.ProvisionComputerName, libraryItem.ProvisionAdministratorPassword,
                    libraryItem.ProvisionNetworkAdapters, libraryItem.DeployScriptParams));
            }

            Xml = string.Format(resultTemplate, string.Join("", items.ToArray()));
        }
    }
}

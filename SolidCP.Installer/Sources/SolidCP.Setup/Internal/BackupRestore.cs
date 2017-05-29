using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Ionic.Zip;

namespace SolidCP.Setup.Internal
{
    public class BackupRestore
    {
        struct DirectoryTag
        {
            public string Name;
            public DateTime Date;
            public Version Version;
        }
        public const string MainConfig = "SolidCP.config";
        const string BackupDirectory = "Backup";
        const string ConfigDirectory = "Config";
        const string AppZip = @"App\app.zip";        
        const string DateFormat = "yyyy-MM-dd";
        public BackupRestore()
        {
            XmlFiles = new List<string>();
        }
        public static BackupRestore Find(string Root, string Product, string Id)
        {
            var Result = default(BackupRestore);
            var Dir = Path.Combine(Root, BackupDirectory);
            var FullId = GetFullId(Product, Id);
            if (Directory.Exists(Dir))
            {
                var DirList = new List<DirectoryTag>();
                foreach (var DateItem in Directory.GetDirectories(Dir))
                {
                    DateTime date;
                    var DateName = new DirectoryInfo(DateItem).Name;
                    if (DateTime.TryParseExact(DateName, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        foreach (var VersionItem in Directory.GetDirectories(DateItem))
                        {
                            var VersionName = new DirectoryInfo(VersionItem).Name;
                            if (VersionName.StartsWith(FullId, StringComparison.InvariantCultureIgnoreCase))
                            {
                                Version BckpVersion;
                                var StrVersion = VersionName.Substring(FullId.Length);
                                if (Version.TryParse(StrVersion, out BckpVersion))
                                    DirList.Add(new DirectoryTag { Name = VersionItem, Date = date, Version = BckpVersion });
                            }
                        }
                    }
                }
                var ByVersion = from i in DirList where i.Version == (from v in DirList select v.Version).Max() select i;
                var ByDate = from i in ByVersion where i.Date == (from v in ByVersion select v.Date).Max() select i;
                var SrcTag = ByDate.First();
                Result = new BackupRestore { Id = Id, Root = GetComponentRoot(SrcTag, Id), BackupFile = Path.Combine(SrcTag.Name, AppZip), BackupMainConfigFile = GetMainConfig(SrcTag) };
            }
            return Result;
        }
        public static bool HaveChild(string XmlDocPath, string XmlPath)
        {
            var Result = false;
            var XCfg = new XmlDocument();
            XCfg.Load(XmlDocPath);
            var Node = XCfg.SelectSingleNode(XmlPath);
            if (Node != null)
                Result = Node.ChildNodes.Count > 0;
            return Result;
        }
        private static string GetComponentRoot(DirectoryTag DirTag, string Id)
        {
            var Cfg = GetMainConfig(DirTag);
            if (string.IsNullOrWhiteSpace(Cfg))
                throw new Exception("Broken backup. Main config file not found.");
            var XCfg = new XmlDocument();
            XCfg.Load(Cfg);
            var Component = XCfg.SelectSingleNode(string.Format("//component[.//add/@key='ComponentName' and .//add/@value='{0}']", Id));
            var InstallFolder = Component.SelectSingleNode(".//add[@key='InstallFolder']");
            return InstallFolder.Attributes["value"].Value;
        }
        private static string GetMainConfig(DirectoryTag DirTag)
        {
            return Path.Combine(DirTag.Name, ConfigDirectory, MainConfig);
        }
        private static string GetFullId(string Product, string Id)
        {
            return string.Format("{0} {1}", Product, Id);
        }
        public virtual void Restore()
        {
            using (var Bckp = new ZipFile(BackupFile))
            {
                foreach (var Xml in XmlFiles)
                {
                    var SrcEntry = from Entry in Bckp.Entries where NormalizePath(Entry.FileName.ToLowerInvariant(), "/") == Xml.ToLowerInvariant() select Entry;
                    if (SrcEntry != null)
                    {
                        if (SrcEntry.LongCount() > 1)
                            throw new Exception(string.Format("Too many backup entries - {0}.", Xml));
                        var FileEntry = SrcEntry.FirstOrDefault();
                        if (FileEntry != null)
                        {
                            using (var InMem = new MemoryStream())
                            {
                                FileEntry.Extract(InMem);
                                InMem.Seek(0, SeekOrigin.Begin);
                                using (var OutFile = new FileStream(Path.Combine(Root, Xml), FileMode.Open, FileAccess.ReadWrite))
                                {
                                    XmlDocumentMerge.Process(InMem, OutFile);
                                }
                            }
                        }
                    }
                }
            }
        }
        private string NormalizePath(string FilePath, string In)
        {
            return Path.Combine(FilePath.Split(new string[] { In }, StringSplitOptions.RemoveEmptyEntries));
        }
        public string Id { get; set; } // Component full name.
        public string Comment { get; set; }
        public string BackupFile { get; set; } // Should be zip archive.
        public string BackupMainConfigFile { get; set; }
        public IList<string> XmlFiles { get; set; } // Xml files (configs) to merge and update.
        public string Root { get; set; }
    }
}

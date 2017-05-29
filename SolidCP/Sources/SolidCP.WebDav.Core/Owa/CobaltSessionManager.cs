using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using Cobalt;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Interfaces.Managers;
using SolidCP.WebDav.Core.Interfaces.Owa;
using SolidCP.WebDav.Core.Interfaces.Storages;

namespace SolidCP.WebDav.Core.Owa
{
    public class CobaltSessionManager : IWopiFileManager
    {
        private readonly IWebDavManager _webDavManager;
        private readonly IAccessTokenManager _tokenManager;
        private readonly ITtlStorage _storage;

        public CobaltSessionManager(IWebDavManager webDavManager, IAccessTokenManager tokenManager, ITtlStorage storage)
        {
            _webDavManager = webDavManager;

            _tokenManager = tokenManager;
            _storage = storage;
        }

        public CobaltFile Create(int accessTokenId)
        {
            var disposal = new DisposalEscrow(accessTokenId.ToString(CultureInfo.InvariantCulture));

            var content = new CobaltFilePartitionConfig
            {
                IsNewFile = true,
                HostBlobStore = new TemporaryHostBlobStore(new TemporaryHostBlobStore.Config(), disposal, accessTokenId + @".Content"),
                cellSchemaIsGenericFda = true,
                CellStorageConfig = new CellStorageConfig(),
                Schema = CobaltFilePartition.Schema.ShreddedCobalt,
                PartitionId = FilePartitionId.Content
            };

            var coauth = new CobaltFilePartitionConfig
            {
                IsNewFile = true,
                HostBlobStore = new TemporaryHostBlobStore(new TemporaryHostBlobStore.Config(), disposal, accessTokenId + @".CoauthMetadata"),
                cellSchemaIsGenericFda = false,
                CellStorageConfig = new CellStorageConfig(),
                Schema = CobaltFilePartition.Schema.ShreddedCobalt,
                PartitionId = FilePartitionId.CoauthMetadata
            };

            var wacupdate = new CobaltFilePartitionConfig
            {
                IsNewFile = true,
                HostBlobStore = new TemporaryHostBlobStore(new TemporaryHostBlobStore.Config(), disposal, accessTokenId + @".WordWacUpdate"),
                cellSchemaIsGenericFda = false,
                CellStorageConfig = new CellStorageConfig(),
                Schema = CobaltFilePartition.Schema.ShreddedCobalt,
                PartitionId = FilePartitionId.WordWacUpdate
            };

            var partitionConfs = new Dictionary<FilePartitionId, CobaltFilePartitionConfig>
            {
                {FilePartitionId.Content, content},
                {FilePartitionId.WordWacUpdate, wacupdate},
                {FilePartitionId.CoauthMetadata, coauth}
            };

            var cobaltFile = new CobaltFile(disposal, partitionConfs, new CobaltHostLockingStore(), null);

            var token = _tokenManager.GetToken(accessTokenId);

            Atom atom;

            if (_webDavManager.FileExist(token.FilePath))
            {
                var fileBytes = _webDavManager.GetFileBytes(token.FilePath);

                atom = new AtomFromByteArray(fileBytes);
            }
            else
            {
                var filePath = HttpContext.Current.Server.MapPath(WebDavAppConfigManager.Instance.OfficeOnline.NewFilePath + Path.GetExtension(token.FilePath));

                atom = new AtomFromByteArray(File.ReadAllBytes(filePath));
            }

            Cobalt.Metrics o1;
            cobaltFile.GetCobaltFilePartition(FilePartitionId.Content).SetStream(RootId.Default.Value, atom, out o1);
            cobaltFile.GetCobaltFilePartition(FilePartitionId.Content).GetStream(RootId.Default.Value).Flush();

            Add(token.FilePath, cobaltFile);

            return cobaltFile;
        }

        public CobaltFile Get(string filePath)
        {
            return _storage.Get<CobaltFile>(GetSessionKey(filePath)); 
        }

        public bool Add(string filePath, CobaltFile file)
        {
            return _storage.Add(GetSessionKey(filePath), file);
        }

        public bool Delete(string filePath)
        {
            return _storage.Delete(GetSessionKey(filePath));
        }

        private string GetSessionKey(string filePath)
        {
            return string.Format("{0}/{1}", ScpContext.User.AccountId, filePath);
        }
    }
}
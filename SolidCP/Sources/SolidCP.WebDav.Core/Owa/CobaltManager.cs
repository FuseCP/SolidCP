using System;
using System.IO;
using System.Threading;
using Cobalt;
using SolidCP.WebDav.Core.Interfaces.Managers;
using SolidCP.WebDav.Core.Interfaces.Owa;

namespace SolidCP.WebDav.Core.Owa
{
    public class CobaltManager : ICobaltManager
    {
        private readonly IWebDavManager _webDavManager;
        private readonly IWopiFileManager _fileManager;
        private readonly IAccessTokenManager _tokenManager;

        public CobaltManager(IWebDavManager webDavManager, IWopiFileManager fileManager,
            IAccessTokenManager tokenManager)
        {
            _webDavManager = webDavManager;
            _fileManager = fileManager;
            _tokenManager = tokenManager;
        }

        public Atom ProcessRequest(int accessTokenId, Stream requestStream)
        {
            var token = _tokenManager.GetToken(accessTokenId);

            var atomRequest = new AtomFromStream(requestStream);

            var requestBatch = new RequestBatch();

            try
            {
                var cobaltFile = _fileManager.Get(token.FilePath) ?? _fileManager.Create(accessTokenId);

                Object ctx;
                ProtocolVersion protocolVersion;

                requestBatch.DeserializeInputFromProtocol(atomRequest, out ctx, out protocolVersion);
                cobaltFile.CobaltEndpoint.ExecuteRequestBatch(requestBatch);


                foreach (var request in requestBatch.Requests)
                {

                    if (request.GetType() == typeof (PutChangesRequest) &&
                        request.PartitionId == FilePartitionId.Content && request.CompletedSuccessfully)
                    {
                        using (var saveStream = new MemoryStream())
                        {
                            CopyStream(cobaltFile, saveStream);
                            _webDavManager.UploadFile(token.FilePath, saveStream.ToArray());
                        }
                    }
                }


                return requestBatch.SerializeOutputToProtocol(protocolVersion);
            }

            catch (Exception e)
            {
                Server.Utils.Log.WriteError("Cobalt manager Process request", e);

                throw;
            }
        }

        private void CopyStream(CobaltFile file, Stream stream)
        {
            var tries = 3;

            for (int i = 0; i < tries; i++)
            {
                try
                {
                    GenericFdaStream myCobaltStream = new GenericFda(file.CobaltEndpoint, null).GetContentStream();

                    myCobaltStream.CopyTo(stream);

                    break;
                }
                catch (Exception)
                {
                    //unable to read update - save failed
                    if (i == tries - 1)
                    {
                        throw;
                    }

                    //waiting for cobalt completion
                    Thread.Sleep(50);
                }
            }
        }
    }
} 
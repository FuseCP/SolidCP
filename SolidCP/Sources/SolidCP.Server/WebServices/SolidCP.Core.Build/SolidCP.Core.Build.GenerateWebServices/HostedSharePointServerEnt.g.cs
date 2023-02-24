#if !Client
using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.SharePoint;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
using SolidCP.Server;
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Services
{
    /// <summary>
    /// Summary description for HostedSharePointServerEnt
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IHostedSharePointServerEnt
    {
        /// <summary>
        /// Gets list of supported languages by this installation of SharePoint.
        /// </summary>
        /// <returns>List of supported languages</returns>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int[] Enterprise_GetSupportedLanguages();
        /// <summary>
        /// Gets list of SharePoint collections within root web application.
        /// </summary>
        /// <returns>List of SharePoint collections within root web application.</returns>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections();
        /// <summary>
        /// Gets SharePoint collection within root web application with given name.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be loaded.</param>
        /// <returns>SharePoint collection within root web application with given name.</returns>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url);
        /// <summary>
        /// Creates site collection within predefined root web application.
        /// </summary>
        /// <param name="siteCollection">Information about site coolection to be created.</param>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_CreateSiteCollection(SharePointEnterpriseSiteCollection siteCollection);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls);
        /// <summary>
        /// Deletes site collection under given url.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection siteCollection);
        /// <summary>
        /// Backups site collection under give url.
        /// </summary>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        /// <param name="filename">Resulting backup file name.</param>
        /// <param name="zip">A value which shows whether created backup must be archived.</param>
        /// <returns>Created backup full path.</returns>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string Enterprise_BackupSiteCollection(string url, string filename, bool zip);
        /// <summary>
        /// Restores site collection under given url from backup.
        /// </summary>
        /// <param name="siteCollection">Site collection to be restored.</param>
        /// <param name="filename">Backup file name to restore from.</param>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_RestoreSiteCollection(SharePointEnterpriseSiteCollection siteCollection, string filename);
        /// <summary>
        /// Gets binary data chunk of specified size from specified offset.
        /// </summary>
        /// <param name="path">Path to file to get bunary data chunk from.</param>
        /// <param name="offset">Offset from which to start data reading.</param>
        /// <param name="length">Binary data chunk length.</param>
        /// <returns>Binary data chunk read from file.</returns>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length);
        /// <summary>
        /// Appends supplied binary data chunk to file.
        /// </summary>
        /// <param name="fileName">Non existent file name to append to.</param>
        /// <param name="path">Full path to existent file to append to.</param>
        /// <param name="chunk">Binary data chunk to append to.</param>
        /// <returns>Path to file that was appended with chunk.</returns>
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        long Enterprise_GetSiteCollectionSize(string url);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_SetPeoplePickerOu(string site, string ou);
    }

    public class HostedSharePointServerEntService : SolidCP.Server.HostedSharePointServerEnt, IHostedSharePointServerEnt
    {
        public new int[] Enterprise_GetSupportedLanguages()
        {
            return base.Enterprise_GetSupportedLanguages();
        }

        public new SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections()
        {
            return base.Enterprise_GetSiteCollections();
        }

        public new SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url)
        {
            return base.Enterprise_GetSiteCollection(url);
        }

        public new void Enterprise_CreateSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            base.Enterprise_CreateSiteCollection(siteCollection);
        }

        public new void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize)
        {
            base.Enterprise_UpdateQuotas(url, maxSize, warningSize);
        }

        public new SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return base.Enterprise_CalculateSiteCollectionsDiskSpace(urls);
        }

        public new void Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            base.Enterprise_DeleteSiteCollection(siteCollection);
        }

        public new string Enterprise_BackupSiteCollection(string url, string filename, bool zip)
        {
            return base.Enterprise_BackupSiteCollection(url, filename, zip);
        }

        public new void Enterprise_RestoreSiteCollection(SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            base.Enterprise_RestoreSiteCollection(siteCollection, filename);
        }

        public new byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.Enterprise_GetTempFileBinaryChunk(path, offset, length);
        }

        public virtual new string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.Enterprise_AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public new long Enterprise_GetSiteCollectionSize(string url)
        {
            return base.Enterprise_GetSiteCollectionSize(url);
        }

        public new void Enterprise_SetPeoplePickerOu(string site, string ou)
        {
            base.Enterprise_SetPeoplePickerOu(site, ou);
        }
    }
}
#endif
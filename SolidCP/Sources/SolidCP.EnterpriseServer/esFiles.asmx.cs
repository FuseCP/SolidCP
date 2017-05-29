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
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;

using SolidCP.Providers.OS;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esFiles : System.Web.Services.WebService
    {
        [WebMethod]
        public SystemSettings GetFileManagerSettings()
        {
            return FilesController.GetFileManagerSettings();
        }

        [WebMethod]
        public static string GetHomeFolder(int packageId)
        {
            return FilesController.GetHomeFolder(packageId);
        }

        [WebMethod]
        public List<SystemFile> GetFiles(int packageId, string path, bool includeFiles)
        {
            return FilesController.GetFiles(packageId, path, includeFiles);
        }

        [WebMethod]
        public List<SystemFile> GetFilesByMask(int packageId, string path, string filesMask)
        {
            return FilesController.GetFilesByMask(packageId, path, filesMask);
        }

        [WebMethod]
        public byte[] GetFileBinaryContent(int packageId, string path)
        {
            return FilesController.GetFileBinaryContent(packageId, path);
        }

		[WebMethod]
		public byte[] GetFileBinaryContentUsingEncoding(int packageId, string path, string encoding)
		{
			return FilesController.GetFileBinaryContentUsingEncoding(packageId, path, encoding);
		}

        [WebMethod]
        public int UpdateFileBinaryContent(int packageId, string path, byte[] content)
        {
            return FilesController.UpdateFileBinaryContent(packageId, path, content);
        }

		[WebMethod]
		public int UpdateFileBinaryContentUsingEncoding(int packageId, string path, byte[] content, string encoding)
		{
			return FilesController.UpdateFileBinaryContentUsingEncoding(packageId, path, content, encoding);
		}

        [WebMethod]
        public byte[] GetFileBinaryChunk(int packageId, string path, int offset, int length)
        {
            return FilesController.GetFileBinaryChunk(packageId, path, offset, length);
        }

        [WebMethod]
        public int AppendFileBinaryChunk(int packageId, string path, byte[] chunk)
        {
            return FilesController.AppendFileBinaryChunk(packageId, path, chunk);
        }

        [WebMethod]
        public int DeleteFiles(int packageId, string[] files)
        {
            return FilesController.DeleteFiles(packageId, files);
        }

        [WebMethod]
        public int CreateFile(int packageId, string path)
        {
            return FilesController.CreateFile(packageId, path);
        }

        [WebMethod]
        public int CreateFolder(int packageId, string path)
        {
            return FilesController.CreateFolder(packageId, path);
        }

        [WebMethod]
        public int CopyFiles(int packageId, string[] files, string destFolder)
        {
            return FilesController.CopyFiles(packageId, files, destFolder);
        }

        [WebMethod]
        public int MoveFiles(int packageId, string[] files, string destFolder)
        {
            return FilesController.MoveFiles(packageId, files, destFolder);
        }

        [WebMethod]
        public int RenameFile(int packageId, string oldPath, string newPath)
        {
            return FilesController.RenameFile(packageId, oldPath, newPath);
        }

        [WebMethod]
        public void UnzipFiles(int packageId, string[] files)
        {
            FilesController.UnzipFiles(packageId, files);
        }

        [WebMethod]
        public int ZipFiles(int packageId, string[] files, string archivePath)
        {
            return FilesController.ZipFiles(packageId, files, archivePath);
        }

        [WebMethod]
        public int ZipRemoteFiles(int packageId, string rootFolder, string[] files, string archivePath)
        {
            return FilesController.ZipRemoteFiles(packageId, rootFolder, files, archivePath);
        }

        [WebMethod]
        public int CreateAccessDatabase(int packageId, string dbPath)
        {
            return FilesController.CreateAccessDatabase(packageId, dbPath);
        }

        [WebMethod]
        public int CalculatePackageDiskspace(int packageId)
        {
            return FilesController.CalculatePackageDiskspace(packageId);
        }

        [WebMethod]
        public UserPermission[] GetFilePermissions(int packageId, string path)
        {
            return FilesController.GetFilePermissions(packageId, path);
        }

        [WebMethod]
        public int SetFilePermissions(int packageId, string path, UserPermission[] users, bool resetChildPermissions)
        {
            return FilesController.SetFilePermissions(packageId, path, users, resetChildPermissions);
        }

        [WebMethod]
        public FolderGraph GetFolderGraph(int packageId, string path)
        {
            return FilesController.GetFolderGraph(packageId, path);
        }

        [WebMethod]
        public void ExecuteSyncActions(int packageId, FileSyncAction[] actions)
        {
            FilesController.ExecuteSyncActions(packageId, actions);
        }

        //CO Changes
        [WebMethod]
        public int ApplyEnableHardQuotaFeature(int packageId)
        {
            return FilesController.ApplyEnableHardQuotaFeature(packageId);
        }
        //END
    }
}

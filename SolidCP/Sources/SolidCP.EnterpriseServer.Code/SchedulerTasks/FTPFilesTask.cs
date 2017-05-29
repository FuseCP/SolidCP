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
using System.IO;
using System.Collections.Generic;
using System.Text;

using SolidCP.Server;

namespace SolidCP.EnterpriseServer
{
    public class FTPFilesTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - FILE_PATH
            //  - FTP_SERVER
            //  - FTP_USERNAME
            //  - FTP_PASSWORD
            //  - FTP_FOLDER

            BackgroundTask topTask = TaskManager.TopTask;

            // get input parameters
            string filePath = (string)topTask.GetParamValue("FILE_PATH");
            string ftpServer = (string)topTask.GetParamValue("FTP_SERVER");
            string ftpUsername = (string)topTask.GetParamValue("FTP_USERNAME");
            string ftpPassword = (string)topTask.GetParamValue("FTP_PASSWORD");
            string ftpFolder = (string)topTask.GetParamValue("FTP_FOLDER");

            // check input parameters
            if (String.IsNullOrEmpty(filePath))
            {
                TaskManager.WriteWarning("Specify 'File' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(ftpServer))
            {
                TaskManager.WriteWarning("Specify 'FTP Server' task parameter");
                return;
            }

            // substitute parameters
            DateTime d = DateTime.Now;
            string date = d.ToString("yyyyMMdd");
            string time = d.ToString("HHmm");

            filePath = Utils.ReplaceStringVariable(filePath, "date", date);
            filePath = Utils.ReplaceStringVariable(filePath, "time", time);

            // build FTP command file
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            // FTP server
            writer.WriteLine("open " + ftpServer);

            // check if anonymous mode
            if (String.IsNullOrEmpty(ftpUsername))
            {
                ftpUsername = "anonymous";
                ftpPassword = "anonymous@email.com";
            }

            // FTP username/password
            writer.WriteLine(ftpUsername);
            writer.WriteLine(ftpPassword);

            // check if we need to change remote folder
            if (!String.IsNullOrEmpty(ftpFolder))
            {
                writer.WriteLine("cd " + ftpFolder.Replace("\\", "/"));
            }

            // file to send
            writer.WriteLine("binary");
            writer.WriteLine("put " + FilesController.GetFullPackagePath(topTask.PackageId, filePath));

            // bye
            writer.WriteLine("bye");

            string cmdBatch = sb.ToString();

            // create temp file in user space
            string cmdPath = Utils.GetRandomString(10) + ".txt";
            string fullCmdPath = FilesController.GetFullPackagePath(topTask.PackageId, cmdPath);

            // upload batch
            FilesController.UpdateFileBinaryContent(topTask.PackageId, cmdPath, Encoding.UTF8.GetBytes(cmdBatch));

            // execute system command
            // load OS service
            int serviceId = PackageController.GetPackageServiceId(topTask.PackageId, ResourceGroups.Os);

            // load service
            ServiceInfo service = ServerController.GetServiceInfo(serviceId);
            if (service == null)
                return;

            WindowsServer winServer = new WindowsServer();
            ServiceProviderProxy.ServerInit(winServer, service.ServerId);
            TaskManager.Write(winServer.ExecuteSystemCommand("ftp.exe", "-s:" + fullCmdPath));

            // delete batch file
            FilesController.DeleteFiles(topTask.PackageId, new string[] { cmdPath });
        }
    }
}

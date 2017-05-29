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

//﻿using System;
//using System.Collections.Generic;
//using System.Web;
//using System.Threading;
//using System.ComponentModel;
//using SolidCP.Providers;
//using SolidCP.Providers.Common;
//using SolidCP.Providers.ResultObjects;
//using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.VirtualizationForPC;

//namespace SolidCP.EnterpriseServer
//{
//    public class CreateAsyncVMfromVM
//    {
//        public static void Run(string sourceVM, VMInfo vmInfo)
//        {
//            BackgroundWorker createVMBackground = new BackgroundWorker();
//            ResultObject taskInfo = null;

//            object[] parameters = { sourceVM, vmInfo };

//            createVMBackground.DoWork += (sender, e) => {
//                string sourceVMName = (string)((object[])e.Argument)[0];
//                VMInfo vm = (VMInfo)((object[])e.Argument)[1];

//                e.Result = vm;
//                Guid taskGuid = Guid.NewGuid();
//                // Add audit log
//                 taskInfo = TaskManager.StartResultTask<ResultObject>("VPSForPC", "CREATE", taskGuid);
//                 TaskManager.ItemId = vm.Id;
//                 TaskManager.ItemName = vm.Name;
//                 TaskManager.PackageId = vm.PackageId;
//                 e.Result = CreateVM(sourceVMName, vm, taskGuid);
//            };
            
//            createVMBackground.RunWorkerCompleted += (sender, e) => {
//                if (e.Error != null || !String.IsNullOrEmpty(((VMInfo)e.Result).exMessage) )
//                {
//                    if (taskInfo != null)
//                    {
//                        TaskManager.CompleteResultTask(taskInfo
//                            , VirtualizationErrorCodes.CREATE_ERROR
//                            , new Exception(((VMInfo)e.Result).exMessage)
//                            , ((VMInfo)e.Result).logMessage);
//                    }
//                }
//                else
//                {
//                    if (taskInfo != null)
//                    {
//                        TaskManager.CompleteResultTask(taskInfo, null, null, ((VMInfo)e.Result).logMessage);
//                    }

//                    PackageController.UpdatePackageItem((VMInfo)e.Result);
//                }
//            };

//            createVMBackground.RunWorkerAsync(parameters);
//        }

//        private static VMInfo CreateVM(string sourceVMName, VMInfo vmInfo, Guid taskGuid)
//        {
//            VirtualizationServerForPC ws = new VirtualizationServerForPC();
//            ServiceProviderProxy.Init(ws, vmInfo.ServiceId);

//            vmInfo = ws.CreateVMFromVM(sourceVMName, vmInfo, taskGuid);

//            return vmInfo;
//        }
//    }
//}


﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.VirtualizationForPC;

namespace SolidCP.EnterpriseServer
{
    public class CreateVMFromVMAsyncWorker
    {
        public int ThreadUserId { get; set; }
        public VMInfo vmTemplate {get; set; }
        public string vmName { get; set; }
        public int packageId { get; set; }

        public CreateVMFromVMAsyncWorker()
        {
            ThreadUserId = -1; // admin
        }

        public void CreateAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(Create));
            t.Start();
        }

        private void Create()
        {
            // impersonate thread
            if (ThreadUserId != -1)
                SecurityContext.SetThreadPrincipal(ThreadUserId);

            // perform backup
            VirtualizationServerControllerForPrivateCloud.CreateVMFromVMAsunc(packageId, vmTemplate, vmName);
        }
    }
}

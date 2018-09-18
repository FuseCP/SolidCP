using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012
{
    class VirtualizationAsyncWorker2012
    {
        #region Main Properties
        public int ThreadUserId { get; set; }
        public int ItemId { get; set; }
        public VirtualMachine Vm { get; set; }
        #endregion

        #region VPS Delete Properties
        public bool SaveFiles { get; set; }
        public bool ExportVps { get; set; }
        public string ExportPath { get; set; }
        #endregion


        public VirtualizationAsyncWorker2012()
        {
            ThreadUserId = -1; // admin
        }

        #region Delete VPS
        public void DeleteVPSAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(DeleteVPS));
            t.Start();
        }
        public void DeleteVPS()
        {
            // impersonate thread
            if (ThreadUserId != -1)
                SecurityContext.SetThreadPrincipal(ThreadUserId);

            VirtualizationServerController2012.DeleteVirtualMachineInternal(ItemId, Vm, SaveFiles, ExportVps, ExportPath);
        }
        #endregion
    }
}

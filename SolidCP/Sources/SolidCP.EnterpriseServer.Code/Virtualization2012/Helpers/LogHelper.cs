using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers
{
    public static class LogHelper
    {
        public static void LogReturnValueResult(ResultObject res, JobResult job)
        {
            res.ErrorCodes.Add(VirtualizationErrorCodes.JOB_START_ERROR + ":" + job.ReturnValue);
        }

        public static void LogJobResult(ResultObject res, ConcreteJob job)
        {
            res.ErrorCodes.Add(VirtualizationErrorCodes.JOB_FAILED_ERROR + ":" + job.ErrorDescription);
        }
    }
}

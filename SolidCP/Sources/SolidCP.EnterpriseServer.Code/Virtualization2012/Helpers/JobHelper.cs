using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers
{
    public class JobHelper: ControllerBase
    {
        public JobHelper(ControllerBase provider) : base(provider) { }

        public bool TryJobCompleted(VirtualizationServer2012 vs, ConcreteJob job, bool resetProgressBarIndicatorAfterFinish = true, bool isPowerShellJob = false)
        {
            bool jobCompleted = false;
            short timeout = 5;
            while (timeout > 0)
            {
                timeout--;
                try
                {
                    jobCompleted = JobCompleted(vs, job, resetProgressBarIndicatorAfterFinish, isPowerShellJob);
                }
                catch (ThreadAbortException) //https://github.com/FuseCP/SolidCP/issues/103
                {
                    //maybe there need to use Thread.ResetAbort(); ???

                    TaskManager.Write("VPS_CREATE_TRY_JOB_COMPLETE_ATTEMPTS_LEFT_AFTER_THREAD_ABORT", timeout.ToString());
                    job = GetLastJobUpdate(vs, job, isPowerShellJob); //get the last job state

                    jobCompleted = (job.JobState == ConcreteJobState.Completed); //is job completed?                                      
                }

                if (jobCompleted)
                {
                    timeout = 0;
                }
            }

            return jobCompleted;
        }

        public bool JobCompleted(VirtualizationServer2012 vs, ConcreteJob job, bool resetProgressBarIndicatorAfterFinish = true, bool isPowerShellJob = false)
        {
            TaskManager.IndicatorMaximum = 100;
            bool jobCompleted = true;
            short timeout = 60;
            while (job.JobState == ConcreteJobState.NotStarted && timeout > 0) //Often jobs are only initialized, need to wait a little, that it started.
            {
                timeout--;
                Thread.Sleep(2000);
                job = GetLastJobUpdate(vs, job, isPowerShellJob);
            }

            while (job.JobState == ConcreteJobState.Starting ||
                job.JobState == ConcreteJobState.Running)
            {
                Thread.Sleep(3000);
                job = GetLastJobUpdate(vs, job, isPowerShellJob);
                TaskManager.IndicatorCurrent = job.PercentComplete;
            }

            if (job.JobState != ConcreteJobState.Completed)
            {
                jobCompleted = false;
            }
            if (resetProgressBarIndicatorAfterFinish)
            {
                TaskManager.IndicatorCurrent = 0;   // reset indicator
            }

            if (isPowerShellJob)
            {
                vs.ClearOldPsJobs();
            }

            return jobCompleted;
        }

        public static ConcreteJob GetLastJobUpdate(VirtualizationServer2012 vs, ConcreteJob job, bool isPowerShellJob = false)
        {
            return isPowerShellJob ? vs.GetPsJob(job.Id) : vs.GetJob(job.Id);
        }
    }
}

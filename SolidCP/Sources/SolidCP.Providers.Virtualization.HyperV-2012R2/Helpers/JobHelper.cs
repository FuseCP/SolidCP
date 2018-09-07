using SolidCP.Providers.HostedSolution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public static class JobHelper
    {
        public static JobResult CreateSuccessResult(ReturnCode returnCode = ReturnCode.OK)
        {
            return new JobResult
            {
                Job = new ConcreteJob { JobState = ConcreteJobState.Completed },
                ReturnValue = returnCode
            };
        }

        public static JobResult CreateResultFromPSResults(Collection<PSObject> objJob)
        {
            if (objJob == null || objJob.Count == 0)
                return null;

            JobResult result = new JobResult();

            result.Job = CreateFromPSObject(objJob);
            result.ReturnValue = ReturnCode.JobStarted;

            switch (result.Job.JobState)
            {
                case ConcreteJobState.Failed:
                    result.ReturnValue = ReturnCode.Failed;
                    break;
            }

            return result;
        }

        public static ConcreteJob CreateFromPSObject(Collection<PSObject> objJob)
        {
            if (objJob == null || objJob.Count == 0)
                return null;

            ConcreteJob job = new ConcreteJob();
            job.Id = objJob[0].GetProperty<int>("Id").ToString();
            job.JobState = objJob[0].GetEnum<ConcreteJobState>("JobStateInfo");
            job.ChildJobs = GetChildJobs(objJob[0].GetProperty<List<Job>>("ChildJobs"));
            job.Caption = objJob[0].GetProperty<string>("Name");
            job.Description = objJob[0].GetProperty<string>("Command");
            job.StartTime = objJob[0].GetProperty<DateTime?>("PSBeginTime") ?? DateTime.Now;
            job.ElapsedTime = objJob[0].GetProperty<DateTime?>("PSEndTime") ?? DateTime.Now;

            // PercentComplete
            job.PercentComplete = 0;
            var progress = (PSDataCollection<ProgressRecord>)objJob[0].GetProperty("Progress");
            if (progress != null && progress.Count > 0)
            {
                job.PercentComplete = progress[progress.Count - 1].PercentComplete;
            }                
            else if (job.ChildJobs != null && !String.IsNullOrEmpty(job.ChildJobs[0].Id))   //ChildJobs can be get if we wrap an command in Job, and ChildJob will be a main process.
            {
                job.PercentComplete = job.ChildJobs[0].PercentComplete; //Important! If we wrap a command that does not have a native "asJob", we can not get the progress bar (always will be 0)!
            }

            // Errors
            var errors = (PSDataCollection<ErrorRecord>)objJob[0].GetProperty("Error");
            if (errors != null && errors.Count > 0)
            {
                job.ErrorDescription = errors[0].ErrorDetails.Message + ". " + errors[0].ErrorDetails.RecommendedAction;
                job.ErrorCode = errors[0].Exception != null ? -1 : 0;
            }

            return job;
        }

        private static List<ConcreteJob> GetChildJobs(List<Job> objJobs)
        {
            if (objJobs.Count == 0)
                return null;

            List<ConcreteJob> childJobs = new List<ConcreteJob>();
            foreach (Job job in objJobs)
            {
                ConcreteJob childJob = new ConcreteJob();
                childJob.Id = job.Id.ToString();
                childJob.JobState = (ConcreteJobState)Enum.Parse(typeof(ConcreteJobState), job.JobStateInfo.State.ToString());
                childJob.ChildJobs = null;
                childJob.Caption = job.Name;
                childJob.Description = job.Command;
                childJob.StartTime = job.PSBeginTime ?? DateTime.Now;
                childJob.ElapsedTime = job.PSEndTime ?? DateTime.Now;
                childJob.PercentComplete = (job.Progress.Count > 0) ? job.Progress[job.Progress.Count - 1].PercentComplete : 0;
                childJobs.Add(childJob);
            }

            return childJobs;
        }
    }
}

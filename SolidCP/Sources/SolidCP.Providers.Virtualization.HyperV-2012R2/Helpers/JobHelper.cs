using Microsoft.Management.Infrastructure;
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
    internal static class JobHelper
    {
        public static JobResult CreateSuccessResult(ReturnCode returnCode = ReturnCode.OK)
        {
            return new JobResult
            {
                Job = new ConcreteJob { JobState = ConcreteJobState.Completed },
                ReturnValue = returnCode
            };
        }

        public static JobResult CreateUnsuccessResult(ReturnCode returnCode = ReturnCode.Failed, string errorString = "")
        {
            return new JobResult
            {
                Job = new ConcreteJob { 
                    JobState = ConcreteJobState.Exception,
                    ErrorDescription = errorString
                },
                ReturnValue = returnCode
            };
        }

        public static JobResult CreateJobResultFromCimResults(MiManager mi, CimMethodResult outParams)
        {
            JobResult result = new JobResult();

            result.ReturnValue = (ReturnCode)Convert.ToInt32(outParams.OutParameters["ReturnValue"].Value);
            try
            {
                using (CimInstance objJob = outParams.OutParameters["Job"].Value as CimInstance)
                {
                    if (objJob == null)
                    { //we suppose if obj is null, then job already finished. Anyway ReturnValue should be checked first
                        result.Job = new ConcreteJob
                        {
                            JobState = ConcreteJobState.New,
                            ErrorDescription = "The job has never been started"
                        };
                        return result;
                    }
                    result.Job = CreateFromCimObject(mi.GetInstance(objJob));
                }                

            } catch (Exception e) {
                HostedSolutionLog.LogError(e);
            }

            return result;
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

        public static ConcreteJob CreateFromCimObject(CimInstance cimJob)
        {
            if (cimJob == null || cimJob.CimInstanceProperties.Count == 0)
                return null;

            var objJob = cimJob.CimInstanceProperties;

            ConcreteJob job = new ConcreteJob();
            job.Id = (string)objJob["InstanceID"].Value;
            job.JobState = (ConcreteJobState)Convert.ToInt32(objJob["JobState"].Value);
            job.Caption = (string)objJob["Caption"].Value;
            job.Description = (string)objJob["Description"].Value;
            job.StartTime = (System.DateTime)objJob["StartTime"].Value;
            // TODO CIM correcly provide ElapsedTime, e.g. 00000000000001.325247:000 so we can just use as it is.
            //we can use after remove all WMIv1 stuff depencies. Need to change job.ElapsedTime type to TimeSpan.
            job.ElapsedTime = DateTime.Now; //(System.TimeSpan)objJob["ElapsedTime"].Value;
            job.ErrorCode = Convert.ToInt32(objJob["ErrorCode"].Value);
            job.ErrorDescription = (string)objJob["ErrorDescription"].Value;
            job.PercentComplete = Convert.ToInt32(objJob["PercentComplete"].Value);
            return job;
        }

        public static ConcreteJob CreateFromPSObject(Collection<PSObject> objJob)
        {
            if (objJob == null || objJob.Count == 0)
                return null;

            ConcreteJob job = new ConcreteJob();
            job.Id = objJob[0].GetProperty<Guid>("InstanceId").ToString();
            job.JobState = objJob[0].GetEnum<ConcreteJobState>("JobStateInfo");
            job.ChildJobs = GetChildJobs(objJob[0].GetProperty<List<Job>>("ChildJobs"));
            job.Caption = objJob[0].GetProperty<string>("Name");
            job.Description = objJob[0].GetProperty<string>("Command");
            job.StartTime = objJob[0].GetProperty<DateTime?>("PSBeginTime") ?? DateTime.Now;
            job.ElapsedTime = objJob[0].GetProperty<DateTime?>("PSEndTime") ?? DateTime.Now;

            // PercentComplete
            job.PercentComplete = -1;
            var progress = (PSDataCollection<ProgressRecord>)objJob[0].GetProperty("Progress");
            if (progress != null && progress.Count > 0)
            {
                job.PercentComplete = progress[progress.Count - 1].PercentComplete;
            }                
            else if (job.ChildJobs != null && !String.IsNullOrEmpty(job.ChildJobs[0].Id))   //ChildJobs can be get if we wrap an command in Job, and ChildJob will be a main process.
            {
                job.PercentComplete = job.ChildJobs[0].PercentComplete; //Important! If we wrap a command that does not have a native "asJob", we can not get the progress bar!
            }

            if (job.PercentComplete < 0) //Ok, we will make a fake progress bar.
            {
                int possibleElapseMin = 5;
                job.PercentComplete = JobFakePercentComplete(job.StartTime, possibleElapseMin);
                if (job.PercentComplete > 90) //If this is true, maybe we didn't guess with the possibleElapseMin, so double it! (:
                {
                    job.PercentComplete = JobFakePercentComplete(job.StartTime, possibleElapseMin * 2) - 10; //never show 100% >:)
                }
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

        private static int JobFakePercentComplete(DateTime jobStartTime, int fakeElapseMin) //if we can't get a progress bar, we will make a fake progress bar.
        {            
            DateTime endDateTime = jobStartTime.AddMinutes(fakeElapseMin);
            double timeSpanStart = Math.Abs((endDateTime - jobStartTime).TotalSeconds);
            double timeSpanNow = Math.Abs((jobStartTime - DateTime.Now).TotalSeconds);
            int FakePercentComplete = 100 - (int)(Math.Abs((timeSpanNow - timeSpanStart) / timeSpanStart) * 100);

            return FakePercentComplete;
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
                childJob.PercentComplete = (job.Progress.Count > 0) ? job.Progress[job.Progress.Count - 1].PercentComplete : -1;
                childJobs.Add(childJob);
            }

            return childJobs;
        }
    }
}

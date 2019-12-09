using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using System;
using System.Collections.Generic;
using System.Text;
using FengWo.BackgroundWorker.Hangfire;

namespace FengWo.BackgroundWorker
{
    public class SynchronizeUserInfoWorker :PeriodicBackgroundWorkerBase, ISingletonDependency// BackgroundWorker<SynchronizeUserInfoWorker>, ISingletonDependency//IBackgroundJob<SynchronizeUserInfoJobArgs>,ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public SynchronizeUserInfoWorker(AbpTimer timer,
            IBackgroundJobManager backgroundJobManager
            )
            : base(timer)
        {
            _backgroundJobManager = backgroundJobManager;
            timer.Period = int.Parse(TimeSpan.FromDays(7).TotalMilliseconds.ToString());
        }

        protected override void DoWork()
        {
           // _backgroundJobManager.Enqueue<SynchronizeUserInfoJob, SynchronizeUserInfoJobArgs>(new SynchronizeUserInfoJobArgs()); send something job to work
            //_backgroundJobManager..Enqueue<SynchronizeUserInfoJob, SynchronizeUserInfoJobArgs>(new SynchronizeUserInfoJobArgs());
        }
    }
}

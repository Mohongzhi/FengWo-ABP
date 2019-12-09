using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using FengWo.DBFactory;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Linq;

namespace FengWo.BackgroundWorker
{
    /// <summary>
    /// 同步班级信息
    /// </summary>
    public class SynchronizeClassInfoJob : BackgroundJob<SynchronizeClassInfoJobArgs>, ITransientDependency
    {
        /// <summary>
        /// ADO.NET支持
        /// </summary>
        private readonly IDBHelper _helper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        public SynchronizeClassInfoJob(
            IDBHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        [UnitOfWork]
        public override void Execute(SynchronizeClassInfoJobArgs args)
        {

            var dt = new DataTable();
            //do something
            UnitOfWorkManager.Current.SaveChanges();//保存
        }
    }
    public class SynchronizeClassInfoJobArgs
    {

    }
}

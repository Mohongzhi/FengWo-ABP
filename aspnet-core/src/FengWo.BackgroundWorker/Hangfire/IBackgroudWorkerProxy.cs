using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.BackgroundWorker.Hangfire
{
    public interface IBackgroudWorkerProxy
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="method"></param>
        void Excete<T>(Action method, WorkerConfig config) where T : IBackgroundWorkerDo;
    }
}

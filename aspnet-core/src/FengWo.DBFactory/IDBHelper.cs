using Abp.Application.Services;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FengWo.DBFactory
{
    /// <summary>
    /// DBHelper工厂
    /// </summary>
    public interface IDBHelper
    {
        string ConnectionString { get; set; }

        void Init(string ConnectionString);

        DataTable ExecuteQuery(string StrCmd);
        DataTable ExecuteQuery(string StrCmd, object dicWhere);
        DataTable ExecuteQuery(string StrCmd, Dictionary<string, object> dicWhere);

        int ExecuteNonQuery(string StrCmd);
        Task<int> ExecuteNonQueryAsync(string StrCmd);

        object ExecuteScalar(string StrCmd);
        object ExecuteScalar(string StrCmd, Dictionary<string,object> dicWhere);
        object ExecuteScalar(string StrCmd, object dicWhere);

        int ExecuteNonQuery(string StrCmd, Dictionary<string, object> dicWhere);
        int ExecuteNonQuery(string StrCmd, object dicWhere);

        int ExecuteNonQueryWithTimeOut(string StrCmd, int TimeOut);
        int ExecuteNonQueryWithTimeOut(string StrCmd, Dictionary<string, object> dicWhere, int TimeOut);
        int ExecuteNonQueryWithTimeOut(string StrCmd, object dicWhere,int TimeOut);
        Task<int> ExecuteNonQueryWithTimeOutAsync(string StrCmd, Dictionary<string, object> dicWhere, int TimeOut);
        Task<int> ExecuteNonQueryWithTimeOutAsync(string StrCmd, object dicWhere, int TimeOut);

        Task<int> ExecuteNonQueryAsync(string StrCmd, Dictionary<string, object> dicWhere);
        Task<int> ExecuteNonQueryAsync(string StrCmd, object dicWhere);

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        int ExecuteNonQueryByTransaction(string StrCmd);
        int ExecuteNonQueryByTransaction(string StrCmd,Dictionary<string,object> dicWhere);

        int BulkInsert(DataTable table);

    }
}

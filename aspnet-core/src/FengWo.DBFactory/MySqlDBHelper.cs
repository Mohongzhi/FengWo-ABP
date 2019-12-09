using Abp.Dependency;
using FengWo.Configuration;
using FengWo.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FengWo.DBFactory
{
    public class MySqlDBHelper : IDBHelper//, ITransientDependency
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MySqlDBHelper(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var configBuild = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            //var configuration = AppConfigurations.Get(Configuration.AppConfigurat);
            ConnectionString = configBuild.GetConnectionString(FengWoConsts.ConnectionStringName);  //configuration.GetConnectionString(FengWoConsts.ConnectionStringName);
            this.ConnectionString = ConnectionString;
        }

        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnectionString
        {
            get; set;
        }

        #region BulkInsert

        ///<summary>
        ///大批量数据插入,返回成功插入行数
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回成功插入行数</returns>
        public int BulkInsert(DataTable table)
        {
            if (string.IsNullOrEmpty(table.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
            if (table.Rows.Count == 0) return 0;
            int insertCount = 0;
            if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/someTemp/"))
            {
                Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/someTemp/");
            }
            string tmpPath = _hostingEnvironment.WebRootPath + "/someTemp/" + Guid.NewGuid().ToString()+".temp";// Path.GetTempFileName();
            string csv = DataTableToCsv(table);
            File.WriteAllText(tmpPath, csv);
            // MySqlTransaction tran = null;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    //tran = conn.BeginTransaction();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = table.TableName,
                    };
                    //bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToArray());
                    insertCount = bulk.Load();
                    // tran.Commit();
                }
                catch (MySqlException ex)
                {
                    File.Delete(tmpPath);
                    // if (tran != null) tran.Rollback();
                    throw ex;
                }
            }
            File.Delete(tmpPath);
            return insertCount;
        }

        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }


        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string StrCmd)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.Connection.Open();
                var result = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return result;
            }
        }

        public int ExecuteNonQuery(string StrCmd, Dictionary<string, object> dicWhere)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.CommandTimeout = 20000;
                cmd.Connection.Open();
                foreach (var item in dicWhere)
                {
                    cmd.Parameters.Add(new MySqlParameter(item.Key, item.Value));
                }
                var result = cmd.ExecuteNonQuery();
                //MySqlBulkLoader loader = new MySqlBulkLoader(cmd.Connection) { }
                cmd.Connection.Close();
                return result;
            }
        }

        public int ExecuteNonQuery(string StrCmd, object dicWhere)
        {
            var str = JsonConvert.SerializeObject(dicWhere);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            return ExecuteNonQuery(StrCmd, dic);
        }


        public Task<int> ExecuteNonQueryAsync(string StrCmd)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.Connection.Open();
                var result = cmd.ExecuteNonQueryAsync();
                cmd.Connection.Close();
                return result;
            }
        }

        public Task<int> ExecuteNonQueryAsync(string StrCmd, Dictionary<string, object> dicWhere)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.Connection.Open();
                foreach (var item in dicWhere)
                {
                    cmd.Parameters.Add(new MySqlParameter(item.Key, item.Value));
                }
                var result = cmd.ExecuteNonQueryAsync();
                cmd.Connection.Close();
                return result;
            }
        }

        public Task<int> ExecuteNonQueryAsync(string StrCmd, object dicWhere)
        {
            var str = JsonConvert.SerializeObject(dicWhere);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            return ExecuteNonQueryAsync(StrCmd, dic);
        }

        #region ExecuteNonByTimeOut
        public int ExecuteNonQueryWithTimeOut(string StrCmd,int TimeOut)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.Connection.Open();
                var result = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return result;
            }
        }

        public int ExecuteNonQueryWithTimeOut(string StrCmd, Dictionary<string, object> dicWhere,int TimeOut)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.CommandTimeout = TimeOut;
                cmd.Connection.Open();
                foreach (var item in dicWhere)
                {
                    cmd.Parameters.Add(new MySqlParameter(item.Key, item.Value));
                }
                var result = cmd.ExecuteNonQuery();
                //MySqlBulkLoader loader = new MySqlBulkLoader(cmd.Connection) { }
                cmd.Connection.Close();
                return result;
            }
        }

        public int ExecuteNonQueryWithTimeOut(string StrCmd, object dicWhere, int TimeOut)
        {
            var str = JsonConvert.SerializeObject(dicWhere);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            return ExecuteNonQueryWithTimeOut(StrCmd, dic,TimeOut);
        }

        public Task<int> ExecuteNonQueryWithTimeOutAsync(string StrCmd, Dictionary<string, object> dicWhere, int TimeOut)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.CommandTimeout = TimeOut;
                cmd.Connection.Open();
                foreach (var item in dicWhere)
                {
                    cmd.Parameters.Add(new MySqlParameter(item.Key, item.Value));
                }
                var result = cmd.ExecuteNonQuery();
                //MySqlBulkLoader loader = new MySqlBulkLoader(cmd.Connection) { }
                cmd.Connection.Close();
                return Task.FromResult(result);
            }
        }

        public Task<int> ExecuteNonQueryWithTimeOutAsync(string StrCmd, object dicWhere, int TimeOut)
        {
            var str = JsonConvert.SerializeObject(dicWhere);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            return ExecuteNonQueryWithTimeOutAsync(StrCmd, dic, TimeOut);
        }
        #endregion
        #endregion

        #region 事务
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public int ExecuteNonQueryByTransaction(string StrCmd)
        {
            var result = 0;
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            MySqlTransaction mySqlTransaction = connection.BeginTransaction();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(StrCmd, connection))
                {
                    result = cmd.ExecuteNonQuery();
                    mySqlTransaction.Commit();
                }
            }
            catch
            {
                mySqlTransaction.Rollback();//回滚
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public int ExecuteNonQueryByTransaction(string StrCmd, Dictionary<string, object> dicWhere)
        {
            var result = 0;
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            MySqlTransaction mySqlTransaction = connection.BeginTransaction();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(StrCmd, connection))
                {
                    foreach (var item in dicWhere)
                    {
                        cmd.Parameters.Add(new MySqlParameter(item.Key, item.Value));
                    }
                    result = cmd.ExecuteNonQuery();
                    mySqlTransaction.Commit();
                }
            }
            catch
            {
                mySqlTransaction.Rollback();//回滚
            }
            finally
            {
                connection.Close();
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string StrCmd)
        {
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(StrCmd, ConnectionString))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string StrCmd, object dicWhere)
        {
            var str = JsonConvert.SerializeObject(dicWhere);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            return ExecuteQuery(StrCmd, dic);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string StrCmd, Dictionary<string, object> dicWhere)
        {
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(StrCmd, ConnectionString))
            {
                foreach (var item in dicWhere)
                {
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter(item.Key, item.Value));
                }

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(string StrCmd)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.Connection.Open();
                var result = cmd.ExecuteScalar();
                cmd.Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrCmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(string StrCmd, Dictionary<string, object> dicWhere)
        {
            using (MySqlCommand cmd = new MySqlCommand(StrCmd, new MySqlConnection(ConnectionString)))
            {
                cmd.Connection.Open();
                foreach (var item in dicWhere)
                {
                    cmd.Parameters.Add(new MySqlParameter(item.Key, item.Value));
                }
                var result = cmd.ExecuteScalar();
                cmd.Connection.Close();
                return result;
            }
        }

        public object ExecuteScalar(string StrCmd, object dicWhere)
        {
            var str = JsonConvert.SerializeObject(dicWhere);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            return ExecuteScalar(StrCmd, dic);
        }

        public void Init(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
    }
}

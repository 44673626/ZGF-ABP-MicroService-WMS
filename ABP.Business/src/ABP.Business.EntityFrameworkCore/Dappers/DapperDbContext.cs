using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.Business.Dappers
{
    /// <summary>
    /// 封装Dapper一些通用方法，执行SQL和存储过程
    /// </summary>
    public class DapperDbContext
    {
        private readonly string _connectionString1;
        private readonly string _connectionString2;

        public DapperDbContext(IConfiguration configuration)
        {
            _connectionString1 = configuration.GetConnectionString("Default");
            _connectionString2 = configuration.GetConnectionString(ABPVNextDbProperties.ConnectionStringName);
        }

        private IDbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null,
            DatabaseType databaseType = DatabaseType.Default)
        {
            var connectionString = databaseType == DatabaseType.Default ? _connectionString1 : _connectionString2;
            using (var connection = CreateConnection(connectionString))
            {
                return await connection.QueryAsync<T>(sql, param);
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, object param = null, DatabaseType databaseType = DatabaseType.Default)
        {
            var connectionString = databaseType == DatabaseType.Default ? _connectionString1 : _connectionString2;
            using (var connection = CreateConnection(connectionString))
            {
                return await connection.ExecuteAsync(sql, param);
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ExecuteProcedureAsync<T>(string procedureName, 
            object param = null, DatabaseType databaseType = DatabaseType.Default)
        {
            var connectionString = databaseType == DatabaseType.Default ? _connectionString1 : _connectionString2;
            using (var connection = CreateConnection(connectionString))
            {
                return await connection.QueryAsync<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public async Task<int> ExecuteProcedureNonQueryAsync(string procedureName, object param = null,
            DatabaseType databaseType = DatabaseType.Default)
        {
            var connectionString = databaseType == DatabaseType.Default ? _connectionString1 : _connectionString2;
            using (var connection = CreateConnection(connectionString))
            {
                return await connection.ExecuteAsync(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }
    }

    public enum DatabaseType
    {
        Default,
        Secondary
    }
}

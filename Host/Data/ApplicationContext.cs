using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Host.Data
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        #region Method Helper
        public async Task<object> ExecuteScalarAsync(string sql, CommandType cmdType = CommandType.Text, params SqlParameter[] sqlParams)
        {
            var con = Database.GetDbConnection();

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                cmd.Parameters.AddRange(sqlParams);

                return await cmd.ExecuteScalarAsync();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, CommandType cmdType = CommandType.Text, params SqlParameter[] sqlParams)
        {
            var con = Database.GetDbConnection();

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                cmd.Parameters.AddRange(sqlParams);

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<DbDataReader> ExecuteReaderAsync(string sql, CommandType cmdType = CommandType.Text, params SqlParameter[] sqlParams)
        {
            var con = Database.GetDbConnection();

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                cmd.Parameters.AddRange(sqlParams);

                return await cmd.ExecuteReaderAsync();
            }
        }

        #endregion
    }
}

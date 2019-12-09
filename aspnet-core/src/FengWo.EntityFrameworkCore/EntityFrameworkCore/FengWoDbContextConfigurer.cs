using System.Data.Common;
using FengWo.Configuration;
using FengWo.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FengWo.EntityFrameworkCore
{
    public static class FengWoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<FengWoDbContext> builder, string connectionString)
        {
            //builder.UseSqlServer(connectionString);
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<FengWoDbContext> builder, DbConnection connection)
        {
            //builder.UseSqlServer(connection);
            builder.UseMySql(connection);
        }
    }
}

using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MHPQ.EntityFrameworkCore
{
    public static class MHPQDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MHPQDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MHPQDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}

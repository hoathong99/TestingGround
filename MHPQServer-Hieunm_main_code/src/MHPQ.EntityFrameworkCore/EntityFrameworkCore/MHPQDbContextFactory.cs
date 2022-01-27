using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MHPQ.Configuration;
using MHPQ.Web;

namespace MHPQ.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class MHPQDbContextFactory : IDesignTimeDbContextFactory<MHPQDbContext>
    {
        public MHPQDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MHPQDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            MHPQDbContextConfigurer.Configure(builder, configuration.GetConnectionString(MHPQConsts.ConnectionStringName));

            return new MHPQDbContext(builder.Options);
        }
    }
}

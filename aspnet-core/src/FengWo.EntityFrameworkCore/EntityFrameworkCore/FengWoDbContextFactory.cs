using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using FengWo.Configuration;
using FengWo.Web;

namespace FengWo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class FengWoDbContextFactory : IDesignTimeDbContextFactory<FengWoDbContext>
    {
        public FengWoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FengWoDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            FengWoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(FengWoConsts.ConnectionStringName));

            return new FengWoDbContext(builder.Options);
        }
    }
}

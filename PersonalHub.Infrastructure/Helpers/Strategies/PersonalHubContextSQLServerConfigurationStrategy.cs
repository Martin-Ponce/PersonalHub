using Microsoft.EntityFrameworkCore;

namespace PersonalHub.Infrastructure.Helpers.Strategies
{
    internal class PersonalHubContextSQLServerConfigurationStrategy : IDbConfigurationsStrategy
    {
        public ModelBuilder ApplyConfigurations(ModelBuilder modelBuilder)
        {
            return modelBuilder;
        }
    }
}

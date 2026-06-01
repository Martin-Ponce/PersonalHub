using Microsoft.EntityFrameworkCore;

namespace PersonalHub.Infrastructure.Helpers.Strategies
{
    internal interface IDbConfigurationsStrategy
    {
        ModelBuilder ApplyConfigurations(ModelBuilder modelBuilder);
    }
}

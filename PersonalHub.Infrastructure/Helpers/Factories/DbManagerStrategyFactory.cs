using PersonalHub.Infrastructure.Helpers.Strategies;
using static PersonalHub.Infrastructure.InfrastructureConstants;

namespace PersonalHub.Infrastructure.Helpers.Factories
{
    internal class DbManagerStrategyFactory
    {
        private static readonly Dictionary<string, IDbConfigurationsStrategy> SecuritySupportedDatabasesConfigurations = new()
        {
            { DatabaseProviders.SqlServer, new PersonalHubContextSQLServerConfigurationStrategy() }
        };
        public static IDbConfigurationsStrategy GetConfigurationStrategy(string databaseProvider)
        {
            if (SecuritySupportedDatabasesConfigurations.TryGetValue(databaseProvider, out var strategy))
            {
                return strategy;
            }
            throw new NotImplementedException(
                $"No se encontró una implementación para el proveedor de base de datos para la configuracion de seguridad: {databaseProvider}"
            );
        }
    }
}

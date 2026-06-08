using Microsoft.AspNetCore.DataProtection;
using PersonalHub.API.Dtos.Configuration;
using PersonalHub.API.Helpers;
using System.Runtime.InteropServices;
using static PersonalHub.API.Helpers.ApiConstants;

namespace PersonalHub.API.Extensions.Configuration
{
    public static partial class ConfigurationExtensions
    {
        private static IDataProtector? _protector;
        private const string _dataProtectionPurpose = "PersonalHub.ConnectionString"; //to do: make this more generic and/or move to constants idk

        public static GeneralConfiguration GetConfiguration(this IConfiguration configuration)
        {
            return new GeneralConfiguration()
            {
                ConnectionString = GetConnectionStringValue(configuration, DATABASE_CONTEXT_CONNECTION_STRING_NAME),
                Provider = GetProvider(configuration),
                RateLimiterMaxCalls = GetRateLimiterMaxCalls(configuration),
            };
        }

        private static string GetConnectionStringValue(this IConfiguration configuration, string connectionStringName)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName)
                .TryOverwriteWithEnviromentValue("CONNECTION_STRING") ??
                throw new InvalidDataException(
                    $"Falta connection string '{connectionStringName}'"
                );

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _protector != null)
            {
                try
                {
                    connectionString = _protector.Unprotect(connectionString);
                }
                catch (Exception)
                {
                    // Value is not yet encrypted TO DO: Add logging and stuff i dont remember rn
                }
            }

            return connectionString;
        }

        private static string GetProvider(this IConfiguration configuration)
        {
            var provider = configuration["ConnectionStrings:Provider"]
                .TryOverwriteWithEnviromentValue("PROVIDER")
                ?? throw new InvalidDataException(
                    "Falta la configuración del proveedor de base de datos"
                );

            if (!Enum.TryParse<SupportedProviders.SupportedProvidersName>(provider, ignoreCase: true, out _))
                throw new InvalidDataException(
                    $"El proveedor '{provider}' no es soportado. Proveedores soportados: {string.Join(", ", Enum.GetNames<SupportedProviders.SupportedProvidersName>())}"
                );

            return provider;
        }

        private static int GetRateLimiterMaxCalls(this IConfiguration configuration)
        {
            var value = configuration["MaxCallsPerMinute"]
                .TryOverwriteWithEnviromentValue("MAX_CALLS")
                ?? throw new InvalidDataException(
                    "Falta la configuración de la máxima cantidad de llamadas"
                );

            if (!int.TryParse(value, out var result))
                throw new InvalidDataException(
                    "La máxima cantidad de llamadas tiene un valor inválido"
                );

            return result;
        }

        public static IConfiguration SetEncryption(this IConfiguration configuration)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var keysDirectory = new DirectoryInfo(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        APP_NAME,
                        "DataProtection-Keys"
                    )
                );

                var services = new ServiceCollection();
                services.AddDataProtection()
                    .SetApplicationName(APP_NAME)
                    .PersistKeysToFileSystem(keysDirectory)
                    .ProtectKeysWithDpapi(); //to do: test outside mini DI container, maybe use the one from the app itself instead of creating a new one just for this

                var serviceProvider = services.BuildServiceProvider();
                var provider = serviceProvider.GetRequiredService<IDataProtectionProvider>();
                _protector = provider.CreateProtector(_dataProtectionPurpose);
            }

            return configuration;
        }
    }
}

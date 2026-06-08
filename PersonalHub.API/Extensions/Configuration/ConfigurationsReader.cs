using BISoft.ECommerce.Prices.Api.Dtos.Configurations;
using BISoft.ECommerce.Prices.Api.Helpers;
using BISoft.ECommerce.Prices.Domain.Exceptions;
using BISoft.ECommerce.Prices.Domain.Validators;
using PersonalHub.API.Dtos.Configuration;
using PersonalHub.API.Extensions.Configuration;
using PersonalHub.API.Helpers;
using System.Runtime.InteropServices;
using TechSoft.Configuration.EncriptacionAppSettings;
using static BISoft.ECommerce.Prices.Api.Helpers.ApiConstants;
using static BISoft.ECommerce.Prices.Domain.DomainConstants;
using static PersonalHub.API.Helpers.ApiConstants;

namespace BISoft.ECommerce.Prices.Api.Extensions.Configuration
{
    public static partial class ConfigurationExtensions
    {
        public static GeneralConfiguration GetConfiguration(this IConfiguration configuration)
        {
            return new GeneralConfiguration()
            {
                ConnectionString = GetConnectionStringValue(configuration, "DefaultConnection"),

            };
        }
        private static string GetConnectionStringValue(this IConfiguration configuration, string connectionStringName)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName).TryOverwriteWithEnviromentValue("CONNECTION_STRING") ??
                throw new InvalidDataException(
                    $"Falta connection string '{connectionStringName}'"
                );
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                //here goes unencrypt logic for connection string
            }
            return connectionString;
        }
        private static string GetProvider(this IConfiguration configuration)
        {
            var provider = configuration["Provider"].TryOverwriteWithEnviromentValue("PROVIDER") ?? throw new InvalidDataException(
                "Falta la configuración del proveedor de base de datos"
            );
            if ()
                throw new InvalidDataException(
                    $"El proveedor '{provider}' no es soportado. Proveedores soportados"
                );
            return provider;
        }
        private static int GetRateLimiterMaxCalls(this IConfiguration configuration)
        {
            var value = configuration["MaxCallsPerMinute"].TryOverwriteWithEnviromentValue("MAX_CALLS") ?? throw new InvalidDataException(
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
                //logic to set encryption for appsettings
            }
            return configuration;
        }
    }
}

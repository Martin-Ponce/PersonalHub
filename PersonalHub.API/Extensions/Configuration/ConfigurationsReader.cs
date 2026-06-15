using Microsoft.AspNetCore.DataProtection;
using PersonalHub.API.Dtos.Configuration;
using PersonalHub.API.Helpers;
using System.Runtime.CompilerServices;
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
            var connectionString = GetConnectionStringValue(configuration, DATABASE_CONTEXT_CONNECTION_STRING_NAME);
            var provider = GetProvider(configuration);
            return new GeneralConfiguration()
            {
                ConnectionString = connectionString,
                Provider = provider,
                RateLimiterMaxCalls = GetRateLimiterMaxCalls(configuration),
                JwtConfiguration = GetJwtConfiguration(configuration),
                Logger = GetLoggerConfiguration(configuration, connectionString, provider),
                Cors = GetCorsConfiguration(configuration)
            };
        }

        private static string GetConnectionStringValue(this IConfiguration configuration, string connectionStringName)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName)
                .TryOverwriteWithEnviromentValue("CONNECTION_STRING") ??
                throw new InvalidDataException(
                    $"Missing connection string '{connectionStringName}'"
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

        private static SupportedProviders GetProvider(this IConfiguration configuration)
        {
            var raw = configuration["ConnectionStrings:Provider"]
                .TryOverwriteWithEnviromentValue("PROVIDER")
                ?? throw new InvalidDataException(
                    "Missing database provider configuration"
                );

            if (!Enum.TryParse<SupportedProviders>(raw, ignoreCase: true, out var provider))
                throw new InvalidDataException(
                    $"The provider '{raw}' is not supported. Supported providers: {string.Join(", ", Enum.GetNames<SupportedProviders>())}"
                );

            return provider;
        }

        private static int GetRateLimiterMaxCalls(this IConfiguration configuration)
        {
            var value = configuration["MaxCallsPerMinute"]
                .TryOverwriteWithEnviromentValue("MAX_CALLS")
                ?? throw new InvalidDataException(
                    "Missing maximum calls configuration"
                );

            if (!int.TryParse(value, out var result))
                throw new InvalidDataException(
                    "Maximum calls has an invalid value"
                );

            return result;
        }

        private static JwtConfiguration GetJwtConfiguration(this IConfiguration configuration)
        {
            var key = configuration["Jwt:Key"]
                .TryOverwriteWithEnviromentValue("JWT_KEY")
                ?? throw new InvalidDataException("Missing JWT key configuration");

            var issuer = configuration["Jwt:Issuer"]
                .TryOverwriteWithEnviromentValue("JWT_ISSUER")
                ?? throw new InvalidDataException("Missing JWT issuer configuration");

            var audience = configuration["Jwt:Audience"]
                .TryOverwriteWithEnviromentValue("JWT_AUDIENCE")
                ?? throw new InvalidDataException("Missing JWT audience configuration");

            var accessDurationRaw = configuration["Jwt:AccessDurationInMinutes"]
                .TryOverwriteWithEnviromentValue("JWT_ACCESS_DURATION_MINUTES")
                ?? throw new InvalidDataException("Missing JWT access token duration configuration");

            if (!int.TryParse(accessDurationRaw, out var accessDurationMinutes))
                throw new InvalidDataException("JWT access token duration has an invalid value");

            return new JwtConfiguration(
                new AccessTokenConfiguration(key, issuer, audience, TimeSpan.FromMinutes(accessDurationMinutes))
            );
        }

        private static GeneralLoggerConfiguration GetLoggerConfiguration(this IConfiguration configuration, string connectionString, SupportedProviders provider)
        {
            var sqlitePath = configuration["Logger:Sqlite:Path"]
                ?? throw new InvalidDataException("Missing SQLite database path configuration for logger");

            var sqliteMinLevelRaw = configuration["Logger:Sqlite:MinimumLevel"]
                ?? throw new InvalidDataException("Missing SQLite minimum log level");

            if (!Enum.TryParse<Serilog.Events.LogEventLevel>(sqliteMinLevelRaw, ignoreCase: true, out var sqliteMinLevel))
                throw new InvalidDataException($"The minimum log level '{sqliteMinLevelRaw}' is not valid");

            var mainMinLevelRaw = configuration["Logger:Main:MinimumLevel"]
                ?? throw new InvalidDataException("Missing minimum log level for main database");

            if (!Enum.TryParse<Serilog.Events.LogEventLevel>(mainMinLevelRaw, ignoreCase: true, out var mainMinLevel))
                throw new InvalidDataException($"The minimum log level '{mainMinLevelRaw}' is not valid");

            var logHttpRaw = configuration["Logger:LogHttpRequests"] ?? "false";
            if (!bool.TryParse(logHttpRaw, out var logHttp))
                throw new InvalidDataException("The value of 'LogHttpRequests' must be a valid boolean");

            return new GeneralLoggerConfiguration
            {
                LogHttpRequests = logHttp,
                Sqlite = new DbLoggerConfiguration(sqlitePath, SupportedProviders.Sqlite, sqliteMinLevel),
                MainDatabase = new DbLoggerConfiguration(connectionString, provider, mainMinLevel),
            };
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
        public static CorsConfiguration GetCorsConfiguration(this IConfiguration configuration)
        {
            return new CorsConfiguration(configuration.GetCorsOrigins(), configuration.GetCorsHeaders());
        }

        private static string[] GetCorsOrigins(this IConfiguration configuration)
        {
            var envValue = Environment.GetEnvironmentVariable("CORS_ORIGINS");
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue.Split(new[] { ',', }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            var section = configuration.GetSection("Cors:Origins");
            var origins = section.Get<string[]>();

            if (origins == null || origins.Length <= 0)
                throw new InvalidDataException("Missing 'Cors:Origins' configuration");

            return origins;
        }
        private static string[] GetCorsHeaders(this IConfiguration configuration)
        {
            var envValue = Environment.GetEnvironmentVariable("CORS_HEADERS");
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue.Split(new[] { ',', }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            var section = configuration.GetSection("Cors:Headers");
            var origins = section.Get<string[]>();

            if (origins == null || origins.Length <= 0)
                throw new InvalidDataException("Missing 'Cors:Headers' configuration");

            return origins;
        }
    }
}

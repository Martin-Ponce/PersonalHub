using PersonalHub.API.Dtos.Configuration;
using PersonalHub.API.Helpers;
using static PersonalHub.API.Helpers.ApiConstants;
using Serilog;
using Serilog.Core;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Runtime.InteropServices;

namespace PersonalHub.API.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration AddConfiguration(this LoggerConfiguration loggerConfiguration, GeneralConfiguration configuration)
        {
            loggerConfiguration
                .AddSqliteConfiguration(configuration.Logger.Sqlite)
                .AddMainDatabase(configuration.Logger.MainDatabase);

            if (!configuration.Logger.LogHttpRequests)
                loggerConfiguration.Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore"));

            var isDebug =
#if DEBUG
                true;
#else
                false;
#endif
            if (isDebug)
                loggerConfiguration.WriteTo.Console(Serilog.Events.LogEventLevel.Debug);

            return loggerConfiguration
                .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore.Database.Command"));
        }

        private static LoggerConfiguration AddMainDatabase(this LoggerConfiguration loggerConfiguration, DbLoggerConfiguration configuration)
        {
            return configuration.DatabaseProvider switch
            {
                SupportedProviders.SqlServer => loggerConfiguration.AddSqlServerConfiguration(configuration),
                _ => throw new InvalidOperationException($"The database provider '{configuration.DatabaseProvider}' is not supported by the logger.")
            };
        }

        private static LoggerConfiguration AddSqlServerConfiguration(this LoggerConfiguration loggerConfiguration, DbLoggerConfiguration configuration)
        {
            return loggerConfiguration.WriteTo.MSSqlServer(
                connectionString: configuration.DatabaseConnectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = ApiConstants.LOGGER_TABLE_NAME,
                    SchemaName = "tlm",
                    LevelSwitch = new LoggingLevelSwitch { MinimumLevel = configuration.MinimumLevel },
                    AutoCreateSqlTable = true
                }
            );
        }

        private static LoggerConfiguration AddSqliteConfiguration(this LoggerConfiguration loggerConfiguration, DbLoggerConfiguration configuration)
        {
            var path = ResolveSqlitePath(configuration.DatabaseConnectionString);
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            var tableName = ApiConstants.LOGGER_TABLE_NAME.ToSnakeCase();
            return loggerConfiguration.WriteTo.SQLite(
                sqliteDbPath: path,
                tableName: tableName,
                restrictedToMinimumLevel: configuration.MinimumLevel
            );
        }

        private static string ResolveSqlitePath(string configuredPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return configuredPath;

            return Path.Combine("/logs", configuredPath);
        }
    }
}

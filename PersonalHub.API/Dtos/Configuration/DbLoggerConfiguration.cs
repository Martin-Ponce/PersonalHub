using PersonalHub.API.Helpers;
using Serilog.Events;
using static PersonalHub.API.Helpers.ApiConstants;

namespace PersonalHub.API.Dtos.Configuration
{
    public class DbLoggerConfiguration
    {
        public string DatabaseConnectionString { get; }
        public SupportedProviders DatabaseProvider { get; }
        public LogEventLevel MinimumLevel { get; }

        public DbLoggerConfiguration(string databaseConnectionString, SupportedProviders databaseProvider, LogEventLevel minimumLevel)
        {
            DatabaseConnectionString = databaseConnectionString;
            DatabaseProvider = databaseProvider;
            MinimumLevel = minimumLevel;
        }
    }
}

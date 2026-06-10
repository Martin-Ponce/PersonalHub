using Serilog.Events;

namespace PersonalHub.API.Dtos.Configuration
{
    public class GeneralConfiguration
    {
        public required string ConnectionString { get; set; }
        public required string Provider { get; set; }
        public required int RateLimiterMaxCalls { get; set; }
        public LogEventLevel MinimumLevel { get; set; }
        public required JwtConfiguration JwtConfiguration { get; set; }


    }
}

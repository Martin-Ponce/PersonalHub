using static PersonalHub.API.Helpers.ApiConstants;

namespace PersonalHub.API.Dtos.Configuration
{
    public class GeneralConfiguration
    {
        public required string ConnectionString { get; set; }
        public required SupportedProviders Provider { get; set; }
        public required int RateLimiterMaxCalls { get; set; }
        public required JwtConfiguration JwtConfiguration { get; set; }
        public required GeneralLoggerConfiguration Logger { get; set; }
        public required CorsConfiguration Cors { get; set; }
    }
}

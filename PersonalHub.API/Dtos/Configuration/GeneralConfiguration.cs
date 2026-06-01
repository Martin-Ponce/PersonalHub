namespace PersonalHub.API.Dtos.Configuration
{
    public class GeneralConfiguration
    {
        public required DbConnectionConfiguration SecurityConnection { get; set; }
        public required GeneralLoggerConfiguration Logger { get; set; }
        public required TelemetryConfiguration Telemetry { get; set; }
        public required int RateLimiterMaxCalls { get; set; }
    }
}

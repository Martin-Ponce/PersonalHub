namespace PersonalHub.API.Dtos.Configuration
{
    public class GeneralLoggerConfiguration
    {
        public bool LogHttpRequests { get; set; }
        public required DbLoggerConfiguration Sqlite { get; set; }
        public required DbLoggerConfiguration MainDatabase { get; set; }
    }
}

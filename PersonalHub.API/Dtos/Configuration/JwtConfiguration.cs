namespace PersonalHub.API.Dtos.Configuration
{
    public class JwtConfiguration
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public TimeSpan TokenDuration { get; set; }
        public TimeSpan RefreshTokenDuration { get; set; }
    }
}

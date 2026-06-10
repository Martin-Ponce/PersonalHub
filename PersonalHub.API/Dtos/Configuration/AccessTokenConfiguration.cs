namespace PersonalHub.API.Dtos.Configuration
{
    public record AccessTokenConfiguration(
        string Key,
        string Issuer,
        string Audience,
        TimeSpan Duration)
    { }
}

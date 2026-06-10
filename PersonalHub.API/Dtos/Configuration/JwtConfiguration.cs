namespace PersonalHub.API.Dtos.Configuration
{
    public class JwtConfiguration(AccessTokenConfiguration accessTokens)
    {
        public AccessTokenConfiguration AccessTokens { get; set; } = accessTokens;
    }
}

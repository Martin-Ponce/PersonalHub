using System.Text.Json.Serialization;

namespace PersonalHub.Application.Dtos.Security
{
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public int AccessExpiration { get; set; }
        public required string RefreshToken { get; set; }
        public int RefreshExpiration { get; set; }
    }
}

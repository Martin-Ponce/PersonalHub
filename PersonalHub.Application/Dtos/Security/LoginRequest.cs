namespace PersonalHub.Application.Dtos.Security
{
    public class LoginRequest
    {
        public required string User { get; set; }
        public required string Password { get; set; }
        public string? Address { get; set; } = string.Empty;
        public bool Remember { get; set; } = false;
    }
}

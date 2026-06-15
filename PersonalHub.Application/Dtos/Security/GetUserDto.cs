namespace PersonalHub.Application.Dtos.Security
{
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

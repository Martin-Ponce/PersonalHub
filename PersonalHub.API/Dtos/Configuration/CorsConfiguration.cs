namespace PersonalHub.API.Dtos.Configuration
{
    public class CorsConfiguration(string[] origins, string[] headers)
    {
        public string[] Origins { get; set; } = origins;
        public string[] Headers { get; set; } = headers;
    }
}

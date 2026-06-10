using PersonalHub.API.Extensions.Configuration;

namespace PersonalHub.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var generalConfiguration = builder.Configuration.GetConfiguration();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
        Console.WriteLine($"{generalConfiguration.Provider}, {generalConfiguration.ConnectionString}, {generalConfiguration.RateLimiterMaxCalls}");
    }
}

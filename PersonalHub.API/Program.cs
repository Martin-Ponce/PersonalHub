using PersonalHub.API.Extensions;
using PersonalHub.API.Extensions.Configuration;

namespace PersonalHub.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var generalConfiguration = builder.Configuration.GetConfiguration();
        builder.Services
            .ConfigureApiVersioning()
            .ConfigureHealthChecks(generalConfiguration)
            .ConfigureLogger(generalConfiguration)
            .ConfigureServices(generalConfiguration)
            .ConfigureContexts(generalConfiguration)
            .InjectConfigurations(generalConfiguration)
            .ConfigureRateLimiter(generalConfiguration)
            .ConfigureCors(generalConfiguration)
            .AddAuthorization()
            .AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapHealthChecks("/health");
        app.MapControllers();

        app.Run();
        Console.WriteLine($"{generalConfiguration.Provider}, {generalConfiguration.ConnectionString}, {generalConfiguration.RateLimiterMaxCalls}");
    }
}

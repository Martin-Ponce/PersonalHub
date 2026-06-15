using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using PersonalHub.API.Dtos.Configuration;
using PersonalHub.API.Helpers;
using PersonalHub.API.Helpers.HealthChecks;
using PersonalHub.API.Services;
using PersonalHub.Application.Services;
using PersonalHub.Domain.Contracts;
using PersonalHub.Infrastructure.Contexts;
using PersonalHub.Infrastructure.Repositories;
using Serilog;
using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using static PersonalHub.API.Helpers.ApiConstants;

namespace PersonalHub.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, GeneralConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddCheck("database", new DatabaseHealthCheck(configuration.ConnectionString), tags: ["ready"]);
            return services;
        }

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, GeneralConfiguration configuration)
        {
            var jwt = configuration.JwtConfiguration;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwt.AccessTokens.Issuer,
                            ValidAudience = jwt.AccessTokens.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.AccessTokens.Key)),
                            ClockSkew = TimeSpan.Zero
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = ctx =>
                            {
                                ctx.Request.Cookies.TryGetValue(ApiConstants.Cookies.ACCESS_TOKEN, out var accessToken);
                                if (!string.IsNullOrEmpty(accessToken))
                                    ctx.Token = accessToken;
                                return Task.CompletedTask;
                            }
                        };
                    });
            return services;
        }
        public static IServiceCollection ConfigureLogger(this IServiceCollection services, GeneralConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                    .AddConfiguration(configuration)
                    .Destructure.ToMaximumDepth(3)
                    .CreateLogger();
            services.AddSerilog();
            return services;
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services, GeneralConfiguration configuration)
        {
            //repos
            services.AddScoped<IRepository, Repository<PersonalHubContext>>();
            services.AddScoped<ILedgerRepository, LedgerRepository>();

            //services
            services.AddScoped<LedgerService>();
            services.AddScoped<SecurityService>();
            services.AddScoped<TokenService>();
            return services;
        }
        public static IServiceCollection ConfigureContexts(this IServiceCollection services, GeneralConfiguration configuration)
        {
            services.AddDbContext<PersonalHubContext>(options =>
            {
                switch (configuration.Provider)
                {
                    case SupportedProviders.SqlServer:
                        options.UseSqlServer(configuration.ConnectionString);
                        break;
                    default:
                        throw new InvalidOperationException($"The database provider '{configuration.Provider}' is not supported.");
                }
            });
            return services;
        }
        public static IServiceCollection InjectConfigurations(this IServiceCollection services, GeneralConfiguration configuration)
        {
            services.AddSingleton(configuration.JwtConfiguration);
            return services;
        }
        public static IServiceCollection ConfigureRateLimiter(this IServiceCollection services, GeneralConfiguration configuration)
        {
            services.AddRateLimiter(_ =>
            {
                _.OnRejected = (context, _) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.WriteAsync("Demasiados requests. Intente más tarde.", cancellationToken: _);
                    return new ValueTask();
                };
                _.AddFixedWindowLimiter(policyName: FIXED_RATE_LIMITING_POLICY, options =>
                {
                    options.PermitLimit = configuration.RateLimiterMaxCalls;
                    options.Window = TimeSpan.FromMinutes(1);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 0;
                });
            });
            return services;
        }
        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = VERSION_1;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("X-Version") // Header X-Version: 1.0
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
        public static IServiceCollection ConfigureCors(this IServiceCollection services, GeneralConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(ALLOW_ALL_CORS_POLICY,
                    builder => builder.WithOrigins(configuration.Cors.Origins)
                    .WithExposedHeaders(configuration.Cors.Headers)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            return services;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalHub.API.Dtos.Configuration;
using PersonalHub.API.Helpers;
using PersonalHub.API.Services;
using PersonalHub.Application.Dtos.Security;
using PersonalHub.Application.Services;

namespace PersonalHub.API.EndPoints.Security
{
    public static class LoginEndpoint
    {
        private const string ENDPOINT_NAME = "Iniciar sesión";
        public static RouteGroupBuilder MapIniciarSesion(this RouteGroupBuilder endpointGroup)
        {
            endpointGroup.MapPost("login", [AllowAnonymous]
            async (
                    JwtConfiguration jwtConfiguration,
                    HttpContext httpContext,
                    SecurityService securityService,
                    TokenService tokenService,
                    SessionService sessionService,
                    [FromBody] LoginRequest request,
                    CancellationToken ct
                ) =>
            {
                //var usuario = await securityService.ConsultarUsuario(request.User, request.Password);
                //var session = await sessionService.CreateSession(usuario.Id.ToString(), request.Address ?? string.Empty, ct);

                //var accessToken = tokenService.CreateAccessToken(new TokenValues() { Id = usuario.Id });
                //var response = new IniciarSesionResponse()
                //{
                //    AccessToken = accessToken,
                //    AccessExpiration = TokenService.GetTimeStampExpiration(accessToken),
                //    RefreshToken = session.RefreshToken,
                //    RefreshExpiration = TokenService.GetTimeStampExpiration(session.RefreshToken)
                //};

                //tokenService.SetTokenInCookie(httpContext, response, request.Remember);

                return Results.Ok();
            }
            )
            .HasApiVersion(ApiConstants.VERSION_1)
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .WithDescription("Inicia sesión en el sistema y devuelve un token de acceso y un token de actualización.")
            .WithSummary(ENDPOINT_NAME)
            .WithName(ENDPOINT_NAME);
            return endpointGroup;
        }
    }
}

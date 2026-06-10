using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;

namespace PersonalHub.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("response has already started"))
            {
                _logger.LogDebug(ex, "Response has already started, ignoring exception: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                //await HandleExceptionAsync(context, ex);
            }
        }

        //private Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
            //context.Response.ContentType = "application/json";
            //var error = new ErrorObjectResponse();
            //IEnumerable<object> additionalData = [];
            //switch (exception)
            //{
            //    case TInvalidOperationException ex:
            //        _logger.LogWarning(ex, "Operación no permitida [Codigo: {Codigo}]: {Message}.", ex.Code, ex.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //        break;
            //    case TArgumentException ex:
            //        _logger.LogWarning(ex, "Error de argumento [Codigo: {Codigo}]: {Message}.", ex.Code, ex.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //        break;
            //    case TNotFoundException ex:
            //        _logger.LogWarning(ex, "No se encontró el recurso [Codigo: {Codigo}]: {Message}.", ex.Code, ex.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            //        break;
            //    case TUnauthorizedAccessException ex when ex.Code == TUnauthorizedAccessException.InsufficientPermissions().Code:
            //        _logger.LogWarning(ex, "Acceso no concedido [Codigo: {Codigo}]: {Message}.", ex.Code, ex.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            //        break;
            //    case TUnauthorizedAccessException ex:
            //        _logger.LogWarning(ex, "Acceso denegado [Codigo: {Codigo}]: {Message}.", ex.Code, ex.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //        break;
            //    case BadHttpRequestException ex:
            //        _logger.LogWarning(ex, "Error de argumento [Codigo: {Codigo}]: {Message}.", TArgumentException.INVALID_FORMAT, ex.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //        break;
            //    case TConflictException ce:
            //        _logger.LogWarning(ce, "Conflicto de datos [Codigo: {Codigo}]: {Message}.", ce.Code, ce.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            //        break;
            //    default:
            //        _logger.LogError(exception, "Error no controlado: {Message}.", exception.Message);
            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        break;
            //}
            //if (exception is TException customException)
            //{
            //    error.Code = customException.Code;
            //    additionalData = customException.Args?.Values.ToList() ?? [];
            //    var template = _localizer[customException.Code];
            //    string detail = template.Value;
            //    if (customException.Args != null)
            //    {
            //        foreach (var kv in customException.Args)
            //        {
            //            detail = detail.Replace($"{{{kv.Key}}}", kv.Value?.ToString());
            //        }
            //    }
            //    error.Message = detail;
            //}
            //else
            //{
            //    if (exception is BadHttpRequestException)
            //        error.Code = TArgumentException.InvalidFormat().Code;
            //    else
            //        error.Code = "ErrorInterno";
            //    var template = _localizer[error.Code];
            //    error.Message = template.Value;
            //}
            //error.Data = additionalData;
            //error.Message = $"WS: {error.Message}";
            //var response = new ExceptionResponse()
            //{
            //    StatusCode = context.Response.StatusCode,
            //    Error = error,
            //    IsError = true
            //};
            //return context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonSerializerOptions));
        //}
    }
}

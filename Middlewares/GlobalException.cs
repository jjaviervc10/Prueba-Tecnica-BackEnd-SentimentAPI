using System.Net;
using System.Text.Json;

namespace SentimentAPI.Middlewares;

public class GlobalException
{
    private readonly RequestDelegate _next;
    private readonly ILogger <GlobalException> _logger;

    public GlobalException(
        RequestDelegate next,
        ILogger<GlobalException>logger
    )
    {
        _next = next;
        _logger = logger;
    }

 public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await  _next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Unhandled exception");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Error interno del servidor",
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
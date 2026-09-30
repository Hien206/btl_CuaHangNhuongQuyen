using System.Net;
using System.Text.Json;

namespace API;

public sealed class GlobalExceptionMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Lỗi chưa xử lý tại {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            if (context.Response.HasStarted)
            {
                throw;
            }

            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, safeDetail) = exception switch
        {
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "Resource Not Found",
                exception.Message),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                exception.Message),
            ArgumentException => (
                HttpStatusCode.BadRequest,
                "Bad Request",
                exception.Message),
            _ => (
                HttpStatusCode.InternalServerError,
                "Internal Server Error",
                _environment.IsDevelopment()
                    ? exception.ToString()
                    : "Đã có lỗi xảy ra phía máy chủ. Vui lòng thử lại sau.")
        };

        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Title = title,
            Detail = safeDetail,
            Instance = context.Request.Path.Value,
            TraceId = context.TraceIdentifier
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
}

namespace API
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;  
                await context.Response.WriteAsync("Bạn không có quyền truy cập!");
                return;
            }

            var apiKey = configuration["AppSettings:MyApiKey"];  
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 403; 
                await context.Response.WriteAsync("Key của bạn không đúng!");
                return;
            }

            await _next(context);
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
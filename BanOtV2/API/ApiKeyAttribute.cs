using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "X-API-KEY";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Bạn không có quyền truy cập!"
                };
                return;
            }

            var apiKey = configuration["AppSettings:MyApiKey"];  

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "Key của bạn không đúng!"
                };
                return;
            }

            await next();
        }
    }

}
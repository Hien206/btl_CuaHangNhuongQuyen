using System.IO.Compression;
using System.Text;
using System.Threading.RateLimiting;
using API;
using API.HealthChecks;
using API.OpenApi;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BLL;
using DAL;
using DAL.Helper;
using Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Compact;
using Swashbuckle.AspNetCore.SwaggerGen;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        new RenderedCompactJsonFormatter(),
        "Logs/app_log_.json",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true)
    .CreateBootstrapLogger();

try
{
    Log.Information("Đang khởi động Web API bán ô tô...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(
            new RenderedCompactJsonFormatter(),
            "Logs/app_log_.json",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            shared: true));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

    builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();
    builder.Services.AddTransient<IItemGroupRepository, ItemGroupRepository>();
    builder.Services.AddTransient<IItemGroupBusiness, ItemGroupBusiness>();
    builder.Services.AddTransient<IItemRepository, ItemRepository>();
    builder.Services.AddTransient<IItemBusiness, ItemBusiness>();
    builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
    builder.Services.AddTransient<ICustomerBusiness, CustomerBusiness>();
    builder.Services.AddTransient<IHoaDonRepository, HoaDonRepository>();
    builder.Services.AddTransient<IHoaDonBusiness, HoaDonBusiness>();
    builder.Services.AddTransient<IUserBusiness, UserBusiness>();
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<INewsBusiness, NewsBusiness>();
    builder.Services.AddTransient<INewsRepository, NewsRepository>();

    builder.Services.AddMemoryCache();

    var redisEnabled = builder.Configuration.GetValue<bool>("Redis:Enabled");
    if (redisEnabled)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection")
                ?? "localhost:6379";
            options.InstanceName = "BanOtoApi_";
        });
    }
    else
    {
        // Cho phép chạy bài tập khi máy chưa cài Redis; bật Redis:Enabled khi triển khai thật.
        builder.Services.AddDistributedMemoryCache();
    }

    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
        {
            "application/problem+json"
        });
    });
    builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        options.Level = CompressionLevel.Fastest);
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        options.Level = CompressionLevel.Fastest);

    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.OnRejected = async (context, cancellationToken) =>
        {
            context.HttpContext.Response.ContentType = "application/problem+json";
            await context.HttpContext.Response.WriteAsJsonAsync(new ErrorResponse
            {
                StatusCode = StatusCodes.Status429TooManyRequests,
                Title = "Too Many Requests",
                Detail = "Bạn đã gọi API quá số lần cho phép. Vui lòng thử lại sau.",
                Instance = context.HttpContext.Request.Path.Value,
                TraceId = context.HttpContext.TraceIdentifier
            }, cancellationToken);
        };
        options.AddPolicy("FixedWindowPolicy", httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 100,
                    QueueLimit = 2,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    Window = TimeSpan.FromMinutes(1)
                }));
    });

    var appSettingsSection = builder.Configuration.GetSection("AppSettings");
    builder.Services.Configure<AppSettings>(appSettingsSection);
    var appSettings = appSettingsSection.Get<AppSettings>()
        ?? throw new InvalidOperationException("Chưa cấu hình AppSettings.");
    var key = Encoding.UTF8.GetBytes(appSettings.Secret);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    }).AddMvc().AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHealthChecks()
        .AddCheck<SqlServerHealthCheck>("sql_server", tags: new[] { "ready" })
        .AddCheck<RedisHealthCheck>("redis_cache", tags: new[] { "ready" });

    var app = builder.Build();

    // Phải đứng đầu để bắt lỗi phát sinh từ toàn bộ middleware phía sau.
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseResponseCompression();
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} trả về {StatusCode} trong {Elapsed:0.0000} ms";
    });

    if (app.Environment.IsDevelopment())
    {
        var descriptions = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in descriptions.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"BanOto API {description.GroupName.ToUpperInvariant()}");
            }
        });
    }

    app.UseRouting();
    app.UseCors("AllowAll");
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecks("/healthz");
    app.MapControllers().RequireRateLimiting("FixedWindowPolicy");

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Ứng dụng bị dừng bất ngờ do lỗi!");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }

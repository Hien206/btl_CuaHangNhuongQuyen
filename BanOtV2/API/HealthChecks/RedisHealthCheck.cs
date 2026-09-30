using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace API.HealthChecks;

public sealed class RedisHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;

    public RedisHealthCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (!_configuration.GetValue<bool>("Redis:Enabled"))
        {
            return HealthCheckResult.Healthy("Redis đang tắt; ứng dụng dùng distributed memory cache.");
        }

        try
        {
            var connectionString = _configuration.GetConnectionString("RedisConnection")
                ?? "localhost:6379";
            using var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
            var latency = await connection.GetDatabase().PingAsync();
            return HealthCheckResult.Healthy(
                $"Kết nối Redis hoạt động bình thường ({latency.TotalMilliseconds:0.##} ms)." );
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("Không thể kết nối Redis.", exception);
        }
    }
}

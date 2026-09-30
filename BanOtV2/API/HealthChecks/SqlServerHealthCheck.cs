using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;

namespace API.HealthChecks;

public sealed class SqlServerHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;

    public SqlServerHealthCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return HealthCheckResult.Unhealthy("Chưa cấu hình kết nối SQL Server.");
            }

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy("Kết nối SQL Server hoạt động bình thường.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(
                "Không thể kết nối SQL Server.",
                exception);
        }
    }
}

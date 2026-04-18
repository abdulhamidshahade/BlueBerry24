using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Berryfy.Infrastructure.Data
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseHealthCheck> _logger;

        public DatabaseHealthCheck(ApplicationDbContext context, ILogger<DatabaseHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed: cannot connect");
                return HealthCheckResult.Unhealthy("Database is not accessible", ex);
            }

            try
            {
                var pending = (await _context.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
                if (pending.Count > 0)
                {
                    _logger.LogWarning(
                        "Database health check degraded: {Count} pending migration(s): {Migrations}",
                        pending.Count, string.Join(", ", pending));
                    return HealthCheckResult.Unhealthy(
                        $"{pending.Count} migration(s) still pending — schema is not ready");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed: cannot query migration state");
                return HealthCheckResult.Unhealthy("Cannot determine migration state", ex);
            }

            _logger.LogInformation("Database health check passed");
            return HealthCheckResult.Healthy("Database is accessible and fully migrated");
        }
    }
} 
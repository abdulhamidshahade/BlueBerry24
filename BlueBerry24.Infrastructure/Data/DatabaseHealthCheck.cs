using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace BlueBerry24.Infrastructure.Data
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
                
                _logger.LogInformation("Database health check passed");
                return HealthCheckResult.Healthy("Database is accessible");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed");
                return HealthCheckResult.Unhealthy("Database is not accessible", ex);
            }
        }
    }
} 
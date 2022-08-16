using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookingApp.Users.API.Utils
{
    public class SampleHealthCheck : IHealthCheck
    {
        private static readonly Random _rnd = new Random();

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isHealthy = true;

            //logic to determine if the healthCheck should be true
            isHealthy = _rnd.Next(5) == 0;

            if (isHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, "An unhealthy result."));
        }
    }
}

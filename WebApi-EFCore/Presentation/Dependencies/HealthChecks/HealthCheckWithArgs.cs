using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Presentation.Dependencies.HealthChecks
{
    /// <summary>
    /// 
    /// </summary>
    public class HealthCheckWithArgs : IHealthCheck
    {
        private readonly int _arg1;
        private readonly string _arg2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public HealthCheckWithArgs(int arg1, string arg2)
        => (_arg1, _arg2) = (arg1, arg2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // ....

            return Task.FromResult(HealthCheckResult.Healthy("A healthy result."));
        }
    }
}

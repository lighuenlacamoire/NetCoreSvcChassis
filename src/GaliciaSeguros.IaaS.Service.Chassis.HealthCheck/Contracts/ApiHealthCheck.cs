using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.HealthCheck.Contracts
{
    public class ApiHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {  
            var healthy = true;
            if (healthy)
                return Task.FromResult(HealthCheckResult.Healthy());
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}

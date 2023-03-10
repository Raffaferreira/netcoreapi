using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Controllers.Base;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/ratelimiting")]
    public class RateLimitingController : BaseController
    {
        private readonly IpRateLimitOptions _options;
        private readonly IIpPolicyStore _policyStore;

        public RateLimitingController(
            IOptions<IpRateLimitOptions> options,
            IIpPolicyStore policyStore)
        {
            _options = options.Value;
            _policyStore = policyStore;
        }

        [HttpGet]
        public async Task<IpRateLimitPolicies> GetIpRateLimitPolicies()
        {
            // Return IP Rate Limit Policies
            IpRateLimitPolicies? policies = await _policyStore.GetAsync(_options.IpPolicyPrefix);
            return policies;
        }

        [HttpPost]
        public async Task AddIpRateLimitPolicies()
        {
            // Get the policies
            IpRateLimitPolicies? policies = await _policyStore.GetAsync(_options.IpPolicyPrefix);

            if (policies != null)
            {
                // Add a new Ip Rule at runtime
                policies.IpRules.Add(new IpRateLimitPolicy
                {
                    Ip = "1.1.1.1",
                    Rules = new List<RateLimitRule>(new RateLimitRule[]
                    {
                        new RateLimitRule
                        {
                            Endpoint = "*:/api/update",
                            Limit = 10,
                            Period = "1d"
                        }
                    })
                });

                // Set the new policy
                await _policyStore.SetAsync(_options.IpPolicyPrefix, policies);
            }
        }
    }
}

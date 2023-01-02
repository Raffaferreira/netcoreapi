using AspNetCoreRateLimit;

namespace Presentation.Dependencies.Startup
{
    public static class RegisterRateLimiting
    {
        public static void AddRegisterRateLimiting(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

            builder.Services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddSingleton<IRateLimitConfiguration, CustomRateLimitConfiguration>();

            builder.Services.AddInMemoryRateLimiting();
        }
    }
}

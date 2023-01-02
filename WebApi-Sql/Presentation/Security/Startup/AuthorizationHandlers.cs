
using Microsoft.AspNetCore.Authorization;
using Presentation.Security.Handlers;
using Presentation.Security.Middleware;

namespace Presentation.Security.Startup
{
    public static class AuthorizationHandlers
    {
        public static void AddAuthorizationHandlers(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IAuthorizationHandler, IsAccountEnableHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, IsVIPCustomerHandler>();
            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();
        }
    }
}

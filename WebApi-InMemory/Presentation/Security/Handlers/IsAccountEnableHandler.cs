using Microsoft.AspNetCore.Authorization;

namespace Presentation.Security.Handlers
{
    public class IsAccountEnableHandler : AuthorizationHandler<IsAccountEnabledRequirement>
    {
        private readonly ILogger _logger;

        public IsAccountEnableHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().FullName!);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAccountEnabledRequirement requirement)
        {
            if (requirement.AllowUsers.Any(user => user.Equals(context.User.Identity!.Name, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            if (context.User.HasClaim(f => f.Type == "Disabled"))
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

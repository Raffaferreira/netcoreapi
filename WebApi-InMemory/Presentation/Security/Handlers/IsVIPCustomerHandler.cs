using Microsoft.AspNetCore.Authorization;
using Presentation.Security.Requirements;

namespace Presentation.Security.Handlers
{
    public class IsVIPCustomerHandler : AuthorizationHandler<IsAllowedToManageProductRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAllowedToManageProductRequirement requirement)
        {
            if (context.User.HasClaim(f => f.Type == "VIP"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

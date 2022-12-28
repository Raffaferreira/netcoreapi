using Microsoft.AspNetCore.Authorization;

namespace Presentation.Security.Handlers
{
    public class IsAllowedToManageProductRequirement : IAuthorizationRequirement
    {
    }
}
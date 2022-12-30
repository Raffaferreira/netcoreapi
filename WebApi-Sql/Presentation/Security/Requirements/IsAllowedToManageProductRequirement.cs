using Microsoft.AspNetCore.Authorization;

namespace Presentation.Security.Requirements
{
    public class IsAllowedToManageProductRequirement : IAuthorizationRequirement
    {
    }
}
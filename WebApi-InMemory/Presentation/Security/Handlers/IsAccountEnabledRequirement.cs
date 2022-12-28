using Microsoft.AspNetCore.Authorization;

namespace Presentation.Security.Handlers
{
    public class IsAccountEnabledRequirement : IAuthorizationRequirement
    {
        public string[] AllowUsers { get; set; }

        public IsAccountEnabledRequirement(params string[] users)
        {
            AllowUsers = users;
        }
    }
}

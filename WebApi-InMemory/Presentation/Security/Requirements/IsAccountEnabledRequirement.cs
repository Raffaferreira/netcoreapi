﻿using Microsoft.AspNetCore.Authorization;

namespace Presentation.Security.Requirements
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

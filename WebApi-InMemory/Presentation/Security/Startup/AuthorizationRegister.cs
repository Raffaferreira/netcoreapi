using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presentation.Security.Requirements;
using System.Security.Claims;
using System.Text;

namespace Presentation.Security.Startup
{
    public static class AuthorizationRegister
    {
        public static void AddAuthorizationAndAuthenticationConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.User));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.Admin));
                options.AddPolicy("ManagerOnly", policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.Manager));
                options.AddPolicy("SysAdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.SysAdmin));
                options.AddPolicy("Founders", policy => policy.RequireClaim("EmployeeNumber", "1", "2", "3", "4", "5"));
                options.AddPolicy("EnableToAll", policy =>
                {
                    policy.RequireRole(UserRoles.Admin, UserRoles.SysAdmin, UserRoles.Manager, UserRoles.User);
                });
                options.AddPolicy("EnableToAllClaims", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, UserRoles.SysAdmin);
                    //policy.RequireClaim(ClaimTypes.Role, UserRoles.Admin);
                    //policy.RequireClaim(ClaimTypes.Role, UserRoles.Manager);
                    //policy.RequireClaim(ClaimTypes.Role, UserRoles.User);
                });
                options.AddPolicy("canManageProduct", policyBuilder => policyBuilder.AddRequirements(new IsAccountEnabledRequirement()));
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["ApplicationSetup:TokenConfigurations:Issuer"],
                    ValidAudience = builder.Configuration["ApplicationSetup:TokenConfigurations:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApplicationSetup:TokenConfigurations:SecretJWTKey"])),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                builder.Configuration.Bind("CookieSettings", options);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ReturnUrlParameter = "ReturnUrl";
            });
        }
    }
}

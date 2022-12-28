﻿using Domain.Models;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation;
using Presentation.Dependencies;
using System.Reflection;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using Presentation.Security;
using Presentation.Security.Handlers;
using Presentation.Security.Middleware;
using Presentation.Security.Requirements;

namespace WebApi.Dependencies.Startup
{
    /// <summary>
    /// 
    /// </summary>
    public static class StartupBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigurationStartupBuilder(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
                p.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                     new HeaderApiVersionReader("x-api-version"),
                                     new MediaTypeApiVersionReader("x-api-version"));
            });

            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.SwaggerDocumentation();

            //builder.Configuration.GetConnectionString("SqliteConnectionString");

            builder.Services.ConfigureOptions<ApplicationOptionsConfiguration>();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
            builder.Services.Configure<TopItemSettings>(TopItemSettings.Month, builder.Configuration.GetSection("TopItem:Month"));
            builder.Services.Configure<TopItemSettings>(TopItemSettings.Year, builder.Configuration.GetSection("TopItem:Year"));
            //builder.Services.Configure<ApplicationSetup>(builder.Configuration.GetSection(nameof(ApplicationSetup)));


            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
            builder.Services.AddDbContext<WebApiDbContext>(options => options.UseInMemoryDatabase(databaseName: "WebApi"));


            builder.Services.AddHealthChecks();
            builder.Services.AddCors();
            builder.Services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.Services.AddSingleton<IAuthorizationHandler, IsAccountEnableHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, IsVIPCustomerHandler>();
            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();
            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();


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

        /// <summary>
        /// Swagger Configuration of rendering detailed information about API
        /// </summary>
        /// <param name="builder"></param>
        private static void SwaggerDocumentation(this WebApplicationBuilder builder)
        {
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo
                //{
                //Version = "v1",
                //Title = "ToDo API",
                //Description = "An ASP.NET Core Web API for managing ToDo items",
                //TermsOfService = new Uri("https://example.com/terms"),
                //Contact = new OpenApiContact
                //{
                //    Name = "Example Contact",
                //    Url = new Uri("https://example.com/contact")
                //},
                //License = new OpenApiLicense
                //{
                //    Name = "Example License",
                //    Url = new Uri("https://example.com/license")
                //}
                //});

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
                        "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
                        "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>
                (builder.Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);

            //builder.Services.AddJwtSecurity(tokenConfigurations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static string ConnectionStringSqlite(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("SqliteConnectionString") ?? "DataSource=WebApi.db";
            builder.Services.AddSqlite<WebApiDbContext>(connectionString);

            return connectionString;
        }

        /// <summary>
        /// 
        /// </summary>        /// <param name="context"></param>
        /// <param name="healthReport"></param>
        /// <returns></returns>
        public static Task WriteResponse(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions { Indented = true };

            using var memoryStream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("status", healthReport.Status.ToString());
                jsonWriter.WriteStartObject("results");

                foreach (var healthReportEntry in healthReport.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key);
                    jsonWriter.WriteString("status",
                        healthReportEntry.Value.Status.ToString());
                    jsonWriter.WriteString("description",
                        healthReportEntry.Value.Description);
                    jsonWriter.WriteStartObject("data");

                    foreach (var item in healthReportEntry.Value.Data)
                    {
                        jsonWriter.WritePropertyName(item.Key);

                        JsonSerializer.Serialize(jsonWriter, item.Value,
                            item.Value?.GetType() ?? typeof(object));
                    }

                    jsonWriter.WriteEndObject();
                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            return context.Response.WriteAsync(
                Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
    }
}

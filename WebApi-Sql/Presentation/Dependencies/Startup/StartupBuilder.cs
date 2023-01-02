using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Presentation.Dependencies;
using Presentation.Dependencies.Startup;
using Presentation.Security.Startup;
using System.Reflection;
using System.Text;
using System.Text.Json;

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

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
            builder.Services.AddDbContext<WebApiDbContext>(options => options.UseInMemoryDatabase(databaseName: "WebApi"));

            builder.Services.AddHealthChecks();
            builder.Services.AddCors();
            builder.Services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.AddAuthorizationHandlers();
            builder.AddAuthorizationAndAuthenticationConfiguration();
            builder.AddRegisterRateLimiting();
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

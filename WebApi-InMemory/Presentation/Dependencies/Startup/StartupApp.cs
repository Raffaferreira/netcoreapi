using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.Dependencies.Startup
{
    /// <summary>
    /// A web application configuration, any doubt check the following link: 
    /// https://code-maze.com/aspnetcore-api-versioning/
    /// https://dotnetthoughts.net/aspnetcore-api-versioning-with-net-6-minimal-apis/
    /// https://blog.christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/
    /// </summary>
    public static class StartupApp
    {
        /// <summary>
        /// A method with webapplication class called on startup 
        /// </summary>
        /// <param name="app"></param>
        public static void StartupConfigurationApp(this WebApplication app)
        {
            app.UseIpRateLimiting();

            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            if (app.Environment.IsDevelopment())
            {
                //app.MapSwagger();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    //options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    //options.RoutePrefix = string.Empty;
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });

                app.UseExceptionHandler("/error-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }            

            app.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },
                AllowCachingResponses = false,
                ResponseWriter = StartupBuilder.WriteResponse
            });


            app.UseStaticFiles();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHealthChecks("/monitor");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapControllers();
            app.Run();
        }
    }
}

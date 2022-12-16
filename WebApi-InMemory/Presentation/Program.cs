using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Presentation.Dependencies.Startup;
using WebApi.Dependencies.Startup;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigurationStartupBuilder();
            builder.AddRegisterRepositories();
            builder.AddRegisterServices();

            string ConnectionString = builder.ConnectionStringSqlite();

            var app = builder.Build();
            app.RegisterMinimalApis();
            app.StartupConfigurationApp();

            async Task CheckExistingDatabaseSQLite(IServiceProvider services, ILogger logger)
            {
                logger.LogInformation("Garantindo que o banco de dados exista e esteja na string de conexão :'{connectionString}'", ConnectionString);
                using var db = services.CreateScope().ServiceProvider.GetRequiredService<WebApiDbContext>();
                await db.Database.EnsureCreatedAsync();
                await db.Database.MigrateAsync();
            }
        }
    }
}
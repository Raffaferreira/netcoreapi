using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Presentation;

namespace WebApi.InMemory.IntegrationTest.Integration
{
    public class CustomWebApiApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : Program, new()
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public IConfiguration? Configuration { get; private set; }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                Configuration = _configuration;
                config.AddConfiguration(_configuration);
            });

            builder.ConfigureServices(services =>
            {
                var root = new InMemoryDatabaseRoot();

                // Remove the app's ApplicationDbContext registration.
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WebApiDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor!);
                }

                //var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                //services.Remove(dbConnectionDescriptor!);

                services.RemoveAll(typeof(DbContextOptions<WebApiDbContext>));
                services.AddDbContext<WebApiDbContext>(options => options.UseInMemoryDatabase("WebApi", root));

                #region commented
                //services.AddScoped(sp =>
                //{
                //    return new DbContextOptionsBuilder<WebApiDbContext>()
                //    .UseInMemoryDatabase("WebApi", root)
                //    .UseApplicationServiceProvider(sp)
                //    .Options;
                //});

                //services.AddDbContext<WebApiDbContext>((container, options) =>
                //{
                //    var connection = container.GetRequiredService<DbConnection>();
                //    options.UseSqlite(connection);
                //});

                //services.AddSingleton<DbConnection>(container =>
                //{
                //    var connection = new SqliteConnection("DataSource=:memory:");
                //    connection.Open();

                //    return connection;
                //});

                //services.AddEntityFrameworkInMemoryDatabase().AddDbContext<WebApiDbContext>((sp, options) =>
                //{
                //    options.UseInMemoryDatabase("WebApi").UseInternalServiceProvider(sp);
                //});
                #endregion
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<ICreditRepository, CreditRepository>();
                services.AddTransient<IDebitRepository, DebitRepository>();
                services.AddAuthentication("IntegrationTest")
                .AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthenticationHandler>("IntegrationTest", options =>
                {

                });
            });

            builder.UseEnvironment("Development");
        }
    }
}

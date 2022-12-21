using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Presentation;
using System.Data.Common;
using TestingXUnit.WebApi.Security;

namespace TestingXUnit
{
    public class CustomWebApiApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : Program, new()
    {
        public IConfiguration? Configuration { get; private set; }

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
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WebApiDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor!);
                }

                services.RemoveAll(typeof(DbContextOptions<WebApiDbContext>));

                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                services.Remove(dbConnectionDescriptor!);


                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<WebApiDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

                //services.AddEntityFrameworkInMemoryDatabase()
                //.AddDbContext<WebApiDbContext>((sp, options) =>
                //{
                //    options.UseInMemoryDatabase("DataSource=:memory:");
                //    options.UseInternalServiceProvider(services.BuildServiceProvider());
                //});
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<ICreditRepository, CreditRepository>();
                services.AddTransient<IDebitRepository, DebitRepository>();
                services.AddAuthentication("IntegrationTest")
                .AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthenticationHandler>
                ("IntegrationTest", options =>
                {

                });
            });

            builder.UseEnvironment("Development");
        }
    }
}

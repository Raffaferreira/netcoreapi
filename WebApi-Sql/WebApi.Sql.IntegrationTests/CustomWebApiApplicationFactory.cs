using Domain.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        protected readonly string _ConnectionStringSqlDatabase = "Data Source=DESKTOP-VP7F5C3\\SQLEXPRESS;Initial Catalog=WebApiTesting;Integrated Security=True";

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

                //It works
                //var root = new InMemoryDatabaseRoot();
                //services.AddScoped(sp =>
                //{
                //    // Replace SQLite with the in memory provider for tests
                //    return new DbContextOptionsBuilder<WebApiDbContext>()
                //                .UseInMemoryDatabase("Tests", root)
                //                .UseApplicationServiceProvider(sp)
                //                .Options;
                //});

                //It works
                //services.AddEntityFrameworkInMemoryDatabase()
                //.AddDbContext<WebApiDbContext>((sp, options) =>
                //{
                //    options.UseInMemoryDatabase("DataSource=:memory:");
                //    options.UseInternalServiceProvider(services.BuildServiceProvider());
                //});

                //It works
                services.AddEntityFrameworkSqlServer()
                .AddDbContext<WebApiDbContext>((sp, options) =>
                {
                    options
                    .UseSqlServer(_ConnectionStringSqlDatabase);
                });

                //It works
                //services.AddScoped(sp =>
                //{
                //    return new DbContextOptionsBuilder<WebApiDbContext>()
                //    .UseSqlServer("Data Source=DESKTOP-VP7F5C3\\SQLEXPRESS;Initial Catalog=WebApi;Integrated Security=True")
                //    .Options;
                //});

                //error
                //services.AddDbContext<WebApiDbContext>((sp, options) =>
                //{
                //    options.UseSqlServer("Data Source=DESKTOP-VP7F5C3\\SQLEXPRESS;Initial Catalog=WebApi;Integrated Security=True");
                //    //options.UseInternalServiceProvider(services.BuildServiceProvider());
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

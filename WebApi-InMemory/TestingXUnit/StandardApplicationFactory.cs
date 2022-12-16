using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace TestingXUnit
{
    public class StandardApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
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
                var root = new InMemoryDatabaseRoot();

                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WebApiDbContext>));

                services.Remove(dbContextDescriptor!);

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
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<ICreditRepository, CreditRepository>();
                services.AddTransient<IDebitRepository, DebitRepository>();
            });

            builder.UseEnvironment("Development");
        }
    }
}

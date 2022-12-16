using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Infrastructure.Context
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext()
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextOptions"></param>
        public WebApiDbContext(DbContextOptions<WebApiDbContext> contextOptions) : base(contextOptions)
        {}

        /// <summary>
        /// Instead of using InMemmory EFCore to prototype your database, you can uncomment this section 
        /// and use SQLite database implemented
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=WebApi.db;Cache=Shared");
            }
        }

        public DbSet<Account> Account => Set<Account>();
        public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<Debito> Debito => Set<Debito>();
        public DbSet<Credito> Credito => Set<Credito>();
        public DbSet<Transactions> Transactions => Set<Transactions>();
    }
}

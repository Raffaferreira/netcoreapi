using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext()
        { }

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
        : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-VP7F5C3\\SQLEXPRESS;Initial Catalog=WebApiTesting;Integrated Security=True");
            }
        }

        public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<Debito> Debito => Set<Debito>();
        public DbSet<Credito> Credito => Set<Credito>();
        public DbSet<Account> Account => Set<Account>();
        public DbSet<Transactions> Transactions => Set<Transactions>();
    }
}

﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Infrastructure.Context
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext()
        {}

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
        : base(options)
        {}

        public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<Debito> Debito => Set<Debito>();
        public DbSet<Credito> Credito => Set<Credito>();
        public DbSet<Account> Account => Set<Account>();
        public DbSet<Transactions> Transactions => Set<Transactions>();
    }
}

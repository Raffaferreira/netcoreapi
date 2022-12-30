using Domain.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WebApiDbContextRepository : IWebApiDbContextRepository
    {
        private readonly WebApiDbContext _db;
        public WebApiDbContextRepository(WebApiDbContext db)
        {
            _db = db;
        }

        public IQueryable<Customer> Customer => _db.Customer;
        public IQueryable<Debito> Debito => _db.Debito;
        public IQueryable<Credito> Credito => _db.Credito;
        public IQueryable<Transactions> Transactions => _db.Transactions;

        public void Add<EntityType>(EntityType entityType)
        {
            _db.Add(entityType!);
        }

        public void Delete<EntityType>(EntityType entity)
        {
            _db.Remove(entity!);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}

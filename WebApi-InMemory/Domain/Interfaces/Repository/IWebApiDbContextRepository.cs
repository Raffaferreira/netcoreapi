using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IWebApiDbContextRepository
    {
        public IQueryable<Customer> Customer { get; }
        public IQueryable<Debito> Debito { get; }
        public IQueryable<Credito> Credito { get; }
        public IQueryable<Transactions> Transactions { get; }
        void Add<EntityType>(EntityType entityType);
        void SaveChanges();
        void Delete<EntityType>(EntityType entity);

    }
}

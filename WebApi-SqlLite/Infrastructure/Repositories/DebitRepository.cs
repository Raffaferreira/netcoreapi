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
    public class DebitRepository : IDebitRepository
    {
        public DebitRepository()
        {
            using (var context = new WebApiDbContext())
            {
                var transactions = new List<Debito>
                {
                   new Debito()
                   {
                       Id = Guid.NewGuid(),
                       AccountTobeWithdraw = 0011,
                       Value = 150.00M
                   }
                };

                context.Debito.AddRange(transactions);
                context.SaveChanges();
            }
        }
    }
}

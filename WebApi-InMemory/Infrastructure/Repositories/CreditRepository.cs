using Domain.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class CreditRepository : ICreditRepository
    {
        public CreditRepository()
        {
            using (var context = new WebApiDbContext())
            {
                var transactions = new List<Transactions>
                {
                   new Transactions()
                   {
                       Id = Guid.NewGuid(),
                       Credited = 2000.00M,
                       Debited = 550.00M,
                       Balance = 5150.00M,
                       AccountNumber = 0011,
                       TransactionDate = new DateTimeOffset()
                   }
                };

                context.Transactions.AddRange(transactions);
                context.SaveChanges();
            }
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return null;
        }
    }
}

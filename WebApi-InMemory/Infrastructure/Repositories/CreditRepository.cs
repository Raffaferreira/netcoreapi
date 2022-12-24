using Domain.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class CreditRepository : ICreditRepository
    {
        private readonly WebApiDbContext _db;

        public CreditRepository(WebApiDbContext db)
        {
            _db = db;

            using var context = new WebApiDbContext();
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

        public async Task<Customer> AddCustomer(Customer customer)
        {
            var result = _db.Customer.Add(customer);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteCustomer(Guid Id)
        {
            var filteredData = _db.Customer.Where(x => x.Id == Id).FirstOrDefault();
            var result = _db.Remove<Customer>(filteredData!);
            await _db.SaveChangesAsync();
            return result != null ? true : false;
        }

        public async Task<Customer> GetCustomerById(Guid id)
        {
            return await Task.FromResult(_db.Customer.Where(x => x.Id == id).First());
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await Task.FromResult(_db.Customer.ToList());
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var result = _db.Customer.Update(customer);
            await _db.SaveChangesAsync();
            return result.Entity;
        }
    }
}

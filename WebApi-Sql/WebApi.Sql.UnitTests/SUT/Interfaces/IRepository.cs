using System.Linq.Expressions;
using WebApi.Sql.UnitTests.SUT.Models;

namespace WebApi.Sql.UnitTests.SUT.Interfaces
{
    public interface IRepository
    {
        event EventHandler FailedDatabaseRequest;
        int TenantId { get; set; }
        Customer CurrentCustomer { get; set; }
        int CountCostumers();
        int SumUpNumbers(int number1, int number2);
        Customer Find(int id);
        IList<Customer> GetSomeRecords(Expression<Func<Customer, bool>> where);
        void AddRecord(Customer customer);
        Task<int> GetCountAsync();
        Customer Get(int id, string name);
        Customer FindCustomer(int id);
    }
}

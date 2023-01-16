using System.Linq.Expressions;
using WebApi.Sql.UnitTests.SUT.Interfaces;
using WebApi.Sql.UnitTests.SUT.Models;

namespace WebApi.Sql.UnitTests.SUT.Fakes
{
    public class FakeCreditRepository : IRepository
    {
        protected virtual int GetNumber() => 5;

        protected virtual int GetNumberWithParam(int param) => 5;

        public int CallProtectedMember() => GetNumber();

        public int CallProtectedMemberWithParam(int param) => GetNumberWithParam(param);

        public Customer FindCustomer(int id)
        {
            return new Customer { Id = id, Name = "Fred Flintstone" };
        }

        public Customer Find(int id) => new Customer { Id = id, Name = "Fred Flintstone" };

        public int TenantId { get; set; }
        public Customer CurrentCustomer { get; set; }

        public event EventHandler? FailedDatabaseRequest;
        public void AddRecord(Customer customer) => throw new NotImplementedException();
        public Customer Get(int id, string name) => throw new NotImplementedException();
        public Task<int> GetCountAsync() => throw new NotImplementedException();
        public IList<Customer> GetSomeRecords(Expression<Func<Customer, bool>> where) => throw new NotImplementedException();
    }
}

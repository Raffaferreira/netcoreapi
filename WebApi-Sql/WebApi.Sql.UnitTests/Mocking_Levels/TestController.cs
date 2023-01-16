using WebApi.Sql.UnitTests.SUT.Interfaces;
using WebApi.Sql.UnitTests.SUT.Models;

namespace WebApi.Sql.UnitTests.Mocking_Levels
{
    public class TestController
    {
        private readonly IRepository _repo;
        private readonly ILogger _logger;

        public TestController(IRepository repo, ILogger logger = null)
        {
            _repo = repo;
            _logger = logger;
            _repo.FailedDatabaseRequest += Repo_FailedDatabaseRequest;
        }

        private void Repo_FailedDatabaseRequest(object sender, EventArgs e)
        {
            _logger.Error("An error occurred");
        }

        public int TenantId() => _repo.TenantId;
        public void SetTenantId(int id) => _repo.TenantId = id;

        public Customer GetCustomer(int id, string name) => _repo.Get(12, "Fred");
        public Customer GetCurrentCustomer => _repo.CurrentCustomer;
        public Customer GetCustomer(int id)
        {
            try
            {
                id++;
                //_repo.AddRecord(new Customer());
                //var c = _repo.Find(id);
                //return new Customer { Id = 12, Name = "Fred Flintstone" };
                return _repo.Find(id);
            }
            catch (Exception ex)
            {
                if (_logger is not null)
                {
                    _logger.Debug("There was an exception");
                }
                throw;
            }
        }

        public int ReturnCustomersAmount()
        {
            return _repo.CountCostumers();
        }

        public int SumUp(int number1, int number2)
        {
            return _repo.SumUpNumbers(number1, number2);
        }

        public Address GetCustomersAddress(int id) => _repo.Find(id).AddressNavigation;

        public async Task<int> GetCustomerCountAsync() => await _repo.GetCountAsync();

        public void SaveCustomer(Customer customer)
        {
            _repo.AddRecord(customer);
        }
    }
}

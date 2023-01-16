using WebApi.Sql.UnitTests.SUT.Interfaces;
using WebApi.Sql.UnitTests.SUT.Models;

namespace WebApi.Sql.UnitTests.SUT.Repositories
{
    public class CreditRepository
    {
        private readonly IRepository _creditRepository;

        public CreditRepository(IRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }

        public Customer GetCustomer(int IdCustomer)
        {
            return _creditRepository.FindCustomer(IdCustomer);
        }
    }
}

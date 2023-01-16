using WebApi.Sql.UnitTests.SUT.Fakes;
using WebApi.Sql.UnitTests.SUT.Repositories;

namespace WebApi.Sql.UnitTests
{
    public class UseCasesFake
    {
        private CreditRepository _sut = new CreditRepository(new FakeCreditRepository());

        [Theory]
        [InlineData(1, "Fred")]
        [InlineData(2, "Karol")]
        [InlineData(3, "Heloisa")]
        public void ShouldGetACustomer(int id, string name)
        {
            Assert.Equal(name, _sut.GetCustomer(id).Name);
            switch (_sut.GetCustomer(id).Name)
            {
                case "Fred":
                    break;
            }
        }
    }
}

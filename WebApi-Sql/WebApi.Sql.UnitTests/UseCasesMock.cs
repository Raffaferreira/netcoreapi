using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Sql.UnitTests.SUT.Fakes;
using WebApi.Sql.UnitTests.SUT.Interfaces;
using WebApi.Sql.UnitTests.SUT.Models;
using WebApi.Sql.UnitTests.SUT.Repositories;

namespace WebApi.Sql.UnitTests
{
    public class UseCasesMock
    {
        //private CreditRepository _sut = new CreditRepository(new Mock<IRepository>().Object);
        private Mock<IRepository> _mockRepo = new Mock<IRepository>();
        private CreditRepository _sut;

        public UseCasesMock()
        {
            _sut = new CreditRepository(_mockRepo.Object);
        }

        [Theory]
        [InlineData(1, "Fred")]
        [InlineData(2, "Karol")]
        [InlineData(3, "Heloisa")]
        public void ShouldGetACustomer(int id, string name)
        {
            _mockRepo.Setup(x => x.FindCustomer(id)).Returns(new Customer { Id = id, Name = name });
            Assert.Equal(name, _sut.GetCustomer(id).Name);
        }

        [Theory]
        [InlineData(1, "Fred")]
        [InlineData(2, "Karol")]
        [InlineData(3, "Heloisa")]
        public void ShouldGetACustomerAndFailSecond(int id, string name)
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

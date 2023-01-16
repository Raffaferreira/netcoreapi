using Moq;
using WebApi.Sql.UnitTests.Mocking_Levels;
using WebApi.Sql.UnitTests.SUT.Interfaces;
using WebApi.Sql.UnitTests.SUT.Models;

namespace WebApi.Sql.UnitTests
{
    public class TestBusinessRulesUsingMock
    {
        private Mock<IRepository> _mockRepo = new Mock<IRepository>(MockBehavior.Strict);
        private Mock<IErrorHandler> _mockErrorHandler = new Mock<IErrorHandler>(MockBehavior.Strict);
        private BetterExample _sut;

        public TestBusinessRulesUsingMock()
        {
            _sut = new BetterExample(_mockRepo.Object, _mockErrorHandler.Object);
        }

        [Theory]
        [InlineData(1, "Fred", CharacterStatusEnum.Father)]
        [InlineData(2, "Wilma", CharacterStatusEnum.Mother)]
        [InlineData(3, "Pebbles", CharacterStatusEnum.Child)]
        public void ShouldGetACustomer(int id, string name, CharacterStatusEnum status)
        {
            _mockRepo.Setup(x => x.Find(id)).Returns(new Customer { Id = id, Name = name });

            _mockErrorHandler.Setup(x => x.HandleIt()).Verifiable(); //Verifying that it didn't get called at all, because it doesn't has any error handler condition 

            Assert.Equal(status, _sut.GetCustomerStatus(id));
            _mockErrorHandler.Verify(x => x.HandleIt(), Times.Never);
        }

        [Theory]
        [InlineData(1, "Grand Poobah", CharacterStatusEnum.Child)]
        public void ShouldFailToGetACustomer(int id, string name, CharacterStatusEnum status)
        {
            var ex1 = new Exception("Unable to locate");

            _mockRepo.Setup(x => x.Find(id)).Throws(ex1);
            _mockErrorHandler.Setup(x => x.HandleIt());

            var ex2 = Assert.Throws<Exception>(() => _sut.GetCustomerStatus(id));

            Assert.Equal(ex1.Message, ex2.Message);
            _mockErrorHandler.Verify(x => x.HandleIt(), Times.Once);
        }

        [Theory]
        [InlineData(1, "Grand Poobah", CharacterStatusEnum.Child)]
        public void ShouldFailToGetACustomerDuplicatingErrorHandling(int id, string name, CharacterStatusEnum status)
        {
            var ex1 = new Exception("Unable to locate");

            _mockRepo.Setup(x => x.Find(id)).Throws(ex1);
            _mockErrorHandler.Setup(x => x.HandleIt());

            var ex2 = Assert.Throws<Exception>(() => _sut.GetCustomerStatusDuplicatedErrorHandler(id));

            Assert.Equal(ex1.Message, ex2.Message);
            _mockErrorHandler.Verify(x => x.HandleIt(), Times.Once);
        }
    }
}

using Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Presentation;

namespace TestingXUnit.WebApi.UseCases
{
    [Trait("Credit Api - Integration", "Integration")]
    public class CreditTest : IntegrationTesting
    {
        public CreditTest(CustomWebApiApplicationFactory<Program> factory):base(factory)
        {}

        [Theory]
        [InlineData("/credit")]
        public async void Get_AllTransactions_ReturnSuccess_Content(string url)
        {
            var httpClient = _factory.CreateClient();

            var responseMessage = await httpClient.GetAsync(url);

            var result = await responseMessage.Content.ReadAsStringAsync();

            var responseConverted = JsonConvert.DeserializeObject<Credito>(result);

            Assert.Equal("Hello World!", result);
        }

        [Theory]
        [InlineData("/credit/{1}")]
        public async void Get_TransactionByAccountNumber_ReturnSuccessMessage(string url)
        {
            //Arrange
            var transaction = new Transactions()
            {
                Id = Guid.NewGuid(),
                AccountNumber = 0011,
                Balance = 5000.00M,
                Credited = 1000.00M,
                Debited = 500.00M,
                TransactionDate = new DateTimeOffset()
            };

            var httpClient = _factory.CreateClient();

            //Act
            var responseMessage = await httpClient.GetAsync(url);
            var result = await responseMessage.Content.ReadAsStringAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Transactions>(result);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal("\"transaction.ToString()\"", result);
            Assert.Equal(transaction, JsonConvert.DeserializeObject<Transactions>(result));
        }

        [Theory]
        [InlineData("/credit")]
        public async void Post_NewTransaction_ReturnSuccessMessage(string url)
        {
            var httpClient = _factory.CreateClient();

            var responseMessage = await httpClient.PostAsJsonAsync(url, new Transactions
            {
                Id = Guid.NewGuid(),
                AccountNumber = 0011,
                Balance = 5000.00M,
                Credited = 1000.00M,
                Debited = 500.00M,
                TransactionDate = new DateTimeOffset()
            });

            var result = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal($"AccountNumber {0011} created", result);
        }
    }
}

using Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Presentation;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.InMemory.IntegrationTest.Integration.WebApi.UseCases
{
    [Trait("Category", "Integration")]
    public class TransactionsTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public TransactionsTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("/transactions")]
        public async void Get_AllTransactions_ReturnSuccess_Content(string url)
        {
            var httpClient = _factory.CreateClient();

            var responseMessage = await httpClient.GetAsync(url);

            var result = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal("Hello World!", result);
        }

        [Theory]
        [InlineData("/transactions/{1}")]
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
        [InlineData("/transactions")]
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
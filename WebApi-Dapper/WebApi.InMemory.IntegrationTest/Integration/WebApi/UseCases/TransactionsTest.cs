using Domain.Models;
using FluentAssertions;
using Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Presentation;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.InMemory.IntegrationTest.Integration.WebApi.UseCases
{
    public class TransactionsTest : IntegrationTesting
    {
        public TransactionsTest(CustomWebApiApplicationFactory<Program> factory) : base(factory)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
                {
                    webApiDbContext.Database.EnsureCreatedAsync();

                    webApiDbContext.Transactions.AddAsync(new Domain.Models.Transactions 
                    { 
                        Id = Guid.Parse("48f13781-7213-4ddf-b52a-35987398208f"), 
                        AccountNumber = 0011,
                        Balance = 10500.00M,
                        Credited = 10M,
                        Debited = 10,
                        TransactionDate = new DateTimeOffset()
                    });
                    webApiDbContext.SaveChangesAsync();
                }
            }
        }

        [Fact]
        [Trait("Category", "GET")]
        public async void Get_AllTransactions_ReturnSuccess_Content()
        {
            var responseMessage = await _httpclient.GetFromJsonAsync<IEnumerable<Transactions>>("/transactions");

            Assert.NotEmpty(responseMessage);
            Assert.NotNull(responseMessage);
            Assert.True(responseMessage!.Count() >= 0);
        }

        [Theory]
        [Trait("Category", "GET")]
        [InlineData("/transactions/{transactionId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Get_TransactionByAccountNumber_ReturnSuccessMessage(string url, Guid accountId)
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

            //Act
            var responseMessage = await _httpclient.GetAsync(url);
            var jsonAsResult = await responseMessage.Content.ReadFromJsonAsync<Transactions>();

            //Assert
            Assert.NotNull(responseMessage);
            Assert.IsType<Transactions>(responseMessage);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal(jsonAsResult, jsonAsResult);
            Assert.Equal(transaction, jsonAsResult);

            jsonAsResult!.Balance.Should().Be(5000.00M);
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
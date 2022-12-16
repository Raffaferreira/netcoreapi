using Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using WebApi;

namespace TestingXUnit.WebApi.UseCases
{
    public class CreditTest : IntegrationTesting
    {
        public CreditTest(WebApplicationFactory<Program> factory):base(factory)
        {}

        [Theory]
        [InlineData("/credit")]
        public async void Get_TransactionsFromSracth_ReturnSuccess_Content(string url)
        {
            //Arrange
            var httpClient = _factory.CreateClient();

            //Act
            var responseTransactions = await httpClient.GetFromJsonAsync<IEnumerable<Credito>>(url);

            //Assert
            Assert.Empty(responseTransactions);
            Assert.NotNull(responseTransactions);
            Assert.True(responseTransactions!.Count() >= 0);
        }

        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Get_TransactionByAccountNumber_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var credito = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            var httpClient = _factory.CreateClient();

            //Act
            var responseMessage = await httpClient.GetAsync(url.Replace("accountId", accountId.ToString()));
            var result = await responseMessage.Content.ReadAsStringAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Credito>(result);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal("\"transaction.ToString()\"", result);
            Assert.Equal(credito, JsonConvert.DeserializeObject<Credito>(result));
        }

        [Theory]
        [InlineData("/credit", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Post_NewTransaction_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var creditoPost = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            var httpClient = _factory.CreateClient();

            //Act
            var responseMessage = await httpClient.PostAsJsonAsync(url, creditoPost);
            var result = await responseMessage.Content.ReadAsStringAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Credito>(result);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal("\"transaction.ToString()\"", result);
            Assert.Equal(creditoPost, JsonConvert.DeserializeObject<Credito>(result));
        }
    }
}

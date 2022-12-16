using Azure;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using WebApi;
using Xunit.Abstractions;

namespace TestingXUnit.Integration.WebApi.UseCases
{
    public class CreditTest : IntegrationTesting, IDisposable
    {
        public CreditTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutput)
            : base(factory, testOutput)
        { }

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
            Assert.Single(responseTransactions);
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
            Assert.Equal(JsonConvert.DeserializeObject<Credito>(result), credito);
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
            var responseAsJson = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            var json = JsonConvert.DeserializeObject<Credito>(result);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(creditoPost), JsonConvert.SerializeObject(json));
        }


        [Theory]
        [InlineData("/credit", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Put_UpdateTransaction_ReturnSuccessMessage(string url, Guid accountId)
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

            var json = JsonConvert.DeserializeObject<Credito>(result);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(creditoPost), JsonConvert.SerializeObject(json));
        }

        [Theory]
        [InlineData("/credit", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Delete_TransactionByAccountId_ReturnSuccessMessage(string url, Guid accountId)
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

            var json = JsonConvert.DeserializeObject<Credito>(result);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(creditoPost), JsonConvert.SerializeObject(json));
        }

        [Theory]
        [InlineData("/credit", "3fa85f64-5717-4562-b3fc-2c963f66afb7")]
        public async void Delete_AttemptingDeletingTransactionNotFound_ReturnSuccessMessage(string url, Guid accountId)
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
            var responseAsJson = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            var json = JsonConvert.DeserializeObject<Credito>(result);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(creditoPost), JsonConvert.SerializeObject(json));
            Assert.Equal(JsonConvert.SerializeObject(creditoPost), JsonConvert.SerializeObject(json));
        }

        public void Dispose()
        {
            //_context.Database.EnsureDeleted();
            //_context.Dispose();
        }
    }
}

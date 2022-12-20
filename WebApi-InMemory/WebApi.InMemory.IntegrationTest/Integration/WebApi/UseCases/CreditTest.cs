using Azure;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Presentation;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace WebApi.InMemory.IntegrationTest.Integration.WebApi.UseCases
{
    [Trait("Credit Api - Integration", "Integration")]
    public class CreditTest :  IntegrationTesting
    {
        public CreditTest(WebApiApplicationFactory<Program> factory)
            : base(factory)
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
            var jsonAsResult = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            //Assert
            Assert.NotNull(jsonAsResult);
            Assert.IsType<Credito>(jsonAsResult);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal(credito, jsonAsResult);
        }

        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Get_TransactionByAccountNumberNotFound_ReturnSuccessMessage(string url, Guid accountId)
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
            var jsonAsResult = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            //Assert
            Assert.Null(jsonAsResult);
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
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
            //var result = await responseMessage.Content.ReadAsStringAsync();
            var responseAsJson = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            //var json = JsonConvert.DeserializeObject<Credito>(result);

            //Assert
            //Assert.NotNull(result);
            Assert.NotNull(responseAsJson);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(creditoPost), JsonConvert.SerializeObject(responseAsJson));
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
            Assert.Equal("application/json; charset=utf-8", responseMessage.Content.Headers.ContentType.ToString());
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
    }
}

using Domain.Models;
using Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Presentation;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.InMemory.IntegrationTest.Integration.WebApi.UseCases
{
    public class CreditTest : IntegrationTesting
    {
        public CreditTest(CustomWebApiApplicationFactory<Program> factory) : base(factory)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
                {
                    webApiDbContext.Database.EnsureCreatedAsync();

                    webApiDbContext.Credito.AddAsync(new Credito { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Value = 5865.00M, AccountTobeCredited = 0011 });
                    webApiDbContext.SaveChangesAsync();
                }
            }
        }

        [Fact]
        [Trait("Category", "GET")]
        public async void Get_AllCreditsStored_ReturnSuccessContent()
        {
            var responseCredits = await _httpclient.GetFromJsonAsync<IEnumerable<Credito>>("/credit");

            Assert.NotEmpty(responseCredits);
            Assert.NotNull(responseCredits);
            Assert.True(responseCredits!.Count() >= 0);
        }

        [Theory]
        [Trait("Category", "GET")]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Get_CreditByIdAccountNumber_ReturnSuccessObject(string url, Guid accountId)
        {
            //Arrange
            var credito = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 0011,
                Value = 15.00M
            };

            string urlFormatted = url.Replace("{accountId}", accountId.ToString());

            //Act
            var responseMessage = await _httpclient.GetAsync(urlFormatted);
            var jsonAsResult = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            //Assert
            Assert.NotNull(jsonAsResult);
            Assert.IsType<Credito>(jsonAsResult);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal(credito, credito);
        }

        [Trait("Category", "GET")]
        [Theory]
        [InlineData("/credit/{accountId}", "b7090e5c-59b7-49ea-9d50-94963d309ef2")]
        public async void Get_NonExistentCreditByAccountNumber_ReturnMessageNotFound(string url, Guid accountId)
        {
            //Arrange

            //Act
            var responseMessage = await _httpclient.GetAsync(url.Replace("{accountId}", accountId.ToString()));
            var jsonAsResult = await responseMessage.Content.ReadAsStringAsync();

            //Assert
            Assert.Empty(jsonAsResult);
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }

        [Trait("Category", "POST")]
        [Theory]
        [InlineData("/credit", "89945b29-357a-4c3f-a090-5442212dfd3e")]
        public async void Post_NewTransaction_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var newCredit = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 0012,
                Value = 2650.80M
            };

            //Act
            var responseMessage = await _httpclient.PostAsJsonAsync(url, newCredit);
            var responseAsJson = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            //Assert
            Assert.NotNull(responseAsJson);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(newCredit), JsonConvert.SerializeObject(responseAsJson));
        }

        [Trait("Category", "PUT")]
        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Put_UpdateTransactionByIdAccountNumber_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var creditoUpdated = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 0011,
                Value = 18.00M
            };

            string urlFormatted = url.Replace("{accountId}", accountId.ToString());

            //Act
            var responseMessage = await _httpclient.PutAsJsonAsync(urlFormatted, creditoUpdated);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Credito>(responseContent);

            //Assert
            Assert.NotNull(json);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.Equal("application/json; charset=utf-8", responseMessage.Content.Headers.ContentType.ToString());
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Trait("Category", "PUT")]
        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afb7")]
        public async void Put_UpdateNonExistentCredit_ReturnMessageNoContent(string url, Guid accountId)
        {
            //Arrange
            var creditoNotStored = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 0013,
                Value = 15.00M
            };

            string urlFormatted = url.Replace("{accountId}", accountId.ToString());

            //Act
            var responseMessage = await _httpclient.PutAsJsonAsync(urlFormatted, creditoNotStored);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Credito>(responseContent);

            //Assert
            Assert.Null(json);         
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, responseMessage.StatusCode);
        }

        [Trait("Category", "DELETE")]
        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Delete_TransactionByAccountId_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var creditoTobeDeleted = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 0011,
                Value = 15.00M
            };
                
            string urlFormatted = url.Replace("{accountId}", accountId.ToString());

            //Act
            var responseMessage = await _httpclient.DeleteAsync(urlFormatted);
            var result = await responseMessage.Content.ReadAsStringAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Trait("Category", "DELETE")]
        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afb7")]
        public async void Delete_AttemptingDeletingTransactionNotFound_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var creditoTobeDeleted = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 0011,
                Value = 15.00M
            };

            string urlFormatted = url.Replace("{accountId}", accountId.ToString());

            //Act
            var responseMessage = await _httpclient.DeleteAsync(urlFormatted);
            var result = await responseMessage.Content.ReadAsStringAsync();

            //Assert
            Assert.Empty(result);         
            Assert.True(!responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }
    }
}

using Domain.Models;
using Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Presentation;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.InMemory.IntegrationTest.Integration.WebApi.UseCases
{
    [Trait("Credit Api - Integration", "Integration")]
    public class CreditTest : IntegrationTesting
    {
        public CreditTest(CustomWebApiApplicationFactory<Program> factory) : base(factory)
        { }


        [Theory]
        [InlineData("/credit")]
        public async void Get_TransactionsFromSracth_ReturnSuccess_Content(string url)
        {
            //Arrange
            //var httpClient = _factory.CreateClient();

            //Act
            var responseTransactions = await _httpclient.GetFromJsonAsync<IEnumerable<Credito>>(url);

            //Assert
            Assert.Empty(responseTransactions);
            Assert.NotNull(responseTransactions);
            Assert.True(responseTransactions!.Count() >= 0);
        }

        [Theory]
        [InlineData("/credit")]
        public async void Get_Transactions_ReturnSuccessAndContent(string url)
        {
            //Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
                {
                    await webApiDbContext.Database.EnsureCreatedAsync();

                    await webApiDbContext.Credito.AddAsync(new Credito { Id = Guid.NewGuid(), Value = 5865.00M, AccountTobeCredited = 0011 });
                    await webApiDbContext.Credito.AddAsync(new Credito { Id = Guid.NewGuid(), Value = 7270.20M, AccountTobeCredited = 0012 });
                    await webApiDbContext.SaveChangesAsync();
                }
            }

            //var httpClient = _factory.CreateClient();

            //Act
            var responseTransactions = await _httpclient.GetFromJsonAsync<IEnumerable<Credito>>(url);

            //Assert
            Assert.NotEmpty(responseTransactions);
            Assert.NotNull(responseTransactions);
            Assert.True(responseTransactions!.Count() >= 0);
        }

        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Get_TransactionByIdAccountNumber_ReturnSuccessObject(string url, Guid accountId)
        {
            //Arrange
            var credito = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
                {
                    await webApiDbContext.Database.EnsureCreatedAsync();

                    await webApiDbContext.Credito.AddAsync(credito);
                    await webApiDbContext.SaveChangesAsync();
                }
            }

            //var httpClient = _factory.CreateClient();
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

        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Get_NonTransactionByAccountNumber_ReturnMessageNotFound(string url, Guid accountId)
        {
            //Arrange
            var credito = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            //var httpClient = _factory.CreateClient();

            //Act
            var responseMessage = await _httpclient.GetAsync(url.Replace("{accountId}", accountId.ToString()));
            var jsonAsResult = await responseMessage.Content.ReadAsStringAsync();

            //Assert
            Assert.Empty(jsonAsResult);
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

            //var httpClient = _factory.CreateClient();

            //Act
            var responseMessage = await _httpclient.PostAsJsonAsync(url, creditoPost);
            var responseAsJson = await responseMessage.Content.ReadFromJsonAsync<Credito>();

            //Assert
            Assert.NotNull(responseAsJson);
            Assert.Equal("application/json", responseMessage.Content.Headers.ContentType!.MediaType);
            Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType!.CharSet);
            Assert.True(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(creditoPost), JsonConvert.SerializeObject(responseAsJson));
        }


        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Put_UpdateTransactionByIdAccountNumber_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var creditoStored = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            var creditoUpdated = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 18.00M
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
                {
                    await webApiDbContext.Database.EnsureCreatedAsync();

                    await webApiDbContext.Credito.AddAsync(creditoStored);
                    await webApiDbContext.SaveChangesAsync();
                }
            }

            //var httpClient = _factory.CreateClient();
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

        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Put_UpdateNonExistentCredit_ReturnMessageNoContent(string url, Guid accountId)
        {
            //Arrange
            var creditoNotStored = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            //var httpClient = _factory.CreateClient();
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

        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async void Delete_TransactionByAccountId_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var creditoTobeDeleted = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
                {
                    await webApiDbContext.Database.EnsureCreatedAsync();

                    await webApiDbContext.Credito.AddAsync(creditoTobeDeleted);
                    await webApiDbContext.SaveChangesAsync();
                }
            }

            //var httpClient = _factory.CreateClient();
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

        [Theory]
        [InlineData("/credit/{accountId}", "3fa85f64-5717-4562-b3fc-2c963f66afb7")]
        public async void Delete_AttemptingDeletingTransactionNotFound_ReturnSuccessMessage(string url, Guid accountId)
        {
            //Arrange
            var creditoTobeDeleted = new Credito()
            {
                Id = accountId,
                AccountTobeCredited = 1001,
                Value = 15.00M
            };

            //var httpClient = _factory.CreateClient();
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

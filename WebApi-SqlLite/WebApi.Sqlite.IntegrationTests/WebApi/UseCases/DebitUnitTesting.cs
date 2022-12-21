using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Presentation;
using Azure;

namespace TestingXUnit.WebApi.UseCases
{
    public class DebitUnitTesting : IClassFixture<CustomWebApiApplicationFactory<Program>>
    {
        private readonly CustomWebApiApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public DebitUnitTesting(CustomWebApiApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("/debits")]
        public async void Get_AllDebits_ReturnSuccess_Content(string url)
        {
            var httpClient = _factory.CreateClient();

            var responseMessage = await httpClient.GetAsync(url);

            var result = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal("Hello World!", result);
            Assert.Equal("text/html; charset=utf-8", responseMessage.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/debits/{1}")]
        public async void Get_DebitByAccountNumber_ReturnSuccessMessage(string url)
        {
            var httpClient = _factory.CreateClient();

            var responseMessage = await httpClient.GetAsync(url);

            var result = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal("Hello World!", result);
        }

        [Theory]
        [InlineData("/debit")]
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

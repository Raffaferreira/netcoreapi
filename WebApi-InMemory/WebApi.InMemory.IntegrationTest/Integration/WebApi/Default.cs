using Microsoft.AspNetCore.Mvc.Testing;
using Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi;

namespace TestingXUnit.Integration.WebApi
{
    public class Default : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public Default(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("/sum?n1=10&n2=6")]
        public async void Sum_Returns16For10And6(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var stringResult = await response.Content.ReadAsStringAsync();
            var intResult = int.Parse(stringResult);

            Assert.Equal(16, intResult);
        }

        [Theory]
        [InlineData("/hello")]
        public async void DefaultRoute_ReturnsHelloWorld(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.Equal("Hello World!", stringResult);
        }
    }
}

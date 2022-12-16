using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;

namespace TestingXUnit
{
    public abstract class IntegrationTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly HttpClient _httpClient;

        public IntegrationTesting(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateDefaultClient();
        }
    }
}

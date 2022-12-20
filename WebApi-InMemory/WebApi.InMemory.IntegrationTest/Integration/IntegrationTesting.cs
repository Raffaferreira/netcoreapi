using Microsoft.AspNetCore.Mvc.Testing;
using Presentation;
using Xunit.Abstractions;

namespace WebApi.InMemory.IntegrationTest.Integration
{
    [Trait("Category", "API Integration")]
    public abstract class IntegrationTesting : IClassFixture<WebApiApplicationFactory<Program>> 
    {
        protected readonly WebApiApplicationFactory<Program> _factory;
        protected readonly HttpClient _client;

        public IntegrationTesting(WebApiApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
    }
}

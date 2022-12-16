using Microsoft.AspNetCore.Mvc.Testing;
using Presentation;
using Xunit.Abstractions;

namespace TestingXUnit.Integration
{
    [Trait("Category", "Integration")]
    public abstract class IntegrationTesting : IClassFixture<StandardApplicationFactory<Program>>//IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly HttpClient _httpClient;
        protected readonly ITestOutputHelper _testOutputHelper;

        public IntegrationTesting(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _factory = factory;
            _httpClient = _factory.CreateDefaultClient();
        }
    }
}

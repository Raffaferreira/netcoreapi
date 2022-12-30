using Presentation;

namespace TestingXUnit
{
    [Trait("Category", "API Integration")]
    public abstract class IntegrationTesting : IClassFixture<CustomWebApiApplicationFactory<Program>>
    {
        protected readonly CustomWebApiApplicationFactory<Program> _factory;
        protected readonly HttpClient _httpClient;

        public IntegrationTesting(CustomWebApiApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateDefaultClient();
        }
    }
}

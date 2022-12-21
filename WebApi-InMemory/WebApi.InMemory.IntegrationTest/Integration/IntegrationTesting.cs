using Presentation;

namespace WebApi.InMemory.IntegrationTest.Integration
{
    [Trait("Category", "API Integration")]
    public abstract class IntegrationTesting : IClassFixture<CustomWebApiApplicationFactory<Program>> 
    {
        protected readonly CustomWebApiApplicationFactory<Program> _factory;
        protected readonly HttpClient _client;

        public IntegrationTesting(CustomWebApiApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
    }
}

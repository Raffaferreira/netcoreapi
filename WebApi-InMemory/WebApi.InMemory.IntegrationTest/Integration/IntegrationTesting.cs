using Presentation;

namespace WebApi.InMemory.IntegrationTest.Integration
{
    [Trait("Category", "API Integration")]
    public abstract class IntegrationTesting : IClassFixture<CustomWebApiApplicationFactory<Program>> 
    {
        protected readonly CustomWebApiApplicationFactory<Program> _factory;
        protected readonly HttpClient _httpclient;

        public IntegrationTesting(CustomWebApiApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpclient = _factory.CreateClient();
        }
    }
}

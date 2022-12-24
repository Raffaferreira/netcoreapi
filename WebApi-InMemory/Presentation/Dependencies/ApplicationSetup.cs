using Microsoft.Extensions.Options;

namespace Presentation.Dependencies
{
    public class ApplicationSetup 
    {
        public string? DisplayMessage { get; set; }
    }

    public class ApplicationOptionsConfiguration : IConfigureOptions<ApplicationSetup>
    {
        private readonly IConfiguration _configuration;

        public ApplicationOptionsConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(ApplicationSetup options)
        {
            _configuration.GetSection(nameof(ApplicationSetup)).Bind(options);
        }
    }
}

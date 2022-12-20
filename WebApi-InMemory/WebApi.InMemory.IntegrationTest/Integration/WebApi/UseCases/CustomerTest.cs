using Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.InMemory.IntegrationTest.Integration.WebApi.UseCases
{
    [Trait("Customer Api - Integration", "Integration")]
    public class CustomerTest : IntegrationTesting
    {
        public CustomerTest(WebApiApplicationFactory<Program> factory) : base(factory)
        {
        }
    }
}

using Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingXUnit;

namespace WebApi.Sql.IntegrationTests.WebApi.UseCases
{
    public class CustomerTest : IntegrationTesting
    {
        public CustomerTest(CustomWebApiApplicationFactory<Program> factory) : base(factory)
        {
        }
    }
}

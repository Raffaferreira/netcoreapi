using Domain.Interfaces.Repository;
using Infrastructure.Repositories;

namespace Presentation.Dependencies.Startup
{
    public static class RegisterServices
    {
        public static void AddRegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICreditRepository, CreditRepository>();
        }
    }
}

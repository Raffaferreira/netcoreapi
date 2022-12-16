using Domain.Interfaces.Repository;
using Infrastructure.Repositories;

namespace Presentation.Dependencies.Startup
{
    public static class RegisterRepositories
    {
        public static void AddRegisterRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICreditRepository, CreditRepository>();
            builder.Services.AddTransient<IDebitRepository, DebitRepository>();
        }
    }
}

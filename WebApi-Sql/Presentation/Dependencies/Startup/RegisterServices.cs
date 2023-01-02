using Application.Services;
using Domain.Interfaces.Services;

namespace Presentation.Dependencies.Startup
{
    public static class RegisterServices
    {
        public static void AddRegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICreditService, CreditService>();
            builder.Services.AddTransient<IDebitService, DebitService>();
        }
    }
}

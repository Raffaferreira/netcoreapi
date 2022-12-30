using MediatR;

namespace Presentation.ViewModel
{
    public class CustomerHandler : IRequestHandler<CustomerRequest, CustomerResponse>
    {
        public CustomerHandler()
        {

        }

        public Task<CustomerResponse> Handle(CustomerRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CustomerResponse()
            {
                Data = "Hello world!"
            });
        }
    }
}

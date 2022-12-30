using MediatR;

namespace Presentation.ViewModel
{
    public class CustomerRequest : IRequest<CustomerResponse>
    {
        public string? FirstName { get; set; }
        public string? Age { get; set; }
        public DateTime BirthDay { get; set; }
    }
}

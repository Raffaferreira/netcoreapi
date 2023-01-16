namespace WebApi.Sql.UnitTests.SUT.Models
{
    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual string? Name { get; set; }
        public virtual Address? AddressNavigation { get; set; }
    }
}

namespace WebApi.Sql.UnitTests.SUT.Models
{
    public class Address
    {
        public virtual int Id { get; set; }
        public virtual int StreetNumber { get; set; }
        public virtual string StreetName { get; set; }
        public virtual string StateCode { get; set; }
        public virtual string Country { get; set; }
    }
}

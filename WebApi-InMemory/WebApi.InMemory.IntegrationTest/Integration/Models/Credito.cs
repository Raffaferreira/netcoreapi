namespace WebApi.InMemory.IntegrationTest.Integration.Models
{
    public class Credito
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public int AccountTobeCredited { get; set; }
    }
}

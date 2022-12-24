namespace WebApi.InMemory.IntegrationTest.Integration.Models
{
    public record Transactions
    {
        public Guid Id { get; set; }
        public decimal Credited { get; set; }
        public decimal Debited { get; set; }
        public decimal Balance { get; set; }
        public int AccountNumber { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}

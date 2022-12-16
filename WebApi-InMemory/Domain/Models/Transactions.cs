using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Transactions
    {
        [Required]
        public Guid Id { get; set; }
        public decimal Credited { get; set; }
        public decimal Debited { get; set; }
        public decimal Balance { get; set; }
        public int AccountNumber { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}

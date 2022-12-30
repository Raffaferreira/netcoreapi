using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Debito
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public int AccountTobeWithdraw { get; set; }
    }
}

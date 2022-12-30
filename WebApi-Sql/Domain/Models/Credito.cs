using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Credito
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public int AccountTobeCredited { get; set; }
    }
}

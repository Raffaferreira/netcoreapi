using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Account
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int AccountNumber { get; set; }      
        public decimal Balance { get; set; }
    }
}

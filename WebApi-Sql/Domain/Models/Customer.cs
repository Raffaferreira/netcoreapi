using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Customer
    {
        [Required]
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string? FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Age { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BirthDay { get; set; }
    }
}

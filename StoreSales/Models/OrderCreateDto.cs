using StoreSales.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class OrderCreateDto
    {
        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}

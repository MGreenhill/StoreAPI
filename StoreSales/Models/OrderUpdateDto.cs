using StoreSales.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class OrderUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}

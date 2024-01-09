using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class TransactionPutDto
    {
        [Required]
        public TransactionUpdateDto Transaction { get; set; }
        [Required]
        public List<OrderPutDto>? Orders { get; set; }
    }
}

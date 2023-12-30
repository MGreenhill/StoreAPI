using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class TransactionPostDto
    {
        [Required]
        public TransactionCreateDto Transaction { get; set; }

        public List<OrderCreateDto>? Orders { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class TransactionCreateDto
    {
        [Required]
        public int PersonId { get; set; }

        public DateTime? TimeOccurred { get;set;}

        public string? Note { get; set; }
    }
}

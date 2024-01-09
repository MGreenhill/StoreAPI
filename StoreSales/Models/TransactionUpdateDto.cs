using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class TransactionUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PersonId { get; set; }
        [Required]
        public DateTime? TimeOccurred { get; set;}

        public string? Note { get; set; }
    }
}

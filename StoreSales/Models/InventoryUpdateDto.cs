using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class InventoryUpdateDto
    {
        [Required]
        public int Id { get; set; }


        [Required(ErrorMessage = "Please include an existing ItemId")]
        public int ItemId { get; set; }

        [Range(0, 99)]
        [Required(ErrorMessage = "Quantity must be between 0 and 99")]
        public int Quantity { get; set; }
    }
}

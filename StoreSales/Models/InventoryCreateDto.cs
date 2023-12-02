using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class InventoryCreateDto
    {
        [Required(ErrorMessage = "Please include the item's id")]
        public int ItemId { get; set; }

        [Range(0, 99)]
        [Required(ErrorMessage = "Please include a quantity between 0 and 99")]
        public int Quantity { get; set; }
    }
}

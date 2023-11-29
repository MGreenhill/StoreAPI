using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class ItemUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please include a name within 50 characters")]
        public string Name { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please include a type within 50 characters")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Please include a price")]
        public decimal Price { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "Please include a description with 200 characters")]
        public string? Description { get; set; }
    }
}

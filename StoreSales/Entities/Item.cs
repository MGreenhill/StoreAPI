using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreSales.API.Entities
{
    public class Item : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public Item(string name, string type, decimal price)
        {
            Name = name;
            Type = type;
            Price = price;
        }
    }
}

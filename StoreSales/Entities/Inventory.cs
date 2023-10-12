using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreSales.API.Entities
{
    public class Inventory : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}

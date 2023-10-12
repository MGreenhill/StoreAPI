using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreSales.API.Entities
{
    public class Order : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TransactionId")]
        public int TransactionId { get; set; }

        [ForeignKey("ItemId")]
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}

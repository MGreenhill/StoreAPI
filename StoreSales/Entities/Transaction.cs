using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreSales.API.Entities
{
    public class Transaction : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public int PersonId { get; set; }

        public DateTime TimeOccurred { get; set; }

        public string? note { get; set; }

        public ICollection<Order> Contents { get; set; } = new List<Order>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreSales.API.Entities
{
    public class Person : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }


        public ICollection<Transaction> Purchases { get; set; } = new List<Transaction>();

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

    }
}

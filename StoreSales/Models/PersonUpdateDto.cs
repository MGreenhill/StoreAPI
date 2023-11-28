using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class PersonUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a first name.")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide a last name.")]
        [StringLength(25)]
        public string LastName { get; set; }


    }
}

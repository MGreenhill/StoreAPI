using System.ComponentModel.DataAnnotations;

namespace StoreSales.API.Models
{
    public class PersonCreateDto
    {
        [Required(ErrorMessage = "")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "")]
        [StringLength(25)]
        public string LastName { get; set; }
    }
}

using StoreSales.API.Entities;
using System.Text.Json.Serialization;

namespace StoreSales.API.Models
{
    public class PersonWithoutTransactionsDto
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public string FirstName { get; set; }

        [JsonIgnore]
        public string LastName { get; set; }

        public string Name { get { return $"{FirstName} {LastName}"; } set { } }
    }
}

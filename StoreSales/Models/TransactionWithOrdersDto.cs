using StoreSales.API.Entities;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StoreSales.API.Models
{
    public class TransactionWithOrdersDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int PersonId { get; set; }

        public PersonWithoutTransactionsDto Person { get; set; }

        public DateTime TimeOccurred { get; set; }

        public string? note { get; set; }

        public ICollection<Order> Contents { get; set; }

        public decimal TotalPrice { get; set; }
    }
}

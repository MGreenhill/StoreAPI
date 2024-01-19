using StoreSales.API.Entities;
using Newtonsoft.Json;

namespace StoreSales.API.Models
{
    public class OrderDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int TransactionId { get; set; }

        [JsonIgnore]
        public int ItemId { get; set; }
        public ItemDto Item { get; set; }


        public int Quantity { get; set; }
    }
}

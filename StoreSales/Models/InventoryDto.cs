using Newtonsoft.Json;
using StoreSales.API.Entities;

namespace StoreSales.API.Models
{
    public class InventoryDto
    {
        [JsonIgnore]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

    }
}

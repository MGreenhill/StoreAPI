using Newtonsoft.Json;

namespace StoreSales.API.Models
{
    public class ItemDto
    {
        //Name, Type, Price, Description
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }
    }
}

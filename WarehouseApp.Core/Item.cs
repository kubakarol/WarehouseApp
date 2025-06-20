using System.ComponentModel.DataAnnotations;

namespace WarehouseApp.Core
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}

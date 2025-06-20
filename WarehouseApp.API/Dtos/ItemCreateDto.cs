using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WarehouseApp.API.Dtos
{
    public class ItemCreateDto
    {
        [Required]
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public int Quantity { get; set; }

        public IFormFile? Image { get; set; }
    }
}

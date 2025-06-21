using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WarehouseApp.API.Dtos;

public class ItemCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public IFormFile? Image { get; set; }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseApp.API.Data;
using WarehouseApp.API.Dtos;
using WarehouseApp.API.Entities;
using WarehouseApp.Core;

namespace WarehouseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly WarehouseDbContext _context;

    public ItemController(WarehouseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _context.Items.ToListAsync();

        var result = items.Select(i => new Item
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            Quantity = i.Quantity,
            ImageUrl = i.ImageUrl
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ItemCreateDto dto)
    {
        string fileName = "";
        if (dto.Image != null)
        {
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
            var path = Path.Combine(uploadsDir, fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await dto.Image.CopyToAsync(stream);
        }

        var entity = new ItemEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Quantity = dto.Quantity,
            ImageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}"
        };

        _context.Items.Add(entity);
        await _context.SaveChangesAsync();

        var item = new Item
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Quantity = entity.Quantity,
            ImageUrl = entity.ImageUrl
        };

        return CreatedAtAction(nameof(GetAll), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Item item)
    {
        if (id != item.Id) return BadRequest();

        var entity = await _context.Items.FindAsync(id);
        if (entity == null) return NotFound();

        entity.Name = item.Name;
        entity.Description = item.Description;
        entity.Quantity = item.Quantity;
        entity.ImageUrl = item.ImageUrl;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Items.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Items.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ➕ Zwiększanie stanu
    [HttpPut("{id}/add/{qty}")]
    public async Task<IActionResult> AddStock(int id, int qty)
    {
        var item = await _context.Items.FindAsync(id);
        if (item is null) return NotFound();

        item.Quantity += qty;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ➖ Zmniejszanie stanu
    [HttpPut("{id}/remove/{qty}")]
    public async Task<IActionResult> RemoveStock(int id, int qty)
    {
        var item = await _context.Items.FindAsync(id);
        if (item is null) return NotFound();

        item.Quantity = Math.Max(0, item.Quantity - qty);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

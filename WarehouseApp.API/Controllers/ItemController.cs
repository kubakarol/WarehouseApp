using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using WarehouseApp.API.Data;
using WarehouseApp.Core;
using WarehouseApp.API.Dtos;

namespace WarehouseApp.API.Controllers
{
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
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.Items.ToListAsync());

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

            var item = new Item
            {
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.Quantity,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}"
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = item.Id }, item);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Item item)
        {
            if (id != item.Id) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

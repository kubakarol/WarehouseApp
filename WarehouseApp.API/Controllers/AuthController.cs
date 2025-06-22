using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseApp.API.Data;
using WarehouseApp.API.Entities;
using WarehouseApp.Core;

namespace WarehouseApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly WarehouseDbContext _context;

        public AuthController(WarehouseDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return BadRequest("Username already exists.");

            var entity = new UserEntity
            {
                Username = user.Username,
                Password = user.Password,
                Role = user.Role
            };

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == login.Username && u.Password == login.Password);

            if (userEntity == null)
                return Unauthorized();

            return Ok(new User
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Password = userEntity.Password,
                Role = userEntity.Role
            });
        }
    }

}

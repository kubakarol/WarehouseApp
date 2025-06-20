using Microsoft.EntityFrameworkCore;
using WarehouseApp.Core;

namespace WarehouseApp.API.Data
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
            : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }

    }
}

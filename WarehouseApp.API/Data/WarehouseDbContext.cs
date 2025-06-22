using Microsoft.EntityFrameworkCore;
using WarehouseApp.Core;
using WarehouseApp.API.Entities;

namespace WarehouseApp.API.Data
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
            : base(options) { }

        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<UserEntity> Users { get; set; }

    }
}

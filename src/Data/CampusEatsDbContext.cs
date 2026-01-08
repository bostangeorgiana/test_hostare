using Microsoft.EntityFrameworkCore;
using CampusEats.Domain.Entities;

namespace CampusEats.Data
{
    public class CampusEatsDbContext : DbContext
    {
        public CampusEatsDbContext(DbContextOptions<CampusEatsDbContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }

        // ...existing code...
    }
}


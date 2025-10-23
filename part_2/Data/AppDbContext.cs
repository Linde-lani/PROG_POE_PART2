using part_2.Models;
using Microsoft.EntityFrameworkCore;

namespace part_2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options) { }

        public DbSet<Claim> Claims { get; set; }

        public DbSet<RegisterViews> RegisterViews { get; set; }
    }
}

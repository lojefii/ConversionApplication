using Microsoft.EntityFrameworkCore;

namespace ConversionApplication
{
    public class AppDbContext : DbContext
    {
        public DbSet<Temperature> Temperature { get; set; }
        public DbSet<Length> Length { get; set; }
        public DbSet<Weight> Weight { get; set; }
        public DbSet<Energy> Energy { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

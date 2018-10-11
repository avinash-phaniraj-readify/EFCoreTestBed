using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TestHostInvalidCast
{
    public class DataContext : DbContext
    {
        public DbSet<InventoryPool> InventoryPool { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryPool>()
                .ToTable("InventoryPool");

            base.OnModelCreating(modelBuilder);
        }
    }

    public partial class InventoryPool
    {
        [Key]
        public int Id { get; set; }
        public double Quantity { get; set; }
    }

}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TestHostForCastException
{
    public class HierarchyTestDataContext : DbContext
    {
        public HierarchyTestDataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasDiscriminator<int>("AnimalType")
                .HasValue<Animal>(1)
                .HasValue<Cow>(2)
                .HasValue<Dog>(3);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AnimalType { get; set; }
    }

    public class Cow : Animal { }

    public class Dog : Animal { }
}

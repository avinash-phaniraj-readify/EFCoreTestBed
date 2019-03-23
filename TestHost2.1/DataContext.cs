using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHost_2_1
{
    public class TestDataContext : DbContext
    {
        public TestDataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasDiscriminator<int>("AnimalType")
                .HasValue<Animal>(1)
                .HasValue<Giraffe>(2);

            modelBuilder.Entity<Giraffe>()
                .Ignore(x => ((IName) x).Name);

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

    public interface IName
    {
        byte[] Name { get; set; }
    }

    public class Giraffe : Animal, IName
    {
        byte[] IName.Name { get; set; }
    }
}

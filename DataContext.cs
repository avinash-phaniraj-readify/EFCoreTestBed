using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestHost
{
    public class TestDataContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Trip> Trips { get; set; }

        public TestDataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>()
          .HasOne(a => a.Truck)
          .WithOne(b => b.Driver)
          .HasForeignKey<Truck>(b => b.DriverId);
        }
    }

    
    [Table("Drivers")]
    public class Driver 
    {
        [Key]
        public int RecordId { get; set; }
        public Truck Truck { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? YearsOfExperience { get; set; }
    }

    [Table("Trucks")]
    public class Truck
    {
        [Key]
        public int RecordId { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; }

    }

    [Table("Departments")]
    public class Department
    {
        [Key]
        public int RecordId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }

    [Table("Trips")]
    public class Trip
    {
        [Key]
        public int RecordId { get; set; }
        public Driver Driver { get; set; }
        public int DriverId { get; set; }
        public int TotalKilometers { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

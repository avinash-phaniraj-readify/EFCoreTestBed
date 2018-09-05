using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHost
{
    public class TestDataContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeDevice> EmployeeDevice { get; set; }


        public TestDataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDevice>()
                .HasOne<Employee>(a => a.Employee)
                .WithMany(p => p.Devices)
                .HasForeignKey(a => a.EmployeeId)
                .IsRequired(true);

            base.OnModelCreating(modelBuilder);
        }
    }


    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<EmployeeDevice> Devices { get; set; }
        public int NumberOfShare { get; set; }
        public int YearsOfService { get; set; }
    }

    public class EmployeeDevice
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Device { get; set; }
        public Employee Employee { get; set; }
    }

}

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHostForCastException
{
    public class TestDataContext : DbContext
    {
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

    public interface IEmployee
    {
        string Name { get; set; }
    }
    public class Employee : IEmployee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<EmployeeDevice> Devices { get; set; }
    }

    public class EmployeeDevice
    {
        [Key]
        public int Id { get; set; }
        public short DeviceId { get; set; }
        public int EmployeeId { get; set; }
        public string Device { get; set; }
        public Employee Employee { get; set; }
    }

}

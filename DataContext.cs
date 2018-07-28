using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        private readonly ILazyLoader _lazyLoader;

        public Employee(ILazyLoader lazyLoader)
        {
            this._lazyLoader = lazyLoader;
            this._devices = new List<EmployeeDevice>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        private ICollection<EmployeeDevice> _devices;
        public ICollection<EmployeeDevice> Devices {
            get
            {
                _lazyLoader.Load(this, nameof(Devices));
                return _devices;
            }
        }
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

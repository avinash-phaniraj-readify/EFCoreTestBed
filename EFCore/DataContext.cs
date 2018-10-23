using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linq2SqlEFCoreBehaviorsTest.EFCore
{
    public class EFCoreDataContext : DbContext
    {
        public EFCoreDataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDevice>()
                .HasOne<Employee>(a => a.Employee)
                .WithMany(p => p.Devices)
                .HasForeignKey(a => a.EmployeeId)
                .IsRequired(true);

            modelBuilder.Entity<EmployeeDetails>()
                .HasOne<Employee>(a => a.Employee)
                .WithOne(p => p.EmployeeDetails)
                .HasForeignKey<EmployeeDetails>(a=> a.EmployeeId)
                .IsRequired(true);

            base.OnModelCreating(modelBuilder);
        }
    }

  
    public partial class Employee 
    {
        public Employee()
        {
            Devices = new HashSet<EmployeeDevice>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [Column("Created", TypeName = "datetime")]  // 
        public DateTime Created { get; set; }

        public EmployeeDetails EmployeeDetails { get; set; }

        public ICollection<EmployeeDevice> Devices { get; set; }
    }

    public partial class EmployeeDevice
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Device { get; set; }
        public Employee Employee { get; set; }
    }


    public partial class EmployeeDetails
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Details { get; set; }
    }

}

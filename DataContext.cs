﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHostForCastException
{
    public class TestDataContext : DbContext
    {
        public TestDataContext(DbContextOptions options) : base(options) { }

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
        protected ILazyLoader _lazyLoader;

        public EmployeeDevice(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Device { get; set; }

        private Employee _employee;
        public Employee Employee
        {
            get
            {
                _lazyLoader?.Load(this, nameof(Employee));
                return _employee;
            }
            set
            {
                _employee = value;
            }
        }
    }
}

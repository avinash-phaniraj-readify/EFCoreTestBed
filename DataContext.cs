using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TestHostForCastException
{
    public class TestDataContext : DbContext
    {
        public TestDataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Account> Accounts  { get; set; }

    }

    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public bool IsCredit { get; set; }
        public int Type { get; set; }
        public double Amount { get; set; }
        public string ACNumber { get; set; }
    }
}

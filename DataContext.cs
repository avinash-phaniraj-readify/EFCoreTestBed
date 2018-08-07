using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestHostForCastException
{
    public class TestDataContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Asset> Providers { get; set; }

        public TestDataContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Asset>()
           .HasDiscriminator<int>("AssetType")
           .HasValue<Product>(1);
        }
    }

    [Table("Assets")]
    public abstract class Asset
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int AssetType { get; set; }
        public int ProviderId { get; set; }
        public TaxCategory TaxCategory { get; set; }
    }

    public class Product : Asset
    {
        public decimal Price { get; set; }
    }

    [Table("Providers")]
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Table("TaxCategories")]
    public class TaxCategory
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public decimal Percentage { get; set; }

        public int? SerialCategory { get; set; }
    }

    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

    }
}

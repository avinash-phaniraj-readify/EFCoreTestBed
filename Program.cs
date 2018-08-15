using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {

            var ceConnectionString = ConfigurationManager.AppSettings["SqlCeConnection"];
            var ceConnection = new SqlCeConnection(ceConnectionString);


            var options = new DbContextOptionsBuilder()
                .UseSqlCe(ceConnection);

            Func<DbContextOptionsBuilder, DbContextOptionsBuilder> configureOptions = (c) => c.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) }));
            options.EnableSensitiveDataLogging();

            options = configureOptions(options);

            using (var dc = new SampleDataContext(options.Options))
            {
                decimal amount = (from employees in dc.Employees select employees).Sum(p => p.Salary);

                var payrollView = dc.Payrolls
                    .Select(p => new
                    {
                        PayRollId = p.RecordId,
                        PayrollDate = p.Date,
                        PayrollTaxRate = p.TaxRate,
                        TotalWages = amount
                    });

                var itemsList = payrollView.ToList();

                Console.ReadLine();
            }
        }
    }

    public class SampleDataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }

        public SampleDataContext(DbContextOptions options) : base(options)
        {

        }
    }

    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int RecordId { get; set; }

        public string Name { get; set; }

        public decimal Salary { get; set; }
    }

    [Table("Payrolls")]
    public class Payroll
    {
        [Key]
        public int RecordId { get; set; }

        public DateTime Date { get; set; }

        public float TaxRate { get; set; }
    }

}

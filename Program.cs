using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();

            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlCe(ceConnection, x => x.UseRelationalNulls(true))
                .UseLoggerFactory(MyLoggerFactory)
                .Options;

            var context = new TestDbContext(options);
            var employees = context.Set<Employee>();
            var employeeDevices = context.Set<EmployeeDevice>();

            Console.WriteLine("Generated Sql query for WHERE:");
            var result1 = (from e in employees
                           join ed in employeeDevices on e.Id equals ed.EmployeeId into x
                           from y in x.DefaultIfEmpty()
                           where y.EmployeeId != 0
                           select y.Device).ToList();

            Console.WriteLine("\nGenerated Sql query for SELECT:");
            var result2 = (from e in employees
                           join ed in employeeDevices on e.Id equals ed.EmployeeId into x
                           from y in x.DefaultIfEmpty()
                           select y.EmployeeId != 0 ? y.Device : "n/a").ToList();

            Console.ReadKey();
        }

        private static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(
            new[] { new ConsoleLoggerProvider((category, level) => level == LogLevel.Information, true) }
        );
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Diagnostics;

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
                .UseSqlCe(ceConnection)
                .UseLoggerFactory(MyLoggerFactory)
                .Options;

            var context = new TestDbContext(options);

            var emp = new Employee { Id = 1, Name = "known employee" };
            emp.Devices = new List<EmployeeDevice>();
            context.Attach(emp);

            var device = new EmployeeDevice { Id = 1, EmployeeId = 1, Device = "known device" };

            emp.Devices.Add(device);
            context.ChangeTracker.Entries();

            Debug.Assert(context.Entry(device).State == EntityState.Unchanged);

            Console.ReadKey();
        }

        private static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(
            new[] { new ConsoleLoggerProvider((category, level) => level == LogLevel.Information, true) }
        );
    }
}
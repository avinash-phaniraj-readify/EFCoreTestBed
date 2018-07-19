using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHostForCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();

            var options = new DbContextOptionsBuilder<HierarchyTestDataContext>()
                .UseSqlCe(ceConnection)
                .UseLoggerFactory(MyLoggerFactory)
                .Options;

            var context = new HierarchyTestDataContext(options);

            Console.WriteLine("Generated Sql query with casting:");
            var dogNamedWoof = context.Set<Animal>().Where(x => x is Dog && ((Dog)x).Name == "Woof").ToList();

            Console.WriteLine("\nGenerated Sql query with as operator:");
            dogNamedWoof = context.Set<Animal>().Where(x => x is Dog && (x as Dog).Name == "Woof").ToList();

            Console.ReadKey();
        }

        private static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(
            new[] { new ConsoleLoggerProvider((category, level) => level == LogLevel.Information, true) }
        );
    }
}
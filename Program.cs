using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using SqlCeConnection = System.Data.SqlServerCe.SqlCeConnection;
using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace TestHost
{
    class Program
    {
        private static DateTime _theLocalDate = DateTime.SpecifyKind(new DateTime(2010, 10, 10), DateTimeKind.Local);

        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer(GetSqlConnection())
                //.UseSqlCe(GetCeConnection()) -- SqlCe provider works consistently.
                .Options;

            var context = new TestDbContext(options);

            //this is translated to a parameter with the timezone info being truncated?
            var results = context.Set<Employee>()
              .Where(w => w.Created > _theLocalDate)
              .ToList();

            Debug.Assert(results.Any());

            //System.Data.SqlClient.SqlException: 'Conversion failed when converting date and/or time from character string.'
            results = context.Set<Employee>()
               .Where(GetFilterWithLocalDateTime<Employee>(_theLocalDate))
               .ToList();

            Console.ReadKey();
        }

        private static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(
            new[] { new ConsoleLoggerProvider((category, level) => level == LogLevel.Information, true) }
        );

        private static SqlCeConnection GetCeConnection()
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();
            return ceConnection;
        }

        private static SqlConnection GetSqlConnection()
        {
            var sqlConnectionString = "Server=localhost;Database=Testbed;Trusted_Connection=True;";
            var sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        private static Expression<Func<T, bool>> GetFilterWithLocalDateTime<T>(DateTime value)
        {
            var itemParam = Expression.Parameter(typeof(T), "i");

            var filter = Expression.MakeBinary(
                    ExpressionType.GreaterThan,
                    Expression.PropertyOrField(itemParam, "Created"),
                    Expression.Constant(value, typeof(DateTime)));

            return Expression.Lambda<Func<T, bool>>(filter, new[] { itemParam });
        }

    }

   
}
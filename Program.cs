using Microsoft.EntityFrameworkCore;
using System.Data.SqlServerCe;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System.Data.SqlClient;

namespace TestHostForCastException
{
    class Program
    {
        public static readonly LoggerFactory WithDebugLogger = new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });
       
        static void Main(string[] args)
        {

            var context = GetContext();
            var accounts = context.Accounts    ;

            var query = (from account in accounts
                         group new
                     {
                         account.IsCredit,
                         Amount = account.Type == 1 ? account.Amount : -account.Amount
                         } by 
                     new
                     {
                         account.Type
                     }
                    into g
                     select new
                     {
                         g.Key.Type,
                         YearToDate = g.Sum(s => s.IsCredit ? s.Amount : -s.Amount)
                     });

            var result = query.ToList();
        }

        private static TestDataContext GetContext()
        {
            //var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            //var ceConnection = new SqlCeConnection(ceConnectionString);
            //ceConnection.Open();

            var sqlConnectionString = "Server=localhost;Database=FruitCake;Trusted_Connection=True;";
            var sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();

            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseLoggerFactory(WithDebugLogger)
                //.UseSqlCe(ceConnection)
                .UseSqlServer(sqlConnection)
                .Options;

            return new TestDataContext(options);
        }

    }
}

using Microsoft.EntityFrameworkCore;
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

            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseSqlCe(ceConnection)
                .Options;

            var context = new TestDataContext(options);

            var employees = context.Set<Employee>();

            var qualified = employees.Select(s => new { s.Id, IsQualified = true });
            var unQualified = employees.Select(s => new { s.Id, IsQualified = false });

            qualified.Union(unQualified).ToList();
        }
    }
}

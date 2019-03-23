using Microsoft.EntityFrameworkCore;
using System.Data.SqlServerCe;

namespace TestHost_2_1
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
            var giraffe = new Giraffe { Name = "Bobby" };
            context.Set<Giraffe>().Add(giraffe);
            context.SaveChanges();
        }
    }
}

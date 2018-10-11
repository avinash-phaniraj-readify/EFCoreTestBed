using System.Data.SqlServerCe;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TestHostInvalidCast
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            using (var ceConnection = new SqlCeConnection(ceConnectionString))
            {
                ceConnection.Open();

                var options = ((DbContextOptionsBuilder)new DbContextOptionsBuilder<DataContext>()
                    .UseSqlCe(ceConnection)).Options;

                using (var context = new DataContext(options))
                {
                    context.InventoryPool.Add(new InventoryPool
                    {
                        ActualCost = 1,
                        Quantity = 2,
                    });
                    context.SaveChanges();

                    var result = context.InventoryPool.Sum(p => (decimal)p.Quantity * p.ActualCost);
                }
            }
        }
    }
}

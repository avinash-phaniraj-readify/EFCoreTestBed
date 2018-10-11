using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestHostInvalidCast.Tests
{
    public abstract class TestBase
    {
        private const string SqlCeConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False;";
        private const string SqlServerConnectionString = "Server=.;Database=Test1;Integrated Security=SSPI;";

        protected void EFCoreSqlCe(Action<DataContext> action)
        {
            using (var connection = new SqlCeConnection(SqlCeConnectionString))
            {
                connection.Open();
                var options = ((DbContextOptionsBuilder)new DbContextOptionsBuilder<DataContext>()
                    .UseSqlCe(connection)).Options;

                ProcessAction(options, action);
            }
        }


        protected void EFCoreSqlServer(Action<DataContext> action)
        {
            using (var connection = new SqlConnection(SqlServerConnectionString))
            {
                connection.Open();
                var options = ((DbContextOptionsBuilder)new DbContextOptionsBuilder<DataContext>()
                    .UseSqlServer(connection)).Options;

                ProcessAction(options, action);
            }
        }

        private void ProcessAction(DbContextOptions options, Action<DataContext> action)
        {
            using (var context = new DataContext(options))
            {
                try
                {
                    context.InventoryPool.Add(new InventoryPool
                    {
                        Quantity = 2,
                    });
                    context.SaveChanges();

                    action(context);
                }
                finally
                {
                    context.InventoryPool.RemoveRange(context.InventoryPool);
                    context.SaveChanges();
                }
            }
        }
    }
}

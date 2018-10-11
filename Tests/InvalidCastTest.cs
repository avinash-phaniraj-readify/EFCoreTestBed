using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestHostInvalidCast.Tests
{
    public class InvalidCastTest: TestBase
    {
        [Fact]
        public void SqlCe_InvalidOperationException_Thrown()
        {
            EFCoreSqlCe(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.Set<InventoryPool>().Sum(p => (decimal)p.Quantity);
                });
            });
        }

        [Fact]
        public void SqlServer_InvalidOperationException_Thrown()
        {
            EFCoreSqlServer(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.InventoryPool.Sum(p => (decimal) p.Quantity);
                });
            });

            EFCoreSqlServer(context =>
            {
                context.InventoryPool.RemoveRange(context.InventoryPool);
                context.SaveChanges();
            });
        }

    }
}

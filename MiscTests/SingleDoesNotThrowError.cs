using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class SingleDoesNotThrowError : TestBase
    {
        public SingleDoesNotThrowError(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Linq2SqlTest()
        {
            base.Linq2SqlContext(context => {
                var results = context.GetTable<Linq2Sql.Employee>()
                .Select(employee => new { Result = employee.EmployeeDevices.Single(s=> s.Id < 0) })
                .ToList();

                Assert.True(results.Any());
                Assert.True(results.All(a=> a.Result == null));
            });
        }

        [Fact]
        public void EFCoreTest()
        {
            base.EFContext(context => {

                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    var results = context.Set<EFCore.Employee>()
                    .Select(employee => new { Result = employee.Devices.Single(s => s.Id < 0) })
                    .ToList();
                });

                Assert.Equal("Sequence contains no elements", exception.Message);
            });
        }

        [Fact]
        public void EFCoreTest_Fixed()
        {
            base.EFContext(context => {

                var results = context.Set<EFCore.Employee>()
                   .Select(employee => new { Result = employee.Devices.FirstOrDefault(s => s.Id < 0) })
                   .ToList();

                Assert.True(results.Any());
                Assert.True(results.All(a => a.Result == null));
            });
        }
    }
}

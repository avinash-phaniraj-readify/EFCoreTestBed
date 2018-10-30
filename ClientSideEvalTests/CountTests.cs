using System;
using System.Linq;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class CountTests : TestBase
    {
        public CountTests(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void EFCoreTest_FirstOrDefaultInCountPops()
        {
            base.EFContext(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Count(x => x.Devices.FirstOrDefault() != null);
                });
            });
        }
    }
}

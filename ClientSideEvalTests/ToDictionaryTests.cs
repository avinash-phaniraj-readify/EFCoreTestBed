using System.Linq;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class ToDictionaryTests : TestBase
    {
        public ToDictionaryTests(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void EFCoreTest_ToDictionaryWorksInMemory()
        {
            base.EFContext(context =>
            {
                var result = context.Set<EFCore.Employee>().ToDictionary(x => x.Name, x => x.Created);
            });
        }
    }
}

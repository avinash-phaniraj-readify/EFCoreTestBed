using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class NullableTypeInConstructor : TestBase
    {
        public NullableTypeInConstructor(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Linq2SqlTest()
        {
            base.Linq2SqlContext(context => {
                var results = context.GetTable<Linq2Sql.Employee>()
                .Select(employee => new TestClass(employee.EmployeeDetails.Id))
                .ToList();

                Assert.True(results.Any());
            });
        }

        [Fact]
        public void EFCoreTest()
        {
            base.EFContext(context => {

                var exception = Assert.Throws<ArgumentException>(() => 
                {
                    context.Set<EFCore.Employee>()
                    .Select(employee => new TestClass(employee.EmployeeDetails.Id))
                    .ToList();
                });

                Assert.Equal("Expression of type 'System.Nullable`1[System.Int32]' cannot be used for constructor parameter of type 'System.Int32'", exception.Message);

            });
        }

        [Fact]
        public void EFCoreTest_Workaround()
        {
            base.EFContext(context => {

                var results = context.Set<EFCore.Employee>()
                    .Select(employee => new TestClass((int)employee.EmployeeDetails.Id))
                    .ToList();

                Assert.True(results.Any());

            });
        }

        public class TestClass
        {
            public TestClass(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }

    }

}

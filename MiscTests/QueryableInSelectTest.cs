using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    //When a queryable is handed to a select and exercised within a method 
    //EF Fails and Linq2Sql is ok
    public class QueryableInSelectTest : TestBase
    {
        public QueryableInSelectTest(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Linq2SqlTest()
        {
            base.Linq2SqlContext(context => {

                var employeeDevicesQueryable = context.GetTable<Linq2Sql.EmployeeDevice>();
                var result = context.GetTable<Linq2Sql.Employee>().Select(employee => TestMethod(employee, employeeDevicesQueryable)).ToList();

                Assert.True(result.Any());
            });
        }

        [Fact]
        public void EFCoreTest()
        {
            base.EFContext(context => {
                var employeeDevicesQueryable = context.Set<EFCore.EmployeeDevice>();

                Assert.Throws<ArgumentNullException>(() => 
                {
                    context.Set<EFCore.Employee>().Select(employee => TestMethod(employee, employeeDevicesQueryable)).ToList();
                });
            });
        }

        private string TestMethod(EFCore.Employee employee, IQueryable<EFCore.EmployeeDevice> employeeDevicesQueryable)
        {
            return employee.Name + employeeDevicesQueryable.Where(w=> w.EmployeeId == employee.Id).Count();
        }

        private string TestMethod(Linq2Sql.Employee employee, IQueryable<Linq2Sql.EmployeeDevice> employeeDevicesQueryable)
        {
            return employee.Name + employeeDevicesQueryable.Where(w => w.EmployeeId == employee.Id).Count();
        }

    }

    
}

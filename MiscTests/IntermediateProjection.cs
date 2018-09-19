using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class IntermediateProjection : TestBase
    {
        public IntermediateProjection(DatabaseFixture fixture) : base(fixture)
        {
            //todo: avinash - repro not complete !
        }

        [Fact]
        public void Linq2SqlTest()
        {
            base.Linq2SqlContext(context => {
                var intermediateQuery = from employee in context.GetTable<Linq2Sql.Employee>()
                                        join detail in context.GetTable<Linq2Sql.EmployeeDetail>() on employee.Id equals detail.EmployeeId
                                        select new IntermediateProjection_LinqHolder { Employee = employee, Detail = detail };

                var query = from data in intermediateQuery
                            join device in context.GetTable<Linq2Sql.EmployeeDevice>() on data.Employee.Id equals device.EmployeeId
                            select new { data.Employee.Id, data.Employee.Name, data.Detail.Details, device.Device };

                var results = query.ToList();

                Assert.True(results.Any());
            });
        }

        [Fact]
        public void EFCoreTest()
        {
            base.EFContext(context => {

                var intermediateQuery = from employee in context.Set<EFCore.Employee>()
                                        join detail in context.Set<EFCore.EmployeeDetails>() on employee.Id equals detail.EmployeeId
                                        select new IntermediateProjection_EfHolder { Employee = employee, Detail = detail };

                var query = from data in intermediateQuery
                            join device in context.Set<EFCore.EmployeeDevice>() on data.Employee.Id equals device.EmployeeId
                            select new { data.Employee.Id, data.Employee.Name, data.Detail.Details, device.Device };

                var results = query.ToList();

                Assert.True(results.Any());
            });
        }

    }

    public class IntermediateProjection_LinqHolder 
    {
        public Linq2Sql.Employee Employee { get; set; }
        public Linq2Sql.EmployeeDetail Detail { get; set; }
    }

    public class IntermediateProjection_EfHolder
    {
        public EFCore.Employee Employee { get; set; }
        public EFCore.EmployeeDetails Detail { get; set; }
    }
}

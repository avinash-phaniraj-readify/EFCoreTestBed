using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.AttachTests
{
    /// <summary>
    /// When entities are loaded from the database 
    /// and the very same entities are attached to the context
    /// the original entities are replaced.
    /// </summary>
    public class Scenario3 : TestBase
    {
        public Scenario3(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Linq2SqlWriteTest()
        {
            base.Linq2SqlContext(context => {
                //load entities
                var originalEmployee = context.Employees.First(f => f.Id == 2);
                var originalEmployeeDevices = originalEmployee.EmployeeDevices.ToList();
                var originalEmployeeDevice1 = originalEmployeeDevices.First(f => f.Id == 2);
                var originalEmployeeDevice2 = originalEmployeeDevices.First(f => f.Id == 2);

                //build graph
                var attachedEmployee = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var attachedEmployeeDevice1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                attachedEmployee.EmployeeDevices.Add(attachedEmployeeDevice1);

                //Attach a complete graph
                Assert.Throws<DuplicateKeyException>(() => context.Employees.Attach(attachedEmployee));
 
                //attach duplicate child entity
                Assert.Throws<DuplicateKeyException>(() => context.EmployeeDevices.Attach(attachedEmployeeDevice1));

                //mutate child.
                attachedEmployee.Name = "new name";
                attachedEmployeeDevice1.Device = "Updated";

                //works!
                context.SubmitChanges();

                var updatedEmployeeName = base.Connection.QuerySingle<string>("select name from employee where id = 2");
                var updatedEmployeeDevice1 = base.Connection.QuerySingle<string>("select device from employeedevice where id = 2");
                Assert.Equal("new name", updatedEmployeeName);
                Assert.Equal("Updated", updatedEmployeeDevice1);
            });
        }

        [Fact]
        public void Linq2SqlReadTest()
        {
            base.Linq2SqlContext(context => {
                //load entities
                var originalEmployee = context.Employees.First(f => f.Id == 2);
                var originalEmployeeDevices = originalEmployee.EmployeeDevices.ToList();
                var originalEmployeeDevice1 = originalEmployeeDevices.First(f => f.Id == 2);
                var originalEmployeeDevice2 = originalEmployeeDevices.First(f => f.Id == 2);

                //build graph
                var attachedEmployee = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var attachedEmployeeDevice1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                attachedEmployee.EmployeeDevices.Add(attachedEmployeeDevice1);

                //Attach a complete graph
                Assert.Throws<DuplicateKeyException>(() => context.Employees.Attach(attachedEmployee));

                //attach duplicate child entity
                Assert.Throws<DuplicateKeyException>(() => context.EmployeeDevices.Attach(attachedEmployeeDevice1));

                //mutate attached graph.
                attachedEmployee.Name = "new name";
                attachedEmployeeDevice1.Device = "Updated";

                //query and verify data
                var updatedEmployeeList = context.Employees.Where(f => f.Id == 2).ToList();
                var updatedEmployeeDevices = context.EmployeeDevices.Where(f => f.EmployeeId == 2).ToList();

                //original data is updated
                Assert.Single(updatedEmployeeList);
                Assert.Equal("new name", updatedEmployeeList.First().Name);

                Assert.Equal(2, updatedEmployeeDevices.Count);
                Assert.Equal("Updated", updatedEmployeeDevices.First().Device);
                Assert.Equal("Laptop", updatedEmployeeDevices.Last().Device);

                Assert.True(updatedEmployeeList.First() == originalEmployee);
                Assert.True(updatedEmployeeDevices.First() == originalEmployeeDevice1);

                //save data 
                context.SubmitChanges();

                var updatedEmployeeName = base.Connection.QuerySingle<string>("select name from employee where id = 2");
                var updatedEmployeeDevice1 = base.Connection.QuerySingle<string>("select device from employeedevice where id = 2");
                Assert.Equal("new name", updatedEmployeeName);
                Assert.Equal("Updated", updatedEmployeeDevice1);
            });
        }

    }
}

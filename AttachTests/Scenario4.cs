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
    /// When an entity graph is attached and subsequently
    /// a duplicate entity/graph is attached 
    /// both entities are tracked
    /// </summary>
    public class Scenario4 : TestBase
    {
        public Scenario4(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Linq2SqlWriteTest()
        {
            base.Linq2SqlContext(context => {
                //build graph
                var attachedEmployee = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var attachedEmployeeDevice1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                attachedEmployee.EmployeeDevices.Add(attachedEmployeeDevice1);

                //Attach a complete graph
                context.Employees.Attach(attachedEmployee);

                //build duplicate graph
                var duplicateAttachedEmployee = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var duplicateAttachedEmployeeDevice1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                duplicateAttachedEmployee.EmployeeDevices.Add(duplicateAttachedEmployeeDevice1);
                
                //attach duplicate entity
                Assert.Throws<DuplicateKeyException>(() => context.Employees.Attach(duplicateAttachedEmployee));
                //attach duplicate child entity
                context.EmployeeDevices.Attach(duplicateAttachedEmployeeDevice1);

                //mutate attached entities.
                duplicateAttachedEmployee.Name = "new name";
                duplicateAttachedEmployeeDevice1.Device = "Updated";

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
                //build graph
                var attachedEmployee = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var attachedEmployeeDevice1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                var attachedEmployeeDevice2 = new Linq2Sql.EmployeeDevice { Device = "Laptop", Id = 3, EmployeeId = 2 };
                attachedEmployee.EmployeeDevices.Add(attachedEmployeeDevice1);
                attachedEmployee.EmployeeDevices.Add(attachedEmployeeDevice2);

                //attach graph
                context.Employees.Attach(attachedEmployee);

                //build duplicate graph
                var duplicateAttachedEmployee = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var duplicateAttachedEmployeeDevice1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                duplicateAttachedEmployee.EmployeeDevices.Add(duplicateAttachedEmployeeDevice1);

                //attach duplicate entity
                Assert.Throws<DuplicateKeyException>(() => context.Employees.Attach(duplicateAttachedEmployee));
                //attach duplicate child entity
                context.EmployeeDevices.Attach(duplicateAttachedEmployeeDevice1);

                //mutate attached entities.
                duplicateAttachedEmployee.Name = "new name";
                duplicateAttachedEmployeeDevice1.Device = "Updated";

                //query and verify data
                var updatedEmployeeList = context.Employees.Where(f => f.Id == 2).ToList();
                var updatedEmployeeDevices = context.EmployeeDevices.Where(f => f.EmployeeId == 2).ToList();

                //original data is not updated
                Assert.Single(updatedEmployeeList);
                Assert.Equal("Bar", updatedEmployeeList.First().Name);

                Assert.Equal(2, updatedEmployeeDevices.Count);
                //original data is updated
                Assert.Equal("Updated", updatedEmployeeDevices.First().Device);
                Assert.Equal("Laptop", updatedEmployeeDevices.Last().Device);

                Assert.True(updatedEmployeeList.First() == attachedEmployee);
                Assert.True(updatedEmployeeDevices.First() != attachedEmployeeDevice1);

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

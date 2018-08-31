using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Linq;
using System.Linq;
using Xunit;


namespace Linq2SqlEFCoreBehaviorsTest.AttachTests
{
    /// <summary>
    /// Attaching a duplicate child entity after loading one from the database
    /// </summary>
    public class Scenario2 : TestBase
    {
        public Scenario2(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Linq2SqlTest()
        {
            base.Linq2SqlContext(context => {
                var originalEmployee = context.Employees.First(f=> f.Id == 2);
                var originalEmployeeDevices = originalEmployee.EmployeeDevices.ToList();

                var e1 = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var ed1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                e1.EmployeeDevices.Add(ed1);

                //Attach a complete graph
                Assert.Throws<DuplicateKeyException>(() => context.Employees.Attach(e1));

                //attach duplicate child entity
                Assert.Throws<DuplicateKeyException>(() => context.EmployeeDevices.Attach(ed1));

                //mutate child.
                ed1.Device = "Updated";

                //works!
                context.SubmitChanges();

                var result = base.Connection.QuerySingle<string>("select device from employeedevice where id = 2");
                Assert.Equal("Updated", result);
            });
        }

        [Fact]
        public void EFCoreTest()
        {
            base.EFContext(context => {
                var originalEmployee = context.Set<EFCore.Employee>().First(f => f.Id == 2);
                var originalEmployeeDevices = originalEmployee.Devices.ToList();

                var e1 = new Linq2Sql.Employee { Name = "Bar", Id = 2 };
                var ed1 = new Linq2Sql.EmployeeDevice { Device = "Phone", Id = 2, EmployeeId = 2 };
                e1.EmployeeDevices.Add(ed1);

                //Attach a complete graph
                Assert.Throws<InvalidOperationException>(() => context.Attach(e1));

                //attach duplicate child entity
                Assert.Throws<InvalidOperationException>(() => context.Attach(ed1));

                //mutate child.
                ed1.Device = "Updated";

                //Bhoom!
                context.SaveChanges();

                var result = base.Connection.QuerySingle<string>("select device from employeedevice where id = 2");
                Assert.Equal("Updated", result);
            });
        }
    }
}

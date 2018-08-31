using System;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.AttachTests
{
    /// <summary>
    /// Attaching a duplicate child entity
    /// </summary>
    public class Scenario1 : TestBase
    {
        public Scenario1(DatabaseFixture fixture): base(fixture)
        {
        }

        [Fact]
        public void Linq2SqlTest()
        {
            base.Linq2SqlContext(context => {
                var e1 = new Linq2Sql.Employee { Name = "Test", Id = 2 };
                var ed1 = new Linq2Sql.EmployeeDevice { Device = "Test Device", Id = 2, EmployeeId = 2 };
                var ed11 = new Linq2Sql.EmployeeDevice { Device = "Test Device", Id = 2, EmployeeId = 2 };
                e1.EmployeeDevices.Add(ed1);

                //Attach a complete graph
                context.Employees.Attach(e1);
                //mutate child entity
                ed1.Device = "new dev";

                //attach duplicate child entity
                context.EmployeeDevices.Attach(ed11);
                //delete duplicate child entity
                context.EmployeeDevices.DeleteOnSubmit(ed11);

                //works!
                context.SubmitChanges();
            });
        }

        [Fact]
        public void EFCoreTest()
        {
            base.EFContext(context => {
                var e1 = new EFCore.Employee { Name = "Test", Id = 2 };
                var ed1 = new EFCore.EmployeeDevice { Device = "Test Device", Id = 2, EmployeeId = 2 };
                var ed11 = new EFCore.EmployeeDevice { Device = "Test Device", Id = 2, EmployeeId = 2 };
                e1.Devices.Add(ed1);

                //Attach a complete graph
                context.Attach(e1);
                //mutate child entity
                ed1.Device = "new dev";

                //attach duplicate child entity

                var ex = Assert.Throws<InvalidOperationException>(() => context.Attach(ed11));
                Assert.Equal("The instance of entity type 'EmployeeDevice' cannot be tracked because another instance with the key value '{Id: 2}' is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached.", ex.Message);

                //delete duplicate child entity
                context.Remove(ed11);

                //works!
                context.SaveChanges();
            });
        }
    }
}

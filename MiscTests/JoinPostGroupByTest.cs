using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class JoinPostGroupByTest : TestBase
    {
        public JoinPostGroupByTest(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Linq2SqlTestLinqQuery()
        {
            base.Linq2SqlContext(context => {

                var q1 = from employee in context.GetTable<Linq2Sql.Employee>()
                        join device in context.GetTable<Linq2Sql.EmployeeDevice>() on employee.Id equals device.EmployeeId into emptyDevice
                        from theDevice in emptyDevice.DefaultIfEmpty()
                        group theDevice by new
                        {
                            employee.Id
                        } into s
                        select new
                        {
                            Id = s.Key.Id,
                            DeviceCount = s.Count(a=> a.Device != null)
                        };

                var q2 = from q in q1
                         join details in context.GetTable<Linq2Sql.EmployeeDetail>() on q.Id equals details.EmployeeId
                         select new { q.Id, details.Details, q.DeviceCount };

                var results = q2.ToList();

                Assert.True(results.Any());
            });
        }

        [Fact]
        public void EfCoreTestLinqQuery_FailsTranslatingDefaultIfEmptyToSql()
        {
            base.EFContext(context => {

                var q1 = from employee in context.Set<EFCore.Employee>()
                         join device in context.Set<EFCore.EmployeeDevice>() on employee.Id equals device.EmployeeId into emptyDevice
                         from theDevice in emptyDevice.DefaultIfEmpty()
                         group theDevice by new
                         {
                             employee.Id,
                         } into s
                         select new
                         {
                             Id = s.Key.Id,
                            DeviceCount = s.Count(a=> a.Device != null)
                         };

                var q2 = from q in q1
                         join details in context.Set<EFCore.EmployeeDetails>() on q.Id equals details.EmployeeId
                         select new { q.Id, details.Details, q.DeviceCount };

                var exception = Assert.Throws<InvalidOperationException>(() => q2.ToList());


                Assert.Equal("Error generated for warning 'Microsoft.EntityFrameworkCore.Query.QueryClientEvaluationWarning: The LINQ expression 'DefaultIfEmpty()' could not be translated and will be evaluated locally.'. This exception can be suppressed or logged by passing event ID 'RelationalEventId.QueryClientEvaluationWarning' to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.", exception.Message);

            });
        }

        [Fact]
        public void EfCoreTestLinqQuery_FailsWithInvalidSelectClause()
        {
            base.EFContext(context => {

                var q1 = from employee in context.Set<EFCore.Employee>()
                         join device in context.Set<EFCore.EmployeeDevice>() on employee.Id equals device.EmployeeId into emptyDevice
                         from theDevice in emptyDevice.DefaultIfEmpty()
                         select new { employee.Id, theDevice.Device };

                var q2 = from q in q1 
                         group q by new
                         {
                             q.Id,
                         } into s
                         select new
                         {
                             Id = s.Key.Id,
                             DeviceCount = s.Count(a=> a.Device != null)
                         };

                var q3 = from q in q2
                         join details in context.Set<EFCore.EmployeeDetails>() on q.Id equals details.EmployeeId
                         select new { q.Id, details.Details };

                var exception = Assert.Throws<SqlCeException>(() => q3.ToList());

                Assert.Equal("In aggregate and grouping expressions, the SELECT clause can contain only aggregates and grouping expressions. [ Select clause = details,Details ]", exception.Message);
            });
        }

        [Fact]
        public void EfCoreTestLinqQuery_FailsWhenCountIsUsed()
        {
            base.EFContext(context => {

                var q1 = from employee in context.Set<EFCore.Employee>()
                         join device in context.Set<EFCore.EmployeeDevice>() on employee.Id equals device.EmployeeId into emptyDevice
                         from theDevice in emptyDevice.DefaultIfEmpty()
                         select new { employee.Id, theDevice.Device };

                var q2 = from q in q1
                         group q by new
                         {
                             q.Id,
                         } into s
                         select new
                         {
                             Id = s.Key.Id,
                             DeviceCount = s.Count(a=> a.Device != null)
                         };

                var q3 = from q in q2
                         join details in context.Set<EFCore.EmployeeDetails>() on q.Id equals details.EmployeeId
                         select new { q.Id, details.Details };

                var exception = Assert.Throws<InvalidOperationException>(() => q3.ToList());

                Assert.Equal("Error generated for warning 'Microsoft.EntityFrameworkCore.Query.QueryClientEvaluationWarning: The LINQ expression 'GroupBy(new <>f__AnonymousType3`1(Id = [employee].Id), new <>f__AnonymousType6`2(Id = [employee].Id, Device = [theDevice]?.Device))' could not be translated and will be evaluated locally.'. This exception can be suppressed or logged by passing event ID 'RelationalEventId.QueryClientEvaluationWarning' to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.", exception.Message);
            });
        }


        [Fact]
        public void EfCoreTestLinqQuery_FailsWithIndexOutOfRange()
        {
            base.EFContext(context => {

                var q1 = from employee in context.Set<EFCore.Employee>()
                         join device in context.Set<EFCore.EmployeeDevice>() on employee.Id equals device.EmployeeId into emptyDevice
                         from theDevice in emptyDevice.DefaultIfEmpty()
                         select new { employee.Id, theDevice.Device };

                var q2 = from q in q1
                         group q by new
                         {
                             q.Id,
                         } into s
                         select new
                         {
                             Id = s.Key.Id,
                             DeviceCount = s.Count(a => a.Device != null)
                         };

                var q3 = from q in q2
                         join details in context.Set<EFCore.EmployeeDetails>() on q.Id equals details.EmployeeId
                         select new { q.Id, details };

                var exception = Assert.Throws<IndexOutOfRangeException>(() => q3.ToList());
            });
        }
    }
}

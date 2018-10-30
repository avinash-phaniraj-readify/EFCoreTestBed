using System;
using System.Data.SqlServerCe;
using System.Linq;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class AnyTests : TestBase
    {
        public AnyTests(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Linq2SqlTest()
        {
            base.Linq2SqlContext(context =>
            {
                var result = context.GetTable<Linq2Sql.Employee>()
                    .Where(x => x.EmployeeDevices.Any(y => y.DeviceId == 3))
                    .ToList();
            });

            /*
            SELECT [t0].[Id], [t0].[Name], [t0].[Created]
            FROM [Employee] AS [t0]
            WHERE EXISTS(
                SELECT NULL AS [EMPTY]
                FROM [EmployeeDevice] AS [t1]
                WHERE ([t1].[DeviceId] = @p0) AND ([t1].[EmployeeId] = [t0].[Id])
                )
            -- @p0: Input Int32 (Size = 0; Prec = 0; Scale = 0) [3]
            -- Context: SqlProvider(SqlCE) Model: AttributedMetaModel Build: 4.7.3056.0 
            */
        }

        [Fact]
        public void EFCoreTest_UnionWorksInMemory()
        {
            base.EFContext(context =>
            {
                var query1 = context.Set<EFCore.Employee>().Where(x => x.Name == "Michael");
                var query2 = context.Set<EFCore.Employee>().Where(x => x.Name == "Avi");
                var result = query1.Union(query2).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_ConcatWorksInMemory()
        {
            base.EFContext(context =>
            {
                var query1 = context.Set<EFCore.Employee>().Where(x => x.Name == "Michael");
                var query2 = context.Set<EFCore.Employee>().Where(x => x.Name == "Avi");
                var result = query1.Concat(query2).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_IntersectWorksInMemory()
        {
            base.EFContext(context =>
            {
                var query1 = context.Set<EFCore.Employee>().Where(x => x.Name == "Michael");
                var query2 = context.Set<EFCore.Employee>().Where(x => x.Name == "Avi");
                var result = query1.Intersect(query2).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_ExceptWorksInMemory()
        {
            base.EFContext(context =>
            {
                var query1 = context.Set<EFCore.Employee>().Where(x => x.Name == "Michael");
                var query2 = context.Set<EFCore.Employee>().Where(x => x.Name == "Avi");
                var result = query1.Except(query2).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_FirstOrDefaultPops()
        {
            base.EFContext(context =>
            {
                Assert.Throws<SqlCeException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.FirstOrDefault() != null).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_FirstInWhereResultsInSelectNPlus1()
        {
            base.EFContext(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.First() != null).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_FirstOrDefaultInWherePops()
        {
            base.EFContext(context =>
            {
                Assert.Throws<SqlCeException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.FirstOrDefault() != null).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_LastInWherePopsDueToBug()
        {
            base.EFContext(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Last() != null).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_LastOrDefaultInWherePopsDueToBug()
        {
            base.EFContext(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.LastOrDefault() != null).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_SingleInWhereResultsInSelectNPlus1()
        {
            base.EFContext(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Single() != null).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_SingleOrDefaultInWhereResultsInSelectNPlus1()
        {
            base.EFContext(context =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.SingleOrDefault() != null).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_MinInWhereResultsInSelectNPlus1()
        {
            base.EFContext(context =>
            {
                var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Any() && x.Devices.Min(y => y.Id) > 0).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_MaxInWhereResultsInSelectNPlus1()
        {
            base.EFContext(context =>
            {
                var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Any() && x.Devices.Max(y => y.Id) > 0).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_SumInWherePops()
        {
            base.EFContext(context =>
            {
                Assert.Throws<SqlCeException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Any() && x.Devices.Sum(y => y.Id) > 0).ToList();
                });
            });
        }

        [Fact]
        public void EFCoreTest_AverageInWhereResultsInSelectNPlus1()
        {
            base.EFContext(context =>
            {
                var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Any() && x.Devices.Average(y => y.Id) > 0).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_CountPops()
        {
            base.EFContext(context =>
            {
                Assert.Throws<SqlCeException>(() =>
                {
                    var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Count() > 0).ToList();
                });                
            });
        }

        [Fact]
        public void EFCoreTest_DistinctWorksInMemory()
        {
            base.EFContext(context =>
            {
                var result = context.Set<EFCore.Employee>().Distinct().ToList();
            });
        }
    }
}

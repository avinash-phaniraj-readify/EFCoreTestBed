using System;
using System.Data.SqlServerCe;
using System.Linq;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.ClientSideEvalTests
{
    public class WhereTests : TestBase
    {
        public WhereTests(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void EFCoreTest_AnyInWhereWorks()
        {
            base.EFContext(context =>
            {
                var result = context.Set<EFCore.Employee>().Where(x => x.Devices.Any(y => y.Device == "Laptop")).ToList();
            });
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
        public void EFCoreTest_CountInWherePops()
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
        public void EFCoreTest_CoaleseceFirstOrDefaultInWhereWorks()
        {
            base.EFContext(context =>
            {
                var result = context.Set<EFCore.Employee>().Where(x => x.Devices.FirstOrDefault().EmployeeId == 1).ToList();
            });
        }

        [Fact]
        public void Linq2SqlTest_CompareToEquals()
        {
            base.Linq2SqlContext(context =>
            {
                var name = "Michael";
                var result = context.GetTable<Linq2Sql.Employee>().Where(x => x.Name.CompareTo(name) == 0).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_CompareToEquals()
        {
            base.EFContext(context =>
            {
                var name = "Michael";
                var result = context.Set<EFCore.Employee>().Where(x => x.Name.CompareTo(name) == 0).ToList();
            });
        }

        [Fact]
        public void Linq2SqlTest_CompareToGreaterThanEqualsTo()
        {
            base.Linq2SqlContext(context =>
            {
                var name = "Michael";
                var result = context.GetTable<Linq2Sql.Employee>().Where(x => x.Name.CompareTo(name) >= 0).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_CompareToGreaterThanEqualsTo()
        {
            base.EFContext(context =>
            {
                var name = "Michael";
                var result = context.Set<EFCore.Employee>().Where(x => x.Name.CompareTo(name) >= 0).ToList();
            });
        }

        [Fact]
        public void Linq2SqlTest_CompareToLessThanEqualsTo()
        {
            base.Linq2SqlContext(context =>
            {
                var name = "Michael";
                var result = context.GetTable<Linq2Sql.Employee>().Where(x => x.Name.CompareTo(name) <= 0).ToList();
            });
        }

        [Fact]
        public void EFCoreTest_CompareToLessThanEqualsTo()
        {
            base.EFContext(context =>
            {
                var name = "Michael";
                var result = context.Set<EFCore.Employee>().Where(x => x.Name.CompareTo(name) <= 0).ToList();
            });
        }
    }
}

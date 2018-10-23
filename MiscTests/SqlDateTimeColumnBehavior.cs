using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest.MiscTests
{
    public class SqlDateTimeColumnBehavior : TestBase
    {
        public SqlDateTimeColumnBehavior(DatabaseFixture fixture) : base(fixture)
        {

        }

        private DateTime _theLocalDate = DateTime.SpecifyKind(new DateTime(2010, 10, 10), DateTimeKind.Local);
        private DateTime _theUnSpecifiedDate = new DateTime(2010, 10, 10);

        [Fact]
        public void Linq2SqlTestLinqQuery()
        {
            base.Linq2SqlContext(context => {
                var results = context.GetTable<Linq2Sql.Employee>()
                .Where(w=> w.Created > _theLocalDate)
                .ToList();

                Assert.True(results.Any());

                /*
                 
                --CE
                 SELECT [t0].[Id], [t0].[Name], [t0].[Created]
                    FROM [Employee] AS [t0]
                    WHERE [t0].[Created] > @p0
                    -- @p0: Input DateTime (Size = 0; Prec = 0; Scale = 0) [10/10/2010 12:00:00 AM]
                    -- Context: SqlProvider(SqlCE) Model: AttributedMetaModel Build: 4.7.3056.0
                 
                --SQL
                SELECT [t0].[Id], [t0].[Name], [t0].[Created]
                    FROM [Employee] AS [t0]
                    WHERE [t0].[Created] > @p0
                    -- @p0: Input DateTime (Size = -1; Prec = 0; Scale = 0) [10/10/2010 12:00:00 AM]
                    -- Context: SqlProvider(Sql2008) Model: AttributedMetaModel Build: 4.7.3056.0
                 */
            });
        }


        [Fact]
        public void Linq2SqlTestLinqDynamic()
        {
            base.Linq2SqlContext(context => {

                var results = context.GetTable<Linq2Sql.Employee>()
                .Where(GetFilterWithLocalDateTime<Linq2Sql.Employee>(_theLocalDate))
                .ToList();

                Assert.True(results.Any());

                /*
                 
                --CE
                    SELECT [t0].[Id], [t0].[Name], [t0].[Created]
                    FROM [Employee] AS [t0]
                    WHERE [t0].[Created] > @p0
                    -- @p0: Input DateTime (Size = 0; Prec = 0; Scale = 0) [10/10/2010 12:00:00 AM]
                    -- Context: SqlProvider(SqlCE) Model: AttributedMetaModel Build: 4.7.3056.0
                 
                --SQL
                SELECT [t0].[Id], [t0].[Name], [t0].[Created]
                    FROM [Employee] AS [t0]
                    WHERE [t0].[Created] > @p0
                    -- @p0: Input DateTime (Size = -1; Prec = 0; Scale = 0) [10/10/2010 12:00:00 AM]
                    -- Context: SqlProvider(Sql2008) Model: AttributedMetaModel Build: 4.7.3056.0
                
                 */
            });
        }

        [Fact]
        public void EFCoreTestLinqQuery_LocalDate()
        {
            base.EFContext(context => {

                var results = context.Set<EFCore.Employee>()
                .Where(w => w.Created > _theLocalDate)
                .ToList();

                Assert.True(results.Any());

                /*
                 
                ---CE without datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (58ms) [Parameters=[@___theLocalDate_0='2010-10-10T00:00:00' (DbType = DateTime)], CommandType='Text', CommandTimeout='0']
                    SELECT [w].[Id], [w].[Created], [w].[Name]
                    FROM [Employee] AS [w]
                    WHERE [w].[Created] > @___theLocalDate_0

                ---CE with datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (62ms) [Parameters=[@___theLocalDate_0='2010-10-10T00:00:00' (DbType = DateTime)], CommandType='Text', CommandTimeout='0']
                    SELECT [w].[Id], [w].[Created], [w].[Name]
                    FROM [Employee] AS [w]
                    WHERE [w].[Created] > @___theLocalDate_0
                 
                ---SQL without datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (113ms) [Parameters=[@___theLocalDate_0='2010-10-10T00:00:00'], CommandType='Text', CommandTimeout='30']
                    SELECT [w].[Id], [w].[Created], [w].[Name]
                    FROM [Employee] AS [w]
                    WHERE [w].[Created] > @___theLocalDate_0

                ---SQL with datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (95ms) [Parameters=[@___theLocalDate_0='2010-10-10T00:00:00' (DbType = DateTime)], CommandType='Text', CommandTimeout='30']
                    SELECT [w].[Id], [w].[Created], [w].[Name]
                    FROM [Employee] AS [w]
                    WHERE [w].[Created] > @___theLocalDate_0
                 
                 */

            });
        }

        [Fact]
        public void EFCoreTestLinqQuery_DateInline()
        {
            base.EFContext(context => {

                var results = context.Set<EFCore.Employee>()
                .Where(w => w.Created > new DateTime(2010, 10, 10))
                .ToList();

                Assert.True(results.Any());

                /*
                 * 
                ---CE without datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (48ms) [Parameters=[], CommandType='Text', CommandTimeout='0']
                    SELECT [w].[Id], [w].[Created], [w].[Name]
                    FROM [Employee] AS [w]
                    WHERE [w].[Created] > '2010-10-10T00:00:00.000'

                ---CE with datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (53ms) [Parameters=[], CommandType='Text', CommandTimeout='0']
                        SELECT [w].[Id], [w].[Created], [w].[Name]
                        FROM [Employee] AS [w]
                        WHERE [w].[Created] > '2010-10-10T00:00:00.000'
                 
                --SQL without datetime type specified --> Error
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (60ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                    SELECT [w].[Id], [w].[Created], [w].[Name]
                    FROM [Employee] AS [w]
                    WHERE [w].[Created] > '2010-10-10T00:00:00.0000000'

                --SQL with datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (63ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                    SELECT [w].[Id], [w].[Created], [w].[Name]
                    FROM [Employee] AS [w]
                    WHERE [w].[Created] > '2010-10-10T00:00:00.000'
            */

            });
        }


        [Fact]
        public void EFCoreTestLinqQuery_DynamicWithLocalDateTime()
        {
            base.EFContext(context => {

                var results = context.Set<EFCore.Employee>()
                .Where(GetFilterWithLocalDateTime<EFCore.Employee>(_theLocalDate))
                .ToList();

                Assert.True(results.Any());

                /*
                 
                ---CE without datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (43ms) [Parameters=[], CommandType='Text', CommandTimeout='0']
                SELECT [i].[Id], [i].[Created], [i].[Name]
                    FROM [Employee] AS [i]
                    WHERE [i].[Created] > '2010-10-10T00:00:00.000'

                ---CE with datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='0']
                SELECT [i].[Id], [i].[Created], [i].[Name]
                    FROM [Employee] AS [i]
                    WHERE [i].[Created] > '2010-10-10T00:00:00.000'
                 
                --SQL without datetime type specified --> Error
                SELECT [i].[Id], [i].[Created], [i].[Name]
                    FROM [Employee] AS [i]
                    WHERE [i].[Created] > '2010-10-10T00:00:00.0000000+11:00'

                --SQL with datetime type specified --> Error
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (79ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                SELECT [i].[Id], [i].[Created], [i].[Name]
                FROM [Employee] AS [i]
                WHERE [i].[Created] > '2010-10-10T00:00:00.000+11:00'
                
            */

            });
        }

        [Fact]
        public void EFCoreTestLinqQuery_DynamicUnSpecifiedDateTime()
        {
            base.EFContext(context => {

                var results = context.Set<EFCore.Employee>()
                .Where(GetFilterWithLocalDateTime<EFCore.Employee>(_theUnSpecifiedDate))
                .ToList();

                Assert.True(results.Any());

                /*
                 
                ---CE without datetime type specified
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (44ms) [Parameters=[], CommandType='Text', CommandTimeout='0']
                SELECT [i].[Id], [i].[Created], [i].[Name]
                FROM [Employee] AS [i]
                WHERE [i].[Created] > '2010-10-10T00:00:00.000'

                ---CE with datetime type specified
               Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (48ms) [Parameters=[], CommandType='Text', CommandTimeout='0']
                SELECT [i].[Id], [i].[Created], [i].[Name]
                FROM [Employee] AS [i]
                WHERE [i].[Created] > '2010-10-10T00:00:00.000'

                --SQL without datetime type specified --> Error
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (90ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                SELECT [i].[Id], [i].[Created], [i].[Name]
                FROM [Employee] AS [i]
                WHERE [i].[Created] > '2010-10-10T00:00:00.0000000'

                --SQL with datetime type specified -- ok
                Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (80ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                SELECT [i].[Id], [i].[Created], [i].[Name]
                FROM [Employee] AS [i]
                WHERE [i].[Created] > '2010-10-10T00:00:00.000'
                
            */

            });
        }

        private Expression<Func<T, bool>> GetFilterWithLocalDateTime<T>(DateTime value)
        {

            var itemParam = Expression.Parameter(typeof(T), "i");


            var filter = Expression.MakeBinary(
                    ExpressionType.GreaterThan,
                    Expression.PropertyOrField(itemParam, "Created"),
                    Expression.Constant(value, typeof(DateTime)));


            return Expression.Lambda<Func<T, bool>>(filter, new[] { itemParam });
        }


    }
}

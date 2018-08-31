using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest
{
    [Collection("Database collection")]
    public class TestBase
    {
        DatabaseFixture fixture;

        public TestBase(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        protected void Linq2SqlContext(Action<Linq2Sql.Linq2SqlDataContext> action)
        {
            var connection = this.fixture.Connection;
            var transaction = connection.BeginTransaction();
            try
            {
                using (var context = new Linq2Sql.Linq2SqlDataContext(connection))
                {
                    context.Log = Console.Out;
                    action(context);
                }
            }
            finally
            {
                transaction.Rollback();
            }
        }

        protected void EFContext(Action<EFCore.EFCoreDataContext> action)
        {
            var connection = this.fixture.Connection;
            var transaction = connection.BeginTransaction();
            var options = new DbContextOptionsBuilder<EFCore.EFCoreDataContext>()
            .UseSqlCe(connection)
            .Options;
            try
            {
                using (var context = new EFCore.EFCoreDataContext(options))
                {
                    action(context);
                }
            }
            finally
            {
                transaction.Rollback();
            }
        }
    }
}

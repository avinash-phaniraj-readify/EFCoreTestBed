using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
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
                    
                    context.Log = new DebugTextWriter();
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

    class DebugTextWriter : System.IO.TextWriter
    {
        public override void Write(char[] buffer, int index, int count)
        {
            System.Diagnostics.Debug.Write(new String(buffer, index, count));
        }

        public override void Write(string value)
        {
            System.Diagnostics.Debug.Write(value);
        }
        public override Encoding Encoding => System.Text.Encoding.Default;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Data;
using System.Text;
using Xunit;
using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace Linq2SqlEFCoreBehaviorsTest
{
    [Collection("Database collection")]
    public class TestBase
    {
        DatabaseFixture fixture;
        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });

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
                    context.Transaction = transaction;
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
            //var transaction = connection.BeginTransaction();
            var builder = new DbContextOptionsBuilder<EFCore.EFCoreDataContext>()
            .EnableSensitiveDataLogging(true)
            .UseLoggerFactory(MyLoggerFactory);

            if (connection is SqlConnection)
            {
                builder = builder.UseSqlServer(connection);
            } else
            {
                builder = builder.UseSqlCe(connection);
            }

            var options = builder.Options;
            try
            {
                using (var context = new EFCore.EFCoreDataContext(options))
                {
                    action(context);
                }
            }
            finally
            {
                //transaction.Rollback();
            }
        }

        protected IDbConnection Connection => fixture.Connection;
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

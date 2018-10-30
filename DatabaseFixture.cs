using System;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.IO;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest
{
    public class DatabaseFixture : IDisposable
    {
        private string databaseFilePath;
        private DbConnection connection;

        public DatabaseFixture()
        {
            databaseFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sdf");

            var dbStream = this.GetType().Assembly.GetManifestResourceStream("Linq2SqlEFCoreBehaviorsTest.TestDb.sdf");
            using (var fileStream = new FileStream(databaseFilePath, FileMode.Create))
            {
                dbStream.CopyTo(fileStream);
            }
            connection = GetCeConnection();
            //connection = GetSqlConnection();
        }

        public DbConnection Connection => connection;

        public void Dispose()
        {
            connection.Close();
            File.Delete(databaseFilePath);
        }

        private SqlCeConnection GetCeConnection()
        {
            var ceConnectionString = $"Data Source={databaseFilePath}; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();
            return ceConnection;
        }

        private SqlConnection GetSqlConnection()
        {
            var sqlConnectionString = "Server=localhost;Database=Testbed;Trusted_Connection=True;";
            var sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
    
}

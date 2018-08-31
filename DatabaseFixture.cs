using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq2SqlEFCoreBehaviorsTest
{
    public class DatabaseFixture : IDisposable
    {
        private string databaseFilePath;
        private SqlCeConnection connection;

        public DatabaseFixture()
        {
            databaseFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sdf");

            var dbStream = this.GetType().Assembly.GetManifestResourceStream("Linq2SqlEFCoreBehaviorsTest.TestDb.sdf");
            using (var fileStream = new FileStream(databaseFilePath, FileMode.Create))
            {
                dbStream.CopyTo(fileStream);
            }
            connection = GetConnection();
        }

        public SqlCeConnection Connection => connection;

        public void Dispose()
        {
            connection.Close();
            File.Delete(databaseFilePath);
        }

        private SqlCeConnection GetConnection()
        {
            var ceConnectionString = $"Data Source={databaseFilePath}; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();
            return ceConnection;
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

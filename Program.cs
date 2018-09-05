using System;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHostForCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "My Name Is Unique";

            var nameIsUnique = CheckIfNameIsUnique(name);

            Console.WriteLine($"Is Name Unique:{nameIsUnique}");

            Console.ReadLine();
        }

        private static bool CheckIfNameIsUnique(string name)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            
            ceConnection.Open();

            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseSqlCe(ceConnection)
                .Options;

            var context = new TestDataContext(options);
            var employees = context.Set<Employee>();

            var employeesList = employees.Where(p => p.Name == name).ToList();

            return employeesList.Count == 0;
        }
    }
}

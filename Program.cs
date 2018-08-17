using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHostForCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnection = new SqlCeConnection("Data Source=TestDb.sdf; Persist Security Info=False;");
            ceConnection.Open();

            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseSqlCe(ceConnection)
                .Options;

            var context = new TestDataContext(options);
            var ds = context.Set<EmployeeDevice>();

            var currentDate = DateTime.Now;

            var q = (from d in ds
                     select (d.ExpiryDate.Value - currentDate).Days).ToList();
        }
    }
}

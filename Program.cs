using Microsoft.EntityFrameworkCore;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHostForCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();

            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseSqlCe(ceConnection)
                .Options;

            var context = new TestDataContext(options);
            var es = context.Set<Employee>();
            var ds = context.Set<EmployeeDevice>();

            var q = (from e in es
                join d in ds on e.Id equals d.EmployeeId into x
                from j in x.DefaultIfEmpty()
                select new Holder
                {
                    Name = e.Name,
                    DeviceId = j.DeviceId //(short?)
                }).ToList();
        }
    }

    public class Holder
    {
        public string Name { get; set; }
        public int? DeviceId { get; set; }
    }
}

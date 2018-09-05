using System;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlServerCe;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";

            SqlCeEngine engine = new SqlCeEngine(ceConnectionString);
            engine.Upgrade(ceConnectionString);

            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();


            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseSqlCe(ceConnection)
               
                .Options;

            using (var dc = new TestDataContext(options))
            {
                var employee =
                    (from emp in dc.Employee
                    join dev in dc.EmployeeDevice
                        on emp.Id equals dev.EmployeeId
                    select new EmployeeInfo
                    {
                        EmployeeId = emp.Id,
                        EmployeeName = emp.Name,
                        Device = dev.Device,

                        //Expression evaluation caused an overflow. [ Name of function (if known) =  ]
                        AverageSharePerYear = emp.YearsOfService == 0 ? 0 : (decimal)emp.NumberOfShare / (decimal)emp.YearsOfService

                        //An exception occurred while reading a database value.The expected type was 'System.Decimal' but the actual value was of type 'System.Int32'.'
                        //AverageSharePerYear = emp.YearsOfService == 0 ? (decimal)emp.NumberOfShare  : (decimal)emp.NumberOfShare / (decimal)emp.YearsOfService

                        //This works!!
                        //AverageSharePerYear = emp.YearsOfService == 0 ? 0 : Convert.ToDecimal(emp.NumberOfShare) / Convert.ToDecimal(emp.YearsOfService)

                    }).ToList();                              
            }

            Console.ReadLine();
        }
        public class EmployeeInfo
        {
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public decimal AverageSharePerYear { get; set; }
            public string Device { get; set; }
        }
    }
}

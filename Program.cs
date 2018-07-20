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
            try
            {
                var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
                var ceConnection = new SqlCeConnection(ceConnectionString);
                ceConnection.Open();

                var options = new DbContextOptionsBuilder<TestDataContext>()
                    .UseSqlCe(ceConnection)
                    .Options;

                var context = new TestDataContext(options);

                //this works.
                var employeeDtos = (from device in context.Set<EmployeeDevice>()
                                    select new EmployeeDto
                                    {
                                        Id = device.Employee.Id,
                                        Name = device.Employee.Name
                                    }).ToList();

                //this fails.
                employeeDtos = (from device in context.Set<EmployeeDevice>()
                                select DtoFactory.CreateEmployeeDto(device)).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}

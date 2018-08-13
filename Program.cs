using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);


            var options = new DbContextOptionsBuilder()
                .UseSqlCe(ceConnection);

            Func<DbContextOptionsBuilder, DbContextOptionsBuilder> configureOptions = (c) => c.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) }));
            options.EnableSensitiveDataLogging();

            options = configureOptions(options);

            using (var dc = new TestDataContext(options.Options))
            {
           
                var driverQuery =
                    from bel in dc.Drivers
                    where bel.YearsOfExperience.GetValueOrDefault() > 1
                    select bel;

                var driverTruckInfoQuery = from truck in dc.Trucks
                               join l in driverQuery on truck.DriverId equals l.RecordId
                               select new DriveTruckInfo
                               {
                                   DepartmentId = l.Department.RecordId,
                                   DepartmentName = l.Department.Name,
                                   TruckId = truck.RecordId,
                                   TruckName = truck.Model,
                                   DriverId = l.RecordId,
                                   DriverName = l.Name
                               };

                var rgttc = from trips in dc.Trips
                            join drivertruckq in driverTruckInfoQuery on trips.DriverId equals drivertruckq.DriverId
                            select new
                            {
                                TripId = trips.RecordId,
                                TripSummary = string.Format($"Driver: {drivertruckq.DriverName}, Truck: {drivertruckq.TruckName}"),
                                trips.TotalKilometers
                            };

                var itemsList = rgttc.ToList();

                Console.ReadLine();
            }
        }
    }

    public class DriveTruckInfo
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int TruckId { get; set; }
        public string TruckName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}

﻿namespace TestHostForCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var ceConnectionString = "Data Source=TestDb.sdf; Persist Security Info = False; ";
            var ceConnection = new SqlCeConnection(ceConnectionString);
            ceConnection.Open();
            
            var options = new DbContextOptionsBuilder<EFCoreDataContext>()
                .UseSqlCe(ceConnection)
                .Options;

            var context = new EFCoreDataContext(options);
            var employees = context.Set<Employee>();
            var employeeDevices = context.Set<EmployeeDevice>();

            var q = (
                from e in employees
                join d in employeeDevices on e.Id equals d.EmployeeId into x 
                from j in x.DefaultIfEmpty()
                select new 
                {
                    Result = j.Id != 0 ? "Was Not Zero" : "Was Zero"
                }).ToList();

            Debug.Assert(q[0].Result == "Was Not Zero");
            Debug.Assert(q[1].Result == "Was Not Zero");
            Debug.Assert(q[2].Result == "Was Not Zero");
            Debug.Assert(q[3].Result == "Was Zero"); // fails - is actually null
            */
        }
    }
}

namespace TestHostForCastException
{
    internal class DtoFactory
    {
        internal static EmployeeDto CreateEmployeeDto(EmployeeDevice device)
        {
            return new EmployeeDto
            {
                Id = device.Employee.Id,
                Name = device.Employee.Name
            };
        }
    }

    internal class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
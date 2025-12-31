namespace EmployeeSystem.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public bool IsWorking { get; set; } = true;

        public bool GetWorkingStatus() => IsWorking;
    }
}
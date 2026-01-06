using EmployeeSystem.Entities;

namespace EmployeeSystem.Services
{
    public class EmployeeService
    {
        public void TerminateEmployee(Employee employee)
        {
            employee.IsWorking = false;
        }
    }
}
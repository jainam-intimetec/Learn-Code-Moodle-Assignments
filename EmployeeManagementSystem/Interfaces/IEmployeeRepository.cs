using EmployeeSystem.Entities;
 
namespace EmployeeSystem.Interfaces
{
    public interface IEmployeeRepository
    {
        void SaveEmployeeToDatabase(Employee employee);
    }
}
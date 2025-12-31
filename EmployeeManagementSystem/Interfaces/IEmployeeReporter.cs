using EmployeeSystem.Entities;
 
namespace EmployeeSystem.Interfaces
{
    public interface IEmployeeReporter
    {
        void Export(Employee employee);
    }
}
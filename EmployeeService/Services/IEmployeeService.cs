using EmployeeService.Core.Entities;

namespace EmployeeService.Services
{
    public interface IEmployeeService
    {
        public Employee? GetById(int id);
        public List<Employee>? GetAllEmployees();
        public Employee Add(Employee employee);
        public Employee Update(Employee employee, int id);
        public Employee Delete(int id);
        
    }
}

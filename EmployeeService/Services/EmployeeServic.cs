using EmployeeService.Core.Entities;
using EmployeeService.Infrastructure.DatabaseContext;
using EmployeeService.Services.EventHubServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Services
{
    public class EmployeeServic
    {
        private readonly EmployeeDBContext _dbContext;
        private readonly EventHubService _eventHubService;

        public EmployeeServic(EmployeeDBContext context, EventHubService eventHubService)
        {
            _dbContext = context;
            _eventHubService = eventHubService;
        }

        public async Task<Employee> Add(Employee employee)
        {
           _dbContext.Add(employee);
           _dbContext.SaveChanges();
        //    string message = string.Format($"Employee {employee.Name} added using email: {employee.Email}");
        //    await _eventHubService.SendNotificationAsync(message);
           return employee;
        }


        public async Task<List<Employee>>? GetAllEmployees()
        {
            List<Employee> employees = null;
            try {
            employees = await _dbContext.Employees.ToListAsync();
            if (employees == null)
            {
                return null;
            }
            }
            catch (Exception ex){
                Console.WriteLine(ex.GetType);
            }
            // _eventHubService.ReciveEvents();
            return employees; 
        }

        public Employee? GetById(int id)
        {
            return _dbContext.Employees.Find(id);
        }

        public Employee Update(Employee updatedEmployee, int id)
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                throw new ArgumentException("Employee does not exist");
            }
            employee.Name = updatedEmployee.Name;
            employee.Email = updatedEmployee.Email;
            _dbContext.SaveChanges();
            return employee;

        }

        public async Task<Employee> DeleteAsync(int id)
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return employee;
            }
            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
            string message = string.Format($"Employee {employee.Name} deleted with email: {employee.Email} and id: {employee.Id}");
            await _eventHubService.SendNotificationAsync(message);
            return employee;
        }
    }
}

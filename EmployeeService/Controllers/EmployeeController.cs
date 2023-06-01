using EmployeeService.Core.Entities;
using EmployeeService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private EmployeeServic _employeeService;
        public EmployeeController(EmployeeServic employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
           
           
            List<Employee> employees = await _employeeService.GetAllEmployees();
            if (employees == null)
            {
                return NotFound();
            }
            return employees;
        }

        [HttpGet("id")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var emp = _employeeService.GetById(id);
            if (emp == null)
            {
                return NotFound();
            }
            return emp;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            if (employee == null) { return BadRequest(); }
            await _employeeService.Add(employee);
            return CreatedAtAction(nameof(AddEmployee), employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, Employee employee)
        {
            if (employee == null)
            {
                Console.WriteLine("Not found");
                return BadRequest();
            }
            return _employeeService.Update(employee, id);
        }
        [HttpDelete("{id}")]
        public bool DeleteEmployee(int id)
        {
            var emp = _employeeService.DeleteAsync(id);
            return emp.Result != null;
        }
    }
}

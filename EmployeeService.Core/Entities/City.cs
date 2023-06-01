using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Core.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }


        //public ICollection<Employee>? Employees { get; set; }
    }
}

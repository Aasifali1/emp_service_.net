using Microsoft.EntityFrameworkCore;
using EmployeeService.Core.Entities;

namespace EmployeeService.Infrastructure.DatabaseContext
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<TestEmp> Tests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasData(new City { Id = 1, Name = "Chandigarh" },
                new City { Id = 2, Name = "Delhi" }
                );

/*            string citiesjson = File.ReadAllText(@"C:\Users\Aasif Ali\source\repos\EmployeeService\EmployeeService.Infrastructure\CitiesData.json");
            List<City> cities = System.Text.Json.JsonSerializer.Deserialize<List<City>>(citiesjson);

            foreach (City city in cities)
            {
                modelBuilder.Entity<City>().HasData(city);
            }*/

        }
    }
}

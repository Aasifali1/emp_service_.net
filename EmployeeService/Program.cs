using EmployeeService.Core.Entities;
using EmployeeService.Infrastructure.DatabaseContext;

using EmployeeService.Services;
using EmployeeService.Services.EventHubServices;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<EmployeeDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<EmployeeServic>();
            builder.Services.AddSingleton<EventHubService>();

            builder.Services.AddControllers();

            builder.Services.AddLogging(options=> options.SetMinimumLevel(LogLevel.Trace));
            Console.WriteLine("Hello");
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var data = builder.Configuration.GetSection("AllowedOrigins").Get<String>().ToString();
            Console.Write(data.ToString());
            var originStr = "http://localhost:4200";
            builder.Services.AddCors(options=>
            {
                options.AddDefaultPolicy(defaultBuilder =>
                {
                    defaultBuilder.WithOrigins(originStr)
                    .WithHeaders("Authorization", "origin", "content-type", "accept");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors();

            /*app.UseHttpsRedirection();*/

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
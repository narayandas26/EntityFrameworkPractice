// See https://aka.ms/new-console-template for more information

using EntityFrameworkPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


internal class Program
{
    private static void Main(string[] args)
    {
        EFSampleCodeMethod();

        Console.WriteLine("Hello, World!");
    }

    private static void EFSampleCodeMethod()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var conStringsConfigSection = config.GetSection("ConnectionStrings");

        var cosmosConStr = conStringsConfigSection.GetValue<string>("NarCosmosConnection") ?? string.Empty;

        var optionsBuilter = new DbContextOptionsBuilder<NarCosmosContext>();
        optionsBuilter.UseSqlServer(cosmosConStr);

        //var dbContext = new NarCosmosContext(optionsBuilter.Options);

        var dbContext = new NarCosmosContext(cosmosConStr);

        //Eager Loading + using async methods (parent method should be async to use await but below is just a reminder for using async)
        //var task = dbContext.Departments.Include("Employees.Manager").AsNoTracking().ToListAsync();
        //task.Wait();
        //var depts = task.Result;


        //using projection for eager loading
        var departments = dbContext.Departments
            .Select(d => new DepartmentDto
            {
                DeptId = d.DeptId,
                DeptName = d.DeptName,
                Employees = d.Employees.Select(e => new UnmappedEmplyeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                }
                    ),
            }
            )
            .AsNoTracking()
            .ToList();


        //returning Unmapped Entity via SqlQuery
        var employeeDtos = dbContext.Database.SqlQuery<UnmappedEmplyeeDto>($"select EmployeeId, FirstName, LastName from Employee").ToList();

    }

}
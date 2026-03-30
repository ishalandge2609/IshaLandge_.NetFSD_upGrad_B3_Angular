using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
        
                // Creating collection (List of Employees)
                List<Employee> employees = new List<Employee>()
            {
                new Employee { Empno = 1, Ename = "Isha", Job = "Developer", Salary = 50000, Deptno = 10 },
                new Employee { Empno = 2, Ename = "Rahul", Job = "Tester", Salary = 40000, Deptno = 20 },
                new Employee { Empno = 3, Ename = "Neha", Job = "Manager", Salary = 70000, Deptno = 30 },
                new Employee { Empno = 4, Ename = "Amit", Job = "HR", Salary = 35000, Deptno = 40 }
            };

                return View(employees);
            }
        }
    }



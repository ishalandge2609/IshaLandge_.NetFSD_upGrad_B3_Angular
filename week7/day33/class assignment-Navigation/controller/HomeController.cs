using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Channels;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;//ApplicationDbContext → your database class, _context → variable to use it

        public HomeController(ApplicationDbContext context)// constructor injection to inject the database context into the controller
        {
            _context = context; //Stores the received object into _context
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Emps()
        {
            var emps =_context.Employees.Include(e=>e.Dept).ToList();// to fetch all employees with their department details and sens it to the view
            return View(emps);//Sending data to UI  View will receive: List<Employee>
        }// here _context.Employees goes to the employee table 
         //Include(e=>e.Dept) is used to include the related department details for each employee in the result set. 
         // ToList() is used to execute the query and brings data from the database and convert it into a list of employee objects which can be easily passed to the view for rendering.



        public IActionResult Depts()
        {
            var depts = _context.Depts.Include(d => d.Employees).ToList();// to fetch all departments with their employee details and sens it to the view
            return View(depts);//Sending data to UI  View will receive: List<Employee>
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

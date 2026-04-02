using Microsoft.AspNetCore.Mvc;

namespace MyFirstMVCApp.Controllers
{
    [Route("student")]
    public class StudentController : Controller
    {
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(string studentName, int age, string course)
        {
            if (age < 16 || age > 30)
            {
                ViewBag.Error = "Age must be between 16 and 35";
                return View();
            }

            return RedirectToAction("Display", new
            {
                studentName = studentName,
                age = age,
                course = course
            });
        }

        [HttpGet("display")]
        public IActionResult Display(string studentName, int age, string course)
        {
            ViewBag.StudentName = studentName;
            ViewBag.Age = age;
            ViewBag.Course = course;

            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using StudentCourseRelationshipUsingDapper.Models;
using StudentCourseRelationshipUsingDapper.Repository;
using System.Diagnostics;

namespace StudentCourseRelationshipUsingDapper.Controllers
{
    public class HomeController : Controller
    {

        private readonly IStudentRepository _repo; //variable declaration for repository

        public HomeController(IStudentRepository repo) //dependency injection of repository
        {
            _repo = repo;                           //assigning repository to variable
        }

        public IActionResult Student()
        {

            var emps = _repo.GetStudentsWithCourse();
            return View(emps);
        }

        public IActionResult Courses()
        {

            var depts = _repo.GetCoursesWithStudents();
            return View(depts);
        }

        public IActionResult Index()
        {
            return View(); 
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

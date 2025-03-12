using Microsoft.AspNetCore.Mvc;
using SomerenApp.Models;

namespace SomerenApp.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index() 
        {
            List<Student> students =
            [
                new Student(734278, "Marilisa", "Kuilboer", "06-94846574", "1b", "B1-03"),
                new Student(728461, "Yasmina", "Baalla", "06-84659503", "1b", "B1-03"),
                new Student(735615, "Bauke", "Bosma", "06-253840281", "1b", "B1-04")
            ];
            return View(students);
        }
    }
}

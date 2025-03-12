using SomerenApp.Models;
using SomerenApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SomerenApp.Controllers
{
    public class LecturersController : Controller
    {
        public IActionResult Index()
        {
            //List<Docent> users = _usersRepository.GetAll();
            return View();
        }
    }
}

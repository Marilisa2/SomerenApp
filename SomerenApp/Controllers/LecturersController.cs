using SomerenApp.Models;
using SomerenApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SomerenApp.Controllers
{
    public class LecturersController : Controller
    {
        private readonly ILecturersRepository _lecturersRepository;

        public LecturersController(ILecturersRepository lecturersRepository)
        {
            _lecturersRepository = lecturersRepository;
        }
        public IActionResult Index()
        {
            /*List<Lecturer> lecturers = [
new Lecturer("Dijkstra", "Edsger", 90, "020 1234567"),
new Lecturer("Tanenbaum", "Andrew", 75, "020 1234567"),
new Lecturer("Stein", "Clifford", 60, "020 1234567"),
                ];*/

            try
            {
                List<Lecturer> lecturers = _lecturersRepository.GetAllLecturers();
                return View(lecturers);
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Lecturer lecturer)
        {
            try
            {
                _lecturersRepository.AddLecturer(lecturer);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View(lecturer);
            }
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                Lecturer? lecturer = _lecturersRepository.GetLecturerByID((int)id);
                return View(lecturer);
            }
            catch
            {
                throw new Exception("Given lecturer ID does not exist.");
            }

        }
        [HttpPost]
        public ActionResult Delete(Lecturer lecturer)
        {
            try
            {
                _lecturersRepository.DeleteLecturer(lecturer);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View(lecturer);
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                Lecturer? lecturer = _lecturersRepository.GetLecturerByID((int)id);
                return View(lecturer);
            }
            catch
            {
                throw new Exception("Given lecturer ID does not exist.");
            }

        }
        [HttpPost]
        public ActionResult Edit(Lecturer lecturer)
        {
            try
            {
                _lecturersRepository.EditLecturer(lecturer);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View(lecturer);
            }
        }
    }
}

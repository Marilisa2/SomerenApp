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

            List<Lecturer> lecturers = _lecturersRepository.GetAllLecturers();
            return View(lecturers);
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
            Lecturer? lecturer = _lecturersRepository.GetLecturerByID((int)id);
            return View(lecturer);
        }
        [HttpPost]
        public ActionResult Delete(Lecturer lecturer)
        {
            try
            {
                _lecturersRepository.DeleteLecturer(lecturer.LecturerNumber);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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
            Lecturer? lecturer = _lecturersRepository.GetLecturerByID((int)id);
            return View(lecturer);
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
                return View(lecturer);
            }
        }

    }
}

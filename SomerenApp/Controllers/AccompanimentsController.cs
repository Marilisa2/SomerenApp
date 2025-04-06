using Microsoft.AspNetCore.Mvc;
using SomerenApp.Models;
using SomerenApp.Repositories;

namespace SomerenApp.Controllers
{
    public class AccompanimentsController : Controller
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly ILecturersRepository _lecturersRepository;
        private readonly IAccompanimentsRepository _accompanimentsRepository;

        public AccompanimentsController(IActivitiesRepository activities, ILecturersRepository lecturersRepository, IAccompanimentsRepository accompanimentsRepository)
        {
            _activitiesRepository = activities;
            _lecturersRepository = lecturersRepository;
            _accompanimentsRepository = accompanimentsRepository;
        }
        [HttpGet]
        public ActionResult Index(int? activityNumber)
        {
            if (activityNumber == null)
            {
                return NotFound();
            }
            try
            {
                Models.Activity activity = _activitiesRepository.GetByID((int)activityNumber);
                List<Lecturer> supervisors = _lecturersRepository.GetSupervisors(activity.ActivityNumber);
                List<Lecturer> nonSupervisors = _lecturersRepository.GetNonSupervisors(activity.ActivityNumber);

                Accompaniment accompaniment = new Accompaniment(activity, supervisors, nonSupervisors);

                return View(accompaniment);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Delete(int activityNumber, int lecturerNumber)
        {
            try
            {
                _accompanimentsRepository.RemoveSuperVisor(activityNumber, lecturerNumber);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult Create(int activityNumber, int lecturerNumber)
        {
            try
            {
                _accompanimentsRepository.AddSuperVisor(activityNumber, lecturerNumber);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Index");
            }
        }
    }
}

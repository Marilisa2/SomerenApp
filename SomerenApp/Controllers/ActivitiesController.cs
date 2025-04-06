using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SomerenApp.Models;
using SomerenApp.Repositories;
using SomerenActivity = SomerenApp.Models.Activity; //prevents confusion with System.Diagnostics.Activity

namespace SomerenApp.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly ILecturersRepository _lecturersRepository;

        public ActivitiesController(IActivitiesRepository activities, ILecturersRepository lecturersRepository)
        {
            _activitiesRepository = activities;
            _lecturersRepository = lecturersRepository;
        }
        

        //GET: ActivitiesController/Index
        public ActionResult Index()
        {
            //get all activities via repository
            List<SomerenActivity> activities = _activitiesRepository.GetAllActivities();

            //pass the list to the view
            return View(activities);
        }

        //GET: ActivitiesControllers/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //POST: ActivitiesControllers/Create
        [HttpPost]
        public ActionResult Create(SomerenActivity activity)
        {
            try
            {

                //adding activity via repository
                _activitiesRepository.Add(activity);

                //go back to activity list via Index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Error occurred while adding activity: " + ex.Message;

                return View(activity);
            }
        }

        //GET ActivitiesController/Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get activity via repository
            SomerenActivity? user = _activitiesRepository.GetByID((int)id);
            return View(user);
        }

        //POST UsersController/Edit
        [HttpPost]
        public ActionResult Edit(SomerenActivity activity)
        {
            try
            {
                //update activity via repository
                _activitiesRepository.Update(activity);

                //go back to activity list via Index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //something's wrong go back to view which activity
                return View(activity);
            }
        }

        //GET: ActivitiesController/Delete
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get activity via repository
            SomerenActivity? activity = _activitiesRepository.GetByID((int)id);
            return View(activity);
        }

        //POST: ActivitiesController/Delete
        [HttpPost]
        public ActionResult Delete(SomerenActivity activity)
        {
            try
            {
                //update activity via repository
                _activitiesRepository.Delete(activity);

                //go back to activity list via Index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //something's wrong go back to view which activity
                return View(activity);
            }
        }
        [HttpGet]
        public ActionResult Accompaniments(int? activityNumber)
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

                //Dictionary<Lecturer, bool> accompanimentsDictionary;
                return View(accompaniment); 
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult RemoveAccompaniment(int activityNumber, int lecturerNumber)
        {
            try
            {
                _lecturersRepository.RemoveSuperVisor(activityNumber, lecturerNumber);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult AddAccompaniment(int activityNumber, int lecturerNumber)
        {
            try
            {
                _lecturersRepository.AddSuperVisor(activityNumber, lecturerNumber);
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

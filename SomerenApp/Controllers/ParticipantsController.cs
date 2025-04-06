using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SomerenApp.Models;
using SomerenApp.Repositories;
using System.Diagnostics.Eventing.Reader;
using SomerenActivity = SomerenApp.Models.Activity;
using System.Security.Cryptography;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering; //prevents confusion with System.Diagnostics.Activity




namespace SomerenApp.Controllers
{
    public class ParticipantsController : Controller
    {
        private readonly IParticipantsRepository _participantsRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly IActivitiesRepository _activitiesRepository;

        public ParticipantsController(IParticipantsRepository participantsRepository, IStudentsRepository studentsRepository, IActivitiesRepository activitiesRepository)
        {
            _participantsRepository = participantsRepository;
            _studentsRepository = studentsRepository;
            _activitiesRepository = activitiesRepository;
        }

        public IActionResult Index(int? activityNumber)
        {
            try
            {
                //gets all activities
                List<SomerenActivity> activities = _activitiesRepository.GetAllActivities();

                //checks if there are any activities
                if (activities == null || activities.Count == 0)
                {
                    return NotFound("No activities were found");
                }

                //if no ActivityNumber is specified then choose the first activity
                if (!activityNumber.HasValue)
                {
                    activityNumber = activities.First().ActivityNumber;
                }

                //search selected activity
                SomerenActivity selectedActivity = _activitiesRepository.GetByID(activityNumber.Value);

                if (selectedActivity == null)
                {
                    return NotFound();
                }

                //retrieves all students and participants
                List<Student> allStudents = _studentsRepository.GetAll();
                List<Participant> allParticipants = _participantsRepository.GetAllParticipants();

                //retrieves participants for the selected activity
                List<Student> participants = allParticipants
                    .Where(p => p.ActivityNumber == activityNumber)
                    .Select(p => allStudents.First(s => s.StudentNumber == p.StudentNumber))
                    .Where(s => s != null).ToList();

                //retrieves nonParticipants
                List<Student> nonParticipants = allStudents.Except(participants).ToList();

                //create viewmodel
                ActivityParticipantsViewModel viewModel = new ActivityParticipantsViewModel
                {
                    Activity = selectedActivity,
                    Participants = participants,
                    NonParticipants = nonParticipants,
                    Activities = activities
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An error occured while retrieving data " + ex.Message;
                return View();
            }

        }

        //method with same code for ActivityParticipantsViewModel for Create
        private ActivityParticipantsViewModel CreateActivityParticipantsViewModelForAdd(int studentNumber, int activityNumber)
        {
            //gets student and activity from repository 
            Student student = _studentsRepository.GetById(studentNumber);
            SomerenActivity activity = _activitiesRepository.GetByID(activityNumber);

            if (student == null || activity == null)
            {
                throw new Exception("Student or activity not found");
            }

            ActivityParticipantsViewModel viewModel = new ActivityParticipantsViewModel
            {
                Student = student,
                Activity = activity,
                SuccessMessage = $"Participant {student.FirstName} {student.LastName} was succesfully added to the activity '{activity.ActivityType}'."
            };

            return viewModel;
        }

        //method with same code for ActivityParticipantsViewModel for Delete
        private ActivityParticipantsViewModel CreateActivityParticipantsViewModelForDelete(int studentNumber, int activityNumber)
        {
            //gets student and activiteit from repository 
            Student student = _studentsRepository.GetById(studentNumber);
            SomerenActivity activity = _activitiesRepository.GetByID(activityNumber);

            if (student == null || activity == null)
            {
                throw new Exception("Student or activity not found");
            }

            ActivityParticipantsViewModel viewModel = new ActivityParticipantsViewModel
            {
                Student = student,
                Activity = activity,
                SuccessMessage = $"Participant {student.FirstName} {student.LastName} was succesfully removed from the activity '{activity.ActivityType}'."
            };

            return viewModel;
        }

        //GET: ParticipantsController/Add
        [HttpGet]
        public IActionResult Create(int studentNumber, int activityNumber)
        {
            try
            {
                //gets student and activity from repository 
                Student student = _studentsRepository.GetById(studentNumber);
                SomerenActivity activity = _activitiesRepository.GetByID(activityNumber);

                if (student == null || activity == null)
                {
                    return NotFound();
                }

                ActivityParticipantsViewModel viewModel = new ActivityParticipantsViewModel
                {
                    Student = student,
                    Activity = activity,
                    Participant = new Participant { StudentNumber = student.StudentNumber, ActivityNumber = activity.ActivityNumber }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An error occured while retrieving data: " + ex.Message;
                return View();
            }
        }

        //POST: ParticipantsController/Add
        [HttpPost]
        public IActionResult Create(Participant participant)
        {
            try
            {
                if (participant == null)
                {
                    ViewData["ErrorMessage"] = "Invalid participant data";
                    return View();
                }

                //checking if participant is already added to the selected activity
                if (_participantsRepository.GetByID(participant.StudentNumber, participant.ActivityNumber) != null)
                {
                    ViewData["ErrorMessage"] = "This participant is already added to this activity.";
                    return View(participant);
                }

                //add participant via repository
                _participantsRepository.Add(participant);

                //retrieving the SuccessMessage from CreateActivityParticipantsViewModelForAdd 
                ActivityParticipantsViewModel viewModel = CreateActivityParticipantsViewModelForAdd(participant.StudentNumber, participant.ActivityNumber);

                //adding the SuccessMessage
                TempData["SuccessMessage"] = viewModel.SuccessMessage;

                //redirect to the index page
                return RedirectToAction("Index", new { activityNumber = participant.ActivityNumber });

            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An error occured while adding the participant: " + ex.Message;
                return View(participant);
            }
        }    

        //GET: ParticipantsController/Delete
        [HttpGet]
        public IActionResult Delete(int studentNumber, int activityNumber)
        {
            //get participant via repository
            Participant participant = _participantsRepository.GetByID(studentNumber, activityNumber);
            if (participant == null)
            {
                return NotFound();
            }

            //retrieve the corresponding student and activity
            Student student = _studentsRepository.GetById(studentNumber);
            SomerenActivity activity = _activitiesRepository.GetByID(activityNumber);

            if (student == null || activity == null)
            {
                return NotFound();
            }

            ActivityParticipantsViewModel viewModel = new ActivityParticipantsViewModel
            { 
                Student = student,
                Activity = activity,
                Participant = participant
            };

            return View(viewModel);
        }

        //POST: ParticipantsController/Delete
        [HttpPost]
        public IActionResult Delete(Participant participant)
        {
            try
            {
                if (participant == null)
                {
                    return NotFound();
                }

                //delete participant from the participants list
                _participantsRepository.Delete(participant);

                //retrieving the SuccessMessage from CreateActivityParticipantsViewModelForDelete
                ActivityParticipantsViewModel viewModel = CreateActivityParticipantsViewModelForDelete(participant.StudentNumber, participant.ActivityNumber);

                //adding the SuccessMessage
                TempData["SuccessMessage"] = viewModel.SuccessMessage;
                
               //return to list with participants for selected activity
                return RedirectToAction("Index", new { activityNumber = participant.ActivityNumber });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An error occured while deleting a participant " + ex.Message;
                return View(participant);
            
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using SomerenApp.Controllers;
using SomerenActivity = SomerenApp.Models.Activity; //prevents confusion with System.Diagnostics.Activity


namespace SomerenApp.Models
{
    public class ActivityParticipantsViewModel
    {
        public SomerenActivity Activity { get; set; }
        public List<Student> Participants { get; set; }
        public List<Student> NonParticipants { get; set; }
        public List<SomerenActivity> Activities { get; set; }
        public Participant Participant { get; set; }
        public Student Student { get; set; }

        //add a success message after deleting the participant from the selected activity
        public string SuccessMessage { get; set; }

        public ActivityParticipantsViewModel()
        {
        }

        public ActivityParticipantsViewModel(Activity activity, List<Student> participants, List<Student> nonParticipants, List<SomerenActivity> activities)
        {
            Activity = activity;
            Participants = participants;
            NonParticipants = nonParticipants;
            Activities = activities;
        }

    }
}

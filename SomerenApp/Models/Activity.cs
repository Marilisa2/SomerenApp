namespace SomerenApp.Models
{
    public class Activity
    {
        public int ActivityNumber { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityDate ActivityDate { get; set; }

        public Activity()
        {
        }

        public Activity(int activityNumber, ActivityType activityType, ActivityDate activityDate)
        {
            ActivityNumber = activityNumber;
            ActivityType = activityType;
            ActivityDate = activityDate;
        }
    }
}

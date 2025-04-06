namespace SomerenApp.Models
{
    public class Participant
    {
        public int StudentNumber { get; set; }
        public int ActivityNumber { get; set; }

        public Participant()
        {
        }

        public Participant(int studentNumber, int activityNumber)
        {
            StudentNumber = studentNumber;
            ActivityNumber = activityNumber;
        }
    }
}

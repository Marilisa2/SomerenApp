namespace SomerenApp.Models
{
    public class Accompaniment
    {
        public Activity Activity {  get; set; }
        public  List<Lecturer> Supervisors { get; set; }
        public List<Lecturer> NonSupervisors { get; set; }
        public Accompaniment()
        {
            
        }

        public Accompaniment(Activity activity, List<Lecturer> supervisors, List<Lecturer> nonSupervisors)
        {
            Activity = activity;
            Supervisors = supervisors;
            NonSupervisors = nonSupervisors;
        }
    }
}

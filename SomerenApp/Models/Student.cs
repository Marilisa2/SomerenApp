namespace SomerenApp.Models
{
    public class Student
    {
        public int StudentNumber {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
        public string ClassName {  get; set; }
       
        public int RoomId { get; set; }

        public Student(int studentnumber, string firstname, string lastname, string telephonenumber, string className, int roomId)
        {
            StudentNumber = studentnumber;
            FirstName = firstname;
            LastName = lastname;
            TelephoneNumber = telephonenumber;
            ClassName = className;
            RoomId = roomId;
        }
        public Student()
        {
        }
    }
}

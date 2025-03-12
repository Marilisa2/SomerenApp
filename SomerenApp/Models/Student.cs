namespace SomerenApp.Models
{
    public class Student
    {
        public int StudentNumber {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Class {  get; set; }
       
        public string RoomNumber { get; set; }

        public Student(int studentnumber, string firstname, string lastname, string telephonenumber, string @class, string kamernummer)
        {
            StudentNumber = studentnumber;
            FirstName = firstname;
            LastName = lastname;
            TelephoneNumber = telephonenumber;
            Class = @class;
            RoomNumber = kamernummer;
        }

        public Student()
        {

        }
    }
}

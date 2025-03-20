namespace SomerenApp.Models
{
    public class Student
    {
        public int StudentNumber {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassName {  get; set; }
        public string PhoneNumber { get; set; }
        public string RoomNumber { get; set; }
        public Student(int studentNumber, string firstName, string lastName, string className, string phoneNumber, string roomNumber)
        {
            StudentNumber = studentNumber;
            FirstName = firstName;
            LastName = lastName;
            ClassName = className;
            PhoneNumber = phoneNumber;
            RoomNumber = roomNumber;
        }
        public Student()
        {

        }
    }
}

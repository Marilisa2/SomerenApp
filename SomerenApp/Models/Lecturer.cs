namespace SomerenApp.Models
{
    public class Lecturer
    {
        public int LecturerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public int RoomNumber { get; set; }

        public Lecturer(int lecturerNumber, string firstName, string lastName, int age, string phoneNumber, int roomNumber)
        {
            LecturerNumber = lecturerNumber;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            PhoneNumber = phoneNumber;
            RoomNumber = roomNumber;
        }

        public Lecturer(string firstName, string lastName, int age, string phoneNumber)
        {
            LastName = lastName;
            FirstName = firstName;
            Age = age;
            PhoneNumber = phoneNumber;
        }

        public Lecturer()
        {
            
        }
    }
}

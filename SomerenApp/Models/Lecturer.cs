namespace SomerenApp.Models
{
    public class Lecturer
    {
        public int LecturerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Age { get; set; }
        public string PhoneNumber { get; set; }
        public int RoomId { get; set; }

        public Lecturer(int lecturerNumber, string firstName, string lastName, byte age, string phoneNumber, int roomId)
        {
            LecturerNumber = lecturerNumber;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            PhoneNumber = phoneNumber;
            RoomId = roomId;
        }
        public Lecturer(int lecturerNumber, string firstName, string lastName, byte age, string phoneNumber)
        {
            LecturerNumber = lecturerNumber;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            PhoneNumber = phoneNumber;
        }

        public Lecturer(){}
    }
}

using Microsoft.Data.SqlClient;
using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public class DbLecturersRepository : ILecturersRepository
    {
        private readonly string? _connectionString;
        public DbLecturersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }
        public List<Lecturer> GetAllLecturers()
        {
            List<Lecturer> lecturers = new List<Lecturer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT lastName, firstName, age, phoneNumber FROM lecturers ORDER BY lastName";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Lecturer lecturer = ReadLecturer(reader);
                    lecturers.Add(lecturer);
                }
            }
            return lecturers;
        }
        private Lecturer ReadLecturer(SqlDataReader reader)
        {
            string lastName = (string)reader["Last Name"];
            string firstName = (string)reader["First Name"];
            int age = (int)reader["Age"];
            string phoneNumber = (string)reader["Phone Number"];
            return new Lecturer(lastName, firstName, age, phoneNumber);
        }
        public Lecturer? GetLecturerByID(int lecturerNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT lastName, firstName, age, phoneNumber FROM lecturers WHERE lecturerNumber={lecturerNumber}";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Lecturer lecturer = ReadLecturer(reader);
                return lecturer;
            }
        }

        public void AddLecturer(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO users (firstName, lastName, age, phoneNumber, roomId){Environment.NewLine}" +
                    "VALUES(@FirstName, @LastName, @Age, @PhoneNumber, @RoomId); " +
                    "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                command.Parameters.AddWithValue("@Age", lecturer.Age);
                command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                command.Parameters.AddWithValue("@RoomId", lecturer.RoomId);
                command.Connection.Open();
                lecturer.LecturerNumber = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void EditLecturer(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE users SET firstName = @FirstName, lastName = @LastName, " +
                    "age = @Age, phoneNumber = @PhoneNumber WHERE lecturerNumber = @LecturerNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@MobileNumber", lecturer.LastName);
                command.Parameters.AddWithValue("@Age", lecturer.Age);
                command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                command.Parameters.AddWithValue("@LecturerNumber", lecturer.LecturerNumber);
                command.Connection.Open();
            }
        }

        public void DeleteLecturer(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM users WHERE lecturerNumber = @lecturerNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@lecturerNumber", lecturer.LecturerNumber);
                command.Connection.Open();
            }
        }
    }
}

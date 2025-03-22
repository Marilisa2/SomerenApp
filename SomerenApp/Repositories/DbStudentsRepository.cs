using Microsoft.Data.SqlClient;
using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public class DbStudentsRepository : IStudentsRepository
    {
        private readonly string? _connectionString;

        public DbStudentsRepository(IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }

        public void Add(Student student) 
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO Students (FirstName, LastName, TelephoneNumber, ClassName, RoomID)" +
                                "VALUES (@FirstName, @LastName, @TelephoneNumber, @ClassName, @RoomID);" +
                                "SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@TelephoneNumber", student.TelephoneNumber);
                command.Parameters.AddWithValue("@ClassName", student.ClassName);
                command.Parameters.AddWithValue("@RoomId", student.RoomId);

                student.StudentNumber = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int GetAvailableRoomId()
        {
            //haalt eerste beschikbare roomId op
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT TOP 1 RoomId FROM Rooms", connection);
                return Convert.ToInt32(command.ExecuteScalar());
            
            }
            
        }
        private Student ReadStudent(SqlDataReader reader) 
        {
            int studentNumber = (int)reader["StudentNumber"];
            string firstName = (string)reader["FirstName"];
            string lastName = (string)reader["LastName"];
            string telephoneNumber = (string)reader["TelephoneNumber"];
            string className = (string)reader["ClassName"];
            int roomId = (int)reader["RoomId"];

            return new Student(studentNumber, firstName, lastName, telephoneNumber, className, roomId);
        }
       
       
        public  List<Student> GetAll()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT StudentNumber, FirstName, LastName, TelephoneNumber, ClassName, RoomId FROM students " +
                                "ORDER BY LastName ASC";

                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = ReadStudent(reader);
                    students.Add(student);
                }
                reader.Close();
            }
            return students;
        }

        public Student? GetById(int studentNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT StudentNumber, FirstName, LastName, TelephoneNumber, ClassName, RoomID FROM students WHERE StudentNumber = @StudentNumber";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StudentNumber", studentNumber);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Student students = ReadStudent(reader);
                    reader.Close();
                    return students;
                }
                reader.Close();
                return null;
            }
        }

      
        public void Update(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //zorgt ervoor dat de RoomId geldig is bij een update
                string checkRoomQuery = "SELECT COUNT(*) FROM Rooms WHERE RoomId = @RoomId";
                SqlCommand checkRoomCommand = new SqlCommand(checkRoomQuery, connection);
                checkRoomCommand.Parameters.AddWithValue("RoomId", student.RoomId);

                connection.Open();
                int roomCount = (int)checkRoomCommand.ExecuteScalar();

                if (roomCount == 0)
                {
                    throw new Exception($"The RoomId {student.RoomId} does not exist");

                }

                //Update query
                string query = $"UPDATE students SET FirstName = @FirstName, LastName = @LastName, " +
                                "TelephoneNumber = @TelephoneNumber, ClassName = @ClassName, " +
                                "RoomID = @RoomID WHERE StudentNumber = @StudentNumber";
                                
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@TelephoneNumber", student.TelephoneNumber);
                command.Parameters.AddWithValue("@ClassName", student.ClassName);
                command.Parameters.AddWithValue("@RoomId", student.RoomId);

                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                {
                    throw new Exception("No records updates!");
                }
            }
        }

        public void Delete(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //zorgt ervoor dat de RoomId geldig is bij een update
                string checkRoomQuery = "SELECT COUNT(*) FROM Rooms WHERE RoomId = @RoomId";
                SqlCommand checkRoomCommand = new SqlCommand(checkRoomQuery, connection);
                checkRoomCommand.Parameters.AddWithValue("RoomId", student.RoomId);

                connection.Open();
                int roomCount = (int)checkRoomCommand.ExecuteScalar();

                if (roomCount == 0)
                {
                    throw new Exception($"The RoomId {student.RoomId} does not exist");

                }
                string query = $"DELETE FROM students WHERE StudentNumber = @StudentNumber";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);

                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                {
                    throw new Exception("No records deleted!");
                }
            }
        }


    }
}

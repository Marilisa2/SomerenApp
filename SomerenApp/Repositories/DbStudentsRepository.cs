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
                command.Parameters.AddWithValue("@Lastname", student.LastName);
                command.Parameters.AddWithValue("@TelephoneNumber", student.TelephoneNumber);
                command.Parameters.AddWithValue("@ClassName", student.ClassName);
                command.Parameters.AddWithValue("@RoomID", student.RoomID);

                command.Connection.Open();
                student.StudentNumber = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private Student ReadStudent(SqlDataReader reader) 
        {
            int studentNumber = (int)reader["StudentNumber"];
            string firstName = (string)reader["FirstName"];
            string lastName = (string)reader["LastName"];
            string telephoneNumber = (string)reader["TelephoneNumber"];
            string className = (string)reader["ClassName"];
            int roomId = (int)reader["RoomID"];

            return new Student(studentNumber, firstName, lastName, telephoneNumber, className, roomId);
        }
       
       
        public  List<Student> GetAll()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT StudentNumber, FirstName, LastName, TelephoneNumber, ClassName, RoomID FROM Students";

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
                string query = $"SELECT StudentNumber, FirstName, LastName, TelephoneNumber, ClassName, RoomID FROM Students WHERE StudentNumber = @StudentNumber";

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
                string query = $"UPDATE Students SET FirstName = @Firstname, LastName = @LastName, " +
                                "TelephoneNumber = @TelephoneNumber, ClassName = @ClassName, " +
                                "RoomID = @RoomID WHERE StudentNumber = @StudentNumber";
                                
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@Lastname", student.LastName);
                command.Parameters.AddWithValue("@TelephoneNumber", student.TelephoneNumber);
                command.Parameters.AddWithValue("@ClassName", student.ClassName);
                command.Parameters.AddWithValue("@RoomID", student.RoomID);

                command.Connection.Open();
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
                string query = $"DELETE FROM Students WHERE StudentNumber = @StudentNumber";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                {
                    throw new Exception("No records deleted!");
                }
            }
        }


    }
}

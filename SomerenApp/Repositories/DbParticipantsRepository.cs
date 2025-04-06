using Microsoft.Data.SqlClient;
using SomerenApp.Models;
using System.Diagnostics;

namespace SomerenApp.Repositories
{
    public class DbParticipantsRepository : IParticipantsRepository
    {
        private readonly string? _connectionString;

        public DbParticipantsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }

        private Participant ReadParticipant(SqlDataReader reader)
        {
            //retrieve data from fields
            int studentNumber = (int)reader["StudentNumber"];
            int activityNumber = (int)reader["ActivityNumber"];

            return new Participant(studentNumber, activityNumber);
        }
        
        public List<Participant> GetAllParticipants()
        {
            List<Participant> participants = new List<Participant>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT p.StudentNumber, p.ActivityNumber " +  
                                 "FROM Participants p " + 
                                 "JOIN Students s ON p.StudentNumber = s.StudentNumber " + 
                                 "JOIN Activities a ON p.ActivityNumber = a.ActivityNumber "; 
                                

                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Participant participant = ReadParticipant(reader);
                    participants.Add(participant);
                }

                reader.Close();
            }

            return participants;
        }

        //get a specific participant based on StudentNumber an ActivityNumber
        public Participant? GetByID(int studentNumber, int activityNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT StudentNumber, ActivityNumber " +
                                 "FROM Participants " +
                                 "WHERE StudentNumber = @StudentNumber AND ActivityNumber = @ActivityNumber "; 
                                  


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentNumber", studentNumber);
                command.Parameters.AddWithValue("@ActivityNumber", activityNumber);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadParticipant(reader);
                }

                return null;
            }

        }

        public void Add(Participant participant)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Participants (StudentNumber, ActivityNumber) " +
                               "VALUES (@StudentNumber, @ActivityNumber);";



                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentNumber", participant.StudentNumber);
                command.Parameters.AddWithValue("@ActivityNumber", participant.ActivityNumber);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("No participant was added!");
                }
            }
        }

        public void Delete(Participant participant)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Participants WHERE StudentNumber = @StudentNumber AND ActivityNumber = @ActivityNumber;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentNumber", participant.StudentNumber);
                command.Parameters.AddWithValue("@ActivityNumber", participant.ActivityNumber);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("No participant was removed!");
                }
            }
        }
    }
}

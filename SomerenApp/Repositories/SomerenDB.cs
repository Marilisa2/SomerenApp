using Microsoft.Data.SqlClient;
using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public class SomerenDB
    {
        private readonly string? _connectionString;
        public SomerenDB(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }
        public List<Docent> GetAll()
        {
            List<Docent> lecturers = new List<Docent>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT achternaam, voornaam, leeftijd, telefoonnummer FROM docenten ORDER BY achternaam";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Docent docent = ReadUser(reader);
                    lecturers.Add(docent);
                }
            }
            return lecturers;
        }
        private Docent ReadUser(SqlDataReader reader)
        {
            string achternaam = (string)reader["Achternaam"];
            string voornaam = (string)reader["Voornaam"];
            int leeftijd = (int)reader["Leeftijd"];
            string telefoonnummer = (string)reader["Telefoonnummer"];
            return new Docent(achternaam, voornaam, leeftijd, telefoonnummer);
        }
    }
}

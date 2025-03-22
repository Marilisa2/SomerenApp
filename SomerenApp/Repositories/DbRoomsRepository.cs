using Microsoft.Data.SqlClient;
using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public class DbRoomsRepository : IRoomsRepository
    {
        private readonly string? _connectionString;

        public DbRoomsRepository(IConfiguration configuration)
        {
            //get database connectionstring from appsettings
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }


        public Room ReadRoom(SqlDataReader reader)
        {
            //retrieve data from fields from database
            int id = (int)reader["RoomId"];
            string roomNumber = (string)reader["RoomNumber"];
            string roomSize = (string)reader["RoomSize"];

            //enum RoomType omzetten naar string want heeft datatype string in database
            string roomTypeString = reader["RoomType"].ToString();

            if (!Enum.TryParse<RoomType>(roomTypeString, out var roomType))
            {
                throw new InvalidOperationException($"Invalid Room type: {roomTypeString}");
            }

            string Building = (string)reader["Building"];

            //return new Room object
            return new Room(id, roomNumber, roomSize, roomType, Building);
        }


        public List<Room> GetAllRooms()
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RoomId, RoomNumber, RoomSize, RoomType, Building FROM Rooms ORDER BY RoomNumber ASC";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Room room = ReadRoom(reader);
                    rooms.Add(room);
                }

                reader.Close();
            }

            return rooms;
           
        }

        public Room? GetByID(int roomId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RoomId, RoomNumber, RoomSize, RoomType, Building FROM Rooms WHERE RoomId = @RoomId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomId", roomId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadRoom(reader);
                }

                return null;
            }
        }     
        
        public void Add(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //controleren of er een room bestaat met dezelfde RoomNumber
                string checkQuery = "SELECT COUNT(*) FROM Rooms WHERE RoomNumber = @RoomNumber";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);

                connection.Open();
                int existingRoomCount = (int)checkCommand.ExecuteScalar();

                if (existingRoomCount > 0)
                {
                    //als kamer met gegeven RoomNumber bestaat geeft het een foutmelding
                    throw new Exception($"A room with the number '{room.RoomNumber}' already exists. ");
                }

                string query = $"INSERT INTO Rooms (RoomNumber, RoomSize, RoomType, Building)" +
                                "VALUES (@RoomNumber, @RoomSize, @RoomType, @Building); " +
                                "SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                command.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                command.Parameters.AddWithValue("@RoomType", room.RoomType.ToString());
                command.Parameters.AddWithValue("@Building", room.Building);

                command.Connection.Open();
                room.RoomId = Convert.ToInt32(command.ExecuteScalar()); //haalt de RoomNumber op
                
            }
        }

        public void Update(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE Rooms SET RoomNumber = @RoomNumber, RoomSize = @Roomsize, " +
                                "RoomType = @RoomType, Building = @Building WHERE RoomId = @RoomId";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomId", room.RoomId);
                command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                command.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                command.Parameters.AddWithValue("@RoomType", room.RoomType.ToString());
                command.Parameters.AddWithValue("@Building", room.Building);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                {
                    throw new Exception("No records updated!");
                }
            }
        }

        public void Delete(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM Rooms WHERE RoomId = @RoomId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomId", room.RoomId);
                
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

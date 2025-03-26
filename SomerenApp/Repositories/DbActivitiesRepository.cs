using Microsoft.Data.SqlClient;
using SomerenApp.Models;
using SomerenActivity = SomerenApp.Models.Activity; //prevents confusion with System.Diagnostics.Activity

namespace SomerenApp.Repositories
{
    public class DbActivitiesRepository : IActivitiesRepository
    {
        private readonly string? _connectionString;

        public DbActivitiesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }

        private Activity ReadActivity(SqlDataReader reader)
        {
            //retrieve data from fields
            int activityNumber = (int)reader["ActivityNumber"];

            //converting enum ActivityType to string because it has datatype string in database
            string activityTypeString = reader["ActivityType"].ToString();

            if (!Enum.TryParse<ActivityType>(activityTypeString, out var activityType))
            {
                throw new InvalidOperationException($"Invalid Activity type: {activityTypeString}");
            }

            string activityDateString = reader["ActivityDate"].ToString();
            if (!Enum.TryParse<ActivityDate>(activityDateString, out var activityDate))
            {
                throw new InvalidOperationException($"Invalid Activity type: {activityDateString}");
            }


            //return new User object
            return new SomerenActivity(activityNumber, activityType, activityDate);
        }


        public List<SomerenActivity> GetAllActivities()
        {
            List<SomerenActivity> activities = new List<SomerenActivity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ActivityNumber, ActivityType, ActivityDate FROM Activities ORDER BY ActivityNumber ASC";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Activity activity = ReadActivity(reader);
                    activities.Add(activity);
                }

                reader.Close();
            }

            return activities;
        }


        public SomerenActivity? GetByID(int activityNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ActivityNumber, ActivityType, ActivityDate FROM Activities WHERE ActivityNumber = @ActivityNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityNumber", activityNumber);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadActivity(reader);
                }

                return null;
            }
        }

        public void Add(SomerenActivity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    
                    string query = $"INSERT INTO Activities (ActivityType, ActivityDate)" +
                                    "VALUES (@ActivityType, @ActivityDate); " +
                                    "SELECT SCOPE_IDENTITY();";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ActivityType", activity.ActivityType.ToString());
                    command.Parameters.AddWithValue("@ActivityDate", activity.ActivityDate.ToString());

                    command.Connection.Open();

                    activity.ActivityNumber = Convert.ToInt32(command.ExecuteScalar()); //retrieves the ActivityNumber
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occured while adding a activity: " + ex.Message);
                }
            }
        }

        public void Update(SomerenActivity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE Activities SET ActivityType = @ActivityType, " +
                                "ActivityDate = @ActivityDate WHERE ActivityNumber = @ActivityNumber";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityNumber", activity.ActivityNumber);
                command.Parameters.AddWithValue("@ActivityType", activity.ActivityType.ToString());
                command.Parameters.AddWithValue("@ActivityDate", activity.ActivityDate.ToString());
                
                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                {
                    throw new Exception("No records updated!");
                }
            }
        }

        public void Delete(SomerenActivity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM Activities WHERE ActivityNumber = @ActivityNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityNumber", activity.ActivityNumber);

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

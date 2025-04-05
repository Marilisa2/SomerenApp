using Microsoft.Data.SqlClient;
using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public class DbDrinkOrdersRepository : IDrinkOrdersRepository
    {
        private readonly string? _connectionString;

        public DbDrinkOrdersRepository(IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }

        public void AddOrder(DrinkOrder order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO orders (StudentNumber, DrinkId, Count)" +
                                "VALUES (@StudentNumber, @DrinkId, @Count);" +
                                "SELECT SCOPE_IDENTITY();";
               
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentNumber", order.StudentNumber);
                command.Parameters.AddWithValue("@DrinkId", order.DrinkId);
                command.Parameters.AddWithValue("@Count", order.Count);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Drink> GetAllDrinks() 
        {
            List<Drink> drinks = new List<Drink>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT DrinkId, DrinkName, DrinkType, Stock, Btw FROM Drinks";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Drink drink = ReadDrink(reader);
                    drinks.Add(drink);
                }
            }
            return drinks;
        }

        public List<DrinkOrder> GetAllOrders()
        {
            List<DrinkOrder> orders = new List<DrinkOrder>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT StudentNumber, DrinkId, Count FROM orders";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DrinkOrder order = new DrinkOrder();
                    {
                        int drinkId = (int)reader["DrinkId"];
                        int studentNumber = (int)reader["StudentNumber"];
                        int count = (int)reader["Count"];
                    };
                    orders.Add(order);
                }

            }
            return orders;
        }
        private Drink ReadDrink(SqlDataReader reader) 
        {
            int drinkId = (int)reader["DrinkId"];
            string drinkName = (string)reader["DrinkName"];
            string drinkType = (string)reader["DrinkType"];
            int stock = (int)reader["Stock"];
            //int btw = (int)reader["Btw"];
            int btw = Convert.ToInt32(reader["Btw"]);

            return new Drink(drinkId, drinkName, drinkType, stock, btw);
        }

        public Drink? GetDrinkById(int drinkId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT DrinkId, DrinkName, DrinkType, Stock, Btw FROM drinks WHERE DrinkId = @DrinkId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("DrinkId", drinkId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Drink drink = ReadDrink(reader);
                    reader.Close();
                    return drink;
                }
                reader.Close();
                return null;
            }
            
        }

        public void UpdateDrink(Drink drink)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Drinks SET Stock = @Stock, DrinkName = @DrinkName WHERE DrinkId = @DrinkId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Stock", drink.Stock);
                command.Parameters.AddWithValue("@DrinkId", drink.DrinkId);
                command.Parameters.AddWithValue("@DrinkName", drink.DrinkName); 
                
                connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                {
                    throw new Exception("No records updates!");
                }

            }
        }
    }
}

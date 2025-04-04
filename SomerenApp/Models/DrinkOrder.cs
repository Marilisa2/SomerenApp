namespace SomerenApp.Models
{
    public class DrinkOrder
    {
        public int DrinkId { get; set; }
        public int StudentNumber { get; set; }
        public int Count { get; set; }


        public DrinkOrder() 
        {
        
        }

        public DrinkOrder(int drinkId, int studentNumber, int count)
        {
            DrinkId = drinkId;
            StudentNumber = studentNumber;
            Count = count;
        }
    }
}

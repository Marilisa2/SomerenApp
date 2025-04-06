namespace SomerenApp.Models
{
    public class Drink
    {
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public string DrinkType { get; set; }
        public int Stock { get; set; }
        public int Btw { get; set; }

        public Drink() 
        {
        
        }

        public Drink(int drinkId, string drinkName, string drinkType, int stock, int btw)
        {
            DrinkId = drinkId;
            DrinkName = drinkName;
            DrinkType = drinkType;
            Stock = stock;
            Btw = btw;
        }
    }
}

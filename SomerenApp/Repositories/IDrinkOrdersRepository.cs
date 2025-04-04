using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public interface IDrinkOrdersRepository
    {
        List<Drink> GetAllDrinks();
        Drink? GetDrinkById(int drinkId);
        void AddOrder(DrinkOrder order);
        List<DrinkOrder> GetAllOrders();
        void UpdateDrink(Drink drink);
    }
}

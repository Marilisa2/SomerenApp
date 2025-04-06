using Microsoft.AspNetCore.Mvc;
using SomerenApp.Models;
using SomerenApp.Repositories;
using SomerenApp.ViewModels;

namespace SomerenApp.Controllers
{
    public class DrinkOrdersController : Controller
    {
        private readonly IDrinkOrdersRepository _drinkOrdersRepository;
        private readonly IStudentsRepository _studentsRepository;

        public DrinkOrdersController(IDrinkOrdersRepository drinksRepository, IStudentsRepository studentsRepository)
        {
            _drinkOrdersRepository = drinksRepository;
            _studentsRepository = studentsRepository;
        }

        [HttpGet]
        public IActionResult Index() 
        {
            var model = new DrinkOrderViewModel() 
            {
                Students = _studentsRepository.GetAll(),
                Drinks = _drinkOrdersRepository.GetAllDrinks()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(int studentNumber, int drinkId, int count)
        {
            //look if stock is enough
            var drink = _drinkOrdersRepository.GetDrinkById(drinkId);
            var student = _studentsRepository.GetById(studentNumber);

            if (drink.Stock >= count)
            {
                //make a drinkOrder object and save it in database
                var order = new DrinkOrder
                {
                    StudentNumber = studentNumber,
                    DrinkId = drinkId,
                    Count = count
                };
                _drinkOrdersRepository.AddOrder(order);

                //update stock
                drink.Stock -= count;
                _drinkOrdersRepository.UpdateDrink(drink);

                TempData["OrderSucces"] = $"{student.FirstName} {student.LastName} has succesfully ordered {count} {drink.DrinkName}(s)";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["OrderError"] = "Not enough stock";
                return RedirectToAction("Index");

            }

        }        
        
    }
}

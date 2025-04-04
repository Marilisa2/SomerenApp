using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SomerenApp.Models;
using SomerenApp.Repositories;
using System.Diagnostics.Eventing.Reader;

namespace SomerenApp.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomsRepository _roomsRepository;

        public RoomsController(IRoomsRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }

        //public RoomsController()
        //{
               //dummyRoomsRepository
        //    _roomsRepository = new DummyRoomsRepository();
        //}

        public IActionResult Index(string? roomSize)
        {
            List<Room> rooms;

            if (!string.IsNullOrEmpty(roomSize) && roomSize != "All")
            {
                rooms = _roomsRepository.GetRoomsBySize(roomSize);
            }
            else 
            {
                //gets all rooms via repository
                rooms = _roomsRepository.GetAllRooms();
            }
            //passes list to view
            return View(rooms);
        }

        //GET: RoomsController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //POST: RoomsController/Create
        [HttpPost]
        public ActionResult Create(Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_roomsRepository.GetAllRooms().Any(x => x.RoomNumber == room.RoomNumber))
                    {
                        //adding error message to ViewData if RoomNumber already exists
                        ViewData["ErrorMessage"] = "This room number already exists.";
                    }

                    //adding room via repository
                    _roomsRepository.Add(room);

                    //go back to room list via Index
                    return RedirectToAction("Index");
                }                     
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(room);
        }

        //GET: RoomsController/Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get room via repository
            Room? room = _roomsRepository.GetByID((int)id);
            return View(room);
        }

        //POST: RoomsController/Edit
        [HttpPost]
        public ActionResult Edit(Room room)
        {
            try
            {
                //updating room via repository
                _roomsRepository.Update(room);

                //go back to room list via Index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //something went wrong go back to View
                return View(room);
            }
        }

        //GET: RoomsController/Delete
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get room via repository
            Room? room = _roomsRepository.GetByID((int)id);
            return View(room);
        }

        //POST: RoomsController/Delete
        [HttpPost]
        public ActionResult delete(Room room)
        {
            try
            {
                //deleting room via repository
                _roomsRepository.Delete(room);

                //go back to room list via Index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //something went wrong
                return View(room);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SomerenApp.Models;
using SomerenApp.Repositories;

namespace SomerenApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsRepository _studentsRepository;

        public IActionResult Index() 
        {
            List<Student> students = _studentsRepository.GetAll();
            return View(students);
        }

        public StudentsController(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        //Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student student)
        {
            try
            {
                //haalt de eerste beschikbare RoomId uit de Room-tabel
                int roomId = _studentsRepository.GetAvailableRoomId();


                //koppel gevonden RoomId aan student
                student.RoomId = roomId;

                _studentsRepository.Add(student);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(student);
            }
        }


        //Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student? student = _studentsRepository.GetById((int)id);
            return View(student);
        }
        [HttpPost]
        public ActionResult Edit(Student student)
        {
            try
            {
                //haalt de eerste beschikbare RoomId uit de Room-tabel
                int roomId = _studentsRepository.GetAvailableRoomId();


                //koppel gevonden RoomId aan student
                student.RoomId = roomId;

                _studentsRepository.Update(student);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(student);
            }
        }

        //Delete
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student? student = _studentsRepository.GetById((int)id);
            return View(student);
        }
        [HttpPost]
        public ActionResult Delete(Student student)
        {
            try
            {
                //haalt de eerste beschikbare RoomId uit de Room-tabel
                int roomId = _studentsRepository.GetAvailableRoomId();


                //koppel gevonden RoomId aan student
                student.RoomId = roomId;


                _studentsRepository.Delete(student);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(student);
            }
        }

    }
}

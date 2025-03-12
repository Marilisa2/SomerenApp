using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public interface ILecturersRepository
    {
        List<Lecturer> GetAllLecturers();
        Lecturer? GetLecturerByID(int lecturerNumber);
        void AddLecturer(Lecturer lecturer);
        void EditLecturer(Lecturer lecturer);
        void DeleteLecturer(int lecturerNumber);
    }
}

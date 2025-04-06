using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public interface ILecturersRepository
    {
        List<Lecturer> GetAllLecturers();
        Lecturer? GetLecturerByID(int lecturerNumber);
        void AddLecturer(Lecturer lecturer);
        void EditLecturer(Lecturer lecturer);
        void DeleteLecturer(Lecturer lecturer);
        void AddSuperVisor(int activityNumber, int lecturerNumber);
        void RemoveSuperVisor(int activityNumber, int lecturerNumber);
        List<Lecturer> GetSupervisors(int activityNumber);
        List<Lecturer> GetNonSupervisors(int activityNumber);

        //int GetAvailableRoomId();
    }
}

using SomerenApp.Models;

namespace SomerenApp.Repositories
{
    public interface IStudentsRepository
    {
        List<Student> GetAll();
        Student? GetById(int studentNumber);
        void Add(Student student);
        void Update(Student student);
        void Delete(Student student);
    }
}

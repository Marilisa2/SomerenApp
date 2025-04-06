namespace SomerenApp.Repositories
{
    public interface IAccompanimentsRepository
    {
        void AddSuperVisor(int activityNumber, int lecturerNumber);
        void RemoveSuperVisor(int activityNumber, int lecturerNumber);
    }
}
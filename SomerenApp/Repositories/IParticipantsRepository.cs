using SomerenApp.Models;
using System.Data.Common;

namespace SomerenApp.Repositories
{
    public interface IParticipantsRepository
    {
        List<Participant> GetAllParticipants();
        Participant? GetByID(int studentNumber, int activityNumber); //specific participant with selected activity
        void Add(Participant participant); 
        void Delete(Participant participant);
    }
}

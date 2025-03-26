using SomerenApp.Models;
using System.Collections.Generic;

namespace SomerenApp.Repositories
{
    public interface IActivitiesRepository
    {
        List<Activity> GetAllActivities();
        Activity? GetByID(int activityNumber); //specific activity
        void Add(Activity activity);
        void Update(Activity activity);
        void Delete(Activity activity);
        List<Lecturer> LecturersAccompanying(int activityNumber);
        List<Lecturer> LecturersNotAccompanying(int activityNumber);
        void UpdateAccompaniments(Dictionary<Lecturer, bool> accompanimentsDictionary);
    }
}

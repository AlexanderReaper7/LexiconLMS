using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Services
{
    public interface IActivityDataService
    {
        Task<List<Activity>> GetActivities();

        Task<Activity> GetActivitiy(Guid Id);

        void DeleteActivity(Guid Id);

        Task<bool> UpdateActivity(Activity Activity);

        Task<bool> AddActivity(Activity Activity);
    }
}

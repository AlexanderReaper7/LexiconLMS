using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Services
{
    public interface ICourseDataService
    {
        Task<List<Course>> GetCourses();

        Task<Course> GetCourse(Guid Id);
        Task<Course> GetMyCourse();

        Task<bool> DeleteCourse(Guid Id);

        Task<bool> UpdateCourse(Course Course);

        Task<bool> AddCourse(Course Course);
    }
}

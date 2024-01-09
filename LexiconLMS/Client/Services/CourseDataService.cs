using LexiconLMS.Client.Components;
using LexiconLMS.Shared.Entities;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Net.Http.Headers;
using LexiconLMS.Client.Pages;
using Microsoft.AspNetCore.Components;
using LexiconLMS.Client.Helpers;

namespace LexiconLMS.Client.Services
{
    public class CourseDataService : ICourseDataService
    {
        private IGenericDataService _GService;

        public CourseDataService(HttpClient httpClient, IGenericDataService GService)
        {
            _GService = GService;
        }
        public async Task<bool> AddCourse(Course Course)
        {
            return await _GService.AddAsync(path: UriHelper.GetCoursesUri(), Course);            
        }

        public async Task<bool> DeleteCourse(Guid Id)
        {
            return await _GService.DeleteAsync(UriHelper.GetCourseUri(Id));
        }

        public async Task<Course> GetCourse(Guid Id)
        {
            return await _GService.GetAsync<Course>(UriHelper.GetCourseUri(Id));
        }

        public async Task<Course> GetMyCourse()
        {
            return await _GService.GetAsync<Course>(UriHelper.GetMyCourseUri());
        }

        public async Task<List<Course>> GetCourses()
        {
            return await _GService.GetAsync<List<Course>>(UriHelper.GetCoursesUri());
        }

        public async Task<bool> UpdateCourse(Course updatedCourse)
        {
            return await _GService.UpdateAsync(UriHelper.GetCourseUri(updatedCourse.Id), updatedCourse);
        }
    }
}

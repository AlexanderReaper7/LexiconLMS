using LexiconLMS.Client.Components;
using LexiconLMS.Shared.Entities;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Net.Http.Headers;

namespace LexiconLMS.Client.Services
{
    public class CourseDataService : ICourseDataService
    {
        HttpClient _httpClient;
        MediaTypeHeaderValue _mediaTypeHeaderValue;
        public CourseDataService()
        {
            _httpClient = new HttpClient();
            _mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
        }
        public async Task<bool> AddCourse(Course Course)
        {
            var json = JsonSerializer.Serialize(Course);
            var httpContent = new StringContent(json, _mediaTypeHeaderValue);
            var response = await _httpClient.PostAsync("/course/add", httpContent);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        public void DeleteCourse(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Course> GetCourse(Guid Id)
        {
            var json = JsonSerializer.Serialize(Id);
            var httpContent = new StringContent(json, _mediaTypeHeaderValue);
            var response = await _httpClient.PostAsync("/course/getcourse", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

                return (Course)JsonSerializer.Deserialize<Course>(responseData, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Course>> GetCourses()
        {

            var response = await _httpClient.GetAsync("/courses");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

                return (List<Course>)JsonSerializer.Deserialize<IEnumerable<Course>>(responseData, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateCourse(Course updatedCourse)
        {
            var course = GetCourse(updatedCourse.Id).Result;
            if (course != null)
            {
                course.Name = updatedCourse.Name;
                course.StartDate = updatedCourse.StartDate;
                course.EndDate = updatedCourse.EndDate;
                course.Description = updatedCourse.Description;

                var json = JsonSerializer.Serialize(course);
                var httpContent = new StringContent(json, _mediaTypeHeaderValue);
                var response = await _httpClient.PostAsync("/course/update", httpContent);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

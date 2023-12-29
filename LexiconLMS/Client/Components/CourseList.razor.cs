using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using LexiconLMS.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace LexiconLMS.Client.Components
{
    public partial class CourseList
    {
        [Inject]
        public ICourseDataService? CourseDataService { get; set; }

        public List<Course> CourseLst { get; set; } = new List<Course>();

        public bool Error = false;
        public string responseData = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            CourseLst = await GService.GetAsync<Task<List<Course>>>(path: $"/api/Courses").Result; //CourseDataService.GetCourses().Result;

            await base.OnInitializedAsync();
        }
    }
}

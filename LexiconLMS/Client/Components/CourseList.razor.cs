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

        protected override async Task OnInitializedAsync()
        {
            CourseLst = await CourseDataService.GetCourses();

            await base.OnInitializedAsync();
        }
    }
}

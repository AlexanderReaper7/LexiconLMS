using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using LexiconLMS.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using LexiconLMS.Shared.Dtos;

namespace LexiconLMS.Client.Components
{
    [Authorize(Roles = "Teacher,Student")]
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

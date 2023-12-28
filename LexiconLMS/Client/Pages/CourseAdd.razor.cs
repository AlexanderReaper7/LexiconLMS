using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Pages
{
    public partial class CourseAdd
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public Course Course { get; set; } = new Course();

        public string responseData = string.Empty;

        //public HttpStatusCode statusCode;

        private async Task HandleValidSubmit()
        {
            if (await CourseDataService.AddCourse(Course))
                NavigationManager.NavigateTo($"/listofcourses");
        }
    }
}
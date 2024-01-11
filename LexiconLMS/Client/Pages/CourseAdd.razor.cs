using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using LexiconLMS.Client.Helpers;

namespace LexiconLMS.Client.Pages
{
    //[Authorize(Roles = "Teacher")]
    public partial class CourseAdd
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public Course Course { get; set; } = new Course();

        public string responseData = string.Empty;

		public string ErrorMessage { get; set; } = string.Empty;

		private async Task HandleValidSubmit()
        {
            if (await CourseDataService.AddCourse(Course))
                NavigationManager.NavigateTo(UriHelper.GetCourseListUri());
            else
            {
				ErrorMessage = "Could not add Course";
			}
        }
    }
}
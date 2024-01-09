using LexiconLMS.Client.Components;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using LexiconLMS.Client.Helpers;

namespace LexiconLMS.Client.Pages
{
    //[Authorize(Roles = "Teacher")]
    public partial class CourseUpdate
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string? CourseId { get; set; }

        public Course Course { get; set; } = new Course();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(CourseId))
            {
                Course = await CourseDataService.GetCourse(Guid.Parse(CourseId));
            }

            base.OnInitializedAsync();
        }

        protected async Task HandleValidSubmit()
        {
            if(await CourseDataService.UpdateCourse(Course))
            NavigationManager.NavigateTo(UriHelper.GetCourseListUri());
        }
    }
}
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Pages
{
    //[Authorize(Roles = "Teacher")]
    public partial class CourseDelete
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

        protected async Task Delete()
        {
            if (await CourseDataService.DeleteCourse(Course.Id))
                NavigationManager.NavigateTo($"listofcourses");
        }
                
    }
}
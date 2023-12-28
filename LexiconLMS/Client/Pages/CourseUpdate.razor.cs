using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Pages
{
    public partial class CourseUpdate
    {
        [Inject]
        public ICourseDataService CourseDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Guid? CourseId { get; set; }

        public Course Course { get; set; } = new Course();

        protected override void OnInitialized()
        {
            if (CourseId.HasValue)
            {
                Course = CourseDataService.GetCourse(CourseId.Value).Result;
            }

            base.OnInitialized();
        }

        protected async Task HandleValidSubmit()
        {
            CourseDataService.UpdateCourse(Course);
            NavigationManager.NavigateTo($"listofcourses");
        }
    }
}
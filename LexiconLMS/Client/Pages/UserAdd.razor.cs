using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Pages
{
    //[Authorize(Roles = "Teacher")]
    public partial class UserAdd
    {
        [Inject]
        public IApplicationUserDataService ApplicationUserDataService { get; set; }

        [Inject]
        public ICourseDataService CourseDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ApplicationUserDtoAdd ApplicationUserDto { get; set; } = new ApplicationUserDtoAdd();

        [Parameter]
        public string? CourseId { get; set; }

        private async Task HandleValidSubmit()
        {
            var course = await CourseDataService.GetCourse(Guid.Parse(CourseId));
            ApplicationUserDto.CourseID = course.Id;
            if (await ApplicationUserDataService.AddApplicationUser(ApplicationUserDto))
                NavigationManager.NavigateTo($"listofcourses");
        }
    }
}

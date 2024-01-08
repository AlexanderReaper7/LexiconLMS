using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System.Data;

namespace LexiconLMS.Client.Pages
{
    public partial class UserUpdate
    {
        [Inject]
        public IApplicationUserDataService ApplicationUserDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string? UserId { get; set; }

        public ApplicationUserDtoUpdate ApplicationUserUpdate { get; set; } = new ApplicationUserDtoUpdate();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                ApplicationUserDto ApplicationUser = await ApplicationUserDataService.GetUser(Guid.Parse(UserId));

                ApplicationUserUpdate.Id = ApplicationUser.Id;
                ApplicationUserUpdate.FirstName = ApplicationUser.FirstName;
                ApplicationUserUpdate.LastName = ApplicationUser.LastName;
                ApplicationUserUpdate.Email = ApplicationUser.Email;
                ApplicationUserUpdate.Role = ApplicationUser.Role;
                ApplicationUserUpdate.Course = ApplicationUser.Course;
                ApplicationUserUpdate.OldRole = ApplicationUser.Role;
            }

            base.OnInitializedAsync();
        }

        protected async Task HandleValidSubmit()
        {
            if (await ApplicationUserDataService.UpdateUser(ApplicationUserUpdate))
                NavigationManager.NavigateTo($"listofcourses");
        }

        
    }
}
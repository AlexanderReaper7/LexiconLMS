using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

using static System.Net.WebRequestMethods;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace LexiconLMS.Client.Pages
{
    public partial class UserDelete
    {
        [Inject]
        public IApplicationUserDataService ApplicationUserDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string? UserId { get; set; }

        public ApplicationUserDtoUpdate ApplicationUser { get; set; } = new ApplicationUserDtoUpdate();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                ApplicationUser = await ApplicationUserDataService.GetUser(Guid.Parse(UserId));
            }

            base.OnInitializedAsync();
        }

        protected async Task Delete()
        {
            if (await ApplicationUserDataService.DeleteUser(ApplicationUser.Id))
                NavigationManager.NavigateTo($"listofcourses");
        }

    }
}

using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Pages
{
    public partial class UserDetails
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

    }
}


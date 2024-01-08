using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LexiconLMS.Client.Components
{
    public partial class AssignmentsListStudents
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IGenericDataService GenericDataService { get; set; } = default!;

        [Parameter]
        [EditorRequired]
        public Guid? ModuleId { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public IEnumerable<AssignmentsDtoForStudents> Assignments { get; set; } = null;


        protected override async Task OnInitializedAsync()
        {
            if (ModuleId == null)
            {
                return;
            }
            var user2 = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.FindFirstValue("sub");

            string username = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity!.Name!;

            ApplicationUser user = (await GenericDataService.GetAsync<ApplicationUser>(UriHelper.GetApplicationUserByNameUri(username)))!;

			Assignments = (await GenericDataService.GetAsync<IEnumerable<AssignmentsDtoForStudents>>(UriHelper.GetAssignmentsStudentsUri(ModuleId, user.Id)))!;

            await base.OnInitializedAsync();
        }
    }
}
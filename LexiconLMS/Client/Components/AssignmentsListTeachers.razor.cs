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
    public partial class AssignmentsListTeachers
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IGenericDataService GenericDataService { get; set; } = default!;

		[Parameter]
        [EditorRequired]
        public Guid? ModuleId { get; set; }

        [Parameter]
        [EditorRequired]
        public Guid? CourseId { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public IEnumerable<AssigmentDtoForTeachers> Assignments { get; set; } = null;


        protected override async Task OnInitializedAsync()
        {
            if (ModuleId == null || CourseId == null)
            {
                return;
            }
            var user2 = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.FindFirstValue("sub");
            Assignments = (await GenericDataService.GetAsync<IEnumerable<AssigmentDtoForTeachers>>(UriHelper.GetAssignmentsTeachersUri(CourseId, ModuleId)))!;

            await base.OnInitializedAsync();
        }
    }
}
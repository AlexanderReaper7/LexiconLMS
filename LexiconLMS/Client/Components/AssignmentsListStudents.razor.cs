using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

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

        [Parameter]
        [EditorRequired]
        public Guid? CourseId { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public IEnumerable<AssigmentDtoForStudents> Assignments { get; set; } = null;


        protected override async Task OnInitializedAsync()
        {
            if (ModuleId == null || CourseId == null)
            {
                return;
            }
			string username = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity!.Name!;

            ApplicationUser user = (await GenericDataService.GetAsync<ApplicationUser>(UriHelper.GetApplicationUserByNameUri(username)))!;

			Assignments = (await GenericDataService.GetAsync<IEnumerable<AssigmentDtoForStudents>>(UriHelper.GetAssignmentsStudentsUri(ModuleId, user.Id)))!;

            await base.OnInitializedAsync();
        }

        private string GetStatusCSSClass(SubmissionState status)
        {
            return status switch
            {
                SubmissionState.Submitted => "alert-success",
                SubmissionState.NotSubmitted => "alert-warning",
                SubmissionState.Late => "alert-danger",
                SubmissionState.SubmittedLate => "alert-info",
                _ => ""
            };
        }
    }
}
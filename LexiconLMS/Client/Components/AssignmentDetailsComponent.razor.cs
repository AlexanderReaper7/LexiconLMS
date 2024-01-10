using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using LexiconLMS.Client.Helpers;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using LexiconLMS.Shared.Dtos;
using System.Reflection;


namespace LexiconLMS.Client.Components
{
    public partial class AssignmentDetailsComponent
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IGenericDataService GenericDataService { get; set; } = default!;

        [Parameter]
        [EditorRequired]
        public Guid? ActivityId { get; set; }

		[Parameter]
		public string? StudentId { get; set; }

        public string StudentName { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public AssignmentDto Assignment { get; set; } = new AssignmentDto();
        public List<Document> ActivityDocuments { get; set; } = new List<Document>();
        public List<Document> StudentDocuments { get; set; } = new List<Document>();
        public IEnumerable<AssignmentDtoStudentAndStatusOnly>? AssignmentForTeachers { get; set; }

        public SubmissionState? SubmissionState { get; set; }
        public DateTime? SubmissionDate { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {

            if (ActivityId == null)
            {
                ErrorMessage = "Assignment not found";
                return;
            }

            Assignment = await GenericDataService.GetAsync<AssignmentDto>(UriHelper.GetAssignmentUri(ActivityId)) ?? Assignment;

            if (Assignment == null)
            {
                ErrorMessage = "Assignment not found";
                return;
            }
            ActivityDocuments = (await GenericDataService.GetAsync<List<Document>>($"activitydocumentsbyactivity/{ActivityId}"))!;

            ClaimsPrincipal user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;

            if (StudentId != null || user.IsInRole(StaticUserRoles.Student.ToString()))
            {
                await InitWhenStudent(user);
			}
            else if (user.IsInRole(StaticUserRoles.Teacher.ToString()))
            {
                //Init when teacher
                AssignmentForTeachers = (await GenericDataService.GetAsync<IEnumerable<AssignmentDtoStudentAndStatusOnly>>(UriHelper.GetAssignmentOnlyStudentsUri(ActivityId)));
            }
            await base.OnInitializedAsync();
        }

        private async Task InitWhenStudent(ClaimsPrincipal user)
        {
			string studentId = StudentId ?? user.FindFirstValue("sub")!;

            var student = await GenericDataService.GetAsync<ApplicationUser>(UriHelper.GetApplicationUserUri(studentId));

            if (student == null) 
            {
                ErrorMessage = "Could not find student";
                return;
            }

            StudentName = student.Fullname;

			StudentDocuments = (await GenericDataService.GetAsync<List<Document>>(UriHelper.GetAssignmentStudentUri(studentId, ActivityId)))!;

			DateTime now = DateTime.Now;
			if (StudentDocuments.Any())
			{
				SubmissionDate = StudentDocuments.Min(d => d.UploadDate);
				SubmissionState = Assignment.DueDate >= SubmissionDate ? LexiconLMS.Shared.Entities.SubmissionState.Submitted : LexiconLMS.Shared.Entities.SubmissionState.SubmittedLate;
			}
			else
			{
				SubmissionState = Assignment.DueDate >= now ? LexiconLMS.Shared.Entities.SubmissionState.NotSubmitted : LexiconLMS.Shared.Entities.SubmissionState.Late;
			}
		}

        private async Task DeleteActivity()
        {
            try
            {
                if (Assignment == null)
                {
                    return;
                }
                if (await GenericDataService.DeleteAsync(UriHelper.GetActivityUri(ActivityId)))
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    ErrorMessage = "Could not delete Module";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

    }
}
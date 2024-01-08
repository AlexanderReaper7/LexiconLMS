using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using LexiconLMS.Client.Helpers;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;


namespace LexiconLMS.Client.Pages
{
    public partial class AssignmentDetails
    {
		[Inject]
		public NavigationManager NavigationManager { get; set; } = default!;

		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;

        [Parameter]
        public Guid? ActivityId { get; set; }

		[Inject]
		public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public AssignmentDtoForStudents Assignment { get; set; } = new AssignmentDtoForStudents();
		public List<Document> ActivityDocuments { get; set; } = new List<Document>();
		public string ErrorMessage { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;

		protected override async Task OnInitializedAsync()
		{

			if (ActivityId == null)
			{
				ErrorMessage = "Activity not found";
				return;
			}

            string userId = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.FindFirstValue("sub")!;

            if (string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "Current user not available";
                return;
            }

            Assignment = (await GenericDataService.GetAsync<AssignmentDtoForStudents>(UriHelper.GetAssignmentStudentsUri(userId, ActivityId))) ?? Assignment;

            if (Assignment == null)
			{
				ErrorMessage = "Activity not found";
				return;
			}
			ActivityDocuments = await GenericDataService.GetAsync<List<Document>>($"activitydocumentsbyactivity/{ActivityId}");
			await base.OnInitializedAsync();

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
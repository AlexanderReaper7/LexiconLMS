using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using LexiconLMS.Client.Helpers;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using Microsoft.AspNetCore.Components.Authorization;


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
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public AssignmentDtoForStudents? Assignment { get; set; } = new AssignmentDtoForStudents();
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

            string username = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity!.Name!;

            ApplicationUser user = (await GenericDataService.GetAsync<ApplicationUser>(UriHelper.GetApplicationUserByNameUri(username)))!;

            Assignment = (await GenericDataService.GetAsync<AssignmentDtoForStudents>(UriHelper.GetAssignmentStudentsUri(user.Id, ActivityId)))!;

            if (Assignment == null)
			{
				ErrorMessage = "Activity not found";
				return;
			}
			ActivityDocuments = await GenericDataService.GetAsync<List<Document>>($"activitydocumentsbyactivity/{ActivityId}") ?? ActivityDocuments;
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
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using LexiconLMS.Client.Helpers;
using System.Reflection;
using System.Net.Security;
using Microsoft.AspNetCore.Components.Authorization;

namespace LexiconLMS.Client.Pages
{
	public partial class DocumentUpload
	{
		

		[Inject]
		NavigationManager NavigationManager { get; set; }

		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;

		[Inject]
		AuthenticationStateProvider AuthenticationStateProvider { get; set; }

		[Parameter]
		public Guid Id { get; set; }
		public Document Document { get; set; } = new Document();

		public Document Response { get; set; } = new Document();

		public string ErrorMessage { get; set; }
		private string userRole { get; set; }

		protected override async Task OnInitializedAsync()
		{
			var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var user = authenticationState.User;
			if (user.Identity.IsAuthenticated)
			{
				// Get user role
				userRole = user.FindFirst("role")?.Value;
			}


			await base.OnInitializedAsync();

		}

		private async Task HandleValidSubmit()
		{
			// Sets Íd to Correct FK
			Response = await GenericDataService.GetAsync<Document>($"documentsetfk/{Id}") ?? Response;			
			Response.Description = Document.Description;
			Response.Name = Document.Name;
			try
			{
				Response.Path = $"api/documents/{Response.Name}";
				Response.UploadDate = DateTime.Now;
				Response.RoleOfUploader = userRole;
				if (await GenericDataService.AddAsync("api/documents", Response))
				{
					string detailsUri = null;

					if (Response.ActivityId != null)
					{
						detailsUri = UriHelper.GetActivityDetailsUri(Response.ActivityId);
					}
					else if (Response.ModuleId != null)
					{
						detailsUri = UriHelper.GetModuleDetailsUri(Response.ModuleId);
					}
					else if (Response.CourseId != null)
					{
						detailsUri = UriHelper.GetCourseDetailsUri(Response.CourseId);
					}

					if (detailsUri != null)
					{
						NavigationManager.NavigateTo(detailsUri);
					}

				}
				else
				{
					ErrorMessage = "Could not add document";
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"{ex.Message} {ex.HResult}";
			}
		}
	}



	//private void UploadDocument()
	//{
	//    var newDocument = new ActivityDocument
	//    {
	//        FileName = fileName,
	//        FilePath = $"uploads/{fileName}", // Adjust the path as needed
	//        ActivitiesId = ActivityId // Set the current activity's ID
	//    };

	//}
}
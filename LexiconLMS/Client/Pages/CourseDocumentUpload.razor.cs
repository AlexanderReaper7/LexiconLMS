using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using LexiconLMS.Client.Helpers;
using System.Reflection;

namespace LexiconLMS.Client.Pages
{
	public partial class CourseDocumentUpload
	{
		[Inject]
		NavigationManager NavigationManager { get; set; }

		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;
		[Parameter]
		public Guid CourseId { get; set; }
		public CourseDocument CourseDocument { get; set; } = new CourseDocument();

		public string ErrorMessage { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();

		}

		private async Task HandleValidSubmit()
		{
			try
			{
				CourseDocument.CourseId = CourseId;
				CourseDocument.Path = $"api/activitydocuments/{CourseDocument.Name}";
				CourseDocument.UploadDate = DateTime.Now;
				// TODO CourseDocument.Uploader = 


				if (await GenericDataService.AddAsync("api/coursedocuments", CourseDocument))

				{
					NavigationManager.NavigateTo("/");
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

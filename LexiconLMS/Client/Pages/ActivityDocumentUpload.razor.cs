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
    public partial class ActivityDocumentUpload
    {
		[Inject]
		NavigationManager NavigationManager { get; set; }
		
		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;
		[Parameter]
        public Guid ActivityId { get; set; }
		public ActivityDocument ActivityDocument { get; set; } = new ActivityDocument();

		public string ErrorMessage { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();

		}

		private async Task HandleValidSubmit()
		{
			try
			{
				ActivityDocument.ActivityId = ActivityId;
				ActivityDocument.Path = $"api/activitydocuments/{ActivityDocument.Name}";
				ActivityDocument.UploadDate = DateTime.Now;
				// TODO ActivityDocument.Uploader = 


				if (await GenericDataService.AddAsync("api/activitydocuments", ActivityDocument))

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

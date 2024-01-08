﻿using System.Text.Json;
using static System.Net.WebRequestMethods;
using LexiconLMS.Client.Helpers;
using System.Reflection;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

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
					NavigationManager.NavigateTo(UriHelper.GetCourseDetailsUri(CourseId));
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
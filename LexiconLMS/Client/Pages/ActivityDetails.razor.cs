﻿using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using LexiconLMS.Client.Helpers;
using System.Reflection;
using Module = LexiconLMS.Shared.Entities.Module;


namespace LexiconLMS.Client.Pages
{
	public partial class ActivityDetails
	{
		[Inject]
		public NavigationManager NavigationManager { get; set; } = default!;

		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;

		[Parameter]
		public Guid? ActivityId { get; set; }

		public Activity Activity { get; set; } = new Activity();
		public List<Document> ActivityDocuments { get; set; } = new List<Document>();
		public List<Document> StudentActivityDocuments { get; set; } = new List<Document>();

		public Module Module { get; set; } = new Module();

		public string ErrorMessage { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;

		protected override async Task OnInitializedAsync()
		{

			if (ActivityId == null)
			{
				ErrorMessage = "Activity not found";
				return;
			}

			Activity = await GenericDataService.GetAsync<Activity>(UriHelper.GetActivityUri(ActivityId)) ?? Activity;

			if (Activity == null)
			{
				ErrorMessage = "Activity not found";
				return;
			}

			if (Activity.Type.Name == "Assignment")
			{
				NavigationManager.NavigateTo(UriHelper.GetAssignmentDetailsUri(ActivityId));
			}

			Module = await GenericDataService.GetAsync<Module>(UriHelper.GetModuleUri(Activity.ModuleId)) ?? Module;
			ActivityDocuments = await GenericDataService.GetAsync<List<Document>>($"activitydocumentsbyactivity/{ActivityId}") ?? ActivityDocuments;
			StudentActivityDocuments = await GenericDataService.GetAsync<List<Document>>($"studentdocumentsbyactivity/{ActivityId}") ?? StudentActivityDocuments;
			await base.OnInitializedAsync();

		}


		private async Task DeleteActivity()
		{
			try
			{
				if (Activity == null)
				{
					return;
				}
				if (await GenericDataService.DeleteAsync(UriHelper.GetActivityUri(ActivityId)))
				{
					NavigationManager.NavigateTo(UriHelper.GetModuleDetailsUri(Activity.ModuleId));
				}
				else
				{
					ErrorMessage = "Could not delete Activity";
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
		}

	}
}
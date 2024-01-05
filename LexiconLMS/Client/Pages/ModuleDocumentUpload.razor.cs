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
	public partial class ModuleDocumentUpload
	{
		[Inject]
		NavigationManager NavigationManager { get; set; }

		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;
		[Parameter]
		public Guid ModuleId { get; set; }
		public ModuleDocument ModuleDocument { get; set; } = new ModuleDocument();

		public string ErrorMessage { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();


		}

		private async Task HandleValidSubmit()
		{
			try
			{
				ModuleDocument.ModuleId = ModuleId;
				ModuleDocument.Path = $"api/moduledocuments/{ModuleDocument.Name}";
				ModuleDocument.UploadDate = DateTime.Now;
				// TODO ActivityDocument.Uploader = 
				// TODO ActivityDocument.UploaderId = 


				if (await GenericDataService.AddAsync("api/moduledocuments", ModuleDocument))

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
}
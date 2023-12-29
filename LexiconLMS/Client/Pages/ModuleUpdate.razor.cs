using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.WebSockets;
using System.Net;
using LexiconLMS.Client.Helpers;

namespace LexiconLMS.Client.Pages
{
    public partial class ModuleUpdate
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IGenericDataService GenericDataService { get; set; } = default!;

        [Parameter]
        public Guid? ModuleId { get; set; }

        public Module Module { get; set; } = new Module();

        public string ErrorMessage { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

		public bool LoadActivities { get; set; } = true;

        protected override async Task OnInitializedAsync()
		{
			if (ModuleId == null)
			{
				ErrorMessage = "Module not found";
				return;
			}

			Module = await GenericDataService.GetAsync<Module>(UriHelper.GetModuleUri(ModuleId.Value))?? Module;
			LoadActivities = true;
			if (Module == null)
			{
				ErrorMessage = "Module not found";
				return;
			}

			await base.OnInitializedAsync();
		}

		private async Task HandleValidSubmit()
        {
			if (Module == null)
			{
				return;
			}
			try
			{
				if (await GenericDataService.UpdateAsync(UriHelper.GetModuleUri(Module.Id), Module))
				{
					Message = "Module saved";
				}
				else
				{
					ErrorMessage = "Could not update module";
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"{ex.Message} {ex.HResult}";
			}
		}

        private async Task DeleteModule()
        {
			try
			{
				if (Module == null)
				{
					return;
				}
				if (await GenericDataService.DeleteAsync(UriHelper.GetModuleUri(Module.Id)))
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
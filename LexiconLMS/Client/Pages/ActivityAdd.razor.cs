using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.WebSockets;
using LexiconLMS.Client.Helpers;
using Microsoft.EntityFrameworkCore;



namespace LexiconLMS.Client.Pages
{
    public partial class ActivityAdd
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IGenericDataService GenericDataService { get; set; } = default!;
	
		[Parameter]
        public Guid? ModuleId { get; set; }

        public Activity Activity { get; set; } = new Activity();

        public string ErrorMessage { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                Activity.ModuleId = ModuleId!.Value;
			 	Activity.Module = await GenericDataService.GetAsync<Module>(UriHelper.GetModuleUri(ModuleId));
                if (await GenericDataService.AddAsync(UriHelper.GetActivitiesUri(), Activity))
		
				{
                    NavigationManager.NavigateTo(UriHelper.GetModuleDetailsUri(ModuleId));
                }
                else
                {

                    ErrorMessage = "Could not add activity";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex.Message} {ex.HResult}";
            }
        }

    }
}
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
    public partial class ActivityUpdate
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IGenericDataService GenericDataService { get; set; } = default!;

        [Parameter]
        public Guid? ActivityId { get; set; }

        public Activity Activity { get; set; } = new Activity();

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

            await base.OnInitializedAsync();
        }

        private async Task HandleValidSubmit()
        {
            if (Activity == null)
            {
                return;
            }
            try
            {
              
                if (await GenericDataService.UpdateAsync(UriHelper.GetActivityUri(ActivityId), Activity))
                {
                    Message = "Activity saved";
                }
                else
                {
                    ErrorMessage = "Could not update Activity";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex.Message} {ex.HResult}";
            }
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